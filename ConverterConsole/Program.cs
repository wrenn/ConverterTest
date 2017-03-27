using ConverterLib;
using ConverterLib.Converters;
using ConverterLib.Interfaces;
using ConverterLib.Models;
using System;

namespace ConverterConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            IConverter converter = null;

            Console.WriteLine("Enter the path of a JSON or XML file to convert:");
            string sourceFilePath = Console.ReadLine().ToLower();

            if (sourceFilePath.EndsWith(".xml"))
                converter = new XmlConverter();
            else if (sourceFilePath.EndsWith(".json"))
                converter = new JsonConverter();
            else
                throw new Exception("Invalid file type. Only .json and .xml files allowed.");

            ConversionManager manager = new ConversionManager(converter, sourceFilePath);
            ConversionResultModel result = manager.Convert();

            Console.WriteLine("\n" + result.Message);
            Console.WriteLine("\nHit return to exit");
            Console.ReadLine();
        }
    }
}
