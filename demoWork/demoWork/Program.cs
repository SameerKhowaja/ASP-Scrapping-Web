using System;
using System.Collections;
using System.IO;
using System.Linq;
using System.Xml;

namespace demoWork
{
    class Program
    {
        static void Main(string[] args)
        {

            string result = Path.GetRandomFileName();
            Console.WriteLine("Random file name is " + result);
        }
    }
}
