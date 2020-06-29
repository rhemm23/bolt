using Microsoft.VisualStudio.TestTools.UnitTesting;
using Bolt.Readers;
using Bolt.Models;
using System.IO;
using Bolt;

namespace Test.Readers {
  [TestClass]
  public class TestJsonStringReader {
    [TestMethod]
    public void TestUnicodeCharacterEscaped() {
      JsonStringReader reader = new JsonStringReader(new StringReader("\"\\uFAFA\""));
      JsonString val = (JsonString)reader.Read();

      // Assure unicode is escaped
      Assert.AreEqual(val.Value, "\uFAFA");
    }

    [TestMethod]
    [ExpectedException(typeof(JsonFormatException), "An unicode character with an invalid format was read without an exception")]
    public void TestUnicodeCharacterFormat() {
      StringReader sr = new StringReader("\"\\u00\"");
      JsonStringReader reader = new JsonStringReader(sr);
      reader.Read();
    }
  }
}
