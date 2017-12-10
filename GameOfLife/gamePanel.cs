using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GameOfLife
{
    class GamePanel : Panel
    {
        //패널의 너비와 높이
        int gamePanel_W;
        int gamePanel_H;

        //세포들의 저장공간 생성
        Dictionary<Point, Cel> cels = new Dictionary<Point, Cel>();
        //x좌표 y좌표 주의
        //y좌표가 행, x좌표가 열이다.

        bool mouseLeftClick = false;
        int generation = 0;

        public int Generation
        {
            get { return generation; }
            set { generation = value; }
        }

        int finish = 0;

        public GamePanel()
        {
            this.DoubleBuffered = true;


            gamePanel_H = Height / 10;//행
            gamePanel_W = Width / 10;//열

            //((MainForm)Parent).Text = "test";
            //test
            //Parent.Text = "h : " + gamePanel_H + ", w : " + gamePanel_W;
        }
        public void ResetGame()
        {
            //세포들의 저장공간 생성
            cels.Clear();

            mouseLeftClick = false;
            generation = 0;

            Invalidate();
        }
        protected override void OnSizeChanged(EventArgs e)
        {
            gamePanel_H = this.Height / 10;//행
            gamePanel_W = this.Width / 10;//열

            Invalidate();

            //test
            //this.Text = "h : " + gamePanel_H + ", w : " + gamePanel_W;

            base.OnSizeChanged(e);
        }
        public void RandomCreate()
        {
            //셀 초기화
            cels.Clear();
            Random rand = new Random();

            //다음 세대 상태를 검사해서 설정
            for (int i = 0; i < gamePanel_H; ++i)
            {
                for (int j = 0; j < gamePanel_W; ++j)
                {
                    if (rand.Next(10) >= 5)//0~9까지의 랜덤 중 5이상이면 생성
                    {
                        Point pt = new Point(i, j);
                        Cel c = new Cel();
                        c.isLive = 1;
                        cels.Add(pt, c);
                    }
                }
            }
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            //g.SmoothingMode = SmoothingMode.HighSpeed;

            System.Drawing.SolidBrush myBrush = new System.Drawing.SolidBrush(System.Drawing.Color.Red);
            foreach (KeyValuePair<Point, Cel> items in cels)
            {
                if (items.Value.isLive == 1)
                {
                    ////test
                    //Text = "pt.X : " + items.Key.X + ", pt.Y : " + items.Key.Y;

                    //x,y좌표 헷갈리게 작업했음
                    g.FillRectangle(myBrush, new Rectangle(items.Key.Y * Cel.size, items.Key.X * Cel.size, Cel.size, Cel.size));
                }
            }


            //십자무늬 출력
            Pen p = new Pen(Color.Black);
            //가로선 그리기
            for (int i = 0; i <= gamePanel_H; ++i)
            {
                g.DrawLine(p, 0, i * 10, this.Width, i * 10);
            }
            //세로선 그리기
            for (int i = 0; i <= gamePanel_W; ++i)
            {
                g.DrawLine(p, i * 10, 0, i * 10, this.Height);
            }
            base.OnPaint(e);
        }


        protected override void OnMouseDown(MouseEventArgs e)
        {
            //Text = "pt.X : " + e.X + ", pt.Y : " + e.Y;
            mouseLeftClick = true;

            base.OnMouseDown(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            mouseLeftClick = false;

            base.OnMouseUp(e);
        }


        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (mouseLeftClick == true)
            {
                Point pt = new Point(e.Y / Cel.size, e.X / Cel.size);
                if (cels.ContainsKey(pt) == false)
                {
                    cels.Add(pt, new Cel());
                }
                else
                {
                    Cel c = cels[pt];
                    if (c.isLive > 0)
                    {
                        c.isLive = 0;
                    }
                    else
                    {
                        c.isLive = 1;
                    }
                }
                Invalidate();
            }


            base.OnMouseMove(e);
        }

        private void NextCheck(Point pt)
        {
            bool ownState = false;
            int num = 0;
            if (cels.ContainsKey(pt) == true)
            {
                ownState = cels[pt].isLive > 0 ? true : false;

                //if(own)
                //{
                //    ++num;
                //}
            }

            //위부터 시계방향으로 확인
            if (Cel.State(cels, new Point(pt.X, pt.Y + 1)))//상
                ++num;
            if (Cel.State(cels, new Point(pt.X + 1, pt.Y + 1)))//우상
                ++num;
            if (Cel.State(cels, new Point(pt.X + 1, pt.Y)))//우
                ++num;
            if (Cel.State(cels, new Point(pt.X + 1, pt.Y - 1)))//우하
                ++num;
            if (Cel.State(cels, new Point(pt.X, pt.Y - 1)))//하
                ++num;
            if (Cel.State(cels, new Point(pt.X - 1, pt.Y - 1)))//좌하
                ++num;
            if (Cel.State(cels, new Point(pt.X - 1, pt.Y)))//좌
                ++num;
            if (Cel.State(cels, new Point(pt.X - 1, pt.Y + 1)))//좌상
                ++num;

            if (ownState)
            {
                if (num >= 2 && num <= 3)
                    cels[pt].next = true;
                else
                    cels[pt].next = false;
            }
            else
            {
                if (num == 3)
                {
                    if (cels.ContainsKey(pt) == true)
                        cels[pt].next = true;
                    else
                    {
                        cels[pt] = new Cel();
                        cels[pt].isLive = 0;
                        cels[pt].next = true;
                    }
                }
            }
        }
        public void NextGeneration()
        {
            //다음 세대 상태를 검사해서 설정
            for (int i = 0; i < gamePanel_H; ++i)
            {
                for (int j = 0; j < gamePanel_W; ++j)
                {
                    NextCheck(new Point(i, j));
                }
            }

            //다음 세대의 상태값으로 현재 상태 치환
            foreach (KeyValuePair<Point, Cel> items in cels)
            {
                if (items.Value.next)
                    items.Value.isLive = 1;
                else
                    items.Value.isLive = 0;
            }
            //for (int i = 0; i < h; ++i)
            //{
            //    for (int j = 0; j < w; ++j)
            //    {
            //        if (cels.ContainsKey(new Point(i, j)) == true)
            //        {
            //            Cel c = cels[new Point(i, j)];
            //            if (c.next)
            //                c.isLive = 1;
            //            else
            //                c.isLive = 0;
            //        }
            //    }
            //}

            //this.Text = (++finish).ToString();

            Invalidate();
        }
    }
}
