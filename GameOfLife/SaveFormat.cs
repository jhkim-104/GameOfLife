using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameOfLife
{
    [Serializable]
    class SaveFormat
    {
        //패널의 너비와 높이
        int gamePanel_W;
        int gamePanel_H;
         
        //세포들의 저장공간 생성
        Dictionary<Point, Cel> cels;
        
        int generation;

        public SaveFormat(int w, int h, Dictionary<Point, Cel> cs, int gen)
        {
            gamePanel_W = w;
            gamePanel_H = h;
            cels = cs;
            generation = gen;
        }

        public void RecoveryData(GamePanel p)
        {
            p.Recovery(gamePanel_W, gamePanel_H, cels, generation);
        }
    }
}
