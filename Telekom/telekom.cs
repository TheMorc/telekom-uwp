using Microsoft.Toolkit.Uwp.Notifications;
using Newtonsoft.Json.Linq;
using System;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Windows.Storage;
using Windows.UI.Notifications;
using Windows.UI.Popups;

namespace Telekom
{
    public class Telekom
    {
        public SplashScreen splashScreen = null;
        public ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
        public Guid deviceId = Windows.Storage.Streams.DataReader.FromBuffer(Windows.System.Profile.SystemIdentification.GetSystemIdForPublisher().Id).ReadGuid();
        private string nonce = "";
        public string accessToken = "";
        public string refreshToken = "";
        public string productId = "";
        public string productLabel = "";
        public string productName = "";
        public string fullName, givenName, familyName = "";
        public double maxGB, remainingGB = 0;
        public long serviceId = 0;
        public string lastError = "";
        public string lastCode = "";
        public int unpaidBillsCount = 0;
        public string unpaidBillsCurrency = "";
        public double unpaidBillsAmount = 0;
        public string telekom_username = "";
        internal static HttpClient httpClient = new HttpClient();

        public async Task ShowError()
        {
            MessageDialog messageDialog = new MessageDialog(App.TLKM.lastError + "\n" + App.TLKM.lastCode);
            messageDialog.Commands.Add(new UICommand("Quit"));
            messageDialog.DefaultCommandIndex = 0;
            messageDialog.CancelCommandIndex = 0;

            await messageDialog.ShowAsync();
        }

        public bool Update_LiveTile()
        {
            string from = productLabel;
            string subject = remainingGB + "/" + maxGB + "GB";


            TileContent content = new TileContent()
            {
                Visual = new TileVisual()
                {
                    Branding = TileBranding.NameAndLogo,

                    TileMedium = new TileBinding()
                    {
                        Content = new TileBindingContentAdaptive()
                        {
                            Children =
                            {
                                new AdaptiveText()
                                {
                                    Text = from,
                                    HintStyle = AdaptiveTextStyle.Base
                                },

                                new AdaptiveText()
                                {
                                    Text = subject,
                                    HintStyle = AdaptiveTextStyle.CaptionSubtle
                            }
                                }
                        }
                    },

                    TileWide = new TileBinding()
                    {
                        Content = new TileBindingContentAdaptive()
                        {
                            Children =
                            {
                                new AdaptiveText()
                                {
                                    Text = from,
                                    HintStyle = AdaptiveTextStyle.Base
                                },

                                new AdaptiveText()
                                {
                                    Text = subject,
                                    HintStyle = AdaptiveTextStyle.CaptionSubtle
                                }
                            }
                        }
                    }
                }
            };

            TileNotification notification = new TileNotification(content.GetXml());
            TileUpdateManager.CreateTileUpdaterForApplication().Update(notification);

            return true;
        }

        public async Task<bool> ProductReport()
        {
            using (HttpRequestMessage request = new HttpRequestMessage(new HttpMethod("GET"), "https://t-app.telekom.sk/manageServices/product/" + productId + "/details?checkCancelEligibility=false&devicesWithEMI=false&disableDocumentManagement=true&enableExtraData=false&enableFreeUnit=true&enableVasCategories=false&profileId=MSISDN_" + serviceId + "&serviceOnboarding=false&serviceOutageEnabled=false&subscriptionServiceEnabled=false&swapEnabled=false&tariffOfferEnable=true&transferUnitsEnabled=false&vasDelay=true"))
            {
                request.Headers.TryAddWithoutValidation("Accept", "*/*");
                request.Headers.TryAddWithoutValidation("Authorization", "Bearer " + accessToken);
                request.Headers.TryAddWithoutValidation("X-Client-Version", "18.8.2 (887) 2-78c3ec0 (HEAD)");
                request.Headers.TryAddWithoutValidation("X-Request-Session-Id", "FC4DF625-01D0-4ACC-A8E5-5260A3F9AC7F");
                request.Headers.TryAddWithoutValidation("X-Request-Tracking-Id", "FFFFFFFF-FFFF-FFFF-FFFF-FFFFFFFFFFFF");

                HttpResponseMessage response = await httpClient.SendAsync(request);
                dynamic json = JObject.Parse(response.Content.ReadAsStringAsync().Result);
                if (json.errorType != null)
                {
                    string errorMessage = "[tlkm_main - productreport] " + json.message + " " + json.code;
                    Debug.WriteLine(errorMessage);
                    lastCode = json.code;
                    lastError = json.message;
                    return false;
                }

                Debug.WriteLine("[tlkm_main - productreport] ");
                Debug.WriteLine(response.Content.ReadAsStringAsync().Result);

                return true;
            }
        }

