# Aniel.SimpleConf

This is a small Helper for consuming strong typed json config files from a path.

## Usage

```csharp
//TestConfig.cs
public class TestConfig
{
  public string TestValue1 { get; set; }
}
```

```json
//TestConfig.json
{
  "TestValue1": "Conf Test Value 2"
}
```

```csharp
var conf = Loader.LoadFile<TestConfig>("TestConf.json");
conf.TestValue1; //Conf Test Value 2
```

## Advanced Usage
It is possible to specify that parts of your config come from other files

```csharp
//TestConfig.cs
public class TestConfig
{
  public string TestValue1 { get; set; }

  [FileSource("SubFile.json")]
  public TestSubFileConfig subConfig { get; set; }
}
```

`TestConfig.json` stays the same

```json
//SubFile.json
{
  "SubFileValue": "SubFileValue Test value"
}
```

```csharp
var conf = Loader.LoadFile<TestConfig>("TestConf.json");
conf.TestValue1; //Conf Test Value 2
conf.subConfig.SubFileValue; //SubFileValue Test value
```