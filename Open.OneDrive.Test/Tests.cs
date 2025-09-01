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
            var clientId = Environment.GetEnvironmentVariable("CLIENT_ID")!;
            var refreshToken = Environment.GetEnvironmentVariable("REFRESH_TOKEN")!;
            var token = await OneDriveClient.RefreshAccessTokenAsync(refreshToken, clientId, "", CancellationToken.None);
            _accessToken = token.AccessToken;
            var client = new OneDriveClient(_accessToken);
            var rootFolderName = Guid.NewGuid().ToString();
            _rootFolderId = rootFolderName;
            await client.CreateFolderAsync("", rootFolderName);
        }

        [TearDown]
        public async Task TearDown()
        {
            var client = new OneDriveClient(_accessToken);
            await client.DeleteItemAsync(_rootFolderId);
        }

        [Test]
        public async Task GetDrivesTest()
        {
            var client = new OneDriveClient(_accessToken);
            var drives = await client.GetDrivesAsync();

            Assert.That(drives.Value, Is.Not.Null);
        }

        [Test]
        public async Task GetUsersDriveTest()
        {
            var client = new OneDriveClient(_accessToken);
            var drive = await client.GetDriveAsync();

            Assert.That(drive, Is.Not.Null);
        }


        [Test]
        public async Task GetItemsTest()
        {
            var stringToUpload = "Hello, World!";
            var client = new OneDriveClient(_accessToken);
            var file = await client.UploadFileAsync(_rootFolderId, "file.txt", new MemoryStream(Encoding.UTF8.GetBytes(stringToUpload)), true, new Progress<StreamProgress>(p => { }));
            var folder = await client.CreateFolderAsync(_rootFolderId, "folder", "", false);
            var items = await client.GetItemsAsync(_rootFolderId);

            Assert.That(items.Value, Is.Not.Null);
            Assert.That(items.Value.Count, Is.EqualTo(2));
            Assert.That(items.Value[0].Name, Is.EqualTo("folder"));
            Assert.That(items.Value[1].Name, Is.EqualTo("file.txt"));
        }

        [Test]
        public async Task GetItemsWithSelectTest()
        {
            var stringToUpload = "Hello, World!";
            var client = new OneDriveClient(_accessToken);
            var file = await client.UploadFileAsync(_rootFolderId, "file.txt", new MemoryStream(Encoding.UTF8.GetBytes(stringToUpload)), true, new Progress<StreamProgress>(p => { }));
            var folder = await client.CreateFolderAsync(_rootFolderId, "folder", "", false);
            var items = await client.GetItemsAsync(_rootFolderId, select: "id,name");

            Assert.That(items.Value, Is.Not.Null);
            Assert.That(items.Value.Count, Is.EqualTo(2));
            Assert.That(items.Value[0].Name, Is.EqualTo("folder"));
            Assert.That(items.Value[0].CreatedBy, Is.Null);
            Assert.That(items.Value[0].CreatedDateTime, Is.Null);
            Assert.That(items.Value[1].Name, Is.EqualTo("file.txt"));
            Assert.That(items.Value[1].CreatedBy, Is.Null);
            Assert.That(items.Value[1].CreatedDateTime, Is.Null);
        }

        [Test]
        public async Task GetItemsWithTopAndSkipTest()
        {
            var stringToUpload = "Hello, World!";
            var client = new OneDriveClient(_accessToken);
            for (int i = 1; i < 10; i++)
            {
                await client.CreateFolderAsync(_rootFolderId, $"folder {i}", "", false);
            }
            var items1 = await client.GetItemsAsync(_rootFolderId, top: 3);
            var items2 = await client.GetItemsAsync(_rootFolderId, top: 3, skipToken: GetSkipToken(items1.NextLink));
            var items3 = await client.GetItemsAsync(_rootFolderId, top: 3, skipToken: GetSkipToken(items2.NextLink));

            Assert.That(items1.Value, Is.Not.Null);
            Assert.That(items1.Value.Count, Is.EqualTo(3));
            Assert.That(items1.Value[0].Name, Is.EqualTo("folder 1"));
            Assert.That(items1.Value[1].Name, Is.EqualTo("folder 2"));
            Assert.That(items1.Value[2].Name, Is.EqualTo("folder 3"));
            Assert.That(items2.Value, Is.Not.Null);
            Assert.That(items2.Value.Count, Is.EqualTo(3));
            Assert.That(items2.Value[0].Name, Is.EqualTo("folder 4"));
            Assert.That(items2.Value[1].Name, Is.EqualTo("folder 5"));
            Assert.That(items2.Value[2].Name, Is.EqualTo("folder 6"));
            Assert.That(items3.Value, Is.Not.Null);
            Assert.That(items3.Value.Count, Is.EqualTo(3));
            Assert.That(items3.Value[0].Name, Is.EqualTo("folder 7"));
            Assert.That(items3.Value[1].Name, Is.EqualTo("folder 8"));
            Assert.That(items3.Value[2].Name, Is.EqualTo("folder 9"));
        }

        private string GetSkipToken(string nextLink)
        {
            var uri = new Uri(nextLink);
            var queryParams = uri.Query.TrimStart('?')
                             .Split('&', StringSplitOptions.RemoveEmptyEntries)
                             .Select(q => q.Split('='))
                             .ToDictionary(kv => kv[0].ToLower(), kv => Uri.UnescapeDataString(kv[1]));
            return queryParams["$skiptoken"];
        }

        //[Test]
        //public async Task GetFilesTest()
        //{
        //    var stringToUpload = "Hello, World!";
        //    var client = new OneDriveClient(_accessToken);
        //    var file = await client.UploadFileAsync(_rootFolderId, "file.txt", new MemoryStream(Encoding.UTF8.GetBytes(stringToUpload)), true, new Progress<StreamProgress>(p => { }));
        //    var folder = await client.CreateFolderAsync(_rootFolderId, "folder", "", false);
        //    var items = await client.SearchAsync(["driveItem"], $"path:\"{_rootFolderId}\" AND isDocument=true");

        //    Assert.That(items.Value, Is.Not.Null);
        //    Assert.That(items.Value.Count, Is.EqualTo(1));
        //    Assert.That(items.Value[0].Name, Is.EqualTo("file.txt"));
        //}

        [Test]
        public async Task GetFoldersTest()
        {
            var stringToUpload = "Hello, World!";
            var client = new OneDriveClient(_accessToken);
            var file = await client.UploadFileAsync(_rootFolderId, "file.txt", new MemoryStream(Encoding.UTF8.GetBytes(stringToUpload)), true, new Progress<StreamProgress>(p => { }));
            var folder = await client.CreateFolderAsync(_rootFolderId, "folder", "", false);
            var items = await client.GetItemsAsync(_rootFolderId, filter: "folder ne null");

            Assert.That(items.Value, Is.Not.Null);
            Assert.That(items.Value.Count, Is.EqualTo(1));
            Assert.That(items.Value[0].Name, Is.EqualTo("folder"));
        }

        [Test]
        public async Task UploadAndDownloadFileTest()
        {
            var stringToUpload = "Hello, World!";
            var client = new OneDriveClient(_accessToken);
            var file = await client.UploadFileAsync(_rootFolderId, "file.txt", new MemoryStream(Encoding.UTF8.GetBytes(stringToUpload)), true, new Progress<StreamProgress>(p => { }));
            var fileStream = await client.DownloadFileAsync($"{_rootFolderId}/file.txt", CancellationToken.None);
            using var reader = new StreamReader(fileStream);
            var fileContent = reader.ReadToEnd();

            Assert.That(file.Name, Is.EqualTo("file.txt"));
            Assert.That(fileContent, Is.EqualTo(stringToUpload));
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

        [Test]
        public async Task CopyFileTest()
        {
            var stringToUpload = "Hello, World!";
            var client = new OneDriveClient(_accessToken);
            var file = await client.UploadFileAsync(_rootFolderId, "file.txt", new MemoryStream(Encoding.UTF8.GetBytes(stringToUpload)), true, new Progress<StreamProgress>(p => { }));

            var item = new Item
            {
                Name = "copiedFile.txt"
            };
            var uri = await client.CopyItemAsync($"{_rootFolderId}/file.txt", item, CancellationToken.None);
            var copyStatus = await client.GetCopyStatusAsync(uri, CancellationToken.None);
            var fileStream = await client.DownloadFileAsync($"{_rootFolderId}/copiedFile.txt", CancellationToken.None);
            using var reader = new StreamReader(fileStream);
            var fileContent = reader.ReadToEnd();

            Assert.That(file.Name, Is.EqualTo("file.txt"));
            Assert.That(copyStatus.status.PercentageComplete, Is.EqualTo(100));
            Assert.That(fileContent, Is.EqualTo(stringToUpload));
        }

        [Test]
        public async Task SearchTest()
        {
            var stringToUpload = "Hello, World!";
            var client = new OneDriveClient(_accessToken);
            var file1 = await client.UploadFileAsync(_rootFolderId, "file.txt", new MemoryStream(Encoding.UTF8.GetBytes(stringToUpload)), true, new Progress<StreamProgress>(p => { }));
            var file2 = await client.UploadFileAsync(_rootFolderId, "file2.txt", new MemoryStream(Encoding.UTF8.GetBytes(stringToUpload)), true, new Progress<StreamProgress>(p => { }));
            var file3 = await client.UploadFileAsync(_rootFolderId, "nile.txt", new MemoryStream(Encoding.UTF8.GetBytes(stringToUpload)), true, new Progress<StreamProgress>(p => { }));
            var items = await client.SearchItemsAsync(_rootFolderId, "file");

            Assert.That(file1.Name, Is.EqualTo("file.txt"));
            Assert.That(file2.Name, Is.EqualTo("file2.txt"));
            //Assert.That(items.Value.Count, Is.EqualTo(2));
        }
    }
}
