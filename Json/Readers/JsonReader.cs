﻿using Bolt.Models;
using System.IO;

namespace Bolt.Readers {
  internal abstract class JsonReader {
    protected StringReader _json;

    public JsonReader(StringReader json) {
      this._json = json;
    }

    public abstract IJsonValue Read();
  }
}
