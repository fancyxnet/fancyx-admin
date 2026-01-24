using System.Collections.Concurrent;
using System.Diagnostics;

namespace Fancyx.Shared.Logger
{
    public class LogRecordContext
    {
        private static readonly ConcurrentDictionary<string, ConcurrentDictionary<string, string>> KeyValues = [];

        private static string? TraceId
        {
            get => Activity.Current?.TraceId.ToString();
            set { }
        }

        public static void Init()
        {
            if (!string.IsNullOrEmpty(TraceId))
            {
                KeyValues.AddOrUpdate(TraceId, new ConcurrentDictionary<string, string>(), (key, oldValue) => []);
            }
        }

        public static void PutVariable(string name, string value)
        {
            if (string.IsNullOrEmpty(TraceId)) return;

            if (!KeyValues.TryGetValue(TraceId, out ConcurrentDictionary<string, string>? single)) return;
            single.AddOrUpdate(name, value, (key, oldValue) => value);
        }

        public static ConcurrentDictionary<string, string> GetVariables()
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