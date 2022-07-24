namespace Structing.Idempotent.Models
{
    public abstract class IdempotentBase
    {
        public IdempotentStatus Status { get; set; }
    }
}
