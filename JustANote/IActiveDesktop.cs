using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace JustANote
{
    [ComImport]
    [Guid("F490EB00-1240-11D1-9888-006097DEACF9")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IActiveDesktop
    {
        [PreserveSig]
        int ApplyChanges(AD_Apply dwFlags);
        [PreserveSig]
        int GetWallpaper([MarshalAs(UnmanagedType.LPWStr)]  System.Text.StringBuilder pwszWallpaper,
                  int cchWallpaper,
                  int dwReserved);
        [PreserveSig]
        int SetWallpaper([MarshalAs(UnmanagedType.LPWStr)] string pwszWallpaper, int dwReserved);
        [PreserveSig]
        int GetWallpaperOptions(ref WALLPAPEROPT pwpo, int dwReserved);
        [PreserveSig]
        int SetWallpaperOptions(ref WALLPAPEROPT pwpo, int dwReserved);
        [PreserveSig]
        int GetPattern([MarshalAs(UnmanagedType.LPWStr)] System.Text.StringBuilder pwszPattern, int cchPattern, int dwReserved);
        [PreserveSig]
        int SetPattern([MarshalAs(UnmanagedType.LPWStr)] string pwszPattern, int dwReserved);
        [PreserveSig]
        int GetDesktopItemOptions(ref COMPONENTSOPT pco, int dwReserved);
        [PreserveSig]
        int SetDesktopItemOptions(ref COMPONENTSOPT pco, int dwReserved);
        //[PreserveSig]
        //int AddDesktopItem(ref COMPONENT pcomp, int dwReserved);
        //[PreserveSig]
        //int AddDesktopItemWithUI(IntPtr hwnd, ref COMPONENT pcomp, DtiAddUI dwFlags);
        //[PreserveSig]
        //int ModifyDesktopItem(ref COMPONENT pcomp, ComponentModify dwFlags);
        //[PreserveSig]
        //int RemoveDesktopItem(ref COMPONENT pcomp, int dwReserved);
        //[PreserveSig]
        //int GetDesktopItemCount(out int lpiCount, int dwReserved);
        //[PreserveSig]
        //int GetDesktopItem(int nComponent, ref COMPONENT pcomp, int dwReserved);
        //[PreserveSig]
        //int GetDesktopItemByID(IntPtr dwID, ref COMPONENT pcomp, int dwReserved);
        //[PreserveSig]
        //int GenerateDesktopItemHtml([MarshalAs(UnmanagedType.LPWStr)] string pwszFileName, ref COMPONENT pcomp, int dwReserved);
        //[PreserveSig]
        //int AddUrl(IntPtr hwnd, [MarshalAs(UnmanagedType.LPWStr)] string pszSource, ref COMPONENT pcomp, AddURL dwFlags);
        //[PreserveSig]
        //int GetDesktopItemBySource([MarshalAs(UnmanagedType.LPWStr)] string pwszSource, ref COMPONENT pcomp, int dwReserved);
    }

    public class Constants
    {
        public const int AD_GETWP_LAST_APPLIED = (0x00000002);

        ////////////////////////////////////////////
        // Flags for IActiveDesktop::ApplyChanges()
        //public const int  AD_APPLY_SAVE             = 0x00000001;
        //public const int  AD_APPLY_HTMLGEN          = 0x00000002;
        //public const int  AD_APPLY_REFRESH          = 0x00000004;
        //public const int  AD_APPLY_ALL              = (AD_APPLY_SAVE | AD_APPLY_HTMLGEN | AD_APPLY_REFRESH);
        //public const int  AD_APPLY_FORCE            = 0x00000008;
        //public const int  AD_APPLY_BUFFERED_REFRESH = 0x00000010;
        //public const int  AD_APPLY_DYNAMICREFRESH   = 0x00000020;

    }

    [Flags]
    public enum AddURL : int
    {
        SILENT = 0x0001
    }

    public enum DtiAddUI : int
    {
        DEFAULT = 0x00000000,
        DISPSUBWIZARD = 0x00000001,
        POSITIONITEM = 0x00000002,
    }

    [Flags]
    public enum AD_Apply : int
    {
        SAVE = 0x00000001,
        HTMLGEN = 0x00000002,
        REFRESH = 0x00000004,
        ALL = SAVE | HTMLGEN | REFRESH,
        FORCE = 0x00000008,
        BUFFERED_REFRESH = 0x00000010,
        DYNAMICREFRESH = 0x00000020
    }

    public enum WallPaperStyle : int
    {
        WPSTYLE_CENTER = 0,
        WPSTYLE_TILE = 1,
        WPSTYLE_STRETCH = 2,
        WPSTYLE_MAX = 3
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct WALLPAPEROPT
    {
        public static readonly int SizeOf = Marshal.SizeOf(typeof(WALLPAPEROPT));
        public int dwSize;
        public WallPaperStyle dwStyle;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct COMPONENTSOPT
    {
        public static readonly int SizeOf = Marshal.SizeOf(typeof(COMPONENTSOPT));
        public int dwSize;
        [MarshalAs(UnmanagedType.Bool)]
        public bool fEnableComponents;
        [MarshalAs(UnmanagedType.Bool)]
        public bool fActiveDesktop;
    }

    [Flags]
    public enum ComponentModify : int
    {
        TYPE = 0x00000001,
        CHECKED = 0x00000002,
        DIRTY = 0x00000004,
        NOSCROLL = 0x00000008,
        POS_LEFT = 0x00000010,
        POS_TOP = 0x00000020,
        SIZE_WIDTH = 0x00000040,
        SIZE_HEIGHT = 0x00000080,
        POS_ZINDEX = 0x00000100,
        SOURCE = 0x00000200,
        FRIENDLYNAME = 0x00000400,
        SUBSCRIBEDURL = 0x00000800,
        ORIGINAL_CSI = 0x00001000,
        RESTORED_CSI = 0x00002000,
        CURITEMSTATE = 0x00004000,
        ALL = TYPE | CHECKED | DIRTY | NOSCROLL | POS_LEFT | SIZE_WIDTH |
                  SIZE_HEIGHT | POS_ZINDEX | SOURCE |
                  FRIENDLYNAME | POS_TOP | SUBSCRIBEDURL | ORIGINAL_CSI |
                  RESTORED_CSI | CURITEMSTATE
    }

    //typedef struct _tagCOMPONENT
    //{
    //    DWORD   dwSize;             //Size of this structure
    //    DWORD   dwID;               //Reserved: Set it always to zero.
    //    int     iComponentType;     //One of COMP_TYPE_*
    //    BOOL    fChecked;           // Is this component enabled?
    //    BOOL    fDirty;             // Had the component been modified and not yet saved to disk?
    //    BOOL    fNoScroll;          // Is the component scrollable?
    //    COMPPOS cpPos;              // Width, height etc.,
    //    WCHAR   wszFriendlyName[MAX_PATH];          // Friendly name of component.
    //    WCHAR   wszSource[INTERNET_MAX_URL_LENGTH]; //URL of the component.
    //    WCHAR   wszSubscribedURL[INTERNET_MAX_URL_LENGTH]; //Subscrined URL
    //
    //    //New fields are added below. Everything above here must exactly match the IE4COMPONENT Structure.
    //    DWORD           dwCurItemState; // Current state of the Component.
    //    COMPSTATEINFO   csiOriginal;    // Original state of the component when it was first added.
    //    COMPSTATEINFO   csiRestored;    // Restored state of the component.
    //}
    //COMPONENT;

    //[StructLayout(LayoutKind.Sequential)]
    //public struct COMPONENT
    //{
    //    public static readonly int SizeOf = Marshal.SizeOf(typeof (COMPONENT));

    //    public int dwSize;
    //    public int dwId;
    //    public int iComponentType;
    //    [MarshalAs(UnmanagedType.Bool)]
    //    public bool fChecked;
    //    [MarshalAs(UnmanagedType.Bool)]
    //    public bool fDirty;
    //    [MarshalAs(UnmanagedType.Bool)]
    //    public bool fNoScroll;
    //}
}
