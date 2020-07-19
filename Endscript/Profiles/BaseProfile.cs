using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Endscript.Core;
using Endscript.Enums;
using Endscript.Interfaces;
using Endscript.Exceptions;
using Nikki.Core;
using CoreExtensions.Management;



namespace Endscript.Profiles
{
	public abstract class BaseProfile : IGameProfile
	{
		private SynchronizedDatabase[] _sdb;
		private int _size;
		private SynchronizedDatabase[] _empty = new SynchronizedDatabase[0];
		private int _capacity => this._sdb.Length;

		public abstract GameINT GameINT { get; }
		public abstract string GameSTR { get; }
		public abstract string Directory { get; }

		public SynchronizedDatabase this[int index]
		{
			get
			{
				if (index < 0 || index > this._size)
				{

					throw new IndexOutOfRangeException();

				}
				else
				{

					return this._sdb[index];

				}
			}
			set
			{
				if (index < 0 || index > this._size)
				{

					throw new IndexOutOfRangeException();

				}
				else
				{

					if (this.Contains(value.Filename))
					{

						throw new DatabaseExistenceException(value.Filename);

					}

					this._sdb[index] = value;

				}
			}
		}
		public SynchronizedDatabase this[string filename] => this.Find(filename);
		~BaseProfile()
		{
			for (int i = 0; i < this._size; ++i) this._sdb[i] = null;
			ForcedX.GCCollect();

			#if DEBUG
			Console.WriteLine($"Profile {this.GameINT} destroyed");
			#endif
		}

		public int Capacity
		{
			get
			{
				return this._capacity;
			}
			set
			{
				if (value < this._size || value == this._capacity)
				{

					return;

				}

				if (value > 0)
				{

					var data = new SynchronizedDatabase[value];

					for (int i = 0; i < this._size; ++i)
					{

						data[i] = this._sdb[i];

					}

					this._sdb = data;
					ForcedX.GCCollect();

				}
				else
				{

					this._sdb = this._empty;
					ForcedX.GCCollect();

				}
			}
		}
		public int Count => this._size;
		public bool IsReadOnly => false;

		#region Main

		public BaseProfile()
		{
			this._sdb = this._empty;
		}

		public BaseProfile(int capacity)
		{
			this._sdb = capacity <= 0 ? this._empty : (new SynchronizedDatabase[capacity]);
		}

		public BaseProfile(IEnumerable<SynchronizedDatabase> collection)
		{
			if (collection == null)
			{

				throw new ArgumentNullException(nameof(collection));

			}
			else
			{

				if (collection is ICollection<SynchronizedDatabase> elements)
				{

					if (elements.Count == 0)
					{

						this._sdb = this._empty;
						return;

					}
					else
					{

						this._sdb = new SynchronizedDatabase[elements.Count];
						elements.CopyTo(this._sdb, 0);
						this._size = elements.Count;

					}

				}
				else
				{

					this._sdb = this._empty;

					foreach (var element in collection)
					{

						this.Add(element);

					}

				}

			}

		}

		public static BaseProfile NewProfile(GameINT game, string directory)
		{
			return game switch
			{
				GameINT.Carbon => new CarbonProfile(directory),
				GameINT.MostWanted => new MostWantedProfile(directory),
				GameINT.Underground2 => new Underground2Profile(directory),
				GameINT.Prostreet => new ProstreetProfile(directory),
				_ => throw new InvalidGameException(game)
			};
		}

		public static BaseProfile NewProfile(string game, string directory)
		{
			if (!Enum.TryParse(game, out GameINT type))
			{

				throw new InvalidGameException(game);

			}

			return NewProfile(type, directory);
		}

		#endregion

		#region Enumerator

		public struct Enumerator : IEnumerator<SynchronizedDatabase>, IDisposable, IEnumerator
		{
			private BaseProfile _profile;
			private int _index;
			private SynchronizedDatabase _current;

			internal Enumerator(BaseProfile profile)
			{
				this._profile = profile;
				this._index = 0;
				this._current = null;
			}

			public SynchronizedDatabase Current => this._current;

