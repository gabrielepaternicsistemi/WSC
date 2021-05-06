using System;
using System.Collections.Generic;
using System.Linq;

using System.Web;
using System.Net.Http;
using System.Web.Http;
using WSC.Model;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using Newtonsoft.Json;
using System.Net;
using CrystalDecisions.CrystalReports.Engine;
using System.Web.Configuration;
using System.Net.Mail;

namespace WSC.Controllers
{

    public class SendArticoloController : ApiController
    {

        private static readonly log4net.ILog log =
        log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        [HttpPost]
        public HttpResponseMessage Post(Articolo x)
        {
            Articolo s = x;

            //INSERT Articolo
            IFormatProvider theCultureInfo = new System.Globalization.CultureInfo("it-IT", true);
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DB_DATI"].ToString()))
            {
                using (SqlCommand _cmd = new SqlCommand("inserisciarticolo", connection))
                {
                    try
                    {
                        _cmd.Connection.Open();
                        _cmd.CommandType = CommandType.StoredProcedure;

                        _cmd.Parameters.AddWithValue("@ar_descr", s.ar_descr);
                        _cmd.Parameters.AddWithValue("@ar_codiva", s.ar_codiva);
                        _cmd.Parameters.AddWithValue("@ar_gruppo", s.ar_gruppo);
                        _cmd.Parameters.AddWithValue("@ar_sotgru", s.ar_sotgru);
                        _cmd.Parameters.AddWithValue("@ar_codart", s.ar_codart);
                        _cmd.Parameters.AddWithValue("@bc_code", s.bc_code);
                        _cmd.Parameters.AddWithValue("@lc_listino1", s.lc_listino1);
                        _cmd.Parameters.AddWithValue("@ar_unmis", s.ar_unmis);
                        _cmd.ExecuteNonQuery();
                    }
                    catch (Exception e)
                    {
                        //SENDMAIL
                        log.Info(e.ToString());
                        return Request.CreateResponse(HttpStatusCode.InternalServerError, e.ToString());
                    }
                }
            }
            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}
