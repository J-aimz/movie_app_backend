using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieApp.Api.Dto
{
    public class ApiResponse<T>
    {
        public string Message { get; set; } = "";
        public bool IsSuccess { get; set; } = true;
        public T? Data { get; set; }


        public static ApiResponse<T> Success(T data)
        {
            return new ApiResponse<T> { Message = "Success", IsSuccess = true, Data = data };
        }

        public static ApiResponse<T> Failed(T data, string message = "Failed")
        {
            return new ApiResponse<T> { Message = message, IsSuccess = false, Data = data };
        }
    }

    public class ApiResponse
    {
        public string Message { get; set; } = "";
        public bool IsSuccess { get; set; } = true;


        public static ApiResponse Success()
        {
            return new ApiResponse { Message = "Success", IsSuccess = true};
        }

        public static ApiResponse Failed(string message = "Failed")
        {
            return new ApiResponse { Message = message, IsSuccess = false };
        }
    }
}