using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContextMenu.Models
{
	public struct ContextMenuRenderingData
	{
		public string text;
		public uint menuFlags;
		public IntPtr hbmpItem;
		/*HBITMAP hbmpChecked;
		HBITMAP hbmpUnchecked;*/
		public ContextMenuPaddingType cmpt;
		public ScaleType scaleType;
		public uint dpi;
		public bool useDarkTheme;
		public bool useSystemPadding;
		public bool forceAccelerators;
		public IntPtr parentArray;
	};
}
