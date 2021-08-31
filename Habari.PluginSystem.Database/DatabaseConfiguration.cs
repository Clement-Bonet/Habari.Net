using Habari.PluginSystem.Database.Objects;
using MySql.Data.MySqlClient;
using Newtonsoft.Json.Linq;
using Npgsql;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;

namespace Habari.PluginSystem.Database
{
    public class DatabaseConfiguration : IConfiguration
    {
        public bool AllowDelete { get; protected set; }

        public bool AllowGet { get; protected set; }

        public bool AllowPost { get; protected set; }

        public bool AllowPut { get; protected set; }

        public int DefaultPageSize { get; protected set; }

        public string MetadataUri { get; protected set; }

        public string Type => "Database";

        public string RestUri { get; protected set; }

        private string ApplicationName { get; set; }

        private string AuthType { get; set; }

        private DatabaseEngine DatabaseEngine { get; set; }

        private string DatabaseName { get; set; }

        private List<ElementPlugin> Elements { get; set; } = new List<ElementPlugin>();

        private string Host { get; set; }

        private bool SslEnabled { get; set; }

        private string CharacterSet { get; set; }

        private string Collate { get; set; }

        private string InstanceName { get; set; }

        private string Login { get; set; }

        private int MaxSimultaneousConnections { get; set; }

        private string Password { get; set; }

        private int Port { get; set; }

        private string Protocol { get; set; }

        private string SchemaName { get; set; }

        private bool SupportTransaction { get; set; }

        #region SQL

        protected DatabaseType ConvertSqlTypeToDatabaseType(string sqlType)
        {
            DatabaseType result = DatabaseType.String;

            switch (DatabaseEngine)
            {
                case DatabaseEngine.SqlServer:
                    switch (sqlType.ToUpper())
                    {
                        default:
                            result = DatabaseType.String;
                            break;
                    }
                    break;
                case DatabaseEngine.Oracle:
                    switch (sqlType.ToUpper())
                    {
                        case "BOOL":
                        case "BOOLEAN":
                            result = DatabaseType.Boolean;
                            break;
                        case "DATE":
                        case "DATETIME":
                        case "TIMESTAMP":
                            result = DatabaseType.DateTime;
                            break;
                        case "NUMBER":
                            result = DatabaseType.Double;
                            break;
                        default:
                            result = DatabaseType.String;
                            break;
                    }
                    break;
                case DatabaseEngine.MySql:
                    switch (sqlType.ToUpper())
                    {
                        default:
                            result = DatabaseType.String;
                            break;
                    }
                    break;
                case DatabaseEngine.Postgresql:
                    switch (sqlType.ToUpper())
                    {
                        default:
                            result = DatabaseType.String;
                            break;
                    }
                    break;
                default:
                    throw new Exception("Database type is not handled");
            }

            return result;
        }
        #region Connection
        #endregion Connection

        #region Functions
        protected string GetSqlFunctionsList()
        {
            string result;

            switch (DatabaseEngine)
            {
                case DatabaseEngine.SqlServer:
                    result = "SELECT TABLE_NAME FROM USER_TABLES";
                    break;
                case DatabaseEngine.Oracle:
                    result = "SELECT TABLE_NAME FROM USER_TABLES";
                    break;
                case DatabaseEngine.MySql:
                    result = "SELECT TABLE_NAME FROM USER_TABLES";
                    break;
                case DatabaseEngine.Postgresql:
                    result = "SELECT TABLE_NAME FROM USER_TABLES";
                    break;
                default:
                    result = string.Empty;
                    break;
            }

            return result;
        }

        protected string GetSqlFunctionParametersList()
        {
            string result;

            switch (DatabaseEngine)
            {
                case DatabaseEngine.SqlServer:
                    result = "SELECT TABLE_NAME FROM USER_TABLES";
                    break;
                case DatabaseEngine.Oracle:
                    result = "SELECT TABLE_NAME FROM USER_TABLES";
                    break;
                case DatabaseEngine.MySql:
                    result = "SELECT TABLE_NAME FROM USER_TABLES";
                    break;
                case DatabaseEngine.Postgresql:
                    result = "SELECT TABLE_NAME FROM USER_TABLES";
                    break;
                default:
                    result = string.Empty;
                    break;
            }

            return result;
        }
        #endregion Functions

