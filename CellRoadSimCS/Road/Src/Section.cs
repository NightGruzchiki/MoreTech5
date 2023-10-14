using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Road
{
    //Section дороги
    public class Section
    {
        /// <summary>
        /// Вектор напавления движения участка 
        /// для отрисовки машины
        /// </summary>
        public int vector = 1;

        /// <summary>
        /// Статус участка 0 - пустой Section,
        /// статус 1 TrafficLight и стоячая Car, 
        /// 2 - едушая Car
        /// </summary>
        public int status = 0;

        /// <summary>
        /// Цвет отображаемого участка
        /// </summary>
        public Brush p_color = Brushes.LightGray;


        public int Status
        {
            get 

            {
                if (p_color == Brushes.OrangeRed)
                    return 1;
                if (status != 0)
                    return status;
                foreach (var data in smeg)
                    if (data.status != 0)
                        return data.status;
                return 0;
            }
        }

        /// <summary>
        /// Является ли Section конечным для дороги
        /// </summary>
        public bool endway = false;

        /// <summary>
        /// Список связных с этим участком путей
        /// </summary>
        public List<Section> nextWay = null;

        /*/ <summary>
        /// Встречный Section пути
        /// </summary>
        public List<Section> vstWay = null;*/

        /// <summary>
        /// Участки хода назад
        /// </summary>
        public Section backWay = null;

        /// <summary>
        /// Смежные участки пути
        /// </summary>
        public List<Section> smeg = new List<Section> ();

        /// <summary>
        /// Section Turnа и перехода на другой Section
        /// </summary>
        public List<Turn> listPovorot = new List<Turn>();

        /// <summary>
        /// Позиция для отрисовки участка по X
        /// </summary>
        public int X = 0;

        /// <summary>
        /// Позиция для отрисовки участка по Y
        /// </summary>
        public int Y = 0;

        public bool InClick(int x, int y)
        {
            if (x <= X + sizePaint &&
                x >= X &&
                y <= Y + sizePaint &&
                y >= Y )
                return true;
            return false;
        }

        /// <summary>
        /// Размер области отрисовки
        /// </summary>
        static public int sizePaint = 15;

        /// <summary>
        /// Конструктор участка пути
        /// </summary>
        /// <param name="backWay">предыдущий Section</param>
        /// <param name="vector">направление движения</param>
        /// <param name="size">размерность участка</param>
        /// <param name="wight">ширина области отрисовки</param>
        /// <param name="height">высота области отрисовки</param>
        public Section(int number, Section backWay, int vector, int size, int wight, int height, int mW, int mH, int polos, bool naprav)
        {
            wight = wight / 2;
            height = height / 2;
            if (naprav)
                number = (size - number) * sizePaint;
            else number = number * sizePaint;
            if (mW != 0)
                X = wight + mW * number;
            else X = wight + polos * sizePaint;
            if (mH != 0)
                Y = height + mH * number;
            else Y = height + polos * sizePaint;
            this.backWay = backWay;
            nextWay = new List<Section>();
            this.vector = vector;
            
        }
    }
}
