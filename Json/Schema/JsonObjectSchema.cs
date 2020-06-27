using System.Collections.Generic;
using System.Reflection;
using System;

namespace Bolt.Schema {
  internal class JsonObjectSchema {
    public Dictionary<string, PropertyInfo> JsonProperties { get; }
    public Type Type { get; }

    public JsonObjectSchema(Type type, Dictionary<string, PropertyInfo> jsonProperties) {
      this.JsonProperties = jsonProperties;
      this.Type = type;
    }
  }
}
