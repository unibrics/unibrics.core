namespace Unibrics.Core.Utils.Csv
{
    public class CsvConfig
    {
        public char Delimiter { get; }
        public string NewLineMark { get; }
        public char QuotationMark { get; }

        public CsvConfig(char delimiter, string newLineMark, char quotationMark)
        {
            Delimiter = delimiter;
            NewLineMark = newLineMark;
            QuotationMark = quotationMark;
        }
        
        public static CsvConfig Default => new CsvConfig(';', "\r\n", '\"');

    }
}