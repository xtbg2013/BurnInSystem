using System;
using System.Data;
using System.Windows.Forms;

namespace SpecEditor
{
    public partial class SpecForm
    {
        private DataTable configTable;

        private void GenerateConfigTable()
        {
            configTable = new DataTable();
            configTable.Columns.AddRange(new DataColumn[]
            {
                new DataColumn("Item"),
                new DataColumn("Value"),  
            });
            dgvConfigDataGrid.DataSource = configTable;
            foreach (DataGridViewTextBoxColumn col in dgvConfigDataGrid.Columns)
                col.SortMode = DataGridViewColumnSortMode.NotSortable;
        }
        
        private void btnAddConfig_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(tbConfigItem.Text))
                    throw new Exception("CONFIG item cannot be empty!");
                if (string.IsNullOrEmpty(tbConfigValue.Text))
                    throw new Exception("CONFIG value cannot be empty!");
                if (configTable.Select("Item='" + tbConfigItem.Text + "'").Length > 0)
                    throw new Exception("Duplicate item is invalid!");

                var row = configTable.NewRow();
                row["Item"] = tbConfigItem.Text;
                row["Value"] = tbConfigValue.Text;
                configTable.Rows.Add(row);
                dgvConfigDataGrid.ClearSelection();
                tbConfigItem.Text = "";
                tbConfigValue.Text = "";
                tbConfigItem.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnDeleteConfig_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvConfigDataGrid.SelectedRows.Count == 0)
                    throw new Exception("CONFIG Selection cannot be empty!");
                configTable.Rows.RemoveAt(dgvConfigDataGrid.SelectedRows[0].Index);
                dgvConfigDataGrid.ClearSelection();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void dgvConfigDataGrid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                dgvConfigDataGrid.Rows[e.RowIndex].Selected = true;
                tbConfigItem.Text = (string)dgvConfigDataGrid.Rows[e.RowIndex].Cells["Item"].Value;
                tbConfigValue.Text = (string) dgvConfigDataGrid.Rows[e.RowIndex].Cells["Value"].Value;
            }
        }
    }
}
