using Android.App;
using Android.Views;
using Android.Widget;
using Android.Graphics;

using System.Collections.Generic;
using System.Net;

namespace Election
{
    public class CustomAdapter : BaseAdapter
    {
        private Activity activity;
        private List<CandidateData> candidates;

        public CustomAdapter(Activity activity, List<CandidateData> candidates)
        {
            this.activity = activity;
            this.candidates = candidates;
        }

        public override int Count
        {
            get { return candidates.Count; }
        }

        public override Java.Lang.Object GetItem(int position)
        {
            return null;
        }

        public override long GetItemId(int position)
        {
            return candidates[position].Id;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var view = convertView ?? activity.LayoutInflater.Inflate(Resource.Layout.candidate_template, parent, false);

            var firstName = view.FindViewById<TextView>(Resource.Id.candidateFirstName);
            var secondName = view.FindViewById<TextView>(Resource.Id.candidateSecondName);
            var thirdName = view.FindViewById<TextView>(Resource.Id.candidateThirdName);

            var votes = view.FindViewById<TextView>(Resource.Id.candidateVotes);
            var percent = view.FindViewById<TextView>(Resource.Id.candidatePercent);

            var photo = view.FindViewById<ImageView>(Resource.Id.candidatePhoto);

            firstName.Text = candidates[position].FirstName;
            secondName.Text = candidates[position].SecondName;
            thirdName.Text = candidates[position].ThirdName;

            votes.Text = "Голосов: " + candidates[position].Votes;
            percent.Text = "Процент: " + candidates[position].Percent + "%";

            var photoBitmap = GetImageBitmapFromUrl("https://adlibtech.ru/elections/upload_images/" + candidates[position].Image);
            photo.SetImageBitmap(photoBitmap);

            return view;
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