        public async Task<bool> Dashboard()
        {
            using (HttpRequestMessage request = new HttpRequestMessage(new HttpMethod("GET"), "https://t-app.telekom.sk/dashboard/product/" + productId + "?enableFreeUnit=true&priority=primary&profileId=MSISDN_" + serviceId + "&serviceOnboarding=false&serviceOutageEnabled=false&showTotalCreditBalance=true&showUnlimited=true"))
            {
                request.Headers.TryAddWithoutValidation("Accept", "*/*");
                request.Headers.TryAddWithoutValidation("Authorization", "Bearer " + accessToken);
                request.Headers.TryAddWithoutValidation("X-Client-Version", "18.8.2 (887) 2-78c3ec0 (HEAD)");
                request.Headers.TryAddWithoutValidation("X-Request-Session-Id", "FC4DF625-01D0-4ACC-A8E5-5260A3F9AC7F");
                request.Headers.TryAddWithoutValidation("X-Request-Tracking-Id", "FFFFFFFF-FFFF-FFFF-FFFF-FFFFFFFFFFFF");

                HttpResponseMessage response = await httpClient.SendAsync(request);
                dynamic json = JObject.Parse(response.Content.ReadAsStringAsync().Result);
                if (json.errorType != null)
                {
                    string errorMessage = "[tlkm_main - dashboard] " + json.message + " " + json.code;
                    Debug.WriteLine(errorMessage);
                    lastCode = json.code;
                    lastError = json.message;
                    return false;
                }

                productName = json.campaignPlanDetail.name;
                maxGB = json.consumption.max.value;
                remainingGB = json.consumption.remaining.value;
                Debug.WriteLine("[tlkm_main - dashboard] " + productName + " - max: " + maxGB + "GB remaining: " + remainingGB + "GB");

                return true;
            }
        }

        public async Task<bool> Unpaid_Bills()
        {
            using (HttpRequestMessage request = new HttpRequestMessage(new HttpMethod("GET"), "https://t-app.telekom.sk/customerBills/unpaid/summary/"))
            {
                request.Headers.TryAddWithoutValidation("Accept", "*/*");
                request.Headers.TryAddWithoutValidation("Authorization", "Bearer " + accessToken);
                request.Headers.TryAddWithoutValidation("X-Client-Version", "18.8.2 (887) 2-78c3ec0 (HEAD)");
                request.Headers.TryAddWithoutValidation("X-Request-Session-Id", "FC4DF625-01D0-4ACC-A8E5-5260A3F9AC7F");
                request.Headers.TryAddWithoutValidation("X-Request-Tracking-Id", "FFFFFFFF-FFFF-FFFF-FFFF-FFFFFFFFFFFF");

                HttpResponseMessage response = await httpClient.SendAsync(request);
                dynamic json = JObject.Parse(response.Content.ReadAsStringAsync().Result);
                if (json.errorType != null)
                {
                    string errorMessage = "[tlkm_main - unpaid_bills] " + json.message + " " + json.code;
                    Debug.WriteLine(errorMessage);
                    lastCode = json.code;
                    lastError = json.message;
                    return false;
                }

                unpaidBillsAmount = json.cost.amount;
                unpaidBillsCount = json.count;
                unpaidBillsCurrency = json.cost.currencyCode;
                Debug.WriteLine("[tlkm_main - unpaid_bills] count:" + unpaidBillsCount + " - amount: " + unpaidBillsAmount + unpaidBillsCurrency);

                return true;
            }
        }

        public async Task<bool> PatchProfile(string simLabel, string firstName, string lastName, string contactTel)
        {
            if (productLabel != simLabel)
            {

            }
            if (givenName != firstName)
            {

            }
            if (familyName != lastName)
            {
            }
            if (serviceId != long.Parse(contactTel.Remove(0, 1)))
            {

            }

            // TODO: implement PATCH

            return false;
        }

