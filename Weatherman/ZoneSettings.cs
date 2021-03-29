﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Weatherman
{
    class ZoneSettings
    {
        public ushort ZoneId;
        public string ZoneName;
        public List<WeathermanWeather> SupportedWeathers;
        public bool WeatherControl = false;
        public int TimeFlow = 0;
        public int CustomTime = 0;

        public ZoneSettings() { }

        public void Init(Weatherman plugin)
        {
            SupportedWeathers = new List<WeathermanWeather>();
            if(plugin.GetWeathers(ZoneId) != null) foreach (var w in plugin.GetWeathers(ZoneId))
            {
                SupportedWeathers.Add(new WeathermanWeather(w, false, plugin.IsWeatherNormal(w, ZoneId)));
            }
        }

        public string GetString()
        {
            var b = new List<string>
            {
                WeatherControl.ToString(),
                TimeFlow.ToString(),
                CustomTime.ToString()
            };
            var sel = new List<string>();
            foreach (var z in SupportedWeathers)
            {
                if(z.Selected) sel.Add(z.Id.ToString());
            }
            if (WeatherControl == false && TimeFlow == 0 && CustomTime == 0 && sel.Count == 0) return null;
            b.Add(string.Join("|", sel));
            return string.Join("/", b);
        }

        public void FromString(string s)
        {
            var ss = s.Split('/');
            WeatherControl = bool.Parse(ss[0]);
            TimeFlow = int.Parse(ss[1]);
            CustomTime = int.Parse(ss[2]);
            var selectedw = ss[3].Split('|');
            foreach (var i in selectedw)
            {
                var ii = byte.Parse(i);
                foreach (var z in SupportedWeathers)
                {
                    if (z.Id == ii) z.Selected = true;
                }
            }
        }
    }
}