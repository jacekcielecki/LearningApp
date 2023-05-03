using Newtonsoft.Json;
using System.Text;

namespace LearningApp.WebApi.Tests.Helpers
{
    public static class HttpContentHelper
    {
        public static HttpContent ToJsonHttpContent(this object obj)
        {
            var json = JsonConvert.SerializeObject(obj);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            return httpContent;
        }

        public static T DeserializeHttpContent<T>(this HttpContent httpContent)
        {
            var responseContent = httpContent.ReadAsStringAsync().Result;
            return JsonConvert.DeserializeObject<T>(responseContent) ?? throw new InvalidOperationException();
        }
    }
}
