using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using ApiRequestManager.Interfaces;
using ApiRequestManager.Models;
using Microsoft.Extensions.Configuration;

namespace ApiRequestManager.Repositories
{
    public class RequestRepository : IRequestRepository
    {

        private string _connectionString;

        private readonly IConfiguration _configuration;

        public RequestRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("ApiRequestManagerDb");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public async Task<Api> GetApiByNameAsync(string name)
        {
            Api api = null;

            string sql = @"SELECT ApiId, ApiName, ApiDescription, BaseUri
                            FROM apirequestmgrdb.dbo.APIs
                            WHERE ApiName = @api_name;";

            try
            {

                using (var connection = new SqlConnection(_connectionString))
                {
                    var command = new SqlCommand(sql, connection)
                    {
                        CommandType = System.Data.CommandType.Text
                    };
                    command.Parameters.AddWithValue("@api_name", name);
                    connection.Open();
                    using (var reader = await command.ExecuteReaderAsync())
                    {

                        while (reader.Read())
                        {
                            api = new Api
                            {
                                ApiId = Convert.ToInt32(reader["ApiId"]),
                                ApiName = reader["ApiName"].ToString(),
                                ApiDescription = reader["ApiDescription"].ToString(),
                                BaseUri = reader["BaseUri"].ToString()
                            };


                        }


                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return api ?? new Api();
        }

        public async Task<Api> GetApiByIdAsync(int apiId)
        {
            Api api = null;

            string sql = @"SELECT ApiId, ApiName, ApiDescription, BaseUri
                            FROM apirequestmgrdb.dbo.APIs
                            WHERE ApiId = @api_id;";

            try
            {

                using (var connection = new SqlConnection(_connectionString))
                {
                    var command = new SqlCommand(sql, connection)
                    {
                        CommandType = System.Data.CommandType.Text
                    };
                    command.Parameters.AddWithValue("@api_id", apiId);
                    connection.Open();
                    using var reader = await command.ExecuteReaderAsync();

                    while (reader.Read())
                    {
                        api = new Api
                        {
                            ApiId = apiId,
                            ApiName = reader["ApiName"].ToString(),
                            ApiDescription = reader["ApiDescription"].ToString(),
                            BaseUri = reader["BaseUri"].ToString()
                        };


                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return api ?? new Api();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="apiId"></param>
        /// <returns></returns>
        public async Task<IList<RequestParameter>> GetRequestParametersAsync(int apiId, int userId)
        {
            var requestParameters = new List<RequestParameter>();
            string sql = @"SELECT 
                            rp.ParameterId, 
                            rp.ApiId , 
                            rp.Name, 
                            rp.ParameterValue, 
                            rp.IsOptional, 
                            rp.SequenceNumber, 
                            rp.TypeId,
                            pt.TypeName,
                            urp.ParameterValue  as 'UserParameterValue'
                            FROM apirequestmgrdb.dbo.RequestParameters rp
                             INNER JOIN apirequestmgrdb.dbo.ParameterTypes pt 
                               ON rp.TypeId = pt.TypeId 
                            LEFT JOIN  apirequestmgrdb.dbo.UserRequestParameters urp
                              ON rp.ParameterId = urp.ParameterId
                              LEFT JOIN apirequestmgrdb.dbo.IgUser iu
                                ON urp.UserId = iu.UserId
                                AND iu.UserId = @user_id
                            WHERE rp.ApiId = @api_id;";

            using (var connection = new SqlConnection(_connectionString))
            {
                try
                {
                    var command = new SqlCommand(sql, connection)
                    {
                        CommandType = System.Data.CommandType.Text
                    };

                    command.Parameters.AddWithValue("@user_id", userId);
                    command.Parameters.AddWithValue("@api_id", apiId);

                    connection.Open();
                    using var reader = await command.ExecuteReaderAsync();

                    while (reader.Read())
                    {

                        var parameter = new RequestParameter
                        {
                            ParameterId = Convert.ToInt32(reader["ParameterId"]),
                            Name = reader["Name"] != DBNull.Value ? Convert.ToString(reader["Name"]) : null,
                            ParameterValue = reader["ParameterValue"] != DBNull.Value ? Convert.ToString(reader["ParameterValue"]) : null,
                            IsOptional = Convert.ToBoolean(reader["IsOptional"]),
                            ApiId = Convert.ToInt32(reader["ApiId"]),
                            UserParameterValue = reader["UserParameterValue"] != DBNull.Value ? Convert.ToString(reader["UserParameterValue"]) : null,
                        };

                        var type = new ParameterType
                        {
                            TypeId = Convert.ToInt32(reader["ParameterId"]),
                            TypeName = reader["TypeName"] != DBNull.Value ? Convert.ToString(reader["TypeName"]) : null

                        };

                        parameter.Type = type;
                       
                        requestParameters.Add(parameter);

                    }
                }
                catch (Exception ex)
                {
                    //TODO: Add logging here ...
                    requestParameters.Clear();
                }
            }
            return requestParameters ?? new List<RequestParameter>();
        }

    }
}
