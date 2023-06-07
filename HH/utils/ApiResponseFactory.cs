using System;
using System.Net;

namespace HH.utils;
 public class ApiResponse<T>
{
    public int StatusCode { get; set; }
    public string Message { get; set; }
    public T Data { get; set; }
}

public static class ApiResponseFactory
{
    public static ApiResponse<T> CreateSuccessResponse<T>(T data, string message = null)
    {
        return new ApiResponse<T>
        {
            StatusCode = (int)HttpStatusCode.OK,
            Message = message ?? "Success",
            Data = data
        };
    }

    public static ApiResponse<T> CreateErrorResponse<T>(string message, int statusCode = (int)HttpStatusCode.BadRequest)
    {
        return new ApiResponse<T>
        {
            StatusCode = statusCode,
            Message = message,
            Data = default(T)
        };
    }
}
