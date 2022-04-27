﻿namespace parser_cont.Services.Api;

public class ClientApi : IDisposable
{
    private readonly TimeSpan _timeout;
    private HttpClient _httpClient;
    private HttpClientHandler _httpClientHandler;
    private readonly string _baseUrl;
    private const string ClientUserAgent = "my-api-client-v1";
    private const string MediaTypeJson = "application/json";

    public ClientApi(string baseUrl, TimeSpan? timeout = null)
    {
        _baseUrl = NormalizeBaseUrl(baseUrl);
        _timeout = timeout ?? TimeSpan.FromSeconds(90);
    }
    public async Task<string> PostAsync(string url, object input)
    {
        EnsureHttpClientCreated();

        using var requestContent = new StringContent(ConvertToJsonString(input), Encoding.UTF8, MediaTypeJson);
        using var response = await _httpClient.PostAsync(url, requestContent);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }

    public async Task<string> PostAsyncByToken(string url, string token, object input)
    {
        EnsureHttpClientCreated();
        if (!_httpClient.DefaultRequestHeaders.Contains("auth-token"))
        {
            _httpClient.DefaultRequestHeaders.Add("auth-token", token);
        }
        using var requestContent = new StringContent(ConvertToJsonString(input), Encoding.UTF8, MediaTypeJson);
        using var response = await _httpClient.PostAsync(url, requestContent);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }

    public async Task<TResult> PostAsyncByToken<TResult>(string url, string token, object input) where TResult : class, new()
    {
        var strResponse = await PostAsyncByToken(url, token, input);

        return JsonSerializer.Deserialize<TResult>(strResponse)
            ?? throw new NullReferenceException();
    }

    public async Task<TResult> PostAsync<TResult>(string url, object input) where TResult : class, new()
    {
        var strResponse = await PostAsync(url, input);

        return JsonSerializer.Deserialize<TResult>(strResponse)
            ?? throw new NullReferenceException();
    }


    public async Task<TResult> GetAsyncByToken<TResult>(string url, string token) where TResult : class, new()
    {
        var strResponse = await GetAsyncByToken(url, token);
        return JsonSerializer.Deserialize<TResult>(strResponse) ?? throw new NullReferenceException();
    }

    public async Task<string> GetAsyncByToken(string url, string token)
    {
        EnsureHttpClientCreated();
        if (!_httpClient.DefaultRequestHeaders.Contains("auth-token"))
        {
            _httpClient.DefaultRequestHeaders.Add("auth-token", token);
        }
        using var response = await _httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }
    public async Task<string> GetAsync(string url)
    {
        EnsureHttpClientCreated();
        using var response = await _httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }

    public async Task<string> PutAsync(string url, object input)
    {
        return await PutAsync(url, new StringContent(JsonSerializer.Serialize(input), Encoding.UTF8, MediaTypeJson));
    }

    public async Task<string> PutAsync(string url, HttpContent content)
    {
        EnsureHttpClientCreated();

        using var response = await _httpClient.PutAsync(url, content);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }

    public async Task<string> DeleteAsync(string url)
    {
        EnsureHttpClientCreated();

        using var response = await _httpClient.DeleteAsync(url);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }

    public void Dispose()
    {
        _httpClientHandler?.Dispose();
        _httpClient?.Dispose();
    }

    private void CreateHttpClient()
    {
        _httpClientHandler = new HttpClientHandler
        {
            AutomaticDecompression = System.Net.DecompressionMethods.Deflate | System.Net.DecompressionMethods.GZip
        };

        _httpClient = new HttpClient(_httpClientHandler, false)
        {
            Timeout = _timeout
        };

        _httpClient.DefaultRequestHeaders.UserAgent.ParseAdd(ClientUserAgent);

        if (!string.IsNullOrWhiteSpace(_baseUrl))
        {
            _httpClient.BaseAddress = new Uri(_baseUrl);
        }

        _httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue(MediaTypeJson));
    }

    private void EnsureHttpClientCreated()
    {
        if (_httpClient == null)
        {
            CreateHttpClient();
        }
    }

    private static string ConvertToJsonString(object obj)
    {
        return obj == null ? string.Empty : JsonSerializer.Serialize(obj);
    }

    private static string NormalizeBaseUrl(string url)
    {
        return url.EndsWith("/") ? url : url + "/";
    }
}

