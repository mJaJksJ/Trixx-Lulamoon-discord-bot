using NPOI.SS.UserModel;

namespace TrixxLulamoon.Utils.Excel
{
    public class Row
    {
        public List<Cell> Cells { get; }
        public Row()
        {
            Cells = new List<Cell>();
        }

        public void Td(string value, ICellStyle style = null)
        {
            var cell = new Cell(value, style);
            Cells.Add(cell);
        }
    }
}
