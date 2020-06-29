using System.Text;
using System;
using System.Threading;
using System.Security.Cryptography.X509Certificates;

namespace Bolt.Models {
  internal class JsonString : IJsonValue {

    public string Value { get; }

    public JsonString(string value) {
      this.Value = value;
    }

    public void WriteJson(StringBuilder json) {
      json.Append('"');
      foreach(char c in this.Value) {
        switch((int)c) {
          case 34:
            json.Append('\\');
            json.Append('\"');
            break;

          case 92:
            json.Append("\\\\");
            break;

          case 47:
            json.Append("\\/");
            break;

          case 8:
            json.Append("\\b");
            break;

          case 12:
            json.Append("\\f");
            break;

          case 10:
            json.Append("\\n");
            break;

          case 13:
            json.Append("\\r");
            break;

          case 9:
            json.Append("\\t");
            break;

          case int i when(i > 255):
            json.Append("\\u");
            json.Append(i.ToString("X4"));
            break;

          default:
            json.Append(c);
            break;
        }
      }
      json.Append('"');
    }

    public object BuildObject(Type type) {
      if(type == typeof(string)) {
        return this.Value;
      } else {
        throw new JsonSchemaException($"Unable to create an object of type {type.FullName} from a json string");
      }
    }
  }
}