        #region Packages
        protected string GetSqlPackagesList()
        {
            string result;

            switch (DatabaseEngine)
            {
                case DatabaseEngine.SqlServer:
                    result = "SELECT TABLE_NAME FROM USER_TABLES";
                    break;
                case DatabaseEngine.Oracle:
                    result = "SELECT TABLE_NAME FROM USER_TABLES";
                    break;
                case DatabaseEngine.MySql:
                    result = "SELECT TABLE_NAME FROM USER_TABLES";
                    break;
                case DatabaseEngine.Postgresql:
                    result = "SELECT TABLE_NAME FROM USER_TABLES";
                    break;
                default:
                    result = string.Empty;
                    break;
            }

            return result;
        }

        protected string GetSqlPackageFunctionsList()
        {
            string result;

            switch (DatabaseEngine)
            {
                case DatabaseEngine.SqlServer:
                    result = "SELECT TABLE_NAME FROM USER_TABLES";
                    break;
                case DatabaseEngine.Oracle:
                    result = "SELECT TABLE_NAME FROM USER_TABLES";
                    break;
                case DatabaseEngine.MySql:
                    result = "SELECT TABLE_NAME FROM USER_TABLES";
                    break;
                case DatabaseEngine.Postgresql:
                    result = "SELECT TABLE_NAME FROM USER_TABLES";
                    break;
                default:
                    result = string.Empty;
                    break;
            }

            return result;
        }

        protected string GetSqlPackageProceduresList()
        {
            string result;

            switch (DatabaseEngine)
            {
                case DatabaseEngine.SqlServer:
                    result = "SELECT TABLE_NAME FROM USER_TABLES";
                    break;
                case DatabaseEngine.Oracle:
                    result = "SELECT TABLE_NAME FROM USER_TABLES";
                    break;
                case DatabaseEngine.MySql:
                    result = "SELECT TABLE_NAME FROM USER_TABLES";
                    break;
                case DatabaseEngine.Postgresql:
                    result = "SELECT TABLE_NAME FROM USER_TABLES";
                    break;
                default:
                    result = string.Empty;
                    break;
            }

            return result;
        }
        #endregion Packages

        #region Procedures
        protected string GetSqlProceduresList()
        {
            string result;

            switch (DatabaseEngine)
            {
                case DatabaseEngine.SqlServer:
                    result = "SELECT TABLE_NAME FROM USER_TABLES";
                    break;
                case DatabaseEngine.Oracle:
                    result = "SELECT TABLE_NAME FROM USER_TABLES";
                    break;
                case DatabaseEngine.MySql:
                    result = "SELECT TABLE_NAME FROM USER_TABLES";
                    break;
                case DatabaseEngine.Postgresql:
                    result = "SELECT TABLE_NAME FROM USER_TABLES";
                    break;
                default:
                    result = string.Empty;
                    break;
            }

            return result;
        }
        #endregion Procedures

        #region Tables
        protected string GetSqlTablesList()
        {
            string result;

            switch (DatabaseEngine)
            {
                case DatabaseEngine.SqlServer:
                    result = "SELECT TABLE_NAME FROM USER_TABLES";
                    break;
                case DatabaseEngine.Oracle:
                    result = "SELECT TABLE_NAME FROM USER_TABLES";
                    break;
                case DatabaseEngine.MySql:
                case DatabaseEngine.Postgresql:
                    result = "SELECT T.TABLE_NAME AS NAME FROM INFORMATION_SCHEMA.TABLES T WHERE T.TABLE_TYPE = 'BASE TABLE' AND LOWER(T.TABLE_SCHEMA) = LOWER('" + SchemaName + "') ORDER BY 1";
                    break;
                default:
                    result = string.Empty;
                    break;
            }

            return result;
        }

        protected string GetSqlTableColumnsList(string tableName)
        {
            string result;

            switch (DatabaseEngine)
            {
                case DatabaseEngine.SqlServer:
                    result = "SELECT TABLE_NAME FROM USER_TABLES";
                    break;
                case DatabaseEngine.Oracle:
                    result = "SELECT TABLE_NAME FROM USER_TABLES";
                    break;
                case DatabaseEngine.MySql:
                case DatabaseEngine.Postgresql:
                    result = "SELECT C.ORDINAL_POSITION AS POS, C.COLUMN_NAME AS NAME, C.DATA_TYPE AS DATATYPE, C.IS_NULLABLE AS ISNULLABLE FROM INFORMATION_SCHEMA.COLUMNS C WHERE LOWER(C.TABLE_SCHEMA) = LOWER('" + SchemaName + "') AND LOWER(C.TABLE_NAME) = LOWER('" + tableName + "') ORDER BY 1";
                    break;
                default:
                    result = string.Empty;
                    break;
            }

            return result;
        }

