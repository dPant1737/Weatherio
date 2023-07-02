using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PublicModels.MetoDataModels
{
    public class AppMetoDataModel
    {
        public double Temperature { get; set; }

        public int Humidity { get; set; }

        public double WindSpeed { get; set; }

        public double Precipitation { get; set; }

        public double Snow { get; set; }

        public double Rain { get; set; }

        public double Showers { get; set; }

        public string Time { get; set; }
    }
}
