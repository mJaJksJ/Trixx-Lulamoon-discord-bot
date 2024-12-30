using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace TrixxLulamoon.Utils.Excel
{
    public static class Style
    {
        [Flags]
        public enum Border
        {
            None = 0,
            Bottom = 1,
            Left = 2,
            Right = 4,
            Top = 8
        }

        public static XSSFCellStyle Default(IWorkbook workbook, Border border = Border.None, XSSFColor color = null)
        {
            var style = (XSSFCellStyle)workbook.CreateCellStyle();

            XSSFFont hFont = (XSSFFont)workbook.CreateFont();
            hFont.FontHeightInPoints = 12;
            hFont.FontName = "Times New Roman";
            style.VerticalAlignment = VerticalAlignment.Center;
            style.WrapText = true;
            style.SetFont(hFont);

            if (border.HasFlag(Border.Bottom)) { style.BorderBottom = BorderStyle.Thin; }
            if (border.HasFlag(Border.Left)) { style.BorderLeft = BorderStyle.Thin; }
            if (border.HasFlag(Border.Right)) { style.BorderRight = BorderStyle.Thin; }
            if (border.HasFlag(Border.Top)) { style.BorderTop = BorderStyle.Thin; }

            if (color != null)
            {
                style.SetFillForegroundColor(color);
                style.FillPattern = FillPattern.SolidForeground;
            }

            return style;
        }

        public static XSSFCellStyle DefaultWithBorder(IWorkbook workbook) => Default(workbook, Border.Bottom | Border.Left | Border.Right | Border.Top);
    }
}
