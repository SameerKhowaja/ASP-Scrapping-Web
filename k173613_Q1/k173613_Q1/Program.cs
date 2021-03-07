using System;
using System.Net.Http;
using System.IO;

namespace k173613_Q1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Enter Website URL: ");
            String url = Console.ReadLine();
            Console.Write("Enter Path Where to Save File: ");
            String path = Console.ReadLine();

            // var url = "https://www.psx.com.pk/market-summary/";
            var httpClient = new HttpClient();
            var html = httpClient.GetStringAsync(url);
            Console.WriteLine(html.Result);

            int day = DateTime.Today.Day;
            int month = DateTime.Today.Month;
            int year = DateTime.Today.Year;

            DateTime date = new DateTime(year, month, day);
            string file_name = @path+"/Summary" + date.ToString("dd") + date.ToString("MMM") + date.ToString("yy") + ".html";

            string content = html.Result;
            File.WriteAllText(file_name, content);
            Console.WriteLine("File Saved to : " + file_name);

            Console.ReadLine();
        }
    }
}
