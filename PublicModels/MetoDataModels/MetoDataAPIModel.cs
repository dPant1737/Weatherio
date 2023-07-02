using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PublicModels.MetoDataModels
{
    public class CurrentWeather
    {
        public double temperature { get; set; }
        public double windspeed { get; set; }
        public double winddirection { get; set; }
        public int weathercode { get; set; }
        public int is_day { get; set; }
        public string time { get; set; }
    }

    public class Hourly
    {
        public List<string> time { get; set; }
        public List<double> temperature_2m { get; set; }
        public List<int> relativehumidity_2m { get; set; }
        public List<double> dewpoint_2m { get; set; }
        public List<double> apparent_temperature { get; set; }
        public List<int> precipitation_probability { get; set; }
        public List<double> precipitation { get; set; }
        public List<double> rain { get; set; }
        public List<double> showers { get; set; }
        public List<double> snowfall { get; set; }
        public List<double> snow_depth { get; set; }
        public List<double> windspeed_10m { get; set; }
        public List<double> windspeed_80m { get; set; }
        public List<double> windspeed_120m { get; set; }
        public List<double> windspeed_180m { get; set; }
    }

    public class HourlyUnits
    {
        public string time { get; set; }
        public string temperature_2m { get; set; }
        public string relativehumidity_2m { get; set; }
        public string dewpoint_2m { get; set; }
        public string apparent_temperature { get; set; }
        public string precipitation_probability { get; set; }
        public string precipitation { get; set; }
        public string rain { get; set; }
        public string showers { get; set; }
        public string snowfall { get; set; }
        public string snow_depth { get; set; }
        public string windspeed_10m { get; set; }
        public string windspeed_80m { get; set; }
        public string windspeed_120m { get; set; }
        public string windspeed_180m { get; set; }
    }

    public class MetoDataAPIModel
    {

        public double latitude { get; set; }
        public double longitude { get; set; }
        public double generationtime_ms { get; set; }
        public int utc_offset_seconds { get; set; }
        public string timezone { get; set; }
        public string timezone_abbreviation { get; set; }
        public double elevation { get; set; }
        public CurrentWeather current_weather { get; set; }
        public HourlyUnits hourly_units { get; set; }
        public Hourly hourly { get; set; }
    }
}
