namespace ApplicationSettings
{
    public class ApplicationConfig: IApplicationConfig
    {
        public string MetoBaseURL { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string WeatherParams { get; set; }
    }
}