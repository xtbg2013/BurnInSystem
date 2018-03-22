using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Windows.Forms;
using BILib;

namespace DataTool
{

    public enum DataTabIndex
    {
        BiResult = 0,
        BiData = 1
    }
     
    public partial class DataTool : Form
    {

        
        private static readonly object[] Items = { "BI_RESULT", "BI_DATA" };
        private DataTool()
        {
            InitializeComponent();
            starttime.Value = DateTime.Now.AddDays(-7);
        }

        private void btnsearch_Click(object sender, EventArgs e)
        {

            btnsearch.Enabled = false;
            try
            {
                if (txtsn.Text == "")
                    MessageBox.Show(@"Please key in the Serial Number before click the Searching button!");
                else
                    DisplayParaInfo(txtsn.Text, starttime.Text, endtime.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                btnsearch.Enabled = true;
            }
        }
        private void DisplayParaInfo(string snInfo,  string startTime, string endTime)
        {
            var station = tbStationName.Text;
            var conStr = ConfigurationManager.AppSettings.Get("connectionstring") ??
                         @"Data Source=<Station>\SQLEXPRESS;Initial Catalog=BMS37;Persist Security Info=True;User ID=sa;Password=cml@shg629;Pooling=False".Replace("<Station>", station);

            var dbstore = DataServiceFactory.CreateDatabaseService(conStr);

            //var summayTable = new DataTable();
            //if (radioButtonSn.Checked)
            //{
            //    summayTable = dbstore.GetBiRecord(ConvertToList(snInfo).ToArray());

            //}
            //else
            //{
            //    summayTable = dbstore.GetBiRecord(Convert.ToDateTime(startTime), Convert.ToDateTime(endTime));

            //}
            var summayTable = dbstore.GetBiRecord(ConvertToList(snInfo).ToArray(), Convert.ToDateTime(startTime), Convert.ToDateTime(endTime));
            dataGridBiSummary.DataSource = summayTable;




            var biDataTable = new DataTable();
            foreach (DataRow row in summayTable.Rows)
            {
                 
                var table = dbstore.GetBiData(new Guid(row["DataSetId"].ToString()));
                biDataTable.Merge(table);
            }

            dataGridBiData.DataSource = biDataTable;
           
        }

        private List<string> ConvertToList(string info)
        {
            var array = info.Split(new[] { ',', '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            var ret = new List<string>();
            foreach (var item in array)
                ret.Add(item.Trim());
            return ret;
        }

        private static DataTool _inst;
        public static DataTool GetInstance()
        {
            return _inst ?? (_inst = new DataTool());
        }

        private void SearchUI_FormClosed(object sender, FormClosedEventArgs e)
        {
            _inst = null;
        }

        private void DataTool_Load(object sender, EventArgs e)
        {
            comboBoxExportData.Items.AddRange(Items);
            comboBoxExportData.SelectedIndex = 0;

        }

        private DataTable ReadDataTable(DataTabIndex index)
        {
            DataTable table = new DataTable();
            switch (index)
            {
                case DataTabIndex.BiResult:
                    table = (DataTable)dataGridBiSummary.DataSource;
                    break;
                case DataTabIndex.BiData:
                    table = (DataTable)dataGridBiData.DataSource;
                    break;
                
            }
            return table ?? new DataTable();

        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            var save = new SaveFileDialog
            {
                Filter = @"xlsx files   (*.xlsx)|*.xlsx",
                FilterIndex = 2,
                RestoreDirectory = true
            };

            if (save.ShowDialog() == DialogResult.OK)
            {
                
                DataTable data = ReadDataTable((DataTabIndex)comboBoxExportData.SelectedIndex);
                BaseExportData export = new ExportBiData();
                export.ExportDataToExcel(save.FileName, data);
            }

        }

    }
}
