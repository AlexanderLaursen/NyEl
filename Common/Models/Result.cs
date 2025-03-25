using Common.Enums;

namespace Common.Models
{
    public class Result<T>
    {
        public bool IsSuccess { get; set; }
        public T? Value { get; set; }
        public string? ErrorMessage { get; set; }
        public ResultStatus Status { get; set; }

        public Result(bool isSuccess, T? value, string? errorMessage, ResultStatus status)
        {
            IsSuccess = isSuccess;
            Value = value;
            ErrorMessage = errorMessage;
            Status = status;
        }

        public static Result<T> Success(T value)
            => new Result<T>(true, value, null, ResultStatus.Ok);
        public static Result<T> Failure(string errorMessage = "Unkown error occurred.", ResultStatus status = ResultStatus.Error)
            => new Result<T>(false, default, errorMessage, status);
        public static Result<T> NotFound(string errorMessage = "Resource not found.")
            => Failure(errorMessage, ResultStatus.NotFound);
        public static Result<T> InvalidInput(string errorMessage = "Invalid input data.")
            => Failure(errorMessage, ResultStatus.InvalidInput);
        public static Result<T> Unauthorized(string errorMessage = "Unauthorized.")
            => Failure(errorMessage, ResultStatus.Unauthorized);
    }
}