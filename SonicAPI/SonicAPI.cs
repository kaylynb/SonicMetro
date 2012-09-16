using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text;
using SonicAPI.RESTSchema;
using System.IO;
using System.Net.Http;
using System.Xml.Serialization;
using SonicAPI.Exceptions;
using SonicUtil.Utility;

namespace SonicAPI
{
    public class Query
    {
        const string APIVersion = "1.8.0";
        const string ClientString = "SonicMetro";

        private readonly string _baseUri;
        private readonly string _user;
        private readonly string _password;

        public string BaseUri { get { return _baseUri; } }
        public string User { get { return _user; } }
        private string Password { get { return _password; } }

        // XmlSerializer is thread safe
        private readonly XmlSerializer _serializer = new XmlSerializer(typeof(Response));

        public Query(string baseUri, string user, string password)
        {
            _baseUri = baseUri;
            _user = user;
            _password = password;
        }

        private Uri ConstructQuery(string type, Dictionary<string, string> values = null)
        {
            var builder = new StringBuilder(BaseUri);

            builder.AppendFormat("/rest/{0}.view?u={1}&p={2}&v={3}&c={4}", type, User, Password, APIVersion, ClientString);

            if(values != null)
                foreach (var value in values)
                    builder.AppendFormat("&{0}={1}", value.Key, value.Value);

            return new Uri(builder.ToString());
        }

        private static bool IsContentXml(HttpContent content)
        {
            ThrowIf.Null(content, "content");

            if (content.Headers.ContentType == null)
                return false;

            return content.Headers.ContentType.MediaType == "text/xml";
        }

        private async Task<Response> GetResponseFromHttpResponseAsync(HttpResponseMessage responseMessage)
        {
            ThrowIf.Null(responseMessage, "responseMessage");

            if (!IsContentXml(responseMessage.Content))
                throw new ResponseDoesNotContainXmlException();

            Response response;

            using (var strReader = new StringReader(await responseMessage.Content.ReadAsStringAsync().ConfigureAwait(false)))
            {
                response = (Response) _serializer.Deserialize(strReader);
            }

            if (response.status == ResponseStatus.failed)
                ThrowResponseStatusFailedException(response);

            return response;
        }

        private async Task<T> GetItemFromHttpResponseAsync<T>(HttpResponseMessage responseMessage)
        {
            ThrowIf.Null(responseMessage, "responseMessage");

            return (T) (await GetResponseFromHttpResponseAsync(responseMessage).ConfigureAwait(false)).Item;
        }

        private static void ThrowResponseStatusFailedException(Response response)
        {
            ThrowIf.Null(response, "response");

            var error = (Error) response.Item;

            switch (error.code)
            {
                case 0:
                    throw new GenericError();
                case 10:
                    throw new RequiredParameterIsMissing();
                case 20:
                    throw new RESTIncompatibilityClientMustUpgrade();
                case 30:
                    throw new RESTIncompatibilityServerMustUpgrade();
                case 40:
                    throw new WrongUsernameOrPassword();
                case 50:
                    throw new UserNotAuthorizedForOperation();
                case 60:
                    throw new TrialPeriodExpired();
                case 70:
                    throw new DataNotFound();
                default:
                    throw new UnknownError();
            }
        }

        public async Task<HttpResponseMessage> QueryServerAsync(Uri query)
        {
            ThrowIf.Null(query, "query");

            using (var client = new HttpClient())
            {
                var ret = await client.GetAsync(query).ConfigureAwait(false);
                ret.EnsureSuccessStatusCode();

                return ret;
            }
        }

        public Uri GetIndexesQuery(string musicFolderId)
        {
            return ConstructQuery("getIndexes", new Dictionary<string, string> { { "musicFolderId", musicFolderId } });
        }

        public Uri GetMusicDirectoryQuery(string id)
        {
            return ConstructQuery("getMusicDirectory", new Dictionary<string, string> { { "id", id } });
        }

        public Uri GetMusicFoldersQuery()
        {
            return ConstructQuery("getMusicFolders");
        }

        public async Task<T> GetAsync<T>(Uri requestUri)
        {
            ThrowIf.Null(requestUri, "requestUri");

            return await GetItemFromHttpResponseAsync<T>(
                await QueryServerAsync(requestUri).ConfigureAwait(false)).ConfigureAwait(false);
        }

        public async Task<HttpResponseMessage> GetCoverArtAsync(string coverArtId)
        {
            ThrowIf.NullOrEmpty(coverArtId, "coverArtId");

            var response = await QueryServerAsync(ConstructQuery("getCoverArt", new Dictionary<string, string> {{"id", coverArtId}})).ConfigureAwait(false);

            if (IsContentXml(response.Content))
            {
                await GetResponseFromHttpResponseAsync(response);

                throw new UnknownError("Unknown data received from getCoverArt");
            }

            return response;
        }

     /*   public async Task<Response> PingAsync()
        {
            return await GetResponseFromHttpResponseAsync(await QueryServerAsync(ConstructQuery("ping")));
        }

        public async Task<Response> GetLicenseAsync()
        {
            return await GetResponseFromHttpResponseAsync(await QueryServerAsync(ConstructQuery("getLicense")));
        }

        public async Task<HttpResponseMessage> GetRawCoverArtAsync(string coverArtID)
        {
            ThrowIf.NullOrEmpty(coverArtID, "coverArtID");

            return await QueryServerAsync(ConstructQuery("getCoverArt", new Dictionary<string, string> {{"id", coverArtID}}));
        }*/
    }
}