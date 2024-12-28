using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sale_Project.Helpers;
using System;

namespace Sale_Project.Tests.MSTest.GlobalHooks;

[TestClass]
public class GlobalMouseHookTests
{
    private GlobalMouseHook _globalMouseHook;
    private string _detectedClick;

    [TestInitialize]
    public void Setup()
    {
        _globalMouseHook = new GlobalMouseHook();
        _globalMouseHook.MouseClickDetected += (sender, clickType) => _detectedClick = clickType;
    }

    [TestCleanup]
    public void Cleanup()
    {
        _globalMouseHook.Unhook();
    }

    [TestMethod]
    public void TestLeftClickDetection()
    {
        // Simulate left mouse button down
        _globalMouseHook.SetHook();
        _globalMouseHook.HookCallback(0, (IntPtr)0x0201, IntPtr.Zero);

        Assert.AreEqual("Left Click Detected", _detectedClick);
    }

    [TestMethod]
    public void TestRightClickDetection()
    {
        // Simulate right mouse button down
        _globalMouseHook.SetHook();
        _globalMouseHook.HookCallback(0, (IntPtr)0x0204, IntPtr.Zero);

        Assert.AreEqual("Right Click Detected", _detectedClick);
    }
}

