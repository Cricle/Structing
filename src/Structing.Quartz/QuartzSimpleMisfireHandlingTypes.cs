namespace Structing.Quartz
{
    public enum QuartzSimpleMisfireHandlingTypes
    {
        IgnoreMisfires = -1,
        FireNow = 1,
        NowWithExistingCount = 2,
        NowWithRemainingCount = 3,
        NextWithRemainingCount = 4,
        NextWithExistingCount = 5,
    }
}
