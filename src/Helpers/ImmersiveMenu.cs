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

            ContextMenuRenderingData renderData = new ContextMenuRenderingData();

            for (uint i = 0; i < 1; ++i)
            {
                string buf = new string('\0', 64);
                MENUITEMINFO mii = new MENUITEMINFO();
                mii.cbSize = (uint)Marshal.SizeOf(typeof(MENUITEMINFO));
                mii.fMask = MIIM.FTYPE | MIIM.BITMAP | MIIM.STRING | MIIM.DATA | MIIM.CHECKMARKS | MIIM.SUBMENU | MIIM.ID | MIIM.STATE;
                mii.dwTypeData = buf;
                mii.cch = (uint)buf.Length;

                if (!NativeInvoke.GetMenuItemInfo(hMenu, i, true, mii))
                {
                    hr = HResultFromWin32Error(Marshal.GetLastWin32Error());
                    continue;
                }

                hr = HRESULT.S_OK;

                // There are some string operations here
                // on buf that I don't think we need for now.

                var parentCmrd = GetContextMenuDataForItem(hWnd, mii.dwItemData, mii.wID);

                if (parentCmrd != IntPtr.Zero)
                {
                    GCHandle handle = (GCHandle)parentCmrd;
                    object handledObj = handle.Target;
                    renderData = (ContextMenuRenderingData)handledObj;
                    handle.Free();
                }

                if ((!mii.fType.HasFlag(MFT.MFT_OWNERDRAW) || mii.dwItemData == IntPtr.Zero) && (parentCmrd == IntPtr.Zero || touchInput != renderData.cmpt.HasFlag(ContextMenuPaddingType.CMPT_TOUCH_INPUT)))
                {
                    ContextMenuRenderingData newRenderData = new ContextMenuRenderingData();

                }

            }

            return hr;
        }

        public static HRESULT HResultFromWin32Error(int error)
        {
            if (error <= 0)
            {
                return (HRESULT)((uint)error);
            }

            return MakeHRESULT(true, Facility.Win32, error & 0x0000FFFF);
        }

        public static HRESULT MakeHRESULT(bool severe, Facility facility, int code)
        {
            return (HRESULT)((uint)((severe ? (1 << 31) : 0) | ((int)facility << 16) | code));
        }

        bool CanApplyOwnerDrawToMenu(IntPtr hMenu, IntPtr hWnd)
        {
            MENUITEMINFO mii;
            for (uint i = 0; ; ++i)
            {
                mii = new MENUITEMINFO();
                mii.cbSize = (uint)Marshal.SizeOf(typeof(MENUITEMINFO));
                mii.fMask = MIIM.FTYPE | MIIM.DATA | MIIM.ID;

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

        HRESULT _GetRenderingDataForMenuItem(ScaleType type, MENUITEMINFO mii, string itemText, POINT point, List<ContextMenuRenderingData> cmrdArray, ContextMenuRenderingData parentCmrd, ImmersiveContextMenuOptions icmoFlags, ref ContextMenuRenderingData cmrd)
	    {
		    cmrd = new ContextMenuRenderingData();
            ContextMenuRenderingData renderData = new ContextMenuRenderingData();

			renderData.text = itemText;
			renderData.hbmpItem = mii.hbmpItem;
            //renderData->hbmpChecked = mii->hbmpChecked;
            //renderData->hbmpUnchecked = mii->hbmpUnchecked;

            if (parentCmrd != null)
            {
                renderData.dpi = parentCmrd.dpi;
            }
            else if (cmrdArray->empty())
                renderData->dpi = _GetDpiForMonitorFromPoint(point);
            else
                renderData->dpi = cmrdArray->front()->dpi;

			if (WI_IsFlagSet(mii->fType, MFT_SEPARATOR))
				renderData->menuFlags = MFT_SEPARATOR;
			else if (mii->hSubMenu)
				renderData->menuFlags = SUBMENU;
			else
				renderData->menuFlags = 0;
			WI_SetFlagIf(renderData->menuFlags, MFT_RADIOCHECK, WI_IsFlagSet(mii->fType, MFT_RADIOCHECK));

			renderData->cmpt = CMPT_NONE;
			renderData->scaleType = type;
			renderData->useDarkTheme = shouldUseDarkTheme;
			renderData->useSystemPadding = WI_IsFlagSet(icmoFlags, ICMO_USESYSTEMTHEME);

			*cmrd = renderData;

			return S_OK;

	    }
    }
}
