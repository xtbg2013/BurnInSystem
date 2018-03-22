using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using  Spire.Xls;
namespace DataTool
{

    internal class ExportBiData:BaseExportData
    {
        public override bool ExportDataToExcel(string path,DataTable data)
        {
            if (data == null || data.Rows.Count==0)return false;
            try
            {
                Workbook workbook = new Workbook();
                Worksheet sheet = workbook.Worksheets[0];
                sheet.InsertDataTable(data, true, 1, 1, true);
                CellStyle style = sheet.Range[$"A1:{(char)('A' + data.Columns.Count)}1"].Style;
                style.Font.Color = Color.White;//设置字体颜色为白色
                style.KnownColor = ExcelColors.Green;//设置单元格的背景颜色为绿色
                style.Font.IsBold = true;//设置字体加粗
                style.HorizontalAlignment = HorizontalAlignType.Center;//设置文本水平居中
                style.VerticalAlignment = VerticalAlignType.Center;//设置文本垂直居中
                style.Borders[BordersLineType.EdgeLeft].LineStyle = LineStyleType.Thin;//设置左边的border
                style.Borders[BordersLineType.EdgeRight].LineStyle = LineStyleType.Thin;//设置右边的border
                style.Borders[BordersLineType.EdgeTop].LineStyle = LineStyleType.Thin;//设置上面的border
                style.Borders[BordersLineType.EdgeBottom].LineStyle = LineStyleType.Thin;//设置下面的border

                CellStyle oddStyle = sheet.Workbook.Styles.Add("oddStyle");//创建一个style并命名为"oddStyle"
                oddStyle.Borders[BordersLineType.EdgeLeft].LineStyle = LineStyleType.Thin;
                oddStyle.Borders[BordersLineType.EdgeRight].LineStyle = LineStyleType.Thin;
                oddStyle.Borders[BordersLineType.EdgeTop].LineStyle = LineStyleType.Thin;
                oddStyle.Borders[BordersLineType.EdgeBottom].LineStyle = LineStyleType.Thin;
                oddStyle.KnownColor = ExcelColors.LightGreen1;

                CellStyle evenStyle = sheet.Workbook.Styles.Add("evenStyle");//创建一个style并命名为"evenStyle"
                evenStyle.Borders[BordersLineType.EdgeLeft].LineStyle = LineStyleType.Thin;
                evenStyle.Borders[BordersLineType.EdgeRight].LineStyle = LineStyleType.Thin;
                evenStyle.Borders[BordersLineType.EdgeTop].LineStyle = LineStyleType.Thin;
                evenStyle.Borders[BordersLineType.EdgeBottom].LineStyle = LineStyleType.Thin;
                evenStyle.KnownColor = ExcelColors.LightTurquoise;

                //为A3到J20的单元格设置格式，如果是奇数行使用oddStyle，如果是偶数行使用evenStyle
                foreach (CellRange range in sheet.Range[$"A2:{(char)('A' + data.Columns.Count)}{data.Rows.Count + 1}"].Rows)
                {
                    range.CellStyleName = range.Row % 2 == 0 ? evenStyle.Name : oddStyle.Name;
                }
                sheet.AllocatedRange.AutoFitColumns();//自动调整列的宽度去适应单元格的数据
                sheet.AllocatedRange.AutoFitRows();   //自动调整行的高度去适应单元格的数据
                sheet.AutoFilters.Range = sheet.Range[$"A1:{(char)('A' + data.Columns.Count)}{data.Rows.Count}"];

                workbook.SaveToFile(path, ExcelVersion.Version2013);
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show($"Export data error,message = {e.Message}");
                return false;
            }
            
            
        }

        public override bool ExportDataToHtml(string path,DataTable data)
        {
            return true;

        }
    }
}
