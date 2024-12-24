using NatroCase.Domain.Exceptions;

namespace NatroCase.Application.Common.Models;

public class ErrorResponse
{
    public string Key { get; set; }

    public Dictionary<string, string> Params { get; set; }

    public ErrorResponse(BaseException ex)
    {
        this.Key = ex.Key;
        this.Params = ex.Params;
    }

    public ErrorResponse(string key, params KeyValuePair<string, string>[] param)
    {
        this.Key = key;
        this.Params = param.ToDictionary((Func<KeyValuePair<string, string>, string>) (p => p.Key), (Func<KeyValuePair<string, string>, string>) (p => p.Value));
    }
}