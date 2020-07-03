using System.Collections.Generic;
using Endscript.Core;
using Nikki.Core;



namespace Endscript.Interfaces
{
	public interface IGameProfile : IList<SynchronizedDatabase>
	{
		public GameINT GameINT { get; }
		public string GameSTR { get; }
		public string Directory { get; }

		public void Load(Launch launch);
		public void Save();
		public void Serialize();
		public void Deserialize();
	}
}
