using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Road
{
    public class Turn
    {
        /// <summary>
        /// Section для Turnа
        /// </summary>
        public Section way = null;

        /// <summary>
        /// Section открыт для TrafficLightа
        /// </summary>
        public bool svetofor = true;

        /// <summary>
        /// Блокируемые участки дороги при въезде на Turn для другого транспорта
        /// </summary>
        public List<Section> blok = new List<Section>();

        /// <summary>
        /// Нет ли встречных машин
        /// </summary>
        public bool IsFree()
        {
            foreach (var data in blok)
            {
                if (data.status != 0)
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Блокируем участки движения
        /// </summary>
        public void Blok()
        {
            foreach(var temp in blok)
            {
                temp.status = 1;
            }
        }

        /// <summary>
        /// Разблокируем участки движения
        /// </summary>
        public void UnBlok()
        {
            foreach (var temp in blok)
            {
                temp.status = 0;
            }
        }

        public Turn(Section way)
        {
            this.way = way;
        }

        public Turn(Section way, Section bk)
        {
            this.way = way;
            blok.Add(bk);
        }
    }
}
