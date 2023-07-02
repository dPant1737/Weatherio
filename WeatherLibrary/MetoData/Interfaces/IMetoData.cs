using ApplicationSettings;
using PublicModels.MetoDataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherLibrary.MetoData.Interfaces
{
    public interface IMetoData
    {

        public AppMetoDataModel GetMetoData();
        string ClothingDecider(double feltTemperature, string clothingGenderPreference, double precipatation, double snow, double rain, double showers, int userColdTemp, int userWarmTemp, int userHotTemp);

        double CalculatingFeltTemperature(double currentTemperature, int humidity, double windSpeed);

        int PrecipatationType(double snow, double rain, double precipatation, double showers);

        string PrecipatationStatements(double precipatation, double snow, double rain, double showers);
    }
}
