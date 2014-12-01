using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hqub.Speckle.Core.Model
{
    public class CorrelationValue
    {
        private DateTime time;

        public double Value { get; set; }

        public DateTime Time
        {
            get
            {
                return this.time;
            }
            set
            {
                this.time = value;
            }
        }

        public string ImageName { get; set; }
        public string ImagePath { get; set; }
        public string EtalonePath { get; set; }

        /// <summary>
        /// Размеры изображения
        /// </summary>
        public Size Area { get; set; }
    }
}
