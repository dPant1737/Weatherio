using ApplicationSettings;
using Newtonsoft.Json;
using PublicModels.MetoDataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using WeatherLibrary.MetoData.Interfaces;

namespace WeatherLibrary.MetoData.Implementation
{
    enum Gender
    {
        male,
        female
    }
    public class MetoData : IMetoData
    {

        private IApplicationConfig _config;
        public MetoData(IApplicationConfig applicationConfig)
        {
            _config = applicationConfig;
        }

        public AppMetoDataModel GetMetoData()
        {
            AppMetoDataModel currentMetoData = new AppMetoDataModel();

            string metoDataEndPoint = GetMetoDataURLEndPoint();
            try
            {

                HttpRequestMessage request = CreateRequestMessage(metoDataEndPoint, HttpMethod.Get, string.Empty);

                using (HttpClient httpClient = new HttpClient())
                {
                    var result = httpClient.SendAsync(request).Result;
                    var response = result.Content.ReadAsStringAsync().Result;
                    MetoDataAPIModel meto = JsonConvert.DeserializeObject<MetoDataAPIModel>(response);
                    if (meto != null)
                    {

                        currentMetoData.Time = meto.current_weather.time;
                        DateTime currTime;
                        int hour = 0;
                        bool validDate = DateTime.TryParse(currentMetoData.Time, out currTime);
                        if (validDate)
                        {
                            hour = currTime.Hour;
                        }

                        currentMetoData.Temperature = meto.hourly.temperature_2m[hour];
                        currentMetoData.Humidity = meto.hourly.relativehumidity_2m[hour];
                        currentMetoData.WindSpeed = meto.hourly.windspeed_10m[hour];
                        currentMetoData.Precipitation = meto.hourly.precipitation[hour];
                        currentMetoData.Snow = meto.hourly.snowfall[hour];
                        currentMetoData.Rain = meto.hourly.rain[hour];
                        currentMetoData.Showers = meto.hourly.showers[hour];

                    }
                }
            }
            catch (Exception)
            {

                throw;
            }

            return currentMetoData;
        }

        //Calculating the actual temperature in degrees Fahrenheit using Wind Chill and Heat Index equations
        //Wind Chill equation is found on https://www.weather.gov/safety/cold-faqs#:~:text=Wind%20chill%20Temperature%20is%20only,F%20to%2018%C2%B0F.
        //Heat Index equation is found on https://www.weather.gov/ama/heatindex
        public double CalculatingFeltTemperature(double currentTemperature, int humidity, double windSpeed)
        {
            double feltTemperature = currentTemperature;
            if (currentTemperature <= 50.0 && windSpeed > 3.0)
            {
                feltTemperature = 35.74 + 0.6215 * currentTemperature - 35.75 * Math.Pow(windSpeed, 0.16) + 0.4275 * currentTemperature * Math.Pow(windSpeed, 0.16);

            }
            else if (currentTemperature >= 80.0 && humidity >= 40)
            {
                feltTemperature = -42.379 + 2.04901523 * currentTemperature + 10.14333127 * humidity - 0.22475541 * currentTemperature * humidity - 0.006838 * (currentTemperature * currentTemperature)
                - 0.05482 * (humidity * humidity) + 0.001228 * (currentTemperature * currentTemperature) * humidity + 0.0008528 * currentTemperature * (humidity * humidity)
                - 0.00000199 * (currentTemperature * currentTemperature) * (humidity * humidity);

            }
            return feltTemperature;

        }

