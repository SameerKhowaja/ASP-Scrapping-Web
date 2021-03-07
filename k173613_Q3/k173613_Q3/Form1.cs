using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Configuration;

namespace k173613_Q3
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        // Timer
        Stopwatch stopWatch = new Stopwatch();
        // DataTable
        DataTable dt = new DataTable();
        BindingSource bs = new BindingSource();

        //Path
        string dir_path = ConfigurationManager.AppSettings.Get("DirPath");

        // Method will load all data to form
        void LoadData()
        {
            // flush all data on first use
            dt.Clear();
            comboBox1.Items.Clear();

            // Get Lastly Created/Modified Folder Path
            var directory = new DirectoryInfo(dir_path);
            var myFolder = (from f in directory.GetDirectories()
                            orderby f.LastWriteTime descending
                            select f).First();
            String myFolder_str = Convert.ToString(myFolder);

            //set label text
            label4.Text = "Folder Name: " + myFolder_str;

            // Get All category names from myFolder
            var allFolders = new DirectoryInfo(dir_path + myFolder_str).GetDirectories()
                .Select(x => x.Name)
                .ToArray();

            XmlDocument doc = new XmlDocument();
            foreach (String folder_name in allFolders)
            {
                // Adding elements in the combobox
                comboBox1.Items.Add(folder_name);

                // Get file information
                string[] oFiles = Directory.GetFiles(dir_path + myFolder_str + @"\" + folder_name, "*.xml");
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
                    dt.Rows.Add(new object[] { folder_name, scriptName, scriptPrize });
                }
            }

            bs.DataSource = dt;
            dataGridView1.DataSource = bs;

            //Data Grid Count
            int rowsCount = dataGridView1.Rows.Count;
            label3.Text = "About " + Convert.ToString(rowsCount) + " results";
        }

        void GetProcessingTime()
        {
            // Get the elapsed time as a TimeSpan value.
            TimeSpan ts = stopWatch.Elapsed;
            // Format and display the TimeSpan value. 
            string elapsedTime = String.Format("{2:00}.{3:00}",
                ts.Hours, ts.Minutes, ts.Seconds,
                ts.Milliseconds / 10);
            label3.Text += " ( " + elapsedTime + " Seconds )";

            stopWatch.Reset();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Column Names add to datatable
            dt.Columns.Add("Category", typeof(String));
            dt.Columns.Add("Scrip", typeof(String));
            dt.Columns.Add("Prize", typeof(String));

            stopWatch.Start();
            LoadData();
            stopWatch.Stop();

            GetProcessingTime();
        }

        // Method for Filtering Data Grid
        void filterDataGrid(String FilterCategoryName = "%")
        {
            bs.DataSource = dt;
            bs.Filter = "Category like '" + FilterCategoryName + "'";
            dataGridView1.DataSource = bs;

            //Data Grid Count
            int rowsCount = dataGridView1.Rows.Count;
            label3.Text = "About " + Convert.ToString(rowsCount) + " results";
        }

        // Search Button
        private void button1_Click(object sender, EventArgs e)
        {
            stopWatch.Start();
            String FilterCategoryName = comboBox1.Text; // Selected Category
            filterDataGrid(FilterCategoryName);
            stopWatch.Stop();

            GetProcessingTime();
        }

        // Refresh Button
        private void button2_Click(object sender, EventArgs e)
        {
            stopWatch.Start();
            comboBox1.Text = "";
            LoadData();
            filterDataGrid();
            stopWatch.Stop();

            GetProcessingTime();
        }
    }
}
