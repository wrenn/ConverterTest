using ConverterLib.Models;

namespace ConverterLib.Interfaces
{
    public interface IConverter
    {
        ConversionResultModel Convert(string file);
    }
}
