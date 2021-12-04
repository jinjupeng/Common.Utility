using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Common.Utility.HttpClientWrapper
{
    public interface IBaseHttpClient
    {
        Task<T> GetAsync<T>(string url, HttpCompletionOption httpCompletionOption = default, JsonSerializerOptions jsonSerializerOptions = default, CancellationToken cancellationToken = default);

        Task<T> PostAsync<T>(string url, HttpContent contentPost, JsonSerializerOptions jsonSerializerOptions = default, CancellationToken cancellationToken = default);

        Task<T> PutAsync<T>(string url, HttpContent contentPut, JsonSerializerOptions jsonSerializerOptions = default, CancellationToken cancellationToken = default);

        Task<T> DeleteAsync<T>(string url, JsonSerializerOptions jsonSerializerOptions = default, CancellationToken cancellationToken = default);
    }


    public class BaseHttpClient : IBaseHttpClient
    {
        private readonly IHttpClientFactory httpClientFactory;
        private HttpClient client;
        private readonly string clientName;

        public BaseHttpClient(IHttpClientFactory httpClientFactory, string clientName)
        {
            this.httpClientFactory = httpClientFactory;
            this.clientName = clientName;
        }

        #region Generic, Async, static HTTP functions for GET, POST, PUT, and DELETE             

        public async Task<T> GetAsync<T>(string url, HttpCompletionOption httpCompletionOption = default, JsonSerializerOptions jsonSerializerOptions = default, CancellationToken cancellationToken = default)
        {
            client = httpClientFactory.CreateClient(clientName);

            var response = await client.GetAsync(url, httpCompletionOption, cancellationToken);
            if (response.IsSuccessStatusCode)
            {
                using var contentStream = await response.Content.ReadAsStreamAsync();
                var data = await JsonSerializer.DeserializeAsync<T>(contentStream, jsonSerializerOptions);
                return data;

            }
            object o = new object();
            return (T)o;
        }

        public async Task<T> PostAsync<T>(string url, HttpContent contentPost, JsonSerializerOptions jsonSerializerOptions = default, CancellationToken cancellationToken = default)
        {
            client = httpClientFactory.CreateClient(clientName);
            var response = await client.PostAsync(url, contentPost, cancellationToken);
            if (response.IsSuccessStatusCode)
            {
                using var contentStream = await response.Content.ReadAsStreamAsync();
                var data = await JsonSerializer.DeserializeAsync<T>(contentStream, jsonSerializerOptions);
                return data;
            }
            object o = new object();
            return (T)o;
        }

        public async Task<T> PutAsync<T>(string url, HttpContent contentPut, JsonSerializerOptions jsonSerializerOptions = default, CancellationToken cancellationToken = default)
        {
            client = httpClientFactory.CreateClient(clientName);

            var response = await client.PutAsync(url, contentPut, cancellationToken);
            if (response.IsSuccessStatusCode)
            {
                using var contentStream = await response.Content.ReadAsStreamAsync();
                var data = await JsonSerializer.DeserializeAsync<T>(contentStream, jsonSerializerOptions);
                return data;
            }
            object o = new object();
            return (T)o;
        }

        public async Task<T> DeleteAsync<T>(string url, JsonSerializerOptions jsonSerializerOptions = default, CancellationToken cancellationToken = default)
        {
            client = httpClientFactory.CreateClient(clientName);

            var response = await client.DeleteAsync(url, cancellationToken);
            if (response.IsSuccessStatusCode)
            {
                using var contentStream = await response.Content.ReadAsStreamAsync();
                var data = await JsonSerializer.DeserializeAsync<T>(contentStream, jsonSerializerOptions);
                return data;
            }
            object o = new object();
            return (T)o;
        }

        #endregion
    }
}
