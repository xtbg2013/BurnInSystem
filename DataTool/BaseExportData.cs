using System.Data;

namespace DataTool
{

    internal abstract class BaseExportData
    {
       public abstract bool ExportDataToExcel(string path,DataTable data);
       public abstract bool ExportDataToHtml(string path,DataTable data);
    }
}
