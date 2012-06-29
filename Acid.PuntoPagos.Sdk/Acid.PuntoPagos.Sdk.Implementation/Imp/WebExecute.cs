using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;
using Acid.PuntoPagos.Sdk.Interfaces;
using Acid.PuntoPagos.Sdk.Services;

namespace Acid.PuntoPagos.Sdk.Imp
{
    internal class ExecutorWeb : IExecutorWeb
    {
        private readonly ILog _logger;

        public ExecutorWeb(ILog logger)
        {
            _logger = logger;
        }

        public IDictionary<string,string> Execute(string url, string method, string jsonData, string authorization, DateTime dateNow)
        {
            try
            {
                _logger.Debug(string.Format(
                    "Start request to {0}, method: {1}, with json: {2}, authorization: {3}, dateTime: {4}",
                    url, method, string.IsNullOrEmpty(jsonData) ? "-" : jsonData, authorization, dateNow));

                var request = WebRequest.Create(url) as HttpWebRequest;
                if (request == null) return null;

                request.Method = method;
                request.UserAgent = "puntopagos-sdk-" + Assembly.GetExecutingAssembly().GetName().Version;
                request.Accept = "application/json";
                request.ContentType = "application/json; charset=utf-8";
                request.Headers.Add("Fecha", dateNow.ToString("r"));
                request.Headers.Add("Autorizacion", authorization);
                _logger.Debug("Add all headers");
                if (!string.IsNullOrEmpty(jsonData))
                {
                    _logger.Debug("Start to write stream json");
                    var content = GetContentByte(jsonData);
                    request.ContentLength = content.Length;

                    var dataStream = request.GetRequestStream();
                    dataStream.Write(content, 0, content.Length);
                    dataStream.Close();
                    _logger.Debug("End to write stream json, close stream");
                }
                _logger.Debug("Start to call response");
                var response = request.GetResponse() as HttpWebResponse;
                if (response == null) return null;
                _logger.Debug("End to call response");
                _logger.Debug("Start to read response");
                var responseDataStream = response.GetResponseStream();
                if (responseDataStream == null) return null;

                var reader = new StreamReader(responseDataStream);
                var json = reader.ReadToEnd();
                _logger.Debug("End to read response, start to Deserialize Json");
                return JsonSerializerService.DeserializeFromString(json);
            }
            catch (Exception ex)
            {
                _logger.Error(string.Format("Error when execute url: {0} and method: {1}", url, method), ex);
                throw;
            }
        }

        public IDictionary<string, string> GetDataFromRequest(WebRequest request)
        {
            try
            {
                _logger.Debug("Start to read request.");
                var responseDataStream = request.GetRequestStream();

                var reader = new StreamReader(responseDataStream);
                var json = reader.ReadToEnd();
                _logger.Debug("End to read request, start to Deserialize Json");
                return JsonSerializerService.DeserializeFromString(json);
            }
            catch (Exception ex)
            {
                _logger.Error(string.Format("Error when get response from url: {0} and method: {1}", request.RequestUri, request.Method), ex);
                throw;
            }
            
        }

        private byte[] GetContentByte(string jsonData)
        {
            return string.IsNullOrEmpty(jsonData) ? Encoding.UTF8.GetBytes("") : Encoding.UTF8.GetBytes(jsonData);
        } 
    }
}