using System.IO;
using System.Collections.Generic;
using Nikki.Core;



namespace Endscript.Profiles
{
	public class Underground2Profile : BaseProfile
	{
		public override GameINT GameINT => GameINT.Underground2;
		public override string GameSTR => GameINT.Underground2.ToString();
		public override string Directory { get; }
		public static string MainHashList { get; set; }
		public static string CustomHashList { get; set; }

		public Underground2Profile(string directory) : base() { this.Directory = directory; }

		public override void LoadHashList()
		{
			Map.ReloadBinKeys();
			Loader.LoadBinKeys(new string[] { MainHashList, CustomHashList });
		}
		public override void SaveHashList()
		{
			System.IO.Directory.CreateDirectory(Path.GetDirectoryName(CustomHashList));

			if (File.Exists(MainHashList))
			{

				var lines = File.ReadAllLines(MainHashList);
				var set = new HashSet<string>(lines.Length);

				foreach (var line in lines)
				{

					if (line.StartsWith("//") || line.StartsWith("#")) continue;
					else set.Add(line);

				}

				using var sw = new StreamWriter(File.Open(CustomHashList, FileMode.Create));

				foreach (var label in Map.BinKeys.Values)
				{

					if (!set.Contains(label)) sw.WriteLine(label);

				}

			}
			else
			{

				System.IO.Directory.CreateDirectory(Path.GetDirectoryName(CustomHashList));
				using var sw = new StreamWriter(File.Open(CustomHashList, FileMode.Create));
				foreach (var label in Map.BinKeys.Values) sw.WriteLine(label);
			}
		}
	}
}
