using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpecEditor
{
    public partial class SpecForm : Form
    {
        private SpecService specService;
        private DataTable specificationList;
        public SpecForm()
        {
            InitializeComponent();
            GenerateSpecTable();
            GenerateConfigTable();
            var conStr = ConfigurationManager.AppSettings.Get("connectionstring") ??
                         @"Data Source = LocalHost\SQLEXPRESS; Initial Catalog = BMS37; Persist Security Info = True; User ID = sa; Password = cml@shg629; Pooling = False";
            specService = new SpecService(conStr);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                var obj = GenerateBISpecification();
                using (var dlg = new SaveFileDialog() {DefaultExt = "json"})
                {
                    dlg.ShowDialog();
                    using (StreamWriter sw = File.CreateText(dlg.FileName))
                    {
                        sw.Write(BISpecification.Serialize(obj));
                        sw.Close();
                    }
                }
                MessageBox.Show("Save File Successful!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private BISpecification GenerateBISpecification()
        {
            if (string.IsNullOrEmpty(tbPlan.Text))
                throw new Exception("PLAN cannot be empty!");
            if (string.IsNullOrEmpty(tbVersion.Text))
                throw new Exception("VERSION cannot be empty!");
            if (string.IsNullOrEmpty(tbDriver.Text))
                throw new Exception("DRIVER cannot be empty!");
            double.Parse(tbSpan.Text);
            double.Parse(tbInterval.Text);
            var ret = new BISpecification();
            ret.Plan = tbPlan.Text;
            ret.Version = tbVersion.Text;
            ret.Driver = tbDriver.Text;
            ret.Span = tbSpan.Text;
            ret.Interval = tbInterval.Text;
            ret.Configuration = new List<ConditionItem>();
            foreach(DataRow row in configTable.Rows)
                ret.Configuration.Add(new ConditionItem() {Item=row["Item"].ToString(),Value=row["Value"].ToString()});
            ret.Specification = new List<SpecItem>();
            foreach(DataRow row in specTable.Rows)
                ret.Specification.Add(new SpecItem()
                {
                    Item = row["Item"].ToString(),
                    Type = row["Type"].ToString(),
                    LBound = row["LBound"].ToString(),
                    UBound = row["UBound"].ToString(),
                });
            return ret;
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            try
            { 
                var result = MessageBox.Show("Current content will be overwrite!\nChoose YES to confirm, Choose NO to abort.",
                    "Load Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (DialogResult.No == result)
                    return;
                var dlg = new OpenFileDialog() {Filter = "json text|*.json|plain text|*.txt|All files(*.*)|*.*" };
                var load = dlg.ShowDialog();
                if (DialogResult.OK == load)
                {
                    using (StreamReader sr = File.OpenText(dlg.FileName))
                    {
                        var obj = BISpecification.Deserialize(sr.ReadToEnd());
                        sr.Close();
                        LoadBISpecification(obj);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void LoadBISpecification(BISpecification biSpec)
        {
            tbPlan.Text = biSpec.Plan;
            tbVersion.Text = biSpec.Version;
            tbDriver.Text = biSpec.Driver;
            tbSpan.Text = biSpec.Span;
            tbInterval.Text = biSpec.Interval;
            configTable.Rows.Clear();
            specTable.Rows.Clear();
            foreach (var item in biSpec.Configuration)
            {
                var row = configTable.NewRow();
                row["Item"] = item.Item;
                row["Value"] = item.Value;
                configTable.Rows.Add(row);
            }
            foreach (var item in biSpec.Specification)
            {
                var row = specTable.NewRow();
                row["Item"] = item.Item;
                row["Type"] = item.Type;
                row["LBound"] = item.LBound;
                row["UBound"] = item.UBound;
                specTable.Rows.Add(row);
            }
        }

        private void btnCommit_Click(object sender, EventArgs e)
        {
            try
            {
                var obj = GenerateBISpecification();
                specService.CommitSpecification(obj);
                MessageBox.Show("Commit Successful!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnFetch_Click(object sender, EventArgs e)
        {
            specificationList = specService.GetSpecificationList();
            cmbPlanFilter.Items.Clear();
            cmbPlanFilter.Items.Add("");
            foreach(DataRow row in specificationList.Rows)
                if(cmbPlanFilter.Items.Contains(row["Plan"])==false)
                    cmbPlanFilter.Items.Add(row["Plan"]);
            
            DisplaySpecificationList();
        }

        private void DisplaySpecificationList()
        {
            DataTable displayList = specificationList.Copy();
            if (cmbPlanFilter.Text != "")
            {
                displayList.Rows.Clear();
                foreach (DataRow row in specificationList.Select("Plan='" + cmbPlanFilter.Text + "'"))
                    displayList.ImportRow(row);
            }
            if (chkValidate.Checked)
            {
                DataTable dt2 = displayList.Copy();
                displayList.Clear();
                foreach (DataRow row in dt2.Select("Validation=1"))
                    displayList.ImportRow(row);
            }
            dgvSpecificationList.DataSource = displayList;
            foreach (DataGridViewColumn col in dgvSpecificationList.Columns)
                col.SortMode = DataGridViewColumnSortMode.NotSortable;
        }

        private void cmbPlanFilter_SelectedValueChanged(object sender, EventArgs e)
        {
            DisplaySpecificationList();
        }

        private void chkValidate_CheckedChanged(object sender, EventArgs e)
        {
            DisplaySpecificationList();
        }

        private void dgvSpecificationList_CellClick(object sender, DataGridViewCellEventArgs e)
        {

            if (e.RowIndex >= 0)
                dgvSpecificationList.Rows[e.RowIndex].Selected = true;
        }

        private void btnLoadFromList_Click(object sender, EventArgs e)
        {
            try
            {
                var result = MessageBox.Show("Current content will be overwrite!\nChoose YES to confirm, Choose NO to abort.",
                    "Load Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (DialogResult.No == result)
                    return;
                string content = (string)dgvSpecificationList.SelectedRows[0].Cells["Content"].Value;
                LoadBISpecification(BISpecification.Deserialize(content));
                this.tabControl1.SelectTab(this.tabPage1);
                this.tabPage1.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void tabControl1_KeyDown(object sender, KeyEventArgs e)
        {
            if((e.KeyCode==Keys.E)&&e.Alt)
                tabControl1.SelectTab(tabPage1);
            if((e.KeyCode==Keys.V)&&e.Alt)
                tabControl1.SelectTab(tabPage2);
            if((e.KeyCode==Keys.P)&&e.Alt)
                tbPlan.Focus();
            if ((e.KeyCode == Keys.C) && e.Alt)
                tbConfigItem.Focus();
            if ((e.KeyCode == Keys.S) && e.Alt)
                tbSpecItem.Focus();
        }
    }
}
