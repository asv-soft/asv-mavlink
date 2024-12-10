using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Asv.IO;
using Asv.Mavlink.AsvSdr;
using DeepEqual.Syntax;
using Moq;
using Xunit;

namespace Asv.Mavlink.Test;

public class AsvSdrStoreTests
{

    [Fact]
    public void Format_CreateFile_Success()
    {
        //Arrange
        Stream stream = new MemoryStream();
        const string fileName = "testFIle";
        var guid = Guid.NewGuid();
        var format = new AsvSdrListDataStoreFormat();
        //Act
        var file = format.CreateFile(stream, guid, fileName);
        //Assert
        Assert.NotNull(file);
        var fileNameString = new string(file.ReadMetadata().Info.RecordName.Where(c => c != 0).ToArray());
        Assert.Equal(fileNameString, fileName);
        var fileGuid = new Guid(file.ReadMetadata().Info.RecordGuid);
        Assert.Equal(fileGuid, guid);
    }

    [Fact]
    public void Format_OpenFile_Success()
    {
        //Arrange
        Stream stream = new MemoryStream();
        const string fileName = "testFIle";
        var guid = Guid.NewGuid();
        var format = new AsvSdrListDataStoreFormat();
        //Act
        var file = format.CreateFile(stream, guid, fileName);
        var file2 = format.OpenFile(stream);
        //Assert
        Assert.True(file.IsDeepEqual(file2));
    }

    [Fact]
    public void MetaData_TryEditWithGoodValues_Success()
    {
        //Arrange
        Stream stream = new MemoryStream();
        const string fileName = "testFIle";
        var guid = Guid.NewGuid();
        var format = new AsvSdrListDataStoreFormat();
        var file = format.CreateFile(stream, guid, fileName);
        var metadata = file.ReadMetadata();


        var serial = new Span<byte>();
        metadata.Serialize(ref serial);
        ReadOnlySpan<byte> span = serial;
        metadata.Deserialize(ref span);
        var size = metadata.GetByteSize();
        Assert.Equal(size, span.Length);
    }


    [Fact]
    public void Helper_WriteReadRecordInfo_Success()
    {
        //Arrange
        var payload = new AsvSdrRecordPayload()
        {
            RecordName = ['n', 'a', 'm', 'e'],
            DataType = AsvSdrCustomMode.AsvSdrCustomModeLlz
        };
        Stream stream = new MemoryStream();
        const string fileName = "testFIle";
        var guid = Guid.NewGuid();
        var format = new AsvSdrListDataStoreFormat();
        var file = format.CreateFile(stream, guid, fileName);
        AsvSdrRecordPayload resultPayload = new();
        //Act
        file.WriteRecordInfo(payload);
        file.ReadRecordInfo(resultPayload);
        //Assert
        Assert.True(payload.IsDeepEqual(resultPayload));
    }

    [Fact]
    public void Helper_ReadRecordInfoCopyMetadataInfoToDestination_Success()
    {
        // Arrange
        var mockFile = new Mock<IListDataFile<AsvSdrRecordFileMetadata>>();
        var tag = new AsvSdrRecordTagPayload();
        var metadata = new AsvSdrRecordFileMetadata();
        metadata.Tags.Add(tag);

        mockFile.Setup(x => x.ReadMetadata()).Returns(metadata);

        var dest = new AsvSdrRecordPayload();

        // Act
        mockFile.Object.ReadRecordInfo(dest);

        // Assert
        Assert.True(metadata.Info.IsDeepEqual(dest));
    }

    [Fact]
    public void Helper_WriteRecordInfoShouldCopySourceInfoToMetadata_Success()
    {
        // Arrange
        var mockFile = new Mock<IListDataFile<AsvSdrRecordFileMetadata>>();
        var src = new AsvSdrRecordPayload();

        mockFile.Setup(x => x.EditMetadata(It.IsAny<Action<AsvSdrRecordFileMetadata>>()))
            .Callback<Action<AsvSdrRecordFileMetadata>>(action =>
            {
                var metadata = new AsvSdrRecordFileMetadata();
                action(metadata);
                Assert.True(src.IsDeepEqual( metadata.Info)); 
            });

        // Act
        mockFile.Object.WriteRecordInfo(src);

        // Assert
        mockFile.Verify(x => x.EditMetadata(It.IsAny<Action<AsvSdrRecordFileMetadata>>()), Times.Once);
    }

