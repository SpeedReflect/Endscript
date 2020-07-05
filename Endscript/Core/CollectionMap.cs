using System;
using System.Collections.Generic;
using Endscript.Profiles;
using Endscript.Exceptions;
using Nikki.Reflection.Abstract;
using CoreExtensions.Management;



namespace Endscript.Core
{
	public class CollectionMap
	{
		private const string delim = "|";
		private readonly Dictionary<string, Collectable> _map;
		public BaseProfile Profile { get; }
		public string Directory { get; }

		public CollectionMap(BaseProfile profile, string directory)
		{
			this.Profile = profile;
			this.Directory = directory;
			this._map = new Dictionary<string, Collectable>();
			this.LoadMapFromProfile(false);
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

		public void LoadMapFromProfile(bool gccollect)
		{
			this._map.Clear();
			this._map.EnsureCapacity(this.FastEstimateCapacity());

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

			if (gccollect) ForcedX.GCCollect(true, true);
		}

		public Collectable GetCollection(string filename, string manager, string cname)
		{
			var path = filename + delim + manager + delim + cname;
			if (this._map.TryGetValue(path, out var result)) return result;
			else throw new LookupFailException($"Collection named {cname} does not exist");
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
