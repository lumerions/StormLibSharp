[Flags]
public enum DwSearchScope : uint
{
    SFILE_OPEN_FROM_MPQ = 0x00000000, // Default, searchs for files in the MPQ archive
    SFILE_OPEN_LOCAL_FILE = 0xFFFFFFFF // Looks on disk instead of the MPQ archive
}