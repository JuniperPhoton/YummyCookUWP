using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YummyCookWindowsUniversal.Model
{
    public class RequestError
    {
        public string Code { get; set; }
        public string ErrorMessage { get; set; }

        public RequestError(string code, string error)
        {
            Code = code;
            ErrorMessage = error;
        }

    }
}
