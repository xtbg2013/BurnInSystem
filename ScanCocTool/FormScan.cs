using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;


using System.Reflection;


using ScanCocTool.MesService;
using GlobalFiles;
using log4net;
using log4net.Config;
using System.IO;
namespace ScanCocTool
{
    public partial class FormScan : Form
    {
        private DataTable _dataTableSn = null;
        private DataTable _dataTableBh1 = null;
        private DataTable _dataTableBh2 = null;
        private JudgeCoc _judgeCoc = null;
        public FormScan()
        {
            InitializeComponent();
            InitUserInfo();
        }
        private void InitUserInfo()
        {
            this._dataTableSn = new DataTable();  
            this._dataTableSn.Columns.Add("SN", System.Type.GetType("System.String"));
            this.dataGridViewBind.DataSource = this._dataTableSn;
            this.dataGridViewBind.Columns[0].Width = (int)(this.dataGridViewBind.Width * 0.8);


            this._dataTableBh1 = new DataTable();
            this._dataTableBh1.Columns.Add("SN", System.Type.GetType("System.String"));
            this._dataTableBh1.Columns.Add("BH INFO", System.Type.GetType("System.String"));
            this._dataTableBh1.Columns.Add("ADD CURRENT", System.Type.GetType("System.String"));
            this._dataTableBh1.Columns.Add("BURN IN HOURS", System.Type.GetType("System.String"));

            this.dataGridViewBh1.DataSource = this._dataTableBh1;

            this.dataGridViewBh1.Columns[0].Width = (int)(this.dataGridViewBh1.Width * 0.3);
            this.dataGridViewBh1.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
            this.dataGridViewBh1.Columns[1].Width = (int)(this.dataGridViewBh1.Width * 0.2);
            this.dataGridViewBh1.Columns[1].SortMode = DataGridViewColumnSortMode.NotSortable;
            this.dataGridViewBh1.Columns[2].Width = (int)(this.dataGridViewBh1.Width * 0.2);
            this.dataGridViewBh1.Columns[2].SortMode = DataGridViewColumnSortMode.NotSortable;
            this.dataGridViewBh1.Columns[3].Width = (int)(this.dataGridViewBh1.Width * 0.2);
            this.dataGridViewBh1.Columns[3].SortMode = DataGridViewColumnSortMode.NotSortable;


            this._dataTableBh2 = new DataTable();
            this._dataTableBh2.Columns.Add("SN", System.Type.GetType("System.String"));
            this._dataTableBh2.Columns.Add("BH INFO", System.Type.GetType("System.String"));
            this._dataTableBh2.Columns.Add("ADD CURRENT", System.Type.GetType("System.String"));
            this._dataTableBh2.Columns.Add("BURN IN HOURS", System.Type.GetType("System.String"));

            this.dataGridViewBh2.DataSource = this._dataTableBh2;

            this.dataGridViewBh2.Columns[0].Width = (int)(this.dataGridViewBh2.Width * 0.3);
            this.dataGridViewBh2.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
            this.dataGridViewBh2.Columns[1].Width = (int)(this.dataGridViewBh2.Width * 0.2);
            this.dataGridViewBh2.Columns[1].SortMode = DataGridViewColumnSortMode.NotSortable;
            this.dataGridViewBh2.Columns[2].Width = (int)(this.dataGridViewBh2.Width * 0.2);
            this.dataGridViewBh2.Columns[2].SortMode = DataGridViewColumnSortMode.NotSortable;
            this.dataGridViewBh2.Columns[3].Width = (int)(this.dataGridViewBh2.Width * 0.2);
            this.dataGridViewBh2.Columns[3].SortMode = DataGridViewColumnSortMode.NotSortable;


            _judgeCoc = new JudgeCoc(new MesServiceClient(), CocPnTableReader.GetInstance(@"BiFiles\CFP8CocPnTable.xml"));

        }
       

        private void dataGridViewBind_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            SolidBrush b = new SolidBrush(this.dataGridViewBind.RowHeadersDefaultCellStyle.ForeColor);
            e.Graphics.DrawString((e.RowIndex + 1).ToString(System.Globalization.CultureInfo.CurrentUICulture), this.dataGridViewBind.DefaultCellStyle.Font, b, e.RowBounds.Location.X + 20, e.RowBounds.Location.Y + 4);
        }
        private bool IsInValid(string input)
        {
            string pattern = @"^[A-Za-z0-9/-]+$";
            Regex regex = new Regex(pattern);
            return regex.IsMatch(input);
        }
        private bool IsRepetitiveSn(string sn)
        {
            var count = 0;
            foreach (DataGridViewRow row in this.dataGridViewBind.Rows)
            {
                if (row.Cells[0].Value != null)
                {
                    if (sn.Equals(row.Cells[0].Value.ToString()))
                        count++;
                }
            }
            if (count > 1)
                return true;
            else
                return false;

        }
        private void ViewCocInfo(string sn)
        {
            Dictionary<string, string> info;
            _judgeCoc.JudgeCocTypeInfo(sn,out info);

        }
        private void buttonLoadFile_Click(object sender, EventArgs e)
        {
        }

        private void buttonSubmit_Click(object sender, EventArgs e)
        {

            this._dataTableBh1.Clear();
            this._dataTableBh2.Clear();
            foreach (DataGridViewRow row in this.dataGridViewBind.Rows)
            {
                if((row.Cells[0].Value != null) && (row.Cells[0].Value.ToString() != ""))
                {
                    string sn = row.Cells[0].Value.ToString();
                    if (!IsInValid(sn))
                    {
                        MessageBox.Show("您输入的SN有非法字符，内容是:" + sn + "");
                        return;
                    }
                    if (IsRepetitiveSn(sn))
                    {
                        MessageBox.Show("您输入的SN有重复，重复的内容是:" + sn + "");
                        return;
                    }  
                     
                } 
            }

            foreach (DataGridViewRow row in this.dataGridViewBind.Rows)
            {
                if ((row.Cells[0].Value != null) && (row.Cells[0].Value.ToString() != ""))
                {
                    string sn = row.Cells[0].Value.ToString();
                    Dictionary<string, string> info;
                    _judgeCoc.JudgeCocTypeInfo(sn, out info);

                    LogHelper.WriteDataLog(sn + "---->" +info["COC"]);
                    if (info["COC"] == "BH1")
                    {
                        this._dataTableBh1.Rows.Add(this._dataTableBh1.NewRow());
                        int pos = this._dataTableBh1.Rows.Count - 1;
                        this._dataTableBh1.Rows[pos][0] = sn;
                        this._dataTableBh1.Rows[pos][1] = info["COC"];
                        this._dataTableBh1.Rows[pos][2] = info["CURRENT"];
                        this._dataTableBh1.Rows[pos][3] = info["HOURS"];

                    }
                    else
                    {
                        this._dataTableBh2.Rows.Add(this._dataTableBh2.NewRow());
                        int pos = this._dataTableBh2.Rows.Count - 1;
                        this._dataTableBh2.Rows[pos][0] = sn;
                        this._dataTableBh2.Rows[pos][1] = info["COC"];
                        this._dataTableBh2.Rows[pos][2] = info["CURRENT"];
                        this._dataTableBh2.Rows[pos][3] = info["HOURS"];
                    }
                }
                   
            }
        }

        private void FormScan_Load(object sender, EventArgs e)
        {

        }
    }
}
