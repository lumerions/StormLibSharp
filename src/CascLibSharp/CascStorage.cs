using System;
using System.Runtime.InteropServices;
using CascNativeMethods;
using Boilerplates;
using CascModels;

namespace CascStorageHandler;

public class CascStorage : IDisposable
{
    private IntPtr _handleStorage = IntPtr.Zero;
    private bool _disposed = false;
    private List<IntPtr> _openFiles = new();

    public CascStorage (string cascStoragePath)
    {
        OpenCascStorage(cascStoragePath);
    }

    public (bool, IntPtr) OpenCascStorage (string cascStoragePath)
    {
        bool success = false;

        Boilerplate.MainBoilerplate(() =>
        {
            success = NativeMethods.CascOpenStorage(cascStoragePath, 0, out _handleStorage);
        });

        return (success, _handleStorage);
    }

    public (bool, IntPtr) CascOpenFile (string pvFileName, uint dwLocaleFlags, uint dwOpenFlags)
    {
        bool success = false;
        IntPtr hFile = IntPtr.Zero;

        Boilerplate.MainBoilerplate(() =>
        {
            success = NativeMethods.CascOpenFile(_handleStorage, pvFileName, dwLocaleFlags, dwOpenFlags, out hFile);
        });

        _openFiles.Add(hFile);

        return (success, hFile);
    }

    public long CascFetchFileSize (IntPtr hFile)
    {
        if (hFile == IntPtr.Zero) throw new ArgumentException($"hFile is zero. {nameof(hFile)}");

        uint sizeHigh = 0;
        uint sizeLow = 0;

        Boilerplate.MainBoilerplate(() =>
        {
            sizeLow = NativeMethods.CascGetFileSize(hFile, out sizeHigh);
        });

        long fileSize = ((long) sizeHigh << 32) | sizeLow;
        return fileSize;
    }

    public bool CascFileClose (IntPtr hFile)
    {
        if (hFile == IntPtr.Zero) throw new ArgumentException($"hFile is zero. {nameof(hFile)}");

        bool success = false;
        Boilerplate.MainBoilerplate(() =>
        {
            success = NativeMethods.CascCloseFile(hFile);
        });

        return success;
    }

    public IntPtr CurrentIntPtrStorageOpened ()
    {
        return _handleStorage;
    }

    public bool CascKeysImportFromFile (IntPtr hStorage, string szFileName)
    {
        if (hStorage == IntPtr.Zero) throw new ArgumentException($"hStorage is zero. {nameof(hStorage)}");

        bool success = false;
        Boilerplate.MainBoilerplate(() =>
        {
            success = NativeMethods.CascImportKeysFromFile(hStorage, szFileName);
        });

        return success;
    }

    public bool CascKeysImportFromString (IntPtr hStorage, string szKeyList)
    {
        /*
         szKeyList -> pointer to a null-terminated text string containing one or more encryption keys
         "1A2B3C4D5E6F7G8H=0123456789ABCDEF0123456789ABCDEF\n" + "8H7G6F5E4D3C2B1A=FEDCBA9876543210FEDCBA9876543210";
        */
        if (hStorage == IntPtr.Zero) throw new ArgumentException($"hStorage is zero. {nameof(hStorage)}");

        bool success = false;
        Boilerplate.MainBoilerplate(() =>
        {
            success = NativeMethods.CascImportKeysFromString(hStorage, szKeyList);
        });

        return success;
    }

    public byte[] CascGetEncryptionKey (IntPtr hStorage, ulong keyName)
    {
        var key = new byte[16];
        if (hStorage == IntPtr.Zero) throw new ArgumentException($"hStorage is zero. {nameof(hStorage)}");

        Boilerplate.MainBoilerplate(() =>
        {
            IntPtr encryptionKeyPointer = NativeMethods.CascFindEncryptionKey(hStorage, keyName);
            Marshal.Copy(encryptionKeyPointer, key, 0, key.Length);
        });

        return key;
    }

    public (bool, ulong) CascGetUnfoundEncryptionKey (IntPtr hStorage)
    {
        if (hStorage == IntPtr.Zero) throw new ArgumentException($"hStorage is zero. {nameof(hStorage)}");

        bool success = false;
        ulong KeyName = 0;
        
        Boilerplate.MainBoilerplate(() =>
        {
            success = NativeMethods.CascGetNotFoundEncryptionKey(hStorage, out KeyName);
        });

        return (success, KeyName);
    }

