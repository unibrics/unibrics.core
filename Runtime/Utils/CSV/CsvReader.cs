namespace Unibrics.Core.Utils.Csv
{
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using UnityEngine;

    public class CsvReader
    {
        private readonly CsvConfig config;
        
        private readonly StringBuilder sb = new();

        private readonly ICsvParsingVisitor visitor;

        public CsvReader(ICsvParsingVisitor visitor, CsvConfig config = null)
        {
            this.visitor = visitor;
            this.config = config ?? CsvConfig.Default;
        }

        public void Read(string csvFileContents)
        {
            using var reader = new StringReader(csvFileContents);
            while (true)
            {
                var fullLine = ParseNextLine(reader);
                if (fullLine == null)
                {
                    break;
                }
                    
                if (fullLine != "")
                {
                    ParseLine(fullLine);    
                }
            }
            
            visitor.OnFileEnd();

            string ParseNextLine(TextReader rd)
            {
                sb.Clear();
                string result;
                do
                {
                    var line = rd.ReadLine();
                    if (line == null)
                    {
                        return null;
                    }

                    if (sb.Length > 0)
                    {
                        sb.Append("\n");
                    }
                    sb.Append(line);
                    result = sb.ToString();
                } while (result.Count(c => c == '\"') % 2 != 0);

                return result;
            }
        }

        private void ParseLine(string line)
        {
            var i = 0;
            while (true)
            {
                var cell = ParseNextCell(line, ref i);
                if (cell == null)
                {
                    break;
                }

                visitor.OnCellParsed(cell);
            }
            
            visitor.OnLineEnd();
        }

        // returns iterator after delimiter or after end of string
        private string ParseNextCell(string line, ref int i)
        {
            if (i >= line.Length)
            {
                return null;
            }

            return line[i] != config.QuotationMark ? ParseNotEscapedCell(line, ref i) : ParseEscapedCell(line, ref i);
        }

        // returns iterator after delimiter or after end of string
        private string ParseNotEscapedCell(string line, ref int i)
        {
            sb.Clear();
            while (true)
            {
                if (i >= line.Length) // return iterator after end of string
                    break;
                if (line[i] == config.Delimiter)
                {
                    i++; // return iterator after delimiter
                    break;
                }

                sb.Append(line[i]);
                i++;
            }

            return sb.ToString();
        }

        // returns iterator after delimiter or after end of string
        private string ParseEscapedCell(string line, ref int i)
        {
            i++; // omit first character (quotation mark)
            sb.Clear();
            var slice = line;
            while (true)
            {
                if (i >= line.Length)
                {
                    break;
                }
                
                if (line[i] == config.QuotationMark)
                {
                    i++; // we're more interested in the next character
                    if (i >= line.Length)
                    {
                        // quotation mark was closing cell;
                        // return iterator after end of string
                        break;
                    }

                    if (line[i] == config.Delimiter)
                    {
                        // quotation mark was closing cell;
                        // return iterator after delimiter
                        i++;
                        break;
                    }

                    if (line[i] == config.QuotationMark)
                    {
                        // it was doubled (escaped) quotation mark;
                        // do nothing -- we've already skipped first quotation mark
                    }
                }

                sb.Append(line[i]);
                i++;
            }

            return sb.ToString();
        }
    }

}