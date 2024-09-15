using WebPizzaSite.Interfaces;

namespace WebPizzaSite.Services;

public class ImageWorker : IImageWorker
{
    private readonly IWebHostEnvironment _webHostEnvironment;

    public ImageWorker(IWebHostEnvironment webHostEnvironment)
    {
        _webHostEnvironment = webHostEnvironment;
    }

    public string ImageSave(string url)
    {
        string imageName = Guid.NewGuid() + ".jpg";
        try
        {
            using (HttpClient client = new())
            {
                HttpResponseMessage response = client.GetAsync(url).Result;

                if (response.IsSuccessStatusCode)
                {
                    var path = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", imageName);
                    var dir = Path.GetDirectoryName(path);
                    if (!Directory.Exists(dir) && dir != null)
                    {
                        Directory.CreateDirectory(dir);
                    }

                    byte[] imageBytes = response.Content.ReadAsByteArrayAsync().Result;
                    File.WriteAllBytes(path, imageBytes);
                }
                else
                {
                    Console.WriteLine($"Failed to retrieve image. Status code: {response.StatusCode}");
                    return String.Empty;
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to retrieve image. Status code: {ex.Message}");
            return String.Empty;
        }
        return imageName;
    }
}