using SixLabors.ImageSharp;
using System.Net.Http;

namespace e_commerce.Helpers
{
    public class FilesManagementHelper
    {
        private readonly HttpClient _httpClient;

        public FilesManagementHelper(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<bool> IsValidImageAsync(string url)
        {
            try
            {
                var response = await _httpClient.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    var stream = await response.Content.ReadAsStreamAsync();
                    var image = Image.Load(stream);
                    return true;
                }
                return false;
            }
            catch{  return false;}
        }
    }
}
