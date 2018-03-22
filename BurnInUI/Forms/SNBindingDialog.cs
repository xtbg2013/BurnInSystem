using System;
using System.Data;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using BiInterface;
using BIModel;
using BIModel.Interface;
using BurnInUI.ConfigReader;

namespace BurnInUI.Forms
{
    public partial class SNBindingDialog : Form
    {
        private DataTable table = new DataTable();
        private string[] snlist = null;
        private IBiModel _model;
        private IMesOperator _MesOperator;
        private IBoard _board;
        private ConfigInfo _configParam;
        private void InitTable(int seatCount)
        {
            table.Columns.Add("Seat", System.Type.GetType("System.String"));
            table.Columns.Add("SN", System.Type.GetType("System.String"));

            for (int i = 0; i < seatCount; i++)
            {
                table.Rows.Add(table.NewRow());
                table.Rows[i]["Seat"] = (i+1).ToString();
            }
            
            dGVSN.DataSource = table;
            dGVSN.Columns[0].Width = (int)(dGVSN.Width * 0.2);
            dGVSN.Columns[0].ReadOnly = true;
            dGVSN.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
            dGVSN.Columns[1].Width = (int)(dGVSN.Width * 0.7);
            dGVSN.Columns[1].SortMode = DataGridViewColumnSortMode.NotSortable;
        }

        public SNBindingDialog(string boardname, IBiModel model, ConfigInfo param)
        {
            InitializeComponent();
            lbBoardName.Text = boardname;
            _model = model;
            int seatCount = _model.GetBoardSeatsCount(boardname);
            _configParam = param;
            snlist = new string[seatCount];
            for (int i = 0; i < seatCount; i++)
                snlist[i] = "";
            InitTable(seatCount);
            _board = _model.GetController(boardname);
            _MesOperator = BiModelFactory.GetMesOperator();

        }
        private void PasteData()
        {
            string clipboardText = Clipboard.GetText();
            if (string.IsNullOrEmpty(clipboardText))
            {
                return;
            }
            int seatCount = _model.GetBoardSeatsCount(lbBoardName.Text);
            string[] snArray = clipboardText.Split(new string[] { "\r\n" }, StringSplitOptions.None);
            int count = snArray.Length > seatCount ? seatCount : snArray.Length;
            for (int i = 0; i < count; i++)
            {
                table.Rows[i]["SN"] = snArray[i];
            }
        }
        public string[] GetSNList()
        {
            return snlist;
        }
        private static bool IsSnValid(string input)
        {
            string pattern = @"^[A-Za-z0-9/-]+$";
            Regex regex = new Regex(pattern);
            return regex.IsMatch(input);
        }

        private bool CheckSnActiveState(string sn,out string message)
        {
            bool ret = true;
            try
            {
                string Stepstate = "";
                string workStep = _MesOperator.GetMesStepBySn(sn,out message);
                if (workStep == _board.GetMesStepName().Trim())
                {
                    Stepstate = _MesOperator.CheckMesStateBySn(sn, out message);
                    if (Stepstate == "Active")
                    {
                        ret = true;
                    }           
                    else
                    {
                        ret = false;
                        message = sn + " is not in state:Active";
                    }
                        
                }
                else
                {
                    ret = false;
                    message = " work step is not matched: mes step name: " + _board.GetMesStepName() + ","+sn+" current step name: " + workStep;
                }
                 
            }
            catch(Exception ex)
            {
                message = ex.Message;
                ret = false;
            }
            return ret;
        }


        private bool IsSnRepetition(string sn)
        {
            var count = 0;
            foreach (DataGridViewRow row in dGVSN.Rows)
            {
                if (row.Cells[1].Value != null)
                {
                    if (sn.Equals(row.Cells[1].Value.ToString()))
                        count++;
                }
            }
            if (count > 1)
                return true;
            else
                return false;

        }
        private bool IsSnMatchedTestPlan(string sn, out string message)
        {
            bool ret = true;
            try
            {
                string[] info = _MesOperator.GetCocInfoBySn(sn, out message);
                string cocType = _board.GetCocTypeBySn(sn,info);
                if (cocType == _board.GetCocTypeByPlan())
                {
                    ret = true;
                }
                else
                {
                    ret = false;
                    message = "Sn is not matched with the slot's test plan,sn:" + sn + "";
                }
               
            }
            catch (Exception ex)
            {
                message = ex.Message;
                ret = false;
            }
            return ret;  
        }

        private void BtnSubmit_Click(object sender, EventArgs e)
        {
            string message;
            foreach (DataGridViewRow row in dGVSN.Rows)
            {
                if ((row.Cells[1].Value != null)&&(row.Cells[1].Value.ToString() != ""))
                {
                    string sn = row.Cells[1].Value.ToString();
                    if (!IsSnValid(sn))
                    {
                        MessageBox.Show("您输入的SN有非法字符，内容是:" + sn + "");
                        return;
                    }
                    if (IsSnRepetition(sn))
                    {
                        MessageBox.Show("您输入的SN有重复，重复的内容是:" + sn + "");
                        return;
                    }

                    if (_model.IsUnitExist(sn))
                    {
                        MessageBox.Show("您输入的SN在数据库中已经存在了，内容是:" + sn + "");
                        return;
                    }
                     
                    //int holdormove;
                    #region Camstar check
                    //Add for MES function, defause values "", if value not null, no need the MES function
                    if (_board.IsMesCheck())
                    {
                        if (_board.IsCocCheck())
                        {
                            if (!IsSnMatchedTestPlan(sn, out message))
                            {
                                MessageBox.Show("您输入的SN与本SLOT的测试计划不匹配:" + sn + " ,可能原因：\n" + message);
                                return;
                            }
                        } 
                        if (!CheckSnActiveState(sn,out message))
                        {
                            MessageBox.Show("您输入的SN在MES中有异常:" + sn + ",可能原因：\n" + message);
                            return;
                        }
                    }
                    //For test the MES Move next station function
                    //UnitManager.Auto_Move_out(v.Cells[1].Value.ToString(), "PASS", out holdormove);
                    #endregion
                }
            }
            for (int i = 0; i < dGVSN.Rows.Count; i++)
                snlist[i] = dGVSN.Rows[i].Cells[1].Value.ToString().Trim();
            SaveManuallyBindingTable();
            Close();
        }

        private void SaveManuallyBindingTable()
        {
            string mbtFile = Path.Combine(_configParam.MbPath,lbBoardName.Text+".mbt");
            using (var sw = File.CreateText(mbtFile))
            {
                foreach (var item in snlist)
                    sw.WriteLine(item);
                sw.Close();
            }
        }

        private void BtnLoadFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.InitialDirectory = _configParam.MbPath;
            if (DialogResult.OK == dlg.ShowDialog())
            {
                using(StreamReader sr= System.IO.File.OpenText(dlg.FileName))
                {
                    int row = 0;
                    do
                    {
                        dGVSN.Rows[row++].Cells["SN"].Value = sr.ReadLine();
                    } while ((!sr.EndOfStream)&&row<dGVSN.RowCount);
                    sr.Close();
                }
            }
        }

        private void btnPaste_Click(object sender, EventArgs e)
        {
            PasteData();
        }
    }
}
