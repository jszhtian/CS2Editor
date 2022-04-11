using System.Diagnostics;
using System.Reflection;
using System.Reflection.PortableExecutable;
using System.Runtime.CompilerServices;
using System.Security;
using System.Security.Permissions;
using System.Text;
using ICSharpCode.SharpZipLib.Zip.Compression;
using ICSharpCode.SharpZipLib.Zip.Compression.Streams;

namespace fesTools
{

    class fesReader
    {
        
        private BinaryReader reader;

        public Stream readFesFile()
        {

            var FileMagic = reader.ReadChars(4);
            var CompressedSize = reader.ReadInt32();
            var UncompressSize = reader.ReadInt32();
            var ZeroPadding = reader.ReadInt32();
            char[] magic = new char[4];
            magic[0] = 'F';
            magic[1] = 'E';
            magic[2] = 'S';
            magic[3] = (char)0;
            if (!FileMagic.SequenceEqual(magic))
                throw new SystemException("Not a fes file!");
            var deflateBuffer = reader.ReadBytes(CompressedSize);
            var output = new MemoryStream();
            using (var compressStream = new MemoryStream(deflateBuffer))
            using (var s = new InflaterInputStream(compressStream))
            {
                s.Flush();
                s.CopyTo(output);
            }
            output.Position = 0;
            return output;

        }
        public fesReader(BinaryReader filerReader)
        {
            reader=filerReader;
        }
    }

    class fesWriter
    {
        private BinaryWriter writer;

        public void WriteStream(Stream textStream)
        {
            char[] magic = new char[4];
            magic[0] = 'F';
            magic[1] = 'E';
            magic[2] = 'S';
            magic[3] = (char)0;
            writer.Write(magic);
            Deflater defl = new Deflater(9, false);
            using (MemoryStream buffer = new MemoryStream())
            using (DeflaterOutputStream s = new DeflaterOutputStream(buffer, defl,0x1000))
            {
                textStream.CopyTo(s);
                s.Finish();
                buffer.Position = 0;
                writer.Write((int)buffer.Length);
                writer.Write((int)textStream.Length);
                writer.Write(0);
                writer.Write(buffer.ToArray());
            }

            
            


        }

        public fesWriter(BinaryWriter fileWriter)
        {
            writer=fileWriter;
        }
    }

    class Program
    {
        

        private static void Main(string[] args)
        {
            if (args.Length != 2)
            {
                Console.WriteLine("Usage: fesTools.exe <option> <file>");
                Console.WriteLine("Option:u/p u:unpack p:pack");
                Console.WriteLine("Case: unpack file should be fes");
                Console.WriteLine("pack: unpack file should be plaintext");
                return;
            }

            var option = args[0];
            var file = args[1];
            if (option == "u")
            {
                using (var filestream = File.OpenRead(file))
                using (var writestream = File.OpenWrite(file.Replace(".fes", ".txt")))
                {
                    var bs = new BinaryReader(filestream);
                    var reader = new fesReader(bs);
                    reader.readFesFile().CopyTo(writestream);
                    writestream.Position = 0;
                }
            }
            else if (option=="p")
            {
                using (var filestream = File.OpenRead(file))
                using (var writestream = File.OpenWrite(file.Replace(".txt", ".fes")))
                {
                    var bwriter = new BinaryWriter(writestream);
                    var writer = new fesWriter(bwriter);
                    writer.WriteStream(filestream);
                }
            }
            else
            {
                Console.WriteLine("Unknown option.");
            }
        }
}

}