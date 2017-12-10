using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GameOfLife
{
    public partial class DisplaySetting : Form
    {
        public DisplaySetting()
        {
            InitializeComponent();

        }
        private void DisplaySetting_Load(object sender, EventArgs e)
        {
            heightTextBox.Text = ((MainForm)Owner).Height.ToString();
            widthTextBox.Text = ((MainForm)Owner).Width.ToString();
            celSizeTextBox.Text = Cel.size.ToString();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            int height = int.Parse(heightTextBox.Text); ;
            int width =  int.Parse(widthTextBox.Text); ;
            int celSize = int.Parse(celSizeTextBox.Text);
            if (Cel.size != celSize)
            {
                Cel.size = celSize;
                ((MainForm)Owner).gamePanel.ChangeCelSize();
            }
            if (((MainForm)Owner).Height != height)
            {
                ((MainForm)Owner).Height = height;
            }
            if (((MainForm)Owner).Width != width)
            {
                ((MainForm)Owner).Width = width;
            }

            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }


    }
}
