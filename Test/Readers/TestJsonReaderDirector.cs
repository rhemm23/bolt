using Microsoft.VisualStudio.TestTools.UnitTesting;
using Bolt.Readers;
using Bolt.Models;
using System.IO;

namespace Test.Readers {
  [TestClass]
  public class TestJsonReaderDirector {
    [TestMethod]
    public void TestWhitespace() {
      const string json = "    {  \"test_key\"    :   null }      \n \r \t  \f     ";
      JsonReaderDirector director = new JsonReaderDirector(new StringReader(json));
      JsonObject obj = (JsonObject)director.ReadValue();

      // Assert key
      IJsonValue val;
      if(!obj.TryGetValue("test_key", out val)) {
        Assert.Fail("Expected key in JsonObject named 'test_key'");
      }

      // Assure value
      if(!(val is JsonNull)) {
        Assert.Fail("Expected value for key 'test_key' in JsonObject to be null");
      }
    }
  }
}