        protected string GetSqlTableForeignKeysList(string tableName)
        {
            string result;

            switch (DatabaseEngine)
            {
                case DatabaseEngine.SqlServer:
                    result = "SELECT TABLE_NAME FROM USER_TABLES";
                    break;
                case DatabaseEngine.Oracle:
                    result = "SELECT TABLE_NAME FROM USER_TABLES";
                    break;
                case DatabaseEngine.MySql:
                case DatabaseEngine.Postgresql:
                    result = "SELECT RC.CONSTRAINT_NAME AS NAME, RC.REFERENCED_TABLE_NAME AS REFERENCE FROM INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS RC WHERE LOWER(RC.CONSTRAINT_SCHEMA) = LOWER('" + SchemaName + "') AND LOWER(RC.TABLE_NAME) = LOWER('" + tableName + "')";
                    break;
                default:
                    result = string.Empty;
                    break;
            }

            return result;
        }

        protected string GetSqlTableForeignKeyColumnList(string tableName, string foreignKeyName)
        {
            string result;

            switch (DatabaseEngine)
            {
                case DatabaseEngine.SqlServer:
                    result = "SELECT TABLE_NAME FROM USER_TABLES";
                    break;
                case DatabaseEngine.Oracle:
                    result = "SELECT TABLE_NAME FROM USER_TABLES";
                    break;
                case DatabaseEngine.MySql:
                case DatabaseEngine.Postgresql:
                    result = "SELECT KCU.ORDINAL_POSITION AS POS, KCU.COLUMN_NAME AS NAME FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE KCU INNER JOIN INFORMATION_SCHEMA.COLUMNS C USING (TABLE_SCHEMA, TABLE_NAME, COLUMN_NAME) WHERE LOWER(KCU.TABLE_SCHEMA) = LOWER('" + SchemaName + "') AND LOWER(KCU.TABLE_NAME) = LOWER('" + tableName + "') AND LOWER(KCU.CONSTRAINT_NAME) = LOWER('" + foreignKeyName + "')";
                    break;
                default:
                    result = string.Empty;
                    break;
            }

            return result;
        }

        protected string GetSqlTablePrimaryKey(string tableName)
        {
            string result;

            switch (DatabaseEngine)
            {
                case DatabaseEngine.SqlServer:
                    result = "SELECT TABLE_NAME FROM USER_TABLES";
                    break;
                case DatabaseEngine.Oracle:
                    result = "SELECT TABLE_NAME FROM USER_TABLES";
                    break;
                case DatabaseEngine.MySql:
                case DatabaseEngine.Postgresql:
                    result = "SELECT TC.CONSTRAINT_NAME AS NAME FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS TC WHERE LOWER(TC.TABLE_SCHEMA) = LOWER('" + SchemaName + "') AND LOWER(TC.TABLE_NAME) = LOWER('" + tableName + "') AND LOWER(TC.CONSTRAINT_TYPE) = 'primary key'";
                    break;
                default:
                    result = string.Empty;
                    break;
            }

            return result;
        }

        protected string GetSqlTablePrimaryKeyColumnList(string tableName, string primaryKeyName)
        {
            string result;

            switch (DatabaseEngine)
            {
                case DatabaseEngine.SqlServer:
                    result = "SELECT TABLE_NAME FROM USER_TABLES";
                    break;
                case DatabaseEngine.Oracle:
                    result = "SELECT TABLE_NAME FROM USER_TABLES";
                    break;
                case DatabaseEngine.MySql:
                case DatabaseEngine.Postgresql:
                    result = "SELECT KCU.ORDINAL_POSITION AS POS, KCU.COLUMN_NAME AS NAME FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE KCU INNER JOIN INFORMATION_SCHEMA.COLUMNS C USING (TABLE_SCHEMA, TABLE_NAME, COLUMN_NAME) WHERE LOWER(KCU.TABLE_SCHEMA) = LOWER('" + SchemaName + "') AND LOWER(KCU.TABLE_NAME) = LOWER('" + tableName + "') AND LOWER(KCU.CONSTRAINT_NAME) = LOWER('" + primaryKeyName + "')";
                    break;
                default:
                    result = string.Empty;
                    break;
            }

            return result;
        }
        #endregion Tables

