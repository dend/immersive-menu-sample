using ContextMenu.Models;
using Microsoft.Win32;
using System.Runtime.InteropServices;

namespace ContextMenu.Helpers
{
    internal class ImmersiveMenu
    {
        static bool shouldUseDarkTheme = false;

        int ScaleBySpecificDPI(int i, int dpi)
        {
            return Util.MulDiv(i, dpi, NativeConstants.USER_DEFAULT_SCREEN_DPI);
        }

        int ScaleByPPI(int i, IntPtr hWnd)
        {
            return Util.MulDiv(i, NativeInvoke.GetDpiForWindow(hWnd), NativeConstants.USER_DEFAULT_SCREEN_DPI);
        }

        int ScaleByType(ScaleType type, int i, IntPtr hWnd, int dpi)
        {
            if (type == ScaleType.PPI)
                return ScaleByPPI(i, hWnd);
            else
                return ScaleBySpecificDPI(i, dpi);
        }

        HRESULT ApplyOwnerDrawToMenu(IntPtr hMenu, IntPtr hWnd, POINT point, ImmersiveContextMenuOptions icmoFlags, List<ContextMenuRenderingData> cmrdArray)
        {
            HRESULT hr = HRESULT.E_FAIL;
            if (icmoFlags.HasFlag(ImmersiveContextMenuOptions.ICMO_OVERRIDECOMPATCHECK))
            {
                if (!CanApplyOwnerDrawToMenu(hMenu, hWnd))
                {
                    return HRESULT.S_FALSE;
                }
            }

            bool touchInput = false;

            if (icmoFlags.HasFlag(ImmersiveContextMenuOptions.ICMO_FORCEMOUSESTYLING))
            {
                INPUT_MESSAGE_SOURCE ims = new INPUT_MESSAGE_SOURCE();
                if (NativeInvoke.GetCurrentInputMessageSource(ref ims) && ims.deviceType == INPUT_MESSAGE_DEVICE_TYPE.IMDT_TOUCH)
                {
                    touchInput = true;
                }
            }

            if (_StoreParentArrayOnWindow(hWnd, cmrdArray))
            {
                hr = HRESULT.S_OK;
            }

            shouldUseDarkTheme = ShouldUseDarkTheme();

            bool forceAccelerators = false;
            NativeInvoke.SystemParametersInfo(SPI.SPI_GETMENUUNDERLINES, 0, ref forceAccelerators, 0);

            List<ContextMenuRenderingData> renderData = new List<ContextMenuRenderingData>();

            return hr;
        }

        bool CanApplyOwnerDrawToMenu(IntPtr hMenu, IntPtr hWnd)
	    {
		    MENUITEMINFO mii;
		    for (uint i = 0; ; ++i)
		    {
                mii = new MENUITEMINFO();
                mii.cbSize = (uint)Marshal.SizeOf(typeof(MENUITEMINFO));
                mii.fMask = (uint)(MIIM.FTYPE | MIIM.DATA | MIIM.ID);

			    if (!NativeInvoke.GetMenuItemInfo(hMenu, i, true, mii))
				    break;
			    if (mii.fType.HasFlag(MFT.MFT_OWNERDRAW) && !(mii.dwItemData != IntPtr.Zero && GetContextMenuDataForItem(hWnd, mii.dwItemData, mii.wID) != IntPtr.Zero))
				    return false;
		    }
            return true;
	    }

        bool ShouldUseDarkTheme()
        {
            const string userRoot = "HKEY_CURRENT_USER";
            const string subkey = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Themes\Personalize";
            const string keyName = userRoot + "\\" + subkey;

            return (bool)Registry.GetValue(keyName, "SystemUsesLightTheme", false);
        }

    bool _StoreParentArrayOnWindow(IntPtr hWnd, List<ContextMenuRenderingData> cmrdArray)
	    {
            GCHandle handle = GCHandle.Alloc(cmrdArray, GCHandleType.Pinned);
            return NativeInvoke.SetProp(hWnd, "ImmersiveContextMenuArray", handle.AddrOfPinnedObject());
        }

    IntPtr GetContextMenuDataForItem(IntPtr hWnd, IntPtr itemData, uint itemID)
	    {
		    string propName; // ImmersiveContextMenuArray_FFFFFFFFFFFFFFFF-4294967295
            propName = $"ImmersiveContextMenuArray_{string.Format("{0:X8}", itemData)}-{itemID}";
			return NativeInvoke.GetProp(hWnd, propName));
	    }
}
}
