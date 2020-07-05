namespace Endscript.Enums
{
	public enum eCommandType : int
	{
		invalid,
		empty,
		game,
		version,
		append,
		update_collection,
		update_string,
		update_texture,
		add_collection,
		add_string,
		add_texture,
		remove_collection,
		remove_string,
		remove_texture,
		copy_collection,
		copy_texture,
		replace_texture,
		@static,
		import,
		@new,
		delete,

		watermark,
		menu,
		create_file,
		create_folder,
		erase_file,
		erase_folder,
		move_file,
		move_folder,

		generate,
		directory,
		filecount,
		capacity,
		checkbox,
		combobox,
		end,
	}
}
