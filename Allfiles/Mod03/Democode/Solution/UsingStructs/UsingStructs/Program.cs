using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UsingStructs
{

    struct Coffee
    {
        public string Name { get; set; }
        public string Bean { get; set; }
        public string CountyOfOrigin { get; set; }
        public int Strength { get; set; }

        public char? this[int index]
        {
            get
            {
                if (index > 0 && index < Name.Length)
                    return Name[index];
                else
                    return null;
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Coffee coffee1 = new Coffee();
            coffee1.Name = "Fourth Coffee Quencher";
            coffee1.CountyOfOrigin = "Indonesia";
            coffee1.Strength = 3;
            Console.WriteLine("Name: {0}", coffee1.Name);
            Console.WriteLine("Country of Origin: {0}", coffee1.CountyOfOrigin);
            Console.WriteLine("Strength: {0}", coffee1.Strength);
            Console.WriteLine("10th Letter: {0}", coffee1[10]);
            Console.WriteLine("30th Letter: {0}", coffee1[30]);

        }
    }
}
