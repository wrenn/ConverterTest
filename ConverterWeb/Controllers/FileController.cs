using ConverterLib;
using ConverterLib.Converters;
using ConverterLib.Interfaces;
using ConverterLib.Models;
using ConverterLib.Utilities;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace ConverterWeb.Controllers
{
    [RoutePrefix("api/v1")]
    public class FileController : ApiController
    {
        /// <summary>
        /// Downloads the specified file resource if it exists
        /// </summary>
        /// <param name="fileType"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        [HttpGet, Route("files/{fileType}/{fileName}", Name = "GetFile")]
        public HttpResponseMessage GetFile(string fileType, string fileName)
        {
            HttpResponseMessage result;

            try
            {
                string filePath = FileUtility.GetSavePath() + $"{fileName}.{fileType}";

                if (File.Exists(filePath))
                {
                    result = new HttpResponseMessage(HttpStatusCode.OK);

                    string mediaType = "application/" + fileType;

                    Byte[] bytes = File.ReadAllBytes(filePath);
                    result.Content = new ByteArrayContent(bytes);
                    result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                    result.Content.Headers.ContentType = new MediaTypeHeaderValue(mediaType);
                    result.Content.Headers.ContentDisposition.FileName = $"{fileName}.{fileType}";
                }
                else
                {
                    result = new HttpResponseMessage(HttpStatusCode.NotFound);
                }
            }
            catch (Exception ex)
            {
                result = new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }

            return result;
        }

        /// <summary>
        /// Converts provided file to XML or JSON and returns link to new resource location
        /// </summary>
        /// <returns></returns>
        [HttpPost, Route("files", Name = "PostFile")]
        public async Task<HttpResponseMessage> PostFile()
        {
            HttpResponseMessage result;

            if (Request.Content.IsMimeMultipartContent())
            {
                string root = HttpContext.Current.Server.MapPath("~/App_Data");
                MultipartFormDataStreamProvider provider = new MultipartFormDataStreamProvider(root);

                await Request.Content.ReadAsMultipartAsync(provider);

                string fileName = provider.FileData[0].Headers.ContentDisposition.FileName.Replace("\"", string.Empty);

                ConversionManager manager = null;
                ConversionResultModel conversionResult = null;
                IConverter converter = null;

                if (fileName.EndsWith(".xml"))
                {
                    converter = new XmlConverter();
                }

                if (fileName.EndsWith(".json"))
                {
                    converter = new JsonConverter();
                }

                if (converter != null)
                {
                    manager = new ConversionManager(converter, provider.FileData[0].LocalFileName);
                    conversionResult = manager.Convert();

                    if (conversionResult.Success)
                    {
                        result = Request.CreateResponse(HttpStatusCode.Created);
                        string[] newFilePathParts = conversionResult.File.Split(new char[] { '\\' });
                        string[] newFileNameParts = newFilePathParts[newFilePathParts.Length - 1].Split(new char[] { '.' });
                        string newFileUri = Url.Link("GetFile", new { fileType = newFileNameParts[1], fileName = newFileNameParts[0] });
                        result.Headers.Location = new Uri(newFileUri);
                    }
                    else
                    {
                        result = new HttpResponseMessage(HttpStatusCode.InternalServerError);
                    }
                }
                else
                {
                    result = new HttpResponseMessage(HttpStatusCode.BadRequest);
                }
            }
            else
            {
                result = new HttpResponseMessage(HttpStatusCode.BadRequest);
            }

            return result;
        }
    }
}
