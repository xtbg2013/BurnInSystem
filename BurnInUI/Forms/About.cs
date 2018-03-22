using System;
using System.IO;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;

namespace BurnInUI.Forms
{
    public partial class About : Form
    {
        private static About _inst;
      
        public About()
        {
            InitializeComponent();
        }
        public static About Instance()
        {
            return _inst ?? (_inst = new About());
        }

        private void InitList()
        {
            listView.Columns.Add(
                new ColumnHeader
                {
                    Text = @"File description",
                    Width = 100 
                }
                );
            //listView.Columns.Add(
            //    new ColumnHeader
            //    {
            //        Text = @"Type",
            //        Width = 100
            //    }
            //);
            listView.Columns.Add(
                new ColumnHeader
                {
                    Text = @"File version",
                    Width = 100
                }
            );
            //listView.Columns.Add(
            //    new ColumnHeader
            //    {
            //        Text = @"Product name",
            //        Width = 100
            //    }
            //);
            //listView.Columns.Add(
            //    new ColumnHeader
            //    {
            //        Text = @"Product version",
            //        Width = 100
            //    }
            //);
            listView.Columns.Add(
                new ColumnHeader
                {
                    Text = @"Copyright",
                    Width = 200
                }
            );
            listView.Columns.Add(
                new ColumnHeader
                {
                    Text = @"Size",
                    Width = 100
                }
            );
            listView.Columns.Add(
                new ColumnHeader
                {
                    Text = @"Date modified",
                    Width = 150
                }
            );
            listView.Columns.Add(
                new ColumnHeader
                {
                    Text = @"Original name",
                    Width = 150
                }
            );
        }

        private void AddFileInfoToList(string path)
        {
            var file = new FileInfo(path);

            if (!file.Exists) return;
            // file.Refresh();
            var version = FileVersionInfo.GetVersionInfo(path);
               
            var listViewItem = new ListViewItem(version.FileDescription);
            //listViewItem.SubItems.Add(file.Extension == ".exe" ? "Application" : "Application extension");
            listViewItem.SubItems.Add(version.FileVersion);
            //listViewItem.SubItems.Add(version.ProductName);
            //listViewItem.SubItems.Add(version.ProductVersion);
            listViewItem.SubItems.Add(version.LegalCopyright);
            listViewItem.SubItems.Add((file.Length/1024.0).ToString("F0")+" KB");
            listViewItem.SubItems.Add(file.LastWriteTime.ToString(CultureInfo.InvariantCulture));
            listViewItem.SubItems.Add(version.OriginalFilename);
            listView.Items.Add(listViewItem);
        }

        private void AddList()
        {
            AddFileInfoToList("BurnInPlatform.exe");
            AddFileInfoToList("BiBsps.dll");
            AddFileInfoToList("BIModel.dll");
            AddFileInfoToList("DataService.dll");
            AddFileInfoToList("TbiesIntf.dll");
            AddFileInfoToList("BiInterfaceLib.dll");
            AddFileInfoToList("SpecEditor.exe");
            AddFileInfoToList("DataTool.exe");

        }
        private void About_Load(object sender, EventArgs e)
        {
            InitList();
            AddList();
        }

        private void About_FormClosed(object sender, FormClosedEventArgs e)
        {
            _inst = null;

        }

        private void btnCopy_Click(object sender, EventArgs e)
        {
            var info = listView.Items.Cast<ListViewItem>().Aggregate("", (current, item) => current + (item.SubItems[5].Text + "\t" + item.SubItems[1].Text + "\r\n"));
            Clipboard.SetDataObject(info);
        }

   
        
    }
}
