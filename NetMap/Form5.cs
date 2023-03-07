using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
namespace NetMap
{
    public partial class Form5 : Form
    {
        public Form5()
        {
            InitializeComponent();
        }
        public string getLogLoc()
        {
            string dataS = "";
            dataS += System.AppContext.BaseDirectory;
            dataS = dataS.Substring(0, dataS.Length - 25);
            dataS += "database\\Log\\";
            return dataS;
        }
        private void Form5_Load(object sender, EventArgs e)
        {             
            string Log = getLogLoc() + "Log.txt";
            Thread.Sleep(100);
            IEnumerable<string> lines = File.ReadLines(Log);
            richTextBox1.Text += (String.Join(Environment.NewLine, lines));
        }
    }
}
