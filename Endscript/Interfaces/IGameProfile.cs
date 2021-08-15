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

		public string[] Load(Launch launch);
		public string[] Save();
		public void Serialize(string directory);
		public void Deserialize(string directory);
	}
}
