using System.Runtime.InteropServices;

namespace CascModels;

[Flags]
public enum FileOpenFlags : uint
{
    CASC_OPEN_BY_NAME = 0x0000000, // Open by internal file name in an cascArchive
    CASC_OPEN_BY_CKEY = 0x0000001,
    CASC_OPEN_BY_EKEY = 0x0000002,
    CASC_OPEN_BY_FILEID = 0x0000003, // Open by file id
    CASC_STRICT_DATA_CHECK = 0x00000010, // Makes data transfers more secure, but it is slower
    CASC_OVERCOME_ENCRYPTED = 0x00000020, // Skips encrypted blocks if key unknown
}

[Flags]
public enum Find_DataCasc : int
{
    CASC_MAX_PATH = 260,
    MD5_HASH_SIZE = 16,
}

[StructLayout(LayoutKind.Explicit, Size = 344)]
public unsafe struct CASC_FIND_DATA
{
    /// <summary>Full virtual path of the file (e.g. "data:data\global\..."), NUL-terminated ASCII.</summary>
    [FieldOffset(0)]
    public fixed byte szFileName[(int) Find_DataCasc.CASC_MAX_PATH];

    /// <summary>Content key — the MD5 of the file's decoded content.</summary>
    [FieldOffset(260)]
    public fixed byte CKey[(int) Find_DataCasc.MD5_HASH_SIZE];

    /// <summary>Encoding key — identifies the stored (BLTE-encoded) blob.</summary>
    [FieldOffset(276)]
    public fixed byte EKey[(int) Find_DataCasc.MD5_HASH_SIZE];

    [FieldOffset(296)]
    public ulong TagBitMask;

    [FieldOffset(304)]
    public ulong FileSize;

    /// <summary>Pointer into szFileName at the start of the plain file name.</summary>
    [FieldOffset(312)]
    public IntPtr szPlainName;

    [FieldOffset(320)]
    public uint dwFileDataId;

    [FieldOffset(324)]
    public uint dwLocaleFlags;

    [FieldOffset(328)]
    public uint dwContentFlags;

    [FieldOffset(332)]
    public uint dwSpanCount;

    /// <summary>Non-zero when the file is locally available in the CASC storage.</summary>
    [FieldOffset(336)]
    public uint bFileAvailable; // DWORD bit-field in native code — check != 0

    [FieldOffset(340)]
    public uint NameType;
}