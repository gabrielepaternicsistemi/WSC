using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WSC.Controllers
{
    public class LoginController :  ApiController
    {
        [HttpGet]
        [Route("api/sync/login")]
        public HttpResponseMessage Login(string username, string password)
        {

            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DB_DATI"].ToString()))
            {
                string command = @"SELECT [us_NOME]
                                          ,[us_COGNOME]
                                          ,[us_USERNAME]
                                          ,[us_PASSWORD]
                                          ,[us_OPERATORE]
                                          ,[us_SUPERAGENTE]
                                          ,[us_ABILITATO]
                                      FROM [dbo].[HH_USERAPP] 
                                      WHERE US_USERNAME='" + username + "' AND US_PASSWORD='" + password + "' AND US_ABILITATO='S'";

                var result = connection.Query(command);
                return Request.CreateResponse(result);
            }
        }
    }
}