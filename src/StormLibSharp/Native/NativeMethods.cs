using System.Runtime.InteropServices;
using StormLibSharpModels;

namespace StormLibSharpNative;

public class NativeMethods {
    [DllImport("StormLib.dll", CharSet = CharSet.Ansi, SetLastError = true)][return: MarshalAs(UnmanagedType.U1)] public static extern bool SFileGetFileInfo(IntPtr hMpq, _SFileInfoClass infoClass, IntPtr pvFileInfo,  uint cbFileInfo, out uint pcbLengthNeeded);
    [DllImport("StormLib.dll", CharSet = CharSet.Ansi, SetLastError = true)][return: MarshalAs(UnmanagedType.U1)] public static extern bool SFileUpdateFileAttributes(IntPtr hMpq, string szFileName);
    [DllImport("StormLib.dll", CharSet = CharSet.Ansi, SetLastError = true)][return: MarshalAs(UnmanagedType.U1)] public static extern bool SFileSetAttributes(IntPtr h, uint dwFlags);
    [DllImport("StormLib.dll", CharSet = CharSet.Ansi, SetLastError = true)][return: MarshalAs(UnmanagedType.U4)] public static extern uint SFileGetAttributes(IntPtr h);
    [DllImport("StormLib.dll", CharSet = CharSet.Ansi, SetLastError = true)][return: MarshalAs(UnmanagedType.U1)] public static extern bool SFileOpenArchive(string path, uint prio, uint flags, out IntPtr h);
    [DllImport("StormLib.dll", CharSet = CharSet.Ansi, SetLastError = true)][return: MarshalAs(UnmanagedType.U1)] public static extern bool SFileAddFileEx(IntPtr h, string disk, string name, uint flags, uint comp, uint comp2);
    [DllImport("StormLib.dll", CharSet = CharSet.Ansi, SetLastError = true)][return: MarshalAs(UnmanagedType.U1)] public static extern bool SFileRemoveFile(IntPtr h, string szFileName, uint dwSearchScope);
    [DllImport("StormLib.dll", CharSet = CharSet.Ansi, SetLastError = true)][return: MarshalAs(UnmanagedType.U1)] public static extern bool SFileOpenFileEx(IntPtr h, string szFileName, uint dwSearchScope, out IntPtr hFile);
    [DllImport("StormLib.dll", CharSet = CharSet.Ansi, SetLastError = true)][return: MarshalAs(UnmanagedType.U1)] public static extern bool SFileCloseFile(IntPtr h);
    [DllImport("StormLib.dll", CharSet = CharSet.Ansi, SetLastError = true)][return: MarshalAs(UnmanagedType.U1)] public static extern bool SFileHasFile(IntPtr hMpq, string szFileName);
    [DllImport("StormLib.dll", CharSet = CharSet.Ansi, SetLastError = true)][return: MarshalAs(UnmanagedType.U1)] public static extern bool SFileCloseArchive(IntPtr h);
    [DllImport("StormLib.dll", CharSet = CharSet.Ansi, SetLastError = true)][return: MarshalAs(UnmanagedType.U1)] public static extern bool SFileFlushArchive(IntPtr h);
    [DllImport("StormLib.dll", CharSet = CharSet.Ansi, SetLastError = true)][return: MarshalAs(UnmanagedType.U1)] public static extern bool SFileReadFile(IntPtr hFile, [Out] byte[] pvBuffer, uint dwToRead, out uint pdwRead, IntPtr lpOverlapped);
    [DllImport("StormLib.dll", CharSet = CharSet.Ansi, SetLastError = true)][return: MarshalAs(UnmanagedType.U4)] public static extern uint SFileGetFileSize(IntPtr hFile, out uint pdwFileSizeHigh);
    [DllImport("StormLib.dll", CharSet = CharSet.Ansi, SetLastError = true)][return: MarshalAs(UnmanagedType.U4)] public static extern uint SFileExtractFile(IntPtr hMpq, string szToExtract, string szExtracted, uint dwSearchScope);
    public static void Main(){}
}