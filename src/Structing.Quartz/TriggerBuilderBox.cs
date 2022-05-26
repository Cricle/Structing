using Quartz;

namespace Structing.Quartz
{
    public struct TriggerBuilderBox
    {
        private bool isIgnore;

        public readonly TriggerBuilder Builder;

        public TriggerBuilderBox(TriggerBuilder builder)
        {
            this.isIgnore = false;
            this.Builder = builder ?? throw new System.ArgumentNullException(nameof(builder));
        }

        public bool IsIgnore => isIgnore;

        public void SetIgnore()
        {
            isIgnore = true;
        }
    }
}
