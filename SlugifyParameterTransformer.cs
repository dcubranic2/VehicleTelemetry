using Microsoft.AspNetCore.Mvc.ApplicationModels;
namespace VehicleTelemetry
{
    public class SlugifyParameterTransformer : IOutboundParameterTransformer
    {
        public string? TransformOutbound(object? value)
        {
            return value?.ToString()?.ToLower();
        }
    }
}
