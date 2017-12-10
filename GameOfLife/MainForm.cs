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
                toolStripButton1.Text = "일시정지";
                timer1.Start();
            }
            else
            {
                toolStripStatusLabel4.Text = "일시정지";
                isPlay = false;
                toolStripButton1.Image = GameOfLife.Properties.Resources.play;
                toolStripButton1.Text = "시작";
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

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            gamePanel.RandomCreate();
        }

        private void 새로하기ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //패널 리셋
            gamePanel.ResetGame();
            
            //메뉴 및 툴박스 리셋
            timer1.Stop();
            toolStripStatusLabel4.Text = "준비중";
            isPlay = false;
            toolStripButton1.Image = GameOfLife.Properties.Resources.play;
            toolStripButton1.Text = "시작";

            //세대 라벨 초기화
            generationLabel.Text = gamePanel.Generation.ToString();

            Invalidate();
        }
    }
}
