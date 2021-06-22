using System;
using System.IO;

namespace Apollo11.Services
{
    public class FileService
    {
        private readonly string TablesDirectoryName = "Tables";
        private string TablesDirectoryPath { get; set; }

        private readonly string CandleTableName = "Candle.csv";
        private readonly string CandleTableFirstLine = "Foo;Bar"; // todo aram change 

        private readonly string RsiTableName = "Rsi.csv";
        private readonly string RsiTableFirstLine = "Fizz;Buzz"; // todo aram change 

        private readonly string DivergenceTableName = "Divergence.csv";
        private readonly string DivergenceTableFirstLine = "Lorem;Ipsum"; // todo aram change 

        public void WriteCandleToTable() 
        {
            Console.WriteLine(nameof(EnsureTables));

            File.AppendAllText(TablesDirectoryPath + "/" + CandleTableName, "Fizz;Buzz"); // todo aram change
        }

        public void WriteRsiToTable()
        {

        }

        public void WriteDivergenceToTable()
        {

        }

        public void EnsureTables()
        {
            Console.WriteLine(nameof(EnsureTables));

            EnsureTablesDirectory();
            EnsureTable(CandleTableName, CandleTableFirstLine);
            EnsureTable(RsiTableName, RsiTableFirstLine);
            EnsureTable(DivergenceTableName, DivergenceTableFirstLine);
        }

        private void EnsureTablesDirectory()
        {
            Console.WriteLine(nameof(EnsureTablesDirectory));

            var currentDirectory = Directory.GetCurrentDirectory();
            var tablesDirectoryPath = currentDirectory + "/" + TablesDirectoryName;

            var tablesDirectoryExists = Directory.Exists(tablesDirectoryPath);
            if (!tablesDirectoryExists)
            {
                Directory.CreateDirectory(tablesDirectoryPath);
            }

            TablesDirectoryPath = tablesDirectoryPath;
        }

        private void EnsureTable(string tableName, string firstLine)
        {
            Console.WriteLine($"{nameof(EnsureTable)} {nameof(tableName)}={tableName} {nameof(firstLine)}={firstLine}");

            var filePath = TablesDirectoryPath + "/" + tableName;
            var fileExists = File.Exists(TablesDirectoryPath + "/" + tableName);

            if (!fileExists)
            {
                Console.WriteLine($"{nameof(EnsureTable)} - Creating table {nameof(tableName)}={tableName}");

                StreamWriter sw = new StreamWriter(filePath);
                sw.WriteLine(firstLine);
                sw.Close();
            }
        }
    }
}
