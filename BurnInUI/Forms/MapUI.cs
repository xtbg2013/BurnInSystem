using System;
using System.IO;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using BIModel;
using BIModel.Data;
using BIModel.Interface;
using BILib;

namespace BurnInUI.Forms
{
    public partial class MapUi : Form
    {
        private readonly IPosMapScheme _posMapScheme;
        private static MapUi _inst;
        private MapUi(IDatabaseService dataService)
        {
            InitializeComponent();
            _posMapScheme = BiModelFactory.CreateIPosMapScheme(dataService);
        }
       
        public static MapUi Instance(IDatabaseService dataService)
        {
            return _inst ?? (_inst = new MapUi(dataService));
        }
        private void MapUI_Load(object sender, EventArgs e)
        {
            InitMapUiData();  
        }
       
        private void InitMapUiData()
        {
            var schemeNames = _posMapScheme.GetMapSchemesName();
            foreach (var name in schemeNames)
            {
                comboBox_mapName.Items.Add(name);
            }
            var posBlock = _posMapScheme.GetDefaultPosMapBlock();
            comboBox_mapName.Text = posBlock.SchemeName;
            textBoxBoardRows.Text = posBlock.BoardRows.ToString();
            textBoxBoardCols.Text = posBlock.BoardCols.ToString();
            textBoxSeatRows.Text = posBlock.SeatRows.ToString();
            textBoxSeatCols.Text = posBlock.SeatCols.ToString();
            DisplayMappingTable(posBlock.PosList);
            btnInsert.Enabled = false;
        }
        
        private void DisplayMappingTable(List<PosEntry> posList)
        {
            var ret = new DataTable();
            string[] columnSet = { "Name", "Floor", "Board" };
            ret.Columns.AddRange((from x in columnSet select new DataColumn(x)).ToArray());
            foreach (var entry in posList)
            {
                var row = ret.NewRow();
                row["Name"] = entry.BoardName;
                row["Floor"] = entry.FloorNum;
                row["Board"] = entry.BoardNum;
                ret.Rows.Add(row);
            }
            dgMapping.DataSource = ret;
        }

