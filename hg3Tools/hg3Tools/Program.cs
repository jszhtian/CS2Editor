﻿using Newtonsoft.Json.Serialization;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace hg3Tools
{
    public class Hg3ArcHead
    {
        public char[]? Magic { get; set; }//4bytes
        public UInt32 SlicHeadSize { get; set; }

        public static char[] ReturnExpectChars()
        {
            char[] exChars = new char[4];
            exChars[0] = 'H';
            exChars[1] = 'G';
            exChars[2] = '-';
            exChars[3] = '3';
            return exChars;
        }
        public static int GetChunkSize()
        {
            return 8;
        }

        public void WriteToStream(BinaryWriter writer)
        {
            var magicBytes = Encoding.ASCII.GetBytes(Magic).ToArray();
            writer.Write(magicBytes,0,magicBytes.Length);
            writer.Write(SlicHeadSize);
        }
    }

    public class Hg3Slice
    {
        public Hg3SliceHead? Head { get; set; }
        public Hg3TagInfo? TagInfo { get; set; }
        public Hg3SupportedTag? Tag { get; set; }
        public Hg3FormatInfo? FormatInfo { get; set; }
        public Hg3SupportedFormat? Format { get; set; }
        [JsonIgnore]
        public byte[]? ImagePayloads { get; set; }
        public Hg3CPtype? CPtype { get; set; }
        [JsonIgnore]
        public byte[]? CPtypePayloads { get; set; }

        public int GetChunkSize()
        {
            return Hg3SliceHead.GetChunkSize() + Hg3TagInfo.GetChunkSize() + Tag.GetChunkSize() +
                   Hg3FormatInfo.GetChunkSize() + Format.GetChunkSize() + ImagePayloads.Length +
                   Hg3CPtype.GetChunkSize() + CPtypePayloads.Length;
        }
        public void WriteToStream(BinaryWriter writer)
        {
            Head.WriteToStream(writer);
            TagInfo.WriteToStream(writer);
            Tag.WriteToStream(writer);
            FormatInfo.WriteToStream(writer);
            Format.WriteToStream(writer);
            writer.Write(ImagePayloads,0,ImagePayloads.Length);
            CPtype.WriteToStream(writer);
            writer.Write(CPtypePayloads);
        }
    }

    public class Hg3SliceHead
    {
        public UInt32 Unk0 { get; set; }
        public UInt32 OffsetToNextSlice { get; set; }
        public UInt32 Index { get; set; }
        public static int GetChunkSize()
        {
            return 12;
        }
        public void WriteToStream(BinaryWriter writer)
        {
            writer.Write(Unk0);
            writer.Write(OffsetToNextSlice);
            writer.Write(Index);
        }
    }

    public class Hg3TagInfo
    {
        public char[]? Magic { get; set; }//8bytes
        public UInt32 OffsetToNextChunk { get; set; }
        public UInt32 Size { get; set; }
        public static int GetChunkSize()
        {
            return 16;
        }
        public void WriteToStream(BinaryWriter writer)
        {
            var magicBytes = Encoding.ASCII.GetBytes(Magic).ToArray();
            writer.Write(magicBytes, 0, magicBytes.Length);
            writer.Write(OffsetToNextChunk);
            writer.Write(Size);
        }

    }

    public class Hg3SupportedTag
    {
        public virtual int GetChunkSize()
        {
            throw new NotImplementedException("shouldn't be here");
        }

        public virtual void WriteToStream(BinaryWriter writer)
        {
            throw new NotImplementedException("shouldn't be here");
        }
    }

    public class Hg3Stdinfo : Hg3SupportedTag
    {
        public UInt32 Width { get; set; }
        public UInt32 Height { get; set; }
        public UInt32 Depth { get; set; }
        public UInt32 OffsetX { get; set; }
        public UInt32 OffsetY { get; set; }
        public UInt32 TotalWidth { get; set; }
        public UInt32 TotalHeight { get; set; }
        public UInt32 Unk0 { get; set; }
        public UInt32 CanvasWidth { get; set; }
        public UInt32 CanvasHeight { get; set; }
        public static char[] ReturnExpectChars()
        {
            char[] exChars = new char[8];
            exChars[0] = 's';
            exChars[1] = 't';
            exChars[2] = 'd';
            exChars[3] = 'i';
            exChars[4] = 'n';
            exChars[5] = 'f';
            exChars[6] = 'o';
            exChars[7] = (char)0;
            return exChars;
        }

        public override int GetChunkSize()
        {
            return 40;
        }
        public override void WriteToStream(BinaryWriter writer)
        {
            writer.Write(Width);
            writer.Write(Height);
            writer.Write(Depth);
            writer.Write(OffsetX);
            writer.Write(OffsetY);
            writer.Write(TotalWidth);
            writer.Write(TotalHeight);
            writer.Write(Unk0);
            writer.Write(CanvasWidth);
            writer.Write(CanvasHeight);
        }
    }

    public class Hg3SupportedFormat
    {
        public virtual int GetChunkSize()
        {
            throw new NotImplementedException("shouldn't be here");
        }
        public virtual void WriteToStream(BinaryWriter writer)
        {
            throw new NotImplementedException("shouldn't be here");
        }
    }

    public class Hg3FormatInfo
    {
        public char[]? Magic { get; set; }//8bytes
        public UInt32 OffsetToNextChunk { get; set; }
        public UInt32 Size { get; set; }
        public static int GetChunkSize()
        {
            return 16;
        }
        public void WriteToStream(BinaryWriter writer)
        {
            var magicBytes = Encoding.ASCII.GetBytes(Magic).ToArray();
            writer.Write(magicBytes, 0, magicBytes.Length);
            writer.Write(OffsetToNextChunk);
            writer.Write(Size);
        }
    }

    public class Img0000Head : Hg3SupportedFormat
    {
        public UInt32 Unk0 { get; set; }
        public UInt32 Height { get; set; }
        public UInt32 PackedDataSize { get; set; }
        public UInt32 DataSize { get; set; }
        public UInt32 PackedCtlSize { get; set; }
        public UInt32 CtlSize { get; set; }
        public static char[] ReturnExpectChars()
        {
            char[] exChars = new char[8];
            exChars[0] = 'i';
            exChars[1] = 'm';
            exChars[2] = 'g';
            exChars[3] = '0';
            exChars[4] = '0';
            exChars[5] = '0';
            exChars[6] = '0';
            exChars[7] = (char)0;
            return exChars;
        }
        public override int GetChunkSize()
        {
            return 24;
        }
        public override void WriteToStream(BinaryWriter writer)
        {
            writer.Write(Unk0);
            writer.Write(Height);
            writer.Write(PackedDataSize);
            writer.Write(DataSize);
            writer.Write(PackedCtlSize);
            writer.Write(CtlSize);
        }
    }

    public class Hg3CPtype
    {
        public char[]? Magic { get; set; }//8bytes
        public UInt32 Unk0 { get; set; }
        public UInt32 Size { get; set; }
        public static char[] ReturnExpectChars()
        {
            char[] exChars = new char[8];
            exChars[0] = 'c';
            exChars[1] = 'p';
            exChars[2] = 't';
            exChars[3] = 'y';
            exChars[4] = 'p';
            exChars[5] = 'e';
            exChars[6] = (char)0;
            exChars[7] = (char)0;
            return exChars;
        }
        public static int GetChunkSize()
        {
            return 0x10;
        }
        public void WriteToStream(BinaryWriter writer)
        {
            var magicBytes = Encoding.ASCII.GetBytes(Magic).ToArray();
            writer.Write(magicBytes, 0, magicBytes.Length);
            writer.Write(Unk0);
            writer.Write(Size);
        }

    }

    public class Hg3Reader
    {
        private readonly BinaryReader _reader;

        public Hg3Reader(BinaryReader fileReader)
        {
            _reader= fileReader;
        }

        public Tuple<Hg3ArcHead, List<Hg3Slice>> ReadAll()
        {
            Hg3ArcHead header = new Hg3ArcHead();
            List<Hg3Slice> list = new List<Hg3Slice>();
            header.Magic = Encoding.ASCII.GetString(_reader.ReadBytes(4)).ToCharArray();
            header.SlicHeadSize = _reader.ReadUInt32();
            if (header.SlicHeadSize != 0xc)
                throw new SystemException("Unexpected slice head size");
            if (!header.Magic.SequenceEqual(Hg3ArcHead.ReturnExpectChars()))
                throw new SystemException("Unsupported file");
            int nextPosition = Hg3ArcHead.GetChunkSize();
            while (_reader.BaseStream.Position < _reader.BaseStream.Length)
            {

                var sliceHeader = new Hg3SliceHead()
                {
                    Unk0 = _reader.ReadUInt32(),
                    OffsetToNextSlice = _reader.ReadUInt32(),
                    Index = _reader.ReadUInt32()
                };

                var tagInfo = new Hg3TagInfo()
                {
                    Magic = header.Magic = Encoding.ASCII.GetString(_reader.ReadBytes(8)).ToCharArray(),
                    OffsetToNextChunk = _reader.ReadUInt32(),
                    Size = _reader.ReadUInt32()
                };

                if (!tagInfo.Magic.SequenceEqual(Hg3Stdinfo.ReturnExpectChars()))
                    throw new SystemException("Unsupported tag");

                var stdInfo = new Hg3Stdinfo
                {
                    Width=_reader.ReadUInt32(),
                    Height=_reader.ReadUInt32(),
                    Depth=_reader.ReadUInt32(),
                    OffsetX=_reader.ReadUInt32(),
                    OffsetY=_reader.ReadUInt32(),
                    TotalWidth=_reader.ReadUInt32(),
                    TotalHeight=_reader.ReadUInt32(),
                    Unk0=_reader.ReadUInt32(),
                    CanvasWidth=_reader.ReadUInt32(),
                    CanvasHeight=_reader.ReadUInt32()
                };
                var formatInfo = new Hg3FormatInfo()
                {
                    Magic = Encoding.ASCII.GetString(_reader.ReadBytes(8)).ToCharArray(),
                    OffsetToNextChunk = _reader.ReadUInt32(),
                    Size = _reader.ReadUInt32()
                };
                if (!formatInfo.Magic.SequenceEqual(Img0000Head.ReturnExpectChars()))
                    throw new SystemException("Unsupported format");
                var img0000Head = new Img0000Head()
                {
                    Unk0 = _reader.ReadUInt32(),
                    Height = _reader.ReadUInt32(),
                    PackedDataSize = _reader.ReadUInt32(),
                    DataSize = _reader.ReadUInt32(),
                    PackedCtlSize = _reader.ReadUInt32(),
                    CtlSize = _reader.ReadUInt32()
                };
                uint imagePayloadSize = img0000Head.PackedDataSize + img0000Head.PackedCtlSize;
                byte[] imageBytes = _reader.ReadBytes((int)imagePayloadSize);
                var cptypeInfo = new Hg3CPtype()
                {
                    Magic =  Encoding.ASCII.GetString(_reader.ReadBytes(8)).ToCharArray(),
                    Unk0 = _reader.ReadUInt32(),
                    Size = _reader.ReadUInt32()
                };
                if (cptypeInfo.Size != 4 || cptypeInfo.Unk0 != 0)
                    throw new SystemException("Unknown cptype info");
                byte[] cptypeBytes = _reader.ReadBytes((int)cptypeInfo.Size);
                var hg3Slice = new Hg3Slice()
                {
                    Head = sliceHeader,
                    CPtype = cptypeInfo,
                    CPtypePayloads = cptypeBytes,
                    Format = img0000Head,
                    FormatInfo = formatInfo,
                    ImagePayloads = imageBytes,
                    Tag = stdInfo,
                    TagInfo = tagInfo
                };
                list.Add(hg3Slice);
                if (hg3Slice.Head.OffsetToNextSlice == 0)
                    break;
                nextPosition += (int)hg3Slice.Head.OffsetToNextSlice;
                int expectedSize = (int)hg3Slice.Head.OffsetToNextSlice;
                int actualSize = hg3Slice.GetChunkSize();
                if (expectedSize != actualSize)
                {
                    var warningText = string.Format("At Image Index:{0}\nExpect Size:{1}\nActual Size:{2}", hg3Slice.Head.Index,expectedSize, actualSize);
                    Console.WriteLine(warningText);
                }
                _reader.BaseStream.Position = nextPosition;
            }

            return new Tuple<Hg3ArcHead, List<Hg3Slice>>(header, list);
        }
    }

    class Hg3ArcCollection
    {
        public Hg3ArcHead? Header { get; set; }
        public List<Hg3Slice>? SliceList { get; set; }
    }
    public class Program
    {
        

        public static void Main(string[] args)
        {

            if (args.Length != 2)
            {
                Console.WriteLine("Usage: hg3Tools.exe <option> <file>");
                Console.WriteLine("Option:u/p/i u:unpack p:pack i:info");
                Console.WriteLine("Case: unpack file should be hg3 file");
                Console.WriteLine("pack: pack file should be hg3 file");
                Console.WriteLine("info: file should be hg3 file");
                return;
            }

            var collection = new Hg3ArcCollection();
            var option = args[0].ToLower();
            var file = args[1].ToLower();
            using (var fileStream = File.OpenRead(file))
            {
                var bs = new BinaryReader(fileStream);
                var reader = new Hg3Reader(bs);
                var (arcHead, sliceList) = reader.ReadAll();
                collection.Header = arcHead;
                collection.SliceList = sliceList;
            }
            switch (option)
            {
                case "u":
                    foreach (var img in collection.SliceList)
                    {
                        string fileName = string.Format("{0}_{1}.hg3",file.Replace(".hg3", ""),img.Head.Index);
                        using (var fs = File.OpenWrite(fileName))
                        {
                            var bw = new BinaryWriter(fs);
                            var newHead = new Hg3ArcHead()
                            {
                                Magic = Hg3ArcHead.ReturnExpectChars(),
                                SlicHeadSize = 0xc
                            };
                            var writenImg = img;
                            writenImg.Head.Index=0;
                            writenImg.Head.OffsetToNextSlice = 0;
                            writenImg.Head.Unk0 = 0x0300;
                            newHead.WriteToStream(bw);
                            writenImg.WriteToStream(bw);
                        }
                    }
                    break;
                case "p":
                    throw new NotImplementedException("not yet");
                case "i":
                {
                    using (var fileStream = File.OpenRead(file))
                    using(var writeStream=File.OpenWrite(file.Replace(".hg3", ".json")))
                    {
                        string jsonString = JsonConvert.SerializeObject(collection, Formatting.Indented, new JsonSerializerSettings
                        {
                            TypeNameHandling = TypeNameHandling.Auto
                        });
                        var utf8Payload = new UTF8Encoding().GetBytes(jsonString);
                        writeStream.Write(utf8Payload, 0, utf8Payload.Length);
                    }

                    break;
                }
                default:
                    Console.WriteLine("Unknown option.");
                    break;
            }
        }
    }
        
}