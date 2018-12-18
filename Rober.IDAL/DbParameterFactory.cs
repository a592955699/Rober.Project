using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Text;

namespace Rober.IDAL
{
    public static class DbParameterFactory
    {
        /// <summary>
        /// Gets a support database parameter object (used by stored procedures)
        /// </summary>
        /// <returns>Parameter</returns>
        public static DbParameter GetParameter()
        {
            return new SqlParameter();
        }
        #region 
        private static DbParameter GetParameter(DbType dbType, string parameterName,
            object parameterValue)
        {
            var parameter = GetParameter();
            parameter.ParameterName = parameterName;
            parameter.Value = parameterValue;
            parameter.DbType = dbType;

            return parameter;
        }

        private static DbParameter GetOutputParameter(DbType dbType, string parameterName)
        {
            var parameter = GetParameter();
            parameter.ParameterName = parameterName;
            parameter.DbType = dbType;
            parameter.Direction = ParameterDirection.Output;

            return parameter;
        }

        /// <summary>
        /// Get string parameter
        /// </summary>
        /// <param name="dataProvider">Data provider</param>
        /// <param name="parameterName">Parameter name</param>
        /// <param name="parameterValue">Parameter value</param>
        /// <returns>Parameter</returns>
        public static DbParameter GetStringParameter(string parameterName, string parameterValue)
        {
            return GetParameter(DbType.String, parameterName, (object)parameterValue ?? DBNull.Value);
        }

        /// <summary>
        /// Get output string parameter
        /// </summary>
        /// <param name="dataProvider">Data provider</param>
        /// <param name="parameterName">Parameter name</param>
        /// <returns>Parameter</returns>
        public static DbParameter GetOutputStringParameter(string parameterName)
        {
            return GetOutputParameter(DbType.String, parameterName);
        }

        /// <summary>
        /// Get int parameter
        /// </summary>
        /// <param name="dataProvider">Data provider</param>
        /// <param name="parameterName">Parameter name</param>
        /// <param name="parameterValue">Parameter value</param>
        /// <returns>Parameter</returns>
        public static DbParameter GetInt32Parameter(string parameterName, int? parameterValue)
        {
            return GetParameter(DbType.Int32, parameterName, parameterValue.HasValue ? (object)parameterValue.Value : DBNull.Value);
        }

        /// <summary>
        /// Get output int32 parameter
        /// </summary>
        /// <param name="dataProvider">Data provider</param>
        /// <param name="parameterName">Parameter name</param>
        /// <returns>Parameter</returns>
        public static DbParameter GetOutputInt32Parameter(string parameterName)
        {
            return GetOutputParameter(DbType.Int32, parameterName);
        }

        /// <summary>
        /// Get boolean parameter
        /// </summary>
        /// <param name="dataProvider">Data provider</param>
        /// <param name="parameterName">Parameter name</param>
        /// <param name="parameterValue">Parameter value</param>
        /// <returns>Parameter</returns>
        public static DbParameter GetBooleanParameter(string parameterName, bool? parameterValue)
        {
            return GetParameter(DbType.Boolean, parameterName, parameterValue.HasValue ? (object)parameterValue.Value : DBNull.Value);
        }

        /// <summary>
        /// Get decimal parameter
        /// </summary>
        /// <param name="dataProvider">Data provider</param>
        /// <param name="parameterName">Parameter name</param>
        /// <param name="parameterValue">Parameter value</param>
        /// <returns>Parameter</returns>
        public static DbParameter GetDecimalParameter(string parameterName, decimal? parameterValue)
        {
            return GetParameter(DbType.Decimal, parameterName, parameterValue.HasValue ? (object)parameterValue.Value : DBNull.Value);
        }


        /// <summary>
        /// Get datetime parameter
        /// </summary>
        /// <param name="dataProvider">Data provider</param>
        /// <param name="parameterName">Parameter name</param>
        /// <param name="parameterValue">Parameter value</param>
        /// <returns>Parameter</returns>
        public static DbParameter GetDateTimeParameter(string parameterName, DateTime? parameterValue)
        {
            return GetParameter(DbType.DateTime, parameterName, parameterValue.HasValue ? (object)parameterValue.Value : DBNull.Value);
        }
        #endregion
    }
}
