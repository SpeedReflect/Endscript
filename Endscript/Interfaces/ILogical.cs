using Endscript.Core;
using Endscript.Enums;



namespace Endscript.Interfaces
{
	public interface ILogical
	{
		public eLogicType LogicType { get; }
		public void Parse(string[] splits);
		public bool Evaluate(CollectionMap map);
	}
}
