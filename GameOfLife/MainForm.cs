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

     //패널 변수
        int gamePanel_W;
        int gamePanel_H;

 ///////////밑으로 메소드/////////////////////////////////////////////////////////////////////////////////////////////////////////

     //메인 폼 관련 이벤트
        public MainForm()
        {
            InitializeComponent();

            //깜빡임을 없애줌
            DoubleBuffered = true;
            //this.SetStyle(ControlStyles.DoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint, true);         

            //패널관련
            gamePanel_H = gamePanel.Height / 10;//행
            gamePanel_W = gamePanel.Width / 10;//열

            //test
            this.Text = "h : " + gamePanel_H + ", w : " + gamePanel_W;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
        }
        protected override void OnSizeChanged(EventArgs e)
        {
            
            base.OnSizeChanged(e);
        }

    //밑으로는 패널 이벤트
        private void gamePanel_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.HighSpeed;

            Pen p = new Pen(Color.Black);
            //가로선 그리기
            for (int i = 0; i < gamePanel_H; ++i)
            {
                g.DrawLine(p, 0, i * 10, this.Width, i * 10);
            }
            //세로선 그리기
            for (int i = 0; i < gamePanel_W; ++i)
            {
                g.DrawLine(p, i * 10, 0, i * 10, this.Height);
            }

        }

        private void gamePanel_SizeChanged(object sender, EventArgs e)
        {
            gamePanel_H = this.Height / 10;//행
            gamePanel_W = this.Width / 10;//열

            //test
            this.Text = "h : " + gamePanel_H + ", w : " + gamePanel_W;
            gamePanel.Invalidate();
        }
    }
}
