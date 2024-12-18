using Core.Common.Logging;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Jobs.OldScheduler.Jobs
{
    public class DatabaseBackupJob : IJob
    {
        private readonly ILogger _log;
        private readonly string _connectionString;
        private readonly string _backupPath;
        private readonly string _databaseName;
        private readonly int _retentionDays = 5;

        private const string DbConnectionStringKey = "PI_DbConnectionString";
        private const string BackupPathKey = "DatabaseBackupJob:backupPath";
        private const string DatabaseName = "DatabaseBackupJob:databaseName";
        private const string RetentionDays = "DatabaseBackupJob:retentionDays";

        public DatabaseBackupJob(
            IConfigurationRoot configuration,
            ILogger log)
        {
            _log = log;
            _connectionString = configuration.GetSection(DbConnectionStringKey).Value;
            _backupPath = configuration.GetSection(BackupPathKey).Value;
            _databaseName = configuration.GetSection(DatabaseName).Value;
            _retentionDays = Convert.ToInt32(configuration.GetSection(RetentionDays).Value);
        }

        public string JobName => nameof(DatabaseBackupJob);

        public void ImmediateRun()
        {
            _log.Info("Performing backup immediatelly.");
            PerformBackup();
        }

        public void ImmediateRun(IEnumerable<string> parameters)
        {
            _log.Info("Performing backup immediatelly.");
            PerformBackup();
        }

        public void Run()
        {
            PerformBackup();
        }

        private void PerformBackup()
        {
            try
            {
                // Create backup filename with timestamp
                string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                string backupFileName = $"{_databaseName}_{timestamp}.bak";
                string fullBackupPath = Path.Combine(_backupPath, backupFileName);

                // Ensure backup directory exists
                Directory.CreateDirectory(_backupPath);

                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    // Create backup command
                    string backupQuery = $@"
                    BACKUP DATABASE [{_databaseName}] 
                    TO DISK = @BackupPath
                    WITH FORMAT, 
                         COMPRESSION, 
                         STATS = 10,
                         NAME = @BackupName";

                    using (SqlCommand command = new SqlCommand(backupQuery, connection))
                    {
                        command.Parameters.AddWithValue("@BackupPath", fullBackupPath);
                        command.Parameters.AddWithValue("@BackupName", $"{_databaseName} Backup {timestamp}");

                        command.ExecuteNonQuery();
                    }

                    _log.Info($"Backup completed successfully: {fullBackupPath}");
                }

                // Clean up old backups
                CleanupOldBackups();
            }
            catch (Exception ex)
            {
                string errorMessage = $"Backup failed: {ex.Message}";
                _log.Error(errorMessage);
            }
        }

        private void CleanupOldBackups()
        {
            try
            {
                var files = Directory.GetFiles(_backupPath, $"{_databaseName}_*.bak");
                var cutoffDate = DateTime.Now.AddDays(-_retentionDays);
                var filesToDelete = files.Select(path => new FileInfo(path)).OrderByDescending(f => f.CreationTime).Skip(5);

                foreach (var file in filesToDelete)
                {
                    if (file.CreationTime < cutoffDate)
                    {
                        File.Delete(file.FullName);
                        _log.Info($"Deleted old backup: {file}");
                    }
                }
            }
            catch (Exception ex)
            {
                _log.Error($"Cleanup error: {ex.Message}");
            }
        }
    }
}
