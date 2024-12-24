using TimeZoneConverter;

namespace NatroCase.Domain.Common;

public static class Clock
{
    private static bool _isFrozen;
    private static DateTime? _dateTime;
    private static readonly TimeZoneInfo _timeZoneInfo = TZConvert.GetTimeZoneInfo("Turkey Standard Time");
    private static readonly DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

    public static void Freeze() => Clock._isFrozen = true;

    public static void Freeze(DateTime dateTime)
    {
        Clock._isFrozen = true;
        Clock._dateTime = new DateTime?(dateTime);
    }

    public static void Unfreeze()
    {
        Clock._isFrozen = false;
        Clock._dateTime = new DateTime?();
    }

    public static DateTime Now
    {
        get
        {
            DateTime now = DateTime.Now;
            if (!Clock._isFrozen)
                return now;
            if (!Clock._dateTime.HasValue)
                Clock._dateTime = new DateTime?(now);
            return Clock._dateTime.Value;
        }
    }

    public static DateTime UtcNow
    {
        get
        {
            DateTime utcNow = DateTime.UtcNow;
            if (!Clock._isFrozen)
                return utcNow;
            if (!Clock._dateTime.HasValue)
                Clock._dateTime = new DateTime?(utcNow);
            return Clock._dateTime.Value;
        }
    }

    public static void SetTime(DateTime dateTime) => Clock._dateTime = new DateTime?(dateTime);

    public static long ToTimestamp(this DateTime dateTime)
    {
        return ((DateTimeOffset) dateTime).ToUnixTimeMilliseconds();
    }

    public static DateTime FromTimeStamp(this long timeStamp)
    {
        return Clock.epoch.AddMilliseconds((double) timeStamp);
    }
}