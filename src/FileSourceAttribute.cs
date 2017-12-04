using System;

namespace Aniel.SimpleConf
{
	[AttributeUsage(System.AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
	public sealed class FileSourceAttribute : Attribute
	{
		private readonly string _path;

		public FileSourceAttribute(string path)
		{
			_path = path;
		}

		public string Path
		{
			get { return _path; }
		}

		public bool ThrowOnMissingFile { get; set; } = true;

		public bool UseEnvironmentConfigBasePath { get; set; } = true;
	}
}