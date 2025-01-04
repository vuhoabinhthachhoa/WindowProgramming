using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Sale_Project.Helpers;

public class GlobalMouseHook
{
    private delegate IntPtr LowLevelMouseProc(int nCode, IntPtr wParam, IntPtr lParam);
    private LowLevelMouseProc _proc;
    private IntPtr _hookID = IntPtr.Zero;

    [DllImport("user32.dll", SetLastError = true)]
    private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelMouseProc lpfn, IntPtr hMod, uint dwThreadId);

    [DllImport("user32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool UnhookWindowsHookEx(IntPtr hhk);

    [DllImport("user32.dll")]
    private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern IntPtr GetModuleHandle(string lpModuleName);

    private const int WH_MOUSE_LL = 14;  // Mouse hook constant
    private const int WM_LBUTTONDOWN = 0x0201; // Left Mouse Button Down
    private const int WM_RBUTTONDOWN = 0x0204; // Right Mouse Button Down

    public event EventHandler<string> MouseClickDetected;

    public GlobalMouseHook()
    {
        _proc = HookCallback;
    }

    public void SetHook()
    {
        using (Process curProcess = Process.GetCurrentProcess())
        using (ProcessModule curModule = curProcess.MainModule)
        {
            _hookID = SetWindowsHookEx(WH_MOUSE_LL, _proc, GetModuleHandle(curModule.ModuleName), 0);
        }
    }

    public void Unhook()
    {
        UnhookWindowsHookEx(_hookID);
    }

    private IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
    {
        if (nCode >= 0)
        {
            if (wParam == (IntPtr)WM_LBUTTONDOWN)
            {
                MouseClickDetected?.Invoke(this, "Left Click Detected");
            }
            else if (wParam == (IntPtr)WM_RBUTTONDOWN)
            {
                MouseClickDetected?.Invoke(this, "Right Click Detected");
            }
        }

        return CallNextHookEx(_hookID, nCode, wParam, lParam);
    }
}
