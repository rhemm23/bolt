using System.Collections.Generic;
using System.Collections;
using System.Text;
using System;

namespace Bolt.Models {
  internal class JsonArray : List<IJsonValue>, IJsonValue {
    public void WriteJson(StringBuilder json) {
      json.Append('[');

      bool first = true;
      foreach(IJsonValue value in this) {
        if(!first) {
          json.Append(',');
        } else {
          first = false;
        }

        value.WriteJson(json);
      }
      json.Append(']');
    }

    public object BuildObject(Type type) {
      if(type.GetInterface(nameof(IList)) != null) {
        Type innerType = type.GetGenericArguments()[0];
        IList collection = (IList)ObjectCreator.NewInstance(type);
        foreach(IJsonValue value in this) {
          collection.Add(value.BuildObject(innerType));
        }
        return collection;
      } else {
        throw new JsonSchemaException($"Unable to create an object of type {type.FullName} from a json array");
      }
    }
  }
}
