using ConverterLib.Interfaces;
using ConverterLib.Models;

namespace ConverterLib
{
    public class ConversionManager
    {
        private IConverter Converter { get; set; }
        private string SourceFile { get; set; }

        public ConversionManager(IConverter fileConverter, string filePath)
        {
            Converter = fileConverter;
            SourceFile = filePath;
        }

        public ConversionResultModel Convert()
        {
            return Converter.Convert(SourceFile);
        }
    }
}
