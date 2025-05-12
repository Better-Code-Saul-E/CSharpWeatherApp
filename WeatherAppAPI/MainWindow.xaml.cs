using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Xml.Linq;

// Saul Enriquez
// Weather app
// 5/10/25

// credits
// 1) used google gemini [https://gemini.google.com/app] to implement "ObservableCollection<ForecastWeatherData>"
//      which is used to automatically update the ui that displays the forecast elements
// 2) got humidity icon from icon finder [https://www.iconfinder.com/icons/2682807/drop_high_humidity_percentage_precipitation_rain_weather_icon]
// 3) got wind speed icon from icon finder [https://www.iconfinder.com/search?q=wind]
// 4) got some insight on how to get xelement from stack overflow [https://stackoverflow.com/questions/52550116/using-xdocument-to-read-the-root-element-from-xml-using-c-sharp-is-not-showing-t]


namespace APIWeatherApp
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private string searchPlaceHolder = "Enter a City";
        private bool locationOn = false;
        private const string DefaultCity = "chicago";

        private WeatherService weatherService;
        private CurrentWeatherData currentWeather;
        public CurrentWeatherData CurrentWeather
        {
            get => currentWeather;
            set { currentWeather = value; OnPropertyChanged(nameof(CurrentWeather)); }
        }
        private ObservableCollection<ForecastWeatherData> forecasts;
        public ObservableCollection<ForecastWeatherData> Forecasts
        {
            get => forecasts;
            set { forecasts = value; OnPropertyChanged(nameof(Forecasts)); }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }



        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;
        }
        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            weatherService = new WeatherService(null);
            CurrentWeather = new CurrentWeatherData(null);
            Forecasts = new ObservableCollection<ForecastWeatherData>();
            LoadWeatherForCurrentLocation();
        }



        private void LoadWeatherData(string city = null, double? lat = null, double? lon = null)
        {
            try
            {
                if (weatherService.GetCurrentData(city) || weatherService.GetCurrentData(lat.Value, lon.Value))
                {
                    CurrentWeather.ParseData(weatherService.GetWeatherData().Root);
                }

                if (weatherService.GetForecastData(city) || weatherService.GetForecastData(lat.Value, lon.Value))
                {
                    Forecasts.Clear();

                    var forecastElements = weatherService.GetWeatherData().Descendants("time");

                    foreach (var timeElement in forecastElements)
                    {
                        var forecastItem = new ForecastWeatherData();
                        forecastItem.ParseData(timeElement);
                        Forecasts.Add(forecastItem);
                    }
                }
            }
            catch { }
        }



        private void LocationButton_Click(object sender, RoutedEventArgs e)
        {
            LoadWeatherForCurrentLocation();
        }

        private void LoadWeatherForCurrentLocation()
        {
            locationOn = !locationOn;
            string locationIcon = locationOn ? "/images/LocationOn.png" : "/images/LocationOff.png";


            if (locationOn)
            {
                (double lat, double lon) coordinates = LocationService.GetCordinates();
                var city = LocationService.GetCity();

                if (coordinates.lat != 0 || coordinates.lon != 0)
                {
                    LoadWeatherData(lat: coordinates.lat, lon: coordinates.lon);
                } else if ( city != "Unknown")
                {
                    LoadWeatherData(city);
                }
            }

            LocationButtonImage.Source = new BitmapImage(new Uri(locationIcon, UriKind.Relative));
        }




        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            string city = SearchBox.Text;
            if (!string.IsNullOrWhiteSpace(city) && city != searchPlaceHolder)
            {
                LoadWeatherData(city: city);
            }
        }
        private void SearchBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (SearchBox.Text == "Enter a City")
            {
                SearchBox.Text = "";
            }
        }
        private void SearchBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(SearchBox.Text))
            {
                SearchBox.Text = searchPlaceHolder;
            }
        }
    }
}