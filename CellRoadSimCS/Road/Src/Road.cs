using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Road
{
    public class Road
    {
        /// <summary>
        /// Список всех машин
        /// </summary>
        public List<Car> listMachin = new List<Car>();

        /// <summary>
        /// Переключение TrafficLightов по команде
        /// </summary>
        public Svetgroup svetGroup = null;

        /// <summary>
        /// Список TrafficLightов на дороге
        /// </summary>
        public List<TrafficLight> listSvet = new List<TrafficLight>();

        public Section[] AO = null;
        public Section[] OA = null;
        public Section[] BO = null;
        public Section[] OB = null;
        public Section[] CO = null;
        public Section[] OC = null;
        public Section[] OD = null;
        public Section[] DO = null;
        /// <summary>
        /// Все участки дороги
        /// </summary>
        public List<Section> allWay = null;

        /// <summary>
        /// имеется ли TrafficLight
        /// </summary>
        public bool haveSvet = false;

        /// <summary>
        /// Размерность дороги
        /// </summary>
        public int SizeWay = 1;


        public void GreatWay(Section temp, int vector, int size, int wight, int height, int mW, int mH, int polos, bool end, bool naprav)
        {
            allWay.Add(temp);
            Section next = temp;
            for (int i = 1; i < size ; i++)
            {
                next = new Section (i, next, vector, size, wight, height, mW, mH, polos, naprav);
                allWay.Add(next);
                if (next.backWay != null)
                next.backWay.nextWay.Add(next);
            }
            next.endway = end;
        }

        public Section GetEnd(Section way)
        {
            Section temp = way;
            while (temp.nextWay.Count != 0)
            {
                temp = temp.nextWay[0];
            }
            return temp;
        }

        public Section GetEnd(Section way, int count)
        {
            Section temp = GetEnd(way);
            for (int i = 0; i < count; i++)
            {
                temp = temp.backWay;
            }
            return temp;
        }

        public Section GetNext(Section way, int count)
        {
            Section temp = way;
            for (int i = 0; i < count; i++)
            {
                temp = temp.nextWay[0];
            }
            return temp;
        }

        public void SetSvaz(Section rite, Section left)
        {
            Section temp1 = rite;
            Section temp2 = left;
            while (temp1 != null && temp2 != null && temp2.nextWay.Count > 0 && temp1.nextWay.Count>0)
            {
                temp1.nextWay.Add(temp2.nextWay[0]);
                temp2.nextWay.Add(temp1.nextWay[0]);
                temp2 = temp2.nextWay[0];
                temp1 = temp1.nextWay[0];
            }
        }

        /// <summary>
        /// Конструктор дороги
        /// </summary>
        public Road(int size, bool typeX, bool haveSvet, int countWay)
        {
            allWay = new List<Section>();

            this.haveSvet = haveSvet;
            this.SizeWay = countWay;
            {
                AO = new Section[countWay];
                OA = new Section[countWay];
                for (int j = 0; j < countWay; j++)
                {
                    AO[j] = new Section(0, null, 1, size, 950, 700, -1, 0, j, true);
                    GreatWay(AO[j], 1, size, 950, 700, -1, 0, j, false, true);
                    OA[j] = new Section(0, null, 1, size, 950, 700, 1, 0, j, false);
                    GreatWay(OA[j], 1, size, 950, 700, 1, 0, j, true, false);
                    if (j > 0)
                    {
                        SetSvaz(AO[j - 1], AO[j]);
                        SetSvaz(OA[j - 1], OA[j]);
                    }
                }
            }
            {
                OB = new Section[countWay];
                BO = new Section[countWay];
                for (int j = 0; j < countWay; j++)
                {
                    OB[j] = new Section(0, null, 3, size, 950 - 2 * Section.sizePaint, 700, -1, 0, -(j + 1), false);
                    GreatWay(OB[j], 3, size, 950 - 2 * Section.sizePaint, 700, -1, 0, -(j + 1), true, false);
                    BO[j] = new Section(0, null, 3, size, 950 - 2*Section.sizePaint , 700, 1, 0, -j - 1, true);
                    GreatWay(BO[j], 3, size, 950 - 2 * Section.sizePaint, 700, 1, 0, -j - 1, false, true);
                    if (j > 0)
                    {
                        SetSvaz(OB[j - 1], OB[j]);
                        SetSvaz(BO[j - 1], BO[j]);
                    }
                }
            
            }
            if (typeX)
            {
                OC = new Section[countWay];
                for (int j = 0; j < countWay; j++)
                {
                    OC[j] = new Section(0, null, 2, size, 950, 700, 0, 1, -(j + 1), false);
                    GreatWay(OC[j], 2, size, 950, 700, 0, 1, -(j + 1), true, false);
                    if (j > 0)
                        SetSvaz(OC[j - 1], OC[j]);
                }
            }
            if (typeX)
            {
                DO = new Section[countWay];
                for (int j = 0; j < countWay; j++)
                {
                    DO[j] = new Section(0, null, 4, size, 950, 700 - 2 * Section.sizePaint, 0, 1, j, true);
                    GreatWay(DO[j], 4, size, 950, 700 - 2 * Section.sizePaint, 0, 1, j, false, true);
                    if (j > 0)
                      SetSvaz(DO[j - 1], DO[j]);
                }
            }
            {
                CO = new Section[countWay];
                for (int j = 0; j < countWay; j++)
                {
                    CO[j] = new Section(0, null, 2, size, 950, 700, 0, -1, -(j + 1), true);
                    GreatWay(CO[j], 2, size, 950, 700, 0, -1, -(j + 1), false, true);
                    if (j > 0)
                        SetSvaz(CO[j - 1], CO[j]);
                }
            }
            {
                OD = new Section[countWay];
                for (int j = 0; j < countWay; j++)
                {
                    OD[j] = new Section(0, null, 4, size, 950, 700 - 2 * Section.sizePaint, 0, -1, j, false);
                    GreatWay(OD[j], 4, size, 950, 700 - 2 * Section.sizePaint, 0, -1, j, true, false);
                    if (j > 0)
                        SetSvaz(OD[j - 1], OD[j]);
                }
            }

            if ( countWay == 1)
            {
                Section temp = GetEnd(AO[0]);
                temp.listPovorot.Add(new Turn(OA[0] ));
                temp.listPovorot.Add(new Turn(OD[0], GetEnd(BO[0]) ));
                if (typeX && OC[0].nextWay.Count>0)
                    temp.listPovorot.Add(new Turn(OC[0].nextWay[0] ));
                temp = GetEnd(BO[0]);
                temp.listPovorot.Add(new Turn( OB[0] ));
                temp.listPovorot.Add(new Turn( OD[0].nextWay[0]));
                if (typeX)
                    temp.listPovorot.Add(new Turn(OC[0], GetEnd(AO[0])));
                temp = GetEnd(CO[0]);
                temp.listPovorot.Add(new Turn(OB[0].nextWay[0]));
                if (typeX)
                {
                    temp.listPovorot.Add(new Turn(OA[0], GetEnd(DO[0])));
                    temp.listPovorot.Add(new Turn(OC[0]));
                }
                else temp.listPovorot.Add(new Turn(OA[0]));
                if (typeX)
                {
                    temp = GetEnd(DO[0]);
                    temp.listPovorot.Add(new Turn( OB[0], GetEnd(CO[0]) ));
                    temp.listPovorot.Add(new Turn( OA[0].nextWay[0] ));
                    temp.listPovorot.Add(new Turn( OD[0] ));
                }
            }
            if (countWay == 2)
            {
                Section temp = GetEnd(AO[0]);
                temp.listPovorot.Add(new Turn( OA[0] ));
                temp.listPovorot.Add(new Turn(OD[0], GetEnd(BO[0])));
                temp = GetEnd(AO[1]);
                temp.listPovorot.Add(new Turn( OA[1] ));
                temp = GetEnd(AO[1], 1);
                temp.listPovorot.Add(new Turn(GetEnd(AO[1])));

                if (typeX)
                {
                    temp.listPovorot.Add(new Turn(GetNext(OC[1], 2)));
                    temp = GetEnd(AO[1]);
                    temp.listPovorot.Add(new Turn(GetNext(OC[1], 2)));
                }

                temp = GetEnd(BO[0]);
                temp.listPovorot.Add(new Turn(OB[0]));
                if (typeX)
                    temp.listPovorot.Add(new Turn(OC[0], GetEnd(AO[0])));
                temp = GetEnd(BO[1]);
                temp.listPovorot.Add(new Turn(OB[1]));
                temp = GetEnd(BO[1],1);
                temp.listPovorot.Add(new Turn(GetEnd(BO[1])));
                temp.listPovorot.Add(new Turn(GetNext(OD[1], 2)));
                temp = GetEnd(BO[1]);
                temp.listPovorot.Add(new Turn(GetNext(OD[1], 2)));

                if (typeX)
                {
                    temp = GetEnd(DO[0]);
                    temp.listPovorot.Add(new Turn(OD[0]));
                    temp.listPovorot.Add(new Turn(OB[0], GetEnd(CO[0])));
                    temp = GetEnd(DO[1]);
                    temp.listPovorot.Add(new Turn(OD[1]));
                    temp = GetEnd(DO[1], 1);
                    temp.listPovorot.Add(new Turn(GetNext(OA[1], 2)));
                    temp.listPovorot.Add(new Turn(GetEnd(DO[1])));
                    temp = GetEnd(DO[1]);
                    temp.listPovorot.Add(new Turn(GetNext(OA[1], 2)));
                }

                temp = GetEnd(CO[0]);
                if (typeX)
                temp.listPovorot.Add(new Turn(OC[0]));
                if (typeX)
                    temp.listPovorot.Add(new Turn(OA[0], GetEnd(DO[0])));
                else temp.listPovorot.Add(new Turn(OA[0]));
                temp = GetEnd(CO[1]);
                if (typeX)
                temp.listPovorot.Add(new Turn(OC[1]));
                temp = GetEnd(CO[1], 1);
                temp.listPovorot.Add(new Turn(GetNext(OB[1], 2)));
                if (typeX)
                {
                    temp.listPovorot.Add(new Turn(GetEnd(CO[1])));
                    temp = GetEnd(CO[1]);
                    temp.listPovorot.Add(new Turn(GetNext(OB[1], 2)));
                }
            }
            if (countWay == 3)
            {
                Section temp = GetEnd(AO[0]);
                temp.listPovorot.Add(new Turn(OD[0], GetEnd(BO[0])));
                temp = GetEnd(AO[1]);
                temp.listPovorot.Add(new Turn(OA[1]));
                temp.listPovorot.Add(new Turn(OA[2]));
                if (typeX)
                {
                    temp = GetEnd(AO[2], 2);
                    temp.listPovorot.Add(new Turn(GetNext(OC[1], 3)));
                    temp.listPovorot.Add(new Turn(GetNext(OC[2], 3)));
                    temp = GetEnd(AO[2]);
                    temp.listPovorot.Add(new Turn(GetNext(OC[2],3)));
                }
                else
                {
                    temp = GetEnd(AO[2]);
                    temp.listPovorot.Add(new Turn(OA[2]));
                }

                temp = GetEnd(BO[0]);
                if (typeX)
                    temp.listPovorot.Add(new Turn(OC[0], GetEnd(AO[0])));
                else
                {
                    temp.listPovorot.Add(new Turn(OB[0]));
                }
                temp = GetEnd(BO[1]);
                temp.listPovorot.Add(new Turn(OB[1]));
                temp.listPovorot.Add(new Turn(OB[2]));
                temp = GetEnd(BO[2], 2);
                temp.listPovorot.Add(new Turn(GetNext(OD[1], 3)));
                temp.listPovorot.Add(new Turn(GetNext(OD[2], 3)));
                temp = GetEnd(BO[2]);
                temp.listPovorot.Add(new Turn(GetNext(OD[2], 3)));

                if (typeX)
                {
                    temp = GetEnd(DO[0]);
                    temp.listPovorot.Add(new Turn(OB[0], GetEnd(CO[0])));
                    temp = GetEnd(DO[1]);
                    temp.listPovorot.Add(new Turn(OD[1]));
                    temp.listPovorot.Add(new Turn(OD[2]));
                    temp = GetEnd(DO[2], 2);
                    temp.listPovorot.Add(new Turn(GetNext(OA[1], 3)));
                    temp.listPovorot.Add(new Turn(GetNext(OA[2], 3)));
                    temp = GetEnd(DO[2]);
                    temp.listPovorot.Add(new Turn(GetNext(OA[2], 3)));
                }
                temp = GetEnd(CO[0]);
                if (!typeX)
                    temp.listPovorot.Add(new Turn(OA[0]));
                else
                    temp.listPovorot.Add(new Turn(OA[0], GetEnd(DO[0])));
                temp = GetEnd(CO[1]);
                if (typeX)
                {
                    temp.listPovorot.Add(new Turn(OC[1]));
                    temp.listPovorot.Add(new Turn(OC[2]));
                }
                else
                {
                    temp.listPovorot.Add(new Turn(GetNext(OB[0],2)));
                }
                temp = GetEnd(CO[2], 2);
                temp.listPovorot.Add(new Turn(GetNext(OB[1], 3)));
                temp.listPovorot.Add(new Turn(GetNext(OB[2], 3)));
                temp = GetEnd(CO[2]);
                temp.listPovorot.Add(new Turn(GetNext(OB[2], 3)));
            }

            List<Section> sv1 = new List<Section>();
            List<Section> sv2 = new List<Section>();
            List<Section> sv3 = new List<Section>();
            List<Section> sv4 = new List<Section>();

            for (int i = 0; i < countWay; i++)
            {
                sv1.Add(GetEnd(AO[i], countWay));
                sv2.Add(GetEnd(BO[i], countWay));
                sv3.Add(GetEnd(CO[i], countWay));                           
                if (typeX)
                    sv4.Add(GetEnd(DO[i], countWay ));
            }

            int smeh = TrafficLight.size + 5;
            //Добавляем TrafficLightы
            listSvet.Add(new TrafficLight(sv1, 0,
                950 / 2 - countWay * Section.sizePaint - smeh,
                700 / 2 + countWay * Section.sizePaint));
            listSvet.Add(new TrafficLight(sv2, 0,
                950 / 2 + countWay * Section.sizePaint + 5,
                700 / 2 - countWay * Section.sizePaint - 3 * smeh));
            listSvet.Add(new TrafficLight(sv3, 2,
                950 / 2 - countWay * Section.sizePaint - smeh,
                700 / 2 - countWay * Section.sizePaint - 3 * smeh));
            TrafficLight non = null;
            if (typeX)
            {
                listSvet.Add(new TrafficLight(sv4, 2,
                     950 / 2 + countWay * Section.sizePaint + 5,
                     700 / 2 + countWay * Section.sizePaint));
                non = listSvet[3];
            }
            SetSvaz();
            svetGroup = new Svetgroup(listSvet[0], listSvet[1], listSvet[2], non);
        }

        /// <summary>
        /// Устанавливаем смежные участки пути
        /// </summary>
        public void SetSvaz()
        {
            for (int i = 0; i < allWay.Count - 1; i++)
                for (int j = i + 1; j< allWay.Count; j++)
                    if (allWay[i].X == allWay[j].X && allWay[i].Y == allWay[j].Y)
                        SetSmeg(allWay[i], allWay[j]);
        }

        /// <summary>
        /// Смежные участки пути
        /// </summary>
        public void SetSmeg (Section a1, Section a2)
        {
            if (a1 == a2)
                return;
            a1.smeg.Add(a2);
            a2.smeg.Add(a1);
        }
    }
}
