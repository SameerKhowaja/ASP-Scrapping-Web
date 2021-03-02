using System;
using System.Net.Http;
using System.IO;
using HtmlAgilityPack;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Xml;

namespace k173613_Q2
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Enter File Name with path: ");
            String path = Console.ReadLine();
            string file_name = @path;
            //string file_name = @"D:\Summary23Feb21.html";
            try
            {
                string check = File.ReadAllText(file_name);
            }
            catch (Exception e)
            {
                Console.WriteLine("File not found...!");
                int code = 0;
                Environment.Exit(code);
            }
            string htmlFile = File.ReadAllText(file_name);

            // Console.WriteLine(htmlFile);

            var doc = new HtmlDocument();
            doc.LoadHtml(htmlFile);

            int totalCategory = doc.DocumentNode.SelectNodes("//div[@class='table-responsive']").Count;
            Console.WriteLine("Total Categories: " + totalCategory);

            String[] categories = new String[totalCategory];

            int i = 0;
            foreach (HtmlNode node in doc.DocumentNode.SelectNodes("//div[@class='table-responsive']"))
            {
                categories[i] = node.SelectSingleNode(".//h4").InnerHtml.Trim().Replace("/", "-");   // Category Heading
                i += 1;
            }

            // Generate Folders
            System.IO.Directory.CreateDirectory("AllRecords");
            DateTime currentDateTime = DateTime.Now;
            String mainFolder = "AllRecords/" + Convert.ToString(currentDateTime).Replace(":", ".");
            System.IO.Directory.CreateDirectory(mainFolder);    // main folder
            for(int j=0; j<totalCategory; j++)
            {
                System.IO.Directory.CreateDirectory(@mainFolder+"/"+categories[j]);     // sub categories folders
            }

            // Time to scrap main data
            i = 0;
            int count;
            int[] category_wise_count = new int[totalCategory];
            foreach (HtmlNode node in doc.DocumentNode.SelectNodes("//div[@class='table-responsive']"))
            {
                count = 0;
                foreach (HtmlNode subnode in node.Descendants("tr"))
                {
                    count += 1;
                }
                count -= 2;
                category_wise_count[i] = count;
                i += 1;
            }

            int totalScripts = 0;
            for(int j=0; j<category_wise_count.Length; j++)
            {
                totalScripts += category_wise_count[j];
            }

            List<String> scriptsName = new List<String>();
            List<String> currentPrize = new List<String>();
            String value;
            foreach (HtmlNode subnode in doc.DocumentNode.SelectNodes("//div[@class='table-responsive']//tr//td[1]"))
            {
                value = subnode.InnerText.Trim();
                if(value != "SCRIP")
                {
                    //Console.WriteLine(value);
                    scriptsName.Add(value);
                }
            }

            foreach (HtmlNode subnode in doc.DocumentNode.SelectNodes("//div[@class='table-responsive']//tr//td[6]"))
            {
                value = subnode.InnerText.Trim();
                if (value != "CURRENT")
                {
                    //Console.WriteLine(value);
                    currentPrize.Add(value);
                }
            }

            String[] scriptsNameArray = scriptsName.ToArray();
            String[] currentPrizeArray = currentPrize.ToArray();

            /*
            for(int j=0; j<currentPrizeArray.Length; j++)
            {
                Console.WriteLine("{0} --- {1}", scriptsNameArray[j], currentPrizeArray[j]);
            }
            */

            //Console.WriteLine(totalScripts);
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;

            int kloop = 0;
            String categoryPath = mainFolder;
            for(int j=0; j<totalCategory; j++)
            {
                String CategoryName = categories[j];
                Console.WriteLine(CategoryName);
                for(int k=0; k<category_wise_count[j]; k++)
                {
                    XmlWriter writer = XmlWriter.Create(categoryPath + "/" + CategoryName + "/" + scriptsNameArray[kloop] + ".xml", settings);
                    writer.WriteStartDocument();

                    writer.WriteStartElement("xml");
                    writer.WriteStartElement("Scripts");
                    writer.WriteElementString("Script", scriptsNameArray[kloop]);
                    writer.WriteElementString("Price", currentPrizeArray[kloop]);
                    writer.WriteEndElement();
                    writer.WriteEndElement();
                    writer.WriteEndDocument();
                    writer.Flush();
                    writer.Close();

                    Console.WriteLine("{0} --- {1}", scriptsNameArray[kloop], currentPrizeArray[kloop]);
                    kloop++;
                }
            }

            Console.WriteLine("\nXML File is been Generated to Folder: k173613_Q2/bin/Debug/netcoreapp3.1/" + mainFolder);

            Console.ReadLine();
        }
    }
}
