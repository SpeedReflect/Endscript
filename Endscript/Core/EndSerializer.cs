using System;
using System.IO;
using Endscript.Profiles;
using Nikki.Reflection.Abstract;
using Nikki.Reflection.Interface;



namespace Endscript.Core
{
	public class EndSerializer
	{
		private readonly BaseProfile _profile;
		private readonly string _directory;
		private readonly string _mainfile;

		public EndSerializer(BaseProfile profile, string filename)
		{
			this._profile = profile;
			this._mainfile = filename;
			this._directory = Path.GetDirectoryName(filename);
		}

		public void Serialize()
		{
			Directory.CreateDirectory(this._directory);

			using var sw = new StreamWriter(File.Open(this._mainfile, FileMode.Create));

			sw.WriteLine("[VERSN4]");
			sw.WriteLine($"// {DateTime.Now:MM/dd/yyyy HH:mm:ss}");
			sw.WriteLine();
			sw.WriteLine($"game {this._profile.GameSTR}");
			sw.WriteLine($"directory \"{this._profile.Directory}\"");
			sw.WriteLine($"filecount {this._profile.Count}");
			sw.WriteLine("generate");
			sw.WriteLine();
			sw.WriteLine();
			sw.WriteLine();

			// Hierarchy:
			//     - Files
			//     - Managers
			//     - Collections
			
			foreach (var sdb in this._profile)
			{

				var sdbpath = this.ReplaceInvalidPathChars(sdb.Filename.ToUpperInvariant());
				var filepath = Path.Combine(this._directory, sdbpath);
				if (Directory.Exists(filepath)) Directory.CreateDirectory(filepath);
				Directory.CreateDirectory(filepath);
				sw.WriteLine($"new synchronize \"{sdb.Filename}\""); // write file name

				foreach (var manager in sdb.Database.Managers)
				{

					sw.WriteLine($"capacity \"{sdb.Filename}\" \"{manager.Name}\" {manager.Count}");
					var managepath = Path.Combine(filepath, manager.Name);
					Directory.CreateDirectory(managepath);
					
					foreach (Collectable collection in manager)
					{

						var cnamepath = Path.Combine(managepath, collection.CollectionName) + ".BIN";
						var actual = Path.Combine(sdbpath, manager.Name, collection.CollectionName) + ".BIN";
						var asm = collection as IAssembly;

						using (var bw = new BinaryWriter(File.Open(cnamepath, FileMode.Create)))
						{

							asm.Serialize(bw);

						}

						sw.WriteLine($"import synchronize \"{sdb.Filename}\" \"{actual}\"");

					}

					sw.WriteLine();

				}

				sw.WriteLine();
				sw.WriteLine();
				sw.WriteLine();

			}
		}

		private string ReplaceInvalidPathChars(string path)
		{
			string result = String.Empty;

			foreach (char c in path)
			{
				
				switch (c)
				{
					case '\\':
					case '/':
						result += '_';
						break;

					default:
						result += c;
						break;
				}

			}

			return result;
		}
	}
}
