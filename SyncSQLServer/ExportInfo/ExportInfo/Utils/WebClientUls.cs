using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


namespace KVDDDCore.Utils
{
    public class WebClientUls
    {
        private static JObject model;

        public static JObject GetString(string url)
        {
            var json = "";
            using (WebClient wc = new WebClient())
            {
                wc.Encoding = System.Text.Encoding.UTF8;

                try
                {
                    json = wc.DownloadString(url);
                }
                catch (Exception)
                {
                    json = null;
                }
                if (!string.IsNullOrEmpty(json))
                    model = JObject.Parse(json.ToString());
                else
                    model = null;
            }
            return model;
        }
    }
}