namespace RedisDeepDive.Application.Wrappers;

public class ServiceResponse
{
   
    public bool IsSuccess { get; set; }
    public string Message { get; set; }
    public int ErrorCode { get; set; }

    public static ServiceResponse Success()
    {
        return new ServiceResponse 
        { 
            IsSuccess = true, 
            Message = "İşlem başarılı." 
        };
    }

    public static ServiceResponse Failure(string message, int code = 0)
    {
        return new ServiceResponse 
        { 
            IsSuccess = false, 
            Message = message, 
            ErrorCode = code 
        };
    }
}