        #region Views
        protected string GetSqlViewsList()
        {
            string result;

            switch (DatabaseEngine)
            {
                case DatabaseEngine.SqlServer:
                    result = "SELECT TABLE_NAME FROM USER_TABLES";
                    break;
                case DatabaseEngine.Oracle:
                    result = "SELECT TABLE_NAME FROM USER_TABLES";
                    break;
                case DatabaseEngine.MySql:
                case DatabaseEngine.Postgresql:
                    result = "SELECT V.TABLE_NAME AS NAME FROM INFORMATION_SCHEMA.VIEWS V WHERE LOWER(V.TABLE_SCHEMA) = LOWER('" + SchemaName + "') ORDER BY 1";
                    break;
                default:
                    result = string.Empty;
                    break;
            }

            return result;
        }

        protected string GetSqlViewColumnsList(string viewName)
        {
            string result;

            switch (DatabaseEngine)
            {
                case DatabaseEngine.SqlServer:
                    result = "SELECT TABLE_NAME FROM USER_TABLES";
                    break;
                case DatabaseEngine.Oracle:
                    result = "SELECT TABLE_NAME FROM USER_TABLES";
                    break;
                case DatabaseEngine.MySql:
                case DatabaseEngine.Postgresql:
                    result = "SELECT C.ORDINAL_POSITION AS POS, C.COLUMN_NAME AS NAME, C.DATA_TYPE AS DATATYPE, C.IS_NULLABLE AS ISNULLABLE FROM INFORMATION_SCHEMA.COLUMNS C WHERE LOWER(C.TABLE_SCHEMA) = LOWER('" + SchemaName + "') AND LOWER(C.TABLE_NAME) = LOWER('" + viewName + "') ORDER BY 1";
                    break;
                default:
                    result = string.Empty;
                    break;
            }

            return result;
        }
        #endregion Views
        #endregion SQL

