using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GameOfLife
{
    public partial class MainForm : Form
    {
        bool isPlay = false;

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

        private void 저장하기ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Save다이얼로그 이용
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "Binary File|*.bin";
            saveFileDialog1.Title = "라이프 게임 저장";
            saveFileDialog1.ShowDialog();

            //파일명 확인
            if (saveFileDialog1.FileName != "")
            {
                // 파일 스스림 생성
                FileStream fs = (FileStream)saveFileDialog1.OpenFile();

                // 저장 포맷을 바이트 배열로 만들기
                MemoryStream stream = new MemoryStream();
                IFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, gamePanel.GetSvaeData());//직렬화를 통해서 객체를 바이트 배열로 치환
                byte[] bDts = stream.GetBuffer();

                //파일에 기록
                BinaryWriter br = new BinaryWriter(fs);
                br.Write(bDts.Length);//바이트 배열의 크기 저장
                br.Write(bDts);//바이트 배열 저장

                //파일 스트림 종료
                fs.Close();
            }
        }

        private void 불러오기ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Save다이얼로그 이용
            OpenFileDialog saveFileDialog1 = new OpenFileDialog();
            saveFileDialog1.Filter = "Binary File|*.bin";
            saveFileDialog1.Title = "라이프 게임 불러오기";
            saveFileDialog1.ShowDialog();

            //파일명 확인
            if (saveFileDialog1.FileName != "")
            {
                // 파일 스스림 생성
                FileStream fs = (FileStream)saveFileDialog1.OpenFile();

                //바이너리 리더생성
                BinaryReader br = new BinaryReader(fs);

                //바이너리 리더를 통해 배열의 크기를 알아본다.
                int size = br.ReadInt32();

                //바이너리 리더에 읽어온 크기만큼 바이트 배열을 읽어서 메모리 스트림생성
                MemoryStream stream = new MemoryStream(br.ReadBytes(size));
                IFormatter formatter = new BinaryFormatter();

                //바이너리 포메터랑 메모리 스트림을 이용해서 저장형식 역직렬화를 통해서 객체 생성
                SaveFormat svFormat = (SaveFormat)formatter.Deserialize(stream);

                //해당 객체의 메소드를 이용해 불러온 정보 gamePanel에 기록
                svFormat.RecoveryData(gamePanel);

                //파일 스트림 종료
                fs.Close();
                
                //게임 세팅
                timer1.Stop();
                toolStripStatusLabel4.Text = "로딩완료";
                isPlay = false;
                toolStripButton1.Image = GameOfLife.Properties.Resources.play;
                toolStripButton1.Text = "시작";
                generationLabel.Text = gamePanel.Generation.ToString();//세대 라벨 초기화
            }
        }
    }
}
