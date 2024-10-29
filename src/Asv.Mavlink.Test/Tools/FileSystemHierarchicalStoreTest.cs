using System;
using System.IO;
using System.IO.Abstractions.TestingHelpers;
using Asv.Common;
using Xunit;

namespace Asv.Mavlink.Test;

public class FileSystemHierarchicalStoreTest
{
    private void ClearAllDirectories(string storeLocation)
    {
        if (_fileSystem.Directory.Exists(storeLocation))
        {
            _fileSystem.Directory.Delete(storeLocation, true);
        }
    }

    private MockFileSystem _fileSystem;
    public FileSystemHierarchicalStoreTest()
    {
        _fileSystem = new MockFileSystem();
    }
    #region Files

    [Fact]
    public void Check_File_Open_After_Store_Was_Disposed()
    {
        var format = new AsvSdrListDataStoreFormat();
        var storeLocation = "TestFolder_Open_File_After_Disposed #" + _fileSystem.Path.GetRandomFileName();
        var storeinfo = new DirectoryInfo(storeLocation);
        _fileSystem.Directory.CreateDirectory(storeinfo.FullName);
        var store = new FileSystemHierarchicalStore<Guid, IListDataFile<AsvSdrRecordFileMetadata>>(storeinfo.FullName,
            format,
            TimeSpan.FromMilliseconds(0),null,_fileSystem);

        var firstGuid = Guid.NewGuid();
        var file = store.CreateFile(firstGuid, "FirstFile_OpenAfter", store.RootFolderId);
        file.Dispose();

        var fs = _fileSystem.File.Open($"{storeinfo.FullName}\\FirstFile_OpenAfter #{ShortGuid.Encode(firstGuid)}.sdr", FileMode.Open,
            FileAccess.Write);

        store.Dispose();

        Assert.Throws<IOException>(() =>
        {
            _fileSystem.File.Open($"{storeinfo.FullName}\\FirstFile_OpenAfter #{ShortGuid.Encode(firstGuid)}.sdr", FileMode.Open);
        });

        fs.Close();

        ClearAllDirectories(storeinfo.FullName);
    }

    [Fact]
    public void Check_Create_File_With_Same_Id()
    {
        var format = new AsvSdrListDataStoreFormat();
        var storeLocation = "TestFolder_Create_File_With_Same_Id #" + _fileSystem.Path.GetRandomFileName();
        var storeinfo = new DirectoryInfo(storeLocation);
        _fileSystem.Directory.CreateDirectory(storeinfo.FullName);
        var store = new FileSystemHierarchicalStore<Guid, IListDataFile<AsvSdrRecordFileMetadata>>(storeinfo.FullName,
            format,
            TimeSpan.FromMilliseconds(0),null,_fileSystem);

        var firstGuid = Guid.NewGuid();

        var file = store.CreateFile(firstGuid, "FirstCreation", store.RootFolderId);
        file.Dispose();

        Assert.Throws<HierarchicalStoreException>(() =>
            store.CreateFile(firstGuid, "SecondCreation", store.RootFolderId));
        store.Dispose();

        ClearAllDirectories(storeinfo.FullName);
    }

    [Fact]
    public void Check_Success_Create_File()
    {
        var storeLocation = "TestFolder_Success_CreateFile #" +  _fileSystem.Path.GetRandomFileName();
        var format = new AsvSdrListDataStoreFormat();
        var storeinfo = new DirectoryInfo(storeLocation);
        _fileSystem.Directory.CreateDirectory(storeinfo.FullName);
        var store = new FileSystemHierarchicalStore<Guid, IListDataFile<AsvSdrRecordFileMetadata>>(storeinfo.FullName,
            format,
            TimeSpan.FromMilliseconds(0),null,_fileSystem);

        var guid = Guid.NewGuid();

        var file = store.CreateFile(guid, "FirstCreation", store.RootFolderId);
        file.Dispose();

        var files = _fileSystem.Directory.GetFiles(storeinfo.FullName);

        Assert.True(files[0].Equals($"{storeinfo.FullName}\\FirstCreation #{ShortGuid.Encode(guid)}.sdr"));
        store.Dispose();

        ClearAllDirectories(storeinfo.FullName);
    }

