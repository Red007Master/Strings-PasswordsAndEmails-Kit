using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RedMTools;

namespace CharSniffer2._0
{
    class Other
    {
        internal static void Initialization()
        {
            Console.ForegroundColor = ConsoleColor.Green;

            Publics.OutputFileMarker = "[CharSnifferOutput]";
            Publics.SearchedString = "a.milon".ToLower();

            Publics.Dirs.InputFolder = @"C:\RedSoft";
            Publics.Dirs.OutputTxt = $@"C:\RedSoft\Output\CharSniffer\{Publics.SearchedString + Publics.OutputFileMarker}.txt";
            Publics.Dirs.TempFolder = Path.GetTempPath();

            Publics.FilesToReadArray = RedTools.Files.CollectFileArrayFromDir(Publics.Dirs.InputFolder, "txt", true);
            Publics.DataFilesArrayLenght = Publics.FilesToReadArray.Length;

            Publics.ConditionMatchingDataRaw = new List<RawConditionMatchingData>();

            Publics.TempFiles = new List<string>();

            Publics.Counter = 0;
        }
    }


    class RawConditionMatchingData
    {
        public string SourceFile { get; set; }
        public string Value { get; set; }

        public RawConditionMatchingData(string sourceFile, string value) { SourceFile = sourceFile; Value = value; }
    }

    class UnprocessedData
    {
        private static List<UnprocessedDataObj> ListMain = new List<UnprocessedDataObj>();
        public static List<UnprocessedDataObj> List { get { return ListMain; } }

        public static void Add(RawConditionMatchingData input)
        {
            ListMain.Add(new UnprocessedDataObj(input.SourceFile, input.Value));
        }
    }
    class UnprocessedDataObj
    {
        public string SourceFile { get; set; }
        public string Value { get; set; }

        public UnprocessedDataObj(string sourceFile, string value) { SourceFile = sourceFile; Value = value; }
    }

    class Credentials
    {
        private static List<CredentialsObj> ListMain = new List<CredentialsObj>();
        public static List<CredentialsObj> List { get { return ListMain; } }

        public static void Add(RawConditionMatchingData input)
        {
            string email;
            string emailKey;
            string password;
            string sourceFile = input.SourceFile;
            char splitChar = 'X';

            if (input.Value.Contains(':') || input.Value.Contains(';'))
            {
                int char1Count = 0;
                int char2Count = 0;

                for (int i = 0; i < input.Value.Length; i++)
                {
                    if (input.Value[i] == ':')
                        char1Count++;

                    if (input.Value[i] == ';')
                        char2Count++;
                }

                if (char1Count >= 1 || char2Count >= 1)
                {
                    if (char1Count == 1)
                    {
                        splitChar = ':';
                    }
                    else if (char2Count == 1)
                    {
                        splitChar = ';';
                    }
                    else
                    {
                        UnprocessedData.Add(input);
                        return;
                    }
                }

                if (splitChar != 'X')
                {
                    string[] splitBuffer1 = input.Value.Split(splitChar);
                    string[] splitBuffer2;

                    email = splitBuffer1[0];
                    password = splitBuffer1[1]; //[andrea.milonova.1@gmail.com:ANDREJKA777]

                    splitBuffer2 = email.Split('@');

                    emailKey = splitBuffer2[0];

                    bool emailKeyOccur = false;
                    for (int i = 0; i < ListMain.Count; i++)
                    {
                        if (ListMain[i].EmailKey == emailKey)
                        {
                            bool emailOccur = false;
                            for (int j = 0; j < ListMain[i].Emails.Count; j++)
                            {
                                if (ListMain[i].Emails[j] == email)
                                    emailOccur = true;
                            }

                            if (!emailOccur)
                                ListMain[i].Emails.Add(email);


                            bool passwordOccur = false;
                            for (int j = 0; j < ListMain[i].Passwords.Count; j++)
                            {
                                if (ListMain[i].Passwords[j] == password)
                                    passwordOccur = true;
                            }

                            if (!passwordOccur)
                                ListMain[i].Passwords.Add(password);


                            bool sourceFileOccur = false;
                            for (int j = 0; j < ListMain[i].SourceFiles.Count; j++)
                            {
                                if (ListMain[i].SourceFiles[j] == sourceFile)
                                    sourceFileOccur = true;
                            }

                            if (!sourceFileOccur)
                                ListMain[i].SourceFiles.Add(sourceFile);

                            emailKeyOccur = true;
                            break;
                        }
                    }

                    if (!emailKeyOccur)
                    {
                        ListMain.Add(new CredentialsObj(emailKey, email, password, sourceFile));
                    }
                }
            }
        }

        public static List<string> GetAsStringList()
        {
            List<string> result = new List<string>();

            for (int i = 0; i < ListMain.Count; i++)
            {
                if (i <= 0)
                    result.Add($"CredntialID=[{i}]");
                else
                    result.Add($"\n\nCredntialID=[{i}]");

                result.Add($"EmailKey=[{ListMain[i].EmailKey}]");

                result.Add("\nEmails:");
                for (int j = 0; j < ListMain[i].Emails.Count; j++)
                {
                    result.Add($"[{j}]=[{ListMain[i].Emails[j]}]");
                }

                result.Add("\nPasswords:");
                for (int j = 0; j < ListMain[i].Passwords.Count; j++)
                {
                    result.Add($"[{j}]=[{ListMain[i].Passwords[j]}]");
                }

                result.Add("\nSourceFiles:");
                for (int j = 0; j < ListMain[i].SourceFiles.Count; j++)
                {
                    result.Add($"[{j}]=[{ListMain[i].SourceFiles[j]}]");
                }
            }

            return result;
        }
    }
    class CredentialsObj
    {
        public string EmailKey { get; set; }
        public List<string> Emails { get; set; }
        public List<string> Passwords { get; set; }
        public List<string> SourceFiles { get; set; }

        public CredentialsObj(string emailKey, List<string> emails, List<string> passwords, List<string> sourceFiles) {EmailKey = emailKey; Emails = emails; Passwords = passwords; SourceFiles = sourceFiles;}

        public CredentialsObj(string emailKey, string email, string password, string sourceFile)
        {
            EmailKey = emailKey;
            
            Emails = new List<string>();
            Emails.Add(email);

            Passwords = new List<string>();
            Passwords.Add(password);

            SourceFiles = new List<string>();
            SourceFiles.Add(sourceFile);
        }
    }
}
