using System.Text;
using System;

namespace Bolt.Models {
  internal class JsonString : IJsonValue {

    public string Value { get; }

    public JsonString(string value) {
      this.Value = value;
    }

    public void WriteJson(StringBuilder json) {
      json.Append('"');
      json.Append(this.Value);
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