    [Fact]
    public void Check_If_File_Exists_After_Manual_Delete()
    {
        var format = new AsvSdrListDataStoreFormat();
        var storeLocation = "TestFolder_Manual_File_Delete #" + _fileSystem.Path.GetRandomFileName();
        var storeinfo = new DirectoryInfo(storeLocation);
        _fileSystem.Directory.CreateDirectory(storeinfo.FullName);
        var store = new FileSystemHierarchicalStore<Guid, IListDataFile<AsvSdrRecordFileMetadata>>(storeinfo.FullName,
            format,
            TimeSpan.FromMilliseconds(0),null,_fileSystem);

        var guid = Guid.NewGuid();

        var file = store.CreateFile(guid, "FirstCreation", store.RootFolderId);
        file.Dispose();
        var files =  _fileSystem.Directory.GetFiles(storeinfo.FullName);

        _fileSystem.File.Delete(files[0]);

        store.UpdateEntries();

        var storeFiles = store.GetFiles();
        Assert.True(storeFiles.Count == 0);
        store.Dispose();

        ClearAllDirectories(storeinfo.FullName);
    }

    [Fact]
    public void Check_GetFiles()
    {
        var format = new AsvSdrListDataStoreFormat();
        var storeLocation = "TestFolder_GetFiles #" + _fileSystem.Path.GetRandomFileName();
        var storeinfo = new DirectoryInfo(storeLocation);
        _fileSystem.Directory.CreateDirectory(storeinfo.FullName);
        var store = new FileSystemHierarchicalStore<Guid, IListDataFile<AsvSdrRecordFileMetadata>>(storeinfo.FullName,
            format,
            TimeSpan.FromMilliseconds(0),null,_fileSystem);
        var guid = Guid.NewGuid();

        var file1 = store.CreateFile(guid, "FirstFile", store.RootFolderId);
        file1.Dispose();
        var file2 = store.CreateFile(Guid.NewGuid(), "SecondFile", store.RootFolderId);
        file2.Dispose();
        var file3 = store.CreateFile(Guid.NewGuid(), "ThirdFileFile", store.RootFolderId);
        file3.Dispose();

        var files = store.GetFiles();

        Assert.True(files.Count == 3);

        store.DeleteFile(guid);
        files = store.GetFiles();

        Assert.True(files.Count == 2);
        store.Dispose();

        ClearAllDirectories(storeinfo.FullName);
    }

    [Fact]
    public void Check_TryGetFile()
    {
        var format = new AsvSdrListDataStoreFormat();
        var storeLocation = "TestFolder_TryGetFile #" +  _fileSystem.Path.GetRandomFileName();
        var storeinfo = new DirectoryInfo(storeLocation);
        _fileSystem.Directory.CreateDirectory(storeinfo.FullName);
        var store = new FileSystemHierarchicalStore<Guid, IListDataFile<AsvSdrRecordFileMetadata>>(storeinfo.FullName,
            format,
            TimeSpan.FromMilliseconds(0),null,_fileSystem);

        var guid1 = Guid.NewGuid();
        var guid2 = Guid.NewGuid();
        var guid3 = Guid.NewGuid();

        var file1 = store.CreateFile(guid1, "FirstFile", store.RootFolderId);
        file1.Dispose();
        var file2 = store.CreateFile(guid2, "SecondFile", store.RootFolderId);
        file2.Dispose();
        var file3 = store.CreateFile(guid3, "ThirdFileFile", store.RootFolderId);
        file3.Dispose();

        Assert.True(store.TryGetFile(guid1, out _));
        Assert.True(store.TryGetFile(guid2, out _));
        Assert.True(store.TryGetFile(guid3, out _));
        store.DeleteFile(guid1);
        Assert.False(store.TryGetFile(guid1, out _));
        store.Dispose();

        ClearAllDirectories(storeinfo.FullName);
    }

