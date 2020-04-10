using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Microsoft.Extensions.Options;

namespace Rajven.PersistableOptions
{
	/// <summary>
	/// An Options implementation that can write back config updates into the config file.
	/// </summary>
	/// <typeparam name="T">The type of config object.</typeparam>
	public class PersistableOptions<T> : IPersistableOptions<T>
		where T : class, new()
	{
		private readonly IOptionsMonitor<T> _optionsMonitor;
		private readonly string _filePath;
		private readonly string _section;

		/// <summary>
		/// Initializes and returns a new PersisableOptions with the specified monitor, path
		/// to the options file and the name of the section to parse.
		/// </summary>
		/// <param name="optionsMonitor">The options monitor that monitors the file for changes.</param>
		/// <param name="filePath">The path to the options file.</param>
		/// <param name="section">The name of the section to parse/monitor.</param>
		public PersistableOptions(IOptionsMonitor<T> optionsMonitor,
			string filePath,
			string section)
		{
			_optionsMonitor = optionsMonitor;
			_filePath = filePath;
			_section = section;
		}

		/// <summary>
		/// Gets the option object.
		/// </summary>
		public T Value => _optionsMonitor.CurrentValue;

		/// <summary>
		/// Gets the specific option value by name.
		/// </summary>
		/// <param name="name">The name of the option to get.</param>
		/// <returns></returns>
		public T Get(string name)
			=> _optionsMonitor.Get(name);

		/// <summary>
		/// Persists a change to the options object in the options file.
		/// </summary>
		/// <param name="applyChanges"></param>
		public void Persist(Action<T> applyChanges)
		{
			var options = new JsonSerializerOptions
			{
				IgnoreReadOnlyProperties = true,
				WriteIndented = true
			};

			applyChanges(Value);

			var fileDictionary = JsonSerializer.Deserialize<Dictionary<string, object>>(File.ReadAllText(_filePath));
			var configDictionary = JsonSerializer.Deserialize<Dictionary<string, object>>(JsonSerializer.Serialize(Value, options));
			fileDictionary[_section] = configDictionary;
			File.WriteAllText(_filePath, JsonSerializer.Serialize(fileDictionary, options));
		}
	}
}