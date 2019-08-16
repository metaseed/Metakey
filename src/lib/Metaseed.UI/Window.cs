﻿using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Threading;
using Metaseed.UI.Implementation;
using Point = System.Drawing.Point;
using Size = System.Drawing.Size;

namespace Metaseed.UI
{
    public class Window
    {

        static Dispatcher _dispatcher = Dispatcher.CurrentDispatcher;

        public static void Dispatch(Delegate action)
        {
            _dispatcher.BeginInvoke(DispatcherPriority.Send, action);
        }

        public static async Task<T> Dispatch<T>(Func<T> action)
        {
            var o = _dispatcher.BeginInvoke(DispatcherPriority.Send, action);
            await o;
            return (T) (o.Result);
        }

        public static string CurrentWindowClass
        {
            get
            {
                var hWnd = PInvokes.GetForegroundWindow();
                return WindowManager.GetClassName(hWnd);
            }
        }



        public static IntPtr CurrentWindowHandle => PInvokes.GetForegroundWindow();


        public static void FocusControl(string className, string text)
        {
            var hWnd = PInvokes.GetForegroundWindow();
            var hControl = PInvokes.FindWindowEx(hWnd, IntPtr.Zero, className, text);
            PInvokes.SetFocus(hControl);
        }

        public static void Show(IntPtr hWnd)
        {
            PInvokes.ShowWindowAsync(hWnd, PInvokes.SW.Show);
            PInvokes.SetForegroundWindow(hWnd);
        }

        public static Rect GetCurrentWindowCaretPosition()
        {
           var guiInfo        = new GUITHREADINFO();
            guiInfo.cbSize = (uint)Marshal.SizeOf(guiInfo);

            PInvokes.GetGUIThreadInfo(0, out guiInfo);

            var lt = new Point((int)guiInfo.rcCaret.Left, (int)guiInfo.rcCaret.Top);
            var rb = new Point((int)guiInfo.rcCaret.Right, (int)guiInfo.rcCaret.Bottom);

            PInvokes.ClientToScreen(guiInfo.hwndCaret, out lt);
            PInvokes.ClientToScreen(guiInfo.hwndCaret, out rb);
            Console.WriteLine(lt.ToString()+ rb.ToString());
            //SystemInformation.WorkingArea
            return new Rect(new System.Windows.Point() { X = lt.X, Y = lt.Y }, new System.Windows.Point() { X = rb.X, Y = rb.Y });
        }

        public static Rect GetCurrentWindowRect()
        {
            PInvokes.GetWindowRect(CurrentWindowHandle, out var rect);
            return new Rect(new System.Windows.Point(){X = rect.Left, Y = rect.Top}, new System.Windows.Point() { X = rect.Right, Y = rect.Bottom });
        }

        public static void InitialConsole()
        {
            PInvokes.AllocConsole();
            var handle = PInvokes.GetConsoleWindow();
#if !DEBUG
            PInvokes.ShowWindowAsync(handle, PInvokes.SW.Hide);
#endif
        }

        public static void ShowConsole()
        {
            var handle = PInvokes.GetConsoleWindow();
            if (handle == IntPtr.Zero)
            {
                PInvokes.AllocConsole();
                return;
            }

            PInvokes.ShowWindowAsync(handle, PInvokes.SW.Show);
        }

        public static void HideConsole()
        {
            var handle = PInvokes.GetConsoleWindow();
            PInvokes.ShowWindowAsync(handle, PInvokes.SW.Hide);
        }

        public static bool IsExplorerOrOpenSaveDialog
        {
            get
            {
                var c = UI.Window.CurrentWindowClass;
                return "CabinetWClass" == c || "#32770" == c;
            }
        }
    }
}
