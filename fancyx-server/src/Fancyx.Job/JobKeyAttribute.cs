namespace Fancyx.Job
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class JobKeyAttribute : Attribute
    {
        public string? Key { get; init; }

        public JobKeyAttribute(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentNullException(nameof(key));
            }
            this.Key = key;
        }
    }
}