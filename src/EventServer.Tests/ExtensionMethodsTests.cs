using System;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EventServer.Tests
{
    namespace ExtensionMethodsTests
    {
        [TestClass]
        public class SerializationTests
        {
            private readonly string _simpleObjectXml = @"<?xml version=""1.0""?>
<SimpleObject xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">
  <A>a property</A>
  <B>b property</B>
</SimpleObject>";

            [TestMethod]
            public void Can_serialize_simple_object()
            {
                var bytes = new SimpleObject {A = "a property", B = "b property"}.SerializeXml();
                Assert.AreEqual(_simpleObjectXml, new ASCIIEncoding().GetString(bytes));
            }

            [TestMethod]
            public void Can_deserialzie_simple_object()
            {
                var simpleObject = new ASCIIEncoding().GetBytes(_simpleObjectXml).DeserializeXml<SimpleObject>();

                Assert.IsNotNull(simpleObject);
                Assert.AreEqual("a property", simpleObject.A);
                Assert.AreEqual("b property", simpleObject.B);
            }

            [TestMethod]
            public void Can_deserialize_into_object_with_new_properties()
            {
                var xml_with_missing_property = _simpleObjectXml.Replace("SimpleObject", "SimpleObject2");
                var simpleObject2 = new ASCIIEncoding().GetBytes(xml_with_missing_property).DeserializeXml<SimpleObject2>();

                Assert.IsNotNull(simpleObject2);
                Assert.AreEqual("a property", simpleObject2.A);
                Assert.AreEqual("b property", simpleObject2.B);
                Assert.IsNull(simpleObject2.C);
            }

            [TestMethod]
            public void Can_deserialize_into_object_with_missing_properties()
            {
                var xml_with_extra_property = _simpleObjectXml.Replace("SimpleObject", "SimpleObject3");
                var simpleObject3 = new ASCIIEncoding().GetBytes(xml_with_extra_property).DeserializeXml<SimpleObject3>();

                Assert.IsNotNull(simpleObject3);
                Assert.AreEqual("a property", simpleObject3.A);
            }

            public class SimpleObject
            {
                public string A { get; set; }
                public string B { get; set; }
            }

            public class SimpleObject2
            {
                public string A { get; set; }
                public string B { get; set; }
                public string C { get; set; }
            }

            public class SimpleObject3
            {
                public string A { get; set; }
            }
        }
    }
}