    [Fact]
    public void Helper_ReadTagShouldReturnTrueWhenTagExists_Success()
    {
        // Arrange
        var mockFile = new Mock<IListDataFile<AsvSdrRecordFileMetadata>>();
        var tag = new AsvSdrRecordTagPayload();
        var tagId = new Guid(tag.TagGuid); 
        var metadata = new AsvSdrRecordFileMetadata();
        metadata.Tags.Add(tag);

        mockFile.Setup(x => x.ReadMetadata()).Returns(metadata);

        var dest = new AsvSdrRecordTagPayload();

        // Act
        var result = mockFile.Object.ReadTag(tagId, dest);

        // Assert
        Assert.True(result);
        Assert.True(tag.IsDeepEqual(dest));
    }

    [Fact]
    public void Helper_ReadTagReturnFalseWhenTagDoesNotExist_Fail()
    {
        // Arrange
        var mockFile = new Mock<IListDataFile<AsvSdrRecordFileMetadata>>();
        var tagId = Guid.NewGuid();
        var metadata = new AsvSdrRecordFileMetadata();

        mockFile.Setup(x => x.ReadMetadata()).Returns(metadata);

        var dest = new AsvSdrRecordTagPayload();

        // Act
        var result = mockFile.Object.ReadTag(tagId, dest);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void Helper_WriteTagAddNewTagWhenTagDoesNotExist_Success()
    {
        // Arrange
        var mockFile = new Mock<IListDataFile<AsvSdrRecordFileMetadata>>();
        var tag = new AsvSdrRecordTagPayload();
        var tagId = new Guid(tag.TagGuid); 
        
        var metadata = new AsvSdrRecordFileMetadata();
        metadata.Tags.Add(tag);
        var tagPayload = new AsvSdrRecordTagPayload();

        mockFile.Setup(x => x.EditMetadata(It.IsAny<Action<AsvSdrRecordFileMetadata>>()))
            .Callback<Action<AsvSdrRecordFileMetadata>>(action =>
            {
                var metadata = new AsvSdrRecordFileMetadata();
                
                action(metadata);

                // Assert
                Assert.Single(metadata.Tags);
                Assert.True(tagPayload.IsDeepEqual( metadata.Tags.First()));
            });

        // Act
        mockFile.Object.WriteTag(tagId, tagPayload);

        // Assert
        mockFile.Verify(x => x.EditMetadata(It.IsAny<Action<AsvSdrRecordFileMetadata>>()), Times.Once);
    }

    [Fact]
    public void Helper_DeleteTagShouldRemoveTagWhenTagExists_Success()
    {
        // Arrange
        var mockFile = new Mock<IListDataFile<AsvSdrRecordFileMetadata>>();
        var tag = new AsvSdrRecordTagPayload();
        var tagId = new Guid(tag.TagGuid); 
        
        var metadata = new AsvSdrRecordFileMetadata();
        metadata.Tags.Add(tag);
        mockFile.Setup(x => x.EditMetadata(It.IsAny<Action<AsvSdrRecordFileMetadata>>()))
            .Callback<Action<AsvSdrRecordFileMetadata>>(action =>
            {
                action(metadata);

                // Assert 
                Assert.Empty(metadata.Tags);
            });

        // Act
        var result = mockFile.Object.DeleteTag(tagId);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Helper_DeleteTagReturnFalseWhenTagDoesNotExist_Success()
    {
        // Arrange
        var mockFile = new Mock<IListDataFile<AsvSdrRecordFileMetadata>>();
        var tagId = Guid.NewGuid();
        var metadata = new AsvSdrRecordFileMetadata();

        mockFile.Setup(x => x.EditMetadata(It.IsAny<Action<AsvSdrRecordFileMetadata>>()));

        // Act
        var result = mockFile.Object.DeleteTag(tagId);

        // Assert
        Assert.False(result);
    }
}