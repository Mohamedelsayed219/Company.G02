using System.Net;
using System.Net.Mail;

namespace Company.G02.PL.Helpers
{
    public static class EmailSettings
    {
        public static bool SendEmail(Email email) 
        {
            try 
            {
                var client = new SmtpClient("smtp.gmail.com", 587);
                client.EnableSsl = true;
                client.Credentials = new NetworkCredential("me0043872@gmail.com", "nwisrdyfnqbdsyzo");
                client.Send("me0043872@gmail.com", email.To, email.Subject, email.Body);


                return true;
            } 
            catch (Exception e)
            {
                return false;

            }
        }

    }
}
