using CrystalDecisions.CrystalReports.Engine;
using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web;
using System.Web.Configuration;
using System.Net.Mail;

namespace WSC.Controllers
{
    //[Authorize]
    public class SyncController : ApiController
    {
        [HttpGet]
        [Route("api/sync/aziende")]
        public HttpResponseMessage Aziende()
        {
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DB_DATI"].ToString()))
            {
                string command = @"SELECT  [an_CONTO]
                                          ,[an_DESCR1]
                                          ,[an_DESCR2]                                        
                                          ,[an_PARIVA]
                                          ,[an_TELEF]
                                          ,[an_EMAIL]
                                          ,[an_EMAILPEC]                                                                            
                                      FROM [dbo].[MBV_ANAGRA] order by an_descr1 ";
                var result = connection.Query(command);
                return Request.CreateResponse(result);
            }
        }


        [HttpGet]
        [Route("api/sync/articoli")]
        public HttpResponseMessage Articoli()
        {

            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DB_DATI"].ToString()))
            {
                string command = @"SELECT [ar_codart]
                                         ,[ar_descr]
                                         ,[ar_unmis]
                                         ,[BARCODE]
                                         ,[ar_gruppo]
                                         ,[ar_sotgru]
                                         ,[LISTINO1]
                                         ,[GPV]
                                   FROM [dbo].[gb_artico];";
                var result = connection.Query(command);
                return Request.CreateResponse(result);
            }
        }
        
        [HttpGet]
        [Route("api/sync/Azienda")]
        public HttpResponseMessage Azienda(int conto)
        {
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DB_DATI"].ToString()))
            {
                var parameter = new {Conto = conto };
                string command = @"SELECT  [an_CONTO]
                                          ,[an_DESCR1]
                                          ,[an_DESCR2]
                                          ,[an_INDIR]
                                          ,[an_CAP]
                                          ,[an_CITTA]
                                          ,[an_PROV]
                                          ,[an_PARIVA]
                                          ,[an_TELEF]
                                          ,[an_EMAIL]
                                          ,[an_EMAILPEC]
                                          ,isnull([an_latitud],0) an_LATITUD
                                          ,isnull([an_longitud],0) an_LONGITUDs                                                                              
                                      FROM [dbo].[MBV_ANAGRA] where an_CONTO = @Conto";
                var result = connection.Query(command, parameter);
                return Request.CreateResponse(result);
            }

        }

        [HttpGet]
        [Route("api/sync/Articolo")]
        public HttpResponseMessage Articolo(string id)
        {
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DB_DATI"].ToString()))
            {
                var parameter = new { artico = id };
                string command = @"SELECT [ar_codart]
                                         ,[ar_descr]
                                         ,[BARCODE]
                                   FROM [dbo].[gb_artico] where ar_codart = @artico";
                var result = connection.Query(command, parameter);
                return Request.CreateResponse(result);
            }

        }

        [HttpGet]
        [Route("api/sync/UnitaMisura")]
        public HttpResponseMessage UnitaMisura()
        {
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DB_DATI"].ToString()))
            {
                string command = @"select tb_codumis, 
	                                      tb_desumis
                                   from tabumis";
                var result = connection.Query(command);
                return Request.CreateResponse(result);
            }

        }

        [HttpGet]
        [Route("api/sync/Gruppi")]
        public HttpResponseMessage Gruppi()
        {
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DB_DATI"].ToString()))
            {
                string command = @"select tb_codgmer, 
	                                      tb_desgmer 
                                   from tabgmer";
                var result = connection.Query(command);
                return Request.CreateResponse(result);
            }

        }

        [HttpGet]
        [Route("api/sync/SottoGruppi")]
        public HttpResponseMessage SottoGruppi(int gruppo)
        {
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DB_DATI"].ToString()))
            {
                var parameter = new { Gruppo = gruppo };
                string command = @"select tb_codsgme, 
                                          tb_dessgme 
                                   from tabsgme 
                                   where tb_codgrupm = @Gruppo";
                var result = connection.Query(command, parameter);
                return Request.CreateResponse(result);
            }

        }

        [HttpGet]
        [Route("api/sync/CodiciPagamento")]
        public HttpResponseMessage CodiciPagamento()
        {
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DB_DATI"].ToString()))
            {
                string command = @"select tb_codpaga, tb_despaga from tabpaga";
                var result = connection.Query(command);
                return Request.CreateResponse(result);
            }

        }

        // GET api/values
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}