        #region pin&verif and login + regen_token
        public async Task<bool> Regen_token()
        {

            using (HttpRequestMessage request = new HttpRequestMessage(new HttpMethod("POST"), "https://t-app.telekom.sk/token/"))
            {
                request.Headers.TryAddWithoutValidation("Accept", "*/*");
                request.Headers.TryAddWithoutValidation("Authorization", "7d06dd59-687c-454e-ade8-3520ff79a00d");
                request.Headers.TryAddWithoutValidation("X-Request-Session-Id", "188C1694-71FC-4CA6-B013-579755690106");
                request.Headers.TryAddWithoutValidation("X-Request-Tracking-Id", "FFFFFFFF-FFFF-FFFF-FFFF-FFFFFFFFFFFF");

                request.Content = new StringContent("{\"genCenToken\":false,\"refreshToken\":\"" + refreshToken + "\",\"deviceId\":\"" + deviceId + "\"}");
                request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");

                HttpResponseMessage response = await httpClient.SendAsync(request);

                dynamic json = JObject.Parse(response.Content.ReadAsStringAsync().Result);
                if (json.errorType != null)
                {
                    string errorMessage = "[tlkm_main - regen_token] " + json.message + " " + json.code;
                    Debug.WriteLine(errorMessage);
                    lastCode = json.code;
                    lastError = json.message;
                    return false;
                }

                Debug.WriteLine("[tlkm_main - regen_token] saving accessToken");
                accessToken = json.accessToken;
                App.TLKM.localSettings.Values["accessToken"] = accessToken;

                Debug.WriteLine("[tlkm_main - regen_token] saving refreshToken");
                refreshToken = json.refreshToken;
                App.TLKM.localSettings.Values["refreshToken"] = refreshToken;

                return true;
            }
        }

        public async Task<bool> Login()
        {
            //this function is not invoked until the variables are not populated
            //this should not happen in any other order.


            Debug.WriteLine("[tlkm_main - login] input number" + serviceId);

            using (HttpRequestMessage request = new HttpRequestMessage(new HttpMethod("GET"), "https://t-app.telekom.sk/profiles/?deviceId=" + deviceId + "&devicesWithEMI=false&genCenToken=true&hybridEnabled=true&loyaltyEnabled=false&sub=MSISDN_" + serviceId + "&subscriptionServiceEnabled=false"))
            {
                request.Headers.TryAddWithoutValidation("Accept", "*/*");
                request.Headers.TryAddWithoutValidation("Authorization", "Bearer " + accessToken);
                request.Headers.TryAddWithoutValidation("X-Request-Session-Id", "FC4DF625-01D0-4ACC-A8E5-5260A3F9AC7F");
                request.Headers.TryAddWithoutValidation("X-Request-Tracking-Id", "FFFFFFFF-FFFF-FFFF-FFFF-FFFFFFFFFFFF");

                HttpResponseMessage response = null;

                try
                {
                    response = await httpClient.SendAsync(request);
                }
                catch
                {
                    Debug.WriteLine("[tlkm_main - login] seems like we dont have an internet connection");
                    lastCode = "nointernet";
                    lastError = "neni internet";
                    return false;
                }

                string semiParsedJson = response.Content.ReadAsStringAsync().Result;

                if (semiParsedJson.StartsWith("["))
                {
                    semiParsedJson = response.Content.ReadAsStringAsync().Result.Remove(0, 2).Remove(response.Content.ReadAsStringAsync().Result.Length - 3, 1);
                }

                dynamic json = JObject.Parse(semiParsedJson);
                JObject jobj = JObject.Parse(semiParsedJson);

                // TODO: implement a better json parser, this method sucks quite a bit

                if (json.errorType != null)
                {
                    string errorMessage = "[tlkm_main - login] " + json.message + " " + json.code;
                    Debug.WriteLine(errorMessage);
                    lastCode = json.code;
                    lastError = json.message;
                    return false;
                }

                System.Collections.Generic.IEnumerable<string> product = from p in jobj["manageableAssets"] select (string)p["id"];
                foreach (string item in product)
                {
                    Debug.WriteLine("[tlkm_main - login] productId: " + item);
                    productId = item;
                }

                System.Collections.Generic.IEnumerable<string> label = from p in jobj["manageableAssets"] select (string)p["label"];
                foreach (string item in label)
                {
                    Debug.WriteLine("[tlkm_main - login] productLabel: " + item);
                    productLabel = item;
                }

                System.Collections.Generic.IEnumerable<string> username = from p in jobj["characteristics"] select (string)p["value"];
                foreach (string item in username)
                {
                    if (item == "B2C")
                    {
                        break;
                    }

                    Debug.WriteLine("[tlkm_main - login] telekom_username: " + item);
                    telekom_username = item;
                }

                // TODO: parse email
                //System.Collections.Generic.IEnumerable<string> contactMediums_email = from p in jobj["contactMediums"] select (string)p["medium"];
                //foreach (string item in contactMediums_email)
                //{
                //  Debug.WriteLine("[tlkm_main - login] email: " + item);
                //email = item;
                //}

                fullName = json.individual.givenName + " " + json.individual.familyName;
                givenName = json.individual.givenName;
                familyName = json.individual.familyName;
                Debug.WriteLine("[tlkm_main - login] fullName: " + fullName);

                return true;
            }
        }

