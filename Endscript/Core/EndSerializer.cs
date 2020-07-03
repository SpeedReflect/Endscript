using System;
using System.IO;
using Endscript.Profiles;
using Nikki.Reflection.Abstract;
using Nikki.Reflection.Interface;



namespace Endscript.Core
{
	public sealed class EndSerializer
	{
		private readonly BaseProfile _profile;
		private readonly string _directory;

		private string MainFile => Path.Combine(this._directory, "info.end");

		public EndSerializer(BaseProfile profile, string directory)
		{
			this._profile = profile;
			this._directory = directory;
		}

		public void Serialize()
		{
			if (Directory.Exists(this._directory)) Directory.Delete(this._directory, true);
			Directory.CreateDirectory(this._directory);

			using var sw = new StreamWriter(File.Open(this.MainFile, FileMode.Create));

			sw.WriteLine("[VERSN4]");
			sw.WriteLine($"// {DateTime.Now:MM/dd/yyyy HH:mm}");
			sw.WriteLine();

			// Hierarchy:
			//     - Files
			//     - Managers
			//     - Collections
			
			foreach (var sdb in this._profile)
			{

				var filepath = this.ReplaceInvalidPathChars(sdb.Filename.ToUpperInvariant());
				Directory.CreateDirectory(Path.Combine(this._directory, filepath));
				sw.WriteLine($"new {sdb.Filename}"); // write file name

				foreach (var manager in sdb.Database.Managers)
				{

					var managepath = Path.Combine(filepath, manager.Name);
					Directory.CreateDirectory(managepath);
					
					foreach (Collectable collection in manager)
					{

						var cnamepath = Path.Combine(managepath, collection.CollectionName) + ".BIN";
						var actual = Path.Combine(filepath, manager.Name, collection.CollectionName) + ".BIN";
						var asm = collection as IAssembly;

						using (var bw = new BinaryWriter(File.Open(cnamepath, FileMode.Create)))
						{

							asm.Serialize(bw);

						}

						sw.WriteLine($"import synchronized {sdb.Filename} {manager.Name} {actual}");

					}

				}

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
