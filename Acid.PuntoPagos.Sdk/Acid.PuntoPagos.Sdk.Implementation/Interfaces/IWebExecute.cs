using System;
using System.Collections.Generic;
using System.Net;

namespace Acid.PuntoPagos.Sdk.Interfaces
{
    public interface IExecutorWeb
    {
        IDictionary<string, string> Execute(string url, string method, string jsonData, string authorization, DateTime dateNow);
        IDictionary<string, string> GetDataFromRequest(WebRequest request);
    }
}