        private void dgMapping_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgMapping.Rows.Count == 0 || e.RowIndex == -1) return;
            dgMapping.Rows[e.RowIndex].Selected = true;
            tbBoardName.Text = dgMapping.Rows[e.RowIndex].Cells[0].Value.ToString();
            tbBoardFloor.Text = dgMapping.Rows[e.RowIndex].Cells[1].Value.ToString();
            tbBoardNumber.Text = dgMapping.Rows[e.RowIndex].Cells[2].Value.ToString();
        }

        private static bool IsInputValid(string input)
        {
            if (input == "")
                return false;
            const string pattern = @"^[A-Za-z0-9/-]+$";
            var regex = new Regex(pattern);
            return regex.IsMatch(input);
        }

        private  bool IsBoardInfoValid()
        {
            
            if (!IsInputValid(tbBoardName.Text))
            {
                //MessageBox.Show(@"Input board name invalid");
                return false;
            }

            if (!IsInputValid(tbBoardFloor.Text))
            {
                //MessageBox.Show(@"Input floor invalid");
                return false;
            }

            if (IsInputValid(tbBoardNumber.Text)) return true;
            //MessageBox.Show(@"Input board invalid");
            return false;
        }

        private bool IsRowColValid()
        {
          
            if (!IsInputValid(textBoxBoardRows.Text))
            {
               // MessageBox.Show(@"Board Rows is invalid");
                return false;
            }
            if (!IsInputValid(textBoxBoardCols.Text))
            {
                //MessageBox.Show(@"Board Cols is invalid");
                return false;
            }
            if (!IsInputValid(textBoxSeatRows.Text))
            {
               // MessageBox.Show(@"Seat Rows is invalid");
                return false;
            }
            if (IsInputValid(textBoxSeatCols.Text)) return true;
           // MessageBox.Show(@"Seat Cols is invalid");
            return false;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            var schemeName = comboBox_mapName.Text;
            if (IsRowColValid())
            {
                
                var boardRows = int.Parse(textBoxBoardRows.Text);
                var boardCols = int.Parse(textBoxBoardCols.Text);
                var seatRows = int.Parse(textBoxSeatRows.Text);
                var seatCols = int.Parse(textBoxSeatCols.Text);
                _posMapScheme.SubmitMap(schemeName, boardRows, boardCols, seatRows, seatCols);
            }
            if (IsBoardInfoValid())
            {
                var boardName = tbBoardName.Text;
                var floor = int.Parse(tbBoardFloor.Text);
                var number = int.Parse(tbBoardNumber.Text);
                _posMapScheme.SubmitMap(schemeName, boardName, floor, number);
            }
            var posMapBlock = _posMapScheme.GetPosMapBlock(schemeName);
            DisplayMappingTable(posMapBlock.PosList);

            MessageBox.Show(@"Save success");
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (!IsBoardInfoValid())
                return;
            var schemeName = comboBox_mapName.Text;
            var boardName = tbBoardName.Text;
            _posMapScheme.RemoveMap(schemeName, boardName);
            PosMapBlock posMapBlock = _posMapScheme.GetPosMapBlock(schemeName);
            DisplayMappingTable(posMapBlock.PosList);
        }

        private void MapUI_FormClosed(object sender, FormClosedEventArgs e)
        {
            _inst = null;
        }

        private void comboBox_mapName_TextUpdate(object sender, EventArgs e)
        {

            btnInsert.Enabled = true;
            btnSave.Enabled = false;
            btnRemove.Enabled = false;
           
        }

        private void btnInsert_Click(object sender, EventArgs e)
        {

            if (!IsRowColValid())
                return;
            var schemeName = comboBox_mapName.Text;
            var boardRows = int.Parse(textBoxBoardRows.Text);
            var boardCols = int.Parse(textBoxBoardCols.Text);
            var seatRows  = int.Parse(textBoxSeatRows.Text);
            var seatCols  = int.Parse(textBoxSeatCols.Text);
           
            if (schemeName != "")
            {
                _posMapScheme.InsertScheme(schemeName, boardRows, boardCols, seatRows, seatCols);
            }
            btnInsert.Enabled = false;
            btnSave.Enabled = true;
            btnRemove.Enabled = true;

            comboBox_mapName.Items.Clear();
            var schemeNames = _posMapScheme.GetMapSchemesName();
            foreach (var name in schemeNames)
            {
                comboBox_mapName.Items.Add(name);
            }
        }

        private void comboBox_mapName_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnInsert.Enabled = false;
            btnSave.Enabled = true;
            btnRemove.Enabled = true;

            tbBoardName.Text ="";
            tbBoardFloor.Text ="";
            tbBoardNumber.Text ="";
            var schemeName = comboBox_mapName.Text;
            var names = _posMapScheme.GetMapSchemesName();
            foreach (var name in names)
            {
                _posMapScheme.SubmitMap(name, false);
            }
            var posMapBlock = _posMapScheme.GetPosMapBlock(schemeName);
            _posMapScheme.SubmitMap(schemeName, true);
            DisplayMappingTable(posMapBlock.PosList);
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            var schemeName = comboBox_mapName.Text;
            var dlg = new SaveFileDialog() {DefaultExt = "json"};
            if (dlg.ShowDialog() != DialogResult.OK) return;
            try
            {
                var writer = File.CreateText(dlg.FileName);
                writer.Write(_posMapScheme.ExportMapScheme(schemeName));
                writer.Close();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
           
        }


        private void btnLoad_Click(object sender, EventArgs e)
        {
            var dlg = new OpenFileDialog() { Filter = @"json text|*.json|plain text|*.txt|All files(*.*)|*.*" };
            if (dlg.ShowDialog() != DialogResult.OK) return;
            try
            {
                var reader = File.OpenText(dlg.FileName);
                _posMapScheme.LoadMapScheme(reader.ReadToEnd());
                InitMapUiData();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }

        }
    }
}