    [Fact]
    public void Check_DeleteFile()
    {
        var format = new AsvSdrListDataStoreFormat();
        var storeLocation = "TestFolder_DeleteFile #" + _fileSystem.Path.GetRandomFileName();
        var storeinfo = new DirectoryInfo(storeLocation);
        _fileSystem.Directory.CreateDirectory(storeinfo.FullName);
        var store = new FileSystemHierarchicalStore<Guid, IListDataFile<AsvSdrRecordFileMetadata>>(storeinfo.FullName,
            format,
            TimeSpan.FromMilliseconds(0),null,_fileSystem);
        var guid1 = Guid.NewGuid();
        var guid2 = Guid.NewGuid();
        var folderGuid = Guid.NewGuid();

        var file1 = store.CreateFile(guid1, "FirstFile", store.RootFolderId);
        file1.Dispose();
        var file2 = store.CreateFile(guid2, "SecondFile", store.RootFolderId);

        var file3 = store.CreateFile(Guid.NewGuid(), "ThirdFileFile", store.RootFolderId);
        file3.Dispose();
        var folder1 = store.CreateFolder(folderGuid, "FirstFolder", store.RootFolderId);

        Assert.Throws<HierarchicalStoreException>(() => { store.DeleteFile(folderGuid); });

        Assert.Throws<HierarchicalStoreException>(() => { store.DeleteFile(guid2); });

        file2.Dispose();

        Assert.True(store.FileExists(guid1));
        store.DeleteFile(guid1);
        Assert.False(store.FileExists(guid1));

        store.Dispose();

        ClearAllDirectories(storeinfo.FullName);
    }

    [Fact]
    public void Check_RenameFile()
    {
        var format = new AsvSdrListDataStoreFormat();
        var storeLocation = "TestFolder_RenameFile #" + _fileSystem.Path.GetRandomFileName();
        var storeinfo = new DirectoryInfo(storeLocation);
        _fileSystem.Directory.CreateDirectory(storeinfo.FullName);
        var store = new FileSystemHierarchicalStore<Guid, IListDataFile<AsvSdrRecordFileMetadata>>(storeinfo.FullName,
            format,
            TimeSpan.FromMilliseconds(0),null,_fileSystem);

        var guid1 = Guid.NewGuid();
        var guid2 = Guid.NewGuid();
        var folderGuid = Guid.NewGuid();

        var file1 = store.CreateFile(guid1, "FirstFile", store.RootFolderId);
        file1.Dispose();
        var file2 = store.CreateFile(guid2, "SecondFile", store.RootFolderId);
        var folder1 = store.CreateFolder(folderGuid, "FirstFolder", store.RootFolderId);

        Assert.Throws<HierarchicalStoreException>(() => { store.RenameFile(folderGuid, "NewFolderName"); });

        Assert.Throws<HierarchicalStoreException>(() => { store.RenameFile(guid2, "NewFileName"); });

        file2.Dispose();

        Assert.Equal(guid1, store.RenameFile(guid1, "NewFirstFileName"));

        Assert.True(_fileSystem.File.Exists($"{storeinfo.FullName}\\NewFirstFileName #{ShortGuid.Encode(guid1)}.sdr"));

        store.Dispose();

        ClearAllDirectories(storeinfo.FullName);
    }

