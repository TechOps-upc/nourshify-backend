namespace Nourishify.API.Shared.Domain.Services.Communication;

public class BaseResponse<TEntity>
{
    protected BaseResponse(string message)
    {
        Success = false;
        Message = message;
        Model = default;
    }

    protected BaseResponse(TEntity model)
    { 
        Success = true;
        Message = string.Empty; 
        Model = model;
    }

    public bool Success { get; set; }
    public string Message { get; set; }
    public TEntity Model { get; set; }
}