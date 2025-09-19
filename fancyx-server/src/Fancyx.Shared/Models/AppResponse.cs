using Fancyx.Shared.Consts;
using System.Text.Json.Serialization;

namespace Fancyx.Shared.Models
{
    public class AppResponse<T>
    {
        [JsonPropertyOrder(1)]
        public string Code { get; set; } = null!;

        [JsonPropertyOrder(2)]
        public string? Message { get; set; }

        [JsonPropertyOrder(3)]
        public T? Data { get; set; }

        public AppResponse()
        { }

        public AppResponse(string code, string? msg)
        {
            Code = code;
            Message = msg;
        }

        public AppResponse(T? data)
        {
            Code = ErrorCode.Success;
            Data = data;
        }

        public bool EnsureSuccess()
        {
            return Code == ErrorCode.Success;
        }

        public AppResponse<T> SetData(T? data)
        {
            Data = data;
            return this;
        }
    }

    public static class Result
    {
        public static AppResponse<bool> Ok(string? msg = default)
        {
            return new AppResponse<bool>
            {
                Code = ErrorCode.Success,
                Message = msg,
                Data = true
            };
        }

        public static AppResponse<bool> Fail(string? msg = default)
        {
            return new AppResponse<bool>
            {
                Code = ErrorCode.Fail,
                Message = msg,
                Data = false
            };
        }

        public static AppResponse<T> Data<T>(T? data)
        {
            return new AppResponse<T>(data);
        }
    }
}