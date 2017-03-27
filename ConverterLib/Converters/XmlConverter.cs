using ConverterLib.Interfaces;
using ConverterLib.Models;
using ConverterLib.Utilities;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Xml;

namespace ConverterLib.Converters
{
    public class XmlConverter : IConverter
    {
        /// <summary>
        /// Convert provided XML source file to JSON file
        /// </summary>
        /// <param name="sourceFile"></param>
        /// <returns></returns>
        public ConversionResultModel Convert(string sourceFile)
        {
            ConversionResultModel result = null;

            try
            {
                // Ensure file exists
                if (!string.IsNullOrEmpty(sourceFile) && File.Exists(sourceFile))
                {
                    // Validate XML formatting and load XML document
                    XmlDocument xmlDoc = null;

                    if (IsWellFormed(sourceFile))
                    {
                        xmlDoc = new XmlDocument();
                        xmlDoc.Load(sourceFile);
                    }
                    else
                    {
                        result = new ConversionResultModel() { Success = false, Message = "Invalid file content: XML not well-formed." };
                    }

                    // If XML document loaded then convert to JSON and save to file
                    if (xmlDoc != null)
                    {
                        string json = JsonConvert.SerializeXmlNode(xmlDoc);

                        string newFilePath = FileUtility.GetSavePath() + FileUtility.CreateUniqueFileName(".json");

                        File.WriteAllText(newFilePath, json);

                        result = new ConversionResultModel() { Success = true, File = newFilePath, Message = $"Source file converted successfully and saved to: {newFilePath}." };
                    }
                }
                else
                {
                    result = new ConversionResultModel() { Success = false, Message = $"File does not exist at this location: {sourceFile}" };
                }
            }
            catch (Exception ex)
            {
                result = new ConversionResultModel() { Success = false, Message = $"Conversion error: {ex.Message}" };
            }

            return result;
        }

        /// <summary>
        /// Test if XML is well-formed
        /// </summary>
        /// <param name="sourceFile"></param>
        /// <returns></returns>
        private bool IsWellFormed(string sourceFile)
        {
            bool result = false;

            try
            {
                XmlReader reader = XmlReader.Create(sourceFile);
                result = true;
            }
            catch (Exception ex)
            {
                result = false;
            }

            return result;
        }
    }
}
