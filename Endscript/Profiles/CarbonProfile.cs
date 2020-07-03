using System;
using System.Collections.Generic;
using System.Text;

using Nikki.Core;

namespace Endscript.Profiles
{
	public sealed class CarbonProfile : BaseProfile
	{
		public override GameINT GameINT => GameINT.Carbon;

		public override string GameSTR => GameINT.Carbon.ToString();

		public override string Directory { get; }

		public CarbonProfile(string directory) : base() { this.Directory = directory; }

		public override void Serialize()
		{

		}

		public override void Deserialize()
		{

		}
	}
}
