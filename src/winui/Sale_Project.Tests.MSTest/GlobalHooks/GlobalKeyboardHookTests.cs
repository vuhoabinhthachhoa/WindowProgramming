using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sale_Project.Helpers;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Sale_Project.Tests.MSTest.GlobalHooks;

[TestClass]
public class GlobalKeyboardHookTests
{
    private GlobalKeyboardHook _keyboardHook;

    [TestInitialize]
    public void Setup()
    {
        _keyboardHook = new GlobalKeyboardHook();
    }

    [TestCleanup]
    public void Cleanup()
    {
        _keyboardHook.Unhook();
    }

    [TestMethod]
    public void TestSetHook()
    {
        _keyboardHook.SetHook();
        Assert.IsNotNull(_keyboardHook);
    }

    [TestMethod]
    public void TestUnhook()
    {
        _keyboardHook.SetHook();
        _keyboardHook.Unhook();
        // No exception means success
    }


    private void SimulateKeyPress(int keyCode)
    {
        // Simulate key press using SendMessage or other method
    }
}
