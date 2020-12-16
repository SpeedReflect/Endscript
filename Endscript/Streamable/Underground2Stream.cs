using System;
using System.IO;
using System.Text.Json;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Nikki.Core;
using Nikki.Utils;
using Nikki.Reflection.Enum;
using CoreExtensions.IO;



namespace Endscript.Streamable
{
	public class Underground2Stream : BaseStream
	{
        private Underground2StreamingSection[] _sections;
        private string _directory;
		public override GameINT GameINT => GameINT.Underground2;
		public override string GameSTR => GameINT.Underground2.ToString();
        public Underground2StreamingSection[] Sections => this._sections;

		public Underground2Stream(string lxry, string streamlxry) : base(lxry, streamlxry)
		{
            this._sections = new Underground2StreamingSection[0];
		}

		public override void Load(string directory, string dest)
		{
            var lxry = Path.Combine(directory, this.LXRY);
            var streamlxry = Path.Combine(directory, this.STREAMLXRY);

            if (!File.Exists(lxry)) throw new FileNotFoundException($"File {lxry} does not exist");
            if (!File.Exists(streamlxry)) throw new FileNotFoundException($"File {streamlxry} does not exist");

            this._directory = Path.Combine(directory, dest);
            Directory.CreateDirectory(this._directory);

            using (var br = new BinaryReader(File.Open(lxry, FileMode.Open, FileAccess.Read)))
			{

                while (br.BaseStream.Position < br.BaseStream.Length)
				{

                    var id = br.ReadEnum<BinBlockID>();
                    var size = br.ReadInt32();
                    var offset = br.BaseStream.Position;

                    if (id == BinBlockID.TrackStreamingSections)
					{

                        var count = size / 0x50;
                        this._sections = new Underground2StreamingSection[count];

                        for (int i = 0; i < count; ++i)
                        {

                            this._sections[i] = br.ReadUnmanaged<Underground2StreamingSection>();

                        }

					}

                    br.BaseStream.Position = offset + size;

				}

			}

            using (var br = new BinaryReader(File.Open(streamlxry, FileMode.Open, FileAccess.Read)))
			{

                for (int i = 0; i < this._sections.Length; ++i)
				{

                    var section = this._sections[i];
                    var totalpath = Path.Combine(this._directory, section.GetName());
                    Directory.CreateDirectory(totalpath);

                    br.BaseStream.Position = section.FileOffset;
                    var array = br.ReadBytes(section.Size);

                    var dataPath = Path.Combine(totalpath, "DATA.BIN");
                    var setsPath = Path.Combine(totalpath, "Settings.end");

                    File.WriteAllBytes(dataPath, array);
                    File.WriteAllText(setsPath, Underground2StreamingSection.Serialize(section));

				}

			}

            using (var sw = new StreamWriter(File.Open(Path.Combine(this._directory, "Sections.end"), FileMode.Create, FileAccess.Write)))
			{

                sw.WriteLine("[VERSN5]");
                sw.WriteLine();

                for (int i = 0; i < this._sections.Length; ++i)
				{

                    sw.WriteLine(this._sections[i].GetName());

				}

			}
        }

