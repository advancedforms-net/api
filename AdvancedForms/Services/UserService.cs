using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AdvancedForms.Models;
using AdvancedForms.Helpers;

namespace AdvancedForms.Services;

public interface IUserService
{
	Task<string> Authenticate(string mail);
	Task<Guid?> ParseToken(string token);
	Task<User?> Get(Guid userId);
}

public class UserService : IUserService
{
	private readonly ILogger<UserService> logger;
	private readonly FormContext db;
	private readonly INowResolver nowResolver;
	private readonly JwtConfig jwtConfig;

	public UserService(ILogger<UserService> logger, FormContext db, INowResolver nowResolver, IOptions<JwtConfig> jwtConfig)
	{
		this.db = db;
		this.logger = logger;
		this.nowResolver = nowResolver;
		this.jwtConfig = jwtConfig.Value;
	}

	public async Task<string> Authenticate(string mail)
	{
		var user = db.Users.SingleOrDefault(u => u.Mail == mail);

		// return null if user not found
		if (user == null)
		{
			logger.LogInformation("User does not yet exists. Creating new user with mail: {mail}", mail);

			// create the user
			user = new User() {
				Id = Guid.NewGuid(),
				Mail = mail,
			};

			db.Users.Add(user);
			await db.SaveChangesAsync();
		}

		// authentication successful so generate jwt token
		TimeSpan duration = TimeSpan.FromDays(jwtConfig.ExpireDays);
		return GenerateJwtToken(user, duration);
	}

	public Task<Guid?> ParseToken(string token)
	{
		try
		{
			logger.LogInformation("Parsing token");

			var tokenHandler = new JwtSecurityTokenHandler();
			var key = Encoding.ASCII.GetBytes(jwtConfig.Secret);
			tokenHandler.ValidateToken(token, new TokenValidationParameters
			{
				ValidateIssuerSigningKey = true,
				IssuerSigningKey = new SymmetricSecurityKey(key),
				ValidateIssuer = false,
				ValidateAudience = false,
				// set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
				ClockSkew = TimeSpan.Zero
			}, out SecurityToken validatedToken);

			var jwtToken = (JwtSecurityToken)validatedToken;
			var userId = Guid.Parse(jwtToken.Claims.First(x => x.Type == "id").Value);

			logger.LogInformation("Parsed user id: {userId}", userId);

			return Task.FromResult<Guid?>(userId);
		}
		catch (Exception e)
		{
			logger.LogWarning(e, "Failed to parse token: {token}", token);

			// if jwt validation fails we return no user
			// user is not attached to context so request won't have access to secure routes
			return Task.FromResult<Guid?>(null);
		}
	}

	private string GenerateJwtToken(User user, TimeSpan duration)
	{
		// generate token that is valid for 7 days
		var tokenHandler = new JwtSecurityTokenHandler();
		var key = Encoding.ASCII.GetBytes(jwtConfig.Secret);
		var tokenDescriptor = new SecurityTokenDescriptor
		{
			Subject = new ClaimsIdentity(new[] { new Claim("id", user.Id.ToString()) }),
			Expires = nowResolver.GetUtcNow().Add(duration),
			SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
		};
		var token = tokenHandler.CreateToken(tokenDescriptor);
		return tokenHandler.WriteToken(token);
	}

	public async Task<User?> Get(Guid userId)
	{
		return await db.Users.FindAsync(userId);
	}
}
