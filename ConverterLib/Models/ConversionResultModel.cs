namespace ConverterLib.Models
{
    public class ConversionResultModel
    {
        // Was successfully converted
        public bool Success { get; set; }
        // Path to converted file
        public string File { get; set; } = string.Empty;
        // Output message about conversion
        public string Message { get; set; } = string.Empty;
    }
}
