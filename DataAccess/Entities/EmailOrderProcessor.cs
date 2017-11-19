using DataAccess.Entities.Base;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Entities
{
    public class EmailSettings
    {
        public string MailToAddress { get; set; } = "myneglectedmail@gmail.com";
        public string MailFromAddress { get; set; } = "myneglectedmail@gmail.com";
        public bool UseSsl { get; set; } = true;
        public string Username { get; set; } = "myneglectedmail@gmail.com";
        public string Password { get; set; } = "g00dluck";
        public string ServerName { get; set; } = "smtp.gmail.com";
        public int ServerPort { get; set; } = 587;
        public bool WriteAsFile { get; set; } = false;
        public string FileLocation { get; set; } = @"d:\";
    }

    public class EmailOrderProcessor : IOrderProcessor
    {
        private EmailSettings emailSettings;

        public EmailOrderProcessor(EmailSettings emailSettings)
        {
            this.emailSettings = emailSettings;
        }

        public void ProcessOrder(Cart cart, ShippingDetails shippingDetails)
        {
            using (var smtpClient = new SmtpClient())
            {
                smtpClient.EnableSsl = emailSettings.UseSsl;
                smtpClient.Host = emailSettings.ServerName;
                smtpClient.Port = emailSettings.ServerPort;
                smtpClient.UseDefaultCredentials = true;
                smtpClient.Credentials = new NetworkCredential(emailSettings.Username, emailSettings.Password);
                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;

                if (emailSettings.WriteAsFile)
                {
                    smtpClient.DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory;
                    smtpClient.PickupDirectoryLocation = emailSettings.FileLocation;
                    smtpClient.EnableSsl = false;
                }

                StringBuilder body = new StringBuilder()
                    .AppendLine("Новый заказ обработан")
                    .AppendLine("----")
                    .AppendLine("Товары");

                foreach (var line in cart.Lines)
                {
                    var subtotal = line.Product.Cost * line.Quantity;
                    body.AppendFormat("{0} x {1} (итого: {2:#.00}руб.)", line.Quantity, line.Product.Name, subtotal).AppendLine("");
                }

                body.AppendFormat("Общая стоимость: {0:#.00}руб.", cart.ComputeTotalValue())
                    .AppendLine("")
                    .AppendLine("---")
                    .AppendLine("Доставка:")
                    .AppendLine(shippingDetails.Name)
                    .AppendLine(shippingDetails.Line1)
                    .AppendLine(shippingDetails.Line2 ?? "")
                    .AppendLine(shippingDetails.Line3 ?? "")
                    .AppendLine(shippingDetails.City)
                    .AppendLine(shippingDetails.Country)
                    .AppendLine("---")
                    .AppendFormat("Подарочная упаковка: {0}", shippingDetails.GiftWrap ? "Да" : "Нет");

                MailMessage mailMessage = new MailMessage(emailSettings.MailFromAddress,
                    emailSettings.MailToAddress, "Новый заказ отправлен!", body.ToString());

                if (emailSettings.WriteAsFile)
                {
                    mailMessage.BodyEncoding = Encoding.UTF8; 
                }

                smtpClient.Send(mailMessage);
            }
        }
    }
}
