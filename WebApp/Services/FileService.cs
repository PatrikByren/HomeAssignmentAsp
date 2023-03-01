using Newtonsoft.Json;
using System.Diagnostics;
using System.Linq.Expressions;

namespace WebApp.Services
{
    public class FileService
    {
        private string _filePath;

        public FileService(string filePath)
        {
            _filePath = filePath;
        }

        /*public async Task Save(string content)
        {
            try
            {
                if (File.Exists(_filePath))
                {
                    using var sw = new StreamWriter(_filePath);
                    sw.WriteLine(JsonConvert.SerializeObject(content));
                }
            }
            catch (Exception ex) { Debug.WriteLine(ex.Message); }

        }
        public string Read()
        {
            try
            {
                if (File.Exists(_filePath))
                {
                    using var sr = new StreamReader(_filePath);
                    return sr.ReadToEnd();
                }
                return string.Empty;
            }

            catch (Exception ex) { Debug.WriteLine(ex.Message); return string.Empty; }
        }*/
    }
}
