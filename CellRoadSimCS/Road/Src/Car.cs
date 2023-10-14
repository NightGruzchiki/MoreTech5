using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Resources;
using Road.Properties;
using System.IO;

namespace Road
{
    public class Car
    {             
        /// <summary>
        /// Точка рождения машины
        /// </summary>
        public int position = 0;

        /// <summary>
        /// Текущий Section
        /// </summary>
        public Section inPoint = null;

        /// <summary>
        /// Section Turnа и перехода на другой Section
        /// </summary>
        public Turn povorot = null;

        /// <summary>
        /// Колличество пройденных участков
        /// </summary>
        public int wayState = 0;

        /// <summary>
        /// Мгновенная скорость автомобиля
        /// </summary>
        public int Speed;

        /// <summary>
        /// Максимальная скорость движения
        /// </summary>
        public int maxSpeed = 5;

        /// <summary>
        /// Отрисовываемая Car
        /// </summary>
        public Image paintIMG;

        /// <summary>
        /// Стоит ли Car
        /// </summary>
        public bool isStay = false;

        /// <summary>
        /// Случайные величины
        /// </summary>
        static public Random rand = new Random();

        /// <summary>
        /// Текущий номер машины
        /// </summary>
        public int _number = 0;

        /// <summary>
        /// Текущий номер машины
        /// </summary>
        static public int number = 0;

        #region Статичные данные
        

        static public Image m3 = null;
        static public Image m4 = null;
        static public Image m5 = null;

        static Car()
        {
            System.Reflection.Assembly myAssembly = System.Reflection.Assembly.GetExecutingAssembly();
            Stream myStream = myAssembly.GetManifestResourceStream("Road.Images.3.jpg");
            m3 = Image.FromStream(myStream);
            myStream = myAssembly.GetManifestResourceStream("Road.Images.4.jpg");
            m4 = Image.FromStream(myStream);
            myStream = myAssembly.GetManifestResourceStream("Road.Images.5.jpg");
            m5 = Image.FromStream(myStream);
        }

        #endregion

        public Car(int maxSpeed, Section inPoint, int pos) 
        {
            this.position = pos;
            _number = Car.number++;
            this.inPoint = inPoint;
            switch (maxSpeed)
            {
                case 2:
                    this.paintIMG = m3;
                    break;
                case 3:
                    this.paintIMG = m4;
                    break;
                case 5:
                    this.paintIMG = m5;
                    break;
                default:
                    this.paintIMG = m5;
                    break;
            }
        }

        /// <summary>
        /// Перемещение заданной машины на следпозицию (1 скорость)
        /// </summary>
        public bool Move(int n_iter)
        {
            if (inPoint.p_color == Brushes.DarkGray)
            {
                this.paintIMG = m3;
                this.Speed = 2;
            }
            else { }

            //ускорение
//            if (Speed == 0)
//                return false;
//            if (Speed < n_iter)
//                Speed++;
//                return true;
            //скорость ограничена
            if (n_iter >= maxSpeed )
                return false;
            //если Section конечный
            if (inPoint.endway)
            {
                this.povorot = null;
                this.inPoint.status = 0;
                return true;
            }

            //если находился в Turnе выходим из него
            if (this.povorot != null) 
            {
                if (povorot.way.status == 0) // Turn свободен
                {
                    this.inPoint.status = 0; //разблокировали Section
                    this.inPoint = povorot.way; // на Section для Turnа
                    this.inPoint.status = 2; // заблокирвали Section
                    this.isStay = false;
                    this.povorot.Blok(); //блокируем Section для Turnа
                    this.povorot = null;
                    wayState++;
                    if (povorot.way.status == 1) { maxSpeed = 0; }
                    return false;
                }
                //Turn не свободен
                this.inPoint.status = 1;
                return false;
            }


            //если на обычном участке пути
            if (HaveWay() != null)//перемещаемся при наличии участка
            {
                Section way = HaveWay(); // доступный Section пути

                
              //  if (this.inPoint.nextWay[50].Status == 2)
           //    
               

                if (way.listPovorot.Count > 0) //если след Section пути Turn
                {
                    //следующий Turn
                    Turn temp = way.listPovorot[rand.Next(0, way.listPovorot.Count)];
                   if (temp.IsFree())// Turn свободен переходим и блокируем его
                   {
                       this.povorot = temp;
                       temp.Blok(); //блокируем участки
                       this.inPoint.status = 0; //разблокировали Section
                       this.inPoint = way;
                       this.inPoint.status = 2; // заблокирвали Section
                       this.isStay = false;
                       wayState++;
                       return false;    
                   }

                }
                else // иначе следуем на другой Section
                {
                    this.inPoint.status = 0; //разблокировали Section
                    this.inPoint = way;
                    this.inPoint.status = 2; // заблокирвали Section
                    this.isStay = false;
                    wayState++;
                    return false;
                }
            }
            
            this.isStay = true;
            this.inPoint.status = 1;
            return false;

        }

        /// <summary>
        /// Конечный Section на которм запрещены обгоны
        /// </summary>
        public bool isEndWay(Section position, int count)
        {
            for (int i = 0; i < count; i++)
            {
                if (position.nextWay.Count <= 0)
                    return true;
                position = position.nextWay[0];
            }
            return false;
        }

        /// <summary>
        /// Возможно ли перемещение на след Section
        /// </summary>
        public Section HaveWay()
        {
         
            if (this.inPoint.nextWay.Count <= 0)
            {
                if (this.inPoint.listPovorot.Count > 0)
                {
                    Section temp = null;
                    foreach (var data in this.inPoint.listPovorot)
                        if (data.way.Status == 0)
                            temp = data.way ;
                    return temp;
                }
                else return null;
            }
            
            if (this.inPoint.nextWay[0].Status != 0)
            {
                if (isEndWay(this.inPoint, 5))
                    return null;
                else if ((this.inPoint.nextWay[0].Status == 1)&&
                         (this.inPoint.nextWay[0].p_color == Brushes.OrangeRed))
                {
                    for (int i = 1; i < this.inPoint.nextWay.Count; i++ )
                        if ((this.inPoint.nextWay[i].p_color != Brushes.OrangeRed) &&
                            (this.inPoint.nextWay[i].status == 0) &&
                            (this.inPoint.nextWay[i].backWay.status == 0))
                            return this.inPoint.nextWay[i];
                }
                else {}
            }
            else
            {
                Section temp = this.inPoint.nextWay[0];
                if (temp.nextWay.Count <= 0)
                    return temp;
                if (temp.nextWay[0].Status == 0 ||
                    temp.nextWay[0].Status == 1)
                    return temp;
                else
                    if (inPoint.nextWay.Count > 1 && inPoint.nextWay[1].Status == 0)
                        return inPoint.nextWay[1];
                    else
                        if (inPoint.nextWay.Count > 2 && inPoint.nextWay[2].Status == 0)
                            return inPoint.nextWay[2];
                        else return null;
            }
            return null;
        }
    }
}
