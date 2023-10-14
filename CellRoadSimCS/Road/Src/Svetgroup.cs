using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;



namespace Road
{
    public class Svetgroup
    {
        public List<TrafficLight> group1 = new List<TrafficLight>();
        public List<TrafficLight> group2 = new List<TrafficLight>();



        public Svetgroup(TrafficLight g11, TrafficLight g12, TrafficLight g21, TrafficLight g22)
        {
            if (g11 != null)
            {
                group1.Add(g11);
            }
            if (g12 != null)
            {
                group1.Add(g12);
            }
            if (g21 != null)
            {
                group2.Add(g21);
            }
            if (g22 != null)
            {
                group2.Add(g22);
            }
        }

        public  void AssimSvet(int Green, int Period)
        {
            foreach (var data in group1)
            {
                TrafficLight.time_Green = Green;
                TrafficLight.time_Red = Period - TrafficLight.time_Green - 2;
                data.time = 0;
                data.stay = 0;
                data.SetStatus();
            }
            foreach (var data in group2)
            {
                data.time = 0;
                data.stay = 0;
                data.SetStatus();
            }

        
        }

        public void SetGreenG1()
        {
            if (group1.Count <= 0 || group2.Count <=0 )
                return;

            if (group1[0].stay == 0)
                return;
            foreach (var data in group1)
            {
                data.time = 0;
                data.stay = 0;
                data.SetStatus();
            }
            foreach (var data in group2)
            {
                data.time = 0;
                data.stay = 2;
                data.SetStatus();
            }
        }

        public void SetGreenG2()
        {
            if (group1.Count <= 0 || group2.Count <= 0)
                return;

            if (group2[0].stay == 0)
                return;
            foreach (var data in group2)
            {
                data.time = 0;
                data.stay = 0;
                data.SetStatus();
            }
            foreach (var data in group1)
            {
                data.time = 0;
                data.stay = 2;
                data.SetStatus();
            }
        }
    }
}
