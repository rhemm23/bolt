using Bolt.Models;
using System.IO;

namespace Bolt.Readers {
  internal class JsonObjectReader : JsonReader {
    public JsonObjectReader(StringReader json) : base(json) { }

    private enum States {
      Start,
      ReadOpeningBracket,
      ReadKeyValuePair,
      End
    }

    protected override IJsonValue ReadValue() {
      JsonReaderDirector director = new JsonReaderDirector(this._json);
      JsonStringReader stringReader = new JsonStringReader(this._json);
      JsonObject result = new JsonObject();
      States state = States.Start;

      while(state != States.End) {
        switch(state) {
          case States.Start:
            switch(this._json.Peek()) {
              case 123:
                this._json.Read();
                state = States.ReadOpeningBracket;
                break;

              default:
                throw new JsonFormatException("Expected '{' to start an object");
            }
            break;

          case States.ReadOpeningBracket:
            ReadWhitespace();
            switch(this._json.Peek()) {
              case 125:
                this._json.Read();
                state = States.End;
                break;

              default:
                ReadKeyValuePair(stringReader, director, result);
                state = States.ReadKeyValuePair;
                break;
            }
            break;

          case States.ReadKeyValuePair:
            switch(this._json.Peek()) {
              case 125:
                this._json.Read();
                state = States.End;
                break;

              case 44:
                this._json.Read();
                ReadKeyValuePair(stringReader, director, result);
                break;

              default:
                throw new JsonFormatException("Expected a ',' or '}' after key value pair in object");
            }
            break;

          case States.End:
            break;
        }
      }
      return result;
    }

    private void ReadKeyValuePair(JsonStringReader keyReader, JsonReaderDirector director, JsonObject result) {
      JsonString key = (JsonString)keyReader.Read();

      // Assure colon between key/value
      if(this._json.Read() != 58) {
        throw new JsonFormatException("Expected ':' after object key");
      }

      result.Add(key.Value, director.ReadValue());
    }
  }
}
