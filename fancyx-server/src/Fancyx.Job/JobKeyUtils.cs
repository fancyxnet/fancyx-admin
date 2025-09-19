namespace Fancyx.Job
{
    internal static class JobKeyUtils
    {
        public static string GetTriggerKey(string key)
        {
            return $"trigger_key_{key}";
        }

        public static string GetJobKey(string key)
        {
            return $"job_key_{key}";
        }

        public static string GetPureJobKey(string jobName)
        {
            var arr = jobName.Split('_');
            return arr.Length > 0 ? arr.Last() : "";
        }
    }
}