    [Fact]
    public void Check_MoveFile()
    {
        var format = new AsvSdrListDataStoreFormat();
        var storeLocation = "TestFolder_MoveFile #" + _fileSystem.Path.GetRandomFileName();
        var storeinfo = new DirectoryInfo(storeLocation);
        _fileSystem.Directory.CreateDirectory(storeinfo.FullName);
        var store = new FileSystemHierarchicalStore<Guid, IListDataFile<AsvSdrRecordFileMetadata>>(storeinfo.FullName,
            format,
            TimeSpan.FromMilliseconds(0),null,_fileSystem);


        var guid1 = Guid.NewGuid();
        var guid2 = Guid.NewGuid();
        var guid3 = Guid.NewGuid();
        var folderGuid = Guid.NewGuid();
        var newFolderGuid = Guid.NewGuid();

        var file1 = store.CreateFile(guid1, "FirstFile", store.RootFolderId);
        file1.Dispose();
        var file2 = store.CreateFile(guid2, "SecondFile", store.RootFolderId);
        file2.Dispose();
        var file3 = store.CreateFile(guid3, "ThirdFile", store.RootFolderId);

        var folder1 = store.CreateFolder(folderGuid, "FirstFolder", store.RootFolderId);
        var folder2 = store.CreateFolder(newFolderGuid, "SecondFolder", store.RootFolderId);

        Assert.Throws<HierarchicalStoreException>(() => { store.MoveFile(folderGuid, newFolderGuid); });

        Assert.Throws<HierarchicalStoreException>(() => { store.MoveFile(guid1, guid2); });

        Assert.Throws<HierarchicalStoreException>(() => { store.MoveFile(guid3, folderGuid); });

        file3.Dispose();

        store.MoveFile(guid1, folderGuid);

        Assert.True(_fileSystem.File.Exists(
            $"{storeinfo.FullName}\\FirstFolder #{ShortGuid.Encode(folderGuid)}\\FirstFile #{ShortGuid.Encode(guid1)}.sdr"));

        store.Dispose();

        ClearAllDirectories(storeinfo.FullName);
    }

    [Fact]
    public async void Check_OpenFile()
    {
        var format = new AsvSdrListDataStoreFormat();
        var storeLocation = "TestFolder_OpenFile #" + _fileSystem.Path.GetRandomFileName();
        var storeinfo = new DirectoryInfo(storeLocation);
        _fileSystem.Directory.CreateDirectory(storeinfo.FullName);
        var store = new FileSystemHierarchicalStore<Guid, IListDataFile<AsvSdrRecordFileMetadata>>(storeinfo.FullName,
            format,
            TimeSpan.FromMilliseconds(0),null,_fileSystem);


        var guid1 = Guid.NewGuid();
        var guid2 = Guid.NewGuid();
        var guid3 = Guid.NewGuid();

        var file1 = store.CreateFile(guid1, "FirstFile", store.RootFolderId);
        file1.Dispose();
        var file2 = store.CreateFile(guid2, "SecondFile", store.RootFolderId);
        file2.Dispose();

        var resultFile = store.OpenFile(guid1);
        resultFile.Dispose();
        Assert.Equal(resultFile.Id, guid1);

        Assert.Throws<FileNotFoundException>(() => { store.OpenFile(guid3); });
        
        _fileSystem.File.Delete($"{storeinfo.FullName}\\SecondFile #{ShortGuid.Encode(guid2)}.sdr");

        Assert.Throws<FileNotFoundException>(() => { store.OpenFile(guid2); });

        store.Dispose();

        ClearAllDirectories(storeinfo.FullName);
    }

    #endregion

    #region Folders

    [Fact]
    public void Check_GetFolders()
    {
        var format = new AsvSdrListDataStoreFormat();
        var storeLocation = "TestFolder_GetFolders #" + _fileSystem.Path.GetRandomFileName();
        var storeinfo = new DirectoryInfo(storeLocation);
        _fileSystem.Directory.CreateDirectory(storeinfo.FullName);
        var store = new FileSystemHierarchicalStore<Guid, IListDataFile<AsvSdrRecordFileMetadata>>(storeinfo.FullName,
            format,
            TimeSpan.FromMilliseconds(0),null,_fileSystem);

        var guid1 = Guid.NewGuid();
        var guid2 = Guid.NewGuid();

        store.CreateFolder(guid1, "FirstFolder", store.RootFolderId);
        store.CreateFolder(guid2, "SecondFolder", store.RootFolderId);

        var folders = store.GetFolders();

        Assert.Equal(2, folders.Count);

        store.DeleteFolder(guid2);
        folders = store.GetFolders();

        Assert.Equal(1, folders.Count);
        store.Dispose();

        ClearAllDirectories(storeinfo.FullName);
    }

