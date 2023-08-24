using Microsoft.VisualStudio.TestTools.UnitTesting;
using Structing.Core.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Structing.Core.Test.Annotations
{
    [TestClass]
    public class FeatureAttributeTest
    {
        [TestMethod]
        public void GivenNullKey_MustThrowArgumentNullException()
        {
            Assert.ThrowsException<ArgumentNullException>(() => new FeatureAttribute(null));
        }
        [TestMethod]
        public void InputMustEqualOutput()
        {
            var key = "aaa";
            var type = typeof(object);
            var extName = "ext";
            var attr=new FeatureAttribute(key) { Type = type, ExtensionName = extName };
            Assert.AreEqual(key, attr.Key);
            Assert.AreEqual(type, attr.Type);
            Assert.AreEqual(extName, attr.ExtensionName);
        }
    }
    [TestClass]
    public class FeatureRegisterAttributeTest
    {
        [TestMethod]
        public void InputMustEqualOutput()
        {
            var key = "aaa";
            var type = typeof(object);
            var attr = new FeatureRegisterAttribute() {Key=key, Type = type};
            Assert.AreEqual(key, attr.Key);
            Assert.AreEqual(type, attr.Type);
        }
    }
}
