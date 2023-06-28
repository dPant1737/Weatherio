using System;
using System.Linq.Expressions;
using System.Text;
using Newtonsoft.Json;

namespace Weather_Project
{
    class Weather
    {

        enum Gender
        {
            male,
            female
        }
        static void Main(string[] args)
        {
            string userName;
            string clothingGenderPreference;
            Console.WriteLine("What is your name?");
            userName = Console.ReadLine();
            Console.WriteLine($"Welcome {userName} to Weather.io! This is an application to meet all of your clothing needs based on the weather.");
            Console.WriteLine("First, we have some questions for you before we can suggest an outfit for you.");
            Console.WriteLine("What is your clothing gender preference? (male or female)");
            clothingGenderPreference = Console.ReadLine();
            clothingGenderPreference.ToLower();

            Console.WriteLine("Here are the conditions you need when entering your values for temperature:");
            Console.WriteLine("     1. The hot temperature must be above 65 degrees Fahrenheit and below 90 degrees Fahrenheit.");
            Console.WriteLine("     2. The cold temperature must be above 32 degrees Fahrenheit and below 50 degrees Fahrenheit.");
            Console.WriteLine("     3. The warm temperature must be between the cold and hot temperatures. It cannot exceed the hot temperature or go below the cold temperature.");
            Console.WriteLine("Those are all the conditions. Let's begin:");

            MetoData oData = GetMetoData();
            
            double currentTemperature = oData.Temperature;
            int humidity = oData.Humidity;
            double windSpeed = oData.WindSpeed;
            double precipatation = oData.Precipitation;
            double snow = oData.Snow;
            double rain = oData.Rain;
            double showers = oData.Showers;


            int hotTemp = 0;
            int coldTemp = 0;
            int warmTemp = 0;

            while (((warmTemp > hotTemp) || (hotTemp > 90) || (hotTemp < 70)) || ((coldTemp > warmTemp) || (coldTemp < 32) || (coldTemp > 50)))
            {
               
                string userHotTemperature, userColdTemperature, userWarmTemperature;
                //Collecting user temperature preferences
                Console.WriteLine("What temperature do you consider hot in degrees Fahrenheit?");
                userHotTemperature = Console.ReadLine();
                Console.WriteLine("What temperature do you consider warm in degrees Fahrenheit?");
                userWarmTemperature = Console.ReadLine();
                Console.WriteLine("What temperature do you consider cold in degrees Fahrenheit?");
                userColdTemperature = Console.ReadLine();


                //Converting user temperature data from string to int

                bool hotResult = Int32.TryParse(userHotTemperature, out hotTemp);
                bool coldResult = Int32.TryParse(userColdTemperature, out coldTemp);
                bool warmResult = Int32.TryParse(userWarmTemperature, out warmTemp);

                
            }

            double feltTemperature = CalculatingFeltTemperature(currentTemperature, humidity, windSpeed);
            feltTemperature = Math.Round(feltTemperature);
            ClothingDecider(feltTemperature, clothingGenderPreference, precipatation, snow, rain, showers, coldTemp, warmTemp, hotTemp);
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }

     
        static void ClothingDecider(double feltTemperature, string clothingGenderPreference, double precipatation, double snow, double rain, double showers, int userColdTemp, int userWarmTemp, int userHotTemp)
        {
            Gender myGender;
            Enum.TryParse(clothingGenderPreference, out myGender);
            if (feltTemperature <= 0)
            {
                Console.WriteLine($"The felt temperature is {feltTemperature}. It is unsafe to go outside as it is too cold. Please consider staying inside for your safety.");
            }
            else if (feltTemperature > 0 && feltTemperature <= 32.0)
            {
                Console.WriteLine($"The felt temperature is {feltTemperature}. You can wear pants, sweatpants, heavy sweaters and a heavy winter jacket. ");
            }
            else if (feltTemperature > 32.0 && feltTemperature <= userColdTemp)
            {
                if (myGender == Gender.male)
                {
                    Console.WriteLine($"The felt temperature is {feltTemperature}. You can wear pants, jeans, sweatpants, sweaters and a light to medium jacket. ");
                }
                else
                {
                    Console.WriteLine($"The felt temperature is {feltTemperature}. You can wear jeans, leggings, sweatpants, sweaters, and a light to medium jacket. ");
                }
            }
            else if (feltTemperature > userColdTemp && feltTemperature <= userWarmTemp)
            {
                if (myGender == Gender.male)
                {
                    Console.WriteLine($"The felt temperature is {feltTemperature}. You can wears pants, jeans, shorts, sweaters, crewnecks, and light jackets.");
                }
                else
                {
                    Console.WriteLine($"The felt temperature is {feltTemperature}. You can wears jeans, leggings, shorts, sweaters, crewnecks, and light jackets.");
                }
            }
            else if (feltTemperature > userWarmTemp && feltTemperature <= userHotTemp)
            {
                if (myGender == Gender.male)
                {
                    Console.WriteLine($"The felt temperature is {feltTemperature}. You can wear pants, jeans, shorts, tshirts and light sweaters.");
                }
                else
                {
                    Console.WriteLine($"The felt temperature is {feltTemperature}. You can wear jeans, leggings, shorts, skirts, dresses, tshirts, tank tops, crop tops and light sweaters.");
                }
            }
            else if (feltTemperature > userHotTemp && feltTemperature < 95)
            {
                if (myGender == Gender.male)
                {
                    Console.WriteLine($"The felt temperature is {feltTemperature}. You can wear shorts and tshirts.");
                }
                else
                {
                    Console.WriteLine($"The felt temperature is {feltTemperature}. You can wear shorts, skirts, tank-tops, tshirts and dresses.");
                }
            }
            else
            {
                Console.WriteLine("It is extremely hot outside. Please reconsider going outside");
            }
            string temperatureStatements = PrecipatationStatements(precipatation, snow, rain, showers);
            Console.WriteLine(temperatureStatements);
        }

