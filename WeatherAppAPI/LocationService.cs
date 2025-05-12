using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace APIWeatherApp
{
    static class LocationService
    {
        public static (double latitude, double longitude) GetCordinates()
        {
            try
            {
                XDocument xdoc = XDocument.Load("http://ip-api.com/xml/?fields=lon,lat");
                var latElement = xdoc.Descendants("lat").FirstOrDefault();
                var lonElement = xdoc.Descendants("lon").FirstOrDefault();

                double latitude = double.Parse(latElement?.Value ?? "0");
                double longitude = double.Parse(lonElement?.Value ?? "0");
                return (latitude, longitude);
            }
            catch
            {
                return (0, 0);
            }
        }
        public static string GetCity()
        {
            try
            {
                XDocument xdoc = XDocument.Load("http://ip-api.com/xml/");
                var cityElement = xdoc.Descendants("city").FirstOrDefault();
                return cityElement?.Value ?? "Unknown";
            }
            catch
            {
                return "Unknown";
            }
        }
    }
}
