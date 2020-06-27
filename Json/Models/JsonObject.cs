using System.Collections.Generic;
using System.Reflection;
using System.Collections;
using Bolt.Schema;
using System.Text;
using System;

namespace Bolt.Models {
  internal class JsonObject : Dictionary<string, IJsonValue>, IJsonValue {
    public void WriteJson(StringBuilder json) {
      json.Append('{');

      bool first = true;
      foreach(KeyValuePair<string, IJsonValue> kvp in this) {
        if(!first) {
          json.Append(',');
        } else {
          first = false;
        }

        json.Append('"');
        json.Append(kvp.Key);
        json.Append("\":");
        kvp.Value.WriteJson(json);
      }
      json.Append('}');
    }

    public object BuildObject(Type type) {
      if(type.GetInterface(nameof(IDictionary)) != null) {
        Type[] generics = type.GetGenericArguments();
        if(generics[0] != typeof(string)) {
          throw new JsonSchemaException("Unable to create a dictionary with a key type that is not string");
        } else {
          IDictionary dictionary = (IDictionary)ObjectCreator.NewInstance(type);
          foreach(KeyValuePair<string, IJsonValue> kvp in this) {
            dictionary.Add(kvp.Key, kvp.Value.BuildObject(generics[1]));
          }
          return dictionary;
        }
      } else {
        object obj = ObjectCreator.NewInstance(type);
        JsonObjectSchema schema = JsonObjectSchemaCache.GetSchema(type);
        foreach(KeyValuePair<string, PropertyInfo> kvp in schema.JsonProperties) {
          if(this.ContainsKey(kvp.Key)) {
            kvp.Value.SetValue(obj, this[kvp.Key].BuildObject(kvp.Value.PropertyType));
          }
        }
        return obj;
      }
    }
  }
}
