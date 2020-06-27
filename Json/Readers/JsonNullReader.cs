using Bolt.Models;
using System.Linq;

namespace Bolt.Readers {
  internal class JsonNullReader : JsonReader {
    private static readonly char[] null_chars = new char[] { 'n', 'u', 'l', 'l' };

    public JsonNullReader(StringReader json) : base(json) { }

    public override IJsonValue Read() {
      char[] buffer = new char[4];
      int readCount = this._json.Read(buffer, 0, 4);
      if(readCount == 4 && buffer.SequenceEqual(null_chars)) {
        return new JsonNull();
      } else {
        throw new JsonFormatException("Expected to read 'null'");
      }
    }
  }
}
