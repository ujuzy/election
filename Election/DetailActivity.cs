using System.Net;

using Android.App;
using Android.Graphics;
using Android.OS;
using Android.Widget;

namespace Election
{
    [Activity(Label = "DetailActivity")]
    public class DetailActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.activity_detail);

            TextView secondName = FindViewById<TextView>(Resource.Id.secondName);
            secondName.Text = CandidateDetail.SecondName;

            TextView firstAndThirdName = FindViewById<TextView>(Resource.Id.firstAndThirdName);
            firstAndThirdName.Text = CandidateDetail.FirstName + " " + CandidateDetail.ThirdName;

            ImageView photo = FindViewById<ImageView>(Resource.Id.photo);
            var photoBitmap = GetImageBitmapFromUrl("https://adlibtech.ru/elections/upload_images/" + CandidateDetail.Image);
            photo.SetImageBitmap(photoBitmap);

            TextView party = FindViewById<TextView>(Resource.Id.party);
            party.Text = "Партия: " + CandidateDetail.Party;

            TextView description = FindViewById<TextView>(Resource.Id.description);
            description.Text = CandidateDetail.Descriptions;

            Button backButton = FindViewById<Button>(Resource.Id.back);

            backButton.Click += (sender, args) =>
            {
                OnBackPressed();
            };
        }

        private Bitmap GetImageBitmapFromUrl(string url)
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
    }
}
