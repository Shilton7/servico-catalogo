using Newtonsoft.Json;
using System;

namespace CatalogoAPI.Models
{
    public class ErrorDetails
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public string StackTrace { get; set; }
        public string DatError { get; private set; } = DateTime.Now.ToLongTimeString();

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }

    }
}
