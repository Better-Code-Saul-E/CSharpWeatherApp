using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace APIWeatherApp
{
    public class ForecastWeatherData : WeatherBase
    {
        private DateTime forecastDate;
        public DateTime ForecastDate
        {
            get => forecastDate;
            private set
            {
                forecastDate = value;
                OnPropertyChanged(nameof(forecastDate));
                OnPropertyChanged(nameof(DaysOfTheWeek));
                OnPropertyChanged(nameof(HourOfTheDay));
            }
        }

        public string DaysOfTheWeek => ForecastDate.ToString("ddd");
        public string HourOfTheDay => ForecastDate.ToString("MMM d hh:00");


        public ForecastWeatherData() { }
        public override void ParseData(XElement forecastdata)
        {
            if (forecastdata == null) return;

            string date = forecastdata.Attribute("from")?.Value;
            DateTime.TryParse(date, out forecastDate);

            double.TryParse(forecastdata.Element("temperature")?.Attribute("value")?.Value, out temperature);

            XElement conditionList = forecastdata.Element("symbol");

            Condition = conditionList.Attribute("name")?.Value?.Trim();
            WeatherImage = LoadImageFromAPI(conditionList.Attribute("var")?.Value);

            OnPropertyChanged(nameof(TemperatureFormatted));
            OnPropertyChanged(nameof(weatherImage));
            OnPropertyChanged(nameof(Condition));
        }
    }
}
