﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Bolt.Attributes;
using System.Linq;
using Bolt;

namespace Test {
  [TestClass]
  public class TestJson {
    [TestMethod]
    public void TestDeserialization() {
      string json = "  { \"test_key\" \n : [ null,  \"test_value\" ] , \"test_key_1\" : null } ";

      TestJsonObject obj = Json.Deserialize<TestJsonObject>(json);
      Assert.IsTrue(obj.Property.SequenceEqual(new string[] { null, "test_value" }));
      Assert.IsNull(obj.Property1);
    }
  }

  [JsonObject]
  public class TestJsonObject {
    [JsonProperty("test_key")]
    public string[] Property { get; set; }

    [JsonProperty("test_key_1")]
    public string Property1 { get; set; }
  }
}
