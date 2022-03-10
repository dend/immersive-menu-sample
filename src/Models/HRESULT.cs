using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContextMenu.Models
{
    internal enum HRESULT : uint
    {
        S_FALSE = 0x0001,
        S_OK = 0x0000,
        E_INVALIDARG = 0x80070057,
        E_OUTOFMEMORY = 0x8007000E,
        E_FAIL = 0x80004005,
    }
}
