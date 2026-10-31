using System;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace CascBoilerplates;

public class Boilerplate
{
    public static void MainBoilerplate (Action action)
    {
        try
        {
            action();
        } catch (Win32Exception)
        {
            Console.WriteLine($"Win32 exception: {Marshal.GetLastWin32Error()}");
        }
    }
}