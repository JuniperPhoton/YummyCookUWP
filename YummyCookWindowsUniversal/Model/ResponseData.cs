using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YummyCookWindowsUniversal.Model
{
    public class ResponseData
    {
        public int Code { get; set; }
        public string ErrorMessage { get; set; }
        public bool IsSuccess { get; set; }

        public ResponseData(bool isSuccess,int code, string error)
        {
            IsSuccess = isSuccess;
            Code = code;
            ErrorMessage = error;
        }

    }
}
