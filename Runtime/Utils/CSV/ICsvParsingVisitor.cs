namespace Unibrics.Core.Utils.Csv
{
    public interface ICsvParsingVisitor
    {
        void OnCellParsed(string value);
        
        void OnLineEnd();

        void OnFileEnd();
    }
}