
using Acr.UserDialogs;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SP_SanHtar.CustomModels;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SP_SanHtar.cls
{
    public class WebApiClient
    {
        public static string UrlApi = "https://sp-sanhtar-web.conveyor.cloud/";
        private WebApiClient(string baseUri)
        {
            BaseUri = baseUri;
        }

        private static WebApiClient _instance;

        public static WebApiClient Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new WebApiClient("https://sp-sanhtar-web.conveyor.cloud");
                }

                return _instance;
            }
        }

        public string BaseUri { get; private set; }

        public async Task<Response> SignInAsync<T>(string action)
        {
            using (var client = new HttpClient())
            {
                var result = await client.GetAsync(BuildActionUri(action));
                string json = await result.Content.ReadAsStringAsync();
                if (result.IsSuccessStatusCode)
                {
                    //CustomModels.ResponseModel obj = JsonConvert.DeserializeObject<CustomModels.ResponseModel>(json);
                    //var options = new System.Text.Json.JsonSerializerOptions()
                    //{
                    //    IncludeFields = true,
                    //};
                    //var forecast = System.Text.Json.JsonSerializer.Deserialize<CustomModels.ResponseModel>(json);
                    var obj = JsonConvert.DeserializeObject<Response>(json);
                    return obj;

                }

                throw new ApiException(result.StatusCode, json);
            }
        }

        public async Task<ResponseModel> GetListAsync<T>(string action, string authToken = null)
        {
            using (var client = new HttpClient())
            {
                var result = await client.GetAsync(BuildActionUri(action));

                string json = await result.Content.ReadAsStringAsync();
                if (result.IsSuccessStatusCode)
                {
                    var totalRecords = 0;
                    if (result.Headers.Contains("X-Paging-TotalRecordCount"))
                    {
                        string xRecordCount = result.Headers.GetValues("X-Paging-TotalRecordCount").FirstOrDefault();
                        int.TryParse(xRecordCount, out totalRecords);

                    }
                    //return (JsonConvert.DeserializeObject<T>(json));
                    ResponseModel ojbect = JsonConvert.DeserializeObject<ResponseModel>(json);
                    return ojbect;
                }
                UserDialogs.Instance.HideLoading();
                UserDialogs.Instance.Alert(json, "Alert", "OK");
                throw new ApiException(result.StatusCode, json);
            }
        }

        public async Task<ResponseDetailModel> GetListAsyncDetail<T>(string action, string authToken = null)
        {
            using (var client = new HttpClient())
            {
                var result = await client.GetAsync(BuildActionUri(action));

                string json = await result.Content.ReadAsStringAsync();
                if (result.IsSuccessStatusCode)
                {
                    var totalRecords = 0;
                    if (result.Headers.Contains("X-Paging-TotalRecordCount"))
                    {
                        string xRecordCount = result.Headers.GetValues("X-Paging-TotalRecordCount").FirstOrDefault();
                        int.TryParse(xRecordCount, out totalRecords);

                    }
                    //return (JsonConvert.DeserializeObject<T>(json));
                    ResponseDetailModel ojbect = JsonConvert.DeserializeObject<ResponseDetailModel>(json);
                    return ojbect;
                }
                UserDialogs.Instance.HideLoading();
                UserDialogs.Instance.Alert(json, "Alert", "OK");
                throw new ApiException(result.StatusCode, json);
            }
        }

        public async Task<string> GetDataApiData(string url)
        {
            try
            {

                HttpWebRequest request = HttpWebRequest.Create(new Uri(BuildActionUri(url))) as HttpWebRequest;
                request.ContentType = "application/json";
                request.Method = "Get";

                // Send the request to the server and wait for the response:
                using (WebResponse response = await request.GetResponseAsync())
                {
                    // Get a stream representation of the HTTP web response:
                    using (Stream stream = response.GetResponseStream())
                    {
                        // Use this stream to build a JSON document object:
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            // Open the stream using a StreamReader for easy access.  

                            // Read the content.  
                            string responseFromServer = reader.ReadToEnd();
                            JsonTextReader readers = new JsonTextReader(new StringReader(responseFromServer));
                            // responseFromServer = responseFromServer.Replace('"', '\'');
                            var workData = await Task.FromResult<string>(responseFromServer.Replace('"', '\''));
                            //JsonValue jsonDoc = await Task.Run(() => JsonObject.Load(stream));

                            //Console.Out.WriteLine("Response: {0}", jsonDoc.ToString());

                            // Return the JSON document:
                            return workData;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<T> GetAsync<T>(string action, string authToken = null)
        {
            using (var client = new HttpClient())
            {

                var result = await client.GetAsync(BuildActionUri(action));

                string json = await result.Content.ReadAsStringAsync();
                if (result.IsSuccessStatusCode)
                {
                    if (typeof(T) == typeof(string))
                    {
                        return GetValue<T>(json);
                    }
                    return JsonConvert.DeserializeObject<T>(json);
                }
                UserDialogs.Instance.HideLoading();
                UserDialogs.Instance.Alert(json, "Alert", "OK");
                throw new ApiException(result.StatusCode, json);
            }
        }
      
        private string BuildActionUri(string action)
        {
            return BaseUri + action;
        }

        private T GetValue<T>(String value)
        {
            return (T)Convert.ChangeType(value, typeof(T));
        }

        public static async Task<string> GetDataApiWorkingTask(string url)
        {
            try
            {
                url = "http://192.168.99.217:23272/api/UploadItems/GetAll";
                HttpWebRequest request = HttpWebRequest.Create(new Uri(url)) as HttpWebRequest;
                request.ContentType = "application/json";
                request.Method = "GET";
                using (WebResponse response = await request.GetResponseAsync())
                {
                    // Get a stream representation of the HTTP web response:
                    using (Stream stream = response.GetResponseStream())
                    {
                        // Use this stream to build a JSON document object:
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            // Open the stream using a StreamReader for easy access.  

                            // Read the content.  
                            string responseFromServer = reader.ReadToEnd();
                            JsonTextReader readers = new JsonTextReader(new StringReader(responseFromServer));
                            // responseFromServer = responseFromServer.Replace('"', '\'');
                            var workData = await Task.FromResult<string>(responseFromServer.Replace('"', '\''));
                            //JsonValue jsonDoc = await Task.Run(() => JsonObject.Load(stream));

                            //Console.Out.WriteLine("Response: {0}", jsonDoc.ToString());

                            // Return the JSON document:
                            return workData;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<Response> PostAsync<T>(string action, ChangePasswordModel data)
        {
            using (var client = new HttpClient())
            {
                Response resModel = new Response();
                var jsonInString = JsonConvert.SerializeObject(data);
                var result = await client.PostAsync(BuildActionUri(action), new StringContent(jsonInString, Encoding.UTF8, "application/json"));

                if (result.IsSuccessStatusCode)
                {
                    string jsonResult = await result.Content.ReadAsStringAsync();
                    if (IsValidJson(jsonResult))
                    {
                        return JsonConvert.DeserializeObject<Response>(jsonResult);
                    }
                    else
                    {
                        return resModel;
                    }
                }

                string json = await result.Content.ReadAsStringAsync();
                throw new ApiException(result.StatusCode, json);
            }
        }

        private static bool IsValidJson(string strInput)
        {
            strInput = strInput.Trim();
            if ((strInput.StartsWith("{") && strInput.EndsWith("}")) || //For object
                (strInput.StartsWith("[") && strInput.EndsWith("]"))) //For array
            {
                try
                {
                    var obj = JToken.Parse(strInput);
                    return true;
                }
                catch (JsonReaderException jex)
                {
                    //Exception in parsing json
                    Console.WriteLine(jex.Message);
                    return false;
                }
                catch (Exception ex) //some other exception
                {
                    Console.WriteLine(ex.ToString());
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
    }
}