    [Fact]
    public void Check_Manual_Delete_GetFolders()
    {
        var format = new AsvSdrListDataStoreFormat();
        var storeLocation = "TestFolder_Folder_Manual_Delete #" + _fileSystem.Path.GetRandomFileName();
        var storeinfo = new DirectoryInfo(storeLocation);
        _fileSystem.Directory.CreateDirectory(storeinfo.FullName);
        var store = new FileSystemHierarchicalStore<Guid, IListDataFile<AsvSdrRecordFileMetadata>>(storeinfo.FullName,
            format,
            TimeSpan.FromMilliseconds(0),null,_fileSystem);

        var guid1 = Guid.NewGuid();
        var guid2 = Guid.NewGuid();

        store.CreateFolder(guid1, "FirstFolder", store.RootFolderId);
        store.CreateFolder(guid2, "SecondFolder", store.RootFolderId);

        var folders = store.GetFolders();

        Assert.Equal(2, folders.Count);

        _fileSystem.Directory.Delete($"{storeinfo.FullName}\\SecondFolder #{ShortGuid.Encode(guid2)}");

        store.UpdateEntries();

        folders = store.GetFolders();

        Assert.Equal(1, folders.Count);
        store.Dispose();

        ClearAllDirectories(storeinfo.FullName);
    }

    [Fact]
    public void Check_CreateFolder()
    {
        var format = new AsvSdrListDataStoreFormat();
        var storeLocation = "TestFolder_CreateFolder #" + _fileSystem.Path.GetRandomFileName();
        var storeinfo = new DirectoryInfo(storeLocation);
        _fileSystem.Directory.CreateDirectory(storeinfo.FullName);
        var store = new FileSystemHierarchicalStore<Guid, IListDataFile<AsvSdrRecordFileMetadata>>(storeinfo.FullName,
            format,
            TimeSpan.FromMilliseconds(0),null,_fileSystem);

        var guid1 = Guid.NewGuid();
        var guid2 = Guid.NewGuid();

        var folder1 = store.CreateFolder(guid1, "FirstFolder", store.RootFolderId);
        var folder2 = store.CreateFolder(guid2, "SecondFolder", store.RootFolderId);

        Assert.Throws<HierarchicalStoreFolderAlreadyExistException>(() =>
        {
            store.CreateFolder(guid1, "ThirdFolder", store.RootFolderId);
        });

        Assert.Throws<HierarchicalStoreFolderAlreadyExistException>(() =>
        {
            store.CreateFolder(Guid.NewGuid(), "FirstFolder", store.RootFolderId);
        });

        Assert.Equal(guid1, folder1);
        Assert.Equal(guid2, folder2);

        var guid3 = Guid.NewGuid();
        var guid4 = Guid.NewGuid();

        Assert.Throws<HierarchicalStoreException>(() => { store.CreateFolder(guid3, "ThirdFolder", guid4); });
        store.Dispose();

        ClearAllDirectories(storeinfo.FullName);
    }

    [Fact]
    public void Check_DeleteFolder()
    {
        var format = new AsvSdrListDataStoreFormat();
        var storeLocation = "TestFolder_DeleteFolder #" + _fileSystem.Path.GetRandomFileName();
        var storeinfo = new DirectoryInfo(storeLocation);
        _fileSystem.Directory.CreateDirectory(storeinfo.FullName);
        var store = new FileSystemHierarchicalStore<Guid, IListDataFile<AsvSdrRecordFileMetadata>>(storeinfo.FullName,
            format,
            TimeSpan.FromMilliseconds(0),null,_fileSystem);

        var guid1 = Guid.NewGuid();
        var guid2 = Guid.NewGuid();
        var guid3 = Guid.NewGuid();
        var fileGuid1 = Guid.NewGuid();
        var fileGuid2 = Guid.NewGuid();

        var folder1 = store.CreateFolder(guid1, "FirstFolder", store.RootFolderId);
        var folder2 = store.CreateFolder(guid2, "SecondFolder", store.RootFolderId);
        var file1 = store.CreateFile(fileGuid1, "FirstFile_DeleteFolder", store.RootFolderId);
        file1.Dispose();
        var file2 = store.CreateFile(fileGuid2, "SecondFile", folder1);

        Assert.Throws<HierarchicalStoreException>(() => { store.DeleteFolder(guid3); });

        Assert.Throws<HierarchicalStoreException>(() => { store.DeleteFolder(fileGuid1); });

        Assert.Throws<HierarchicalStoreException>(() => { store.DeleteFolder(guid1); });

        file2.Dispose();

        var files = _fileSystem.Directory.GetDirectories(storeinfo.FullName);
        Assert.Equal(2, files.Length);

        store.DeleteFolder(guid2);
        files = _fileSystem.Directory.GetDirectories(storeinfo.FullName);
        Assert.Single(files);

        store.Dispose();

        ClearAllDirectories(storeinfo.FullName);
    }

