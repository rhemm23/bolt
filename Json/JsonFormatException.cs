using System;

namespace Bolt {
  public class JsonFormatException : Exception {
    public JsonFormatException(string message) : base(message) { }
  }
}
