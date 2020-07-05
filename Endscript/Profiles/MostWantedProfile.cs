using Nikki.Core;



namespace Endscript.Profiles
{
	public class MostWantedProfile : BaseProfile
	{
		public override GameINT GameINT => GameINT.MostWanted;

		public override string GameSTR => GameINT.MostWanted.ToString();

		public override string Directory { get; }

		public MostWantedProfile(string directory) : base() { this.Directory = directory; }
	}
}
