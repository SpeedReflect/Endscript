using System;
using Endscript.Core;
using Endscript.Enums;
using Endscript.Exceptions;
using Nikki.Core;



namespace Endscript.Commands
{
	public class GameCommand : BaseCommand
	{
		private GameINT _game;

		public override eCommandType Type => eCommandType.game;

		public override void Prepare(string[] splits)
		{
			if (splits.Length != 2) throw new InvalidArgsNumberException(splits.Length, 2);

			if (!Enum.TryParse(splits[1], out this._game))
			{

				throw new Exception($"Unable to recognize game {splits[1]}");

			}
		}

		public override void Execute(CollectionMap map)
		{
			if (map.Profile.GameINT == this._game) return;
			else throw new InvalidGameException(this._game);
		}
	}
}
