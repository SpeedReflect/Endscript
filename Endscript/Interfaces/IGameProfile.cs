using System;
using System.Collections.Generic;
using Endscript.Version;
using Nikki.Core;



namespace Endscript.Interfaces
{
	public interface IGameProfile : IList<SynchronizedDatabase>
	{
		public GameINT GameINT { get; }
		public string GameSTR { get; }
		public string Directory { get; }
		public string Watermark { get; set; }

		public void Load(Launch launch);
		public void Save();
	}
}
