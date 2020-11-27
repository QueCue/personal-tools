using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Memo
{
    class MouseHook
    {
        public delegate void MouseClickHandler(object sender, MouseEventArgs e);

        public delegate void MouseDownHandler(object sender, MouseEventArgs e);

        public delegate void MouseMoveHandler(object sender, MouseEventArgs e);

        public delegate void MouseUpHandler(object sender, MouseEventArgs e);

        private const int WM_MOUSEMOVE = 0x200;
        private const int WM_LBUTTONDOWN = 0x201;
        private const int WM_RBUTTONDOWN = 0x204;
        private const int WM_MBUTTONDOWN = 0x207;
        private const int WM_LBUTTONUP = 0x202;
        private const int WM_RBUTTONUP = 0x205;
        private const int WM_MBUTTONUP = 0x208;
        private const int WM_LBUTTONDBLCLK = 0x203;
        private const int WM_RBUTTONDBLCLK = 0x206;
        private const int WM_MBUTTONDBLCLK = 0x209;

        public const int WH_MOUSE_LL = 14;
        private static int hMouseHook = 0;
        private int hHook;
        public Win32.HookProc hProc;
        private Point point;

        public MouseHook() => Point = new Point();

        private Point Point
        {
            get => point;
            set
            {
                if (point != value)
                {
                    point = value;
                    if (MouseMoveEvent != null)
                    {
                        var e = new MouseEventArgs(MouseButtons.None, 0, point.X, point.Y, 0);
                        MouseMoveEvent(this, e);
                    }
                }
            }
        }

        public int SetHook()
        {
            hProc = MouseHookProc;
            hHook = Win32.SetWindowsHookEx(WH_MOUSE_LL, hProc, IntPtr.Zero, 0);
            return hHook;
        }

        public void UnHook()
        {
            Win32.UnhookWindowsHookEx(hHook);
        }

        private int MouseHookProc(int nCode, IntPtr wParam, IntPtr lParam)
        {
            var MyMouseHookStruct =
                (Win32.MouseHookStruct) Marshal.PtrToStructure(lParam, typeof(Win32.MouseHookStruct));
            if (nCode < 0)
            {
                return Win32.CallNextHookEx(hHook, nCode, wParam, lParam);
            }

            var button = MouseButtons.None;
            var clickCount = 0;
            switch ((int) wParam)
            {
                case WM_LBUTTONDOWN:
                    button = MouseButtons.Left;
                    clickCount = 1;
                    MouseDownEvent(this, new MouseEventArgs(button, clickCount, point.X, point.Y, 0));
                    break;
                case WM_RBUTTONDOWN:
                    button = MouseButtons.Right;
                    clickCount = 1;
                    MouseDownEvent(this, new MouseEventArgs(button, clickCount, point.X, point.Y, 0));
                    break;
                case WM_MBUTTONDOWN:
                    button = MouseButtons.Middle;
                    clickCount = 1;
                    MouseDownEvent(this, new MouseEventArgs(button, clickCount, point.X, point.Y, 0));
                    break;
                case WM_LBUTTONUP:
                    button = MouseButtons.Left;
                    clickCount = 1;
                    MouseUpEvent(this, new MouseEventArgs(button, clickCount, point.X, point.Y, 0));
                    break;
                case WM_RBUTTONUP:
                    button = MouseButtons.Right;
                    clickCount = 1;
                    MouseUpEvent(this, new MouseEventArgs(button, clickCount, point.X, point.Y, 0));
                    break;
                case WM_MBUTTONUP:
                    button = MouseButtons.Middle;
                    clickCount = 1;
                    MouseUpEvent(this, new MouseEventArgs(button, clickCount, point.X, point.Y, 0));
                    break;
            }

            Point = new Point(MyMouseHookStruct.pt.x, MyMouseHookStruct.pt.y);
            return Win32.CallNextHookEx(hHook, nCode, wParam, lParam);
        }

        public event MouseMoveHandler MouseMoveEvent;
        public event MouseClickHandler MouseClickEvent;
        public event MouseDownHandler MouseDownEvent;
        public event MouseUpHandler MouseUpEvent;
    }
}