    public bool CascAddStringEncryptionKey (IntPtr hStorage, ulong KeyName, string szKeyList)
    {
        if (hStorage == IntPtr.Zero) throw new ArgumentException($"hStorage is zero. {nameof(hStorage)}");

        bool success = false;
        
        Boilerplate.MainBoilerplate(() =>
        {
            success = NativeMethods.CascAddStringEncryptionKey(hStorage, KeyName, szKeyList);
        });

        return success;
    }

    public bool CascAddEncryptionKey (IntPtr hStorage, ulong keyName, byte[] key)
    {
        if (hStorage == IntPtr.Zero) throw new ArgumentException($"hStorage is zero. {nameof(hStorage)}");
        if (key.Length != 16) throw new ArgumentException("hStorage is zero.");

        bool success = false;
        
        Boilerplate.MainBoilerplate(() =>
        {
            success = NativeMethods.CascAddEncryptionKey(hStorage, keyName, key);
        });

        return success;
    }

    public bool CascFindClose (IntPtr hFile)
    {
        if (hFile == IntPtr.Zero) throw new ArgumentException($"hFile is zero. {nameof(hFile)}");

        bool success = false;
        
        Boilerplate.MainBoilerplate(() =>
        {
            success = NativeMethods.CascFindClose(hFile);
        });

        return success;
    }

    public bool CascReadFile (IntPtr hFile, byte[] buffer, uint pdwRead)
    {
        if (hFile == IntPtr.Zero) throw new ArgumentException($"hFile is zero. {nameof(hFile)}");

        bool success = false;
        
        Boilerplate.MainBoilerplate(() =>
        {
            success = NativeMethods.CascReadFile(hFile, buffer, (uint) buffer.Length, out pdwRead);
        });

        return success;
    }

    public (IntPtr, CASC_FIND_DATA) CascFindFirstFile (IntPtr hStorage, string szMask, string? szListFile)
    {
        if (hStorage == IntPtr.Zero) throw new ArgumentException($"hStorage is zero. {nameof(hStorage)}");

        IntPtr hFile = IntPtr.Zero;
        CASC_FIND_DATA cascFindData = new CASC_FIND_DATA();

        Boilerplate.MainBoilerplate(() =>
        {
            hFile = NativeMethods.CascFindFirstFile(hStorage, szMask, out CASC_FIND_DATA cascFindData, szListFile);
        });

        return (hFile, cascFindData);
    }

    public unsafe string ReadFileName (ref CASC_FIND_DATA d)
    {
        fixed (byte* p = d.szFileName)
        {
            int len = 0;
            while (len < (int) Find_DataCasc.CASC_MAX_PATH && p[len] != 0) len++;
            return System.Text.Encoding.ASCII.GetString(p, len).Replace('/', '\\');
        }
    }

    public bool CascFindNextFile (IntPtr hFile)
    {
        if (hFile == IntPtr.Zero) throw new ArgumentException($"hFile is zero. {nameof(hFile)}");
        bool success = false;
        CASC_FIND_DATA cascFindData = new CASC_FIND_DATA();
        
        Boilerplate.MainBoilerplate(() =>
        {
            success = NativeMethods.CascFindNextFile(hFile, out CASC_FIND_DATA cascFindData);
        });

        if (!success)
        {
            cascFindData = new CASC_FIND_DATA(); 
        }

        return success;
    }

    public void Dispose ()
    {
        if (_disposed)
        {
            return;
        }

        if (_handleStorage != IntPtr.Zero)
        {
            Boilerplate.MainBoilerplate(() =>
            {
                NativeMethods.CascCloseStorage(_handleStorage);
            });
            _handleStorage = IntPtr.Zero;
        }

        foreach (var hFile in _openFiles)
        {
            if (hFile == IntPtr.Zero) continue;
            Boilerplate.MainBoilerplate(() =>
            {
                NativeMethods.CascCloseFile(hFile);
            });
        }

        _openFiles.Clear();
        _disposed = true;
    }
}