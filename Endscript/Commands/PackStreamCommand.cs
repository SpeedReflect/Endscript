using Endscript.Core;
using Endscript.Enums;
using Endscript.Profiles;
using Endscript.Exceptions;
using Endscript.Interfaces;
using Endscript.Streamable;



namespace Endscript.Commands
{
	/// <summary>
	/// Command of type 'pack_stream [lxry] [streamlxry] [source]'.
	/// </summary>
	public class PackStreamCommand : BaseCommand, ISingleParsable
	{
		private string _lxry;
		private string _streamlxry;
		private string _src;

		public override eCommandType Type => eCommandType.pack_stream;

		public override void Prepare(string[] splits)
		{
			if (splits.Length != 4) throw new InvalidArgsNumberException(splits.Length, 4);

			this._lxry = splits[1].ToUpperInvariant();
			this._streamlxry = splits[2].ToUpperInvariant();
			this._src = splits[3].ToUpperInvariant();
		}

		public override void Execute(CollectionMap map)
		{
			var streamer = BaseStream.GetStreamer(map.Profile.GameINT, this._lxry, this._streamlxry);
			streamer.Save(map.Profile.Directory, this._src);
		}

		public void SingleExecution(BaseProfile profile)
		{
			var streamer = BaseStream.GetStreamer(profile.GameINT, this._lxry, this._streamlxry);
			streamer.Save(profile.Directory, this._src);
		}
	}
}
