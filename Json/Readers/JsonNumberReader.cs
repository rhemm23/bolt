using System.Text;
using Bolt.Models;
using System.IO;

namespace Bolt.Readers {
  internal class JsonNumberReader : JsonReader {
    public JsonNumberReader(StringReader json) : base(json) { }

    private enum States {
      Start,
      ReadSign,
      ReadingWholeDigits,
      ReadWholeDigits,
      ReadFractionIndicator,
      ReadingFractionDigits,
      ReadExponentIndicator,
      ReadExponentSign,
      ReadingExponentDigits,
      End
    }

    public override IJsonValue Read() {
      StringBuilder sb = new StringBuilder();
      States state = States.Start;

      while(state != States.End) {
        switch(state) {
          case States.Start:
            switch(this._json.Peek()) {
              case -1:
                throw new JsonFormatException("Expected to read number");

              case 45:
                sb.Append((char)this._json.Read());
                goto default;

              default:
                state = States.ReadSign;
                break;
            }
            break;

          case States.ReadSign:
            switch(this._json.Peek()) {
              case int digit when(digit >= 49 && digit <= 57):
                sb.Append((char)this._json.Read());
                state = States.ReadingWholeDigits;
                break;

              case 48:
                sb.Append((char)this._json.Read());
                state = States.ReadWholeDigits;
                break;

              default:
                throw new JsonFormatException("Expected to read digit");
            }
            break;

          case States.ReadingWholeDigits:
            switch(this._json.Peek()) {
              case int digit when(digit >= 48 && digit <= 57):
                sb.Append((char)this._json.Read());
                break;

              default:
                state = States.ReadWholeDigits;
                break;
            }
            break;

          case States.ReadWholeDigits:
            switch(this._json.Peek()) {
              case 46:
                sb.Append((char)this._json.Read());
                state = States.ReadFractionIndicator;
                break;

              case 69:
              case 101:
                sb.Append((char)this._json.Read());
                state = States.ReadExponentIndicator;
                break;

              default:
                state = States.End;
                break;
            }
            break;

          case States.ReadFractionIndicator:
            switch(this._json.Peek()) {
              case int digit when(digit >= 48 && digit <= 57):
                sb.Append((char)this._json.Read());
                state = States.ReadingFractionDigits;
                break;

              default:
                throw new JsonFormatException("Expected to read a digit after fraction indicator");
            }
            break;

          case States.ReadingFractionDigits:
            switch(this._json.Peek()) {
              case int digit when(digit >= 48 && digit <= 57):
                sb.Append((char)this._json.Read());
                break;

              case 69:
              case 101:
                sb.Append((char)this._json.Read());
                state = States.ReadExponentIndicator;
                break;

              default:
                state = States.End;
                break;
            }
            break;

          case States.ReadExponentIndicator:
            switch(this._json.Peek()) {
              case 43:
              case 45:
                sb.Append((char)this._json.Read());
                state = States.ReadExponentSign;
                break;

              case int digit when(digit >= 48 && digit <= 57):
                sb.Append((char)this._json.Read());
                state = States.ReadingExponentDigits;
                break;

              default:
                throw new JsonFormatException("Expected to read '-' or '+' or a digit after exponent indicator");
            }
            break;

          case States.ReadExponentSign:
            switch(this._json.Peek()) {
              case int digit when(digit >= 48 && digit <= 57):
                sb.Append((char)this._json.Read());
                state = States.ReadingExponentDigits;
                break;

              default:
                throw new JsonFormatException("Expected to read a digit after exponent sign");
            }
            break;

          case States.ReadingExponentDigits:
            switch(this._json.Peek()) {
              case int digit when(digit >= 48 && digit <= 57):
                sb.Append((char)this._json.Read());
                break;

              default:
                state = States.End;
                break;
            }
            break;

          case States.End:
            break;
        }
      }
      return new JsonNumber(sb.ToString());
    }
  }
}
