using System.Collections.Concurrent;
using System;

namespace Bolt.Schema {
  internal static class JsonObjectSchemaCache {
    private static readonly ConcurrentDictionary<Type, JsonObjectSchema> schema;

    static JsonObjectSchemaCache() {
      schema = new ConcurrentDictionary<Type, JsonObjectSchema>();
    }

    public static JsonObjectSchema GetSchema(Type type) {
      JsonObjectSchema objectSchema;
      if(!schema.TryGetValue(type, out objectSchema)) {
        JsonObjectSchemaBuilder builder = new JsonObjectSchemaBuilder(type);
        objectSchema = builder.Build();
        schema.TryAdd(type, objectSchema);
      }
      return objectSchema;
    }
  }
}
