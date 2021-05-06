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

    public class SendAziendaController : ApiController
    {

        private static readonly log4net.ILog log =
        log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        [HttpPost]
        public HttpResponseMessage Post(Azienda x)
        {
            Azienda s = x;

            //INSERT Azienda
            IFormatProvider theCultureInfo = new System.Globalization.CultureInfo("it-IT", true);
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DB_DATI"].ToString()))
            {
                using (SqlCommand _cmd = new SqlCommand("inseriscicliente", connection))
                {
                    try
                    {
                        _cmd.Connection.Open();
                        _cmd.CommandType = CommandType.StoredProcedure;

                        _cmd.Parameters.AddWithValue("@an_descr", s.an_descr);
                        _cmd.Parameters.AddWithValue("@piva", s.piva);
                        _cmd.Parameters.AddWithValue("@email", s.email);
                        _cmd.Parameters.AddWithValue("@cellulare", s.cellulare);
                        _cmd.Parameters.AddWithValue("@codpaga", s.codpaga);
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
