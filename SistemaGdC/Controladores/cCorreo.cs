using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Mail;

namespace Controladores
{
    public class cCorreo
    {
        public void enviarCorreo(string direccion, string asunto, string mensaje)
        {
            var origen = new MailAddress("cdag.soporte@gmail.com", "SISCA");
            var destino = new MailAddress(direccion, "To Name");
            const string fromPassword = "SopTec2o18";

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(origen.Address, fromPassword)
            };

            try
            {
                using (var message = new MailMessage(origen, destino)
                {
                    Subject = asunto,
                    Body = mensaje
                })
                {
                    smtp.Send(message);
                }
                //return true;
            }
            catch
            {
                //return false;
            }
            
        }
    }
}
