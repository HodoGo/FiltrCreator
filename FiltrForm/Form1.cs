using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FiltrForm
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }


        private void btn_open_Click_1(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.ShowDialog();
            var result = ofd.FileName;
            var exl = new TopHeaderClass(result);
            dataGridView1.DataSource = exl.dt;
            dataGridView2.DataSource = exl.dt2;
        }

        private void Create_txt_Click_1(object sender, EventArgs e)
        {
            richTextBox1.Text = "";
            List<string> strList = new List<string>();
            List<string> strRow = new List<string>();
            foreach (DataGridViewRow row in this.dataGridView1.Rows)
            {
                string tmlRow = "";
                foreach (DataGridViewRow col in this.dataGridView2.Rows)
                {
                    object checkValue = col.Cells["Active"].EditedFormattedValue;
                    if (checkValue.ToString() == "True")
                    {
                        string celName = col.Cells["Name_Col"].Value.ToString();
                        string cell = row.Cells[celName].Value.ToString();
                        string extension = cell.Length > 7 ? cell.Substring(cell.Length - 7) : "";
                        if (extension == "0:00:00") cell = cell.Replace("0:00:00", "00:00:00");
                        string psevdonim = (col.Cells["Psevdonim"].Value.ToString().Length > 0) ? col.Cells["Psevdonim"].Value.ToString() : celName;
                        if (cell.Length > 0)
                            tmlRow += String.Format("{{ {2} }} {1} \"{0}\" {3} ", cell, col.Cells["Symbol"].Value.ToString(), psevdonim, col.Cells["Znak"].Value.ToString());
                    }
                }
                if (tmlRow.Length > 0) strList.Add(tmlRow);
            }
            string result = String.Join(") or (", strList.ToArray());
            richTextBox1.Text += '(' + result + ')';
        }
    }
}
