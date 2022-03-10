using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ContextMenu.Models
{
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
	public struct INPUT_MESSAGE_SOURCE
	{
		/// <summary>The device type (INPUT_MESSAGE_DEVICE_TYPE) of the source of the input message.</summary>
		public INPUT_MESSAGE_DEVICE_TYPE deviceType;

		/// <summary>The ID (INPUT_MESSAGE_ORIGIN_ID) of the source of the input message.</summary>
		public INPUT_MESSAGE_ORIGIN_ID originId;
	}

}
