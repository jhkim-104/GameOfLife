using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GameOfLife
{
    public partial class MainForm : Form
    {

        bool isPlay = false;
        ///////////밑으로 메소드/////////////////////////////////////////////////////////////////////////////////////////////////////////

        //메인 폼 관련 이벤트
        public MainForm()
        {
            InitializeComponent();

            //타이머와 시간조절 GUI 초기화
            timer1.Interval = 100;
            trackBar1.Value = timer1.Interval;
            toolStripTextBox1.Text = timer1.Interval.ToString();

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            generationLabel.Text = (++gamePanel.Generation).ToString();
            gamePanel.NextGeneration();   
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            if (!isPlay)
            {
                toolStripStatusLabel4.Text = "동작";
                isPlay = true;
                toolStripButton1.Image = GameOfLife.Properties.Resources.stop;
                timer1.Start();
            }
            else
            {
                toolStripStatusLabel4.Text = "일시정지";
                isPlay = false;
                toolStripButton1.Image = GameOfLife.Properties.Resources.play;                
                timer1.Stop();
            }
        }
        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            generationLabel.Text = (++gamePanel.Generation).ToString();
            gamePanel.NextGeneration();
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            timer1.Interval = trackBar1.Value;
            toolStripTextBox1.Text = timer1.Interval.ToString();
        }
    }
}
