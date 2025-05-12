using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Xml.Linq;

namespace APIWeatherApp
{
    // make constructor for initial data???
    // make method for updating the data?? or use the same one ---
    public class CurrentWeatherData : WeatherBase
    {
        private string country;
        public string Location => $"{city}, {country.ToUpper()}";


        private double windSpeed;
        public string WindSpeedFormatted => $"{(int)Math.Floor(windSpeed)} m/s";


        private double humidity;
        public string HumidityFormatted => $"{(int)Math.Floor(humidity)}%";

        public CurrentWeatherData(XElement data) 
        {
            city = "Unknown";
            country = "Unknown";
            temperature = 0; ;
            condition = "NA";
            if (data != null) 
            { 
                ParseData(data); 
            }
        }
        public override void ParseData(XElement currentData)
        {
            if (currentData == null) return;

            var cityList = currentData.Descendants("city").FirstOrDefault();
            city = cityList.Attribute("name").Value;
            country = cityList.Element("country").Value;

            double.TryParse(currentData.Descendants("temperature").FirstOrDefault().Attribute("value")?.Value, out temperature);
            double.TryParse(currentData.Descendants("humidity").FirstOrDefault().Attribute("value")?.Value, out humidity);
            double.TryParse(currentData.Descendants("speed").FirstOrDefault().Attribute("value")?.Value, out windSpeed);


            var conditionList = currentData.Descendants("weather").FirstOrDefault();

            Condition = conditionList.Attribute("value")?.Value.Trim();

            string iconCode = conditionList.Attribute("icon")?.Value;
            weatherImage = LoadImageFromAPI(iconCode);

            OnPropertyChanged(nameof(WeatherImage));
            OnPropertyChanged(nameof(Location));
            OnPropertyChanged(nameof(Condition));
            OnPropertyChanged(nameof(WindSpeedFormatted));
            OnPropertyChanged(nameof(HumidityFormatted));
            OnPropertyChanged(nameof(TemperatureFormatted));
        }

    }
}