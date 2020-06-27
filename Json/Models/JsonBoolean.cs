using System.Text;
using System;

namespace Bolt.Models {
  internal class JsonBoolean : IJsonValue {

    public bool Value { get; }

    public JsonBoolean(bool value) {
      this.Value = value;
    }

    public void WriteJson(StringBuilder json) {
      json.Append(this.Value ? "true" : "false");
    }

    public object BuildObject(Type type) {
      if(type == typeof(bool)) {
        return this.Value;
      } else {
        throw new JsonSchemaException($"Unable to create an object of type {type.FullName} from a json boolean");
      }
    }
  }
}
