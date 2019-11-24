using System;
using System.Text;
using System.Security.Cryptography;


namespace digitalsignatur
{
    public class DigitalSignaturData
    {
        public string PublicKey { get; set; }

        public string Signatur { get; set; }

        public string Text { get; set; }
    }

    public static class DigitalSignatur
    {
        public static DigitalSignaturData Sign(string text, RSACryptoServiceProvider algorithm = null)
        {
            var signer = algorithm ?? new RSACryptoServiceProvider();
            byte[] data = Encoding.UTF8.GetBytes(text);
            byte[] signature = signer.SignData(data, new SHA1CryptoServiceProvider());

            return new DigitalSignaturData
            {
                PublicKey = signer.ToXmlString(false),
                Signatur = Convert.ToBase64String(signature),
                Text = text
            };
        }

        public static bool Validate(DigitalSignaturData digitalSignaturData, RSACryptoServiceProvider algorithm = null)
        {
            var verifier = algorithm ?? new RSACryptoServiceProvider();
            verifier.FromXmlString(digitalSignaturData.PublicKey);
            byte[] receivedData = Encoding.UTF8.GetBytes(digitalSignaturData.Text);

            return verifier.VerifyData(receivedData, new SHA1CryptoServiceProvider(), Convert.FromBase64String(digitalSignaturData.Signatur));
        }
    }
}