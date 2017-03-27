using System;
using System.IO;
using System.Web;

namespace ConverterLib.Utilities
{
    public static class FileUtility
    {
        /// <summary>
        /// Get output file path for saving documents
        /// </summary>
        /// <returns></returns>
        public static string GetSavePath()
        {
            string result = string.Empty;

            try
            {
                if (!Directory.Exists("Output"))
                {
                    Directory.CreateDirectory("Output");
                }

                result = Directory.GetCurrentDirectory() + @"\Output\";
            }
            catch (Exception ex)
            {
                result = HttpContext.Current.Server.MapPath("~/App_Data/");
            }

            return result;
        }

        /// <summary>
        /// Get a unique file name based on DateTime and provided file extension
        /// </summary>
        /// <param name="fileExtension"></param>
        /// <returns></returns>
        public static string CreateUniqueFileName(string fileExtension)
        {
            return DateTime.Now.ToString("yyyyMMddHHmmss") + fileExtension;
        }
    }
}
