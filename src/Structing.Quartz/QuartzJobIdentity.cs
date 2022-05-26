using Quartz;
using Quartz.Util;
using System;

namespace Structing.Quartz
{
    public class QuartzJobIdentity
    {
        public QuartzJobIdentity(string identity)
            : this(identity, Key<JobKey>.DefaultGroup)
        {

        }
        public QuartzJobIdentity(string identity, string group)
        {
            Identity = identity ?? throw new ArgumentNullException(nameof(identity));
            Group = group ?? throw new ArgumentNullException(nameof(group));
        }

        public string Identity { get; }

        public string Group { get; }

        public override bool Equals(object obj)
        {
            if (obj is QuartzJobIdentity identity)
            {
                return identity.Identity == Identity &&
                    Group == identity.Group;
            }
            return false;
        }
        public override int GetHashCode()
        {
            return Identity.GetHashCode() ^ Group.GetHashCode();
        }
        public override string ToString()
        {
            return $"{{Identity: {Identity}, Group: {Group}}}";
        }
    }

}
