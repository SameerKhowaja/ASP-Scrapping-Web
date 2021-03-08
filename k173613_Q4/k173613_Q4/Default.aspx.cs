using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

namespace WebApplication2
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        [System.Web.Services.WebMethod]
        public static string TestFunction(string Username)
        {
            return "Hello " + Username;
        }

        [System.Web.Services.WebMethod]
        public static string[] LoadCategory()
        {
            string dir_path = @"D:\Projects\ASP Assignments\ASP-Scrapping-Web\k173613_Q2\k173613_Q2\bin\Debug\netcoreapp3.1\All-PSX-Records\";
            // Get Lastly Created/Modified Folder Path
            var directory = new DirectoryInfo(dir_path);
            var myFolder = (from f in directory.GetDirectories()
                            orderby f.LastWriteTime descending
                            select f).First();
            String myFolder_str = Convert.ToString(myFolder);

            // Get All category names from myFolder
            var allFolders = new DirectoryInfo(dir_path + myFolder_str).GetDirectories().Select(x => x.Name).ToArray();

            return allFolders;
        }

        [System.Web.Services.WebMethod]
        public static Tuple<string[], string[], string[]> GetAllData()
        {
            string dir_path = @"D:\Projects\ASP Assignments\ASP-Scrapping-Web\k173613_Q2\k173613_Q2\bin\Debug\netcoreapp3.1\All-PSX-Records\";
            // Get Lastly Created/Modified Folder Path
            var directory = new DirectoryInfo(dir_path);
            var myFolder = (from f in directory.GetDirectories()
                            orderby f.LastWriteTime descending
                            select f).First();
            String myFolder_str = Convert.ToString(myFolder);
            var allFolders = new DirectoryInfo(dir_path + myFolder_str).GetDirectories().Select(x => x.Name).ToArray();

            XmlDocument doc = new XmlDocument();

            var CategoryList = new List<string>();
            var ScriptList = new List<string>();
            var PriceList = new List<string>();

            foreach (String folder_name in allFolders)
            {
                // Get file information
                string[] oFiles = Directory.GetFiles(dir_path + myFolder_str + @"\" + folder_name, "*.xml");
                foreach (String file_name in oFiles)
                {
                    doc.Load(file_name);

                    // Category
                    CategoryList.Add(folder_name);
                    // Name
                    XmlNodeList node1 = doc.GetElementsByTagName("Script");
                    String scriptName = node1[0].InnerText;
                    ScriptList.Add(scriptName);
                    // Price
                    XmlNodeList node2 = doc.GetElementsByTagName("Price");
                    String scriptPrize = node2[0].InnerText;
                    PriceList.Add(scriptPrize);
                }
            }

            var categoryArray = CategoryList.ToArray();
            var scriptArray = ScriptList.ToArray();
            var prizeArray = PriceList.ToArray();

            return Tuple.Create(categoryArray, scriptArray, prizeArray);
        }

    }
}