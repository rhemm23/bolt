using System;
using System.Diagnostics;
using Bolt;
using Bolt.Attributes;

namespace Test {
  class Program {
    static void Main(string[] args) {
      string json = "{     \"name\":   null    }";
      long val1, val2, val3, val4;

      Console.WriteLine(json);
      Stopwatch sw = Stopwatch.StartNew();

      Dog dog = Json.Deserialize<Dog>(json);
      val1 = sw.ElapsedTicks;

      Dog dog1 = Json.Deserialize<Dog>("{     \"name\":   \"fido\"    }");
      val2 = sw.ElapsedTicks;

      Dog dog2 = Json.Deserialize<Dog>("{     \"name\":   \"fido1\"    }");
      val3 = sw.ElapsedTicks;

      Dog dog3 = Json.Deserialize<Dog>("{     \"name\":   \"fido2\"    }");
      val4 = sw.ElapsedTicks;

      Console.WriteLine($"{val1} {val2} {val3} {val4}");
      Console.ReadKey();
    }
  }

  [JsonObject]
  class Dog {
    [JsonProperty("name")]
    public string Name { get; set; }
  }
}
