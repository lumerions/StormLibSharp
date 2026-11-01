using System;
using System.Text;
using CascStorageHandler;
using CascModels;
using StormLibSharpModernMpq;
using StormLibSharpModels;

public class Program
{
    public static void Main(string[] args)
    {
        using (CascStorage cascStorage = new CascStorage(@"C:\Program Files (x86)\StarCraft II"))
        {
            var (success, hFile) = cascStorage.CascOpenFile(@"campaigns\voidstory.sc2campaign\base.sc2data\ui\layout\custom\lotv_debug.sc2layout", 0, (uint) FileOpenFlags.CASC_OPEN_BY_NAME);
            var byteSize = cascStorage.CascFetchFileSize(hFile);
            var buffer = new byte[byteSize];
            var readSuccess = cascStorage.CascReadFile(hFile, buffer, 0);
            Console.WriteLine(readSuccess);
            Console.WriteLine(Encoding.UTF8.GetString(buffer));
            //IntPtr ptr = cascStorage.CascFindFirstFile(cascStorage.CurrentIntPtrStorageOpened(), "*", null); // Your own list file internal file location may need to be provided, if the third argument is null it will find its own list file
            //Console.WriteLine(ptr);
        }

        using  (MpqArchiveModern mpqArchive = new MpqArchiveModern(@"C:\Users\asdfg\OneDrive\Desktop\New folder (2)\blankback.SC2Map", false))
        {
            var (fileInfoSuccess, fileinfoResult) = mpqArchive.FileGetFileInfo(mpqArchive.GetLoadedIntPtr(), _SFileInfoClass.SFileMpqNumberOfFiles); // still wip
            Console.WriteLine(fileInfoSuccess);
            Console.WriteLine(fileinfoResult);
        }
    }
}