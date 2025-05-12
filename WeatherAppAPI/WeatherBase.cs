using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Xml.Linq;

namespace APIWeatherApp
{
    public abstract class WeatherBase : INotifyPropertyChanged
    {
        protected string city;
        protected string condition;
        public string Condition {
            get => condition;
            set {
                condition = value;
                OnPropertyChanged(nameof(Condition));
            }
        }

        protected double temperature;
        public string TemperatureFormatted => $"{Math.Floor(temperature)}°";

        protected BitmapImage weatherImage;
        public BitmapImage WeatherImage
        {
            get => weatherImage;
            set {
                weatherImage = value;
                OnPropertyChanged(nameof(WeatherImage));
            }
        }

        protected BitmapImage LoadImageFromAPI(string iconCode)
        {
            return new BitmapImage(new Uri($"http://openweathermap.org/img/wn/{iconCode}@2x.png", UriKind.Absolute));
        }

        public abstract void ParseData(XElement data);

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