			object IEnumerator.Current
			{
				get
				{
					if (this._index == 0 || this._index == this._profile._size + 1)
					{

						throw new InvalidOperationException();

					}

					return this._current;
				}
			}

			public void Dispose() { }

			public bool MoveNext()
			{
				if (this._index < this._profile._size)
				{

					this._current = this._profile._sdb[this._index++];
					return true;

				}

				this._index = this._profile._size + 1;
				this._current = null;
				return false;
			}

			public void Reset()
			{
				this._index = 0;
				this._current = null;
			}
		}

		public IEnumerator<SynchronizedDatabase> GetEnumerator() => new Enumerator(this);

		IEnumerator IEnumerable.GetEnumerator() => new Enumerator(this);

		#endregion

		public void Add(string filename)
		{
			if (this.Contains(filename))
			{

				throw new DatabaseExistenceException(filename);

			}

			if (this._size == this._capacity) ++this.Capacity;
			this._sdb[this._size++] = new SynchronizedDatabase(this.GameINT, this.Directory, filename);
		}

		public void Add(SynchronizedDatabase item)
		{
			if (this.Contains(item))
			{

				throw new DatabaseExistenceException(item.Filename);

			}

			if (this._size == this._capacity) ++this.Capacity;
			this._sdb[this._size++] = item;
		}
		
		public void Clear()
		{
			if (this._size > 0)
			{

				Array.Clear(this._sdb, 0, this._size);
				this._size = 0;

			}
		}

		public bool Contains(string filename)
		{
			if (String.IsNullOrEmpty(filename)) return false;

			for (int i = 0; i < this._size; ++i)
			{

				if (String.Equals(this._sdb[i].Filename, filename, StringComparison.OrdinalIgnoreCase))
				{

					return true;

				}

			}

			return false;
		}

		public bool Contains(SynchronizedDatabase item)
		{
			for (int i = 0; i < this._size; ++i)
			{

				if (this._sdb[i].Equals(item))
				{

					return true;

				}

			}

			return false;
		}

		public void CopyTo(SynchronizedDatabase[] array)
		{
			this.CopyTo(array, 0);
		}

		public void CopyTo(SynchronizedDatabase[] array, int arrayIndex)
		{
			Array.Copy(this._sdb, 0, array, arrayIndex, this._size);
		}

		public SynchronizedDatabase Find(string filename)
		{
			if (String.IsNullOrEmpty(filename)) return null;

			for (int i = 0; i < this._size; ++i)
			{

				if (String.Equals(this._sdb[i].Filename, filename, StringComparison.OrdinalIgnoreCase))
				{

					return this._sdb[i];

				}

			}

			return null;
		}

		public SynchronizedDatabase Find(Predicate<SynchronizedDatabase> predicate)
		{
			if (predicate == null)
			{

				throw new ArgumentNullException(nameof(predicate));

			}
			else
			{

				for (int i = 0; i < this._size; ++i)
				{

					if (predicate(this._sdb[i]))
					{

						return this._sdb[i];

					}

				}

			}

			return null;
		}

		public void ForEach(Action<SynchronizedDatabase> action)
		{
			if (action == null)
			{

				throw new ArgumentNullException(nameof(action));

			}
			else
			{

				for (int i = 0; i < this._size; ++i)
				{

					action(this._sdb[i]);

				}

			}
		}
		
		public int IndexOf(string filename)
		{
			if (String.IsNullOrEmpty(filename)) return -1;

			for (int i = 0; i < this._size; ++i)
			{

				if (String.Equals(this._sdb[i].Filename, filename, StringComparison.OrdinalIgnoreCase))
				{

					return i;

				}

			}

			return -1;
		}

		public int IndexOf(SynchronizedDatabase item)
		{
			for (int i = 0; i < this._size; ++i)
			{

				if (this._sdb[i].Equals(item)) return i;

			}

			return -1;
		}
		
