using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text;
using Bolt.Models;

namespace Test.Models {
  [TestClass]
  public class TestJsonString {
    [TestMethod]
    public void TestWriteJsonWithUnicodeCharacter() {
      StringBuilder sb = new StringBuilder();
      JsonString js = new JsonString("\uFAFA");
      js.WriteJson(sb);
      Assert.AreEqual("\"\\uFAFA\"", sb.ToString());
    }
  }
}
