using System;
using Xunit;

namespace Aniel.SimpleConf.Test
{
	public class Tests
	{
		[Fact]
		public void ShouldLoadConf()
		{
			Environment.SetEnvironmentVariable(SimpleConfConstants.EnvironmentConfigPathName, Environment.CurrentDirectory);
			var conf = Loader.LoadFile<TestConfig>("TestConf.json", true, true);
			Assert.Equal("Value1", conf.TestValue1);
			Assert.Equal("SubFileValue", conf.subConfig.SubFileValue);
		}
	}
}
