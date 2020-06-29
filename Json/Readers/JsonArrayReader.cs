using Bolt.Models;
using System.IO;

namespace Bolt.Readers {
  internal class JsonArrayReader : JsonReader {
    public JsonArrayReader(StringReader json) : base(json) { }

    private enum States {
      Start,
      ReadOpeningBracket,
      ReadValue,
      End
    }

    protected override IJsonValue ReadValue() {
      JsonReaderDirector director = new JsonReaderDirector(this._json);
      JsonArray result = new JsonArray();
      States state = States.Start;

      while(state != States.End) {
        switch(state) {
          case States.Start:
            switch(this._json.Peek()) {
              case 91:
                this._json.Read();
                state = States.ReadOpeningBracket;
                break;

              default:
                throw new JsonFormatException("Expected '[' to start an array");
            }
            break;

          case States.ReadOpeningBracket:
            ReadWhitespace();
            switch(this._json.Peek()) {
              case 93:
                this._json.Read();
                state = States.End;
                break;

              default:
                result.Add(director.ReadValue());
                state = States.ReadValue;
                break;
            }
            break;

          case States.ReadValue:
            switch(this._json.Peek()) {
              case 93:
                this._json.Read();
                state = States.End;
                break;

              case 44:
                this._json.Read();
                result.Add(director.ReadValue());
                break;

              default:
                throw new JsonFormatException("Expected a ',' or ']' after value in array");
            }
            break;

          case States.End:
            break;
        }
      }
      return result;
    }
  }
}
