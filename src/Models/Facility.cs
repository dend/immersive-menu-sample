using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContextMenu.Models
{
    internal enum Facility
    {
        /// FACILITY_NULL

        Null = 0,
        /// FACILITY_RPC


        Rpc = 1,
        /// FACILITY_DISPATCH


        Dispatch = 2,
        /// FACILITY_STORAGE

        Storage = 3,
        /// FACILITY_ITF


        Itf = 4,
        /// FACILITY_WIN32


        Win32 = 7,
        /// FACILITY_WINDOWS

        Windows = 8,
        /// FACILITY_CONTROL

        Control = 10,
        /// MSDN doced facility code for ESE errors.

        Ese = 0xE5E,
    }
}
