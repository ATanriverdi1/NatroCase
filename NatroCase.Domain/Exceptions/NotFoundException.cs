namespace NatroCase.Domain.Exceptions;

public class NotFoundException(string key, params KeyValuePair<string, string>[] param) : BaseException(key, param)
{
    public NotFoundException() : this("NOT_FOUND")
    {
    }
}