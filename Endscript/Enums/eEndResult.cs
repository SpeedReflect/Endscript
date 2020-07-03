using System;



namespace Endscript.Enums
{
	public enum eEndResult : int
	{
		Invalid = 0,
		Comment = 1,
		CollectionName,


		UpdateNode,
		AddNode,
		RemoveNode,
		CopyNode,
		StaticNode,

		UpdateSubPart,

		UpdateTexture,
		AddTexture,
		RemoveTexture,
		CopyTexture,
		ReplaceTexture,

		UpdateString,
		AddString,
		RemoveString,

		UpdateFEng,

		NewDatabase,
		DeleteDatabase,

		Export,

		ImportNegate,
		ImportSynchronize,
		ImportOverride,



		Error = Int32.MaxValue
	}
}
