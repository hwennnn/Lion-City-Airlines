using System;
using System.Collections.Generic;

namespace web2020apr_p01_assignment_group5.Models
{
    public class WeatherDetails
    {
        
        public class Temperature
        {
            public int low { get; set; }
            public int high { get; set; }
        }

        public class RelativeHumidity
        {
            public int low { get; set; }
            public int high { get; set; }
        }

        public class Speed
        {
            public int low { get; set; }
            public int high { get; set; }
        }

        public class Wind
        {
            public Speed speed { get; set; }
            public string direction { get; set; }
        }

        public class Forecast
        {
            public Temperature temperature { get; set; }
            public string date { get; set; }
            public string forecast { get; set; }
            public RelativeHumidity relative_humidity { get; set; }
            public Wind wind { get; set; }
            public DateTime timestamp { get; set; }
        }

        public class Item
        {
            public DateTime update_timestamp { get; set; }
            public DateTime timestamp { get; set; }
            public List<Forecast> forecasts { get; set; }
        }

        public class ApiInfo
        {
            public string status { get; set; }
        }

        public class Root
        {
            public List<Item> items { get; set; }
            public ApiInfo api_info { get; set; }
        }


    }
}
