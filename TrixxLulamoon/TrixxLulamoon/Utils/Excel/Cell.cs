using NPOI.SS.UserModel;

namespace TrixxLulamoon.Utils.Excel
{
    public class Cell
    {
        public string Value { get; }
        public ICellStyle Style { get; }

        public Cell(string value, ICellStyle style)
        {
            Value = value;
            Style = style;
        }
    }
}
