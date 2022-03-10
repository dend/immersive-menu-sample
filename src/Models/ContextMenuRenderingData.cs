using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContextMenu.Models
{
	struct ContextMenuRenderingData
	{
		string text;
		uint menuFlags;
		IntPtr hbmpItem;
		/*HBITMAP hbmpChecked;
		HBITMAP hbmpUnchecked;*/
		ContextMenuPaddingType cmpt;
		ScaleType scaleType;
		uint dpi;
		bool useDarkTheme;
		bool useSystemPadding;
		bool forceAccelerators;
		IntPtr parentArray;
	};
}
