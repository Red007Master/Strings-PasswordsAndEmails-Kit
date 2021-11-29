using RedMTools;
using System;
using System.Collections.Generic;
using System.IO;

namespace CharSniffer2._0
{
    class Work
    {
        internal static void StartMain()
        {
            MainLoop();

            SortRawData();

            Output();

            CleanUpTemp();
        }

        private static void MainLoop()
        {
            DateTime start;
            DateTime end;

            for (int i = 0; i < Publics.FilesToReadArray.Length; i++)
            {
                string[] splitBuffer = Publics.FilesToReadArray[i].Split(Convert.ToChar(@"\"));

                start = DateTime.Now;
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write($"\nStart reading [{splitBuffer[splitBuffer.Length - 1]}] [{i + 1}/{ Publics.DataFilesArrayLenght}] in [{start}] > ");
                Console.ForegroundColor = ConsoleColor.Green;

                ReadAndDetectSearchedStringInFile(Publics.FilesToReadArray[i]);

                end = DateTime.Now;
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"Complete in [{end}], time spent [{end - start}]");
                Console.ForegroundColor = ConsoleColor.Green;
            }
        }

        private static void ReadAndDetectSearchedStringInFile(string fileDir)
        {
            string line;

            if (fileDir.Contains(Publics.OutputFileMarker))
                fileDir = FormatCharSnifferOutputFile(fileDir);

            using (StreamReader sr = new StreamReader(fileDir, System.Text.Encoding.Default))
            {
                while ((line = sr.ReadLine()) != null)
                {
                    if (line.ToLower().Contains(Publics.SearchedString))
                    {
                        Publics.ConditionMatchingDataRaw.Add(new RawConditionMatchingData(fileDir, line));
                    }
                    //Counter++;
                }
            }
        }

        private static void SortRawData()
        {
            for (int i = 0; i < Publics.ConditionMatchingDataRaw.Count; i++)
            {
                Credentials.Add(Publics.ConditionMatchingDataRaw[i]);
            }
        }

        private static void Output()
        {
            List<string> temp1 = Credentials.GetAsStringList();

            Console.WriteLine("\n\n");
            for (int i = 0; i < temp1.Count; i++)
            {
                Console.WriteLine(temp1[i]);
            }

            RedTools.Files.Write.WriteInTxt(Publics.Dirs.OutputTxt, temp1.ToArray());
        }


        private static string FormatCharSnifferOutputFile(string fileDir)
        {
            string result = "";

            string[] fileContent = RedTools.Files.Read.GetTXTFileContent(fileDir);
            List<string> formattedFileContent = new List<string>();

            string buffer1;
            string buffer2;
            string[] buffer3;

            for (int i = 0; i < fileContent.Length; i++)
            {
                buffer1 = fileContent[i];
                buffer2 = "";

                buffer1 = buffer1.Replace("Line=[", "");

                for (int j = 0; j < buffer1.Length; j++)
                {
                    if (buffer1[j] == ']' && buffer1[j + 1] == ' ' && buffer1[j + 2] == 'i' && buffer1[j + 3] == 'n' && buffer1[j + 4] == ' ' && buffer1[j + 5] == '[')
                    {
                        break;
                    }
                    else
                    {
                        buffer2 += buffer1[j];
                    }
                }

                formattedFileContent.Add(buffer2);
            }

            buffer1 = "";
            buffer3 = fileDir.Split('.');

            for (int i = 0; i < buffer3.Length - 1; i++)
            {
                if (i <= 0)
                {
                    buffer1 += $"{buffer3[i]}";
                }
                else if (i >= 0)
                {
                    buffer1 += $".{buffer3[i]}";
                }
            }

            buffer1 += "[temp].txt";

            RedTools.Files.Write.WriteInTxt(buffer1, formattedFileContent.ToArray());

            Publics.TempFiles.Add(buffer1);

            result = buffer1;

            return result;
        }

        private static void CleanUpTemp()
        {
            foreach (var file in Publics.TempFiles)
            {
                File.Delete(file);
            }
        }
    }
}
