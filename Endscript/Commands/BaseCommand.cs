using Endscript.Core;
using Endscript.Enums;
using Endscript.Profiles;
using Endscript.Exceptions;
using Nikki.Reflection.Abstract;



namespace Endscript.Commands
{
	/// <summary>
	/// An <see langword="abstract"/> wrapper around Endscript command.
	/// </summary>
	public abstract class BaseCommand
	{
		public abstract eCommandType Type { get; }
		public string Filename { get; set; }
		public string Line { get; set; }
		public int Index { get; set; }

		public abstract void Prepare(string[] splits);
		public abstract void Execute(CollectionMap map);
		protected Collectable GetManualCollection(string filename, string manager, string cname, 
			BaseProfile profile)
		{
			var sdb = profile[filename];

			if (sdb is null)
			{

				throw new LookupFailException($"File {filename} was never loaded");

			}

			var manage = sdb.Database.GetManager(manager);

			if (manage is null)
			{

				throw new LookupFailException($"Manager named {manager} does not exist");

			}

			int index = manage.IndexOf(cname);

			if (index == -1) throw new LookupFailException($"Collection named {cname} does not exist");
			else return manage[index] as Collectable;
		}
	}
}
