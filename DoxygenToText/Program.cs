using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace DoxygenToText
{
    internal class Program
    {


        private const string Charset = "utf-8";

        private static Regex regexWhitespace = new Regex("\n\\s+");

        private static void Main()

        {
            Console.WriteLine("Path?");
            var InputFileName = Console.ReadLine().Trim().Trim('"');
            var OutputFileName = "output\\Output.txt";
            if (!File.Exists(InputFileName))

            {
                Console.WriteLine( "File " + InputFileName + " not found.");
                Console.ReadKey();
                return;
            }

            StreamReader reader = null;

            StreamWriter writer = null;

            try

            {
                Encoding encoding = Encoding.GetEncoding(Charset);
                Directory.CreateDirectory("output");
            
                using (FileStream fs = File.Create(OutputFileName)) { };

                reader = new StreamReader(InputFileName, encoding);
                writer = new StreamWriter(OutputFileName,false, encoding);

                
                    RemoveHtmlTags(reader, writer);

                
            }
            catch (IOException)

            {
                Console.WriteLine(

                    "Cannot read file " + InputFileName + ".");
            }
            finally

            {
                if (reader != null)

                {
                    reader.Close();
                }

                if (writer != null)

                {
                    writer.Close();
                }
            }
            Console.WriteLine("Ready");
            Console.ReadKey();
        }

        /// <summary>Removes the tags from a HTML text</summary>

        /// <param name="reader">Input text</param>

        /// <param name="writer">Output text (result)</param>

        private static void RemoveHtmlTags( StreamReader reader, StreamWriter writer)

        {
            int openedTags = 0;

            StringBuilder buffer = new StringBuilder();

            while (true)

            {
                int nextChar = reader.Read();

                if (nextChar == -1)

                {
                    // End of file reached

                    PrintBuffer(writer, buffer);

                    break;
                }

                char ch = (char) nextChar;

                if (ch == '<')

                {
                    if (openedTags == 0)

                    {
                        PrintBuffer(writer, buffer);

                        buffer.Length = 0;
                    }

                    openedTags++;
                }
                else if (ch == '>')

                {
                    openedTags--;
                }
                else

                {
                    // We aren't in tags (not "<" or ">")

                    if (openedTags == 0)

                    {
                        buffer.Append(ch);
                    }
                }
            }

        }

        /// <summary>Removes the whitespace and prints the buffer

        /// in a file</summary>

        /// <param name="writer">the result file</param>

        /// <param name="buffer">the input for processing</param>

        private static void PrintBuffer(

            StreamWriter writer, StringBuilder buffer)

        {
            string str = buffer.ToString();

            string trimmed = str.Trim();

            string textOnly = regexWhitespace.Replace(trimmed, "\n");

            if (!string.IsNullOrEmpty(textOnly))

            {
                writer.WriteLine(textOnly);
            }
        }
    }
}