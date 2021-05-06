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
        
    public class Results
    {
        public RisChiamata chiamataResult;
        public List<RisAttivita> attivitaResult = new List<RisAttivita>();


    }
    public class RisChiamata
    {
        public int nuovoCodiceChiamata;
        public int vecchioCodiceChiamata;
    }
    public class RisAttivita
    {
        public int vecchioCodiceAttivita;
        public int nuovoCodiceAttivita;
    }

    [Authorize]
    public class SendChiamataController : ApiController
    {

        private static readonly log4net.ILog log =
    log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        [HttpPost]
        public HttpResponseMessage Post(ChiamataMobile x)
        {


            ChiamataMobile s = x;
            Results r = new Results();

            int codiceChiamata;
            //WEBHOOK
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DB_DATI"].ToString()))
            {
                try
                {
                    using (SqlCommand _cmd = new SqlCommand("InserisciWebHook", connection))
                    {
                        _cmd.Connection.Open();
                        _cmd.CommandType = CommandType.StoredProcedure;
                        _cmd.Parameters.AddWithValue("@json", JsonConvert.SerializeObject(x));
                        _cmd.ExecuteNonQuery();
                        _cmd.Connection.Close();
                    }
                }
                catch(Exception e)
                {
                    log.Info(e.ToString());
                }
            }

          
            if (x.chiamata.op_CODCHIA < 0)
            {
                //INSERT CHIAMATA
                IFormatProvider theCultureInfo = new System.Globalization.CultureInfo("it-IT", true);
                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DB_DATI"].ToString()))
                {
                    using (SqlCommand _cmd = new SqlCommand("CSSUP_inserisciChiamataMobile", connection))
                    {
                        try
                        {
                            _cmd.Connection.Open();
                            _cmd.CommandType = CommandType.StoredProcedure;

                            _cmd.Parameters.AddWithValue("@op_CODART", s.chiamata.op_CODART);
                            _cmd.Parameters.AddWithValue("@op_CONTO", s.chiamata.op_CONTO);
                            _cmd.Parameters.AddWithValue("@op_CODTCHI", s.chiamata.op_CODTCHI);
                            //_cmd.Parameters.AddWithValue("@op_DATCHIA", new DateTime(1970, 1, 1).AddMilliseconds((s.chiamata.op_DATCHIA)));

                            _cmd.Parameters.AddWithValue("@op_DATCHIA", new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(Math.Round(s.chiamata.op_DATCHIA / 1000d)).ToLocalTime());
                            _cmd.Parameters.AddWithValue("@op_MATRIC1", s.chiamata.op_MATRIC1);
                            _cmd.Parameters.AddWithValue("@op_NOTE", s.chiamata.op_NOTE);
                            _cmd.Parameters.AddWithValue("@op_NUMCONTR", s.chiamata.op_NUMCONTR);
                            _cmd.Parameters.AddWithValue("@op_OGGETTO", s.chiamata.op_OGGETTO);
                            _cmd.Parameters.AddWithValue("@op_OPINC", s.chiamata.op_OPINC);
                            _cmd.Parameters.AddWithValue("@op_STATUS", s.chiamata.op_STATUS);

                            _cmd.Parameters.Add("@op_CODCHIA", SqlDbType.Decimal, 20).Direction = ParameterDirection.Output;
                            _cmd.ExecuteNonQuery();
                            RisChiamata c = new RisChiamata();
                            c.vecchioCodiceChiamata = s.chiamata.op_CODCHIA;
                            c.nuovoCodiceChiamata = Convert.ToInt32(_cmd.Parameters["@op_CODCHIA"].Value);
                            r.chiamataResult = c;
                            codiceChiamata = c.nuovoCodiceChiamata;

                        }
                        catch (Exception e)
                        {
                            //SENDMAIL
                            log.Info(e.ToString());
                            return Request.CreateResponse(HttpStatusCode.InternalServerError, e.ToString());
                        }
                    }
                }
            }
            else
            {
                //UPDATE CHIAMATA
                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DB_DATI"].ToString()))
                {
                    using (SqlCommand _cmd = new SqlCommand("CSSUP_aggiornaChiamataMobile", connection))
                    {
                        try
                        {
                            _cmd.Connection.Open();
                            _cmd.CommandType = CommandType.StoredProcedure;
                            _cmd.Parameters.AddWithValue("@op_CODCHIA", s.chiamata.op_CODCHIA);
                            _cmd.Parameters.AddWithValue("@op_CODART", s.chiamata.op_CODART);
                            _cmd.Parameters.AddWithValue("@op_CONTO", s.chiamata.op_CONTO);
                            _cmd.Parameters.AddWithValue("@op_CODTCHI", s.chiamata.op_CODTCHI);
                            //  _cmd.Parameters.AddWithValue("@op_DATCHIA", new DateTime(1970, 1, 1).AddMilliseconds((s.chiamata.op_DATCHIA)));
                            _cmd.Parameters.AddWithValue("@op_DATCHIA", new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(Math.Round(s.chiamata.op_DATCHIA / 1000d)).ToLocalTime());

                            _cmd.Parameters.AddWithValue("@op_MATRIC1", s.chiamata.op_MATRIC1);
                            _cmd.Parameters.AddWithValue("@op_NOTE", s.chiamata.op_NOTE);
                            _cmd.Parameters.AddWithValue("@op_NUMCONTR", s.chiamata.op_NUMCONTR);
                            _cmd.Parameters.AddWithValue("@op_OGGETTO", s.chiamata.op_OGGETTO);
                            _cmd.Parameters.AddWithValue("@op_OPINC", s.chiamata.op_OPINC);
                            _cmd.Parameters.AddWithValue("@op_STATUS", s.chiamata.op_STATUS);

                            _cmd.ExecuteNonQuery();
                            RisChiamata c = new RisChiamata();
                            c.vecchioCodiceChiamata  = s.chiamata.op_CODCHIA;
                            c.nuovoCodiceChiamata = s.chiamata.op_CODCHIA;
                            r.chiamataResult = c;
                            codiceChiamata = c.nuovoCodiceChiamata;

                        }
                        catch (Exception e)
                        {
                            //SENDMAIL
                            log.Info(e.ToString());
                            return Request.CreateResponse(HttpStatusCode.InternalServerError, e.ToString());
                        }
                    }
                }
            }

            int rigaDoc = 0;
            foreach (Attivita attivita in s.attivita)
            {
                RisAttivita ris = new RisAttivita();
                rigaDoc++;
                if(attivita.ac_CODATTC <0)
                {
                    //INSERT ATTIVITA
              
                    using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DB_DATI"].ToString()))
                    {
                        using (SqlCommand _cmd = new SqlCommand("CSSUP_inserisciAttivitaMobile", connection))
                        {
                            try
                            {
                                _cmd.Connection.Open();
                                _cmd.CommandType = CommandType.StoredProcedure;

                                _cmd.Parameters.AddWithValue("@ac_CODART",attivita.ac_CODART);
                                _cmd.Parameters.AddWithValue("@ac_CODARTVIA", attivita.ac_CODARTVIA);
                                _cmd.Parameters.AddWithValue("@ac_CODCHIA",codiceChiamata);
                                _cmd.Parameters.AddWithValue("@ac_CODLEAD", attivita.ac_CODLEAD);
                                _cmd.Parameters.AddWithValue("@ac_CODTACO", attivita.ac_CODTACO);
                                _cmd.Parameters.AddWithValue("@ac_CODTCHI", attivita.ac_CODTCHI);
                                //  _cmd.Parameters.AddWithValue("@ac_DATAESEC", new DateTime(1970, 1, 1).AddMilliseconds((attivita.ac_DATAESEC)));
                                _cmd.Parameters.AddWithValue("@ac_DATAESEC", new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(Math.Round(attivita.ac_DATAESEC / 1000d)).ToLocalTime());

                                _cmd.Parameters.AddWithValue("@ac_HHMATRIC1", attivita.ac_HHMATRIC1);
                                _cmd.Parameters.AddWithValue("@ac_NOTE", attivita.ac_NOTE);
                                _cmd.Parameters.AddWithValue("@ac_OGGETTO", attivita.ac_OGGETTO);
                                _cmd.Parameters.AddWithValue("@ac_OPINC", attivita.ac_OPINC);
                                _cmd.Parameters.AddWithValue("@ac_ORAESEC", attivita.ac_ORAESEC);
                                _cmd.Parameters.AddWithValue("@ac_ORAINIZIO", attivita.ac_ORAINIZIO);
                                _cmd.Parameters.AddWithValue("@ac_ORAFINE", attivita.ac_ORAFINE);
                                _cmd.Parameters.AddWithValue("@ac_QUANT", attivita.ac_QUANT);
                                _cmd.Parameters.AddWithValue("@ac_PREZZO", attivita.ac_PREZZO);
                                _cmd.Parameters.AddWithValue("@ac_VALORE", attivita.ac_VALORE);
                                _cmd.Parameters.AddWithValue("@ac_TIPOADDE", attivita.ac_TIPOADDE);
                                _cmd.Parameters.AddWithValue("@ac_PREZZOVIA", attivita.ac_PREZZOVIA);                                
                                _cmd.Parameters.AddWithValue("@ac_QUANTVIA", attivita.ac_QUANTVIA);
                                _cmd.Parameters.AddWithValue("@ac_VALOREVIA", attivita.ac_VALOREVIA);
                                _cmd.Parameters.AddWithValue("@ac_TIPOADDEVIA", attivita.ac_TIPOADDEVIA);
                                _cmd.Parameters.AddWithValue("@ac_STATUSFATT", attivita.ac_STATUSFATT);


                                _cmd.Parameters.AddWithValue("@ac_FIRMA", attivita.ac_FRIMA);

                                //SqlParameter imageParameter = new SqlParameter("@ac_FIRMA", SqlDbType.Image);
                                //imageParameter.Value = DBNull.Value;
                                //_cmd.Parameters.Add(attivita.ac_FRIMA);


                                _cmd.Parameters.Add("@ac_CODATTC", SqlDbType.BigInt, 20).Direction = ParameterDirection.Output;
                                _cmd.ExecuteNonQuery();
                                ris.nuovoCodiceAttivita = Convert.ToInt32(_cmd.Parameters["@ac_CODATTC"].Value);
                                ris.vecchioCodiceAttivita = attivita.ac_CODATTC;
                                r.attivitaResult.Add(ris);


                            }
                            catch (Exception e)
                            {
                                //SENDMAIL
                                log.Info(e.ToString());
                                return Request.CreateResponse(HttpStatusCode.InternalServerError, e.ToString());
                            }
                        }
                    }


                }
                else
                {
                    //UPDATE ATTIVITA

                    using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DB_DATI"].ToString()))
                    {
                        using (SqlCommand _cmd = new SqlCommand("CSSUP_aggiornaAttivitaMobile", connection))
                        {
                            try
                            {
                                _cmd.Connection.Open();
                                _cmd.CommandType = CommandType.StoredProcedure;

                                _cmd.Parameters.AddWithValue("@ac_CODART", attivita.ac_CODART);
                                _cmd.Parameters.AddWithValue("@ac_CODARTVIA", attivita.ac_CODARTVIA);
                                _cmd.Parameters.AddWithValue("@ac_CODCHIA", codiceChiamata);
                                _cmd.Parameters.AddWithValue("@ac_CODLEAD", attivita.ac_CODLEAD);
                                _cmd.Parameters.AddWithValue("@ac_CODTACO", attivita.ac_CODTACO);
                                _cmd.Parameters.AddWithValue("@ac_CODTCHI", attivita.ac_CODTCHI);
                                //_cmd.Parameters.AddWithValue("@ac_DATAESEC", new DateTime(1970, 1, 1).AddMilliseconds((attivita.ac_DATAESEC)));
                                _cmd.Parameters.AddWithValue("@ac_DATAESEC", new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(Math.Round(attivita.ac_DATAESEC / 1000d)).ToLocalTime());

                                _cmd.Parameters.AddWithValue("@ac_FIRMA", attivita.ac_FRIMA);
                                _cmd.Parameters.AddWithValue("@ac_HHMATRIC1", attivita.ac_HHMATRIC1);
                                _cmd.Parameters.AddWithValue("@ac_NOTE", attivita.ac_NOTE);
                                _cmd.Parameters.AddWithValue("@ac_OGGETTO", attivita.ac_OGGETTO);
                                _cmd.Parameters.AddWithValue("@ac_OPINC", attivita.ac_OPINC);
                                _cmd.Parameters.AddWithValue("@ac_ORAESEC", attivita.ac_ORAESEC);
                                _cmd.Parameters.AddWithValue("@ac_ORAINIZIO", attivita.ac_ORAINIZIO);
                                _cmd.Parameters.AddWithValue("@ac_ORAFINE", attivita.ac_ORAFINE);
                                _cmd.Parameters.AddWithValue("@ac_QUANT", attivita.ac_QUANT);
                                _cmd.Parameters.AddWithValue("@ac_PREZZO", attivita.ac_PREZZO);
                                _cmd.Parameters.AddWithValue("@ac_VALORE", attivita.ac_VALORE);
                                _cmd.Parameters.AddWithValue("@ac_TIPOADDE", attivita.ac_TIPOADDE);
                                _cmd.Parameters.AddWithValue("@ac_PREZZOVIA", attivita.ac_PREZZOVIA);
                                _cmd.Parameters.AddWithValue("@ac_QUANTVIA", attivita.ac_QUANTVIA);
                                _cmd.Parameters.AddWithValue("@ac_VALOREVIA", attivita.ac_VALOREVIA);
                                _cmd.Parameters.AddWithValue("@ac_TIPOADDEVIA", attivita.ac_TIPOADDEVIA);
                                _cmd.Parameters.AddWithValue("@ac_STATUSFATT", attivita.ac_STATUSFATT);
                                _cmd.Parameters.AddWithValue("@ac_CODATTC", attivita.ac_CODATTC);


                                ris.nuovoCodiceAttivita = attivita.ac_CODATTC;
                                ris.vecchioCodiceAttivita = attivita.ac_CODATTC;
                                r.attivitaResult.Add(ris);


                            }
                            catch (Exception e)
                            {
                                //SENDMAIL
                                log.Info(e.ToString());
                                return Request.CreateResponse(HttpStatusCode.InternalServerError, e.ToString());
                            }
                        }
                    }

                }
            }

            // INVIO REPORT solo nuove attivita         
            try
            {
                foreach (RisAttivita ki in r.attivitaResult)
                {

                    if (ki.vecchioCodiceAttivita < 0)
                    {
                        ReportDocument rep = new ReportDocument();
                        rep.Load(System.Web.HttpContext.Current.Server.MapPath("~/Report/CSCHIARPT.rpt"));

                        rep.SetDatabaseLogon(WebConfigurationManager.AppSettings["UserId"], WebConfigurationManager.AppSettings["Password"], WebConfigurationManager.AppSettings["ServerName"], WebConfigurationManager.AppSettings["DataBaseName"]);

                        foreach (ReportDocument subRep in rep.Subreports)
                        {
                            subRep.SetDatabaseLogon(WebConfigurationManager.AppSettings["UserId"], WebConfigurationManager.AppSettings["Password"], WebConfigurationManager.AppSettings["ServerName"], WebConfigurationManager.AppSettings["DataBaseName"]);
                        }

                        rep.SetParameterValue("ACCODATTC", ki.nuovoCodiceAttivita);
                        rep.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, "c:/bus/reportattivita/" + r.chiamataResult.nuovoCodiceChiamata.ToString() + "-" + ki.nuovoCodiceAttivita.ToString() + ".pdf");
                        MailMessage message = new MailMessage();
                        message.From = new MailAddress(WebConfigurationManager.AppSettings["MAILSENDER"]);
                        if (WebConfigurationManager.AppSettings["INVIOCLIENTE"].Equals("N"))
                        {
                            message.To.Add(WebConfigurationManager.AppSettings["MAILSENDER"]);

                        }
                        else
                        {
                            using (SqlConnection con3 = new SqlConnection(ConfigurationManager.ConnectionStrings["DB_DATI"].ToString()))
                            {

                                SqlCommand cmd3 = new SqlCommand("SELECT * from ANAGRA WHERE AN_CONTO = @CONTO ", con3);
                                cmd3.Parameters.AddWithValue("@CONTO", x.chiamata.op_CONTO);
                                con3.Open();

                                SqlDataReader reader3 = cmd3.ExecuteReader();
                             
                                while (reader3.Read())
                                {
                                    message.To.Add(reader3["AN_EMAIL"].ToString());
                                }
                            }
                        }
                        message.Attachments.Add(new Attachment(@"C:\bus\reportattivita\" + r.chiamataResult.nuovoCodiceChiamata.ToString() + "-" + ki.nuovoCodiceAttivita.ToString() + ".pdf"));
                        message.Bcc.Add(WebConfigurationManager.AppSettings["EMAIL"]);
                        message.Subject = "REPORT ATTIVITA' N." + ki.nuovoCodiceAttivita.ToString();
                        message.IsBodyHtml = true;
                        message.Body = "in allegato report attività";

                        using (SmtpClient smtpClient = new SmtpClient())
                        {
                            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                            smtpClient.UseDefaultCredentials = false;

                            smtpClient.Host = "smtps.aruba.it";
                            smtpClient.Port = 587;
                            smtpClient.EnableSsl = true;
                            smtpClient.Credentials = new System.Net.NetworkCredential("noreply@deviot.it", "5Eivtl66ye@");

                            smtpClient.Send(message);
                        }
                    }
                }
            }
            catch(Exception e )
            {
                log.Info(e.ToString());
                return Request.CreateResponse(HttpStatusCode.InternalServerError, e.ToString());
            }

            return Request.CreateResponse(HttpStatusCode.OK, r);
        }
    }
}
