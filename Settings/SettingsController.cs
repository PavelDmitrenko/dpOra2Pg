using System;
using System.IO;

namespace dpOra2Pg
{
	public class SettingsController
	{
		public SettingsModel Settings { get; }

		public SettingsController(string configName)
		{
			string configFileWithExt = configName.EndsWith(".json", StringComparison.InvariantCultureIgnoreCase)
				? configName
				: $"{configName}.json";

			string fileName = Path.Combine(Directory.GetCurrentDirectory(), configFileWithExt);
			
			Settings = new SettingsModel();

			string fileContents = File.ReadAllText(fileName);
			Settings = Newtonsoft.Json.JsonConvert.DeserializeObject<SettingsModel>(fileContents);

		}

		public void ReadSettings()
		{

		}
	}
}
