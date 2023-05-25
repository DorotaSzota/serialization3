using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace serialization3
{
    public enum CloudCover
    { overcast, moderate, partly, none }
    public enum WindDirection
    { north, south, east, west }
    
    internal class Weather: ICSVserializationDeserialization<Weather>
    {
        //fields
        private static Random random = new Random();
        private DateTime measurementTime;
        private double temperature;
        private double windSpeed;
        private int humidity;

        //properties
        public DateTime MeasurementTime { get { return measurementTime; } set { measurementTime = value; } }
        public CloudCover RandomCloudCover { get; set; }
        public double Temperature { get { return temperature; } set { temperature = random.Next(-45, 45); } }
        public WindDirection RandomWindDirection { get; set; }
        public double WindSpeed { get { return windSpeed; } set { windSpeed = random.Next(121); } }
        public int Humidity { get { return humidity; } set { humidity = random.Next(121); } }
        
        //constructors
        public Weather() { }
        public Weather( CloudCover cc, double temperature, WindDirection wd, double ws, int hum)
        {
            MeasurementTime = RandomDate();
            RandomCloudCover = GenerateRandomCloudCover();
            Temperature = temperature;
            RandomWindDirection = GenerateRandomWindDirection();
            WindSpeed = ws;
            Humidity = hum ;
        }
        //a method that generates a random wind direction 
        public WindDirection GenerateRandomWindDirection()
        {
            Array windDirectionValues = Enum.GetValues(typeof(WindDirection));
            int randomIndex = random.Next(4);
            return (WindDirection)windDirectionValues.GetValue(randomIndex);
        }
        //a method that generates a random cloud cover
        public CloudCover GenerateRandomCloudCover()
        {
            Array cloudCoverValues = Enum.GetValues(typeof(CloudCover));
            int randomIndex = random.Next(4);
            return (CloudCover)cloudCoverValues.GetValue(randomIndex);
        }
        //a method that generates a random date within a spcified time frame
        public DateTime RandomDate()
        {
            Random random = new Random();

            int year = random.Next(2000, 2023);
            int month = random.Next(1, 13);
            int day = random.Next(1, DateTime.DaysInMonth(year, month)+1);
            int hour = random.Next(0, 24);
            int minute = random.Next(0, 60);
            int second = random.Next(0, 60);

            DateTime randomDateTime = new DateTime(year, month, day, hour, minute, second,DateTimeKind.Utc);
            return randomDateTime;
        }

        public override string ToString()
        {
            return $"{this.measurementTime};{RandomCloudCover};{Temperature};{RandomWindDirection};{WindSpeed};{Humidity};";
        }

        //serialization method
        public void SerilizeToCSV(List<Weather> list, string filepath)
        {
            using (StreamWriter sw = new StreamWriter(filepath)) 
            { 
                sw.WriteLine("Time;Cloud Cover;Temperature;Wind Direction;Wind Speed;Humidity"); //adding a header
                foreach (Weather w in list)
                {
                    sw.WriteLine(w.ToString());
                }
            }
        }
        //deserialization method
        public  List<Weather> DeserializeCSV(string filepath)
        {
            List<Weather> weatherFromCSV = new List<Weather>();

            using (StreamReader sr = new StreamReader(filepath))
            {
                sr.ReadLine(); // getting rid of headers

                while (!sr.EndOfStream) 
                { 
                    var line = sr.ReadLine(); 
                    var values = line.Split(';');

                    var weather = new Weather
                    {
                        measurementTime = DateTime.Parse(values[0]),
                        RandomCloudCover = (Enum.TryParse<CloudCover>(values[1], out CloudCover cloudCover)) ? cloudCover : CloudCover.none,
                        Temperature = double.Parse(values[2]),
                        RandomWindDirection = Enum.TryParse<WindDirection>(values[3], out WindDirection windDirection) ? windDirection : GenerateRandomWindDirection(),
                        WindSpeed = double.Parse(values[4]),
                        Humidity = int.Parse(values[5])
                        };   

                    weatherFromCSV.Add(weather);
                }
            }
                return weatherFromCSV;     
        }

    }

}
