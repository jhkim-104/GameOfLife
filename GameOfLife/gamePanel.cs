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

        //모드 제어용 변수
        bool mouseClick = false;//마우스 클릭확인
        bool removeMode = false;//지우는 모드인지 확인

        //세대 변수
        int generation = 0;
        public int Generation
        {
            get { return generation; }
            set { generation = value; }
        }


        public GamePanel()
        {
            this.DoubleBuffered = true;

            gamePanel_H = Height / 10;//행
            gamePanel_W = Width / 10;//열
        }
        public void ResetGame()
        {
            //세포들의 저장공간 생성
            cels.Clear();

            mouseClick = false;
            removeMode = false;
            generation = 0;

            Invalidate();
        }
        public void ChangeCelSize()
        {
            gamePanel_H = this.Height / Cel.size;//행
            gamePanel_W = this.Width / Cel.size;//열

            Invalidate();
        }
        protected override void OnSizeChanged(EventArgs e)
        {
            gamePanel_H = this.Height / Cel.size;//행
            gamePanel_W = this.Width / Cel.size;//열

            Invalidate();

            base.OnSizeChanged(e);
        }
        //랜덤 생성용 메소드
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
        //저장용 데이터 반환
        public SaveFormat GetSvaeData()
        {
            return new SaveFormat(gamePanel_W, gamePanel_H, cels, generation);
        }
        //불러오기시 사용하는 메소드
        public void Recovery(int w, int h, Dictionary<Point, Cel> cs, int gen)
        {
            //데이터 복구
            gamePanel_W = w;
            gamePanel_H = h;
            cels = cs;
            generation = gen;

            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            
            foreach (KeyValuePair<Point, Cel> items in cels)
            {
                if (items.Value.isLive >= 1)
                {
                    //색 조절 // 세포가 오래될 수록 색이 검은색으로 변한다.
                    int red = 100 - items.Value.isLive >= 0 ? 100 - items.Value.isLive : 0;
                    int green = 200 - items.Value.isLive >= 0 ? 200 - items.Value.isLive : 0;
                    int blue = 255 - items.Value.isLive >= 0 ? 255 - items.Value.isLive : 0;

                    System.Drawing.SolidBrush myBrush = new System.Drawing.SolidBrush(System.Drawing.Color.FromArgb(red, green, blue));
                    //x,y좌표 헷갈리게 작업했음
                    g.FillRectangle(myBrush, new Rectangle(items.Key.Y * Cel.size, items.Key.X * Cel.size, Cel.size, Cel.size));
                }
            }

            //십자무늬 출력
            Pen p = new Pen(Color.Black);

            //가로선 그리기
            for (int i = 0; i <= gamePanel_H; ++i)
            {
                g.DrawLine(p, 0, i * Cel.size, this.Width, i * Cel.size);
            }
            //세로선 그리기
            for (int i = 0; i <= gamePanel_W; ++i)
            {
                g.DrawLine(p, i * Cel.size, 0, i * Cel.size, this.Height);
            }
            base.OnPaint(e);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            mouseClick = true;
            Point pt = new Point(e.Y / Cel.size, e.X / Cel.size);
            if (cels.ContainsKey(pt) == false)
            {
                cels.Add(pt, new Cel());
            }
            else
            {
                removeMode = true;
                cels.Remove(pt);
            }

            base.OnMouseDown(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            mouseClick = false;
            removeMode = false;

            Invalidate();//일반 클릭시에도 반응하기 위하여 여기 최신화

            base.OnMouseUp(e);
        }


        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (mouseClick == true)
            {
                Point pt = new Point(e.Y / Cel.size, e.X / Cel.size);
                if (!removeMode)
                {
                    if (cels.ContainsKey(pt) == false)
                    {
                        cels.Add(pt, new Cel());
                    }
                }
                else
                {
                    if (cels.ContainsKey(pt))
                    {
                        cels.Remove(pt);
                    }

                }
                Invalidate();//그냥 마우스 움직임일때는 최신화x
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

            //필요없는 Cel삭제시 딕셔너리의 특성상 기준점 변경시 에러가 나기 때문에 
            //Key값들을 리스트로 변경하여 foreach문을 돌림으로 성능을 강화
            foreach (var key in cels.Keys.ToList())
            {
                if (cels[key].next)
                    cels[key].isLive += 1;
                else
                {
                    cels.Remove(key);
                }
            }

            Invalidate();
        }
    }
}
