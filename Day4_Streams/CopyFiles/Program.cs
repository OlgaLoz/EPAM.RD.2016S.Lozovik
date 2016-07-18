using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace FileStreams
{
    public class Program
    {
        public static void Main(string[] args)
        {
            /*   if (args.Length < 2)
               {
                   Console.WriteLine("Arguments: <source> <destination>");
                   Console.ReadLine();
                   return;
               }

               string source = args[0];
               string destin = args[1];*/

            //  ByteCopy(source, destin);
            //  BlockCopy("TextFile.txt", "out.txt");
            //  LineCopy("TextFile.txt", "out.txt");
             //  MemoryBufferCopy("TextFile.txt", "out.txt");
                WebClient();
            Console.ReadLine();
        }

        public static void ByteCopy(string source, string destin)
        {
            int bytesCounter = 0;

            // TODO: Implement byte-copy here.
            
            using (var sourceStream = new FileStream(source,FileMode.Open, FileAccess.Read))
            using (var destinStream = new FileStream(destin, FileMode.OpenOrCreate, FileAccess.Write))
            {
                int b;
                while ((b = sourceStream.ReadByte()) != -1) // TODO: read byte
                {
                    bytesCounter++;
                    destinStream.WriteByte((byte)b); // TODO: write byte
                }
            }
            
            Console.WriteLine("ByteCopy() done. Total bytes: {0}", bytesCounter);
        }

        public static void BlockCopy(string source, string destin)
        {
            // TODO: Implement block copy via buffer.
            int bufSize = 1024;
            using (var sourceStream = new FileStream(source, FileMode.Open, FileAccess.Read))
            using (var destinStream = new FileStream(destin, FileMode.OpenOrCreate, FileAccess.Write))
            {
                byte[] buffer = new byte[bufSize];
                int bytesRead;

                do
                {
                    bytesRead = sourceStream.Read(buffer, 0, bufSize); // TODO: read in buffer
                    
                    Console.WriteLine("BlockCopy(): writing {0} bytes.", bytesRead);
                    destinStream.Write(buffer, 0 , bytesRead); // TODO: write to buffer
                }
                while (bytesRead == buffer.Length);
            }
        }

        public static void LineCopy(string source, string destin)
        {
            int linesCount = 0;

            // TODO: implement copying lines using StreamReader/StreamWriter.
            
            using (var sourceStream = new FileStream(source, FileMode.Open, FileAccess.Read))
            using (var destinStream = new FileStream(destin, FileMode.OpenOrCreate, FileAccess.Write))
            {
                using (var streamReader = new StreamReader(sourceStream))
                using (var streamWriter = new StreamWriter(destinStream))
                {
                    while (true)
                    {
                        linesCount++;
                        string line;
                        if ((line = streamReader.ReadLine()) == null) // TODO: read line
                        {
                            break;
                        }
                        streamWriter.WriteLine(line); // TODO: write line
                    }
                }
            }
            
            Console.WriteLine("LineCopy(): {0} lines.", linesCount);
        }

        public static void MemoryBufferCopy(string source, string destin)
        {
            var stringBuilder = new StringBuilder();

            string content;

            using (var textReader = (TextReader)new StreamReader(source)) // TODO: use StreamReader here
            {
                content = textReader.ReadToEnd(); // TODO: read entire file
            }

            using (var sourceReader = new StringReader(content)) // TODO: Use StringReader here with content
            using (var sourceWriter = new StringWriter(stringBuilder)) // TODO: Use StringWriter here with stringBuilder
            {
                string line;

                do
                {
                    line = sourceReader.ReadLine(); // TODO: read line
                    if (line != null)
                    {
                        sourceWriter.WriteLine(line); // TODO: write line
                    }

                } while (line != null);
            }

            Console.WriteLine("MemoryBufferCopy(): chars in StringBuilder {0}", stringBuilder.Length);

            const int blockSize = 1024;

            using (var stringReader = new StringReader(stringBuilder.ToString())) // TODO: Use StringReader to read from stringBuilder.
            using (var memoryStream = new MemoryStream(blockSize))
            using (var streamWriter = new StreamWriter(memoryStream)) // TODO: Compose StreamWriter with memory stream.
            using (var destinStream = new FileStream(destin, FileMode.OpenOrCreate, FileAccess.Write)) // TODO: Use file stream.
            {
                char[] buffer = new char[blockSize];
                int bytesRead;

                do
                {
                    bytesRead = stringReader.ReadBlock(buffer, 0, blockSize); // TODO: Read block from stringReader to buffer.
                    streamWriter.Write(buffer, 0, bytesRead); // TODO: Write buffer to streamWriter.

                    // TODO: After implementing everything check the content of NewTextFile. What's wrong with it, and how this may be fixed?
                }
                while (bytesRead == blockSize);
                destinStream.Write(memoryStream.GetBuffer(), 0, memoryStream.GetBuffer().Length); // TODO: write memoryStream.GetBuffer() content to destination stream.
            }
        }

        public static void WebClient()
        {
            WebClient webClient = new WebClient();
            using (var stream = webClient.OpenRead("http://google.com"))
            {

                Console.WriteLine("WebClient(): CanRead = {0}", stream.CanRead); // TODO: print if it is possible to read from the stream
                Console.WriteLine("WebClient(): CanWrite = {0}", stream.CanWrite); // TODO: print if it is possible to write to the stream
                Console.WriteLine("WebClient(): CanSeek = {0}", stream.CanSeek); // TODO: print if it is possible to seek through the stream

                // TODO: Save steam content to "google_request.txt" file.
                using (var sr = new StreamReader(stream, Encoding.Unicode))
                using (var fs = new FileStream("google_request.txt", FileMode.Create, FileAccess.Write))
                using (var sw = new StreamWriter(fs))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        sw.WriteLine(line);
                    }
                }
            }
        }
    }
}