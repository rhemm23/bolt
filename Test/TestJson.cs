using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Bolt.Attributes;
using System.Linq;
using Bolt;

namespace Test {
  [TestClass]
  public class TestJson {
    [TestMethod]
    public void TestDeserialization() {
      string json = "  { \"test_key\" \n : [ null,  \"test_value\" ] } ";
      TestJsonObject obj = Json.Deserialize<TestJsonObject>(json);
      Assert.IsTrue(obj.Property.SequenceEqual(new string[] { null, "test_value" }));
    }
  }

  [JsonObject]
  public class TestJsonObject {
    [JsonProperty("test_key")]
    public string[] Property { get; set; }
  }
}