    [Fact]
    public void Check_FolderExists()
    {
        var format = new AsvSdrListDataStoreFormat();
        var storeLocation = "TestFolder_FolderExists #" + _fileSystem.Path.GetRandomFileName();
        var storeinfo = new DirectoryInfo(storeLocation);
        _fileSystem.Directory.CreateDirectory(storeinfo.FullName);
        var store = new FileSystemHierarchicalStore<Guid, IListDataFile<AsvSdrRecordFileMetadata>>(storeinfo.FullName,
            format,
            TimeSpan.FromMilliseconds(0),null,_fileSystem);

        var guid1 = Guid.NewGuid();
        var guid2 = Guid.NewGuid();

        var folder1 = store.CreateFolder(guid1, "FirstFolder", store.RootFolderId);
        var folder2 = store.CreateFolder(guid2, "SecondFolder", store.RootFolderId);

        Assert.True(store.FolderExists(guid1));

        _fileSystem.Directory.Delete($"{storeinfo.FullName}\\SecondFolder #{ShortGuid.Encode(guid2)}");

        store.UpdateEntries();

        Assert.False(store.FolderExists(guid2));
        store.Dispose();

        ClearAllDirectories(storeinfo.FullName);
    }

    [Fact]
    public void Check_RenameFolder()
    {
        var format = new AsvSdrListDataStoreFormat();
        var storeLocation = "TestFolder_RenameFolder #" + _fileSystem.Path.GetRandomFileName();
        var storeinfo = new DirectoryInfo(storeLocation);
        _fileSystem.Directory.CreateDirectory(storeinfo.FullName);
        var store = new FileSystemHierarchicalStore<Guid, IListDataFile<AsvSdrRecordFileMetadata>>(storeinfo.FullName,
            format,
            TimeSpan.FromMilliseconds(0),null,_fileSystem);

        var guid1 = Guid.NewGuid();
        var guid2 = Guid.NewGuid();
        var guid3 = Guid.NewGuid();

        var folder1 = store.CreateFolder(guid1, "FirstFolder", store.RootFolderId);
        var folder2 = store.CreateFolder(guid2, "SecondFolder", store.RootFolderId);

        Assert.Throws<HierarchicalStoreException>(() => { store.RenameFolder(guid3, "ThirdFolder"); });

        var file1 = store.CreateFile(guid3, "FirstFile_RenameFolder", store.RootFolderId);
        file1.Dispose();

        Assert.Throws<HierarchicalStoreException>(() => { store.RenameFolder(guid3, "ThirdFolder"); });

        store.DeleteFile(guid3);

        var file2 = store.CreateFile(guid3, "SecondFile", folder1);

        Assert.Throws<HierarchicalStoreException>(() => { store.RenameFolder(folder1, "NewFirstFolder"); });

        file2.Dispose();

        Assert.True(_fileSystem.Directory.Exists($"{storeinfo.FullName}\\SecondFolder #{ShortGuid.Encode(folder2)}"));

        store.RenameFolder(folder2, "NewSecondFolder");

        Assert.True(_fileSystem.Directory.Exists($"{storeinfo.FullName}\\NewSecondFolder #{ShortGuid.Encode(folder2)}"));

        store.Dispose();

        ClearAllDirectories(storeinfo.FullName);
    }

