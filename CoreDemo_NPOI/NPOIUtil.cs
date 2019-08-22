using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CoreDemo_NPOI
{
    public class NPOIUtil
    {
        public static void Test()
        {// 创建工作薄
            IWorkbook wb = new XSSFWorkbook();
            // 创建工作表
            ISheet sheet = wb.CreateSheet("Hyperlinks");

            // 创建第一行
            IRow row = sheet.CreateRow(0);

            // 创建第一列
            ICell cell = row.CreateCell(0);
            cell.SetCellValue(123);
            // 保存文件
            FileStream sw = File.Create("assert/test.xlsx");//new FileStream("G://test.xlsx", FileMode.Open, FileAccess.Write);//
            wb.Write(sw);
            sw.Close();
        }
        /// <summary>
        /// excel保存
        /// </summary>
        /// <param name="path">地址</param>
        /// <param name="name">文件名称不加后缀</param>
        /// <param name="suffix">文件后缀 .xlsx 或者 xls</param>
        public static void CreateExcel(string path, string name, string suffix, List<string> sheetList, List<List<string>> CellTitleList)
        {
            IWorkbook workbook;
            if (suffix.ToLower() == ".xls")
            {
                workbook = new HSSFWorkbook();
            }
            else
            {
                workbook = new XSSFWorkbook();
            }


        }
        public static void T()
        {
            BatchWoorkBook(new List<WoorkBook> {
                new WoorkBook(){Name="文件1",Path="assert",Suffix=".xlsx",
                    Sheets =new List<Sheet>(){
                        new Sheet() {
                        Name="sheet1",
                        Title=new List<string>(){ "a","b"},
                        Value=new List<List<object>>(){
                            new List<object>(){1,2 },
                            new List<object>(){"微笑","dd" }
                        }
                    }
                    }
                }
            });
        }
        public static void BatchWoorkBook(List<WoorkBook> woorkBooks)
        {
            IWorkbook _workbook;
            ISheet _sheet;
            foreach (var wook in woorkBooks)
            {
                if (wook.Suffix == ".xls")
                {
                    _workbook = new HSSFWorkbook();
                }
                else
                {
                    _workbook = new XSSFWorkbook();
                }
                foreach (var sheet in wook.Sheets)
                {
                    _sheet = _workbook.CreateSheet(sheet.Name);
                    IRow _row = _sheet.CreateRow(0);
                    for (int i = 0; i < sheet.Title.Count; i++)
                    {
                        ICell _cell = _row.CreateCell(i);
                        _cell.SetCellValue(sheet.Title[i]);
                    }
                    for (int i = 0; i < sheet.Value.Count; i++)
                    {
                        IRow rowValue = _sheet.CreateRow(i + 1);
                        for (int j = 0; j < sheet.Value[i].Count; j++)
                        {
                            ICell cellValue = rowValue.CreateCell(j);
                            cellValue.SetCellValue(sheet.Value[i][j].ToString());
                        }
                    }
                }
                FileStream fileStream = File.Create(wook.Path + "/" + wook.Name + wook.Suffix);
                _workbook.Write(fileStream);
                fileStream.Close();
            }
        }
    }
    public class WoorkBook
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public string Suffix { get; set; }
        public List<Sheet> Sheets { get; set; }
    }
    public class Sheet
    {
        public string Name { get; set; }
        public List<string> Title { get; set; }
        public List<List<object>> Value { get; set; }
    }
    public class Rows
    {

    }
    public class Cells
    {

    }
}
