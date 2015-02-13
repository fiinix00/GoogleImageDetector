using System;
using System.Linq;

namespace GoogleImageDetector
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var file = "American_Eskimo_Dog.jpg";

            var result = Detector.Detect(file);

            Console.WriteLine(string.Join(Environment.NewLine, result));
        }
    }
}
