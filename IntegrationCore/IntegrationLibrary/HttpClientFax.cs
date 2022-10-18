using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;
using System.Net;


namespace IntegrationCore.IntegrationLibrary
{
    public static class HttpClientFax
    {
        /// <summary>
        /// Creates an instance of the HTTP client with the specified credentials
        /// and with various other useful settings.
        ///
        /// If the url parameter is provided, the client will use cookie session after authentication.
        /// Otherwise, the Basic Auth credentials will be provided in the header of each request using this client.
        /// </summary>
        public static async Task<HttpClient> CreateClient(string username, string password, string url = null)
        {
            try
            {

           
            if (string.IsNullOrEmpty(username))
                throw new ArgumentException("Can't be null or empty.", "username");

            var handler = new HttpClientHandler
            {
                Credentials = new NetworkCredential(username, password),
                PreAuthenticate = true,
            };

            var client = new HttpClient(handler);

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.Default.GetBytes(username + ":" + password)));
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            if (!string.IsNullOrEmpty(url))
            {
                var loginUrl = url + "/login";
                client = await Authenticate(client, loginUrl);
            }

            return client;
            }
            catch (Exception ex)
            {

                 throw new Exception(ex.Message.ToString()); ;
            }
        }

        /// <summary>
        /// Authenticates to the API Login endpoint with BasicAuth. If Authentication is successful, the
        /// basic authentication credentials are removed and subsequent calls will use cookie-based authentication.
        /// </summary>
        static async Task<HttpClient> Authenticate(HttpClient client, string loginUrl)
        {
            var response = await client.GetAsync(loginUrl);
            response.EnsureSuccessStatusCode();

            client.DefaultRequestHeaders.Authorization = null;

            return client;
        }

        /// <summary>
        /// Serializes an object as JSON and POSTs the result to the specified URL.
        /// </summary>
        public static Task<string> Post(this HttpClient client, string url, object instance)
        {
            //url = url.Replace("localhost:", "localhost.fiddler:");
            var data = JsonConvert.SerializeObject(instance, Formatting.None, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            var content = new StringContent(data, Encoding.UTF8, "application/json");

            return Post(client, url, content);
        }

        /// <summary>
        /// POSTs the specified content to the specified URL and returns the location header from the response,
        /// which is the URL of the newly created resource.
        /// </summary>
        public static async Task<string> Post(this HttpClient client, string url, HttpContent content)
        {
            //url = url.Replace("localhost:", "localhost.fiddler:");
            var response = await client.PostAsync(url, content);
            response.EnsureSuccessStatusCode();

            return response.Headers.Location.ToString();
        }


        /// <summary>
        /// GETs from the specified URL and deserializes the JSON response as an instance of the specified type.
        /// </summary>
        public static async Task<T> Get<T>(this HttpClient client, string url)
        {
            //url = url.Replace("localhost:", "localhost.fiddler:");
            var response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var contentAsString = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(contentAsString);
        }
    }
}
