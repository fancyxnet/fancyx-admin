using System.Collections.Concurrent;
using System.Diagnostics;

namespace Fancyx.Logger
{
    public class LogRecordContext
    {
        private static readonly ConcurrentDictionary<string, ConcurrentDictionary<string, object>> KeyValues = [];
        private static readonly Lock _lockObj = new();

        private static string? TraceId
        {
            get => Activity.Current?.TraceId.ToString();
            set { }
        }

        public static void Init()
        {
            if (!string.IsNullOrEmpty(TraceId))
            {
                KeyValues.AddOrUpdate(TraceId, new ConcurrentDictionary<string, object>(), (key, oldValue) => []);
            }
        }

        public static void PutVariable(string name, object value)
        {
            if (string.IsNullOrEmpty(TraceId)) return;

            lock (_lockObj)
            {
                KeyValues[TraceId].AddOrUpdate(name, value, (key, oldValue) => value);
            }
        }

        public static ConcurrentDictionary<string, object> GetVariables()
        {
            if (string.IsNullOrEmpty(TraceId)) return [];

            return KeyValues.TryGetValue(TraceId, out var data) ? data : [];
        }

        public static void Dispose()
        {
            if (string.IsNullOrEmpty(TraceId)) return;

            KeyValues.TryRemove(TraceId, out _);
            TraceId = null;
        }
    }
}