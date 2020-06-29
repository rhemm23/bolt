using Bolt.Readers;
using System.Text;
using System.IO;
using System;

namespace Bolt {
  public static class Json {
    public static T Deserialize<T>(string json) {
      return (T)Deserialize(typeof(T), json);
    }

    public static object Deserialize(Type type, string json) {
      StringReader stringReader = new StringReader(json);
      JsonReaderDirector director = new JsonReaderDirector(stringReader);
      return director.ReadValue().BuildObject(type);
    }

    public static bool TryDeserialize<T>(string json, out T result) {
      object res;
      if(TryDeserialize(typeof(T), json, out res)) {
        result = (T)res;
        return true;
      } else {
        result = default;
        return false;
      }
    }

    public static bool TryDeserialize(Type type, string json, out object result) {
      try {
        result = Deserialize(type, json);
        return true;
      } catch(Exception e) {
        if(e is JsonSchemaException || e is JsonFormatException) {
          result = default;
          return false;
        } else {
          throw e;
        }
      }
    }

    public static string Serialize(object obj) {
      StringBuilder sb = new StringBuilder();
      JsonWriterDirector writer = new JsonWriterDirector(obj);
      writer.BuildToken().WriteJson(sb);
      return sb.ToString();
    }

    public static bool TrySerialize(object obj, out string json) {
      string result;
      if(TrySerialize(obj, out result)) {
        json = result;
        return true;
      } else {
        json = null;
        return false;
      }
    }
  }
}
