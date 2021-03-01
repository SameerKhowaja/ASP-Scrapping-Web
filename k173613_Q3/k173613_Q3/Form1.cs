using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace k173613_Q3
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Contain category names from folder
            var allFolders = new DirectoryInfo(@"D:\Projects\ASP Assignments\ASP-Scrapping-Web\k173613_Q2\k173613_Q2\bin\Debug\netcoreapp3.1\AllRecords\01-Mar-21 10-21-27 PM").GetDirectories()
                .Select(x => x.Name)
                .ToArray();

            foreach (String folder_name in allFolders)
            {
                // Adding elements in the combobox
                comboBox1.Items.Add(folder_name);
            }

            // Column Names
            dataGridView1.Columns.Add("CategoryName", "Category");
            dataGridView1.Columns.Add("ScripName", "Scrip");
            dataGridView1.Columns.Add("ScripPrize", "Prize");

            XmlDocument doc = new XmlDocument();
            foreach (String folder_name in allFolders)
            {
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
                    //Console.WriteLine(scriptName + scriptPrize);

                    // Add Rows to Data Grid
                    dataGridView1.Rows.Add(folder_name, scriptName, scriptPrize);
                }
            }

            //Data Grid Count
            int rowsCount = dataGridView1.Rows.Count;
            label3.Text = "About " + Convert.ToString(rowsCount) + " results";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            String FilterCategoryName = comboBox1.Text; // Selected Category
            BindingSource bs = new BindingSource();
            bs.DataSource = dataGridView1.DataSource;
            bs.Filter = "Category like '%CHEMICAL%'";
            dataGridView1.DataSource = bs;

            //Data Grid Count
            int rowsCount = dataGridView1.Rows.Count;
            label3.Text = "About " + Convert.ToString(rowsCount) + " results";
        }
    }
}
