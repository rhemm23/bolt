using System.Collections.Generic;
using System.Text;
using System;

namespace Bolt.Models {
  internal class JsonNumber : IJsonValue {
    private static readonly HashSet<Type> validNumericTypes = new HashSet<Type>() {
      typeof(sbyte),
      typeof(byte),
      typeof(ushort),
      typeof(short),
      typeof(uint),
      typeof(int),
      typeof(ulong),
      typeof(long),
      typeof(float),
      typeof(double),
      typeof(decimal)
    };

    public string StringValue { get; }

    public JsonNumber(string stringValue) {
      this.StringValue = stringValue;
    }

    public void WriteJson(StringBuilder json) {
      json.Append(this.StringValue);
    }

    public object BuildObject(Type type) {
      if(validNumericTypes.Contains(type)) {
        return Convert.ChangeType(this.StringValue, type);
      } else {
        throw new JsonSchemaException($"Unable to create object of type {type.FullName} from a json number");
      }
    }
  }
}
