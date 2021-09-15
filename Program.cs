using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AutoSwiper
{
    class Program
    {
        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int wMsg, int wParam, int lParam);

        const int WM_KEYDOWN = 0x0100;
        const int WM_KEYUP = 0x0101;
        const int VK_SPACE = 0x20;

        static void ClickOnWindowContinually(IntPtr windowHandle)
        {
            while (true)
            {
                SendMessage(windowHandle, WM_KEYDOWN, VK_SPACE, 0);
                Thread.Sleep(5);
                SendMessage(windowHandle, WM_KEYUP, VK_SPACE, 0);
                Thread.Sleep(5);
            }
        }
        static void Main(string[] args)
        {
            const string tinderName = "Tinder | Dating, Make Friends & Meet New People";
            List<IntPtr> tinderWindows = new List<IntPtr>();
            //find all open tinders on the computer
            Process[] processlist = Process.GetProcesses();
            foreach (Process process in processlist)
            {
                if (!String.IsNullOrEmpty(process.MainWindowTitle))
                {
                    Console.WriteLine("Process: {0} ID: {1} Window title: {2}", process.ProcessName, process.Id, process.MainWindowTitle);

                    if (process.MainWindowTitle.Contains(tinderName))
                    {
                        IntPtr WindowHandle = process.MainWindowHandle;
                        tinderWindows.Add(WindowHandle);
                    }
                }
            }


            //loop through all tinder windows, create threads for each and send window message for automatic swiping
            foreach (IntPtr windowHandle in tinderWindows)
                new Thread(() => ClickOnWindowContinually(windowHandle)).Start();

            while (true)
                Thread.Sleep(100);
        }
    }
}
