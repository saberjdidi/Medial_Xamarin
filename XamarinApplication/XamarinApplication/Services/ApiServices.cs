using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Plugin.Connectivity;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using XamarinApplication.Helpers;
using XamarinApplication.Models;
using XamarinApplication.ViewModels;

namespace XamarinApplication.Services
{
    public class ApiServices
    {
        public INavigation Navigation { get; set; }
        public async Task<Response> CheckConnection()
        {
            if (!CrossConnectivity.Current.IsConnected)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = "Please turn on your internet settings.",
                };
            }

            var isReachable = await CrossConnectivity.Current.IsRemoteReachable(
                "google.com");
            if (!isReachable)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = "Check your internet connection.",
                };
            }

            return new Response
            {
                IsSuccess = true,
                Message = "Ok",
            };
        }
        //save
        public async Task<Response> Save<T>(
            string urlBase,
            string servicePrefix,
            string controller,
            T model)
        {
            try
            {
                var request = JsonConvert.SerializeObject(model);
                Debug.WriteLine("********request*************");
                Debug.WriteLine(request);
                var content = new StringContent(
                    request,
                    Encoding.UTF8,
                    "application/json");
                var client = new HttpClient();
                client.BaseAddress = new Uri(urlBase);
                var url = string.Format("{0}{1}", servicePrefix, controller);
                var response = await client.PostAsync(url, content);
                Debug.WriteLine("********response*************");
                Debug.WriteLine(response);
                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = response.StatusCode.ToString(),
                    };
                }

                var result = await response.Content.ReadAsStringAsync();
                var newRecord = JsonConvert.DeserializeObject<T>(result);
                Debug.WriteLine("********newRecord*************");
                Debug.WriteLine(newRecord);
                return new Response
                {
                    IsSuccess = true,
                    Message = "Record added OK",
                    Result = newRecord,
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message,
                };
            }
        }
        //save Event
        public async Task<Response> SaveEvent<T>(
            string urlBase,
            string servicePrefix,
            string controller,
            int id,
            string events,
            T model)
        {
            try
            {
                var request = JsonConvert.SerializeObject(model);
                Debug.WriteLine("********request*************");
                Debug.WriteLine(request);
                var content = new StringContent(
                    request,
                    Encoding.UTF8,
                    "application/json");
                var client = new HttpClient();
                client.BaseAddress = new Uri(urlBase);
                //var url = string.Format("{0}{1}", servicePrefix, controller);
                var url = $"{servicePrefix}{controller}/{id}{events}";
                var response = await client.PostAsync(url, content);
                Debug.WriteLine("********response*************");
                Debug.WriteLine(response);
                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = response.StatusCode.ToString(),
                    };
                }

                var result = await response.Content.ReadAsStringAsync();
                var newRecord = JsonConvert.DeserializeObject<T>(result);
                Debug.WriteLine("********newRecord*************");
                Debug.WriteLine(newRecord);
                return new Response
                {
                    IsSuccess = true,
                    Message = "Record added OK",
                    Result = newRecord,
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message,
                };
            }
        }
        //put
        public async Task<Response> Put<T>(
            string urlBase,
            string servicePrefix,
            string controller,
            T model)
        {
            try
            {
                var request = JsonConvert.SerializeObject(model);
                Debug.WriteLine("********request*************");
                Debug.WriteLine(request);
                var content = new StringContent(
                    request,
                    Encoding.UTF8,
                    "application/json");
                var client = new HttpClient();
                client.BaseAddress = new Uri(urlBase);
                var url = string.Format("{0}{1}", servicePrefix, controller);
                Debug.WriteLine("+++++++++++++++++++++++++url++++++++++++++++++++++++");
                Debug.WriteLine(url);
                var response = await client.PutAsync(url, content);
                Debug.WriteLine("********response*************");
                Debug.WriteLine(response);
                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = response.StatusCode.ToString(),
                    };
                }

                var result = await response.Content.ReadAsStringAsync();
                Debug.WriteLine("********result*************");
                Debug.WriteLine(result);
                var newRecord = JsonConvert.DeserializeObject<T>(result);
                Debug.WriteLine("********newRecord*************");
                Debug.WriteLine(newRecord);
                return new Response
                {
                    IsSuccess = true,
                    Message = "Record updated OK",
                    Result = newRecord,
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message,
                };
            }
        }
        public async Task<Response> PutProduct<T>(
            string urlBase,
            string servicePrefix,
            string controller,
            ProductJson model)
        {
            try
            {
                var request = JsonConvert.SerializeObject(model);
                Debug.WriteLine("********request*************");
                Debug.WriteLine(request);
                var content = new StringContent(
                    request,
                    Encoding.UTF8,
                    "application/json");
                var client = new HttpClient();
                client.BaseAddress = new Uri(urlBase);
                var url = string.Format("{0}{1}", servicePrefix, controller);
                var response = await client.PutAsync(url, content);
                Debug.WriteLine("********response*************");
                Debug.WriteLine(response);
                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = response.StatusCode.ToString(),
                    };
                }

                var result = await response.Content.ReadAsStringAsync();
                Debug.WriteLine("********result*************");
                Debug.WriteLine(result);
                var newRecord = JsonConvert.DeserializeObject<T>(result);
                Debug.WriteLine("********newRecord*************");
                Debug.WriteLine(newRecord);
                return new Response
                {
                    IsSuccess = true,
                    Message = "Record updated OK",
                    Result = newRecord,
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message,
                };
            }
        }
        public async Task PutEvent<T>(
            string urlBase,
            string servicePrefix,
            string controller,
            T model)
        {
            try
            {
                var request = JsonConvert.SerializeObject(model);
                Debug.WriteLine("********request*************");
                Debug.WriteLine(request);
                var content = new StringContent(
                    request,
                    Encoding.UTF8,
                    "application/json");
                var client = new HttpClient();
                client.BaseAddress = new Uri(urlBase);
                var url = string.Format("{0}{1}", servicePrefix, controller);
                Debug.WriteLine("+++++++++++++++++++++++++url++++++++++++++++++++++++");
                Debug.WriteLine(url);
                var response = await client.PutAsync(url, content);
                Debug.WriteLine("********response*************");
                Debug.WriteLine(response);
                /* if (!response.IsSuccessStatusCode)
                 {
                     return new Response
                     {
                         IsSuccess = false,
                         Message = response.StatusCode.ToString(),
                     };
                 }

                  var result = await response.Content.ReadAsStringAsync();
                  Debug.WriteLine("********result*************");
                  Debug.WriteLine(result);
                  var newRecord = JsonConvert.DeserializeObject<T>(result);
                  Debug.WriteLine("********newRecord*************");
                  Debug.WriteLine(newRecord);
                  return new Response
                  {
                      IsSuccess = true,
                      Message = "Record updated OK",
                      Result = newRecord,
                  };*/
            }
            catch (Exception ex)
            {
                Debug.WriteLine("********Exception*************");
                Debug.WriteLine(ex);
            }
        }
        //delete
        public async Task<Response> Delete<T>(
            string urlBase,
            string servicePrefix,
            string controller,
            int id)
        {
            try
            {
                var client = new HttpClient();
                client.BaseAddress = new Uri(urlBase);
                /*var url = string.Format(
                    "{0}{1}/{2}",
                    servicePrefix,
                    controller,
                    id);*/
                var url = $"{servicePrefix}{controller}/{id}";
                var response = await client.DeleteAsync(url);
                var result = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    var error = JsonConvert.DeserializeObject<Response>(result);
                    error.IsSuccess = false;
                    return error;
                }

                return new Response
                {
                    IsSuccess = true,
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message,
                };
            }
        }
        public async Task<Response> DeleteEvent<T>(
           string urlBase,
           string servicePrefix,
           string controller,
           int id,
           string events,
           T model)
        {
            try
            {
                var jsonRequest = JsonConvert.SerializeObject(model);
                Debug.WriteLine("********jsonRequest*************");
                Debug.WriteLine(jsonRequest);
                var client = new HttpClient();
                client.BaseAddress = new Uri(urlBase);
                var url = $"{servicePrefix}{controller}/{id}{events}";
                Debug.WriteLine("********url*************");
                Debug.WriteLine(url);
                HttpRequestMessage request = new HttpRequestMessage
                {
                    Content = new StringContent(jsonRequest, Encoding.UTF8, "application/json"),
                    Method = HttpMethod.Delete,
                    RequestUri = new Uri(url)
                };
                
                var response = await client.SendAsync(request);
                Debug.WriteLine("********response*************");
                Debug.WriteLine(response);
                var result = await response.Content.ReadAsStringAsync();
                Debug.WriteLine("********result*************");
                Debug.WriteLine(result);
                if (!response.IsSuccessStatusCode)
                {
                    var error = JsonConvert.DeserializeObject<Response>(result);
                    error.IsSuccess = false;
                    return error;
                }

                return new Response
                {
                    IsSuccess = true,
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message,
                };
            }
        }
        //Products of client
        public async Task<Response> ClientProducts<T>(
            string urlBase,
            string servicePrefix,
            string controller,
            long id,
            string selling,
            SearchRequestByClient searchRequest)
        {
            try
            {
                var request = JsonConvert.SerializeObject(searchRequest);
                Debug.WriteLine("********------------request------------*************");
                Debug.WriteLine(request);
                var content = new StringContent(
                    request,
                    Encoding.UTF8,
                    "application/json");
                var client = new HttpClient();
                client.BaseAddress = new Uri(urlBase);
                //var url = string.Format("{0}{1}", servicePrefix, controller);
                var url = $"{servicePrefix}{controller}/{id}{selling}";
                Debug.WriteLine("********------------url------------*************");
                Debug.WriteLine(url);
                var response = await client.PostAsync(url, content);
                Debug.WriteLine("+++++++++++++++++++++++++response++++++++++++++++++++++++");
                Debug.WriteLine(response);
                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = response.StatusCode.ToString(),
                    };
                }

                var result = await response.Content.ReadAsStringAsync();
                // var newRecord = JsonConvert.DeserializeObject<T>(result);
                var list = JsonConvert.DeserializeObject<List<T>>(result, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                Debug.WriteLine("+++++++++++++++++++++++++list++++++++++++++++++++++++");
                Debug.WriteLine(list);

                return new Response
                {
                    IsSuccess = true,
                    Message = "Record added OK",
                    Result = list,
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message,
                };
            }
        }
        //Component of Product
        public async Task<Response> GetComponentProduct<T>(
           string urlBase,
           string servicePrefix,
           string controller,
            int id,
            string component)
        {
            try
            {
                var client = new HttpClient();
                client.BaseAddress = new Uri(urlBase);
                var url = $"{servicePrefix}{controller}/{id}{component}";
                var response = await client.GetAsync(url);
                var result = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = result,
                    };
                }

                var list = JsonConvert.DeserializeObject<List<T>>(result, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                return new Response
                {
                    IsSuccess = true,
                    Message = "Ok",
                    Result = list,
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message,
                };
            }
        }
        public async Task<Response> PutComponent<T>(
           string urlBase,
           string servicePrefix,
           string controller,
           int id,
           string component,
           object model)
        {
            try
            {
                var request = JsonConvert.SerializeObject(model);
                Debug.WriteLine("********request*************");
                Debug.WriteLine(request);
                var content = new StringContent(
                    request,
                    Encoding.UTF8,
                    "application/json");
                var client = new HttpClient();
                client.BaseAddress = new Uri(urlBase);
                var url = $"{servicePrefix}{controller}/{id}{component}";
                var response = await client.PutAsync(url, content);
                Debug.WriteLine("********response*************");
                Debug.WriteLine(response);
                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = response.StatusCode.ToString(),
                    };
                }

                var result = await response.Content.ReadAsStringAsync();
                Debug.WriteLine("********result*************");
                Debug.WriteLine(result);
                var newRecord = JsonConvert.DeserializeObject<T>(result);
                Debug.WriteLine("********newRecord*************");
                Debug.WriteLine(newRecord);
                return new Response
                {
                    IsSuccess = true,
                    Message = "Record updated OK",
                    Result = newRecord,
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message,
                };
            }
        }
        public async Task<Response> PostComponent<T>(
            string urlBase,
            string servicePrefix,
            string controller,
            SearchProductComponent searchRequest)
        {
            try
            {
                var request = JsonConvert.SerializeObject(searchRequest);
                Debug.WriteLine("********------------request------------*************");
                Debug.WriteLine(request);
                var content = new StringContent(
                    request,
                    Encoding.UTF8,
                    "application/json");
                var client = new HttpClient();
                client.BaseAddress = new Uri(urlBase);
                var url = string.Format("{0}{1}", servicePrefix, controller);
                var response = await client.PostAsync(url, content);
                Debug.WriteLine("+++++++++++++++++++++++++response++++++++++++++++++++++++");
                Debug.WriteLine(response);
                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = response.StatusCode.ToString(),
                    };
                }

                var result = await response.Content.ReadAsStringAsync();
                // var newRecord = JsonConvert.DeserializeObject<T>(result);
                var list = JsonConvert.DeserializeObject<List<T>>(result, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                Debug.WriteLine("+++++++++++++++++++++++++list++++++++++++++++++++++++");
                Debug.WriteLine(list);

                return new Response
                {
                    IsSuccess = true,
                    Message = "Record added OK",
                    Result = list,
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message,
                };
            }
        }
        //end component
        public async Task<Response> SearchClientByCategory<T>(
            string urlBase,
            string servicePrefix,
            string controller,
            SearchRequestByCategory searchRequest)
        {
            try
            {
                var request = JsonConvert.SerializeObject(searchRequest);
                Debug.WriteLine("********------------request------------*************");
                Debug.WriteLine(request);
                var content = new StringContent(
                    request,
                    Encoding.UTF8,
                    "application/json");
                var client = new HttpClient();
                client.BaseAddress = new Uri(urlBase);
                var url = string.Format("{0}{1}", servicePrefix, controller);
                var response = await client.PostAsync(url, content);
                Debug.WriteLine("+++++++++++++++++++++++++response++++++++++++++++++++++++");
                Debug.WriteLine(response);
                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = response.StatusCode.ToString(),
                    };
                }

                var result = await response.Content.ReadAsStringAsync();
                // var newRecord = JsonConvert.DeserializeObject<T>(result);
                var list = JsonConvert.DeserializeObject<List<T>>(result, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                Debug.WriteLine("+++++++++++++++++++++++++list++++++++++++++++++++++++");
                Debug.WriteLine(list);

                return new Response
                {
                    IsSuccess = true,
                    Message = "Record added OK",
                    Result = list,
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message,
                };
            }
        }
        public async Task<Response> SearchProductBySupplier<T>(
            string urlBase,
            string servicePrefix,
            string controller,
            SearchRequestBySupplier searchRequest)
        {
            try
            {
                var request = JsonConvert.SerializeObject(searchRequest);
                Debug.WriteLine("********------------request------------*************");
                Debug.WriteLine(request);
                var content = new StringContent(
                    request,
                    Encoding.UTF8,
                    "application/json");
                var client = new HttpClient();
                client.BaseAddress = new Uri(urlBase);
                var url = string.Format("{0}{1}", servicePrefix, controller);
                var response = await client.PostAsync(url, content);
                Debug.WriteLine("+++++++++++++++++++++++++response++++++++++++++++++++++++");
                Debug.WriteLine(response);
                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = response.StatusCode.ToString(),
                    };
                }

                var result = await response.Content.ReadAsStringAsync();
                // var newRecord = JsonConvert.DeserializeObject<T>(result);
                var list = JsonConvert.DeserializeObject<List<T>>(result, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                Debug.WriteLine("+++++++++++++++++++++++++list++++++++++++++++++++++++");
                Debug.WriteLine(list);

                return new Response
                {
                    IsSuccess = true,
                    Message = "Record added OK",
                    Result = list,
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message,
                };
            }
        }
        //paging listview lazy load
        public async Task<Response> LoadMoreData<T>(
           string urlBase,
           string servicePrefix,
           string controller,
           SearchRequest searchRequest)
        {
            try
            {
                var request = JsonConvert.SerializeObject(searchRequest);
                var content = new StringContent(
                    request,
                    Encoding.UTF8,
                    "application/json");
                var client = new HttpClient();
                client.BaseAddress = new Uri(urlBase);
                var url = string.Format("{0}{1}", servicePrefix, controller);
                var response = await client.PostAsync(url, content);

                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = response.StatusCode.ToString(),
                    };
                }

                var result = await response.Content.ReadAsStringAsync();
                // var newRecord = JsonConvert.DeserializeObject<T>(result);
                var list = JsonConvert.DeserializeObject<List<T>>(result, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });


                return new Response
                {
                    IsSuccess = true,
                    Message = "Record added OK",
                    Result = list,
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message,
                };
            }
        }

        public async Task<Response> SeviceProduct<T>(
           string urlBase,
           string servicePrefix,
           string controller,
           SearchRequest searchRequest)
        {
            try
            {
                var request = JsonConvert.SerializeObject(searchRequest);
                Debug.WriteLine("********request*************");
                Debug.WriteLine(request);
                var content = new StringContent(
                    request,
                    Encoding.UTF8,
                    "application/json");
                var client = new HttpClient();
                client.BaseAddress = new Uri(urlBase);
                var url = string.Format("{0}{1}", servicePrefix, controller);
                var response = await client.PostAsync(url, content);
                Debug.WriteLine("********response*************");
                Debug.WriteLine(response);
                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = response.StatusCode.ToString(),
                    };
                }

                var result = await response.Content.ReadAsStringAsync();
                //var newRecord = JsonConvert.DeserializeObject<T>(result);
                var list = JsonConvert.DeserializeObject<List<T>>(result, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                await Task.Delay(1000);
                Debug.WriteLine("+++++++++++++++++++++++++list++++++++++++++++++++++++");
                Debug.WriteLine(list);
                // int skip = pageIndex * pageSize;
                // Debug.WriteLine($"Getting Page:{pageIndex}; Page Size: {pageSize}");
              //  Debug.WriteLine($"Getting Page:{maxResult}");
                return new Response
                {
                    IsSuccess = true,
                    Message = "OK",
                    // Result = list.Skip(skip).Take(pageSize).ToList(),
                    Result = list.ToList(),
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message,
                };
            }
        }
        public async Task<Response> SearchRequestProduct<T>(
           string urlBase,
           string servicePrefix,
           string controller,
           int maxResult,
           string offset,
           int pageIndex,
           SearchRequest searchRequest)
        {
            try
            {
                var request = JsonConvert.SerializeObject(searchRequest);
                Debug.WriteLine("********request*************");
                Debug.WriteLine(request);
                var content = new StringContent(
                    request,
                    Encoding.UTF8,
                    "application/json");
                var client = new HttpClient();
                client.BaseAddress = new Uri(urlBase);
               // var url = string.Format("{0}{1}", servicePrefix, controller, maxResult, offset, pageIndex);
                var url = $"{servicePrefix}{controller}{maxResult}{offset}{pageIndex}";
                var response = await client.PostAsync(url, content);
                Debug.WriteLine("********response*************");
                Debug.WriteLine(response);
                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = response.StatusCode.ToString(),
                    };
                }

                var result = await response.Content.ReadAsStringAsync();
                //var newRecord = JsonConvert.DeserializeObject<T>(result);
                var list = JsonConvert.DeserializeObject<List<T>>(result, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                await Task.Delay(2000);
                Debug.WriteLine("+++++++++++++++++++++++++list++++++++++++++++++++++++");
                Debug.WriteLine(list);
                // int skip = int.Parse(pageIndex) * int.Parse(pageSize);
               // int skip = pageIndex * pageSize;
                // Debug.WriteLine($"Getting Page:{pageIndex}; Page Size: {pageSize}");
                Debug.WriteLine($"Getting Page:{pageIndex}");
                return new Response
                {
                    IsSuccess = true,
                    Message = "OK",
                     Result = list.Skip(pageIndex * maxResult).Take(maxResult).ToList(),
                   // Result = list.ToList(),
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message,
                };
            }
        }
        //autocomplete
        public async Task<List<T>> AutocompleteClient<T>(
          string urlBase,
          string servicePrefix,
          string controller,
          SearchRequest searchRequest)
        {

            var request = JsonConvert.SerializeObject(searchRequest);
            Debug.WriteLine("********request*************");
            Debug.WriteLine(request);
            var content = new StringContent(
                request,
                Encoding.UTF8,
                "application/json");
            var client = new HttpClient();
            client.BaseAddress = new Uri(urlBase);
            var url = string.Format("{0}{1}", servicePrefix, controller);
            var response = await client.PostAsync(url, content);
            Debug.WriteLine("********response*************");
            Debug.WriteLine(response);

            var result = await response.Content.ReadAsStringAsync();
            //var newRecord = JsonConvert.DeserializeObject<T>(result);
            var list = JsonConvert.DeserializeObject<List<T>>(result, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
           
            return list;

        }
        //Calendar
        public async Task<Response> GetEvents<T>(
            string urlBase,
            string servicePrefix,
            string controller,
            long id,
            string eventCalendar)
        {
            try
            {
                var client = new HttpClient
                {
                    BaseAddress = new Uri(urlBase)
                };

                var url = $"{servicePrefix}{controller}/{id}{eventCalendar}";
                var response = await client.GetAsync(url);
                var result = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = result,
                    };
                }

                var list = JsonConvert.DeserializeObject<List<T>>(result, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                return new Response
                {
                    IsSuccess = true,
                    Result = list
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message
                };
            }
        }
        //end

        public async Task<Response> SearchRequest<T>(
           string urlBase,
           string servicePrefix,
           string controller,
           string pageIndex,
           SearchRequest searchRequest)
        {
            try
            {
                var request = JsonConvert.SerializeObject(searchRequest);
                Debug.WriteLine("********request*************");
                Debug.WriteLine(request);
                var content = new StringContent(
                    request,
                    Encoding.UTF8,
                    "application/json");
                var client = new HttpClient();
                client.BaseAddress = new Uri(urlBase);
                var url = string.Format("{0}{1}", servicePrefix, controller, pageIndex);
                var response = await client.PostAsync(url, content);
                Debug.WriteLine("********response*************");
                Debug.WriteLine(response);
                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = response.StatusCode.ToString(),
                    };
                }

                var result = await response.Content.ReadAsStringAsync();
                //var newRecord = JsonConvert.DeserializeObject<T>(result);
                var list = JsonConvert.DeserializeObject<List<T>>(result, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                await Task.Delay(1000);
                Debug.WriteLine("+++++++++++++++++++++++++list++++++++++++++++++++++++");
                Debug.WriteLine(list);
                // int skip = pageIndex * pageSize;
                // Debug.WriteLine($"Getting Page:{pageIndex}; Page Size: {pageSize}");
                Debug.WriteLine($"Getting Page:{pageIndex}");
                return new Response
                {
                    IsSuccess = true,
                    Message = "OK",
                    // Result = list.Skip(skip).Take(pageSize).ToList(),
                    Result = list.ToList(),
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message,
                };
            }
        } 
        public async Task<Response> Post<T>(
            string urlBase,
            string servicePrefix,
            string controller,
            SearchRequest searchRequest)
        {
            try
            {
                var request = JsonConvert.SerializeObject(searchRequest);
                Debug.WriteLine("********------------request------------*************");
                Debug.WriteLine(request);
                var content = new StringContent(
                    request,
                    Encoding.UTF8,
                    "application/json");
                var client = new HttpClient();
                client.BaseAddress = new Uri(urlBase);
                var url = string.Format("{0}{1}", servicePrefix, controller);
                var response = await client.PostAsync(url, content);
                Debug.WriteLine("+++++++++++++++++++++++++response++++++++++++++++++++++++");
                Debug.WriteLine(response);
                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = response.StatusCode.ToString(),
                    };
                }

                var result = await response.Content.ReadAsStringAsync();
               // var newRecord = JsonConvert.DeserializeObject<T>(result);
                var list = JsonConvert.DeserializeObject<List<T>>(result, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                Debug.WriteLine("+++++++++++++++++++++++++list++++++++++++++++++++++++");
                Debug.WriteLine(list);

                return new Response
                {
                    IsSuccess = true,
                    Message = "Record added OK",
                    Result = list,
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message,
                };
            }
        }
        // Offer
        public async Task<Response> PostOffer<T>(
            string urlBase,
            string servicePrefix,
            string controller,
            SearchOffer search)
        {
            try
            {
                var request = JsonConvert.SerializeObject(search);
                Debug.WriteLine("********------------request------------*************");
                Debug.WriteLine(request);
                var content = new StringContent(
                    request,
                    Encoding.UTF8,
                    "application/json");
                var client = new HttpClient();
                client.BaseAddress = new Uri(urlBase);
                var url = string.Format("{0}{1}", servicePrefix, controller);
                var response = await client.PostAsync(url, content);
                Debug.WriteLine("+++++++++++++++++++++++++response++++++++++++++++++++++++");
                Debug.WriteLine(response);
                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = response.StatusCode.ToString(),
                    };
                }

                var result = await response.Content.ReadAsStringAsync();
                // var newRecord = JsonConvert.DeserializeObject<T>(result);
                var list = JsonConvert.DeserializeObject<List<T>>(result, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                Debug.WriteLine("+++++++++++++++++++++++++list++++++++++++++++++++++++");
                Debug.WriteLine(list);

                return new Response
                {
                    IsSuccess = true,
                    Message = "Record added OK",
                    Result = list,
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message,
                };
            }
        }
        //Users Agents
        public async Task<Response> PostAgents<T>(
            string urlBase,
            string servicePrefix,
            string controller,
            Checked search)
        {
            try
            {
                var request = JsonConvert.SerializeObject(search);
                Debug.WriteLine("********------------request------------*************");
                Debug.WriteLine(request);
                var content = new StringContent(
                    request,
                    Encoding.UTF8,
                    "application/json");
                var client = new HttpClient();
                client.BaseAddress = new Uri(urlBase);
                var url = string.Format("{0}{1}", servicePrefix, controller);
                var response = await client.PostAsync(url, content);
                Debug.WriteLine("+++++++++++++++++++++++++response++++++++++++++++++++++++");
                Debug.WriteLine(response);
                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = response.StatusCode.ToString(),
                    };
                }

                var result = await response.Content.ReadAsStringAsync();
                // var newRecord = JsonConvert.DeserializeObject<T>(result);
                var list = JsonConvert.DeserializeObject<List<T>>(result, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                Debug.WriteLine("+++++++++++++++++++++++++list++++++++++++++++++++++++");
                Debug.WriteLine(list);

                return new Response
                {
                    IsSuccess = true,
                    Message = "Record added OK",
                    Result = list,
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message,
                };
            }
        }
        public async Task<Response> SearchProduct<T>(
            string urlBase,
            string servicePrefix,
            string controller,
            SearchProduct searchRequest)
        {
            try
            {
                var request = JsonConvert.SerializeObject(searchRequest);
                Debug.WriteLine("********------------request------------*************");
                Debug.WriteLine(request);
                var content = new StringContent(
                    request,
                    Encoding.UTF8,
                    "application/json");
                var client = new HttpClient();
                client.BaseAddress = new Uri(urlBase);
                var url = string.Format("{0}{1}", servicePrefix, controller);
                var response = await client.PostAsync(url, content);
                Debug.WriteLine("+++++++++++++++++++++++++response++++++++++++++++++++++++");
                Debug.WriteLine(response);
                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = response.StatusCode.ToString(),
                    };
                }

                var result = await response.Content.ReadAsStringAsync();
                // var newRecord = JsonConvert.DeserializeObject<T>(result);
                var list = JsonConvert.DeserializeObject<List<T>>(result, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                Debug.WriteLine("+++++++++++++++++++++++++list++++++++++++++++++++++++");
                Debug.WriteLine(list);

                return new Response
                {
                    IsSuccess = true,
                    Message = "Record added OK",
                    Result = list,
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message,
                };
            }
        }
        public async Task<(int, List<T>)> SearchClient<T>(
          string urlBase,
          string servicePrefix,
          string controller,
          int pageIndex,
          int pageSize,
          SearchRequest searchRequest)
        {

            var request = JsonConvert.SerializeObject(searchRequest);
            Debug.WriteLine("********request*************");
            Debug.WriteLine(request);
            var content = new StringContent(
                request,
                Encoding.UTF8,
                "application/json");
            var client = new HttpClient();
            client.BaseAddress = new Uri(urlBase);
            var url = string.Format("{0}{1}", servicePrefix, controller);
            var response = await client.PostAsync(url, content);
            Debug.WriteLine("********response*************");
            Debug.WriteLine(response);

            var result = await response.Content.ReadAsStringAsync();
            //var newRecord = JsonConvert.DeserializeObject<T>(result);
            var list = JsonConvert.DeserializeObject<List<T>>(result, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            await Task.Delay(2000);
            Debug.WriteLine("+++++++++++++++++++++++++list++++++++++++++++++++++++");
            Debug.WriteLine(list);
            int skip = pageIndex * pageSize;
            Debug.WriteLine($"Getting Page:{pageIndex}; Page Size: {pageSize}");
            int totalCount = list.Count;
            return (totalCount, list.Skip(skip).Take(pageSize).ToList());

        }

        public async Task<Response> PostRequest<T>(
          string urlBase,
          string servicePrefix,
          string controller,
          SearchRequest searchRequest)
        {
            try
            {
                var request = JsonConvert.SerializeObject(searchRequest);
                Debug.WriteLine("********request*************");
                Debug.WriteLine(request);
                var content = new StringContent(
                    request,
                    Encoding.UTF8,
                    "application/json");
                var client = new HttpClient();
                client.BaseAddress = new Uri(urlBase);
                var url = string.Format("{0}{1}", servicePrefix, controller);
                var response = await client.PostAsync(url, content);
                Debug.WriteLine("********response*************");
                Debug.WriteLine(response);
                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = response.StatusCode.ToString(),
                    };
                }

                var result = await response.Content.ReadAsStringAsync();
                //var newRecord = JsonConvert.DeserializeObject<T>(result);
                var list = JsonConvert.DeserializeObject<List<T>>(result, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

                Debug.WriteLine("+++++++++++++++++++++++++list++++++++++++++++++++++++");
                Debug.WriteLine(list);
                return new Response
                {
                    IsSuccess = true,
                    Message = "OK",
                    Result = list,
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message,
                };
            }
        }

        public async Task<Response> GetList<T>(
            string urlBase,
            string servicePrefix,
            string controller)
        {
            try
            {
                var client = new HttpClient();
                //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                client.BaseAddress = new Uri(urlBase);
                var url = string.Format("{0}{1}", servicePrefix, controller);
                var response = await client.GetAsync(url);
                var result = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = result,
                    };
                }

                var list = JsonConvert.DeserializeObject<List<T>>(result, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                return new Response
                {
                    IsSuccess = true,
                    Message = "Ok",
                    Result = list,
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message,
                };
            }
        }
        public async Task<Response> PostRequest<T>(
           string urlBase,
           string servicePrefix,
           string controller,
           string cookie,
           SearchModel searchModel)
        {
            try
            {
                var request = JsonConvert.SerializeObject(searchModel);
                Debug.WriteLine("********request*************");
                Debug.WriteLine(request);
                var content = new StringContent(
                    request,
                    Encoding.UTF8,
                    "application/json");
                var cookieContainer = new CookieContainer();
                var handler = new HttpClientHandler() { CookieContainer = cookieContainer };
                var client = new HttpClient(handler);
                client.BaseAddress = new Uri(urlBase);
                var url = string.Format("{0}{1}", servicePrefix, controller);
                cookieContainer.Add(client.BaseAddress, new Cookie("JSESSIONID",cookie));
                var response = await client.PostAsync(url, content);
                Debug.WriteLine("********response*************");
                Debug.WriteLine(response);
                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = response.StatusCode.ToString(),
                    };
                }

                var result = await response.Content.ReadAsStringAsync();
                //var newRecord = JsonConvert.DeserializeObject<T>(result);
                var list = JsonConvert.DeserializeObject<List<T>>(result, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

                Debug.WriteLine("+++++++++++++++++++++++++list++++++++++++++++++++++++");
                Debug.WriteLine(list);
                return new Response
                {
                    IsSuccess = true,
                    Message = "OK",
                    Result = list,
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message,
                };
            }
        }
        public async Task<Response> PostRequest<T>(
           string urlBase,
           string servicePrefix,
           string controller,
           int pageIndex,
           int pageSize,
           string cookie,
           SearchModel searchModel)
        {
            try
            {
                var request = JsonConvert.SerializeObject(searchModel);
                Debug.WriteLine("********request*************");
                Debug.WriteLine(request);
                var content = new StringContent(
                    request,
                    Encoding.UTF8,
                    "application/json");
                var cookieContainer = new CookieContainer();
                var handler = new HttpClientHandler() { CookieContainer = cookieContainer };
                var client = new HttpClient(handler);
                client.BaseAddress = new Uri(urlBase);
                var url = string.Format("{0}{1}", servicePrefix, controller);
                cookieContainer.Add(client.BaseAddress, new Cookie("JSESSIONID", cookie));
                var response = await client.PostAsync(url, content);
                Debug.WriteLine("********response*************");
                Debug.WriteLine(response);
                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = response.StatusCode.ToString(),
                    };
                }

                var result = await response.Content.ReadAsStringAsync();
                //var newRecord = JsonConvert.DeserializeObject<T>(result);
                var list = JsonConvert.DeserializeObject<List<T>>(result, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                await Task.Delay(1000);
                Debug.WriteLine("+++++++++++++++++++++++++list++++++++++++++++++++++++");
                Debug.WriteLine(list);
                return new Response
                {
                    IsSuccess = true,
                    Message = "OK",
                    Result = list.Skip(pageIndex * pageSize).Take(pageSize).ToList(),
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message,
                };
            }
        }
        //Prices of Product
        public async Task<Response> PostPrice<T>(
            string urlBase,
            string servicePrefix,
            string controller,
            SearchPriceProduct searchRequest)
        {
            try
            {
                var request = JsonConvert.SerializeObject(searchRequest);
                Debug.WriteLine("********------------request------------*************");
                Debug.WriteLine(request);
                var content = new StringContent(
                    request,
                    Encoding.UTF8,
                    "application/json");
                var client = new HttpClient();
                client.BaseAddress = new Uri(urlBase);
                var url = string.Format("{0}{1}", servicePrefix, controller);
                var response = await client.PostAsync(url, content);
                Debug.WriteLine("+++++++++++++++++++++++++response++++++++++++++++++++++++");
                Debug.WriteLine(response);
                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = response.StatusCode.ToString(),
                    };
                }

                var result = await response.Content.ReadAsStringAsync();
                Debug.WriteLine("********------------result------------*************");
                Debug.WriteLine(result);
                // var newRecord = JsonConvert.DeserializeObject<T>(result);
                var list = JsonConvert.DeserializeObject<List<T>>(result, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                Debug.WriteLine("+++++++++++++++++++++++++list++++++++++++++++++++++++");
                Debug.WriteLine(list);

                return new Response
                {
                    IsSuccess = true,
                    Message = "Record added OK",
                    Result = list,
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message,
                };
            }
        }
        //Commercial Supplier
        public async Task<Response> CommercialSupplier<T>(
            string urlBase,
            string servicePrefix,
            string controller,
            SearchCommercialSupplier searchRequest)
        {
            try
            {
                var request = JsonConvert.SerializeObject(searchRequest);
                Debug.WriteLine("********------------request------------*************");
                Debug.WriteLine(request);
                var content = new StringContent(
                    request,
                    Encoding.UTF8,
                    "application/json");
                var client = new HttpClient();
                client.BaseAddress = new Uri(urlBase);
                var url = string.Format("{0}{1}", servicePrefix, controller);
                var response = await client.PostAsync(url, content);
                Debug.WriteLine("+++++++++++++++++++++++++response++++++++++++++++++++++++");
                Debug.WriteLine(response);
                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = response.StatusCode.ToString(),
                    };
                }

                var result = await response.Content.ReadAsStringAsync();
                Debug.WriteLine("********------------result------------*************");
                Debug.WriteLine(result);
                // var newRecord = JsonConvert.DeserializeObject<T>(result);
                var list = JsonConvert.DeserializeObject<List<T>>(result, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                Debug.WriteLine("+++++++++++++++++++++++++list++++++++++++++++++++++++");
                Debug.WriteLine(list);

                return new Response
                {
                    IsSuccess = true,
                    Message = "Record added OK",
                    Result = list,
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message,
                };
            }
        }
        //Commercial Product
        public async Task<Response> CommercialProduct<T>(
            string urlBase,
            string servicePrefix,
            string controller,
            SearchCommercialProduct searchRequest)
        {
            try
            {
                var request = JsonConvert.SerializeObject(searchRequest);
                Debug.WriteLine("********------------request------------*************");
                Debug.WriteLine(request);
                var content = new StringContent(
                    request,
                    Encoding.UTF8,
                    "application/json");
                var client = new HttpClient();
                client.BaseAddress = new Uri(urlBase);
                var url = string.Format("{0}{1}", servicePrefix, controller);
                var response = await client.PostAsync(url, content);
                Debug.WriteLine("+++++++++++++++++++++++++response++++++++++++++++++++++++");
                Debug.WriteLine(response);
                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = response.StatusCode.ToString(),
                    };
                }

                var result = await response.Content.ReadAsStringAsync();
                Debug.WriteLine("********------------result------------*************");
                Debug.WriteLine(result);
                // var newRecord = JsonConvert.DeserializeObject<T>(result);
                var list = JsonConvert.DeserializeObject<List<T>>(result, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                Debug.WriteLine("+++++++++++++++++++++++++list++++++++++++++++++++++++");
                Debug.WriteLine(list);

                return new Response
                {
                    IsSuccess = true,
                    Message = "Record added OK",
                    Result = list,
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message,
                };
            }
        }
        /*
        public async Task<Response> Post<T>(
            string urlBase,
            string servicePrefix,
            string controller,
            string tokenType,
            string accessToken,
            SearchModel searchModel)
        {
            try
            {
                var request = JsonConvert.SerializeObject(searchModel);
                var content = new StringContent(
                    request, Encoding.UTF8,
                    "application/json");
                var client = new HttpClient();
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue(tokenType, accessToken);
                client.BaseAddress = new Uri(urlBase);
                var url = string.Format("{0}{1}", servicePrefix, controller);
                var response = await client.PostAsync(url, content);
                var result = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    var error = JsonConvert.DeserializeObject<Response>(result);
                    error.IsSuccess = false;
                    return error;
                }

                var newRecord = JsonConvert.DeserializeObject<T>(result);

                return new Response
                {
                    IsSuccess = true,
                    Message = "Record added OK",
                    Result = newRecord,
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message,
                };
            }
        }


        
          public async Task GetRequestsSearch(SearchModel searchModel)
        {
            var client = new HttpClient();

            var json = JsonConvert.SerializeObject(searchModel);
            HttpContent content = new StringContent(json);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var response = await client.PostAsync("https://portalesp.smart-path.it/Portalesp/request/searchByExample", content);

           // var response = await client.PostAsync("https://portalesp.smart-path.it/Portalesp/request/searchByExample", content);
           // var ideas = JsonConvert.DeserializeObject<List<Request>>(response);
           // return response;
        }
        public async Task<List<Request>> SearchRequestsAsync(SearchModel searchModel)
        {
            var client = new HttpClient();

            var json = await client.GetStringAsync("https://portalesp.smart-path.it/Portalesp/request/searchByExample" + searchModel);

            var requests = JsonConvert.DeserializeObject<List<Request>>(json);

            return requests;
        }
          
          public async Task PostIdeaAsync(SearchModel searchModel, string accessToken)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var json = JsonConvert.SerializeObject(searchModel);
            HttpContent content = new StringContent(json);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var response = await client.PostAsync("https://portalesp.smart-path.it/Portalesp/request/searchByExample", content);
            if (response.IsSuccessStatusCode)
            {
                //await Navigation.PopAsync();
                await Application.Current.MainPage.DisplayAlert("Message", "Idea Added", "ok");
            }
            else
            {
                //DisplayAlert("Message", "Data Failed To Save", "Ok");
                await Application.Current.MainPage.DisplayAlert("Message", "Failed to add Idea", "ok");
            }
            
        }
        */
    }
}
