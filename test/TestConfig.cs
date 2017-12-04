namespace Aniel.SimpleConf.Test
{
	public class TestConfig
	{
		public string TestValue1 { get; set; }

		[FileSource("SubFile.json")]
		public TestSubFileConfig subConfig { get; set; }
	}
}