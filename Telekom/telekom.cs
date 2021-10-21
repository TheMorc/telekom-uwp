using Microsoft.Toolkit.Uwp.Notifications;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Diagnostics;
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
        public string fullName = "";
        public long serviceId = 0;
        public string lastError = "";
        public string lastCode = "";

        public JSON_.ProductReport prodRep = null;
        public JSON_.UnpaidBills unpaidBills = null;
        public JSON_.Login login = null;

        internal static HttpClient httpClient = new HttpClient();

        public async Task ShowMessage()
        {
            MessageDialog messageDialog = new MessageDialog(App.TLKM.lastError + "\n" + App.TLKM.lastCode);
            messageDialog.Commands.Add(new UICommand("OK"));
            messageDialog.DefaultCommandIndex = 0;
            messageDialog.CancelCommandIndex = 0;

            await messageDialog.ShowAsync();
            App.TLKM.lastCode = "";
            App.TLKM.lastError = "";
        }

        public bool Update_LiveTile()
        {
            string from = prodRep.Label;
            string subject = prodRep.ConsumptionGroups[0].Consumptions[0].Remaining.Value + "/" + prodRep.ConsumptionGroups[0].Consumptions[0].Max.Value + "GB"; //assuming that data is first on the list..


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

        #region overview
        public async Task<bool> ProductReport()
        {
            using (HttpRequestMessage request = new HttpRequestMessage(new HttpMethod("GET"), "https://t-app.telekom.sk/manageServices/product/" + login.ManageableAssets[0].Id + "/details?checkCancelEligibility=false&devicesWithEMI=false&disableDocumentManagement=true&enableExtraData=false&enableFreeUnit=true&enableVasCategories=false&profileId=MSISDN_" + serviceId + "&serviceOnboarding=false&serviceOutageEnabled=false&subscriptionServiceEnabled=false&swapEnabled=false&tariffOfferEnable=true&transferUnitsEnabled=false&vasDelay=true"))
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

                prodRep = JsonConvert.DeserializeObject<JSON_.ProductReport>(response.Content.ReadAsStringAsync().Result); //populating the public prodRep variable with valuable items!
                Debug.WriteLine("[tlkm_main - productreport] " + prodRep.Label + " - max: " + prodRep.ConsumptionGroups[0].Consumptions[0].Max.Value + "GB remaining: " + prodRep.ConsumptionGroups[0].Consumptions[0].Remaining.Value + "GB");

                return true;
            }
        }

        //i don't think i'll ever use dashboard again since productreport includes all information
        //but i'll leave it here
        public async Task<bool> Dashboard()
        {
            using (HttpRequestMessage request = new HttpRequestMessage(new HttpMethod("GET"), "https://t-app.telekom.sk/dashboard/product/" + login.ManageableAssets[0].Id + "?enableFreeUnit=true&priority=primary&profileId=MSISDN_" + serviceId + "&serviceOnboarding=false&serviceOutageEnabled=false&showTotalCreditBalance=true&showUnlimited=true"))
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

                /*productName = json.campaignPlanDetail.name;
                maxGB = json.consumption.max.value;
                remainingGB = json.consumption.remaining.value;
                Debug.WriteLine("[tlkm_main - dashboard] " + productName + " - max: " + maxGB + "GB remaining: " + remainingGB + "GB");*/

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

                unpaidBills = JsonConvert.DeserializeObject<JSON_.UnpaidBills>(response.Content.ReadAsStringAsync().Result);
                Debug.WriteLine("[tlkm_main - unpaid_bills] count:" + unpaidBills.Count.Value + " - amount: " + unpaidBills.Cost.Amount.Value + unpaidBills.Cost.CurrencyCode);

                return true;
            }
        }
        #endregion


        public async Task<bool> PatchProfile(string simLabel, string firstName, string lastName, string contactTel, string email)
        {
            string patchURL = "https://t-app.telekom.sk/profiles/MSISDN_" + serviceId + "?fields=";
            string patchJSON = "{"; //i can't quite figure out how to serialize a json properly with arrays..

            if (prodRep.Label != simLabel)
            {
                string patchSIMLabel = "{\"label\": \"" + simLabel + "\"}";
                using (HttpRequestMessage request = new HttpRequestMessage(new HttpMethod("PATCH"), "https://t-app.telekom.sk/profiles/MSISDN_" + serviceId + "/manageableAssets/" + login.ManageableAssets[0].Id + "?fields=label"))
                {
                    request.Headers.TryAddWithoutValidation("Accept", "*/*");
                    request.Headers.TryAddWithoutValidation("Authorization", "Bearer " + accessToken);
                    request.Headers.TryAddWithoutValidation("X-Request-Session-Id", "188C1694-71FC-4CA6-B013-579755690106");
                    request.Headers.TryAddWithoutValidation("X-Request-Tracking-Id", "FFFFFFFF-FFFF-FFFF-FFFF-FFFFFFFFFFFF");

                    request.Content = new StringContent(patchSIMLabel);
                    request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");

                    HttpResponseMessage response = await httpClient.SendAsync(request);

                    dynamic json = JObject.Parse(response.Content.ReadAsStringAsync().Result);
                    if (json.errorType != null)
                    {
                        string errorMessage = "[tlkm_main - patchsimlabel] " + json.message + " " + json.code;
                        Debug.WriteLine(errorMessage);
                        lastCode = json.code;
                        lastError = json.message;
                        return false;
                    }

                    JSON_.PatchSIMResult result = JsonConvert.DeserializeObject<JSON_.PatchSIMResult>(response.Content.ReadAsStringAsync().Result);
                    if (result.Label == simLabel)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }

            if (login.Individual.GivenName != firstName || login.Individual.FamilyName != lastName)
            {
                patchURL = patchURL + "individual%2C";
                patchJSON = patchJSON + "\"individual\": { \"givenName\": \"" + firstName + "\", \"familyName\": \"" + lastName + "\"},";

            }
            if (contactTel.Remove(0, 1) != serviceId.ToString() || login.ContactMediums[1].Medium.EmailAddress != email)
            {
                patchURL = patchURL + "contactMediums%2C";
                patchJSON = patchJSON + "\"contactMediums\": [{\"medium\": {\"number\": \"" + contactTel + "\"},\"role\": {\"name\": \"contact\"},\"type\": \"mobile\"},{\"medium\": {\"emailAddress\": \"" + email + "\"},\"role\": {\"name\": \"contact\"},\"type\": \"email\"}]";
            }
            if (patchJSON.EndsWith(","))
            {
                patchJSON = patchJSON.Remove(patchJSON.Length - 1, 1) + "}";
            }
            else if (patchJSON.EndsWith("{"))
            {
                App.TLKM.lastError = App.resourceLoader.GetString("Profile/NoChanges");
                return false;
            }
            if (patchURL.EndsWith("%2C"))
            {
                patchURL = patchURL.Remove(patchURL.Length - 3, 3);
            }


            using (HttpRequestMessage request = new HttpRequestMessage(new HttpMethod("PATCH"), patchURL))
            {
                request.Headers.TryAddWithoutValidation("Accept", "*/*");
                request.Headers.TryAddWithoutValidation("Authorization", "Bearer " + accessToken);
                request.Headers.TryAddWithoutValidation("X-Request-Session-Id", "188C1694-71FC-4CA6-B013-579755690106");
                request.Headers.TryAddWithoutValidation("X-Request-Tracking-Id", "FFFFFFFF-FFFF-FFFF-FFFF-FFFFFFFFFFFF");

                request.Content = new StringContent(patchJSON);
                request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");

                HttpResponseMessage response = await httpClient.SendAsync(request);

                dynamic json = JObject.Parse(response.Content.ReadAsStringAsync().Result);
                if (json.errorType != null)
                {
                    string errorMessage = "[tlkm_main - patchprofile] " + json.message + " " + json.code;
                    Debug.WriteLine(errorMessage);
                    lastCode = json.code;
                    lastError = json.message;
                    return false;
                }

                JSON_.PatchProfileResult result = JsonConvert.DeserializeObject<JSON_.PatchProfileResult>(response.Content.ReadAsStringAsync().Result);
                if (result.Status == "validated")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        #region setup_pin, setup_verif, extendedsplash
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

                accessToken = json.accessToken;
                App.TLKM.localSettings.Values["accessToken"] = accessToken;
                refreshToken = json.refreshToken;
                App.TLKM.localSettings.Values["refreshToken"] = refreshToken;

                Debug.WriteLine("[tlkm_main - regen_token] accessToken " + accessToken);
                Debug.WriteLine("[tlkm_main - regen_token] refreshToken " + refreshToken);

                return true;
            }
        }

        public async Task<bool> Login()
        {
            Debug.WriteLine("[tlkm_main - login] input number " + serviceId);

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
                    return false;
                }

                string semiParsedJson = response.Content.ReadAsStringAsync().Result;
                if (semiParsedJson.StartsWith("["))
                {
                    semiParsedJson = response.Content.ReadAsStringAsync().Result.Remove(0, 2).Remove(response.Content.ReadAsStringAsync().Result.Length - 3, 1);
                }

                dynamic json = JObject.Parse(semiParsedJson);
                if (json.errorType != null)
                {
                    string errorMessage = "[tlkm_main - login] " + json.message + " " + json.code;
                    Debug.WriteLine(errorMessage);
                    lastCode = json.code;
                    lastError = json.message;
                    return false;
                }

                login = JsonConvert.DeserializeObject<JSON_.Login>(semiParsedJson);
                Debug.WriteLine("[tlkm_main - login] productId: " + login.ManageableAssets[0].Id);
                Debug.WriteLine("[tlkm_main - login] productLabel: " + login.ManageableAssets[0].Label);
                Debug.WriteLine("[tlkm_main - login] telekom_username: " + login.Characteristics[0].Value);
                Debug.WriteLine("[tlkm_main - login] emailAddress: " + login.ContactMediums[1].Medium.EmailAddress);


                fullName = login.Individual.GivenName + " " + login.Individual.FamilyName;
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

                App.TLKM.localSettings.Values["deviceId"] = deviceId;
                accessToken = json.accessToken;
                App.TLKM.localSettings.Values["accessToken"] = accessToken;
                refreshToken = json.refreshToken;
                App.TLKM.localSettings.Values["refreshToken"] = refreshToken;
                App.TLKM.localSettings.Values["serviceId"] = serviceId;
                App.TLKM.localSettings.Values["hasAccount"] = "yes";

                Debug.WriteLine("[tlkm_main - verif] deviceId " + deviceId);
                Debug.WriteLine("[tlkm_main - verif] accessToken " + accessToken);
                Debug.WriteLine("[tlkm_main - verif] refreshToken " + refreshToken);
                Debug.WriteLine("[tlkm_main - verif] serviceId " + serviceId);
                Debug.WriteLine("[tlkm_main - verif] hasAccount yes");

                return true;
            }
        }
        #endregion
    }
}
