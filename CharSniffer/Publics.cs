using System.Collections.Generic;

namespace CharSniffer2._0
{
    class Publics
    {
        public static string[] FilesToReadArray { get; set; }
        public static int DataFilesArrayLenght { get; set; }

        public static List<string> TempFiles { get; set; }

        public static List<RawConditionMatchingData> ConditionMatchingDataRaw { get; set; }

        public static long Counter { get; set; }
        public static string SearchedString { get; set; }
        public static string OutputFileMarker { get; set; }

        public static class Dirs
        {
            public static string InputFolder { get; set; }
            public static string OutputTxt { get; set; }
            public static string TempFolder { get; set; }
        }
    }
}