        #region Objects
        #region Tables
        protected List<Table> GetTablesList()
        {
            List<Table> result = new List<Table>();
            using (DbConnection dbConnection = GetDbConnection())
            {
                DbCommand dbCommandTable = dbConnection.CreateCommand();
                dbCommandTable.CommandText = GetSqlTablesList();
                dbCommandTable.CommandType = CommandType.Text;
                DbDataReader dbDataReaderTable = dbCommandTable.ExecuteReader();
                while (dbDataReaderTable.Read())
                {
                    // Table
                    Table table = new Table() { Name = dbDataReaderTable.GetString("NAME") };
                    result.Add(table);
                }
                dbDataReaderTable.Close();

                foreach (var table in result)
                {
                    // Columns
                    DbCommand dbCommandTableColumns = dbConnection.CreateCommand();
                    dbCommandTableColumns.CommandText = GetSqlTableColumnsList(table.Name);
                    dbCommandTableColumns.CommandType = CommandType.Text;
                    DbDataReader dbDataReaderTableColumns = dbCommandTableColumns.ExecuteReader();
                    while (dbDataReaderTableColumns.Read())
                    {
                        table.Columns.Add(dbDataReaderTableColumns.GetInt32("POS"), new Column() { Name = dbDataReaderTableColumns.GetString("NAME"), Type = ConvertSqlTypeToDatabaseType(dbDataReaderTableColumns.GetString("DATATYPE")), IsNullabled = (dbDataReaderTableColumns.GetString("ISNULLABLE").ToLower() == "y") });
                    }
                    dbDataReaderTableColumns.Close();
                    // Primary Key
                    DbCommand dbCommandPrimaryKey = dbConnection.CreateCommand();
                    dbCommandPrimaryKey.CommandText = GetSqlTablePrimaryKey(table.Name);
                    dbCommandPrimaryKey.CommandType = CommandType.Text;
                    DbDataReader dbDataReaderPrimaryKey = dbCommandPrimaryKey.ExecuteReader();
                    if (dbDataReaderPrimaryKey.Read())
                    {
                        table.PrimaryKey = new PrimaryKey() { Name = dbDataReaderPrimaryKey.GetString("NAME") };
                    }
                    dbDataReaderPrimaryKey.Close();
                    // Primary Key columns
                    DbCommand dbCommandPrimaryKeyColumns = dbConnection.CreateCommand();
                    dbCommandPrimaryKeyColumns.CommandText = GetSqlTablePrimaryKeyColumnList(table.Name, table.PrimaryKey.Name);
                    dbCommandPrimaryKeyColumns.CommandType = CommandType.Text;
                    DbDataReader dbDataReaderPrimaryKeyColumns = dbCommandPrimaryKeyColumns.ExecuteReader();
                    while (dbDataReaderPrimaryKeyColumns.Read())
                    {
                        table.PrimaryKey.Columns.Add(dbDataReaderPrimaryKeyColumns.GetInt32("POS"), table.Columns.First(c => c.Value.Name == dbDataReaderPrimaryKeyColumns.GetString("NAME")).Value);
                    }
                    dbDataReaderPrimaryKeyColumns.Close();
                }

                foreach (var table in result)
                {
                    //Foreign keys
                    DbCommand dbCommandForeignKeys = dbConnection.CreateCommand();
                    dbCommandForeignKeys.CommandText = GetSqlTableForeignKeysList(table.Name);
                    dbCommandForeignKeys.CommandType = CommandType.Text;
                    DbDataReader dbDataReaderForeignKeys = dbCommandForeignKeys.ExecuteReader();
                    while (dbDataReaderForeignKeys.Read())
                    {
                        ForeignKey foreignKey = new ForeignKey() { Name = dbDataReaderForeignKeys.GetString("NAME"), ReferencedTable = result.First(t => t.Name == dbDataReaderForeignKeys.GetString("REFERENCE")) };
                        table.ForeignKeys.Add(foreignKey);
                    }
                    dbDataReaderForeignKeys.Close();
                    foreach (var foreignKey in table.ForeignKeys)
                    {
                        //Foreign key columns
                        DbCommand dbCommandForeignKeyColumns = dbConnection.CreateCommand();
                        dbCommandForeignKeyColumns.CommandText = GetSqlTableForeignKeyColumnList(table.Name, foreignKey.Name);
                        dbCommandForeignKeyColumns.CommandType = CommandType.Text;
                        DbDataReader dbDataReaderForeignKeyColumns = dbCommandForeignKeyColumns.ExecuteReader();
                        while (dbDataReaderForeignKeyColumns.Read())
                        {
                            foreignKey.Columns.Add(dbDataReaderForeignKeyColumns.GetInt32("POS"), table.Columns.First(c => c.Value.Name == dbDataReaderForeignKeyColumns.GetString("NAME")).Value);
                        }
                        dbDataReaderForeignKeyColumns.Close();
                    }
                }

                dbConnection.Close();
            }
            return result;
        }
        #endregion Tables

        #region Views
        protected List<View> GetViewsList()
        {
            List<View> result = new List<View>();
            using (DbConnection dbConnection = GetDbConnection())
            {
                DbCommand dbCommandView = dbConnection.CreateCommand();
                dbCommandView.CommandText = GetSqlViewsList();
                dbCommandView.CommandType = CommandType.Text;
                DbDataReader dbDataReaderView = dbCommandView.ExecuteReader();
                while (dbDataReaderView.Read())
                {
                    // View
                    View view = new View() { Name = dbDataReaderView.GetString("NAME") };
                    result.Add(view);
                }
                dbDataReaderView.Close();

                foreach (var view in result)
                {
                    // Columns
                    DbCommand dbCommandViewColumns = dbConnection.CreateCommand();
                    dbCommandViewColumns.CommandText = GetSqlViewColumnsList(view.Name);
                    dbCommandViewColumns.CommandType = CommandType.Text;
                    DbDataReader dbDataReaderViewColumns = dbCommandViewColumns.ExecuteReader();
                    while (dbDataReaderViewColumns.Read())
                    {
                        view.Columns.Add(dbDataReaderViewColumns.GetInt32("POS"), new Column() { Name = dbDataReaderViewColumns.GetString("NAME"), Type = ConvertSqlTypeToDatabaseType(dbDataReaderViewColumns.GetString("DATATYPE")), IsNullabled = (dbDataReaderViewColumns.GetString("ISNULLABLE").ToLower() == "y") });
                    }
                    dbDataReaderViewColumns.Close();
                }

                dbConnection.Close();
            }
            return result;
        }
        #endregion Views
        #endregion Objects

