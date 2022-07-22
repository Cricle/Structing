namespace Structing.Idempotent.Models
{
    public enum IdempotentStatus
    {
        Skip = 0,
        MethodHit = 1,
        IdempotentHit = 2,
    }
}