		public override void Save(string directory, string src)
		{
            var lxry = Path.Combine(directory, this.LXRY);
            var streamlxry = Path.Combine(directory, this.STREAMLXRY);

            if (!File.Exists(lxry)) throw new FileNotFoundException($"File {lxry} does not exist");

            this._directory = Path.Combine(directory, src);
            if (!Directory.Exists(this._directory)) throw new DirectoryNotFoundException($"Folder {this._directory} does not exist");

            var sectionsPath = Path.Combine(this._directory, "Sections.end");
            if (!File.Exists(sectionsPath)) throw new FileNotFoundException($"Section.end file does not exist");

            var sectionList = File.ReadAllLines(sectionsPath);
            if (sectionList is null || sectionList.Length < 1) throw new Exception("Sections.end has no information");
            if (sectionList[0] != "[VERSN5]") throw new Exception("Sections.end file is not a Version 5 endscript");
            
            var sectionNames = new List<string>(sectionList.Length);

            for (int i = 1; i < sectionList.Length; ++i)
			{

                var line = sectionList[i];
                if (String.IsNullOrWhiteSpace(line)) continue;
                sectionNames.Add(line);

			}

            this._sections = new Underground2StreamingSection[sectionNames.Count];

            using (var bw = new BinaryWriter(File.Open(streamlxry, FileMode.Create, FileAccess.Write)))
            {

                for (int i = 0; i < sectionNames.Count; ++i)
                {

                    var dir = Path.Combine(this._directory, sectionNames[i]);
                    var dataPath = Path.Combine(dir, "DATA.BIN");
                    var setsPath = Path.Combine(dir, "Settings.end");

                    var array = File.ReadAllBytes(dataPath);
                    var section = Underground2StreamingSection.Deserialize(File.ReadAllText(setsPath));

                    section.FileOffset = (uint)bw.BaseStream.Position;
                    section.Size = array.Length;
                    section.CompressedSize = array.Length;
                    bw.Write(array);
                    var mark = Options.Default.Watermark;
                    bw.GeneratePadding(mark, new Alignment(0x1000, Alignment.AlignmentType.Modular));
                    this._sections[i] = section;

                }

            }

            var mainFile = File.ReadAllBytes(lxry);

            using (var ms = new MemoryStream(mainFile))
            using (var br = new BinaryReader(ms))
            using (var bw = new BinaryWriter(File.Open(lxry, FileMode.Create, FileAccess.Write)))
			{

                while (br.BaseStream.Position < br.BaseStream.Length)
				{

                    var id = br.ReadEnum<BinBlockID>();
                    var size = br.ReadInt32();
                    var offset = br.BaseStream.Position;

                    if (id == BinBlockID.TrackStreamingSections)
					{

                        bw.WriteEnum(id);
                        bw.Write(this._sections.Length * 0x50);

                        for (int i = 0; i < this._sections.Length; ++i)
						{

                            bw.WriteUnmanaged(this._sections[i]);

						}

					}
                    else
					{

                        bw.WriteEnum(id);
                        bw.Write(size);
                        bw.Write(br.ReadBytes(size));

					}

                    br.BaseStream.Position = offset + size;

				}

			}
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct Underground2StreamingSection
	{
        public long SectionName { get; set; }
        public short SectionNumber { get; set; }
        public byte WasRendered { get; set; }
        public byte CurrentlyVisible { get; set; }
        public int Status { get; set; }
        public int FileType { get; set; }
        public uint FileOffset { get; set; }
        public int Size { get; set; }
        public int CompressedSize { get; set; }
        public int SectionPriority { get; set; }
        public float CenterX { get; set; }
        public float CenterY { get; set; }
        public float Radius { get; set; }
        public uint Checksum { get; set; }
        public int LastNeededTimestamp { get; set; }
        public uint UnactivatedFrameCount { get; set; }
        public int LoadedTime { get; set; }
        public int BaseLoadingPriority { get; set; }
        public int LoadingPriority { get; set; }
        public int MemoryPointer { get; set; }
        public int DiscBundlePointer { get; set; }

        public string GetName()
        {
            return Helpers.NativeHelper.GetString(this, 0, 8);
        }
        public override string ToString() => this.GetName();

        private static readonly JsonSerializerOptions options = new JsonSerializerOptions()
        {
            AllowTrailingCommas = true,
            IgnoreReadOnlyProperties = true,
            ReadCommentHandling = JsonCommentHandling.Skip,
            WriteIndented = true,
        };

        public static string Serialize(Underground2StreamingSection section)
        {
            return JsonSerializer.Serialize(section, options);
        }

        public static Underground2StreamingSection Deserialize(string data)
        {
            return JsonSerializer.Deserialize<Underground2StreamingSection>(data, options);
        }
    }
}
