using FileHelpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StatementsOrganizer
{
    public class StatementsProcessor
    {

        private string _rootPath;
        private string _outputFilePath;

        //declared constructor
        public StatementsProcessor(string rootPath, string outputFilePath)
        {
            _rootPath = rootPath;
            _outputFilePath = outputFilePath;

        }


        public void Process()
        {

            //step1 - Created variable to store all file paths from folder of cc statements 
            string[] filePaths = Directory.GetFiles(_rootPath, "*.csv", SearchOption.AllDirectories);


            //created before proceeding step 2e
            List<Transaction> transactions = new List<Transaction>();



            //v2
            var engine = new FileHelperEngine<CsvTransaction>();

            //step2 - iterating filePaths (create the foreach part for indiv files)
            foreach (string filePath in filePaths)
            {
                //step5a - extract account owner from path
                //string owner = filePath.Replace(statementsPath, "").TrimStart('\\').Split('\\')[0];

                PathParameters pathParameters = GetPathParameters(filePath, _rootPath);


                //v2file helper is reading then parsing the file to give us an object for each line (result is collection of objects)
                var result = engine.ReadFile(filePath);

                //v2 for each csv object, create and populate transaction object
                foreach (CsvTransaction csvTransaction in result)
                {
                    //step 2d - create Transaction object from components (syntax DataType variableName = new DataType() )
                    Transaction transaction = new Transaction();

                    //populate the object's properties (fill in the blanks of form)
                    transaction.PostedDate = csvTransaction.PostedDate;
                    transaction.ReferenceNumber = csvTransaction.ReferenceNumber;
                    transaction.Payee = csvTransaction.Payee;
                    transaction.Address = csvTransaction.Address;
                    transaction.Amount = csvTransaction.Amount;

                    //step7
                    transaction.Owner = pathParameters.Owner;

                    //step 2e - add object transaction to list of Transactions
                    transactions.Add(transaction);
                }



            }

            //step6b declare variable that gets instance of dictionary (a value)
            var dictionary = GetDictionary();


            //must define SB before using in step 3b
            StringBuilder sb = new StringBuilder();

            //this will be a header
            sb.AppendLine("Posted Date,Reference Number,Payee,Address,Amount,Owner,Category");

            //step 3a - build string value for each object
            foreach (Transaction transaction in transactions)
            {
                //step6b- populate category
                PopulateCategory(transaction, dictionary);


                //turn properties of object into one string value
                string fileLine = string.Join(",", transaction.PostedDate, transaction.ReferenceNumber, "\"" + transaction.Payee + "\"", "\"" + transaction.Address + "\"", transaction.Amount, transaction.Owner, transaction.Category);

                //step 3b - use StringBuilder to combine strings to make one big chunk of text
                sb.AppendLine(fileLine);

            }
            //saved output to variable
            string outputFileContents = sb.ToString();

            //step 4 - write the file content to a new output file
            string outputFileName = GetOutputFileName();
            File.WriteAllText(outputFileName, outputFileContents);
        }

        private string GetOutputFileName()
        {
            string outputFileName = _outputFilePath;

            if (string.IsNullOrEmpty(outputFileName))
            {
                outputFileName = "OrganizedStatements\\output.csv";
            }

            return outputFileName;
        }

        private Dictionary<string, string[]> GetDictionary()
        {
            var dictionary = new Dictionary<string, string[]>
            {
                {"Shopping", new [] {"Amazon", "Target"}},
                {"Eating Out", new [] {"Chipotle", "Subway", "Mosaic", "Panera", "Dunkin", "applebees", "tropical", "sweet frog", "quiznos", "red mango", "bagel"}},
                {"Gas", new [] {"Hess", "Sunoco"}},
                {"Groceries", new [] {"Stop & Shop", "Angora", "Pathmark", "Lombardis"}},
                {
                    "Travel", new []
                    {
                        "LIRR",
				        // JFK Airport
				        "kennedy",
				        //subway tickets nyc
				        "MTA",
                        "orbitz",
                        "car rental",
                        "travel insurance",
                        "delta",
                        "tolls",
                    }
                },
                {"Payments", new [] {"payment", "kamburov"}},

            };

            return dictionary;
        }

        private void PopulateCategory(Transaction transaction, Dictionary<string, string[]> dictionary)
        {
            foreach (var entry in dictionary)
            {
                foreach (string term in entry.Value)
                {
                    if (transaction.Payee.IndexOf(term, StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        transaction.Category = entry.Key;
                        break;
                    }
                }

                if (!string.IsNullOrEmpty(transaction.Category))
                {
                    break;
                }
            }
        }

        private PathParameters GetPathParameters(string filePath, string rootPath)
        {
            PathParameters pathParameters = new PathParameters();

            //populate properties
            string[] folderNames = filePath.Replace(rootPath, string.Empty).TrimStart(Path.DirectorySeparatorChar).Split(Path.DirectorySeparatorChar);
            string owner = folderNames[0];
            string account = folderNames[1];

            pathParameters.Owner = owner;
            pathParameters.Account = account;

            return pathParameters;
        }
    }
}
