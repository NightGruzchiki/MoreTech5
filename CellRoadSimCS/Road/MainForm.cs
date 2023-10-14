using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;

namespace Road
{
    public partial class MainForm : Form
    {
        public static Road road = null;

        int sizeRoad = 30;


        private List<int> admt_hor_road_list = null;
        private List<int> admt_ver_road_list = null;

        private List<List<int>> cargraf_1road = null;
        private List<List<int>> cargraf_2road = null;
        private List<List<int>> cargraf_3road = null;
        private List<List<int>> cargraf_4road = null;

        
        public int SvetAlgoritm;

        public MainForm()
        {
            InitializeComponent();
            MainPictureBox.Image = new Bitmap(950, 700);
            cargraf_1road = new List<List<int>>();
            cargraf_2road = new List<List<int>>();
            cargraf_3road = new List<List<int>>();
            cargraf_4road = new List<List<int>>();
            admt_hor_road_list = new List<int>();
            admt_ver_road_list = new List<int>();

        }



        /// <summary>
        /// функции переключения 1 для адаптивного TrafficLightа
        /// </summary>
        /// 
  //      public int HorisontOch = 0;        //переменные мгновенных очередей
  //      public int VerticalOch = 0;

        public void FuncG1()
        {
            foreach (var data in road.listMachin)
            {
                foreach (var temp in road.svetGroup.group1)
                {
                        if (temp.stay == 0)
                        {                       
                            if (data.inPoint.p_color == Brushes.DarkGray)
                            {

                                if (AdaptiveVerticalRoad_ListBox.Items.IndexOf(data._number) < 0)
                                {
                                    if (data.position == 2 || data.position == 3)
                                        AdaptiveVerticalRoad_ListBox.Items.Add(data._number);
  


                                }
                            }
                        }
                           
                     if (data.inPoint.p_color == Brushes.DarkSlateBlue)
                     {
                                AdaptiveHorizontalRoad_ListBox.Items.Remove(data._number);
                                AdaptiveVerticalRoad_ListBox.Items.Remove(data._number);
                     }
                }
            }
        }
        /// <summary>
        /// функции переключения 2
        /// </summary>
        public void FuncG2()
        {
            foreach (var data in road.listMachin)
            {
                foreach (var temp in road.svetGroup.group2)
                {
                    if (temp.stay == 0)
                    {
                        if (data.inPoint.p_color == Brushes.DarkGray)
                        {
                            if (AdaptiveHorizontalRoad_ListBox.Items.IndexOf(data._number) < 0)
                            {
                                if (data.position == 0 || data.position == 1)
                                    AdaptiveHorizontalRoad_ListBox.Items.Add(data._number);
                            }
                        }
                        if (data.inPoint.p_color == Brushes.DarkSlateBlue)
                        {
                            AdaptiveHorizontalRoad_ListBox.Items.Remove(data._number);
                            AdaptiveVerticalRoad_ListBox.Items.Remove(data._number);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Текущая анимационная картинка
        /// </summary>
        /// 
        private bool last_hor_road_green = true;


        /// <summary>
        /// Ввод длительности фазы зеленого
        /// </summary>
        ///  
        void Svet()
        {
            TrafficLight.time_Green = Convert.ToInt32(toolStripTextBox7.Text);
            TrafficLight.CiclSvet = Convert.ToInt32(toolStripTextBox9.Text);
            TrafficLight.time_Red = TrafficLight.CiclSvet - TrafficLight.time_Green - 2;
        }

        public int animeTick = 0;
        private void anime_Tick(object sender, EventArgs e)
        {
            try
            {
                if (animeTick >= 5)
                {
                    cargraf_1road.Add(new List<int>() {0,0,0,0,0 });
                    cargraf_2road.Add(new List<int>() {0,0,0,0,0 });
                    cargraf_3road.Add(new List<int>() {0,0,0,0,0 });
                    cargraf_4road.Add(new List<int>() {0,0,0,0,0 });
                    var cv_carwaylong_denum = cv_carwaylong_dict.GetEnumerator();
                    while(cv_carwaylong_denum.MoveNext())
                    
                    {
                        int cv_carspd_val = cv_carwaylong_denum.Current.Key.wayState;
                        cv_carspd_val -= cv_carwaylong_denum.Current.Value;
                        if((cv_carspd_val>-1)&&(cv_carspd_val<5))
                        {
                            if (cv_carwaylong_denum.Current.Key.position == 0)
                            {
                                cargraf_1road[cargraf_1road.Count - 1][cv_carspd_val]++;
                            }
                            else if (cv_carwaylong_denum.Current.Key.position == 1)
                            {
                                cargraf_2road[cargraf_1road.Count - 1][cv_carspd_val]++;
                            }
                            else if (cv_carwaylong_denum.Current.Key.position == 2)
                            {
                                cargraf_3road[cargraf_1road.Count - 1][cv_carspd_val]++;
                            }
                            else if (cv_carwaylong_denum.Current.Key.position == 3)
                            {
                                cargraf_4road[cargraf_1road.Count - 1][cv_carspd_val]++;
                            }
                            else { }
                        }
                        else {}
                   }
                    if (cv_carspeedgraf_frm != null)
                    {
                        cv_carspeedgraf_frm.BeginInvoke(new Action<Form>(delegate(Form cv_form)
                            {
                                try
                                {
                                    string cv_itercar_infstr = cargraf_1road.Count.ToString().PadRight(10).PadLeft(13);
                                    for (int k = 0; k < cargraf_1road[cargraf_1road.Count - 1].Count; k++)
                                    {
                                        cv_itercar_infstr = cv_itercar_infstr + cargraf_1road[cargraf_1road.Count - 1][k].ToString().PadRight(4).PadLeft(5);
                                    }
                                    for (int k = 0; k < cargraf_2road[cargraf_2road.Count - 1].Count; k++)
                                    {
                                        cv_itercar_infstr = cv_itercar_infstr + cargraf_1road[cargraf_2road.Count - 1][k].ToString().PadRight(4).PadLeft(5);
                                    }
                                    for (int k = 0; k < cargraf_3road[cargraf_3road.Count - 1].Count; k++)
                                    {
                                        cv_itercar_infstr = cv_itercar_infstr + cargraf_1road[cargraf_3road.Count - 1][k].ToString().PadRight(4).PadLeft(5);
                                    }
                                    for (int k = 0; k < cargraf_4road[cargraf_4road.Count - 1].Count; k++)
                                    {
                                        cv_itercar_infstr = cv_itercar_infstr + cargraf_1road[cargraf_4road.Count - 1][k].ToString().PadRight(4).PadLeft(5);
                                    }
                                    cv_itercar_infstr = cv_itercar_infstr + "\r\n";

                                    ((TextBox)(cv_form.Controls[0])).Text += cv_itercar_infstr;
                                }
                                catch (Exception ex)
                                { }
                            }
                        ), cv_carspeedgraf_frm);
                    }
                    else { }


                    animeTick = 0;
                    anime.Enabled = false;
                    cv_carwaylong_dict = null;
                    return;
                }

                bool cur_hor_road_green = false;
                for (int i = 0; i < road.svetGroup.group1.Count; i++)
                {
                    if (road.svetGroup.group1[i].stay == 0)
                    {
                        cur_hor_road_green = true;
                        break;
                    }
                    else { }
                }

                List<Car> rem = new List<Car>();

                foreach (var data in road.listMachin)
                {
                    if (data.Move(animeTick) || data.inPoint.endway)
                        rem.Add(data);
                }

                foreach (var temp in road.listSvet)
                {
                    FuncG1();
                    FuncG2();


                    if (AdSv.Checked == true)
                    {
                        SvetAlgoritm = 0;
                    }
                        else
                    {
                        if (FxSv.Checked == true)
                        {
                            SvetAlgoritm = 1;
                        }
                    }
                    

                    
                    switch (SvetAlgoritm)
                    {
                        case 0:         // Управление адаптивным алгоритмом TrafficLightного регулирования АСУДД "ВЗГЛЯД"

                            if ((AdaptiveHorizontalRoad_ListBox.Items.Count > 0) || (AdaptiveVerticalRoad_ListBox.Items.Count > 0))
                            {// управлние функций переключения TrafficLightов
                                if (AdaptiveHorizontalRoad_ListBox.Items.Count == 0)
                                {
                                    road.svetGroup.SetGreenG2();
                                }
                                else
                                {
                                }
                                if (AdaptiveVerticalRoad_ListBox.Items.Count == 0)
                                {
                                    road.svetGroup.SetGreenG1();
                                }
                                else
                                {
                                }
                            }
                            else { }
                            break;

     //                   case 1:             // Управление фиксированным циклом TrafficLightного регулирования                             
                             
     //                       if (TrafficLight.time_Green != TrafficLight.time_Red)
     //                       {// управлние функций переключения TrafficLightов
     //                          Svetgroup.AssimSvet(R, G);
    //                           R = Convert.ToInt32(toolStripTextBox7.Text);
    //                           G = Convert.ToInt32(toolStripTextBox9.Text);
  
    //                        }
    //                        else { }
    //                        break;
                        
                        
                        case 2:         // Управление адаптивным алгоритмом TrafficLightного регулирования программного комплекса “PTV Vision VISSIM”

                            if ((AdaptiveHorizontalRoad_ListBox.Items.Count > 0) || (AdaptiveVerticalRoad_ListBox.Items.Count > 0))
                            {// управлние функций переключения TrafficLightов
                                if (AdaptiveHorizontalRoad_ListBox.Items.Count == 0)
                                {
                                    road.svetGroup.SetGreenG2();
                                }
                                else
                                {
                                }
                                if (AdaptiveVerticalRoad_ListBox.Items.Count == 0)
                                {
                                    road.svetGroup.SetGreenG1();
                                }
                                else
                                {
                                }
                            }
                            else { }
                            break;


                    }
                }
                    
                        

                if ((last_hor_road_green == false) && (cur_hor_road_green == true))
                {
                    admt_hor_road_list.Add(0);
                    for (int i = 0; i < road.AO.Length; i++)
                    {
                        Section cv_uchroad_fow = road.AO[i];
                        while ((cv_uchroad_fow != null) && !((cv_uchroad_fow.nextWay == null) ||
                            (cv_uchroad_fow.nextWay.Count == 0)))
                        {
                            cv_uchroad_fow = cv_uchroad_fow.nextWay[0];
                        }
                        cv_uchroad_fow = cv_uchroad_fow.backWay.backWay;
                        for (int k = 0; k < road.AO.Length; k++)
                        {
                            cv_uchroad_fow = cv_uchroad_fow.backWay;
                        }
                        while ((cv_uchroad_fow != null) && (cv_uchroad_fow.Status == 1) &&
                            (cv_uchroad_fow.p_color != Brushes.OrangeRed))
                        {
                            admt_hor_road_list[admt_hor_road_list.Count - 1]++;
                            if (cv_uchroad_fow.backWay != null)
                            {
                                cv_uchroad_fow = cv_uchroad_fow.backWay;
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                    for (int i = 0; i < road.BO.Length; i++)
                    {
                        Section cv_uchroad_fow = road.BO[i];
                        while ((cv_uchroad_fow != null) && !((cv_uchroad_fow.nextWay == null) ||
                            (cv_uchroad_fow.nextWay.Count == 0)))
                        {
                            cv_uchroad_fow = cv_uchroad_fow.nextWay[0];
                        }
                        cv_uchroad_fow = cv_uchroad_fow.backWay.backWay;
                        for (int k = 0; k < road.BO.Length; k++)
                        {
                            cv_uchroad_fow = cv_uchroad_fow.backWay;
                        }
                        while ((cv_uchroad_fow != null) && (cv_uchroad_fow.Status == 1) &&
                            (cv_uchroad_fow.p_color != Brushes.OrangeRed))
                        {
                            admt_hor_road_list[admt_hor_road_list.Count - 1]++;
                            if (cv_uchroad_fow.backWay != null)
                            {
                                cv_uchroad_fow = cv_uchroad_fow.backWay;
                            }
                            else
                            {
                                break;
                            }
                        }

                    }
                    label22.Text = admt_hor_road_list.Average() + "";
                }
                else if ((last_hor_road_green == true) && (cur_hor_road_green == false))
                {
                    admt_ver_road_list.Add(0);
                    try
                    {
                        for (int i = 0; i < road.CO.Length; i++)
                        {
                            Section cv_uchroad_fow = road.CO[i];
                            while ((cv_uchroad_fow != null) && !((cv_uchroad_fow.nextWay == null) ||
                                (cv_uchroad_fow.nextWay.Count == 0)))
                            {
                                cv_uchroad_fow = cv_uchroad_fow.nextWay[0];
                            }
                            cv_uchroad_fow = cv_uchroad_fow.backWay.backWay;
                            for (int k = 0; k < road.CO.Length; k++)
                            {
                                cv_uchroad_fow = cv_uchroad_fow.backWay;
                            }
                            while ((cv_uchroad_fow != null) && (cv_uchroad_fow.Status == 1) &&
                                (cv_uchroad_fow.p_color != Brushes.OrangeRed))
                            {
                                admt_ver_road_list[admt_ver_road_list.Count - 1]++;
                                if (cv_uchroad_fow.backWay != null)
                                {
                                    cv_uchroad_fow = cv_uchroad_fow.backWay;
                                }
                                else
                                {
                                    break;
                                }
                            }
                        }
                    }
                    catch (Exception ex) { }
                    try
                    {
                        for (int i = 0; i < road.DO.Length; i++)
                        {
                            Section cv_uchroad_fow = road.DO[i];
                            while ((cv_uchroad_fow != null) && !((cv_uchroad_fow.nextWay == null) ||
                                (cv_uchroad_fow.nextWay.Count == 0)))
                            {
                                cv_uchroad_fow = cv_uchroad_fow.nextWay[0];
                            }
                            cv_uchroad_fow = cv_uchroad_fow.backWay.backWay;
                            for (int k = 0; k < road.DO.Length; k++)
                            {
                                cv_uchroad_fow = cv_uchroad_fow.backWay;
                            }
                            while ((cv_uchroad_fow != null) && (cv_uchroad_fow.Status == 1) &&
                                (cv_uchroad_fow.p_color != Brushes.OrangeRed))
                            {
                                admt_ver_road_list[admt_ver_road_list.Count - 1]++;
                                if (cv_uchroad_fow.backWay != null)
                                {
                                    cv_uchroad_fow = cv_uchroad_fow.backWay;
                                }
                                else
                                {
                                    break;
                                }
                            }
                        }
                    }
                    catch (Exception ex) { }
                    label23.Text = admt_ver_road_list.Average() + "";
                }
                else { }


                last_hor_road_green = cur_hor_road_green;

                //удаляем выехавшие машины
                foreach (var data in rem)
                    road.listMachin.Remove(data);

                //обновляем блокировку дороги
                BlokRefresh();
                //отрисовка машин
                PaintRoad();
                PaintMach();
                PaintSvet();
                MainPictureBox.Refresh();
                animeTick++;
            }
            catch (Exception ex)
            {
                anime.Enabled = false;
            }

        }


        private void t_type_CheckedChanged(object sender, EventArgs e)
        {
            if (t_type.Checked)
                x_type.Checked = false;
            else x_type.Checked = true;
        }

        private void x_type_CheckedChanged(object sender, EventArgs e)
        {
            if (x_type.Checked)
                t_type.Checked = false;
            else t_type.Checked = true;
        }

        private void AdSv_CheckedChanged(object sender, EventArgs e)
        {
            if (AdSv.Checked)
                FxSv.Checked = false;
            else FxSv.Checked = true;
        }

        private void FxSv_CheckedChanged(object sender, EventArgs e)
        {
            if (FxSv.Checked)
                AdSv.Checked = false;
            else AdSv.Checked = true;
        }

        public void PaintRoad()
        {
            Graphics g = Graphics.FromImage(MainPictureBox.Image);
            
            Brush h = Brushes.Blue;
            g.FillRectangle(Brushes.White,0,0,MainPictureBox.Image.Size.Width,MainPictureBox.Image.Size.Height);

            foreach (var data in road.allWay)
            {
                g.FillRectangle(data.p_color, data.X, data.Y, Section.sizePaint - 1, Section.sizePaint - 1);
            }
        }

        public void PaintMach()
        {
            Graphics g = Graphics.FromImage(MainPictureBox.Image);
            Brush r = Brushes.LightGray;
            Brush h = Brushes.Blue;
            int size = Section.sizePaint;
            foreach (var data in road.listMachin)
            {
                Image temp = new Bitmap(data.paintIMG);
                switch (data.inPoint.vector)
                {

                    case 1:
                    g.DrawImage(temp, data.inPoint.X, data.inPoint.Y + size/4 , size, size /2);
                    break;
                    case 2:
                    temp.RotateFlip(RotateFlipType.Rotate90FlipNone);
                    g.DrawImage(temp, data.inPoint.X + size / 4, data.inPoint.Y, size / 2, size);
                    break;
                    case 3:
                    temp.RotateFlip(RotateFlipType.Rotate180FlipNone);
                    g.DrawImage(temp, data.inPoint.X, data.inPoint.Y + size / 4, size, size / 2);
                    break;
                    case 4:
                    temp.RotateFlip(RotateFlipType.Rotate270FlipNone);
                    g.DrawImage(temp, data.inPoint.X + size / 4, data.inPoint.Y, size / 2, size);
                    break;
                    default :
                    g.DrawImage(temp, data.inPoint.X, data.inPoint.Y, size, size /2);
                    break;
                }
            }

        }

        /// <summary>
        /// Отрисовка TrafficLightа
        /// </summary>
        public void PaintSvet()
        {
            Graphics g = Graphics.FromImage(MainPictureBox.Image);
            List<Brush> br = new List<Brush>();
            br.Add(Brushes.Green);
            br.Add(Brushes.Yellow);
            br.Add(Brushes.Red);
            foreach (var data in road.listSvet)
            {
                g.DrawEllipse(Pens.DarkBlue, data.X, data.Y, TrafficLight.size, TrafficLight.size);
                g.DrawEllipse(Pens.DarkBlue, data.X, data.Y + TrafficLight.size + 5,TrafficLight.size, TrafficLight.size);
                g.DrawEllipse(Pens.DarkBlue, data.X, data.Y + 2*TrafficLight.size + 10,TrafficLight.size, TrafficLight.size);
                g.FillEllipse(br[data.stay], data.X, data.Y + data.stay * (TrafficLight.size + 5), TrafficLight.size, TrafficLight.size);
            }

        }

        private Dictionary<Car, int> cv_carwaylong_dict = new Dictionary<Car, int>();
        private int iter_counter = 0;
        private void timer_Tick(object sender, EventArgs e)
        {
            if (anime.Enabled)
                return;
            //изменяем TrafficLight
            foreach (var data in road.listSvet)
                data.Move();
            //изменяем цикл TrafficLightа
            Svet();
            //добавляем новые машины
            SetNewMachin();
            //сортировка списка машин
            road.listMachin.Sort(delegate(Car m1, Car m2)
            { return m1.wayState.CompareTo(m2.wayState); });
            anime.Start();
            GosNomer();
            Schetchick();
            Zelen();
            iter_counter++;
            cv_carwaylong_dict = new Dictionary<Car, int>();
            for (int i = 0; i < road.listMachin.Count; i++)
            {
                cv_carwaylong_dict.Add(road.listMachin[i], road.listMachin[i].wayState);
            }
            int max_iter_cnt = 0;
            Int32.TryParse(toolStripTextBox6.Text, out max_iter_cnt);
            if ((max_iter_cnt != 0) && (iter_counter >= max_iter_cnt))
            {
                Pause_MenuButton.Enabled = false;
                PlaySync_MenuButton.Enabled = true;
                ForwardStep_MenuItem.Enabled = true;
                timer.Enabled = false;
            }
            
        }

        /// <summary>
        /// Функция для записи номеров автомобилей въезжающих на дороги
        /// </summary>
        private void GosNomer()
        {
            Direction1_ListBox.Items.Clear();
            Direction2_ListBox.Items.Clear();
            Direction3_ListBox.Items.Clear();
            Direction4_ListBox.Items.Clear();
            label21.Text = Car.number+"";


            foreach (var temp in road.listMachin)
            {
                switch (temp.position)
                {
                    case 0:
                        Direction1_ListBox.Items.Add(temp._number.ToString());
                        break;
                    case 1:
                        Direction2_ListBox.Items.Add(temp._number.ToString());
                        break;
                    case 2:
                        Direction3_ListBox.Items.Add(temp._number.ToString());
                        break;
                    case 3:
                        Direction4_ListBox.Items.Add(temp._number.ToString());
                        break;                        
                }
            }
            
        }
                
        /// <summary>
        /// Счетчик фазы зеленого
        /// </summary>
        public int PereklZelen = 0;
        private void Zelen()
        {
            
            foreach (var temp in road.svetGroup.group1)
            {
                if (temp.stay != 0)
                {
                    label20.Text = PereklZelen.ToString();                    
                }
            }
            PereklZelen++;
        }


        /// <summary>
        /// Счетчик шагов
        /// </summary>
        private int xyx = 0;
        private void Schetchick()
        {
            xyx++;
            label18.Text = xyx+"";            
        }



        /// <summary>
        /// Выработка рандомных значений
        /// </summary>
        public Random rand = new Random();

        /// <summary>
        /// Устанавливаем новые машины на конце участков пути
        /// </summary>
        ///           
        public int sdv;


        void SetNewMachin()
        {        

            int a = Convert.ToInt32(toolStripTextBox1.Text);
            int b = Convert.ToInt32(toolStripTextBox2.Text);
            int c = Convert.ToInt32(toolStripTextBox3.Text);
            int d = Convert.ToInt32(toolStripTextBox4.Text);

            int temp = Convert.ToInt32(toolStripTextBox5.Text);
            if (road.DO != null)
            {
                int all = a + a + c + c;
                temp = (road.AO.Length * 100 * all) / temp;
                for (int i = 0; i < road.AO.Length; i++)
                    if (rand.Next(0, temp / a) == 0)
                        road.listMachin.Add(new Car(rand.Next(3, 6), road.AO[i], 0));
                for (int i = 0; i < road.AO.Length; i++)
                    if (rand.Next(0, temp / a) == 0)
                        road.listMachin.Add(new Car(rand.Next(3, 6), road.BO[i], 1));
                for (int i = 0; i < road.AO.Length; i++)
                    if (rand.Next(0, temp / c) == 0)
                        road.listMachin.Add(new Car(rand.Next(3, 6), road.CO[i], 2));
                for (int i = 0; i < road.AO.Length; i++)
                    if (rand.Next(0, temp / c) == 0)
                        road.listMachin.Add(new Car(rand.Next(3, 6), road.DO[i], 3));
            }
            else
            {
                int all = a + b + c;
                temp = (road.AO.Length * 100 * all) / temp;
                for (int i = 0; i < road.AO.Length; i++)
                    if (rand.Next(0, temp/a) == 0)
                        road.listMachin.Add(new Car(rand.Next(3, 6), road.AO[i],0));
                for (int i = 0; i < road.AO.Length; i++)
                    if (rand.Next(0, temp/a) == 0)
                        road.listMachin.Add(new Car(rand.Next(3, 6), road.BO[i],1));
                for (int i = 0; i < road.AO.Length; i++)
                    if (rand.Next(0, temp/c) == 0)
                        road.listMachin.Add(new Car(rand.Next(3, 6), road.CO[i],2));
            }
        }

        /// <summary>
        /// Обновим состояние блокировки дороги
        /// </summary>
        public void BlokRefresh()
        {
            foreach (var data in road.allWay)
                data.status = 0;
            foreach (var data in road.listSvet)
            {
                data.SetStatus();
            }
            foreach (var data in road.listMachin)
            {
                if (!data.isStay)
                    data.inPoint.status = 2;
                else data.inPoint.status = 1;
                if (data.povorot != null)
                    foreach (var temp in data.povorot.blok)
                        temp.status = 1;
            }
        }

    

        /// <summary>
        /// Расстановка ячеек для подсчета выходных машин
        /// </summary>


        /// <summary>
        /// Расстановка детекторов
        /// </summary>
        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            try
            {
                int y = e.Y;
                int x = e.X;
                foreach (var data in road.allWay)
                {
           
                    if (data.InClick(x, y))
                    {
                        if (e.Button == MouseButtons.Left)
                        {
                            if (data.p_color == Brushes.LightGray)
                            {
                                data.p_color = Brushes.DarkGray;
                            }
                            else if (data.p_color == Brushes.DarkGray)
                            {
                                data.p_color = Brushes.OrangeRed;
                            }
                            else if (data.p_color == Brushes.OrangeRed)
                            {
                                data.p_color = Brushes.LightGray;
                            }
                            PaintRoad();
                            PaintMach();
                            PaintSvet();
                            MainPictureBox.Refresh();
                        
                        }
                        else {}
     

                    }
                }
            }
            catch (Exception ex) 
            { }
        }

     

        private void CreateUpdate_MenuButton_Click(object sender, EventArgs e)
        {
            try { Reset_MenuButton_Click(sender, e); }
            catch (Exception ex) { }
            Pause_MenuButton.Enabled = false;
            ForwardStep_MenuItem.Enabled = true;
            PlaySync_MenuButton.Enabled = true;
            iter_counter = 0;
            int countWay = toolStripComboBox1.SelectedIndex + 1;

            road = new Road(sizeRoad, x_type.Checked, true, countWay);

               if(countWay == 1)
               {
                   road.GetEnd(road.AO[0]).backWay.p_color = Brushes.DarkSlateBlue;
                   road.GetEnd(road.BO[0]).backWay.p_color = Brushes.DarkSlateBlue;
                   road.GetEnd(road.CO[0]).backWay.p_color = Brushes.DarkSlateBlue;
                   road.GetEnd(road.DO[0]).backWay.p_color = Brushes.DarkSlateBlue;
               }
               else if (countWay == 2)
               {
                   road.GetEnd(road.AO[0]).backWay.backWay.p_color = Brushes.DarkSlateBlue;
                   road.GetEnd(road.BO[0]).backWay.backWay.p_color = Brushes.DarkSlateBlue;
                   road.GetEnd(road.CO[0]).backWay.backWay.p_color = Brushes.DarkSlateBlue;
                   road.GetEnd(road.DO[0]).backWay.backWay.p_color = Brushes.DarkSlateBlue;
                   road.GetEnd(road.AO[1]).backWay.backWay.p_color = Brushes.DarkSlateBlue;
                   road.GetEnd(road.BO[1]).backWay.backWay.p_color = Brushes.DarkSlateBlue;
                   road.GetEnd(road.CO[1]).backWay.backWay.p_color = Brushes.DarkSlateBlue;
                   road.GetEnd(road.DO[1]).backWay.backWay.p_color = Brushes.DarkSlateBlue;

               }
               else if (countWay == 3)
               {
                   road.GetEnd(road.AO[0]).backWay.backWay.backWay.p_color = Brushes.DarkSlateBlue;
                   road.GetEnd(road.BO[0]).backWay.backWay.backWay.p_color = Brushes.DarkSlateBlue;
                   road.GetEnd(road.CO[0]).backWay.backWay.backWay.p_color = Brushes.DarkSlateBlue;
                   road.GetEnd(road.DO[0]).backWay.backWay.backWay.p_color = Brushes.DarkSlateBlue;
                   road.GetEnd(road.AO[1]).backWay.backWay.backWay.p_color = Brushes.DarkSlateBlue;
                   road.GetEnd(road.BO[1]).backWay.backWay.backWay.p_color = Brushes.DarkSlateBlue;
                   road.GetEnd(road.CO[1]).backWay.backWay.backWay.p_color = Brushes.DarkSlateBlue;
                   road.GetEnd(road.DO[1]).backWay.backWay.backWay.p_color = Brushes.DarkSlateBlue;
                   road.GetEnd(road.AO[2]).backWay.backWay.backWay.p_color = Brushes.DarkSlateBlue;
                   road.GetEnd(road.BO[2]).backWay.backWay.backWay.p_color = Brushes.DarkSlateBlue;
                   road.GetEnd(road.CO[2]).backWay.backWay.backWay.p_color = Brushes.DarkSlateBlue;
                   road.GetEnd(road.DO[2]).backWay.backWay.backWay.p_color = Brushes.DarkSlateBlue;


               }

               else { }
                
 
            //удалить
            //Car m = new Car(5, road.GetEnd(road.AO[1],2));
            //road.listMachin.Add(m);

            PaintRoad();
            PaintMach();
            PaintSvet();
            MainPictureBox.Refresh();

            Pause_MenuButton.Enabled = false;
            PlaySync_MenuButton.Enabled = true;
            timer.Enabled = false;
 

        }

        private void Pause_MenuButton_Click(object sender, EventArgs e)
        {
            Pause_MenuButton.Enabled = false;
            PlaySync_MenuButton.Enabled = true;
            ForwardStep_MenuItem.Enabled = true;
            timer.Enabled = false;
        }

        private void ForwardStep_MenuItem_Click(object sender, EventArgs e)
        {
            int max_iter_cnt = 0;
            Int32.TryParse(toolStripTextBox6.Text, out max_iter_cnt);
            if ((max_iter_cnt == 0) || (iter_counter < max_iter_cnt))
            {
                //изменяем TrafficLight
                foreach (var data in road.listSvet)
                    data.Move();
                //добавляем новые машины
                SetNewMachin();
                //сортировка списка машин
                road.listMachin.Sort(delegate(Car m1, Car m2)
                { return m1.wayState.CompareTo(m2.wayState); });
                iter_counter++;
                anime.Start();
                GosNomer();
                Schetchick();
                Zelen();
            }
            else {
            }
        }

        private void PlaySync_MenuButton_Click(object sender, EventArgs e)
        {
            int max_iter_cnt = 0;
            Int32.TryParse(toolStripTextBox6.Text, out max_iter_cnt);
            if ((max_iter_cnt == 0) || (iter_counter < max_iter_cnt))
            {
                Pause_MenuButton.Enabled = true;
                PlaySync_MenuButton.Enabled = false;
                ForwardStep_MenuItem.Enabled = false;
                timer.Start();
            }
            else { }
        }

        private void Reset_MenuButton_Click(object sender, EventArgs e)
        {
            Pause_MenuButton.Enabled = false;
            ForwardStep_MenuItem.Enabled = false;
            PlaySync_MenuButton.Enabled = false;
            Pause_MenuButton.Enabled = false;
            AdaptiveHorizontalRoad_ListBox.Items.Clear();
            AdaptiveVerticalRoad_ListBox.Items.Clear();
            Direction1_ListBox.Items.Clear();
            Direction2_ListBox.Items.Clear();
            Direction3_ListBox.Items.Clear();
            Direction4_ListBox.Items.Clear();
            label22.Text = "0";
            label23.Text = "0";
            label18.Text = "0";
            label20.Text = "0";
            label21.Text = "0";
            Car.number = 0;
            iter_counter = 0;
            PereklZelen = 0;
            xyx = 0;
            cargraf_1road = new List<List<int>>();
            cargraf_2road = new List<List<int>>();
            cargraf_3road = new List<List<int>>();
            cargraf_4road = new List<List<int>>();
            admt_hor_road_list = new List<int>();
            admt_ver_road_list = new List<int>();
            timer.Enabled = false;
            MainPictureBox.Image = new Bitmap(950, 700);
            MainPictureBox.Refresh();
            road = null;
            sizeRoad = 30;
        }

        private void toolStripTextBox6_Click(object sender, EventArgs e)
        {

        }

        private Form cv_carspeedgraf_frm = null;
        private void CarSpeedGraf_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (cv_carspeedgraf_frm == null)
            {
                cv_carspeedgraf_frm = new Form() { Text="Таблица скоростей машин", FormBorderStyle= System.Windows.Forms.FormBorderStyle.FixedSingle,
                MinimizeBox=false, MaximizeBox=false, ShowInTaskbar=false, ShowIcon=false, Width=1000, Height=600};
                cv_carspeedgraf_frm.Load +=new EventHandler(delegate(object cv_obj, EventArgs cv_args)
                    {
                        try
                        {
                            TextBox cv_carinf_txbox = new TextBox() { Dock = DockStyle.Fill, Multiline = true, ScrollBars = ScrollBars.Both, ReadOnly=true,
                            Font = new Font(FontFamily.GenericMonospace, 8), BackColor= Color.White, ForeColor=Color.Black};
                            cv_carspeedgraf_frm.Controls.Add(cv_carinf_txbox);
                            cv_carinf_txbox.Text = "|   Road    |                       Горизонтальная                      |                       Вертикальная                        |\r\n" +
                             "|-----------+-------------------------------------------------+-------------------------------------------------|\r\n" +
                             "|Направление|             1          |         2              |         3              |          4             |\r\n" +
                             "| движения  |                        |                        |                        |                        |\r\n" +
                             "|-----------+------------------------+------------------------+------------------------+------------------------|\r\n" +
                             "| Мгновенна |  0 |  1 |  2 | 3  |  4 | 0  |  1 | 2  |  3 | 4  |  0 | 1  |  2 | 3  |  4 | 0  |  1 |  2 | 3  | 4  |\r\n" +
                             "|  скорость |    |    |    |    |    |    |    |    |    |    |    |    |    |    |    |    |    |    |    |    |\r\n" +
                             "|-----------+----+----+----+----+----+----+----+----+----+----+----+----+----+----+----+----+----+----+----+----|\r\n";
                            string cv_itercar_infstr = "";
                            for (int i = 0; i < cargraf_1road.Count; i++)
                            {
                                cv_itercar_infstr = cv_itercar_infstr +  i.ToString().PadRight(10).PadLeft(13);
                                for (int k = 0; k < cargraf_1road[i].Count; k++)
                                {
                                    cv_itercar_infstr = cv_itercar_infstr + cargraf_1road[i][k].ToString().PadRight(4).PadLeft(5);
                                }
                                for (int k = 0; k < cargraf_2road[i].Count; k++)
                                {
                                    cv_itercar_infstr = cv_itercar_infstr + cargraf_2road[i][k].ToString().PadRight(4).PadLeft(5);
                                }
                                for (int k = 0; k < cargraf_3road[i].Count; k++)
                                {
                                    cv_itercar_infstr = cv_itercar_infstr + cargraf_3road[i][k].ToString().PadRight(4).PadLeft(5);
                                }
                                for (int k = 0; k < cargraf_4road[i].Count; k++)
                                {
                                    cv_itercar_infstr = cv_itercar_infstr + cargraf_4road[i][k].ToString().PadRight(4).PadLeft(5);
                                }
                                cv_itercar_infstr = cv_itercar_infstr + "\r\n";
                            }
                            cv_carinf_txbox.Text += cv_itercar_infstr;
                        }
                        catch (Exception ex)
                        { }
                    });
                cv_carspeedgraf_frm.FormClosed += new FormClosedEventHandler(delegate(object cv_obj, FormClosedEventArgs cv_args)
                    {
                        cv_carspeedgraf_frm = null;
                    });
                cv_carspeedgraf_frm.Show(this);
            }
            else {
                cv_carspeedgraf_frm.Close();
                cv_carspeedgraf_frm = null;
            }



        }



        private void MainPictureBox_Click(object sender, EventArgs e)
        {

        }

        private void label23_Click(object sender, EventArgs e)
        {

        }

        private void label22_Click(object sender, EventArgs e)
        {

        }

        private void toolStripTextBox5_Click(object sender, EventArgs e)
        {

        }

        public void toolStripTextBox7_Click(object sender,  EventArgs e)
        {

        }


        private void Direction3_ListBox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void x_type_Click(object sender, EventArgs e)
        {

        }

        private void t_type_Click(object sender, EventArgs e)
        {

        }

        private void AdSv_Click(object sender, EventArgs e)
        {

        }

        private void FxSv_Click(object sender, EventArgs e)
        {

        }

        private void toolStripTextBox8_Click(object sender, EventArgs e)
        {

        }

        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {

        }

        private void label26_Click(object sender, EventArgs e)
        {

        }

        private void label43_Click(object sender, EventArgs e)
        {

        }

        private void label42_Click(object sender, EventArgs e)
        {

        }

        private void label62_Click(object sender, EventArgs e)
        {

        }

        private void label25_Click(object sender, EventArgs e)
        {

        }

        class RoadIterOptions
        {
            public string intersec_type = "X";
            public int line_number = 1;
            public int[] car_power = new int[4] { 25, 25, 25, 25 };
            public int intense = 500;
            public int[][] detectors = new int[][] { 
                new int[] { 30, 210 }, new int[] { 30, 210 } 
            };
        }

        RoadIterOptions[] ParseInstructions(string file_instruct_name)
        {
            FileStream instr_file = new FileStream(file_instruct_name, FileMode.Open);
            StreamReader instr_stream = new StreamReader(instr_file);
            string instr_full_str = instr_stream.ReadToEnd();
            instr_full_str = instr_full_str.Replace("\r\n", "\n");
            Regex cv_instr_grms_dlm = new Regex("\\n\\*+\\n?", (RegexOptions.CultureInvariant |
                RegexOptions.IgnoreCase | RegexOptions.Singleline));
            string[] cv_drinsts_strs = cv_instr_grms_dlm.Split(instr_full_str);
            RoadIterOptions[] cv_iter_opts = new RoadIterOptions[cv_drinsts_strs.Length];
            for (int i = 0; i < cv_drinsts_strs.Length; i++)
            {
                try
                {
                    cv_iter_opts[i] = new RoadIterOptions();
                    string[] cv_oneomt_grm_strs = cv_drinsts_strs[i].Split(new string[] { "\n" },
                        StringSplitOptions.None);
                    Regex cv_instype_dlm = new Regex("^(\\w+?)(.*?)$", (RegexOptions.CultureInvariant |
                    RegexOptions.IgnoreCase | RegexOptions.Singleline));
                    cv_iter_opts[i].intersec_type = cv_instype_dlm.Match(cv_oneomt_grm_strs[0]).
                        Groups[1].Captures[0].Value;
                    if ((cv_iter_opts[i].intersec_type != "X") || (cv_iter_opts[i].intersec_type != "T"))
                    {
                        cv_iter_opts[i].intersec_type = "X";
                    }
                    else { }
                    Regex cv_linecnt_dlm = new Regex("^(.*?)(\\s+?)(\\d+)(.*?)$", (RegexOptions.CultureInvariant |
                    RegexOptions.IgnoreCase | RegexOptions.Singleline));
                    string cv_num_str = cv_linecnt_dlm.Match(cv_oneomt_grm_strs[0]).
                        Groups[3].Captures[0].Value;
                    Int32.TryParse(cv_num_str, out cv_iter_opts[i].line_number);

                    Regex cv_carpower_dlm = new Regex("^((\\d+)(,|\\s)?){4}(.*?)$", (RegexOptions.CultureInvariant |
                    RegexOptions.IgnoreCase | RegexOptions.Singleline));
                    Match cv_matches = cv_carpower_dlm.Match(cv_oneomt_grm_strs[1]);
                    for (int k = 0; k < cv_matches.Groups[2].Captures.Count; k++)
                    {
                        cv_num_str = cv_matches.Groups[2].Captures[k].Value;
                        Int32.TryParse(cv_num_str, out cv_iter_opts[i].car_power[k]);
                    }

                    Regex cv_intense_dlm = new Regex("^(\\d+)(.*?)$", (RegexOptions.CultureInvariant |
                   RegexOptions.IgnoreCase | RegexOptions.Singleline));
                    cv_num_str = cv_intense_dlm.Match(cv_oneomt_grm_strs[2]).
                        Groups[1].Captures[0].Value;
                    Int32.TryParse(cv_num_str, out cv_iter_opts[i].intense);

                    Regex cv_detcords_dlm = new Regex("^((\\d+)(,\\s*?)(\\d+)(,|\\s+)?)+(.*?)$", (RegexOptions.CultureInvariant |
                    RegexOptions.IgnoreCase | RegexOptions.Singleline));
                    cv_matches = cv_detcords_dlm.Match(cv_oneomt_grm_strs[3]);
                    List<int[]> cv_greydet_list = new List<int[]>();
                    for (int k = 0; k < cv_matches.Groups[2].Captures.Count; k++)
                    {
                        cv_num_str = cv_matches.Groups[2].Captures[k].Value;
                        int[] cv_cordelm_arr = new int[2];
                        Int32.TryParse(cv_num_str, out cv_cordelm_arr[0]);
                        cv_num_str = cv_matches.Groups[4].Captures[k].Value;
                        Int32.TryParse(cv_num_str, out cv_cordelm_arr[1]);
                        cv_greydet_list.Add(cv_cordelm_arr);
                    }
                    cv_iter_opts[i].detectors = cv_greydet_list.ToArray();



                }
                catch (Exception ex) {
                    cv_iter_opts[i] = null;
                }
                    

            }

            List<RoadIterOptions> cv_real_opts_list = new List<RoadIterOptions>();
            for (int i = 0; i < cv_iter_opts.Length; i++)
            {
                if (cv_iter_opts[i] != null)
                {
                    cv_real_opts_list.Add(cv_iter_opts[i]);
                }
                else { }
            }
            cv_iter_opts = cv_real_opts_list.ToArray();

            return cv_iter_opts;
            
        }

        private string out_work_log = "";
        private void MainForm_Load(object sender, EventArgs e)
        {
            OpenFileDialog input_file_dialog = new OpenFileDialog();
            input_file_dialog.Title = "Выберите входной файл с инструкциями пакетного режима";
            RoadIterOptions[] input_instructions = null;
            while (true)
            {
                if (input_file_dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    input_instructions = ParseInstructions(input_file_dialog.FileName);
                    if (input_instructions != null) { break; }
                    else { }

                }
                else {
                    this.Close();
                    break;
                }
            }
            Thread instr_thread = new Thread(new ParameterizedThreadStart(delegate(object cv_obj)
                {
                    Form orig_form = (Form)cv_obj;
                    for (int i = 0; i < input_instructions.Length; i++)
                    {   int cv_iter_quant = input_instructions.Length * 2118;
                        Thread.Sleep(1000);
                        this.Invoke(new Action(delegate()
                        {
                            toolStripTextBox1.Text = input_instructions[i].car_power[0].ToString();
                            toolStripTextBox2.Text = input_instructions[i].car_power[1].ToString();
                            toolStripTextBox3.Text = input_instructions[i].car_power[2].ToString();
                            toolStripTextBox4.Text = input_instructions[i].car_power[3].ToString();
                            if (input_instructions[i].intersec_type == "X")    { x_type.Checked = true; }
                            else { t_type.Checked = true;  }
                            if (input_instructions[i].line_number == 1)
                            { toolStripComboBox1.SelectedIndex = 0; 
                            }
                            else if (input_instructions[i].line_number == 2)
                            {
                                toolStripComboBox1.SelectedIndex = 1; 
                            }
                            else if (input_instructions[i].line_number == 3)
                            {
                                toolStripComboBox1.SelectedIndex = 2;
                            }
                            else 
                            {
                                toolStripComboBox1.SelectedIndex = 0; 
                            }
                            toolStripTextBox5.Text = input_instructions[i].intense.ToString();
                            CreateUpdate_MenuButton.PerformClick();

                            if (input_instructions[i].detectors != null)
                            {
                                for (int k = 0; k < input_instructions[i].detectors.Length; k++)
                                {
                                    int det_x_pos = input_instructions[i].detectors[k][0];
                                    int det_y_pos = input_instructions[i].detectors[k][0];
                                    foreach (var data in road.allWay)
                                    {
                                        if (data.InClick(det_x_pos, det_y_pos))
                                        {
                                            data.p_color = Brushes.DarkGray;
                                        }
                                        else { }
                                    }

                                }
                            }
                            else { }

                            PlaySync_MenuButton.PerformClick();

                        }), null);
                        Thread.Sleep(1000);
                        while (!PlaySync_MenuButton.Enabled)
                        {
                            int cv_iter_cnt = i * 2118;
                            this.Invoke(new Action(delegate()
                            {
                                try { cv_iter_cnt += Int32.Parse(label18.Text); }
                                catch (Exception ex) { }
                            }), null);
                            string cv_inter_info = "(Выполняется " + (i + 1) + "-ое задание из " + input_instructions.Length +
                            "; Всего итерация " + cv_iter_cnt + " из " + cv_iter_quant + ")";

                            this.Invoke(new Action(delegate()
                            {
                                label64.Text = cv_inter_info;
                            }), null);

                            Thread.Sleep(1000);
                        }

                        string current_iter_log="";
                        this.Invoke(new Action(delegate()
                        {
                            current_iter_log += label12.Text + "\n";
                            current_iter_log += label22.Text + "\n";
                            current_iter_log += label23.Text + "\n";

                        }), null);
                        current_iter_log += "***\n";
                        out_work_log += current_iter_log;


                }

                    SaveFileDialog output_file_dialog = new SaveFileDialog();
                    output_file_dialog.Title = "Выберите вызодной файл для сохранения результатов работы";
                    if (output_file_dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        Stream out_file_stream =  output_file_dialog.OpenFile();
                        StreamWriter out_file_wrt = new StreamWriter(out_file_stream);
                        out_file_wrt.Write(out_work_log);

                    }
                    else { }


                    Application.Exit();


                 }
            ));
            this.BeginInvoke(new Action(delegate()
                {
                    instr_thread.Start(this);
                }
            ), null);
          
 

        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }







    }
}