		public void Insert(int index, string filename)
		{
			if (index < 0 || index > this._size)
			{

				throw new ArgumentOutOfRangeException(nameof(index));

			}

			if (this.Contains(filename))
			{

				throw new DatabaseExistenceException(filename);

			}

			if (this._size == this._capacity) ++this.Capacity;

			if (index < this._size)
			{

				Array.Copy(this._sdb, index, this._sdb, index + 1, this._size - index);

			}


			this._sdb[index] = new SynchronizedDatabase(this.GameINT, this.Directory, filename);
			++this._size;
		}

		public void Insert(int index, SynchronizedDatabase item)
		{
			if (index < 0 || index > this._size)
			{

				throw new ArgumentOutOfRangeException(nameof(index));

			}

			if (this.Contains(item))
			{

				throw new DatabaseExistenceException(item.Filename);

			}

			if (this._size == this._capacity) ++this.Capacity;

			if (index < this._size)
			{

				Array.Copy(this._sdb, index, this._sdb, index + 1, this._size - index);

			}

			this._sdb[index] = item;
			++this._size;
		}
		
		public void Remove(string filename)
		{
			int index = this.IndexOf(filename);

			if (index != -1) this.RemoveAt(index);
			else throw new ArgumentException($"File named {filename} was never loaded");
		}

		public bool Remove(SynchronizedDatabase item)
		{
			int index = this.IndexOf(item);

			if (index != -1)
			{

				this.RemoveAt(index);
				return true;

			}

			return false;
		}

		public void RemoveAt(int index)
		{
			if (index >= this._size || index < 0)
			{

				throw new ArgumentOutOfRangeException(nameof(index));

			}

			--this._size;

			if (index < this._size)
			{

				Array.Copy(this._sdb, index + 1, this._sdb, index, this._size - index);

			}

			this._sdb[this._size] = null;
		}

		public void Sort()
		{
			if (this._size <= 1) return;
			Array.Sort(this._sdb, 0, this._size);
		}

		public void New(eImportType type, string filename)
		{
			this.Add(filename);
			var sdb = this._sdb[this._size - 1];

			switch (type)
			{
				case eImportType.negate:
					if (File.Exists(sdb.FullPath)) { sdb.Load(); break; }
					else goto default;

				case eImportType.synchronize:
					if (File.Exists(sdb.FullPath)) break;
					else goto default;

				default: // eImportType.@override
					var directory = Path.GetDirectoryName(sdb.FullPath);
					System.IO.Directory.CreateDirectory(directory);
					File.Create(sdb.FullPath);
					break;

			}
		}

		public void Delete(string filename)
		{
			var index = this.IndexOf(filename);
			
			if (index == -1)
			{

				throw new ArgumentException($"File named {filename} was never loaded");

			}

			this._sdb[index].Save();
			this.RemoveAt(index);
			this.SaveHashList();
			ForcedX.GCCollect();
		}

		public async void Load(Launch launch)
		{
			this.LoadHashList();
			launch.LoadLinks();
			if (launch.Files.Count == 0) return;
			launch.CheckFiles();

			var tasks = new List<Task>(launch.Files.Count);

			foreach (var file in launch.Files)
			{

				var sdb = new SynchronizedDatabase(this.GameINT, this.Directory, file);
				this.Add(sdb);
				tasks.Add(this.LoadOneSDB(sdb));

			}

			await Task.WhenAll(tasks);
		}

		public async void Save()
		{
			var tasks = new List<Task>(this._size);

			foreach (var sdb in this)
			{

				tasks.Add(this.SaveOneSDB(sdb));

			}

			await Task.WhenAll(tasks);
			this.SaveHashList();
		}

		public abstract void LoadHashList();

		public abstract void SaveHashList();

		public void Serialize(string filename)
		{
			var serializer = new EndSerializer(this, filename);
			serializer.Serialize();
		}

		public void Deserialize(string filename)
		{
			var deserializer = new EndDeserializer(filename);
			deserializer.Deserialize();
		}

		private Task LoadOneSDB(SynchronizedDatabase sdb)
		{
			sdb.Load();
			return Task.CompletedTask;
		}

		private Task SaveOneSDB(SynchronizedDatabase sdb)
		{
			sdb.Save();
			return Task.CompletedTask;
		}
	}
}
