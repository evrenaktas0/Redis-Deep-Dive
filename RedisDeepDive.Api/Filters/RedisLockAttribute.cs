using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using RedisDeepDive.Application.Interfaces.Services;
using RedisDeepDive.Application.Wrappers;

namespace RedisDeepDive.Api;

[AttributeUsage(AttributeTargets.Method)]
public class RedisLockAttribute : Attribute, IAsyncActionFilter
{
    private readonly string _keyPrefix;

    public RedisLockAttribute(string keyPrefix)
    {
        _keyPrefix = keyPrefix;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var lockService = context.HttpContext.RequestServices.GetRequiredService<IRedisLockService>();

        var lockKey = GenerateLockKey(context);
        var token = Guid.NewGuid().ToString();

        var isLocked = await lockService.TryAcquireLock(lockKey, token, TimeSpan.FromSeconds(30)); 

        if (!isLocked)
        {
            var response =
                ServiceResponse.Failure("Sık aralıklarla istek yaptınız. Lütfen işlemin tamamlanmasını bekleyin", 429);
            context.Result = new JsonResult(response) 
            { 
                StatusCode = 429 
            };
            return;
        }

        try
        {
            await next();
        }
        finally
        {
            await lockService.ReleaseLock(lockKey, token);
        }
    }

    private string GenerateLockKey(ActionExecutingContext context)
    {
        var values = context.ActionArguments.Values;
        return $"{_keyPrefix}:{string.Join(":", values)}";
    }
    }
    