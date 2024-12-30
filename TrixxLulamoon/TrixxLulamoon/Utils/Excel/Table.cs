using NPOI.SS.UserModel;

namespace TrixxLulamoon.Utils.Excel
{
    public class Table
    {
        ICellStyle DefaultCellStyle { get; set; }
        public List<Row> Rows { get; set; }

        public Table(ICellStyle defaultCellStyle)
        {
            Rows = new List<Row>();
            DefaultCellStyle = defaultCellStyle;
        }

        public void Tr(Action<Row> write)
        {
            var row = new Row();
            Rows.Add(row);
            write(row);
        }

        public void SaveChanges(ISheet sheet)
        {
            for (var i = 0; i < Rows.Count; i++)
            {
                var row = Rows[i];
                for (var j = 0; j < row.Cells.Count; j++)
                {
                    var cell = row.Cells[j];
                    var sRow = sheet.GetRow(i) ?? sheet.CreateRow(i);
                    var sCell = sRow.GetCell(j) ?? sRow.CreateCell(j);
                    sCell.SetCellType(CellType.String);
                    sCell.SetCellValue(cell.Value);
                    sCell.CellStyle = cell.Style ?? DefaultCellStyle;
                }
            }
        }
    }
}
