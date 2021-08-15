using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Endscript.Core;
using Nikki.Core;



namespace Endscript.Interfaces
{
	public interface IGameProfile : IList<SynchronizedDatabase>
	{
		public GameINT GameINT { get; }
		public string GameSTR { get; }
		public string Directory { get; }

		public Task<Exception[]> Load(Launch launch);
		public Task<Exception[]> Save();
		public void Serialize(string directory);
		public void Deserialize(string directory);
	}
}
