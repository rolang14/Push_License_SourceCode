using WebSocketSharp;
using Newtonsoft.Json.Linq;
using DeviceLib;

namespace ConnectLib
{
    public class Connect
    {
        private static Connect instance;
        public static Connect Instance
        {
            get
            {
                if (instance == null)
                    instance = new Connect();
                return instance;
            }
        }

        private readonly string wsAddr = RES.SERV_WS_CONFIG;
        public readonly string t_timeout_str = RES.TSK_TIME_OUT;
        public readonly string c_timeout_str = RES.CONN_TIME_OUT;
        private readonly Device devlib = new Device();

        public async Task<string> _wsLoginSend(string id, string password)
        {
            // wait for task res completed.
            var tcs = new TaskCompletionSource<string>();
            // wait till timeout then err.
            var cts = new CancellationTokenSource();
            string result = "false";

            cts.CancelAfter(500);

            try
            {
                using (var ws = new WebSocket(wsAddr))
                {
                    ws.OnOpen += (sender, e) =>
                    {
                        var jsonLoginData = new JObject();  // JSON 객체 생성

                        string extipAddress = devlib.GetExtIPAddress();
                        string intipaddress = devlib.GetIPAddress();

                        jsonLoginData.Add("msg", "InternalLoginCheck");
                        jsonLoginData.Add("email", id);
                        jsonLoginData.Add("password", password);
                        jsonLoginData.Add("eip", extipAddress);
                        jsonLoginData.Add("iip", intipaddress);

                        devlib.IgnoreBadCertificates();

                        ws.Send(jsonLoginData.ToString());
                    };

                    ws.OnMessage += (sender, e) =>
                    {
                        JObject objJson = JObject.Parse(e.Data);
                        result = objJson["login_res"]?.ToString();
                        tcs.TrySetResult(result);
                    };

                    var connectTask = Task.Run(() => ws.Connect());
                    // If CTS Token exists, Delay 500 can be canceled
                    // But CTS Token not exsists, Delay 500 must be passed eventhough the Task is already done !
                    if (await Task.WhenAny(connectTask, Task.Delay(500, cts.Token)) == connectTask)
                    {
                        result = await tcs.Task;
                    }
                    else
                    {
                        cts.Cancel();
                        throw new TimeoutException();
                    }

                }
            }
            catch (TaskCanceledException te)
            {
                return t_timeout_str;
            }
            catch (TimeoutException te)
            {
                return c_timeout_str;
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }

            return result;
        }

        public async Task<string[]> _wsCheckCompanyCode(string companyCode, string licenseType)
        {
            var tcs = new TaskCompletionSource<string[]>();
            var cts = new CancellationTokenSource();
            string[] res = new string[2];

            cts.CancelAfter(500);

            try
            {
                using (var ws = new WebSocket(wsAddr))
                {
                    ws.OnOpen += (sender, e) =>
                    {
                        var jsonCompanyCodeData = new JObject();  // JSON 객체 생성

                        jsonCompanyCodeData.Add("msg", "InternalCompanyCodeCheck");
                        jsonCompanyCodeData.Add("company_code", companyCode);
                        jsonCompanyCodeData.Add("licensetype", licenseType);

                        devlib.IgnoreBadCertificates();

                        ws.Send(jsonCompanyCodeData.ToString());
                    };

                    ws.OnMessage += (sender, e) =>
                    {
                        JObject objJson = JObject.Parse(e.Data);
                        res[0] = objJson["companyCode"]?.ToString();
                        res[1] = objJson["licenseKey"]?.ToString();
                        tcs.TrySetResult(res);
                    };

                    var connectTask = Task.Run(() => ws.Connect());
                    // If CTS Token exists, Delay 500 can be canceled
                    // But CTS Token not exsists, Delay 500 must be proceeded eventhough the Task is already done !
                    if (await Task.WhenAny(connectTask, Task.Delay(500, cts.Token)) == connectTask)
                    {
                        res = await tcs.Task;
                    }
                    else
                    {
                        cts.Cancel();
                        throw new TimeoutException();
                    }

                }
            }
            catch (TaskCanceledException te)
            {
                return new string[] { t_timeout_str};
            }
            catch (TimeoutException te)
            {
                return new string[] { c_timeout_str };
            }
            catch (Exception ex)
            {
                return new string[] { ex.ToString() };
            }

            return res;
        }

