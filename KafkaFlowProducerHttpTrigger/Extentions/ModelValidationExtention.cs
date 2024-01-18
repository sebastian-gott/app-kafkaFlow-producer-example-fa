using KafkaFlowProducerHttpTrigger.Models;
using Microsoft.Azure.Functions.Worker.Http;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace KafkaFlowProducerHttpTrigger.Extentions
{
    public static class ModelValidationExtention
    {
        private static ValidationWrapper<T> ValidationWrapperBuilder<T>(string res)
        {
            ValidationWrapper<T> body = new ValidationWrapper<T>();
            body.Value = JsonSerializer.Deserialize<T>(res);

            var result = new List<ValidationResult>();
            body.IsValid = Validator.TryValidateObject(body.Value, new ValidationContext(body.Value, null, null), result, true);
            body.ValidationResults = result;

            return body;
        }

        public static async Task<ValidationWrapper<T>> GetBody<T>(this HttpRequestData httpRequestData)
        {
            var body = await httpRequestData.ReadAsStringAsync();
            return ValidationWrapperBuilder<T>(body);
        }
    }
}
