namespace Structing.Idempotent.Models
{
    public abstract class IdempotentBase
    {
        public IdempotentStatus Status { get; set; }

        public string CacheKey { get; set; }

        public object[] Args { get; set; }

        public bool Skip { get; set; }
    }
}
