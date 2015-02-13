using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;

namespace GoogleImageDetector
{
    public static class Detector
    {
        private static string URL = "https://www.google.se/searchbyimage/upload";
        private static string UA = "Mozilla/5.0 (Windows NT 6.3; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/40.0.2214.111 Safari/537.36";
        private static byte[] transformToJpg(string filename, out int width, out int height)
        {
            var bmp = Bitmap.FromFile(filename);

            var ms = new MemoryStream();

            bmp.Save(ms, ImageFormat.Jpeg);

            width = bmp.Width;
            height = bmp.Height;

            return ms.ToArray();
        }

        private static HttpWebResponse upload(byte[] data, int width, int height)
        {
            var collection = new Dictionary<string, object>
            {
                { "encoded_image", new FormUpload.FileParameter(data) },
                { "image_url", "" },
                { "sbisrc", "Google Chrome 40.0.2214.111 (Official) Windows m" },
                { "original_width", width.ToString() },
                { "original_height", height.ToString() }
            };

            return FormUpload.MultipartFormDataPost(URL, UA, collection);
        }

        private static void addOrIncrease(Dictionary<string, int> dictionary, string name)
        {
            name = name.ToLower();

            int value;
            dictionary[name] = dictionary.TryGetValue(name, out value) ? ++value : 1;
        }

        private static Dictionary<string, int> extract(HttpWebResponse resp)
        {
            var doc = new HtmlDocument();
            doc.Load(resp.GetResponseStream());

            var nav = doc.CreateNavigator();

            var items = nav.Select("//li/div/h3");

            var counter = new Dictionary<string, int>();

            foreach (HtmlNodeNavigator item in items)
            {
                var value = (string)item.TypedValue;

                foreach (var v in value.Split(' ').Where(ex => ex != "-" && ex != "_" && ex != "|" && ex != "–"))
                {
                    addOrIncrease(counter, v.Replace(",", ""));
                }
            }

            return counter;
        }

        public static Dictionary<string, int> Detect(string filename)
        {
            int width, height;
            var data = transformToJpg(filename, out width, out height);

            var response = upload(data, width, height);

            var couter = extract(response);

            return couter;
        }
    }
}
