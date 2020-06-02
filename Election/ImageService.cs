using System.Net;
using System.Threading.Tasks;

using Android.Graphics;

namespace Election
{
    public static class ImageService
    {
        private static Bitmap GetImageBitmapFromUrl(string url)
        {
            Bitmap imageBitmap = null;

            using (var webClient = new WebClient())
            {
                var imageBytes = webClient.DownloadData(url);
                if (imageBytes != null && imageBytes.Length > 0)
                {
                    imageBitmap = BitmapFactory.DecodeByteArray(imageBytes, 0, imageBytes.Length);
                }
            }

            return imageBitmap;
        }

        public static async Task<Bitmap> GetImageBitmapFromUrlAsync(string url)
        {
            var image = await Task.FromResult<Bitmap>(GetImageBitmapFromUrl(url));
            return image;
        }
    }
}
