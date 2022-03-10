using ContextMenu.Models;
using System.Runtime.InteropServices;

namespace ContextMenu.Helpers
{
    internal static class NativeInvoke
    {
        [DllImport("user32.dll")]
        internal static extern IntPtr CreatePopupMenu();

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        internal static extern bool InsertMenu(IntPtr hMenu, uint uPosition, uint uFlags, uint uIDNewItem, string lpNewItem);

        [DllImport("user32.dll")]
        internal static extern bool TrackPopupMenuEx(IntPtr hmenu, uint fuFlags, int x, int y, IntPtr hwnd, IntPtr lptpm);

        [DllImport("user32.dll")]
        internal static extern int GetDpiForWindow(IntPtr hWnd);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        internal static extern bool GetMenuItemInfo(IntPtr hMenu, UInt32 uItem, bool fByPosition, [In, Out] MENUITEMINFO lpmii);

        [DllImport("user32.dll", SetLastError = true)]
        internal static extern IntPtr GetProp(IntPtr hWnd, string lpString);
    }
}
