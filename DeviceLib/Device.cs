using System.Net;

namespace DeviceLib
{
    public class Device
    {
        private readonly string ALLOWED_EXT_IP = RES.EXT_IP;
        private readonly string[] ALLOWED_INT_IP = { RES.INT_IP1, RES.INT_IP2, RES.INT_IP3 };

        //내부IP검색
        public string GetIPAddress()
        {
            IPHostEntry GetIP = Dns.GetHostEntry(Dns.GetHostName());

            string ipAddr = string.Empty;

            for (int i = 0; i < GetIP.AddressList.Length; i++)
            {
                if (GetIP.AddressList[i].AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    ipAddr = GetIP.AddressList[i].ToString(); // IPv4에 해당되는 IP 

                    break;
                }
            }
            return ipAddr;
        }

        //공인IP검색
        public string GetExtIPAddress()
        {
            try
            {
                string ipAddr = new WebClient().DownloadString("http://ipinfo.io/ip").Trim();
                //못가져오면 null
                if (String.IsNullOrEmpty(ipAddr))
                    ipAddr = null;

                return ipAddr;
            }
            catch
            {
                return null;
            }
        }

        public bool CheckIP()
        {
            string extip = GetExtIPAddress();
            string intip = GetIPAddress();
            if (extip != ALLOWED_EXT_IP)
                return false;
            foreach (var ip in ALLOWED_INT_IP)
            {
                if (ip == intip)
                    return true;
            }
            return false;
        }

        //인증서 무시
        public void IgnoreBadCertificates()
        {
            System.Net.ServicePointManager.ServerCertificateValidationCallback
             = new System.Net.Security.RemoteCertificateValidationCallback(AcceptAllCertifications);
        }
        private static bool AcceptAllCertifications(object sender, System.Security.Cryptography.X509Certificates.X509Certificate certification,
            System.Security.Cryptography.X509Certificates.X509Chain chain, System.Net.Security.SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }
    }
}