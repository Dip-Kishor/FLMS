using FLMS.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace FLMS.Data
{
    internal class StoredPocedureExtension
    {
        private readonly FLMSContext _context;

        public StoredPocedureExtension(FLMSContext context)
        {
            this._context = context;
        }

        public async Task<List<T>> ExecuteStoredProcedureAsync<T>(string storedProcedureName, params SqlParameter[] parameters) where T : class, new()
        {
            var results = new List<T>();
            var connection = _context.Database.GetDbConnection();

            try
            {
                await connection.OpenAsync();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = storedProcedureName;
                    command.CommandType = CommandType.StoredProcedure;

                    if (parameters != null)
                    {
                        command.Parameters.AddRange(parameters);
                    }

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            T obj = new T();

                            foreach (var prop in typeof(T).GetProperties())
                            {
                                if (!reader.HasColumn(prop.Name) || reader[prop.Name] == DBNull.Value)
                                    continue;

                                var value = reader[prop.Name];
                                prop.SetValue(obj, Convert.ChangeType(value, prop.PropertyType));
                            }

                            results.Add(obj);
                        }
                    }
                }
            }
            finally
            {
                await connection.CloseAsync();
            }

            return results;
        }
    }
    public static class DataReaderExtensions
    {
        public static bool HasColumn(this DbDataReader reader, string columnName)
        {
            for (int i = 0; i < reader.FieldCount; i++)
            {
                if (reader.GetName(i).Equals(columnName, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }
            return false;
        }
    }

}
