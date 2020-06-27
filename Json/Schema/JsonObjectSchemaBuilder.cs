using System.Collections.Generic;
using System.Reflection;
using Bolt.Attributes;
using System;

namespace Bolt.Schema {
  internal class JsonObjectSchemaBuilder {
    private Type _type;

    public JsonObjectSchemaBuilder(Type type) {
      this._type = type;
    }

    public JsonObjectSchema Build() {
      if(!Attribute.IsDefined(this._type, typeof(JsonObjectAttribute))) {
        throw new JsonSchemaException("Types used for json serializing or deserializing must have a 'JsonObject' attribute");
      } else {
        Dictionary<string, PropertyInfo> jsonProperties = new Dictionary<string, PropertyInfo>();
        foreach(PropertyInfo property in this._type.GetProperties()) {
          if(Attribute.IsDefined(property, typeof(JsonPropertyAttribute))) {
            JsonPropertyAttribute propertyAttribute = (JsonPropertyAttribute)property.GetCustomAttribute(typeof(JsonPropertyAttribute));
            jsonProperties[propertyAttribute.Name == null ? property.Name : propertyAttribute.Name] = property;
          }
        }
        return new JsonObjectSchema(this._type, jsonProperties);
      }
    }
  }
}
