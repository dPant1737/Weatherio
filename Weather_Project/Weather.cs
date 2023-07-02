using PublicModels.MetoDataModels;
using WeatherLibrary.MetoData.Implementation;
using WeatherLibrary.MetoData.Interfaces;

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
            var startUp = new StartUp();

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

            //MetoData oData = GetMetoData();
            IMetoData applicationMetoData = new MetoData(startUp.ApplicationConfig);
            AppMetoDataModel oData = applicationMetoData.GetMetoData();
            
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

            double feltTemperature = applicationMetoData.CalculatingFeltTemperature(currentTemperature, humidity, windSpeed);
            feltTemperature = Math.Round(feltTemperature);
            string clothingDecider = applicationMetoData.ClothingDecider(feltTemperature, clothingGenderPreference, precipatation, snow, rain, showers, coldTemp, warmTemp, hotTemp);
            Console.WriteLine($"{clothingDecider}");
            string temperatureStatements = applicationMetoData.PrecipatationStatements(precipatation, snow, rain, showers);
            Console.WriteLine(temperatureStatements);
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }

     
        

        


        

        




      





    }
}