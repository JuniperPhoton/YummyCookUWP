using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YummyCookWindowsUniversal.Interface
{
    interface INavigable
    {
        void Activate(object param);
        void Deactivate(object param);
    }
}
