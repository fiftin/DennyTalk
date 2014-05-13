using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace DennyTalk
{

    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
    public struct FLASHWINFO
    {
        public UInt32 cbSize;
        public IntPtr hwnd;
        public Int32 dwFlags;
        public UInt32 uCount;
        public Int32 dwTimeout;
    }

    public static class WinFormsHelper
    {

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        static extern Int32 FlashWindowEx(ref FLASHWINFO pwfi);

        // stop flashing
        const int FLASHW_STOP = 0;

        // flash the window title 
        const int FLASHW_CAPTION = 1;

        // flash the taskbar button
        const int FLASHW_TRAY = 2;

        // 1 | 2
        const int FLASHW_ALL = 3;
        // flash continuously 
        const int FLASHW_TIMER = 4;

        // flash until the window comes to the foreground 
        const int FLASHW_TIMERNOFG = 12;


        public static void StartBlinking (Form form)
		{
			if (Common.CommonUtil.IsWindows) {
				FLASHWINFO fw = new FLASHWINFO ();
				fw.cbSize = Convert.ToUInt32 (Marshal.SizeOf (typeof(FLASHWINFO)));
				fw.hwnd = form.Handle;
				fw.dwFlags = FLASHW_TRAY;
				fw.uCount = UInt32.MaxValue;
				FlashWindowEx (ref fw);
			}
        }

        public static void StopBlinking (Form form)
		{
			if (Common.CommonUtil.IsWindows) {
				FLASHWINFO fw = new FLASHWINFO ();
				fw.cbSize = Convert.ToUInt32 (Marshal.SizeOf (typeof(FLASHWINFO)));
				fw.hwnd = form.Handle;
				fw.dwFlags = FLASHW_STOP;
				fw.uCount = UInt32.MaxValue;
				FlashWindowEx (ref fw);
			}
        }

        public static bool IsFocused(Control cont)
        {
            if (cont.Focused)
                return true;
            foreach (Control child in cont.Controls)
            {
                if (IsFocused(child))
                    return true;
            }
            return false;
        }
    }
}
