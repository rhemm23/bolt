# How to use

Using the library is very simple

* First install the nuget package 'Bolt'
* Then add the 'JsonObject' attribute to any class that you want to model
* Then add the 'JsonProperty' attribute to any property that you want to be included in the model

For example:

```csharp
using Bolt.Attributes;

[JsonObject]
public class Dog {
  [JsonProperty]
  public string Name { get; set; }
  
  [JsonProperty("age")]
  public int Age { get; set; }
}
```

Any property without a 'JsonProperty' attribute will be ignored

You can either specify the object key in the attribute constructor, or pass nothing and leave it as the property name

Then you can use the following methods to deserialize/serialize json:

```csharp
using Bolt;

public class Program {
  public static void Main(string[] args) {
    Dog dog = new Dog() {
      Name = "Fido",
      Age = 3
    };
    
    // Serialize
    string json = Json.Serialize(dog);
    
    // Deserialize with generic
    dog = Json.Deserialize<Dog>(json);
    
    // Deserialize with type parameter
    dog = Json.Deserialize(typeof(Dog), json);
    
    // Try deserializing
    bool success = Json.TryDeserialize<Dog>(json, out dog);
    
    // Try serializing
    success = Json.TrySerialize(dog, out json);
  }
}
```