        public async Task<string> _wsCheckLicenseKey(string companyCode, string licenseKey, string licenseType)
        {
            var tcs = new TaskCompletionSource<string>();
            var cts = new CancellationTokenSource();
            string res;

            cts.CancelAfter(500);

            try
            {
                using (var ws = new WebSocket(wsAddr))
                {
                    ws.OnOpen += (sender, e) =>
                    {
                        var jsonCompanyCodeData = new JObject();  // JSON 객체 생성

                        jsonCompanyCodeData.Add("msg", "InternalLicenseKeyCheck");
                        jsonCompanyCodeData.Add("company_code", companyCode);
                        jsonCompanyCodeData.Add("license_key", licenseKey);
                        jsonCompanyCodeData.Add("licensetype", licenseType);

                        devlib.IgnoreBadCertificates();

                        ws.Send(jsonCompanyCodeData.ToString());
                    };

                    ws.OnMessage += (sender, e) =>
                    {
                        JObject objJson = JObject.Parse(e.Data);
                        res = objJson["licenseKey"]?.ToString();
                        tcs.TrySetResult(res);
                    };

                    var connectTask = Task.Run(() => ws.Connect());
                    // If CTS Token exists, Delay 500 can be canceled
                    // But CTS Token not exsists, Delay 500 must be proceeded eventhough the Task is already done !
                    if (await Task.WhenAny(connectTask, Task.Delay(500, cts.Token)) == connectTask)
                    {
                        res = await tcs.Task;
                    }
                    else
                    {
                        cts.Cancel();
                        throw new TimeoutException();
                    }

                }
            }
            catch (TaskCanceledException te)
            {
                return t_timeout_str;
            }
            catch (TimeoutException te)
            {
                return c_timeout_str;
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }

            return res;
        }

        public async Task<string[]> _wsLicenseInput(string companyCode, string licenseKey, int licenseCount, string start, string end, string licenseType)
        {
            var tcs = new TaskCompletionSource<string[]>();
            var cts = new CancellationTokenSource();
            string[] res = new string[4];

            cts.CancelAfter(500);

            try
            {
                using (var ws = new WebSocket(wsAddr))
                {
                    ws.OnOpen += (sender, e) =>
                    {
                        var jsonLoginData = new JObject();  // JSON 객체 생성

                        string extipAddress = devlib.GetExtIPAddress();
                        string intipaddress = devlib.GetIPAddress();

                        jsonLoginData.Add("msg", "InternalLicenseInput");
                        jsonLoginData.Add("company_code", companyCode);
                        jsonLoginData.Add("license_key", licenseKey);
                        jsonLoginData.Add("license_count", licenseCount);
                        jsonLoginData.Add("start", start);
                        jsonLoginData.Add("end", end);
                        jsonLoginData.Add("licensetype", licenseType);

                        devlib.IgnoreBadCertificates();

                        ws.Send(jsonLoginData.ToString());
                    };

                    ws.OnMessage += (sender, e) =>
                    {
                        JObject objJson = JObject.Parse(e.Data);
                        res[0] = objJson["companyCode"]?.ToString();
                        res[1] = objJson["ccresult"]?.ToString();
                        res[2] = objJson["licenseKey"]?.ToString();
                        res[3] = objJson["lkresult"]?.ToString();
                        tcs.TrySetResult(res);
                    };

                    var connectTask = Task.Run(() => ws.Connect());
                    // If CTS Token exists, Delay 500 can be canceled
                    // But CTS Token not exsists, Delay 500 must be passed eventhough the Task is already done !
                    if (await Task.WhenAny(connectTask, Task.Delay(500, cts.Token)) == connectTask)
                    {
                        res = await tcs.Task;
                    }
                    else
                    {
                        cts.Cancel();
                        throw new TimeoutException();
                    }

                }
            }
            catch (TaskCanceledException te)
            {
                return new string[] { t_timeout_str };
            }
            catch (TimeoutException te)
            {
                return new string[] { c_timeout_str };
            }
            catch (Exception ex)
            {
                return new string[] { ex.ToString() };
            }

            return res;
        }
    }
}