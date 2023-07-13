using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Formatters;

namespace AdvancedForms.Helpers;

/// <summary>
/// The input formatter for multipart/form-data or x-www-form-urlencoded request bodies.
/// </summary>
public class FormInputFormatter : TextInputFormatter
{
	/// <summary>
	/// Initializes a new instance of the <see cref="FormInputFormatter"/> class.
	/// </summary>
	public FormInputFormatter()
	{
		this.SupportedEncodings.Add(UTF8EncodingWithoutBOM);
		this.SupportedEncodings.Add(UTF16EncodingLittleEndian);

		this.SupportedMediaTypes.Add("multipart/form-data");
		this.SupportedMediaTypes.Add("application/x-www-form-urlencoded");
	}

	/// <inheritdoc/>
	public override async Task<InputFormatterResult> ReadRequestBodyAsync(InputFormatterContext context, Encoding encoding)
	{
		if (context == null)
		{
			throw new ArgumentNullException(nameof(context));
		}

		if (encoding == null)
		{
			throw new ArgumentNullException(nameof(encoding));
		}

		object? _model = null;

		try
		{
			if (context.HttpContext.Request.HasFormContentType)
			{
				Dictionary<string, string?> _form = new();
				foreach (string _key in context.HttpContext.Request.Form.Keys)
				{
					_form.Add(_key, context.HttpContext.Request.Form[_key]);
				}

				string _json = JsonSerializer.Serialize(_form);
				Type _type = context.ModelType;
				_model = JsonSerializer.Deserialize(_json, _type, new JsonSerializerOptions
				{
					AllowTrailingCommas = true,
					PropertyNameCaseInsensitive = true,
				});
			}
		}
		catch (JsonException _ex)
		{
			var _path = _ex.Path ?? string.Empty;
			var _modelStateException = new InputFormatterException(_ex.Message, _ex);
			context.ModelState.TryAddModelError(_path, _modelStateException, context.Metadata);
			return await InputFormatterResult.FailureAsync();
		}
		catch (Exception _ex)
		{
			context.ModelState.TryAddModelError(string.Empty, _ex, context.Metadata);
			return await InputFormatterResult.FailureAsync();
		}

		if (_model == null && !context.TreatEmptyInputAsDefaultValue)
		{
			return await InputFormatterResult.FailureAsync();
		}

		return await InputFormatterResult.SuccessAsync(_model);
	}
}

