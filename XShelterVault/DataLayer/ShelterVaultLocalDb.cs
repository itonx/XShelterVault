using Dapper;
using Microsoft.Data.Sqlite;

namespace ShelterVault.DataLayer
{
    public interface IShelterVaultDBConfiguration
    {
        string DefaultShelterVaultPath { get; }
        string UserPath { get; }
        string DbExtension { get; }
        string DbName { get; }
        string DbPath { get; }
        string DbConnectionString { get; }
        void SetDbName(string dbName);
    }

    public interface ICommonDbOperation
    {
        bool ExecuteQueries(IEnumerable<KeyValuePair<string, object>> queries);
        IEnumerable<T> QueryAcrossDatabases<T>(IEnumerable<string> fileNames, string query);
        T QueryFirst<T>(string query, object param = null);
        T QueryFirstOrDefault<T>(string query, object param = null);
        IEnumerable<T> Query<T>(string query, object param = null);
        int Execute(string query, object param = null);
        object ExecuteScalar(string query, object param = null);
    }

    public interface IShelterVaultLocalDb : IShelterVaultDBConfiguration, ICommonDbOperation
    {
    }

    public class ShelterVaultLocalDb : IShelterVaultLocalDb
    {
        private readonly ILogger<ShelterVaultLocalDb> _logger;

        public string DefaultShelterVaultPath => Path.Combine(UserPath, ".sheltervault");
        public string UserPath => Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        public string DbExtension => ".db";
        public string DbName { get; private set; }
        public string TmpDbName { get; private set; }
        public string DbPath => Path.Combine(DefaultShelterVaultPath, string.Concat(DbName, DbExtension));
        public string DbConnectionString => $"Data Source={DbPath};Pooling=True;";
        public string TmpDbPath => Path.Combine(DefaultShelterVaultPath, string.Concat(TmpDbName, DbExtension));
        public string TmpDbConnectionString => $"Data Source={TmpDbPath};Pooling=True;";

        public ShelterVaultLocalDb(ILogger<ShelterVaultLocalDb> logger)
        {
            _logger = logger;
        }

        public void SetDbName(string dbName)
        {
            DbName = dbName;
        }

        public bool ExecuteQueries(IEnumerable<KeyValuePair<string, object>> queries)
        {
            try
            {
                using SqliteConnection connection = GetOpenSqliteConnection();
                foreach (var query in queries)
                {
                    connection.Execute(query.Key, param: query.Value);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to execute queries.");
                return false;
            }

            return true;
        }

        public IEnumerable<T> QueryAcrossDatabases<T>(IEnumerable<string> fileNames, string query)
        {
            IList<T> results = new List<T>();

            try
            {
                foreach (var path in fileNames)
                {
                    using SqliteConnection connection = GetOpenSqliteConnection(path);
                    T result = connection.QueryFirstOrDefault<T>(query);
                    if (result != null) results.Add(result);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to query across databases.");
                return Enumerable.Empty<T>();
            }

            return results;
        }

        public T QueryFirst<T>(string query, object param = null)
        {
            try
            {
                using SqliteConnection connection = GetOpenSqliteConnection();
                T result = connection.QueryFirst<T>(query, param);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to query first.");
                return default(T);
            }
        }

        public T QueryFirstOrDefault<T>(string query, object param = null)
        {
            try
            {
                using SqliteConnection connection = GetOpenSqliteConnection();
                T result = connection.QueryFirstOrDefault<T>(query, param);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to query first or default.");
                return default(T);
            }
        }

        public IEnumerable<T> Query<T>(string query, object param = null)
        {
            try
            {
                using SqliteConnection connection = GetOpenSqliteConnection();
                IEnumerable<T> result = connection.Query<T>(query, param);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to query.");
                return Enumerable.Empty<T>();
            }
        }

        public int Execute(string query, object param = null)
        {
            try
            {
                using SqliteConnection connection = GetOpenSqliteConnection();
                int affected = connection.Execute(query, param);
                return affected;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to execute");
                return 0;
            }
        }

        public object ExecuteScalar(string query, object param = null)
        {
            try
            {
                using SqliteConnection connection = GetOpenSqliteConnection();
                object result = connection.ExecuteScalar(query, param);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to execute scalar.");
                return 0;
            }
        }

        private SqliteConnection GetOpenSqliteConnection(string tmpDbName = null)
        {
            if (string.IsNullOrWhiteSpace(tmpDbName) && string.IsNullOrWhiteSpace(DbName)) throw new MissingMemberException("Database name is not set.");
            if (!Path.Exists(DefaultShelterVaultPath)) Directory.CreateDirectory(DefaultShelterVaultPath);

            SqliteConnection connection = null;

            if (string.IsNullOrWhiteSpace(tmpDbName))
            {
                connection = new SqliteConnection(DbConnectionString);
            }
            else
            {
                TmpDbName = tmpDbName;
                connection = new SqliteConnection(TmpDbConnectionString);
            }

            connection.Open();
            return connection;
        }
    }
}
