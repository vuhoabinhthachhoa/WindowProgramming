using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Sale_Project.Helpers;

/// <summary>
/// A class to set a global mouse hook to detect mouse clicks.
/// </summary>
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

    /// <summary>
    /// Event triggered when a mouse click is detected.
    /// </summary>
    public event EventHandler<string> MouseClickDetected;

    /// <summary>
    /// Initializes a new instance of the <see cref="GlobalMouseHook"/> class.
    /// </summary>
    public GlobalMouseHook()
    {
        _proc = HookCallback;
    }

    /// <summary>
    /// Sets the global mouse hook.
    /// </summary>
    public void SetHook()
    {
        using (Process curProcess = Process.GetCurrentProcess())
        using (ProcessModule curModule = curProcess.MainModule)
        {
            _hookID = SetWindowsHookEx(WH_MOUSE_LL, _proc, GetModuleHandle(curModule.ModuleName), 0);
        }
    }

    /// <summary>
    /// Unhooks the global mouse hook.
    /// </summary>
    public void Unhook()
    {
        UnhookWindowsHookEx(_hookID);
    }

    /// <summary>
    /// The callback method that processes the mouse events.
    /// </summary>
    /// <param name="nCode">The hook code.</param>
    /// <param name="wParam">The identifier of the mouse message.</param>
    /// <param name="lParam">A pointer to a <see cref="MSLLHOOKSTRUCT"/> structure.</param>
    /// <returns>A pointer to the next hook procedure.</returns>
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
