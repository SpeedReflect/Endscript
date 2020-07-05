using Endscript.Core;
using Endscript.Enums;
using Endscript.Profiles;
using Endscript.Exceptions;
using Nikki.Reflection.Abstract;



namespace Endscript.Commands
{
	public abstract class BaseCommand
	{
		public abstract eCommandType Type { get; }
		public string Line { get; set; }

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

			var collection = manage[manage.IndexOf(cname)];

			if (collection is Collectable result) return result;
			else throw new LookupFailException($"Collection named {cname} does not exist");
		}
	}
}
