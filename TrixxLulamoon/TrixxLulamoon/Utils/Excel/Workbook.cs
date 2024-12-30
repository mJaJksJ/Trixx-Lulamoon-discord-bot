using NPOI.SS.UserModel;

namespace TrixxLulamoon.Utils.Excel
{
    public class Workbook
    {
        public IWorkbook Instance { get; }

        public Workbook(string filePath)
        {
            using FileStream inputStream = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            Instance = WorkbookFactory.Create(inputStream);
        }

        public ISheet GetSheetAt(int index) => Instance.GetSheetAt(index);
    }
}
