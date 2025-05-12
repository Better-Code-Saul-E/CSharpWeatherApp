
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Xml.Linq;
using static System.Net.WebRequestMethods;

namespace APIWeatherApp
{
    class WeatherService
    {
        private string APIKey = "848b578a44d55a95bee0c884bcb85d35";
        private string CurrentWeatherAPI = "http://api.openweathermap.org/data/2.5/weather";
        private string ForecastWeatherAPI = "https://api.openweathermap.org/data/2.5/forecast";
        private XDocument weatherData;

        public WeatherService(string city)
        {
            GetCurrentData(city);
        }

        public bool GetForecastData(string city)
        {
            try
            {
                string url = $"{ForecastWeatherAPI}?q={city}&appid={APIKey}&mode=xml&units=imperial";
                weatherData = XDocument.Load(url);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool GetForecastData(double latitude, double longitude)
        {
            try
            {
                string url = $"{ForecastWeatherAPI}?lat={latitude}&lon={longitude}&appid={APIKey}&mode=xml&units=imperial";
                weatherData = XDocument.Load(url);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool GetCurrentData(string city)
        {
            try
            {
                string url = $"{CurrentWeatherAPI}?q={city}&appid={APIKey}&mode=xml&units=imperial";
                weatherData = XDocument.Load(url);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool GetCurrentData(double latitude, double longitude)
        {
            try
            {
                string url = $"{CurrentWeatherAPI}?lat={latitude}&lon={longitude}&appid={APIKey}&mode=xml&units=imperial";
                weatherData = XDocument.Load(url);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public XDocument GetWeatherData() => weatherData;
    }
}