        public Task<bool> Pin(long parsedNumber)
        {
            Debug.WriteLine("[tlkm_main] PIN for " + parsedNumber);

            using (HttpRequestMessage request = new HttpRequestMessage(new HttpMethod("POST"), "https://t-app.telekom.sk/pin/"))
            {
                request.Headers.TryAddWithoutValidation("Authorization", "7d06dd59-687c-454e-ade8-3520ff79a00d");
                request.Headers.TryAddWithoutValidation("X-Request-Session-Id", "FC4DF625-01D0-4ACC-A8E5-5260A3F9AC7F");
                request.Headers.TryAddWithoutValidation("X-Request-Tracking-Id", "FFFFFFFF-FFFF-FFFF-FFFF-FFFFFFFFFFFF");

                request.Content = new StringContent("{\"serviceId\":\"+" + parsedNumber + "\",\"serviceType\":\"phoneNumber\",\"device\":{\"os\":\"ios\"},\"context\":\"login\"}");
                request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");

                Task<HttpResponseMessage> response = httpClient.SendAsync(request);

                dynamic json = JObject.Parse(response.Result.Content.ReadAsStringAsync().Result);
                if (json.errorType != null)
                {
                    string errorMessage = "[tlkm_main - pin] " + json.message + " " + json.code;
                    Debug.WriteLine(errorMessage);
                    lastCode = json.code;
                    lastError = json.message;
                    return Task.FromResult(false);
                }
                string jsonNonce = json.nonce;
                nonce = jsonNonce;
                serviceId = parsedNumber;

                return Task.FromResult(true);
            }
        }

        public async Task<bool> Verif(long PIN)
        {
            Debug.WriteLine("[tlkm_main] PIN verif input " + PIN);

            using (HttpRequestMessage request = new HttpRequestMessage(new HttpMethod("PUT"), "https://t-app.telekom.sk/pin/"))
            {
                request.Headers.TryAddWithoutValidation("Accept", "*/*");
                request.Headers.TryAddWithoutValidation("Authorization", "7d06dd59-687c-454e-ade8-3520ff79a00d");
                request.Headers.TryAddWithoutValidation("X-Request-Session-Id", "FC4DF625-01D0-4ACC-A8E5-5260A3F9AC7F");
                request.Headers.TryAddWithoutValidation("X-Request-Tracking-Id", "FFFFFFFF-FFFF-FFFF-FFFF-FFFFFFFFFFFF");

                request.Content = new StringContent("{\"device\":{\"id\":\"" + deviceId + "\",\"os\":\"ios\"},\"enableProfilePin\":false,\"serviceId\":\"+" + serviceId + "\",\"serviceType\":\"phoneNumber\",\"context\":\"login\",\"PIN\":\"" + PIN + "\",\"nonce\":\"" + nonce + "\"}");
                request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");

                HttpResponseMessage response = await httpClient.SendAsync(request);

                dynamic json = JObject.Parse(response.Content.ReadAsStringAsync().Result);
                if (json.errorType != null)
                {
                    string errorMessage = "[tlkm_main - verif]" + json.message + " " + json.code;
                    Debug.WriteLine(errorMessage);
                    lastCode = json.code;
                    lastError = json.message;
                    return false;
                }

                Debug.WriteLine("[tlkm_main - verif] saving deviceId");
                App.TLKM.localSettings.Values["deviceId"] = deviceId;

                Debug.WriteLine("[tlkm_main - verif] saving accessToken");
                accessToken = json.accessToken;
                App.TLKM.localSettings.Values["accessToken"] = accessToken;

                Debug.WriteLine("[tlkm_main - verif] saving refreshToken");
                refreshToken = json.refreshToken;
                App.TLKM.localSettings.Values["refreshToken"] = refreshToken;

                Debug.WriteLine("[tlkm_main - verif] saving serviceId");
                App.TLKM.localSettings.Values["serviceId"] = serviceId;

                Debug.WriteLine("[tlkm_main - verif] saving hasAccount");
                App.TLKM.localSettings.Values["hasAccount"] = "yes";


                return true;
            }
        }
        #endregion
    }
}
