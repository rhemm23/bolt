using Bolt.Models;
using System.IO;

namespace Bolt.Readers {
  internal abstract class JsonReader {
    protected StringReader _json;

    public JsonReader(StringReader json) {
      this._json = json;
    }

    protected abstract IJsonValue ReadValue();

    public IJsonValue Read() {
      ReadWhitespace();
      IJsonValue result = ReadValue();
      ReadWhitespace();
      return result;
    }

    protected virtual void ReadWhitespace() {
      while(char.IsWhiteSpace((char)this._json.Peek())) {
        this._json.Read();
      }
    }
  }
}
