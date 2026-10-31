using System;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace Boilerplates;

public class Boilerplate
{
    public static void MainBoilerplate (Action action)
    {
        try
        {
            action();
        } catch (Win32Exception)
        {
            var err = Marshal.GetLastWin32Error();
            Console.WriteLine($"LOG: Win32 error ({err}).");
        } catch (Exception ex)
        {
            Console.WriteLine($"LOG: Read error ({ex.Message}).");
        }
    }
}