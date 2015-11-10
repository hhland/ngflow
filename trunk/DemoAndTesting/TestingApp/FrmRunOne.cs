using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using BP.WF;
using BP.Port;
using BP.En;
using BP.Sys;
using BP.DA;
using BP;
using BP.Web;
using System.Collections;

namespace TestingApp
{
    public partial class FrmRunOne : Form
    {
        ArrayList al = null;
        public FrmRunOne()
        {
            InitializeComponent();

            this.listBox1.Items.Clear();

            // Begin execution information .
            al = BP.En.ClassFactory.GetObjects("BP.CT.TestBase");
          //  al.Sort(0,100,null);
            int idx = 0;
            foreach (BP.CT.TestBase en in al)
            {
                ListBox lb = new ListBox();
                if (en.EditState == BP.CT.EditState.Editing)
                {
                    lb.BorderStyle = BorderStyle.Fixed3D;
                    lb.Text = idx.ToString().PadLeft(3, '0') + "-" + en.ToString() + "-" + en.Title + "(edit)";
                }
                else
                    lb.Text = idx.ToString().PadLeft(3, '0') + "-" + en.ToString() + "-" + en.Title;
                
                lb.Tag = en;
                this.listBox1.Items.Add(lb.Text);
                idx++;
            }
        }
        private void FrmRunOne_Load(object sender, EventArgs e)
        { 
        }
        private void Btn_Exit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void Btn_Do_Click(object sender, EventArgs e)
        {
            string[] infos = this.listBox1.SelectedItem.ToString().Split('-');
            int idx= int.Parse( infos[0]);
            ListBox lb = this.listBox1.SelectedItem as ListBox;
            BP.CT.TestBase en = (BP.CT.TestBase)al[idx];
            en.Do();
            MessageBox.Show(en.Title + "  Successful implementation  "); 
        }
    }
}
