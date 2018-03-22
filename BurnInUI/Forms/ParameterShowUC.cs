using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using BIModel.Interface;

namespace BurnInUI.Forms
{
    public partial class ParameterShowUC : UserControl
    {
        private List<string> paramShowing = new List<string>();
        private volatile bool redraw = false;
     
        public ParameterShowUC()
        {
            InitializeComponent();
            
            Chartpara.ChartAreas[0].AxisX.LabelStyle.Format = "MM-dd\r\nHH:mm";
            redrawTimer = new Timer();
            redrawTimer.Interval = 500;
            redrawTimer.Tick += new EventHandler((s, e) =>
            {
                try
                {
                    redrawTimer.Stop();
                    if(redraw==true)
                        this.RefreshChart();
                }
                finally
                {
                    redraw = false;
                    redrawTimer.Start();
                }
            });
            redrawTimer.Start();
        }
      
        private void RefreshChart()
        {
            this.BeginInvoke(new Action(() =>
            {
                try
                {
                    Chartpara.SuspendLayout();
                    Chartpara.Series.Clear();
                    List<string> paramList = new List<string>();
                    foreach (DataGridViewRow row in this.dgvParamShowList.Rows)
                        if (((bool)row.Cells["IsShow"].Value) == true)
                        {
                            string boardName = row.Cells["Parameter"].Value.ToString();
                            paramList.Add(row.Cells["Parameter"].Value.ToString());
                        }
                           
                    
                    string sn = this.txtsn.Text;
                    if (string.IsNullOrEmpty(sn))
                        return;
                    var info = this.model.GetProductInformationBySn(sn);
                    if (info == null)
                        return;
                    var data = model.FetchUnitData(sn);
                   
                    var specList = (cbShowSpec.Checked ? this.model.GetSpecByPlan(info["Plan"]) : null);
                    foreach (var para in paramList)
                    {
                        Series series1 = CreateDateTimeSeries();
                        series1.MarkerStyle = MarkerStyle.Circle;
                        series1.MarkerSize = 6;
                        series1.Name = para;
                        for (int i = 0; i < data.Rows.Count; i++)
                        {
                            series1.Points.AddXY(data.Rows[i]["Load_Time"], double.Parse((string) data.Rows[i][para]));
                        }
                        Chartpara.Series.Add(series1);
                        if (cbShowSpec.Checked)
                        {
                            var result = from x in specList where x.Item == para select x;
                            if (result.Any())
                            {
                                var item = result.First();
                                var highSpec = CreateDateTimeSeries();
                                highSpec.MarkerStyle = MarkerStyle.None;
                                highSpec.Name = para + "_HIGH_SPEC";
                                highSpec.Points.AddXY(data.Rows[0]["Load_Time"], item.UBound);
                                var lowSpec = CreateDateTimeSeries();
                                lowSpec.MarkerStyle = MarkerStyle.None;
                                lowSpec.Name = para + "_LOW_SPEC";
                                lowSpec.Points.AddXY(data.Rows[0]["Load_Time"], item.LBound);
                                for (int i = 0; i < data.Rows.Count; i++)
                                {
                                    highSpec.Points.AddXY(data.Rows[i]["Load_Time"], item.UBound);
                                    lowSpec.Points.AddXY(data.Rows[i]["Load_Time"], item.LBound);
                                }
                                Chartpara.Series.Add(highSpec);
                                Chartpara.Series.Add(lowSpec);
                            }
                        }
                    }
                    Chartpara.ChartAreas[0].AxisX.LabelStyle.Format = "MM-dd\r\nHH:mm";
                    Chartpara.ChartAreas[0].RecalculateAxesScale();
                }
                finally
                {
                    Chartpara.ResumeLayout();
                }
            }));
        }

        private static Series CreateDateTimeSeries()
        {
            var ret = new Series();
            ret.ChartType = SeriesChartType.Line;
            ret.XValueType = ChartValueType.DateTime;
            ret.BorderWidth = 1;
            ret.ShadowOffset = 1;
            ret.IsVisibleInLegend = true;
            ret.IsValueShownAsLabel = false;
            return ret;
        }

        public void SetSN( string sn)
        {
            this.txtsn.Text = sn;
        }

        public void SetModel(IBiModel model)
        {
            this.model = model;
        }
        private IBiModel model = null;
        private void txtsn_TextChanged(object sender, EventArgs e)
        {
            string sn = this.txtsn.Text;
            this.dgvParamShowList.Rows.Clear();
            if (sn != "")
            {
                var info = this.model.GetProductInformationBySn(sn);
                if (info != null)
                {
                    var productInfo = new ProductInformation();
                    productInfo.BoardName = info["Board"];
                    productInfo.BoardSeat = info["Seat"];
                    productInfo.CostTime = info["CostTime"];
                    productInfo.CreateTime = info["CreateTime"];
                    productInfo.LastMonitorTime = info["MonitorTime"];
                    productInfo.Plan = info["Plan"];
                    productInfo.SerialNumber = info["SerialNumber"];
                    productInfo.UnitResult = info["UnitResult"];
                    productInfo.UnitState = info["UnitState"];
                    productInfo.RemainTime = info["RemainTime"];
                    this.pgProduct.SelectedObject = productInfo;
                    string[] paraList = model.GetUnitParaSet(sn);
                    foreach (string item in paraList)
                    {
                        if ((item.ToUpper() == "BOARDNAME") || (item.ToUpper() == "PCNAME"))
                        {
                        }  
                        else
                            this.dgvParamShowList.Rows.Add(new object[] { (this.paramShowing.Contains(item)) ? true : false, item });
                    }
                }
                else
                {
                    this.pgProduct.SelectedObject = null;
                    this.dgvParamShowList.Rows.Clear();
                }
            }
            else
            {
                this.pgProduct.SelectedObject = null;
                this.dgvParamShowList.Rows.Clear();
            }
            this.redraw = true;
        }

        private void dgvParamShowList_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            List<string> paramList = new List<string>();
            bool isUpdate = false;
            foreach (DataGridViewRow row in this.dgvParamShowList.Rows)
                if (((bool)row.Cells["IsShow"].Value) == true)
                {
                    string param = row.Cells["Parameter"].Value.ToString();
                    paramList.Add(param);
                    if (!this.paramShowing.Contains(param))
                        isUpdate = true;
                }
            if (isUpdate == true || (paramList.Count != this.paramShowing.Count))
                this.redraw = true;
            this.paramShowing = paramList;
        }
        
        private void dgvParamShowList_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            this.dgvParamShowList.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }

        private void btnFetch_Click(object sender, EventArgs e)
        {
            this.redraw = true;
        }

        private void ckbSelect_CheckedChanged(object sender, EventArgs e)
        {
            this.dgvParamShowList.SuspendLayout();
            foreach (DataGridViewRow row in this.dgvParamShowList.Rows)
                row.Cells["IsShow"].Value = this.ckbSelect.Checked;
            this.dgvParamShowList.ResumeLayout();
        }

        private Timer redrawTimer;
    }

}