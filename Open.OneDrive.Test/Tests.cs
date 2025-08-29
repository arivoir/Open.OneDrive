using DotNetEnv;
using Open.IO;
using System.Text;

namespace Open.OneDrive.Test
{
    public class Tests
    {
        private string _accessToken;
        private string _rootFolderId;

        [SetUp]
        public async Task Setup()
        {
            Env.Load();
            _accessToken = Environment.GetEnvironmentVariable("ACCESS_TOKEN")!;
            var client = new OneDriveClient(_accessToken);
            var rootFolderName = Guid.NewGuid().ToString();
            _rootFolderId = OneDriveClient.GetPath(rootFolderName);
            await client.CreateFolderAsync(OneDriveClient.GetPath(""), rootFolderName, "", true);
        }

        [TearDown]
        public async Task TearDown()
        {
            var client = new OneDriveClient(_accessToken);
            await client.DeleteItemAsync(_rootFolderId);
        }

        [Test]
        public async Task UploadAndDownloadFileTest()
        {
            var client = new OneDriveClient(_accessToken);
            var file = await client.UploadFileAsync(_rootFolderId, "file.txt", new MemoryStream(Encoding.UTF8.GetBytes("Hello, World!")), true, new Progress<StreamProgress>(p => { }));
            var fileStream = await client.DownloadFileAsync($"{_rootFolderId}/file.txt", CancellationToken.None);
            using var reader = new StreamReader(fileStream);
            var fileContent = reader.ReadToEnd();

            Assert.That(file.Name, Is.EqualTo("file.txt"));
            Assert.That(fileContent, Is.EqualTo("Hello, World!"));
        }

        [Test]
        public async Task UploadAndDownloadLargeFileTest()
        {
            var stringToUpload = "Hello, World!";
            var client = new OneDriveClient(_accessToken);
            var uploadSession = await client.CreateUploadSession(_rootFolderId, "largeFile.txt", "", true);
            var (session1, file1) = await client.UploadBytesToSession(uploadSession.UploadUrl, 0, stringToUpload.Length, new MemoryStream(Encoding.UTF8.GetBytes(stringToUpload.AsSpan().Slice(0, 8).ToString())), new Progress<StreamProgress>(p => { }));
            var (session2, file2) = await client.UploadBytesToSession(uploadSession.UploadUrl, 8, stringToUpload.Length, new MemoryStream(Encoding.UTF8.GetBytes(stringToUpload.AsSpan().Slice(8).ToString())), new Progress<StreamProgress>(p => { }));
            var fileStream = await client.DownloadFileAsync($"{_rootFolderId}/largeFile.txt", CancellationToken.None);
            using var reader = new StreamReader(fileStream);
            var fileContent = reader.ReadToEnd();

            Assert.That(session1.NextExpectedRanges.Length, Is.EqualTo(1));
            Assert.That(session1.NextExpectedRanges[0], Is.EqualTo("8-12"));
            Assert.That(file1, Is.Null);
            Assert.That(session2, Is.Null);
            Assert.That(file2.Name, Is.EqualTo("largeFile.txt"));
            Assert.That(fileContent, Is.EqualTo(stringToUpload));
        }
    }
}
