using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
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
        public static void Write()
        {// 创建工作薄
            IWorkbook wb = new XSSFWorkbook();
            // 创建工作表
            ISheet sheet = wb.CreateSheet("Hyperlinks");
            //-----文件保护
            //sheet.LockFormatRows();
            //sheet.LockFormatCells();
            //sheet.LockFormatColumns();
            //sheet.LockDeleteColumns();
            //sheet.LockDeleteRows();
            //sheet.LockInsertHyperlinks();
            //sheet.LockInsertColumns();
            //sheet.LockInsertRows();
            //sheet.ProtectSheet("123456");
            // 创建第一行
            IRow row = sheet.CreateRow(0);
            // 创建第一列
            ICell cell = row.CreateCell(0);
            cell.SetCellValue(123);
            //创建第一列为链接
            ICell cell1 = row.CreateCell(1);
            cell1.SetCellValue("baidu");
            //除此之外，HyperlinkType 枚举类型还可以是： HyperlinkType.File（文件路径）、HyperlinkType.Email（电子邮件地址）、HyperlinkType.Document（内部文档跳转）。
            XSSFHyperlink link = new XSSFHyperlink(HyperlinkType.Url)
            {
                Address="https://www.baidu.com"
            };
            cell1.Hyperlink = link;

            //设置单元格字体
            IFont font = wb.CreateFont();
            font.Color = IndexedColors.Red.Index;//设置颜色
            font.IsItalic = false;//斜体字
            font.Underline = FontUnderlineType.Double;//下划线
            font.IsStrikeout = false;//删除
            font.IsBold = false;//粗体
            font.FontHeightInPoints = 20;
            // 绑定字体样式到样式对象上
            ICellStyle style1 = wb.CreateCellStyle();
            style1.SetFont(font);
            cell1.CellStyle = style1;

            //设置单元格边框样式
            ICellStyle style = wb.CreateCellStyle();
            style.BorderBottom = BorderStyle.Thin;
            style.BottomBorderColor = IndexedColors.Black.Index;
            style.BorderLeft = BorderStyle.DashDotDot;
            style.LeftBorderColor = IndexedColors.Green.Index;
            style.BorderRight = BorderStyle.Hair;
            style.RightBorderColor = IndexedColors.Blue.Index;
            style.BorderTop = BorderStyle.MediumDashed;
            style.TopBorderColor = IndexedColors.Orange.Index;

            // 设置边框对角线样式
            style.BorderDiagonalLineStyle = BorderStyle.Medium; // BorderDiagonalLineStyle 属性必须在  BorderDiagonal 和 BorderDiagonalColor 之前设置
            style.BorderDiagonal = BorderDiagonal.Forward;
            style.BorderDiagonalColor = IndexedColors.Gold.Index;

            IDataFormat format = wb.CreateDataFormat();

            ICell cell3 = sheet.CreateRow(0).CreateCell(0);
            // 小数保留两位 - "1.20"
            cell3.SetCellValue(1.2);
            ICellStyle cellStyle = wb.CreateCellStyle();
            cellStyle.DataFormat = HSSFDataFormat.GetBuiltinFormat("0.00");
            cell3.CellStyle = cellStyle;
            // 绑定样式
            cell3.CellStyle = style;
            // 设置列宽，第一个参数为第几列（从 0 开始计数），第二个参数为宽度值，注意值为 256 的倍数
            sheet.SetColumnWidth(1, 100 * 256);
            sheet.SetColumnWidth(2, 150 * 256);

            // 设置行高，注意值为 20 的倍数
            sheet.CreateRow(1).Height = 200 * 20;
            sheet.CreateRow(2).Height = 300 * 20;
            cell.SetCellValue(new XSSFRichTextString("This is a test of merging"));

            ISheet sheet1 = wb.CreateSheet("new sheet");
            ISheet sheet2 = wb.CreateSheet("second sheet");
            ISheet sheet3 = wb.CreateSheet("third sheet");
            ISheet sheet4 = wb.CreateSheet("fourth sheet");

            // CreateFreezePane 方法参数说明：
            // 第一个参数表示要冻结的列数，从 1 开始计数，如果不需要冻结设为 0。
            // 第二个参数表示要冻结的行数，从 1 开始计数，如果不需要冻结设为 0。
            // 第三个参数表示右边区域可见的首列序号，从 1 开始计数，如果不需要设置则设为 0。
            // 第四个参数表示下边区域可见的首行序号，从 1 开始计数，如果不需要设置则设为 0。

            // 冻结第一行
            sheet1.CreateFreezePane(0, 1, 0, 1);
            // 冻结第一列
            sheet2.CreateFreezePane(1, 0, 1, 0);
            // 冻结列和行（忽略右下象限的滚动位置）
            sheet3.CreateFreezePane(2, 2);
            // 创建一个左下角为活动象限的分割
            sheet4.CreateSplitPane(2000, 2000, 0, 0, PanePosition.LowerLeft);
            // 参数格式：new CellRangeAddress(起始第几行，结束第几行，起始第几列，结束第几列)
            // 合并 A1 和 B1 两个单元格
            sheet.AddMergedRegion(new CellRangeAddress(1, 1, 1, 2));
            // 保存文件
            FileStream sw = File.Create("assert/test.xlsx");//new FileStream("G://test.xlsx", FileMode.Open, FileAccess.Write);//
            wb.Write(sw);
            sw.Close();
        }
        public void Read()
        {

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
        /// <summary>
        /// 简单的生成多个excel
        /// </summary>
        /// <param name="woorkBooks"></param>
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
