using System;

namespace serialization3
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string path = "WeatherMeasurements.csv";
            List<Weather> weathers = new List<Weather>();
           
            for (int i = 0; i <= 100; i++)
            {
             weathers.Add(new Weather(CloudCover.overcast, 13, WindDirection.east,15,60));
            }

            List<Weather> sortedWeather = weathers.OrderByDescending(w=>w.MeasurementTime).ToList();
            Console.WriteLine("Data to serialize:");
            foreach (Weather weather in sortedWeather) { Console.WriteLine(weather.ToString()); }

            Weather weatherData = new Weather();
            //serialization
           weatherData.SerilizeToCSV(sortedWeather, "WeatherMeasurements.csv");

            //deserialization
            List<Weather> importedWeatherData = weatherData.DeserializeCSV("WeatherMeasurements.csv");
            //displaying the deserialized data
            Console.WriteLine("Deserialized data from a .csv file:");
            foreach (Weather weather in importedWeatherData) { Console.WriteLine(weather.ToString()); }

            Console.ReadKey();    

        }
    }
}