#nullable enable
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using Asv.Common;

namespace Asv.Mavlink;


public class Sync
{
    public byte[] SyncMagic = new byte[] { 0x0FF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF };
    public byte[] FullToken = new byte[] { 0, 7, (byte)'S', 0x0FA, 0xFF, 0xFF, 0, 7, (byte)'S', 0xFF,0xFF };
}



public static class GuidHierarchicalStoreFormat
{
    class Comp : IEqualityComparer<Guid>
    {
        public bool Equals(Guid x, Guid y)
        {
            return x == y;
        }
        public int GetHashCode(Guid obj)
        {
            return obj.GetHashCode();
        }
    }
    
    public const string DisplayNameOfFolderRegexString = @"[A-Za-z][A-Za-z0-9_\- +]{0,28}";
    public static readonly Regex DisplayNameOfFolderRegex = new($"^{DisplayNameOfFolderRegexString}$", RegexOptions.Compiled);
    
    public const string DisplayNameOfFileRegexString = @"[A-Za-z][A-Za-z0-9_\- +]{0,28}";
    public static readonly Regex DisplayNameOfFileRegex = new($"^{DisplayNameOfFileRegexString}$", RegexOptions.Compiled);
    
    public const string GuidOfFolderOrFileRegexString = @"[0-9a-zA-Z_-]{22}";
    public static readonly Regex FileSystemNameOfFolderRegex = new($"^({DisplayNameOfFolderRegexString}) #({GuidOfFolderOrFileRegexString})$", RegexOptions.Compiled);
    public static readonly Regex FileSystemNameOfFileRegex = new($"^({DisplayNameOfFileRegexString}) #({GuidOfFolderOrFileRegexString})$", RegexOptions.Compiled);
    
    public static IEqualityComparer<Guid> GuidComparer { get; } = new Comp();
}

public abstract class GuidHierarchicalStoreFormat<TFile> : IHierarchicalStoreFormat<Guid, TFile> where TFile : IDisposable
{
    private readonly string _defaultFileExt;
    
    

    public GuidHierarchicalStoreFormat(string defaultFileExt)
    {
        _defaultFileExt = defaultFileExt;
    }
    
    public Guid RootFolderId => Guid.Empty;
    public IEqualityComparer<Guid> KeyComparer => GuidHierarchicalStoreFormat.GuidComparer;
    public bool TryGetFolderInfo(DirectoryInfo folderInfo, out Guid id, out string? displayName)
    {
        var matches = GuidHierarchicalStoreFormat.FileSystemNameOfFolderRegex.Match(folderInfo.Name);
        if (matches.Groups.Count !=3)
        {
            id = default;
            displayName = default;
            return false;
        }
        var nameMatch = matches.Groups[1];
        var idMatch = matches.Groups[2];
        if (ShortGuid.TryParse(idMatch.Value, out id) == false)
        {
            displayName = null; 
            return false;
        }
        displayName = nameMatch.Value;
        return true;
    }
    public string GetFileSystemFileName(Guid id, string displayName)
    {
        return $"{displayName} #{ShortGuid.Encode(id)}{_defaultFileExt}";
    }
    public string GetFileSystemFolderName(Guid id, string displayName)
    {
        return $"{displayName} #{ShortGuid.Encode(id)}";
    }

    public bool TryGetFileInfo(FileInfo fileInfo, out Guid id, out string displayName)
    {
        var ext = Path.GetExtension(fileInfo.Name);
        if (ext != _defaultFileExt)
        {
            id = default;
            displayName = null;
            return false;
        }

        var name = Path.GetFileNameWithoutExtension(fileInfo.Name);
        var matches = GuidHierarchicalStoreFormat.FileSystemNameOfFileRegex.Match(name);
        if (matches.Groups.Count != 3)
        {
            id = default;
            displayName = default;
            return false;
        }
        var nameMatch = matches.Groups[1];
        var idMatch = matches.Groups[2];
        if (ShortGuid.TryParse(idMatch.Value, out id) == false)
        {
            displayName = null; 
            return false;
        }
        displayName = nameMatch.Value;
        return GuidHierarchicalStoreFormat.DisplayNameOfFileRegex.IsMatch(displayName);
    }

    
    public abstract TFile OpenFile(Stream stream);
    public abstract TFile CreateFile(Stream stream, Guid id, string name);
    public void CheckFolderDisplayName(string name)
    {
        if (GuidHierarchicalStoreFormat.DisplayNameOfFolderRegex.IsMatch(name) == false)
        {
            throw new ArgumentException($"Folder name '{name}' is invalid. Must match '{GuidHierarchicalStoreFormat.DisplayNameOfFolderRegexString}'");
        }
    }

    public abstract void Dispose();

}