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
      if(type.IsArray) {
        Type elementType = type.GetElementType();
        Array array = (Array)ObjectCreator.NewInstance(type, this.Count);
        for(int i = 0; i < this.Count; i++) {
          array.SetValue(this[i].BuildObject(elementType), i);
        }
        return array;
      } else if(type.GetInterface(nameof(IList)) != null) {
        IList collection = (IList)ObjectCreator.NewInstance(type);
        if(collection.IsFixedSize) {
          throw new JsonSchemaException("Unable to create a list with fixed size that is not an array");
        } else {
          Type[] generics = type.GetGenericArguments();
          if(generics.Length != 1) {
            throw new JsonSchemaException("Expected one generic argument for list type");
          }
          foreach(IJsonValue value in this) {
            collection.Add(value.BuildObject(generics[0]));
          }
          return collection;
        }
      } else {
        throw new JsonSchemaException($"Unable to create an object of type {type.FullName} from a json array");
      }
    }
  }
}