        public Schema ExploreSchema()
        {
            Schema result = new Schema()
            {
                Name = SchemaName
            };
            result.Tables.AddRange(GetTablesList());
            result.Views.AddRange(GetViewsList());
            return result;
        }

        public DbConnection GetDbConnection()
        {
            DbConnection result;
            switch (DatabaseEngine)
            {
                case DatabaseEngine.MySql:
                    result = new MySqlConnection();
                    result.ConnectionString = "Server=" + Host + ";Database=" + DatabaseName + ";port=" + Port + ";User Id=" + Login + ";password=" + Password + ";Character Set=" + CharacterSet + (SslEnabled ? ";SSLMode=Required" : ";SSLMode=None");
                    result.Open();
                    using (DbCommand cmd = result.CreateCommand())
                    {
                        cmd.CommandText = "SET NAMES '" + CharacterSet + "' COLLATE '" + Collate + "'";
                        cmd.ExecuteNonQuery();
                    }
                    break;
                case DatabaseEngine.Oracle:
                    result = new OracleConnection();
                    result.ConnectionString = "user id=" + Login + ";password=" + Password + ";data source=(DESCRIPTION=(ADDRESS=(PROTOCOL=" + Protocol + ")(HOST=" + Host + ")(PORT=" + Port + "))(CONNECT_DATA=(SERVICE_NAME=" + ApplicationName + ")))";
                    result.Open();
                    break;
                case DatabaseEngine.Postgresql:
                    result = new NpgsqlConnection();
                    result.ConnectionString = "Host=" + Host + ";Username=" + Login + ";password=" + Password + ";Database=" + DatabaseName + ";port=" + Port + (SslEnabled ? ";SSLMode=Required" : "");
                    result.Open();
                    break;
                case DatabaseEngine.SqlServer:
                    result = new SqlConnection();
                    result.ConnectionString = "Data Source=" + Host + "," + Port + ";Network Library=DBMSSOCN;Initial Catalog=" + DatabaseName + ";User ID=" + Login + ";Password=" + Password;
                    result.Open();
                    break;
                default:
                    throw (new Exception("Unhandled database connection"));
            }
            return result;
        }

        public void LoadFromJToken(JToken token, int defaultPageSize, bool allowDelete, bool allowGet, bool allowPost, bool allowPut)
        {
            AllowDelete = token.ValueOrDefault("allowDelete", allowDelete);
            AllowGet = token.ValueOrDefault("allowSelect", allowGet);
            AllowPost = token.ValueOrDefault("allowInsert", allowPost);
            AllowPut = token.ValueOrDefault("allowUpdate", allowPut);
            DefaultPageSize = token.ValueOrDefault("defaultPageSize", defaultPageSize);
            MetadataUri = token.Value<string>("metadataUri");
            RestUri = token.Value<string>("restUri");
            ApplicationName = token.ValueOrDefault("applicationName", "Habari.Database." + token.Value<string>("databaseEngine").ToString());
            AuthType = token.Value<string>("authType");
            DatabaseEngine = token.ValueFromEnum<DatabaseEngine>("databaseEngine");
            DatabaseName = token.Value<string>("databaseName");
            Host = token.Value<string>("host");
            SslEnabled = token.Value<bool>("sslEnabled");
            CharacterSet = token.Value<string>("characterSet");
            Collate = token.Value<string>("collate");
            InstanceName = token.Value<string>("instanceName");
            Login = token.Value<string>("login");
            MaxSimultaneousConnections = token.Value<int>("maxSimultaneousConnections");
            Password = token.Value<string>("password");
            Port = token.Value<int>("port");
            Protocol = token.ValueOrDefault("protocol", "tcp");
            SchemaName = token.Value<string>("schemaName");
            SupportTransaction = token.Value<bool>("supportTransaction");
            foreach (JToken element in token["elements"].Children())
            {
                ElementPlugin elm = new ElementPlugin();
                elm.Configure(element, DefaultPageSize, AllowDelete, AllowGet, AllowPost, AllowPut);
                Elements.Add(elm);
            }
        }
    }
}
