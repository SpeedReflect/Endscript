using System;
using System.IO;
using Nikki.Core;
using Nikki.Reflection.Abstract;



namespace Endscript
{
	public class SynchronizedDatabase
	{
		public FileBase Database { get; set; }

		public string Filename { get; }

		public string Folder { get; }

		public string FullPath => Path.Combine(this.Folder, this.Filename);

		public static string Watermark { get; set; }

		private Options LoadingOpts { get; }

		private Options SavingOpts { get; }

		public SynchronizedDatabase(GameINT game, string folder, string file)
		{
			this.Database = game switch
			{
				GameINT.Carbon => new Nikki.Support.Carbon.Datamap(),
				GameINT.MostWanted => new Nikki.Support.MostWanted.Datamap(),
				GameINT.Prostreet => new Nikki.Support.Prostreet.Datamap(),
				_ => throw new ArgumentException(nameof(game)),
			};

			this.Folder = folder;
			this.Filename = file.ToUpperInvariant();
			this.LoadingOpts = new Options(this.FullPath);
			this.SavingOpts = new Options(this.FullPath, Watermark);
		}

		public void Load() => this.Database.Load(this.LoadingOpts);

		public void Save() => this.Database.Save(this.SavingOpts);
	}
}