    [Fact]
    public void Check_MoveFolder()
    {
        var format = new AsvSdrListDataStoreFormat();
        var storeLocation = "TestFolder_MoveFolder #" + _fileSystem.Path.GetRandomFileName();
        var storeinfo = new DirectoryInfo(storeLocation);
        _fileSystem.Directory.CreateDirectory(storeinfo.FullName);
        var store = new FileSystemHierarchicalStore<Guid, IListDataFile<AsvSdrRecordFileMetadata>>(storeinfo.FullName,
            format,
            TimeSpan.FromMilliseconds(0),null,_fileSystem);

        var guid1 = Guid.NewGuid();
        var guid2 = Guid.NewGuid();
        var guid3 = Guid.NewGuid();

        var folder1 = store.CreateFolder(guid1, "FirstFolder", store.RootFolderId);
        var folder2 = store.CreateFolder(guid2, "SecondFolder", store.RootFolderId);

        Assert.Throws<HierarchicalStoreException>(() => { store.MoveFolder(guid3, folder1); });

        Assert.Throws<HierarchicalStoreException>(() => { store.MoveFolder(folder1, guid3); });

        var file1 = store.CreateFile(guid3, "FirstFile_MoveFolder", store.RootFolderId);
        file1.Dispose();

        Assert.Throws<HierarchicalStoreException>(() => { store.MoveFolder(guid3, folder1); });

        Assert.Throws<HierarchicalStoreException>(() => { store.MoveFolder(folder1, guid3); });

        Assert.True(_fileSystem.Directory.Exists($"{storeinfo.FullName}\\SecondFolder #{ShortGuid.Encode(folder2)}"));

        store.MoveFolder(folder2, folder1);

        Assert.True(_fileSystem.Directory.Exists(
            $"{storeinfo.FullName}\\FirstFolder #{ShortGuid.Encode(folder1)}\\SecondFolder #{ShortGuid.Encode(folder2)}"));
        store.Dispose();

        ClearAllDirectories(storeinfo.FullName);
    }

    #endregion

    #region Entries

    [Fact]
    public void Check_GetEntries()
    {
        var format = new AsvSdrListDataStoreFormat();
        var storeLocation = "TestFolder_GetEntries #" + _fileSystem.Path.GetRandomFileName();
        var storeinfo = new DirectoryInfo(storeLocation);
        _fileSystem.Directory.CreateDirectory(storeinfo.FullName);
        var store = new FileSystemHierarchicalStore<Guid, IListDataFile<AsvSdrRecordFileMetadata>>(storeinfo.FullName,
            format,
            TimeSpan.FromMilliseconds(0),null,_fileSystem);

        var fileGuid1 = Guid.NewGuid();
        var fileGuid2 = Guid.NewGuid();
        var folderGuid1 = Guid.NewGuid();
        var folderGuid2 = Guid.NewGuid();

        var file1 = store.CreateFile(fileGuid1, "FirstFile_GetEntries", store.RootFolderId);
        file1.Dispose();
        var file2 = store.CreateFile(fileGuid2, "SecondFile", store.RootFolderId);
        file2.Dispose();

        store.CreateFolder(folderGuid1, "FirstFolder", store.RootFolderId);
        store.CreateFolder(folderGuid2, "SecondFolder", store.RootFolderId);

        var entries = store.GetEntries();

        Assert.Equal(4, entries.Count);

        store.DeleteFile(fileGuid1);
        entries = store.GetEntries();

        Assert.Equal(3, entries.Count);

        store.MoveFile(fileGuid2, folderGuid1);
        entries = store.GetEntries();

        Assert.Equal(3, entries.Count);

        store.DeleteFile(fileGuid2);
        entries = store.GetEntries();

        Assert.Equal(2, entries.Count);

        store.DeleteFolder(folderGuid1);
        entries = store.GetEntries();

        Assert.Equal(1, entries.Count);

        store.Dispose();

        ClearAllDirectories(storeinfo.FullName);
    }

