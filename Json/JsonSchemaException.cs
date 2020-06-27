using System;

namespace Bolt {
  public class JsonSchemaException : Exception {
    public JsonSchemaException(string message) : base(message) { }
  }
}
