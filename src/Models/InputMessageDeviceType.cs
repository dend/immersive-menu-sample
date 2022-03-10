using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContextMenu.Models
{
	[Flags]
	public enum INPUT_MESSAGE_DEVICE_TYPE
	{
		/// <summary>The device type isn't identified.</summary>
		IMDT_UNAVAILABLE = 0x00,

		/// <summary>Keyboard input.</summary>
		IMDT_KEYBOARD = 0x01,

		/// <summary>Mouse input.</summary>
		IMDT_MOUSE = 0x02,

		/// <summary>Touch input.</summary>
		IMDT_TOUCH = 0x04,

		/// <summary>Pen or stylus input.</summary>
		IMDT_PEN = 0x08,

		/// <summary>Touchpad input (Windows 8.1 and later).</summary>
		IMDT_TOUCHPAD = 0x10,
	}
}
