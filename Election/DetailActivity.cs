using System.Net;
using System.Threading.Tasks;
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
            photo.SetImageBitmap(CandidateDetail.Image);

            TextView party = FindViewById<TextView>(Resource.Id.party);
            party.Text = "Партия: " + CandidateDetail.Party;

            TextView web = FindViewById<TextView>(Resource.Id.web);
            web.Text = "Веб-сайт: " + CandidateDetail.Web;

            TextView description = FindViewById<TextView>(Resource.Id.description);
            description.Text = CandidateDetail.Descriptions;

            Button backButton = FindViewById<Button>(Resource.Id.back);

            backButton.Click += (sender, args) =>
            {
                OnBackPressed();
            };
        }
    }
}
