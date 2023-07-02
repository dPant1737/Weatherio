using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationSettings
{
    public interface IApplicationConfig
    {
        public string MetoBaseURL { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string WeatherParams { get; set; }
    }
}
