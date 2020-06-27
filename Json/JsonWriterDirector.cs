using System.Collections.Generic;
using System.Collections;
using System.Reflection;
using Bolt.Models;
using Bolt.Schema;
using System;

namespace Bolt {
  internal class JsonWriterDirector {
    private object _value;

    public JsonWriterDirector(object value) {
      this._value = value;
    }

    public IJsonValue BuildToken() {
      switch(this._value) {
        case null:
          return new JsonNull();

        case string s:
          return new JsonString(s);

        case bool b:
          return new JsonBoolean(b);

        case sbyte sb:
        case byte b:
        case ushort us:
        case short s:
        case uint ui:
        case int i:
        case ulong ul:
        case long l:
        case float f:
        case double d:
        case decimal dc:
          return new JsonNumber(this._value.ToString());
      }

      // Array or object
      if(this._value.GetType().GetInterface(nameof(IList)) != null) {
        JsonWriterDirector writer;
        JsonArray arr = new JsonArray();
        foreach(object obj in (IList)this._value) {
          writer = new JsonWriterDirector(obj);
          arr.Add(writer.BuildToken());
        }
        return arr;
      } else if(this._value.GetType().GetInterface(nameof(IDictionary)) != null) {
        JsonWriterDirector writer;
        JsonObject obj = new JsonObject();
        Type[] generics = this._value.GetType().GetGenericArguments();
        if(generics[0] != typeof(string)) {
          throw new JsonSchemaException("Can only serialize dictionaries with key type string");
        }
        foreach(KeyValuePair<object, object> kvp in (IDictionary)this._value) {
          writer = new JsonWriterDirector(kvp.Value);
          obj.Add((string)kvp.Key, writer.BuildToken());
        }
        return obj;
      } else {
        JsonWriterDirector writer;
        JsonObject obj = new JsonObject();
        JsonObjectSchema schema = JsonObjectSchemaCache.GetSchema(this._value.GetType());
        foreach(KeyValuePair<string, PropertyInfo> kvp in schema.JsonProperties) {
          writer = new JsonWriterDirector(kvp.Value.GetValue(this._value));
          obj.Add(kvp.Key, writer.BuildToken());
        }
        return obj;
      }
    }
  }
}
