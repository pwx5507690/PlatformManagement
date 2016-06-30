using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Hosting;
using DataSource;
using PlatformManagement.Models;

namespace PlatformManagement.Controllers
{
    public class AttachmentController : BaseCommonController
    {
        public static readonly string ATTACH = "ATTACH";

        private readonly string _filePath = HostingEnvironment.MapPath(@"~/Download/");

        private object GetFilePath()
        {
            string fileName = base.RequestBase["fileName"];
            return new
            {
                filePath = string.Format("{0}/{1}", _filePath, fileName),
                fileName = fileName
            };
        }
        private object AddAttachment(string fileName)
        {
            return base.OperationToResult(() =>
            {
                Attachment attach = new Attachment();
                attach.DateTime = DateTime.UtcNow.ToString();
                attach.FileName = fileName;
                attach.SetGuid();
                KeyValuePair<string, Attachment> model = new KeyValuePair<string, Attachment>(attach.Guid, attach);

                return RedisBase.HashSet<KeyValuePair<string, Attachment>>(ATTACH, model.Key,model);
            });
        }
        [HttpPost]
        public async Task<HttpResponseMessage> AddAttachment()
        {
           
            if (!Request.Content.IsMimeMultipartContent("form-data"))
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            HttpResponseMessage response = null;

            dynamic fileInfo = GetFilePath();

            if (System.IO.File.Exists(fileInfo["filePath"]))
            {
                 string fileName = string.Format("{0}{1}", fileInfo["fileName"],
                   new Random().Next(999).ToString());
                fileInfo["filePath"] = fileInfo["filePath"]
                   .Replace(Path.GetFileNameWithoutExtension(fileInfo["filePath"]), fileName);
            }

            try
            {
                
                var provider = new MultipartFormDataStreamProvider(fileInfo["filePath"]);
                
                var bodyparts = await Request.Content.ReadAsMultipartAsync(provider);

                AddAttachment(fileInfo["filePath"]);
                response = Request.CreateResponse(HttpStatusCode.Accepted);
            }
            catch
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
            return response;
        }
        [HttpPost]
        public HttpResponseMessage GetAttachment()
        {
            try
            {
                dynamic fileInfo = GetFilePath();

                if (!System.IO.File.Exists(fileInfo["filePath"]))
                {
                    throw new HttpResponseException(HttpStatusCode.BadRequest);
                }

                var stream = new FileStream(fileInfo["filePath"], FileMode.Open);
                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);

                response.Content = new StreamContent(stream);
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
                {
                    FileName = fileInfo["fileName"]
                };
                return response;
            }
            catch
            {
                return new HttpResponseMessage(HttpStatusCode.NoContent);
            }
        }
    }
}
