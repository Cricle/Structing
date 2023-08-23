﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Structing.Core.Annotations
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public sealed class FeatureRegisterAttribute : Attribute
    {
        public object Key { get; set; }

        public Type Type { get; set; }
    }
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public sealed class FeatureAttribute : Attribute
    {
        public FeatureAttribute(string Key)
        {
            this.Key = Key ?? throw new ArgumentNullException(nameof(Key));
        }

        public object Key { get; }
    }
}
