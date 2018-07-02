using Open.IO;
using Open.Net.Http;
using Open.OAuth2;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace Open.OneDrive
{
    public class OneDriveClient : OAuth2Client
    {
        #region ** fields

        private static readonly string ApiServiceUri = "https://api.onedrive.com/v1.0";
        private static readonly string OAUTH2 = "https://login.live.com/oauth20_authorize.srf";
        private static readonly string TOKEN = "https://login.live.com/oauth20_token.srf";
        private string _accessToken = null;

        //Get root folder for user's default Drive GET /drive/root 
        //List children under the Drive GET /drive/root/children 
        //List changes for all Items in the Drive GET /drive/root/view.delta 
        //Search for Items in the Drive (preview) GET /drive/root/view.search 
        //Access special folder GET /drive/special/{name} 

        //Get metadata for an Item GET /drive/items/{id} GET /drive/root:/{path}
        //Create an Item PUT /drive/items/{parent-id}/children/{name} PUT /drive/root:/{parent-path}/{name} 
        //Upload an Item's contents PUT /drive/items/{parent-id}/children/{name}/content PUT /drive/root:/{parent-path}/{name}:/content 
        //Update an Item's contents PATCH /drive/items/{id} PATCH /drive/root:/{path} 
        //Delete an Item DELETE /drive/items/{id} DELETE /drive/root:/{path} 
        //Move an Item PATCH /drive/items/{id} PATCH /drive/root:/{path} 
        //Copy an Item POST /drive/items/{id}/action.copy POST /drive/root:/{path}:/action.copy
        //Download an Item's contents GET /drive/items/{id}/content GET /drive/root:/{path}:/content 
        //Search for an Item GET /drive/items/{id}/view.search GET /drive/root:/{path}:/view.search
        //View changes on an Item GET /drive/items/{id}/view.delta GET /drive/root:/{path}:/view.delta
        //Get thumbnails for an Item GET /drive/items/{id}/thumbnails GET /drive/root:/{path}:/thumbnails

        #endregion

        #region ** initialization

        public OneDriveClient()
        {
        }

        public OneDriveClient(string accessToken)
        {
            _accessToken = accessToken;
        }

        #endregion

        #region ** authentication

        public static string GetRequestUrl(string clientId, string scope, string callbackUrl = "https://oauth.live.com/desktop", string response_type = "code")
        {
            return OAuth2Client.GetRequestUrl(OAUTH2, clientId, scope, callbackUrl, response_type: response_type, parameters: new Dictionary<string, string> { { "prompt", "login" } });
        }

        public static async Task<OAuth2Token> ExchangeCodeForAccessTokenAsync(string code, string clientId, string clientSecret, string callbackUrl)
        {
            return await OAuth2Client.ExchangeCodeForAccessTokenAsync(TOKEN, code, clientId, clientSecret, callbackUrl);
        }

#if NETFX_CORE
        public static async Task<OAuth2Token> RefreshAccessTokenAsync(string refreshToken, string clientId, string clientSecret)
        {
            return await OAuth2Client.RefreshAccessTokenAsync(OAuthUri, refreshToken, clientId, clientSecret);
        }
#else
        public static async Task<OAuth2Token> RefreshAccessTokenAsync(string refreshToken, string clientId, string clientSecret, CancellationToken cancellationToken)
        {
            return await OAuth2Client.RefreshAccessTokenAsync(TOKEN, refreshToken, clientId, clientSecret, cancellationToken);
        }
#endif

        #endregion

        #region ** public methods

        /// <summary>
        /// Get user's default Drive metadata 
        /// </summary>
        /// <remarks>GET /drive</remarks>
        /// <param name="cancellationToken">The cancellation token.</param>
        public async Task<Drive> GetDriveAsync(string expand = null, string select = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            var uri = BuildApiUri("/drive", expand, select);
            var client = CreateClient();
            var response = await client.GetAsync(uri, cancellationToken);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadJsonAsync<Drive>();
            }
            else
            {
                throw await ProcessException(response.Content);
            }
        }

        /// <summary>
        /// Get Drive metadata of another Drive  
        /// </summary>
        /// <remarks>GET /drives/{drive-id}</remarks>
        /// <param name="driveId">The id of the drive.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public async Task<Drive> GetDriveByIdAsync(string driveId, string expand = null, string select = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            var uri = BuildApiUri("/drives/" + driveId, expand, select);
            var client = CreateClient();
            var response = await client.GetAsync(uri, cancellationToken);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadJsonAsync<Drive>();
            }
            else
            {
                throw await ProcessException(response.Content);
            }
        }

        /// <summary>
        /// List an Item's children
        /// </summary>
        /// <remarks>
        /// GET /drive/items/{id}/children GET /drive/root:/{path}:/children
        /// </remarks>
        /// <param name="folderId">The folder identifier.</param>
        /// <param name="expand">The expand.</param>
        /// <param name="select">The select.</param>
        /// <param name="skipToken">The skip token.</param>
        /// <param name="top">The top.</param>
        /// <param name="orderby">The orderby.</param>
        /// <param name="filter">The filter.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public async Task<Items> GetItemsAsync(string folderPath, string expand = null, string select = null, string skipToken = null, int? top = null, string orderby = null, string filter = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            var uri = BuildApiUri(folderPath + "/children", expand, select, skipToken, top, orderby, filter);
            var client = CreateClient();
            var response = await client.GetAsync(uri, cancellationToken);
            if (response.IsSuccessStatusCode)
            {
                return (await response.Content.ReadJsonAsync<Items>());
            }
            else
            {
                throw await ProcessException(response.Content);
            }
        }

        /// <summary>
        /// Get metadata for an Item.
        /// </summary>
        /// <remarks>GET /drive/items/{id} GET /drive/root:/{path}</remarks>
        /// <param name="itemPath">The folder path.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public async Task<Item> GetItemAsync(string itemPath, string expand = null, string select = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            var uri = BuildApiUri(itemPath, expand, select);
            var client = CreateClient();
            var response = await client.GetAsync(uri, cancellationToken);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadJsonAsync<Item>();
            }
            else
            {
                throw await ProcessException(response.Content);
            }
        }

        public async Task<Items> SearchAsync(string filePath, string q, string expand = null, string select = null, string skipToken = null, int? top = null, string orderby = null, string filter = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            var uri = BuildApiUri(filePath + "/view.search", expand, select, skipToken, top, filter: filter, q: q);
            var client = CreateClient();
            var response = await client.GetAsync(uri, cancellationToken);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadJsonAsync<Items>();
            }
            else
            {
                throw await ProcessException(response.Content);
            }
        }

        public async Task<Stream> DownloadFileAsync(string filePath, CancellationToken cancellationToken)
        {
            var client = CreateClient();
            var uri = BuildApiUri(filePath + "/content");
            var response = await client.GetAsync(uri, HttpCompletionOption.ResponseHeadersRead, cancellationToken);
            if (response.IsSuccessStatusCode)
            {
                return new StreamWithLength(await response.Content.ReadAsStreamAsync(), response.Content.Headers.ContentLength);
            }
            else
            {
                throw await ProcessException(response.Content);
            }
        }

        public async Task<Item> UploadFileAsync(string folderPath, string fileName, Stream fileStream, bool? overwrite, IProgress<StreamProgress> progress, string expand = null, string select = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            var uri = BuildApiUri(folderPath + "/children", expand);
            var client = CreateClient();
            var content = new MultipartContent("related");
            var item = new Item();
            item.Name = fileName;
            item.File = new FileFacet();
            item.SourceUrl = "cid:content";
            item.ConflictBehavior = overwrite.HasValue ? overwrite.Value ? ConflictBehavior.Replace : ConflictBehavior.Rename : ConflictBehavior.Fail;
            var text = item.SerializeJson();
            var textContent = new StringContent(text);
            textContent.Headers.Add("Content-ID", "<metadata>");
            textContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            content.Add(textContent);
            var fileContent2 = new StreamedContent(fileStream, progress, cancellationToken);
            fileContent2.Headers.Add("Content-ID", "<content>");
            content.Add(fileContent2);
            var response = await client.PostAsync(uri, content, cancellationToken);//.AsTask(cancellationToken, progress);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadJsonAsync<Item>();
            }
            else
            {
                throw await ProcessException(response.Content);
            }
        }

        public async Task DeleteItemAsync(string itemPath, CancellationToken cancellationToken = default(CancellationToken))
        {
            var uri = BuildApiUri(itemPath);
            var client = CreateClient();
            var response = await client.DeleteAsync(uri, cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                throw await ProcessException(response.Content);
            }
        }

        public async Task<Item> CreateFolderAsync(string folderPath, string name, string description, bool? overwrite, CancellationToken cancellationToken = default(CancellationToken))
        {
            var uri = BuildApiUri(folderPath + "/children");
            var client = CreateClient();
            var item = new Item { Name = name, Description = description };
            item.Folder = new FolderFacet();
            item.ConflictBehavior = overwrite.HasValue ? overwrite.Value ? ConflictBehavior.Replace : ConflictBehavior.Rename : ConflictBehavior.Fail;
            var content = new StringContent(item.SerializeJson());
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var response = await client.PostAsync(uri, content, cancellationToken);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadJsonAsync<Item>();
            }
            else
            {
                throw await ProcessException(response.Content);
            }
        }

        public async Task<Uri> CopyItemAsync(string itemPath, Item item, CancellationToken cancellationToken = default(CancellationToken))
        {

            var uri = BuildApiUri(itemPath + "/action.copy");
            var client = CreateClient();
            var requestStream = new MemoryStream();
            var content = new StringContent(item.SerializeJson());
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            content.Headers.Add("Prefer", "respond-async");
            var response = await client.PostAsync(uri, content, cancellationToken);
            if (response.IsSuccessStatusCode)
            {
                return response.Headers.Location;
            }
            else
            {
                throw await ProcessException(response.Content);
            }
        }

        public async Task<Tuple<AsyncOperationStatus, Uri>> GetCopyStatusAsync(Uri uri, CancellationToken cancellationToken)
        {
            var client = CreateClient(false);
            var response = await client.GetAsync(uri, cancellationToken);
            if (response.IsSuccessStatusCode)
            {
                return new Tuple<AsyncOperationStatus, Uri>(await response.Content.ReadJsonAsync<AsyncOperationStatus>(), null);
            }
            else if (response.StatusCode == (HttpStatusCode)303)
            {
                return new Tuple<AsyncOperationStatus, Uri>(null, response.Headers.Location);
            }
            else
            {
                throw await ProcessException(response.Content);
            }
        }

        public async Task<Item> UpdateItemAsync(string itemPath, Item item, string expand = null, string select = null, CancellationToken cancellationToken = default(CancellationToken))
        {

            var uri = BuildApiUri(itemPath, expand, select);
            var client = CreateClient();
            var content = new StringContent(item.SerializeJson());
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var request = new HttpRequestMessage(new HttpMethod("PATCH"), uri);
            request.Content = content;
            var response = await client.SendAsync(request, HttpCompletionOption.ResponseContentRead, cancellationToken);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadJsonAsync<Item>();
            }
            else
            {
                throw await ProcessException(response.Content);
            }
        }

        #endregion

        #region ** private stuff

        /// <summary>
        /// Build the API service URI.
        /// </summary>
        /// <param name="path">The relative path requested.</param>
        /// <returns>The request URI.</returns>
        private Uri BuildApiUri(string path, string expand = null, string select = null, string skipToken = null, int? top = null, string orderby = null, string filter = null, string q = null)
        {
            var builder = new UriBuilder(ApiServiceUri);
            builder.Path += path;
            var query = builder.Query ?? "";
            if (query.StartsWith("?"))
                query = query.Substring(1);
            if (!string.IsNullOrWhiteSpace(expand))
                query += "&$expand=" + Uri.EscapeDataString(expand);
            if (!string.IsNullOrWhiteSpace(select))
                query += "&$select=" + Uri.EscapeDataString(select);
            if (top.HasValue && top > 0)
                query += "&$top=" + top;
            if (!string.IsNullOrWhiteSpace(orderby))
                query += "&$orderby=" + Uri.EscapeDataString(orderby);
            if (!string.IsNullOrWhiteSpace(filter))
                query += "&$filter=" + Uri.EscapeDataString(filter);
            if (!string.IsNullOrWhiteSpace(skipToken))
                query += "&$skiptoken=" + skipToken;
            if (!string.IsNullOrWhiteSpace(q))
                query += "&q=" + Uri.EscapeDataString(q);
            builder.Query = query;
            return builder.Uri;

        }

        public static string GetPath(string itemPath)
        {
            return string.IsNullOrWhiteSpace(itemPath) ? "/drive/root" : "/drive/root/" + itemPath;
        }
        public static string GetPathById(string itemId)
        {
            return "/drive/items/" + itemId;
        }

        private HttpClient CreateClient(bool allowAutoRedirect = true)
        {
            var client = new HttpClient(new RetryMessageHandler(HttpMessageHandlerFactory.Default.GetHttpMessageHandler(allowAutoRedirect: allowAutoRedirect)));
            //Necessary in WP8.1, the cache is not invalidated after updating an item.
            client.DefaultRequestHeaders.CacheControl = new CacheControlHeaderValue() { NoCache = true };
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);
            client.Timeout = Timeout.InfiniteTimeSpan;
            return client;
        }

        private async Task<Exception> ProcessException(HttpContent content)
        {
            if (content.Headers.ContentType?.MediaType == "text/html")
                return new OneDriveException(await content.ReadAsStringAsync());
            var error = await content.ReadJsonAsync<ErrorResponse>();
            return new OneDriveException(error.Error);
        }

        #endregion
    }
}
