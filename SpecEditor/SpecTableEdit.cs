using System;
using System.Data;
using System.Windows.Forms;

namespace SpecEditor
{
    public partial class SpecForm
    {
        private DataTable specTable;

        private void GenerateSpecTable()
        {
            specTable = new DataTable();
            specTable.Columns.AddRange(new DataColumn[]
            {
                new DataColumn("Item"),
                new DataColumn("Type"),
                new DataColumn("LBound"),
                new DataColumn("UBound"),
            });
            dgvSpecDataGrid.DataSource = specTable;
            foreach (DataGridViewTextBoxColumn col in dgvSpecDataGrid.Columns)
                col.SortMode = DataGridViewColumnSortMode.NotSortable;
        }

        private void btnAddSpec_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(tbSpecItem.Text))
                    throw new Exception("SPEC item cannot be empty!");
                if (string.IsNullOrEmpty(cmbSpecType.Text))
                    throw new Exception("SPEC type cannot be empty!");
                if (double.Parse(tbLBound.Text) > double.Parse(tbUBound.Text))
                    throw new Exception("Range Compare Error!");
                if (specTable.Select("Item='" + tbSpecItem.Text + "'").Length > 0)
                    throw new Exception("Duplicate item is invalid!");

                var row = specTable.NewRow();
                row["Item"] = tbSpecItem.Text;
                row["Type"] = cmbSpecType.Text;
                row["LBound"] = tbLBound.Text;
                row["UBound"] = tbUBound.Text;
                specTable.Rows.Add(row);
                dgvSpecDataGrid.ClearSelection();
                tbSpecItem.Text = "";
                tbLBound.Text = "";
                tbUBound.Text = "";
                tbSpecItem.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnDeleteSpec_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvSpecDataGrid.SelectedRows.Count == 0)
                    throw new Exception("SPEC Selection cannot be empty!");
                specTable.Rows.RemoveAt(dgvSpecDataGrid.SelectedRows[0].Index);
                dgvSpecDataGrid.ClearSelection();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void dgvSpecDataGrid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                dgvSpecDataGrid.Rows[e.RowIndex].Selected = true;
                tbSpecItem.Text = (string) dgvSpecDataGrid.Rows[e.RowIndex].Cells["Item"].Value;
                cmbSpecType.Text = (string) dgvSpecDataGrid.Rows[e.RowIndex].Cells["Type"].Value;
                tbLBound.Text = (string) dgvSpecDataGrid.Rows[e.RowIndex].Cells["LBound"].Value;
                tbUBound.Text = (string) dgvSpecDataGrid.Rows[e.RowIndex].Cells["UBound"].Value;
            }
        }
    }
}
