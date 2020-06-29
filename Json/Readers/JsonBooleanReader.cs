using Bolt.Models;
using System.Linq;
using System.IO;

namespace Bolt.Readers {
  internal class JsonBooleanReader : JsonReader {
    private static readonly char[] false_chars = new char[] { 'f', 'a', 'l', 's', 'e' };
    private static readonly char[] true_chars = new char[] { 't', 'r', 'u', 'e' }; 

    public JsonBooleanReader(StringReader json) : base(json) { }

    protected override IJsonValue ReadValue() {
      if(this._json.Peek() == 't') {
        char[] buffer = new char[4];
        int readCount = this._json.Read(buffer, 0, 4);
        if(readCount == 4 && buffer.SequenceEqual(true_chars)) {
          return new JsonBoolean(true);
        } else {
          throw new JsonFormatException("Expected to read 'true'");
        }
      } else {
        char[] buffer = new char[5];
        int readCount = this._json.Read(buffer, 0, 5);
        if(readCount == 5 && buffer.SequenceEqual(false_chars)) {
          return new JsonBoolean(false);
        } else {
          throw new JsonFormatException("Expected to read 'false'");
        }
      }
    }
  }
}
