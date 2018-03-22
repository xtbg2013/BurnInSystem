using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using BurnInUI.ConfigReader;

namespace BurnInUI.Forms
{
    public partial class ConfigUi : Form
    {
        private DataTable _dataTable;
        private static ConfigUi _inst;
        private readonly LoadConfig _reader;
        public static ConfigUi Instance(LoadConfig reader)
        {
            return _inst ?? (_inst = new ConfigUi(reader));
        }
        public ConfigUi(LoadConfig reader)
        {
            InitializeComponent();
            _reader = reader;
            InitGridView();
            GetConfigInfo();
        }
        private void InitGridView()
        {
            _dataTable = new DataTable();
            _dataTable.Columns.Add("ITEMS");
            _dataTable.Columns.Add("CONTENT");

            dataGridView.DataSource = _dataTable;

            dataGridView.Columns[0].Width = (int)(dataGridView.Width * 0.3);
            dataGridView.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridView.Columns[1].Width = (int)(dataGridView.Width * 0.6);
            dataGridView.Columns[1].SortMode = DataGridViewColumnSortMode.NotSortable;
        }
        private void GetConfigInfo()
        {
            var items =  _reader.LoadXmlForConfigUi();
            foreach (var item in items)
            {
                _dataTable.Rows.Add(_dataTable.NewRow());
                var pos = _dataTable.Rows.Count - 1;
                _dataTable.Rows[pos][0] = item.Key;
                _dataTable.Rows[pos][1] = item.Value;
            }
            
        }

        private void ConfigUi_FormClosed(object sender, FormClosedEventArgs e)
        {
            _inst = null;
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            var items = new Dictionary<string, string>() ;
            foreach (DataGridViewRow row in dataGridView.Rows)
            {
                if ((row.Cells[0].Value != null) && 
                    (row.Cells[0].Value.ToString() != "")&&
                    (row.Cells[1].Value != null)
                    )
                {
                    items[row.Cells[0].Value.ToString()] = row.Cells[1].Value.ToString();
                    
                }

            }
            _reader.Save(items);

        }

        private void ConfigUi_Load(object sender, EventArgs e)
        {

        }
    }
}
