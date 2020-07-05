using Nikki.Core;



namespace Endscript.Profiles
{
	public class ProstreetProfile : BaseProfile
	{
		public override GameINT GameINT => GameINT.Prostreet;

		public override string GameSTR => GameINT.Prostreet.ToString();

		public override string Directory { get; }

		public ProstreetProfile(string directory) : base() { this.Directory = directory; }
	}
}
