using Bolt.Models;

namespace Bolt.Readers {
  internal class JsonReaderDirector {
    private StringReader _json;

    public JsonReaderDirector(StringReader json) {
      this._json = json;
    }

    public IJsonValue ReadValue() {
      JsonReader reader;
      switch(this._json.Peek()) {
        case int digit when(digit >= 48 && digit <= 57):
        case 45:
          reader = new JsonNumberReader(this._json);
          break;

        case 34:
          reader = new JsonStringReader(this._json);
          break;

        case 116:
        case 102:
          reader = new JsonBooleanReader(this._json);
          break;

        case 110:
          reader = new JsonNullReader(this._json);
          break;

        case 91:
          reader = new JsonArrayReader(this._json);
          break;

        case 123:
          reader = new JsonObjectReader(this._json);
          break;

        default:
          throw new JsonFormatException("Expected to read a value");
      }
      return reader.Read();
    }
  }
}
