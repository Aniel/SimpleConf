using System;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Reflection;

namespace Aniel.SimpleConf
{
	public static class Loader
	{
		public static string ConfigRoot { get; set; } = AppContext.BaseDirectory;
		public static bool ThrowOnMissingEnvironmentVariable { get; set; } = false;

		/// <summary>
		/// When the Environment Variable is set ConfigBasePath will be overwritten
		/// </summary>
		/// <returns>Config base path</returns>
		public static string GetConfigBasePath()
		{
			var folderPath = Environment.GetEnvironmentVariable(SimpleConfConstants.EnvironmentConfigPathName);
			if (folderPath != null)
			{
				folderPath = ConfigRoot;
			}
			else if (ThrowOnMissingEnvironmentVariable)
			{
				throw new Exception("The environment variable " + SimpleConfConstants.EnvironmentConfigPathName + " is not set!");
			}
			if (!Directory.Exists(folderPath))
			{
				throw new Exception("The directory " + folderPath + " does not exist!");
			}
			return folderPath;
		}

		/// <summary>
		/// Generates a full file path with the ConfigBasePath and the file name
		/// </summary>
		/// <param name="fileName">The name of the file</param>
		/// <returns>A full qualified file path</returns>
		public static string CreateFullFilePath(string fileName) => Path.Combine(GetConfigBasePath(), fileName);

		/// <summary>
		/// Generates a full file path to a json file named after Assembly.GetEntryAssembly().GetName().Name
		/// </summary>
		/// <returns>A full qualified file path with the entry assembly name as json fille</returns>
		public static string GetConfigFilePath() => CreateFullFilePath(Assembly.GetEntryAssembly().GetName().Name + ".json");

		/// <summary>
		/// Loads a json file from the Type T with the name of the EntryAssembly
		/// </summary>
		/// <param name="throwOnMissingFile">Indicates if an exception is thrown when the file is not found. Default true</param>
		/// <returns>The file content as object</returns>
		public static T LoadFile<T>(bool throwOnMissingFile = true) where T : new() => LoadFile<T>(GetConfigFilePath(), throwOnMissingFile);

		/// <summary>
		/// Loads a file into an object
		/// </summary>
		/// <param name="file">The File to load</param>
		/// <param name="throwOnMissingFile">Indicates if an exception is thrown when the file is not found. Default true</param>
		/// <param name="useConfigRoot">Should the config root be used in file resolution. Default false</param>
		/// <returns>The file content as object</returns>
		public static T LoadFile<T>(string file, bool throwOnMissingFile = true, bool useConfigRoot = false) where T : new()
		{
			return (T)LoadFile(file, typeof(T), throwOnMissingFile, useConfigRoot);
		}

		/// <summary>
		/// Loads a file into a object
		/// </summary>
		/// <param name="file">The File to load</param>
		/// <param name="type">The object type to create</param>
		/// <param name="throwOnMissingFile">Indicates if an exception is thrown when the file is not found. Default true</param>
		/// <param name="useConfigRoot">Should the config root be used in file resolution. Default false</param>
		/// <returns></returns>
		public static object LoadFile(string file, Type type, bool throwOnMissingFile = true, bool useConfigRoot = false)
		{
			if (useConfigRoot)
			{
				file = CreateFullFilePath(file);
			}

			if (!File.Exists(file))
			{
				if (throwOnMissingFile)
				{
					throw new Exception("The Configuration file " + file + " does not exist. Did you forget creating it?");
				}
				return _loadDependencies(Activator.CreateInstance(type));
			}

			var option = new ConfigurationBuilder()
							.AddJsonFile(file)
							.Build()
							.Get(type);
			if (option == null)
				option = Activator.CreateInstance(type);
			return _loadDependencies(option);
		}

		private static object _loadDependencies(object options)
		{
			//See if a property has the FileSource attribute and load the file
			foreach (var property in options.GetType().GetProperties())
			{
				if (property.GetCustomAttribute(typeof(FileSourceAttribute)) is FileSourceAttribute attribute)
				{
					property.SetValue(options, LoadFile(
						attribute.Path,
						property.PropertyType,
						attribute.ThrowOnMissingFile,
						attribute.UseEnvironmentConfigBasePath));
				}
				else if (property.PropertyType != typeof(string) && property.PropertyType.GetConstructor(new Type[0]) != null)
				{
					if (property.GetValue(options) == null)
					{
						property.SetValue(options, Activator.CreateInstance(property.PropertyType));
					}
					_loadDependencies(property.GetValue(options));
				}

			}
			return options;
		}
	}
}
