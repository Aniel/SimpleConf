# Aniel.SimpleConf

This is a small Helper for consuming strong typed json config files from a path.

Available via [nuget](https://www.nuget.org/packages/Aniel.SimpleConf)

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

## Setting the file config root
You can set the config root
```csharp
Aniel.SimpleConf.Loader.ConfigRoot = "<path to folder with config files>";

//Make sure you set useConfigRoot
Loader.LoadFile<TestConfig>("TestConf.json", useConfigRoot: true);
```

## Overwriting config root with environment variables
If you set the environment variable `CONFIG_ROOT_PATH` you can overwrite the config root.

If you want to change the name of the environment variable you can use:
```csharp
Aniel.SimpleConf.SimpleConfConstants.EnvironmentConfigPathName = "<yor environment variable name>";
```