        public string ClothingDecider(double feltTemperature, string clothingGenderPreference, double precipatation, double snow, double rain, double showers, int userColdTemp, int userWarmTemp, int userHotTemp)
        {
            Gender myGender;
            Enum.TryParse(clothingGenderPreference, out myGender);
            String stringToReturn = String.Empty;
            if (feltTemperature <= 0)
            {
                stringToReturn = $"The felt temperature is {feltTemperature}. It is unsafe to go outside as it is too cold. Please consider staying inside for your safety.";
            }
            else if (feltTemperature > 0 && feltTemperature <= 32.0)
            {
                stringToReturn = $"The felt temperature is {feltTemperature}. You can wear pants, sweatpants, heavy sweaters and a heavy winter jacket. ";
            }
            else if (feltTemperature > 32.0 && feltTemperature <= userColdTemp)
            {
                if (myGender == Gender.male)
                {
                    stringToReturn = $"The felt temperature is {feltTemperature}. You can wear pants, jeans, sweatpants, sweaters and a light to medium jacket. ";
                }
                else
                {
                    stringToReturn = $"The felt temperature is {feltTemperature}. You can wear jeans, leggings, sweatpants, sweaters, and a light to medium jacket. ";
                }
            }
            else if (feltTemperature > userColdTemp && feltTemperature <= userWarmTemp)
            {
                if (myGender == Gender.male)
                {
                    stringToReturn = $"The felt temperature is {feltTemperature}. You can wears pants, jeans, shorts, sweaters, crewnecks, and light jackets.";
                }
                else
                {
                    stringToReturn = $"The felt temperature is {feltTemperature}. You can wears jeans, leggings, shorts, sweaters, crewnecks, and light jackets.";
                }
            }
            else if (feltTemperature > userWarmTemp && feltTemperature <= userHotTemp)
            {
                if (myGender == Gender.male)
                {
                    stringToReturn = $"The felt temperature is {feltTemperature}. You can wear pants, jeans, shorts, tshirts and light sweaters.";
                }
                else
                {
                    stringToReturn = $"The felt temperature is {feltTemperature}. You can wear jeans, leggings, shorts, skirts, dresses, tshirts, tank tops, crop tops and light sweaters.";
                }
            }
            else if (feltTemperature > userHotTemp && feltTemperature < 95)
            {
                if (myGender == Gender.male)
                {
                    stringToReturn = $"The felt temperature is {feltTemperature}. You can wear shorts and tshirts.";
                }
                else
                {
                    stringToReturn = $"The felt temperature is {feltTemperature}. You can wear shorts, skirts, tank-tops, tshirts and dresses.";
                }
            }
            else
            {
                stringToReturn = "It is extremely hot outside. Please reconsider going outside";
            }

            return stringToReturn;

        }


        public int PrecipatationType(double snow, double rain, double precipatation, double showers)
        {

            int precipatationNumber = 0;

            if (snow > 0.0)
            {
                //precipatationType = "snow";
                precipatationNumber = 1;
            }
            else if (snow > 0.0 && rain > 0.0)
            {
                //precipatationType = "sleet";
                precipatationNumber = 2;
            }
            else if (rain > 0.0)
            {
                //precipatationType = "rain";
                precipatationNumber = 3;
            }
            else if (showers > 0.0)
            {
                //precipatationType = "showers"
                precipatationNumber = 4;
            }

            return precipatationNumber;
        }

        public string PrecipatationStatements(double precipatation, double snow, double rain, double showers)
        {
            string precipatationStatementsString = string.Empty;

            switch (PrecipatationType(snow, rain, precipatation, showers))
            {
                case 0:
                    precipatationStatementsString = "There is no precipatation";
                    break;
                case 1:
                    precipatationStatementsString = "It is snowing outside. You can wear boots, gloves, a hat and a scarf";
                    break;
                case 2:
                    precipatationStatementsString = "It is sleeting outside. Wear gloves and a hat and bring an umbrella.";
                    break;
                case 3:
                    precipatationStatementsString = "It is raining outside. Bring an umbrella.";
                    break;
                case 4:
                    precipatationStatementsString = "There are showers outside. Bring an umbrella.";
                    break;
                default:
                    break;
            }

            return precipatationStatementsString;
        }

        #region PrivateMethods
        private HttpRequestMessage CreateRequestMessage(string url, HttpMethod method, object tokenHeader)
        {

            var request = new HttpRequestMessage();
            request.RequestUri = new Uri(url);
            request.Headers.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            request.Method = method;
            request.Content = new StringContent(JsonConvert.SerializeObject(tokenHeader), Encoding.UTF8, "application/json");
            return request;
        }

        private string GetMetoDataURLEndPoint()
        {
            StringBuilder metoDataEndPoint = new StringBuilder();
            metoDataEndPoint.Append(_config.MetoBaseURL);
            metoDataEndPoint.Append("latitude=");
            metoDataEndPoint.Append(_config.Latitude);
            metoDataEndPoint.Append("&longitude=");
            metoDataEndPoint.Append(_config.Longitude);
            metoDataEndPoint.Append(_config.WeatherParams);
            return metoDataEndPoint.ToString();

        }
        #endregion
    }
}
