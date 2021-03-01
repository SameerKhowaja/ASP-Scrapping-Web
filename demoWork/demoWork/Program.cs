using System;
using System.IO;
using System.Linq;
using System.Xml;

namespace demoWork
{
    class Program
    {
        static void Main(string[] args)
        {
            // Contain category names from folder
            var allFolders = new DirectoryInfo(@"D:\Projects\ASP Assignments\ASP-Scrapping-Web\k173613_Q2\k173613_Q2\bin\Debug\netcoreapp3.1\AllRecords\01-Mar-21 10-21-27 PM").GetDirectories()
                .Select(x => x.Name)
                .ToArray();


            XmlDocument doc = new XmlDocument();
            foreach (String folder_name in allFolders)
            {
                Console.WriteLine(folder_name);
                string[] oFiles = Directory.GetFiles(@"D:\Projects\ASP Assignments\ASP-Scrapping-Web\k173613_Q2\k173613_Q2\bin\Debug\netcoreapp3.1\AllRecords\01-Mar-21 10-21-27 PM\" + folder_name, "*.xml");
                foreach (String file_name in oFiles)
                {
                    doc.Load(file_name);
                    //XmlNode node = doc.DocumentElement.SelectSingleNode("/Scripts/Script");
                    XmlNodeList node1 = doc.GetElementsByTagName("Script");
                    //Console.WriteLine(nodeScript[0].InnerText);
                    XmlNodeList node2 = doc.GetElementsByTagName("Price");
                    //Console.WriteLine(nodePrize[0].InnerText);
                    String scriptName = node1[0].InnerText;
                    String scriptPrize = node2[0].InnerText;
                    Console.WriteLine(scriptName + scriptPrize);

                }
            }
        }
    }
}
