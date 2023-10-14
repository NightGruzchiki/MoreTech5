using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;
using System.Data;


namespace Road
{
    public class TrafficLight
    {
        public static int size = 15; //размеры элементов
        public int X = 0; // начальная позиция по x
        public int Y = 0; //начальная позиция по y
        public  Brush p_color = Brushes.DarkGray;      
    
        public int time_Yellow = 2;

        public static int time_Green = 15;

        public static int CiclSvet = 32;

        public static int time_Red = 15;

        public static bool Perekl = true;

        public int stay = 0;//текущая фаза TrafficLightа

        public int time = 0;

        public List<Section> stop = new List<Section>();


        public TrafficLight(List<Section> stop, int stay, int X, int Y)
        {          
            this.stop = stop;
            this.stay = stay;
            this.X = X;
            this.Y = Y;
            if (stay == 1 || stay == 2)

                Block();
        }

        /// <summary>
        /// Перемещение времени на 1 единицу
        /// </summary>
        public void Move()
        {
            
            time++;
            switch (stay)
            {
                
                case 0:
                    if (time >= time_Green)
                    {
                        time = 0;
                        stay = 1;
                        Block();
                    }
                    break;

                case 1:
                    if (time >= time_Yellow)
                    {
                        time = 0;
                        stay = 2;
                        Perekl = false;
                    }
                    break;

                case 2:
                    if (time >= time_Red)
                    {
                        time = 0;
                        stay = 0;
                        UnBlock();
                        Perekl = true;
                    }
                    break;
            }
            
        }



        public void SetStatus()
        {
            if (stay == 1 || stay == 2)
                Block();
            else UnBlock();
        }

        private void Block()
        {
            foreach (var data in stop)
                data.status = 1;
            p_color = Brushes.DarkSlateBlue;
        }
        private void UnBlock()
        {
            foreach (var data in stop)
                data.status = 0;
            p_color = Brushes.DarkSlateBlue;
        }
    }
}
