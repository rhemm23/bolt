using System.Text;
using System;

namespace Bolt.Models {
  internal interface IJsonValue {
    void WriteJson(StringBuilder json);
    object BuildObject(Type type);
  }
}
