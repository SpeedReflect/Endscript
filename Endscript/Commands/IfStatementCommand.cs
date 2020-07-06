using System;
using Endscript.Core;
using Endscript.Enums;
using Endscript.Interfaces;



namespace Endscript.Commands
{
	public class IfStatementCommand : BaseCommand, ISelectable
	{
		public override eCommandType Type => eCommandType.@if;

		public int Choice { get; set; }

		public string Description => throw new NotImplementedException();

		public string[] Options => throw new NotImplementedException();

		public override void Prepare(string[] splits)
		{
			throw new NotSupportedException($"If statements are not implemented yet");
		}

		public override void Execute(CollectionMap map)
		{
		}

		public int ParseOption(string option) => throw new NotImplementedException();
		public bool Contains(string option) => throw new NotImplementedException();
		public void Evaluate() => throw new NotImplementedException();
	}
}
