using System;
using System.Collections.Generic;

using Endscript.Profiles;
using Endscript.Exceptions;

using Nikki.Reflection.Abstract;
using Nikki.Reflection.Interface;



namespace Endscript.Core
{
	public sealed class CollectionMap
	{
		private const string delim = "|";
		private readonly Dictionary<string, Collectable> _map;
		public BaseProfile Profile { get; }

		public CollectionMap(BaseProfile profile)
		{
			this.Profile = profile;
			this._map = new Dictionary<string, Collectable>(this.FastEstimateCapacity());
			this.LoadMapFromProfile();
		}

		private int FastEstimateCapacity()
		{
			int result = 10;
			foreach (var sdb in this.Profile)
			{

				foreach (var manager in sdb.Database.Managers)
				{

					result += manager.Count;

				}

			}

			return result;
		}

		private void LoadMapFromProfile()
		{
			foreach (var sdb in this.Profile)
			{

				foreach (var manager in sdb.Database.Managers)
				{

					foreach (Collectable collection in manager)
					{

						var path = sdb.Filename + delim + manager.Name + delim + collection.CollectionName;
						this._map.Add(path, collection);

					}

				}

			}
		}
	
		public Collectable GetCollection(string filename, string manager, string cname)
		{
			var path = filename + delim + manager + delim + cname;
			if (this._map.TryGetValue(path, out var result)) return result;
			throw new LookupFailException($"Collection named {cname} does not exist");
		}
	
		public bool ContainsCollection(string filename, string manager, string cname)
		{
			var path = filename + delim + manager + delim + cname;
			return this._map.ContainsKey(path);
		}
	
		public void AddCollection(string filename, string manager, string cname, object collection)
		{
			var path = filename + delim + manager + delim + cname;
			this._map.Add(path, (Collectable)collection);
		}

		public void RemoveCollection(string filename, string manager, string cname)
		{
			var path = filename + delim + manager + delim + cname;
			this._map.Remove(path);
		}
	}
}
