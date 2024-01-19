using System;
using System.Net;
using System.Text;
using System.Text.Json;
using Mango.Web.Models;
using Mango.Web.Services.IServices;
using static Mango.Web.Utility.SD;

namespace Mango.Web.Services
{
	public class BaseService : IBaseService
	{
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ITokenProvider _tokenProvider;

        public BaseService(IHttpClientFactory httpClientFactory, ITokenProvider tokenProvider)
		{
            _httpClientFactory = httpClientFactory;
            _tokenProvider = tokenProvider;
        }

        public async Task<ResponseDTO?> SendAsync(RequestDTO requestDTO, bool withBearer = true)
        {
            try
            {

                var client = _httpClientFactory.CreateClient("MangoAPI");

                HttpRequestMessage message = new();

                message.Headers.Add("Accept", "application/json");

                if (withBearer)
                {
                    string token = _tokenProvider.GetToken();
                    message.Headers.Add("Authorization", $"Bearer {token}");
                }
                message.RequestUri = new Uri(requestDTO.Url);

                if (requestDTO.Data != null)
                {
                    message.Content = new StringContent(JsonSerializer.Serialize(requestDTO.Data), Encoding.UTF8, "application/json");
                }

                switch (requestDTO.ApiType)
                {
                    case ApiType.POST:
                        message.Method = HttpMethod.Post;
                        break;

                    case ApiType.PUT:
                        message.Method = HttpMethod.Put;
                        break;
                    case ApiType.DELETE:
                        message.Method = HttpMethod.Delete;
                        break;
                    default:
                        message.Method = HttpMethod.Get;
                        break;

                }

                var response = await client.SendAsync(message);

                switch (response.StatusCode)
                {
                    case HttpStatusCode.NotFound:
                        return new ResponseDTO() { IsSuccess = false, Message = "Not Found" };
                    case HttpStatusCode.Forbidden:
                        return new() { IsSuccess = false, Message = "Access Denied" };

                    case HttpStatusCode.Unauthorized:
                        return new() { IsSuccess = false, Message = "Unauthorized" };
                    case HttpStatusCode.InternalServerError:
                        return new ResponseDTO() { IsSuccess = false, Message = "Internal server error" };
                    case HttpStatusCode.MethodNotAllowed:
                        return new ResponseDTO() { IsSuccess = false, Message = "Method not Allowed" };
                    default:
                        //var apiContent = await response.Content.ReadAsStringAsync();
                        //var responseDTO = JsonSerializer.Deserialize<ResponseDTO>(apiContent);
                        // OR
                        var responseDTO = await response.Content.ReadFromJsonAsync<ResponseDTO>();
                        return responseDTO;


                }
            }
            catch (Exception ex)
            {
                var dto = new ResponseDTO()
                {
                    IsSuccess = false,
                    Message = ex.Message.ToString()
                };

                return dto;
            }
        }
    }
}

