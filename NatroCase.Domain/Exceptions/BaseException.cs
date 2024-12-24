using System.Runtime.CompilerServices;

namespace NatroCase.Domain.Exceptions;

public class BaseException : Exception
{
    public string Key { get; set; }
    public Dictionary<string, string> Params { get; set; } = new Dictionary<string, string>();
    
    public BaseException()
    {
    }

    public BaseException(string key, params KeyValuePair<string, string>[] param)
    {
        this.Key = key;
        this.Params = param.ToDictionary((Func<KeyValuePair<string, string>, string>) (p => p.Key), (Func<KeyValuePair<string, string>, string>) (p => p.Value));
    }
    
    public override string Message => this.ToString();
    
    public override string ToString()
    {
        var interpolatedStringHandler1 = new DefaultInterpolatedStringHandler(17, 2);
        interpolatedStringHandler1.AppendLiteral("key: ");
        interpolatedStringHandler1.AppendFormatted(this.Key);
        interpolatedStringHandler1.AppendLiteral(", params: [");
        ref var local = ref interpolatedStringHandler1;
        var source = this.Params;
        var str = string.Join(", ", (source != null ? source.Select((Func<KeyValuePair<string, string>, string>) (p =>
        {
            var interpolatedStringHandler2 = new DefaultInterpolatedStringHandler(18, 2);
            interpolatedStringHandler2.AppendLiteral("{ key: ");
            interpolatedStringHandler2.AppendFormatted(p.Key);
            interpolatedStringHandler2.AppendLiteral(", value: ");
            interpolatedStringHandler2.AppendFormatted(p.Value);
            interpolatedStringHandler2.AppendLiteral(" }");
            return interpolatedStringHandler2.ToStringAndClear();
        })) : null) ?? new List<string>());
        
        local.AppendFormatted(str);
        interpolatedStringHandler1.AppendLiteral("]");
        return interpolatedStringHandler1.ToStringAndClear();
    }
}