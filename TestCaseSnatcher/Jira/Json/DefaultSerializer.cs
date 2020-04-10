using Flurl.Http.Configuration;
using System.IO;
using System.Text.Json;

namespace TestCaseSnatcher.Jira.Json
{
	internal class DefaultSerializer : ISerializer
	{
		private readonly JsonSerializerOptions? _options;

		public DefaultSerializer(JsonSerializerOptions? options = null)
		{
			_options = options;
		}

		public T? Deserialize<T>(string s)
			=> JsonSerializer.Deserialize<T>(s, _options);

		public T? Deserialize<T>(Stream stream)
			=> JsonSerializer.DeserializeAsync<T>(stream, _options).Result;

		public string Serialize(object obj)
			=> JsonSerializer.Serialize(obj, _options);
	}
}