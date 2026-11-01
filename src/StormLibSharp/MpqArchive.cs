using System;
using System.Runtime.InteropServices;
using System.IO;
using System.Text;
using StormLibSharpNative;
using System.Buffers;
using System.ComponentModel;
using Boilerplates;
using StormLibSharpModels;

namespace StormLibSharpModernMpq;

public class MpqArchiveModern : IDisposable
{
    private IntPtr _loadedPtr = IntPtr.Zero;
    private bool _disposed = false;
    private readonly uint STREAM_FLAG_READ_ONLY = 0x00000100;
    public MpqArchiveModern (string MPQArchivePath, bool ReadOnly)
    {
        OpenArchive(MPQArchivePath, ReadOnly);
    }
    public IntPtr OpenArchive (string MPQArchivePath, bool ReadOnly)
    {
        NativeMethods.SFileOpenArchive(MPQArchivePath, 0, ReadOnly == true ? STREAM_FLAG_READ_ONLY : 0, out IntPtr MpqH);
        _loadedPtr = MpqH;
        return MpqH;
    }

    public void CloseArchive (IntPtr MpqH)
    {
        Boilerplate.MainBoilerplate(() =>
        {
            NativeMethods.SFileCloseArchive(MpqH);
        });
    }

    public string ReadFile (IntPtr MpqH, string InternalFileName)
    {
        byte[]? buffer = null;
        IntPtr currenthFile = IntPtr.Zero;
        try {
            uint bytesRead;
            if (!NativeMethods.SFileOpenFileEx(MpqH, InternalFileName, 0, out IntPtr hFile))
            {
                return "";
            }
            
            currenthFile = hFile;
            long fileLong = GetFileSize(hFile);

            if (fileLong <= 0)
            {
               return "";
            }
            
            if (fileLong > Array.MaxLength)
            {
                Console.WriteLine($"LOG: File too large error.");
                return "";
            }
            
            var size = (int) fileLong;
            buffer = ArrayPool<byte>.Shared.Rent(size);

            NativeMethods.SFileReadFile(hFile, buffer, (uint) size, out bytesRead, 0);
            return Encoding.UTF8.GetString(buffer, 0, (int) bytesRead);
        } catch (Win32Exception)
        {
            var win32Error = Marshal.GetLastWin32Error();
            Console.WriteLine($"LOG: Win32 error ({win32Error}).");
            return "";
        } catch (Exception ex)
        {
            Console.WriteLine($"LOG: Read error ({ex.Message}).");
            return "";
        } finally 
        {
            if (currenthFile != IntPtr.Zero)
            {
                NativeMethods.SFileCloseFile(currenthFile);
            }

            if (buffer != null)
            {
                ArrayPool<byte>.Shared.Return(buffer);
            }
        }
    }

    public void ReadFileAndWriteToDisk (IntPtr MpqH, string InternalFileName, byte[] buffer, string WriteLocation)
    {
        var WritePath = Path.Combine(WriteLocation, InternalFileName);
        var TextContent = ReadFile(MpqH, InternalFileName);
        File.WriteAllText(WritePath, TextContent);
    }

    public void ReplaceFile (IntPtr mpqH, string internalFileName, string diskFilePath, bool exists)
    {
        if (exists)
        {
            RemoveFile(mpqH, internalFileName);
        }
       NativeMethods.SFileAddFileEx(mpqH, diskFilePath, internalFileName, 0x80000200, 2, 0xFFFFFFFF);
    }

    public long GetFileSize (IntPtr hFile)
    {
        uint high;
        var low = NativeMethods.SFileGetFileSize(hFile, out high);
        return ((long)high << 32) | low;
    }

    public bool RemoveFile (IntPtr mpqH, string internalFileName)
    {
        bool success = false;

        Boilerplate.MainBoilerplate(() =>
        {
            success = NativeMethods.SFileRemoveFile(mpqH, internalFileName, 0);
        });

        return success;
    }

    public void FlushArchive (IntPtr mpqH)
    {
        Boilerplate.MainBoilerplate(() =>
        {
            NativeMethods.SFileFlushArchive(mpqH);
        });
    }

    public void ExtractFile (IntPtr mpqH, string internalFileName, string extractPath)
    {
        Boilerplate.MainBoilerplate(() =>
        {
            NativeMethods.SFileExtractFile(mpqH, internalFileName, extractPath, 0);
        });
    }

    public bool HasFile (IntPtr mpqH, string internalFileName)
    {
        bool exists = false;
        Boilerplate.MainBoilerplate(() =>
        {
            exists = NativeMethods.SFileHasFile(mpqH, internalFileName);
        });

        return exists;
    }

    public uint GetAttributeFlags (IntPtr mpqH)
    {
        uint attributeFlags = 0;
        Boilerplate.MainBoilerplate(() =>
        {
            attributeFlags = NativeMethods.SFileGetAttributes(mpqH);
        });

        return attributeFlags;
    }

    public bool SetAttributeFlags (IntPtr mpqH, uint dwFlags)
    {
        bool success = true;
        Boilerplate.MainBoilerplate(() =>
        {
            success = NativeMethods.SFileSetAttributes(mpqH, dwFlags);
        });

        return success;
    }

    public bool UpdateAttributeFlags (IntPtr mpqH, string szFileName)
    {
        bool success = true;
        Boilerplate.MainBoilerplate(() =>
        {
            success = NativeMethods.SFileUpdateFileAttributes(mpqH, szFileName);
        });

        return success;
    }

    public (bool, uint?) FileGetFileInfo (IntPtr mpqH, _SFileInfoClass infoClass)
    {
        uint bytesNeeded = 0;
        bool success = true;
        var bufferSize = sizeof(uint);
        uint? result = null;

        IntPtr buffer = Marshal.AllocHGlobal(bufferSize);

        try {
            Boilerplate.MainBoilerplate(() =>
            {
                success = NativeMethods.SFileGetFileInfo(mpqH, infoClass, buffer, (uint) bufferSize, out bytesNeeded);
                result = (uint) Marshal.ReadInt32(buffer);
            });
        } finally
        {
            Marshal.FreeHGlobal(buffer);
        }

        return (success, result);
    }

    public IntPtr GetLoadedIntPtr ()
    {
        return _loadedPtr;
    }

    public void Dispose ()
    {
        if (_disposed) {
            return;
        }

        if (_loadedPtr != IntPtr.Zero)
        {
            NativeMethods.SFileCloseArchive(_loadedPtr);
            _loadedPtr = IntPtr.Zero;
        }

        _disposed = true;
    }
}