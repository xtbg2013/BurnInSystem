using System;
using System.Windows.Forms;
using BurnInUI.ConfigReader;
using BILib;

namespace BurnInUI.Forms
{
    public partial class DatabaseControl : Form
    {
        private readonly IDatabaseService _dataService;
        private readonly ConfigInfo _configInfo;
        
        public DatabaseControl(IDatabaseService dataService, ConfigInfo configInfo)
        {
            InitializeComponent();
            _dataService = dataService;
            _configInfo =  configInfo;
        }
        private static DatabaseControl _inst;
        
        public static DatabaseControl Instance(IDatabaseService dataService, ConfigInfo configInfo)
        {
            return _inst ?? (_inst = new DatabaseControl(dataService, configInfo));
        }
       
        private void btnCreateLocalDb_Click(object sender, EventArgs e)
        {
            string msg;
            if (!_dataService.CreateDataBase(_configInfo.DataSource, _configInfo.UserId, _configInfo.Password, _configInfo.SqlScript, out msg))
            {
                MessageBox.Show(@"Initialize local database unsuccess,message:" + msg);
                
            }
            else
            {
                MessageBox.Show(@"Initialize local database success");
            }

        }

        private void btnCreateRemoteDb_Click(object sender, EventArgs e)
        {
            string msg;
            if (!_dataService.CreateDataBase(_configInfo.RemoteDataSource, _configInfo.RemoteUserId, _configInfo.RemotePassword, _configInfo.RemoteSqlScript, out msg))
            {
                MessageBox.Show(@"Initialize remote database BI_Data unsuccess ,message:" + msg);

            }
            else
            {
                MessageBox.Show(@"Initialize remote database success");
            }
            
        }


        private void DatabaseControl_FormClosed(object sender, FormClosedEventArgs e)
        {
            _inst = null;
        }

        private void DatabaseControl_Load(object sender, EventArgs e)
        {
            
            checkBoxUpload.Checked = _dataService.GetBakServiceStatus();
        }

        private void checkBoxUpload_CheckStateChanged(object sender, EventArgs e)
        {
            _dataService.StartBakService(checkBoxUpload.Checked);
        }
    }
}
