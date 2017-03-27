using ConverterLib.Interfaces;
using ConverterLib.Models;
using ConverterLib.Utilities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Xml;

namespace ConverterLib.Converters
{
    public class JsonConverter : IConverter
    {
        /// <summary>
        /// Convert provided JSON source file to XML file
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
                    // Load file contents
                    string contents = File.ReadAllText(sourceFile);

                    // Validate JSON formatting and convert to XML
                    XmlDocument xmlDoc = null;

                    if (IsJsonArray(contents) && IsWellFormed(contents, true))
                    {
                        xmlDoc = JsonConvert.DeserializeXmlNode($"{{row:{contents}}}", "root", true);
                    }
                    else if (IsJsonObject(contents) && IsWellFormed(contents, false))
                    {
                        xmlDoc = JsonConvert.DeserializeXmlNode(contents);
                    }
                    else
                    {
                        result = new ConversionResultModel() { Success = false, Message = "Invalid file content: JSON not well-formed." };
                    }

                    // If file contents was successfully deserialized to XML then save the converted file
                    if (xmlDoc != null)
                    {
                        string newFilePath = FileUtility.GetSavePath() + FileUtility.CreateUniqueFileName(".xml");

                        XmlWriterSettings settings = new XmlWriterSettings();
                        settings.Indent = true;
                        settings.CloseOutput = true;

                        XmlWriter writer = XmlWriter.Create(new FileStream(newFilePath, FileMode.Create), settings);

                        xmlDoc.Save(writer);
                        writer.Close();

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
        /// Validate file contents is well-formed JSON
        /// </summary>
        /// <param name="fileContents"></param>
        /// <returns></returns>
        public bool IsWellFormed(string fileContents, bool isArray)
        {
            bool result = false;

            try
            {
                // Ensure file is either JSON object or array
                if (isArray)
                {
                    JArray array = JArray.Parse(fileContents);
                    result = true;
                }
                else
                {
                    JObject obj = JObject.Parse(fileContents);
                    result = true;
                }
            }
            catch (Exception ex)
            {
                result = false;
            }

            return result;
        }

        /// <summary>
        /// Test if file contents is a JSON array
        /// </summary>
        /// <param name="fileContents"></param>
        /// <returns></returns>
        private bool IsJsonArray(string fileContents)
        {
            return fileContents.StartsWith("[") && fileContents.EndsWith("]");
        }

        /// <summary>
        /// Test if file contents is a JSON object
        /// </summary>
        /// <param name="fileContents"></param>
        /// <returns></returns>
        private bool IsJsonObject(string fileContents)
        {
            return fileContents.StartsWith("{") && fileContents.EndsWith("}");
        }
    }
}
