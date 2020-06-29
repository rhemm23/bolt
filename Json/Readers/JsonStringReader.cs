using System.Runtime.CompilerServices;
using System.Text;
using Bolt.Models;
using System.IO;

[assembly: InternalsVisibleTo("Test")]
namespace Bolt.Readers {
  internal class JsonStringReader : JsonReader {
    public JsonStringReader(StringReader json) : base(json) { }

    private enum States {
      Start,
      ReadingCharacters,
      ReadEscapeCharacter,
      End
    }

    public override IJsonValue Read() {
      StringBuilder sb = new StringBuilder();
      States state = States.Start;

      while(state != States.End) {
        switch(state) {
          case States.Start:
            switch(this._json.Peek()) {
              case 34:
                this._json.Read();
                state = States.ReadingCharacters;
                break;

              default:
                throw new JsonFormatException("Expected '\"' to start string");
            }
            break;

          case States.ReadingCharacters:
            switch(this._json.Peek()) {
              case 92:
                this._json.Read();
                state = States.ReadEscapeCharacter;
                break;

              case 34:
                this._json.Read();
                state = States.End;
                break;

              default:
                sb.Append((char)this._json.Read());
                break;
            }
            break;

          case States.ReadEscapeCharacter:
            switch(this._json.Peek()) {
              case 34:
              case 47:
              case 92:
                sb.Append((char)this._json.Read());
                state = States.ReadingCharacters;
                break;

              case 98:
                this._json.Read();
                sb.Append((char)8);
                state = States.ReadingCharacters;
                break;

              case 102:
                this._json.Read();
                sb.Append((char)12);
                state = States.ReadingCharacters;
                break;

              case 110:
                this._json.Read();
                sb.Append((char)10);
                state = States.ReadingCharacters;
                break;

              case 114:
                this._json.Read();
                sb.Append((char)13);
                state = States.ReadingCharacters;
                break;

              case 116:
                this._json.Read();
                sb.Append((char)9);
                state = States.ReadingCharacters;
                break;

              case 117:
                this._json.Read();
                sb.Append(ReadUnicodeCharacter());
                state = States.ReadingCharacters;
                break;

              default:
                throw new JsonFormatException("Invalid escape sequence");
            }
            break;

          case States.End:
            break;
        }
      }
      return new JsonString(sb.ToString());
    }

    private char ReadUnicodeCharacter() {
      int res = 0;
      int mul = 4096;
      for(int i = 0; i < 4; i++) {
        int j = this._json.Read();
        if(j >= 48 && j <= 57) {
          res += mul * (j - 48); // Digits
        } else if(j >= 65 && j <= 70) {
          res += mul * (j - 55); // A-F
        } else if(j >= 97 && j <= 102) {
          res += mul * (j - 87); // a-f
        } else {
          throw new JsonFormatException("Invalid unicode escape sequence");
        }
        mul /= 16;
      }
      return (char)res;
    }
  }
}
