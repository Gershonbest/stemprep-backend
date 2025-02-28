using Serilog;

namespace Application.Common.Models;
public class Result
{
    public Result()
    {
    }

    public bool Succeeded { get; set; }
    public object Entity { get; set; }

    public string Error { get; set; }
    public string ExceptionError { get; set; }
    public string Message { get; set; }
    //public int Id { get; internal set; }

    internal Result(bool succeeded, string message, object entity = null, string exception = null)
    {
        Succeeded = succeeded;
        Message = message;
        ExceptionError = exception;
        Entity = entity;
    }


    internal Result(bool succeeded, object entity = default)
    {
        Succeeded = succeeded;
        Entity = entity;
    }

    public static Result Success(object entity)
    {
        Log.Information("Request executed successfully! Entity retrieved.", entity);
        return new Result(true, "Request was executed successfully!", entity);
    }

    public static Result Success(Type request, string message)
    {
        Log.Information("Request executed successfully!.", request, message);
        return new Result(true, message);
    }

    public static Result Success(object entity, string message, Type request)
    {
        Log.Information("Request executed successfully!.", entity, request, message);
        return new Result(true, message, entity);
    }

    public static Result Success(object entity, Type request)
    {
        Log.Information("Request executed successfully!.", entity, request);
        return new Result(true, entity);
    }

    public static Result Success<T>(string message)
    {
        Log.Information("Request executed successfully!.", message);
        return new Result(true, message);
    }

    public static Result Success<T>(T request, string message)
    {
        Log.Information("Request executed successfully!. Request {request}", message, request);
        return new Result(true, message);
    }

    public static Result Success<T>(string message, object entity, T request)
    {
        Log.Information("Request executed successfully!.", entity, message, request);
        return new Result(true, message, entity);
    }

    public static Result Success<T>(object entity, T request)
    {
        Log.Information("Request executed successfully!.", entity, request);
        return new Result(true, entity);
    }

 
    public static Result Success<T>( string message, object entity)
    {
        Log.Information("Request executed successfully!.", entity, message);
        return new Result(true, message, entity);
    }

    public static Result Success<T>(object entity, string message, T request)
    {
        Log.Information("Request executed successfully! Entity retrieved.");
        return new Result(true, message, entity);
    }

    public static Result Failure(string error)
    {
        Log.Error("Failure in request - Error: {Message}", error);
        return new Result(false, error);
    }
    public static Result Failure<T>(string error)
    {
        Log.Error("Failure in request  - Error: {error}", error);
        return new Result(false, error);
    }

    public static Result Failure<T>(T request, string error)
    {
        Log.Error("Failure in request - Error: {error}, Request: {request}", error, request);
        return new Result(false, error);
    }

    public static Result Failure(string prefixMessage, Exception ex)
    {
        Log.Error("Failure in request - Error: {prefixMessage}, Exception: {ex}", prefixMessage, ex);
        return new Result(false, $"{prefixMessage}");
    }

    public static Result Failure<T>(object entity)
    {
        Log.Error("Failure in request - Entity: {entity}");
        return new Result(false, entity);
    }

}