        //Calculating the actual temperature in degrees Fahrenheit using Wind Chill and Heat Index equations
        //Wind Chill equation is found on https://www.weather.gov/safety/cold-faqs#:~:text=Wind%20chill%20Temperature%20is%20only,F%20to%2018%C2%B0F.
        //Heat Index equation is found on https://www.weather.gov/ama/heatindex
        static double CalculatingFeltTemperature(double currentTemperature, int humidity, double windSpeed)
        {
            double feltTemperature = currentTemperature;
            if (currentTemperature <= 50.0 && windSpeed > 3.0)
            {
                feltTemperature = (35.74 + 0.6215 * currentTemperature - 35.75 * (Math.Pow(windSpeed, 0.16)) + 0.4275 * currentTemperature * (Math.Pow(windSpeed, 0.16)));

            }
            else if (currentTemperature >= 80.0 && humidity >= 40)
            {
                feltTemperature = (-42.379 + 2.04901523 * currentTemperature + 10.14333127 * humidity - 0.22475541 * currentTemperature * humidity - 0.006838 * (currentTemperature * currentTemperature)
                - 0.05482 * (humidity * humidity) + 0.001228 * (currentTemperature * currentTemperature) * humidity + 0.0008528 * currentTemperature * (humidity * humidity)
                - 0.00000199 * (currentTemperature * currentTemperature) * (humidity * humidity));

            }
            return feltTemperature;

        }


        static int PrecipatationType(double snow, double rain, double precipatation, double showers)
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
            else if(rain > 0.0) 
            {
                //precipatationType = "rain";
                precipatationNumber = 3;
            }
            else if(showers > 0.0)
            {
                //precipatationType = "showers"
                precipatationNumber = 4;
            }

            return precipatationNumber;
        }

        static string PrecipatationStatements(double precipatation, double snow, double rain, double showers)
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




        private static MetoData GetMetoData()
        {
            MetoData currentMetoData = new MetoData();

            string metoDataEndPoint = "https://api.open-meteo.com/v1/forecast?latitude=39.95&longitude=-75.16&hourly=temperature_2m,relativehumidity_2m,dewpoint_2m,apparent_temperature,precipitation_probability,precipitation,rain,showers,snowfall,snow_depth,windspeed_10m,windspeed_80m,windspeed_120m,windspeed_180m&current_weather=true&temperature_unit=fahrenheit&windspeed_unit=mph&precipitation_unit=inch&timezone=America%2FNew_York";
            try                        
            {

                HttpRequestMessage request = CreateRequestMessage(metoDataEndPoint, HttpMethod.Get, string.Empty);

                using (HttpClient httpClient = new HttpClient())
                {
                    var result = httpClient.SendAsync(request).Result;
                    var response = result.Content.ReadAsStringAsync().Result;
                    OpenMeto meto = JsonConvert.DeserializeObject<OpenMeto>(response);
                    if (meto != null)
                    {
                        int time = 0;
                        currentMetoData.Time = meto.current_weather.time;
                        string timeString = currentMetoData.Time.TrimEnd(':', '0', '0');
                        bool timeResult = Int32.TryParse(timeString.Substring(11), out time);
                        currentMetoData.Temperature = meto.hourly.temperature_2m[time];
                        currentMetoData.Humidity = meto.hourly.relativehumidity_2m[time];
                        currentMetoData.WindSpeed = meto.hourly.windspeed_10m[time];
                        currentMetoData.Precipitation = meto.hourly.precipitation[time];
                        currentMetoData.Snow = meto.hourly.snowfall[time];
                        currentMetoData.Rain = meto.hourly.rain[time];
                        currentMetoData.Showers = meto.hourly.showers[time];
                        
                    }
                }
            }
            catch (System.Exception)
            {

                throw;
            }

            return currentMetoData;
        }

        //private static async 

        private static HttpRequestMessage CreateRequestMessage(string url, HttpMethod method, object tokenHeader)
        {

            var request = new HttpRequestMessage();
            request.RequestUri = new Uri(url);
            request.Headers.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            request.Method = method;
            request.Content = new StringContent(JsonConvert.SerializeObject(tokenHeader), Encoding.UTF8, "application/json");
            return request;
        }





    }
}