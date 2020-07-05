using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using Nikki.Core;
using Nikki.Reflection.Abstract;



namespace Endscript.Core
{
	public class SynchronizedDatabase : IComparable<SynchronizedDatabase>
	{
		public FileBase Database { get; }
		public string Filename { get; }
		public string Folder { get; }
		public string FullPath => Path.Combine(this.Folder, this.Filename);
		public static string Watermark { get; set; }

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
		}

		public void Load() => this.Database.Load(new Options() { File = this.FullPath });
		public void Save() => this.Database.Save(new Options() { File = this.FullPath, Watermark = Watermark });

		public override bool Equals(object obj) => obj is SynchronizedDatabase sdb && this == sdb;
		public override int GetHashCode() => this.FullPath.GetHashCode();
		public static bool operator ==(SynchronizedDatabase sdb1, SynchronizedDatabase sdb2)
		{
			if (sdb1 is null) return sdb2 is null;
			else if (sdb2 is null) return false;
			return String.Equals(sdb1.FullPath, sdb2.FullPath, StringComparison.OrdinalIgnoreCase);
		}
		public static bool operator !=(SynchronizedDatabase sdb1, SynchronizedDatabase sdb2) => !(sdb1 == sdb2);
		public override string ToString() => this.FullPath.ToUpperInvariant();
		public int CompareTo([AllowNull] SynchronizedDatabase other)
		{
			return String.Compare(this.Filename, other?.Filename, StringComparison.OrdinalIgnoreCase);
		}
	}
}
