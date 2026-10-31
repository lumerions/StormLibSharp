using System;
using System.Net.Http.Headers;
using System.Runtime.InteropServices;
using CascModels;

namespace CascNativeMethods;

public class NativeMethods
{
    private const string dllName = "CascLib.dll";
    [DllImport(dllName, CharSet = CharSet.Ansi, SetLastError = true)] [return: MarshalAs(UnmanagedType.U1)] public static extern bool CascOpenStorage(string szParams, uint dwLocaleMask, out IntPtr hStorage);
    [DllImport(dllName, CharSet = CharSet.Ansi, SetLastError = true)] [return: MarshalAs(UnmanagedType.U1)] public static extern bool CascCloseStorage(IntPtr hStorage);
    [DllImport(dllName, CharSet = CharSet.Ansi, SetLastError = true)] [return: MarshalAs(UnmanagedType.U1)] public static extern bool CascOpenFile(IntPtr hStorage, string pvFileName, uint dwLocaleFlags, uint dwOpenFlags, out IntPtr hFile);
    [DllImport(dllName, CharSet = CharSet.Ansi, SetLastError = true)] [return: MarshalAs(UnmanagedType.U4)] public static extern uint CascGetFileSize(IntPtr hFile, out uint lpdwValue);
    [DllImport(dllName, CharSet = CharSet.Ansi, SetLastError = true)] [return: MarshalAs(UnmanagedType.U1)] public static extern bool CascCloseFile(IntPtr hFile);
    [DllImport(dllName, CharSet = CharSet.Ansi, SetLastError = true)] [return: MarshalAs(UnmanagedType.U1)] public static extern bool CascImportKeysFromFile(IntPtr hStorage, string szFileName);
    [DllImport(dllName, CharSet = CharSet.Ansi, SetLastError = true)] [return: MarshalAs(UnmanagedType.U1)] public static extern bool CascImportKeysFromString(IntPtr hStorage, string szKeyList);
    [DllImport(dllName, CharSet = CharSet.Ansi, SetLastError = true)] public static extern IntPtr CascFindEncryptionKey(IntPtr hStorage, ulong keyName);
    [DllImport(dllName, CharSet = CharSet.Ansi, SetLastError = true)] [return: MarshalAs(UnmanagedType.U1)] public static extern bool CascGetNotFoundEncryptionKey(IntPtr hStorage, out ulong KeyName);
    [DllImport(dllName, CharSet = CharSet.Ansi, SetLastError = true)] [return: MarshalAs(UnmanagedType.U1)] public static extern bool CascAddStringEncryptionKey(IntPtr hStorage, ulong KeyName, string szKeyList);
    [DllImport(dllName, CharSet = CharSet.Ansi, SetLastError = true)] [return: MarshalAs(UnmanagedType.U1)] public static extern bool CascAddEncryptionKey(IntPtr hStorage, ulong KeyName, [In] byte[] Key);
    [DllImport(dllName, CharSet = CharSet.Ansi, SetLastError = true)] [return: MarshalAs(UnmanagedType.U1)] public static extern bool CascFindClose(IntPtr hFind);
    [DllImport(dllName, CharSet = CharSet.Ansi, SetLastError = true)] [return: MarshalAs(UnmanagedType.U1)] public static extern bool CascReadFile(IntPtr hFile, [Out] byte[] lpBuffer, uint dwToRead, out uint pdwRead);
    [DllImport(dllName, CharSet = CharSet.Ansi, SetLastError = true)] public static extern IntPtr CascFindFirstFile(IntPtr hStorage, string szMask, out CASC_FIND_DATA pFindData, string? szListFile);
    [DllImport(dllName, CharSet = CharSet.Ansi, SetLastError = true)] [return: MarshalAs(UnmanagedType.U1)] public static extern bool CascFindNextFile(IntPtr hFile, out CASC_FIND_DATA pFindData);

    public static void Main(string[] args){}
}