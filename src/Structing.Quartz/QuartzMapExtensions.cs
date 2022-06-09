namespace Quartz
{
    public static class QuartzMapExtensions
    {
        public static T GetValueOrDefault<T>(this JobDataMap map, string key)
        {
            if (TryGetValue<T>(map, key, out var v))
            {
                return v;
            }
            return default;
        }
        public static bool TryGetValue<T>(this JobDataMap map, string key, out T value)
        {
            if (map.TryGetValue(key, out var val) && val is T v)
            {
                value = v;
                return true;
            }
            value = default;
            return false;
        }
    }
}
