using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileSystemGlobbing;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlphaOneA
{
    public class FileWatcherService
    {

        private readonly ILogger<FileWatcherService> _logger;
       // private readonly string _filePath;
        private readonly string _fileName;
        private readonly SqliteDatabaseService _databaseService;


        public FileWatcherService(ILogger<FileWatcherService> logger, IConfiguration configuration, SqliteDatabaseService databaseService)
        {
            _logger = logger;
            //_filePath = configuration["FilePath"];
            _fileName = configuration["FileName"];
            _databaseService = databaseService;
            Console.WriteLine("Inside FSW");
        }

        public void Start()
        {
            _logger.LogInformation("FileWatcherService started.");
            var watcher = new FileSystemWatcher(@"C:\Temp")
            {
                Filter = "*.csv",
                NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName,
                EnableRaisingEvents = true
            };

            watcher.Created += OnFileCreated;
            //Preventing watcher to get collected by GC
            GC.KeepAlive(watcher);

        }
            private void OnFileCreated(object sender, FileSystemEventArgs e)
            {

                _logger.LogInformation($"New file detected: {e.FullPath}");

                _logger.LogInformation($"Deleting old data from database");
                _databaseService.DeleteOldData();

                _logger.LogInformation($"Storing new data from file to database");
                _databaseService.StoreData(e.FullPath);

                _logger.LogInformation($"File content stored in the database.");

                string result = _fileName + Stopwatch.GetTimestamp();

                result = result + ".csv";

                var targetFile = "C:\\Temp\\archive\\" + result;

                File.Move(e.FullPath, targetFile);

                _logger.LogInformation($"Made new file at archive");


            }


           
       
    }

    
}

