using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Sale_Project.Helpers;

/// <summary>
/// A class to set a global keyboard hook to capture keyboard events.
/// </summary>
public class GlobalKeyboardHook
{
    private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);
    private LowLevelKeyboardProc _proc;
    private IntPtr _hookID = IntPtr.Zero;

    [DllImport("user32.dll", SetLastError = true)]
    private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

    [DllImport("user32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool UnhookWindowsHookEx(IntPtr hhk);

    [DllImport("user32.dll")]
    private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern IntPtr GetModuleHandle(string lpModuleName);

    private const int WH_KEYBOARD_LL = 13;
    private const int WM_KEYDOWN = 0x0100;

    /// <summary>
    /// Event triggered when a key is pressed.
    /// </summary>
    public event EventHandler<(int KeyCode, bool IsCtrlPressed)> KeyPressed;

    /// <summary>
    /// Initializes a new instance of the <see cref="GlobalKeyboardHook"/> class.
    /// </summary>
    public GlobalKeyboardHook()
    {
        _proc = HookCallback;
    }

    /// <summary>
    /// Sets the keyboard hook.
    /// </summary>
    public void SetHook()
    {
        using (Process curProcess = Process.GetCurrentProcess())
        using (ProcessModule curModule = curProcess.MainModule)
        {
            _hookID = SetWindowsHookEx(WH_KEYBOARD_LL, _proc, GetModuleHandle(curModule.ModuleName), 0);
        }
    }

    /// <summary>
    /// Unhooks the keyboard hook.
    /// </summary>
    public void Unhook()
    {
        UnhookWindowsHookEx(_hookID);
    }

    /// <summary>
    /// The callback method that processes the keyboard events.
    /// </summary>
    /// <param name="nCode">The hook code.</param>
    /// <param name="wParam">The identifier of the keyboard message.</param>
    /// <param name="lParam">A pointer to a KBDLLHOOKSTRUCT structure.</param>
    /// <returns>A pointer to the next hook procedure.</returns>
    private IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
    {
        if (nCode >= 0 && wParam == (IntPtr)WM_KEYDOWN)
        {
            int vkCode = Marshal.ReadInt32(lParam);
            bool isCtrlPressed = (GetKeyState(0x11) & 0x8000) != 0; // VK_CONTROL (Ctrl key)

            // Invoke event for Ctrl + S
            KeyPressed?.Invoke(this, (vkCode, isCtrlPressed));
        }
        return CallNextHookEx(_hookID, nCode, wParam, lParam);
    }

    [DllImport("user32.dll")]
    private static extern short GetKeyState(int nVirtKey);
}

