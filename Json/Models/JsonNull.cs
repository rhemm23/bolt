using System;
using System.Text;

namespace Bolt.Models {
  internal class JsonNull : IJsonValue {
    public void WriteJson(StringBuilder json) {
      json.Append("null");
    }

    public object BuildObject(Type type) {
      return null;
    }
  }
}
