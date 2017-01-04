// ***********************************************************************
// Assembly         : DataAccessLayer
// Author           : 
// Created          : 07-25-2016
//
// Last Modified By : 
// Last Modified On : 08-26-2016
// ***********************************************************************
// <copyright file="DbUtilities.cs" company="">
//     Copyright ©  2016
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace DataAccessLayer.Helper
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Common;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Transactions;

    /// <summary>
    /// Class DbUtilities.
    /// </summary>
    public class DbUtilities
    {
        /// <summary>
        /// The retry count
        /// </summary>
        private const int RetryCount = 3;

        /// <summary>
        /// Inserts the specified connection string.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="value">The value.</param>
        /// <returns>System.Int32.</returns>
        public int Insert(SqlConnection conn, object value)
        {
            var sqlStub = this.GetInsertSqlStub(value);
            var parameters = this.GetInsertSQLParams(value);
            return this.ExecuteNonQuery(conn, sqlStub, parameters);
        }

        /// <summary>
        /// Inserts the batch.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="value">The value.</param>
        /// <returns>System.Int32.</returns>
        public int InsertBatch(string connectionString, IEnumerable<object> value)
        {
            var result = 0;
            using (
                var transactionScope = new TransactionScope(
                    TransactionScopeOption.Required,
                    new TransactionOptions
                    {
                        IsolationLevel = IsolationLevel.ReadCommitted,
                        Timeout = TimeSpan.FromSeconds(10)
                    }))
            {
                using (var sqlConn = new SqlConnection(connectionString))
                {
                    if (sqlConn.State != System.Data.ConnectionState.Open)
                    {
                        sqlConn.Open();
                    }
                    foreach (var val in value)
                    {
                        result += this.Insert(sqlConn, val);
                    }

                }
                transactionScope.Complete();
            }

            return result;
        }

        /// <summary>
        /// Executes the non query.
        /// </summary>
        /// <param name="dbConnectionString">The database connection string.</param>
        /// <param name="sql">The SQL.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>System.Int32.</returns>
        public int ExecuteNonQuery(SqlConnection connection, string sql, SqlParameter[] parameters)
        {
            var count = 0;
            while (true)
            {
                if (connection.State != System.Data.ConnectionState.Open)
                {
                    connection.Open();
                }
                SqlCommand command = null;
                try
                {
                    command = new SqlCommand(sql, connection);
                    if (parameters != null)
                    {
                        command.Parameters.AddRange(parameters);
                    }


                    return command.ExecuteNonQuery();
                }

                catch
                {
                    if (count >= RetryCount)
                    {
                        throw;
                    }
                }

                finally
                {
                    if (command != null && parameters != null)
                    {
                        command.Parameters.Clear();
                        command.Dispose();
                        command = null;
                    }
                }

                count++;
            }
        }

        /// <summary>
        /// Executes the store query.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="context">The context.</param>
        /// <param name="commandText">The command text.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>IEnumerable&lt;T&gt;.</returns>
        /// <exception cref="InvalidOperationException"></exception>
        public IEnumerable<T> ExecuteStoreQuery<T>(DbContext context, string commandText, params object[] parameters)
        {
            if (parameters.Any(p => p is DbParameter))
            {
                throw new InvalidOperationException();
            }

            var objContext = ((IObjectContextAdapter)context).ObjectContext;
            objContext.CommandTimeout = 300;
            return objContext.ExecuteStoreQuery<T>(commandText, parameters);
        }

        /// <summary>
        /// Gets the insert SQL stub.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>System.String.</returns>
        private string GetInsertSqlStub(object value)
        {
            var type = value.GetType();
            var sb = new StringBuilder("insert into ");
            var sbSqlParameter = new StringBuilder(" values(");
            var tableAttr = type.GetCustomAttribute<TableNameAttribute>();
            if (tableAttr != null)
            {
                sb.Append(tableAttr.TableName);
            }
            else
            {
                sb.Append(type.Name);
            }

            sb.Append(" (");
            foreach (var property in type.GetProperties())
            {
                if (property.GetValue(value) != null && !this.HaveIgnoreAttribute(property))
                {
                    var colAttr = property.GetCustomAttribute<ColumnAttribute>();
                    if (colAttr != null)
                    {
                        sb.Append(colAttr.Name);
                    }
                    else
                    {
                        sb.Append(property.Name);
                    }
                    sb.Append(",");
                    sbSqlParameter.Append("@" + property.Name);
                    sbSqlParameter.Append(",");
                }
            }

            sb.Replace(',', ')', sb.Length - 1, 1);
            sbSqlParameter.Replace(',', ')', sbSqlParameter.Length - 1, 1);
            sb.Append(sbSqlParameter);
            return sb.ToString();
        }

        /// <summary>
        /// Gets the insert SQL parameters.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>SqlParameter[].</returns>
        private SqlParameter[] GetInsertSQLParams(object value)
        {
            var parameters = new List<SqlParameter>();
            var type = value.GetType();
            foreach (var property in type.GetProperties())
            {
                if (property.GetValue(value) != null && !this.HaveIgnoreAttribute(property))
                {
                    parameters.Add(new SqlParameter("@" + property.Name, property.GetValue(value)));
                }
            }

            return parameters.ToArray();
        }

        /// <summary>
        /// Haves the ignore attribute.
        /// </summary>
        /// <param name="propertyInfo">The property information.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        private bool HaveIgnoreAttribute(PropertyInfo propertyInfo)
        {
            return propertyInfo.GetCustomAttribute<DBNotMapAttribute>() != null;
        }
    }
}