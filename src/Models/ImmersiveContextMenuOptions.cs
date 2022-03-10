using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContextMenu.Models
{
	enum ImmersiveContextMenuOptions
	{
		ICMO_USEPPI = 0x1,
		ICMO_OVERRIDECOMPATCHECK = 0x2,
		ICMO_FORCEMOUSESTYLING = 0x4,
		ICMO_USESYSTEMTHEME = 0x8,
		ICMO_ICMBRUSHAPPLIED = 0x10
	};
}
