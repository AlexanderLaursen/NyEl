using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;
using Common.Exceptions;
using Common.Models;

namespace MVC.Services
{
    public class CommonApiService
    {
        protected readonly HttpClient _httpClient;
        private readonly ILogger<CommonApiService> _logger;
        private readonly string _apiBaseUrl;

        public CommonApiService(HttpClient httpClient, ILogger<CommonApiService> logger, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _logger = logger;
            _apiBaseUrl = configuration["ApiBaseUrl"];
        }

        public async Task<Result<T>> GetAsync<T>(string url, string? bearerToken = default)
        {
            string fullUrl = $"{_apiBaseUrl}{url}";
            try
            {
                if (!string.IsNullOrEmpty(bearerToken))
                {
                    _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);
                }

                HttpResponseMessage response = await _httpClient.GetAsync(fullUrl);

                if (!response.IsSuccessStatusCode)
                {
                    throw new ApiResponseException($"Failed to retrieve the requested data from the API with status code {response.StatusCode}", response.StatusCode);
                }

                if (response.Content.Headers.ContentLength == 0)
                {
                    return Result<T>.Success(default);
                }

                T? result = await response.Content.ReadFromJsonAsync<T>(
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                if (result == null)
                {
                    throw new SerializationException("Failed to deserialize API response.");
                }

                return Result<T>.Success(result);
            }
            catch (ApiResponseException ex)
            {
                _logger.LogError(ex.Message);
                return HandleError<T>(ex.StatusCode);
            }
            catch (SerializationException ex)
            {
                _logger.LogError(ex.Message);
                return Result<T>.Failure();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unkown error occured while fetching from API.");
                return Result<T>.Failure();
            }
        }

        public async Task<Result<T>> PostAsync<T>(string url, object data, string? bearerToken = default)
        {
            string fullUrl = $"{_apiBaseUrl}{url}";
            try
            {
                if (!string.IsNullOrEmpty(bearerToken))
                {
                    _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);
                }

                HttpResponseMessage response = await _httpClient.PostAsJsonAsync(fullUrl, data);

                if (!response.IsSuccessStatusCode)
                {
                    throw new ApiResponseException($"Failed to post data to the API with status code {response.StatusCode}", response.StatusCode);
                }

                if (response.Content.Headers.ContentLength == 0)
                {
                    return Result<T>.Success(default);
                }

                T? result = await response.Content.ReadFromJsonAsync<T>(
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                if (result == null)
                {
                    throw new SerializationException("Failed to deserialize API response.");
                }

                return Result<T>.Success(result);
            }
            catch (ApiResponseException ex)
            {
                _logger.LogError(ex.Message);
                return HandleError<T>(ex.StatusCode);
            }
            catch (SerializationException ex)
            {
                _logger.LogError(ex.Message);
                return Result<T>.Failure("Failed to deserialize API response.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unknown error occurred while posting to API.");
                return Result<T>.Failure();
            }
        }

        public async Task<Result<T>> PutAsync<T>(string url, object data, string? bearerToken = default)
        {
            string fullUrl = $"{_apiBaseUrl}{url}";
            try
            {
                if (!string.IsNullOrEmpty(bearerToken))
                {
                    _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);
                }

                HttpResponseMessage response = await _httpClient.PutAsJsonAsync(fullUrl, data);

                if (!response.IsSuccessStatusCode)
                {
                    throw new ApiResponseException($"Failed to update data in the API with status code {response.StatusCode}", response.StatusCode);
                }

                if (response.Content.Headers.ContentLength == 0)
                {
                    return Result<T>.Success(default);
                }

                T? result = await response.Content.ReadFromJsonAsync<T>(
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                if (result == null)
                {
                    throw new SerializationException("Failed to deserialize API response.");
                }

                return Result<T>.Success(result);
            }
            catch (ApiResponseException ex)
            {
                _logger.LogError(ex.Message);
                return HandleError<T>(ex.StatusCode);
            }
            catch (SerializationException ex)
            {
                _logger.LogError(ex.Message);
                return Result<T>.Failure("Failed to deserialize API response.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unknown error occurred while updating data in API.");
                return Result<T>.Failure();
            }
        }

        public async Task<Result<bool>> DeleteAsync(string url, string? bearerToken = default)
        {
            string fullUrl = $"{_apiBaseUrl}{url}";
            try
            {
                if (!string.IsNullOrEmpty(bearerToken))
                {
                    _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);
                }

                HttpResponseMessage response = await _httpClient.DeleteAsync(fullUrl);

                if (!response.IsSuccessStatusCode)
                {
                    throw new ApiResponseException($"Failed to delete data from the API with status code {response.StatusCode}", response.StatusCode);
                }

                return Result<bool>.Success(true);
            }
            catch (ApiResponseException ex)
            {
                _logger.LogError(ex.Message);
                return HandleError<bool>(ex.StatusCode);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unknown error occurred while deleting from API.");
                return Result<bool>.Failure();
            }
        }

        public async Task<Result<T>> PostCustomUrlAsync<T>(string url, object data, string? bearerToken = default)
        {
            try
            {
                if (!string.IsNullOrEmpty(bearerToken))
                {
                    _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);
                }

                HttpResponseMessage response = await _httpClient.PostAsJsonAsync(url, data);

                if (!response.IsSuccessStatusCode)
                {
                    throw new ApiResponseException($"Failed to post data to the API with status code {response.StatusCode}", response.StatusCode);
                }

                if (response.Content.Headers.ContentLength == 0)
                {
                    return Result<T>.Success(default);
                }

                T? result = await response.Content.ReadFromJsonAsync<T>(
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                if (result == null)
                {
                    throw new SerializationException("Failed to deserialize API response.");
                }

                return Result<T>.Success(result);
            }
            catch (ApiResponseException ex)
            {
                _logger.LogError(ex.Message);
                return HandleError<T>(ex.StatusCode);
            }
            catch (SerializationException ex)
            {
                _logger.LogError(ex.Message);
                return Result<T>.Failure("Failed to deserialize API response.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unknown error occurred while posting to API.");
                return Result<T>.Failure();
            }
        }

        public Result<T> HandleError<T>(HttpStatusCode statusCode)
        {
            switch (statusCode)
            {
                case HttpStatusCode.NotFound:
                    return Result<T>.NotFound();
                case HttpStatusCode.BadRequest:
                    return Result<T>.InvalidInput();
                case HttpStatusCode.Unauthorized:
                    return Result<T>.Unauthorized();
                default:
                    return Result<T>.Failure();
            }
        }
    }
}