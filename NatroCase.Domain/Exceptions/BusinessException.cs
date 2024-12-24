namespace NatroCase.Domain.Exceptions;

public class BusinessException : BaseException
{
    public BusinessException()
    {
    }

    public BusinessException(string key, params KeyValuePair<string, string>[] param)
        : base(key, param)
    {
    }
}