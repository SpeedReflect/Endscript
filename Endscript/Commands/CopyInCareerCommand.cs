using System;
using Endscript.Core;
using Endscript.Enums;
using Endscript.Profiles;
using Endscript.Exceptions;
using Endscript.Interfaces;
using Nikki.Support.Shared.Class;



namespace Endscript.Commands
{
	/// <summary>
	/// Command of type 'copy_incareer [filename] [manager] [gcareer] [root] [from] [to]'.
	/// </summary>
	public class CopyInCareerCommand : BaseCommand, ISingleParsable
	{
		private string _filename;
		private string _manager;
		private string _gcareer;
		private string _root;
		private string _from;
		private string _to;

		public override eCommandType Type => eCommandType.copy_incareer;

		public override void Prepare(string[] splits)
		{
			if (splits.Length != 7) throw new InvalidArgsNumberException(splits.Length, 7);

			this._filename = splits[1].ToUpperInvariant();
			this._manager = splits[2];
			this._gcareer = splits[3];
			this._root = splits[4];
			this._from = splits[5];
			this._to = splits[6];
		}

		public override void Execute(CollectionMap map)
		{
			var collection = map.GetCollection(this._filename, this._manager, this._gcareer);

			if (collection is GCareer gcareer)
			{

				gcareer.CloneCollection(this._to, this._from, this._root);

			}
			else
			{

				throw new Exception($"Object {this._gcareer} is not a GCareer");

			}
		}

		public void SingleExecution(BaseProfile profile)
		{
			var collection = this.GetManualCollection(this._filename, this._manager, this._gcareer, profile);

			if (collection is GCareer gcareer)
			{

				gcareer.CloneCollection(this._to, this._from, this._root);

			}
			else
			{

				throw new Exception($"Object {this._gcareer} is not a GCareer");

			}
		}
	}
}
