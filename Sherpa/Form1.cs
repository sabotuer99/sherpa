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

namespace Sherpa
{
    public partial class Sherpa : Form
    {
        public Sherpa()
        {
            InitializeComponent();
        }

        private void SherpaForm_Load(object sender, EventArgs e)
        {
            uploadButton.Enabled = false;
        }

        private void browseButton_Click(object sender, EventArgs e)
        {
            //clear status label
            statusLabel.Text = "";

            OpenFileDialog openFileDialog1 = createDialog();
            DialogResult result = openFileDialog1.ShowDialog(); 
            if (result == DialogResult.OK) 
            {
                string file = openFileDialog1.FileName;
                try
                {
                    filepathTextbox.Text = file;
                    uploadButton.Enabled = true;
                }
                catch (IOException ioe)
                {
                    Console.WriteLine(ioe);
                }
            }
        }

        private OpenFileDialog createDialog()
        {
            OpenFileDialog fDialog = new OpenFileDialog();
            fDialog.Title = ConfigurationManager.AppSettings["dialogTitle"];
            fDialog.Filter = ConfigurationManager.AppSettings["dialogFilter"];
            fDialog.InitialDirectory = ConfigurationManager.AppSettings["dialogInitialDirectory"];
            return fDialog;
        }

        private void loadExcelIntoDb(string file)
        {
            if(new DbService().appendExcelToDatabase(file))
            {
                statusLabel.ForeColor = Color.Green;
                statusLabel.Text = "Successful upload!";
            }
            else
            {
                statusLabel.ForeColor = Color.Red;
                statusLabel.Text = "Something went wrong...";
            }
        }

        private void uploadButton_Click(object sender, EventArgs e)
        {
            string file = filepathTextbox.Text;
            loadExcelIntoDb(file);
            filepathTextbox.Text = "";
            uploadButton.Enabled = false;
        }
    }
}