    [Fact]
    public void Check_TryGetEntry()
    {
        var format = new AsvSdrListDataStoreFormat();
        var storeLocation = "TestFolder_TryGetEntry #" + _fileSystem.Path.GetRandomFileName();
        var storeinfo = new DirectoryInfo(storeLocation);
        _fileSystem.Directory.CreateDirectory(storeinfo.FullName);
        var store = new FileSystemHierarchicalStore<Guid, IListDataFile<AsvSdrRecordFileMetadata>>(storeinfo.FullName,
            format,
            TimeSpan.FromMilliseconds(0),null,_fileSystem);

        var fileGuid1 = Guid.NewGuid();
        var fileGuid2 = Guid.NewGuid();
        var guid3 = Guid.NewGuid();
        var folderGuid1 = Guid.NewGuid();
        var folderGuid2 = Guid.NewGuid();

        var file1 = store.CreateFile(fileGuid1, "FirstFile_TryGetEntry", store.RootFolderId);
        file1.Dispose();
        var file2 = store.CreateFile(fileGuid2, "SecondFile", store.RootFolderId);
        file2.Dispose();

        store.CreateFolder(folderGuid1, "FirstFolder", store.RootFolderId);
        store.CreateFolder(folderGuid2, "SecondFolder", store.RootFolderId);

        Assert.True(store.TryGetEntry(fileGuid1, out _));
        Assert.True(store.TryGetEntry(fileGuid2, out _));
        Assert.True(store.TryGetEntry(folderGuid1, out _));
        Assert.True(store.TryGetEntry(folderGuid2, out _));
        Assert.False(store.TryGetEntry(guid3, out _));

        store.DeleteFile(fileGuid1);

        Assert.False(store.TryGetEntry(fileGuid1, out _));

        store.DeleteFolder(folderGuid1);

        Assert.False(store.TryGetEntry(folderGuid1, out _));

        store.Dispose();

        ClearAllDirectories(storeinfo.FullName);
    }

    [Fact]
    public void Check_EntryExists()
    {
        var format = new AsvSdrListDataStoreFormat();
        var storeLocation = "TestFolder_EntryExists #" + _fileSystem.Path.GetRandomFileName();
        var storeinfo = new DirectoryInfo(storeLocation);
        _fileSystem.Directory.CreateDirectory(storeinfo.FullName);
        var store = new FileSystemHierarchicalStore<Guid, IListDataFile<AsvSdrRecordFileMetadata>>(storeinfo.FullName,
            format,
            TimeSpan.FromMilliseconds(0),null,_fileSystem);

        var fileGuid1 = Guid.NewGuid();
        var fileGuid2 = Guid.NewGuid();
        var guid3 = Guid.NewGuid();
        var folderGuid1 = Guid.NewGuid();
        var folderGuid2 = Guid.NewGuid();

        var file1 = store.CreateFile(fileGuid1, "FirstFile_EntryExists", store.RootFolderId);
        file1.Dispose();
        var file2 = store.CreateFile(fileGuid2, "SecondFile", store.RootFolderId);
        file2.Dispose();

        store.CreateFolder(folderGuid1, "FirstFolder", store.RootFolderId);
        store.CreateFolder(folderGuid2, "SecondFolder", store.RootFolderId);

        Assert.True(store.EntryExists(fileGuid1));
        Assert.True(store.EntryExists(fileGuid2));
        Assert.False(store.EntryExists(guid3));

        store.DeleteFile(fileGuid1);

        Assert.False(store.EntryExists(fileGuid1));

        Assert.True(store.EntryExists(folderGuid1));
        Assert.True(store.EntryExists(folderGuid2));

        store.DeleteFolder(folderGuid1);

        Assert.False(store.EntryExists(folderGuid1));

        store.Dispose();

        ClearAllDirectories(storeinfo.FullName);
    }

    #endregion
}