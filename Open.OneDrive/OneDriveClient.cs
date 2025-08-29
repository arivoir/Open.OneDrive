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
        #region fields

        private static readonly string ApiServiceUri = "https://graph.microsoft.com/v1.0";
        private static readonly string OAUTH2 = "https://login.microsoftonline.com/common/oauth2/v2.0/authorize";
        private static readonly string TOKEN = "https://login.microsoftonline.com/common/oauth2/v2.0/token";
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

        #region initialization

        public OneDriveClient()
        {
        }

        public OneDriveClient(string accessToken)
        {
            if (string.IsNullOrWhiteSpace(accessToken))
                throw new ArgumentNullException(nameof(accessToken));
            _accessToken = accessToken;
        }

        #endregion

        #region authentication

        public static string GetRequestUrl(string clientId, string scope, string callbackUrl = "https://login.microsoftonline.com/common/oauth2/nativeclient", string response_type = "code")
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

        #region public methods

        /// <summary>
        /// Gets the list of drives. 
        /// </summary>
        /// <remarks>GET me/drives</remarks>
        /// <param name="cancellationToken">The cancellation token.</param>
        public async Task<Drives> GetDrivesAsync(string expand = null, string select = null, string skipToken = null, int? top = null, string orderby = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            var uri = BuildApiUri("/me/drives", expand, select, skipToken, top, orderby);
            var client = CreateClient();
            var response = await client.GetAsync(uri, cancellationToken);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadJsonAsync<Drives>();
            }
            else
            {
                throw await ProcessException(response.Content);
            }
        }

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
            var uri = BuildApiUri($"{folderPath}:/children", expand, select, skipToken, top, orderby, filter);
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
            var uri = BuildApiUri($"/me{filePath}:/search(q='{q}')", expand, select, skipToken, top, filter: filter);
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
            var uri = BuildApiUri($"/me{filePath}:/content");
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

        public async Task<Item> UploadFileAsync(string folderPath, string fileName, Stream fileStream, bool? overwrite, IProgress<StreamProgress> progress, string expand = null, string select = null, CancellationToken cancellationToken = default)
        {
            var uri = BuildApiUri($"/me{folderPath}/{fileName}:/content", expand);
            var client = CreateClient();
            var content = new StreamedContent(fileStream, progress, cancellationToken);
            content.Headers.ContentType = new MediaTypeHeaderValue("text/plain");
            var response = await client.PutAsync(uri, content, cancellationToken);//.AsTask(cancellationToken, progress);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadJsonAsync<Item>();
            }
            else
            {
                throw await ProcessException(response.Content);
            }
        }

        public async Task<UploadSession> CreateUploadSession(string folderPath, string fileName, string description, bool? overwrite, string expand = null, string select = null, CancellationToken cancellationToken = default)
        {
            var uri = BuildApiUri($"/me{folderPath}/{fileName}:/createUploadSession", expand);
            var client = CreateClient();
            var item = new Item { Name = fileName, Description = description };
            item.ConflictBehavior = overwrite.HasValue ? overwrite.Value ? ConflictBehavior.Replace : ConflictBehavior.Rename : ConflictBehavior.Fail;
            var content = new StringContent(item.SerializeJson());
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var response = await client.PostAsync(uri, content, cancellationToken);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadJsonAsync<UploadSession>();
            }
            else
            {
                throw await ProcessException(response.Content);
            }
        }

        public async Task<(UploadSession, Item)> UploadBytesToSession(string uri, int from, int total, Stream fileStream, IProgress<StreamProgress> progress, string expand = null, string select = null, CancellationToken cancellationToken = default)
        {
            var client = new HttpClient();
            var content = new StreamedContent(fileStream, progress, cancellationToken);
            content.Headers.ContentRange = new ContentRangeHeaderValue(from, from + fileStream.Length - 1, total);
            var response = await client.PutAsync(uri, content, cancellationToken);//.AsTask(cancellationToken, progress);
            if (response.IsSuccessStatusCode)
            {
                if (response.StatusCode == HttpStatusCode.Created)
                    return (null, await response.Content.ReadJsonAsync<Item>());
                else
                    return (await response.Content.ReadJsonAsync<UploadSession>(), null);
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

            var uri = BuildApiUri($"/me{itemPath}:/copy");
            var client = CreateClient();
            var content = new StringContent(item.SerializeJson());
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
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

        public async Task<(AsyncJobStatus status, Uri url)> GetCopyStatusAsync(Uri uri, CancellationToken cancellationToken)
        {
            var client = new HttpClient();
            var response = await client.GetAsync(uri, cancellationToken);
            if (response.IsSuccessStatusCode)
            {
                return new(await response.Content.ReadJsonAsync<AsyncJobStatus>(), null);
            }
            else if (response.StatusCode == (HttpStatusCode)303)
            {
                return new(null, response.Headers.Location);
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

        #region private stuff

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
                query += "&$expand=" + expand;
            if (!string.IsNullOrWhiteSpace(select))
                query += "&$select=" + select;
            if (top.HasValue && top > 0)
                query += "&$top=" + top;
            if (!string.IsNullOrWhiteSpace(orderby))
                query += "&$orderby=" + orderby;
            if (!string.IsNullOrWhiteSpace(filter))
                query += "&$filter=" + filter;
            if (!string.IsNullOrWhiteSpace(skipToken))
                query += "&$skiptoken=" + skipToken;
            if (!string.IsNullOrWhiteSpace(q))
                query += "&q=" + q;
            builder.Query = Uri.EscapeDataString(query);
            return builder.Uri;

        }

        public static string GetPath(string itemPath)
        {
            return string.IsNullOrWhiteSpace(itemPath) ? "/drive/root" : "/drive/root:/" + itemPath;
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
