using System;
using System.IO;
using Apollo11.Models;

namespace Apollo11.Services
{
    public class FileService
    {
        private readonly string TablesDirectoryName = "Tables";
        private string TablesDirectoryPath { get; set; }

        private readonly string CandleTableName = "Candle.csv";
        private readonly string CandleTableFirstLine = "Time Interval;High;Low;Open;Close"; // todo aram change placeholder

        private readonly string RsiTableName = "Rsi.csv";
        private readonly string RsiTableFirstLine = "Time Interval;Value"; // todo aram change placeholder

        private readonly string DivergenceTableName = "Divergence.csv";
        private readonly string DivergenceTableFirstLine = "Time Interval;Divergence Index;Current Price"; // todo aram change placeholder

        public void WriteCandleToTable(Candle candle) 
        {
            Console.WriteLine(nameof(WriteCandleToTable));

            var timeInterval = candle.EventTime;
            var high = candle.Kline.High;
            var low = candle.Kline.Low;
            var open = candle.Kline.Open;
            var close = candle.Kline.Close;

            File.AppendAllText(TablesDirectoryPath + "/" + CandleTableName, timeInterval + high + low + open + close + Environment.NewLine); // todo aram change
        }

        public void ReadCellFromTable(string tableName)
        {
            String line;
            tableName = CandleTableName; // todo aram for testing
            var filePath = TablesDirectoryPath + "/" + tableName;
            try
            {
                //Pass the file path and file name to the StreamReader constructor
                StreamReader sr = new StreamReader(filePath);
                //Read the first line of text
                line = sr.ReadLine();
                //Continue to read until you reach end of file
                while (line != null)
                {
                    //write the lie to console window
                    Console.WriteLine(line);
                    //Read the next line
                    line = sr.ReadLine();
                }
                //close the file
                sr.Close();
                Console.ReadLine();
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }
            finally
            {
                Console.WriteLine("Executing finally block.");
            }
        }

        public void WriteRsiToTable(string timeInterval, string rsiValue)
        {
            Console.WriteLine(nameof(WriteRsiToTable));

            File.AppendAllText(TablesDirectoryPath + "/" + RsiTableName, timeInterval + rsiValue + Environment.NewLine); // todo aram change
        }

        public void WriteDivergenceToTable(Divergence divergence)
        {
            Console.WriteLine(nameof(WriteDivergenceToTable));

            File.AppendAllText(TablesDirectoryPath + "/" + DivergenceTableName, divergence.TimeInterval +
                divergence.DivergenceIndex + divergence.CurrentPrice + Environment.NewLine); // todo aram change
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
