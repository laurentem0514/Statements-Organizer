using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StatementsOrganizer
{
    class Program
    {
        static void Main(string[] args)
        {
            //step1 - Created variable to store folder path to cc statements 
            string statementsPath = "C:\\Users\\etoth_000\\Downloads\\CCStatements";

            //this is for step4 to name the output file
            //string outputFileName = "C:\\Users\\etoth_000\\Downloads\\CCStatementsOutput2.csv";
            string outputFileName = null;


            StatementsProcessor statementsProcessor = new StatementsProcessor(statementsPath, outputFileName);
            statementsProcessor.Process();


            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
    }
}
