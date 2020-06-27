using System;

namespace Bolt.Attributes {
  [AttributeUsage(AttributeTargets.Property)]
  public class JsonPropertyAttribute : Attribute {

    public string Name { get; }

    public JsonPropertyAttribute() { }

    public JsonPropertyAttribute(string name) {
      this.Name = name;
    }
  }
}
