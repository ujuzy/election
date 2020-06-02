using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;

using Xamarin.Essentials;

namespace Election
{
    public class CustomAdapter : RecyclerView.Adapter
    {
        private List<CandidateData> m_Candidates;
        private RecyclerView m_RecyclerView;
        private Context m_Context;

        public event EventHandler<int> ItemClick;

        public CustomAdapter(Context context, List<CandidateData> candidates, RecyclerView recyclerView)
        {
            m_Candidates = candidates;
            m_RecyclerView = recyclerView;
            m_Context = context;
        }

        public override int ItemCount
        {
            get { return m_Candidates.Count; }
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            MyView myHolder = holder as MyView;

            myHolder.firstName.Text = m_Candidates[position].FirstName;
            myHolder.secondName.Text = m_Candidates[position].SecondName;
            myHolder.thirdName.Text = m_Candidates[position].ThirdName;
            myHolder.votes.Text = "Голосов: " + m_Candidates[position].Votes;
            myHolder.percent.Text = "Процент: " + m_Candidates[position].Percent + "%";

            Task<Bitmap> outerTask = GetImageBitmapFromUrlAsync("https://adlibtech.ru/elections/upload_images/" + m_Candidates[position].Image);
            outerTask.ContinueWith(task =>
            {
                Bitmap imageBitmap = task.Result;

                myHolder.photo.SetImageBitmap(imageBitmap);
            });

            if (m_Candidates[position].IsVoiceSent == 1)
            {
                myHolder.checkbox.SetImageResource(Resource.Drawable.checkbox_full);
            }
            else
            {
                myHolder.checkbox.SetImageResource(Resource.Drawable.checkbox_empty);
            }

            myHolder.mainView.Click -= Candidate_Click;
            myHolder.mainView.Click += Candidate_Click;

            myHolder.checkbox.Click += (sender, e) =>
            {
                int previous = m_Candidates.FindIndex(candidate => candidate.IsVoiceSent == 1);

                if (previous != -1)
                {
                    m_Candidates[previous].IsVoiceSent = 2;
                }

                m_Candidates[position].IsVoiceSent = 1;

                SendVoice();

                Toast.MakeText(m_Context, $"Вы проголосовали за {m_Candidates[position].SecondName} {m_Candidates[position].FirstName} {m_Candidates[position].ThirdName}", ToastLength.Short).Show();
            };
        }

        private void Candidate_Click(object sender, EventArgs e)
        {
            int position = m_RecyclerView.GetChildAdapterPosition((View)sender);
            CandidateData candidateClicked = m_Candidates[position];

            CandidateDetail.Id = candidateClicked.Id;

            foreach (var i in m_Candidates)
            {
                if (CandidateDetail.Id == i.Id)
                {
                    CandidateDetail.FirstName = i.FirstName;
                    CandidateDetail.SecondName = i.SecondName;
                    CandidateDetail.ThirdName = i.ThirdName;

                    CandidateDetail.Image = i.Image;

                    CandidateDetail.Party = i.Party;

                    CandidateDetail.Web = i.Web;

                    CandidateDetail.Descriptions = i.Descriptions;

                    break;
                }
            }

            m_Context.StartActivity(new Intent(Application.Context, typeof(DetailActivity)));
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View row = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.candidate_template, parent, false);

            var FirstName = row.FindViewById<TextView>(Resource.Id.candidateFirstName);
            var SecondName = row.FindViewById<TextView>(Resource.Id.candidateSecondName);
            var ThirdName = row.FindViewById<TextView>(Resource.Id.candidateThirdName);
            var Votes = row.FindViewById<TextView>(Resource.Id.candidateVotes);
            var Percent = row.FindViewById<TextView>(Resource.Id.candidatePercent);
            var Photo = row.FindViewById<ImageView>(Resource.Id.candidatePhoto);
            var CheckBox = row.FindViewById<ImageButton>(Resource.Id.checkbox);

            MyView view = new MyView(row)
            {
                firstName = FirstName,
                secondName = SecondName,
                thirdName = ThirdName,
                votes = Votes,
                percent = Percent,
                photo = Photo,
                checkbox = CheckBox
            };

            return view;
        }

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

        void SendVoice()
        {
            int previousCandidate, currentCandidate;
            GetCurrentAndPreviousCandidate(out currentCandidate, out previousCandidate);

            int previousCandidateIndex = MainActivity.listOfCandidates.FindIndex(candidate => candidate.Id == previousCandidate);

            if (previousCandidateIndex != -1)
            {
                MainActivity.listOfCandidates[previousCandidateIndex].IsVoiceSent = 0;
            }

            var request = (HttpWebRequest)WebRequest.Create("https://adlibtech.ru/elections/api/getcandidates.php");
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";

            string deviceName = DeviceInfo.Name;
            string deviceId = DeviceInfo.Model;

            string postRequest = "device_id=" + deviceId + "&device_name=" + deviceName + "&candidate_id=" + currentCandidate.ToString() + "&last_id=" + previousCandidate.ToString();
            byte[] byteArray = System.Text.Encoding.UTF8.GetBytes(postRequest);

            using (Stream dataStream = request.GetRequestStream())
            {
                dataStream.Write(byteArray, 0, byteArray.Length);
            }

            NotifyDataSetChanged();
        }

        private void GetCurrentAndPreviousCandidate(out int current, out int previous)
        {
            current = MainActivity.listOfCandidates[MainActivity.listOfCandidates.FindIndex(candidate => candidate.IsVoiceSent == 1)].Id;

            previous = MainActivity.listOfCandidates.FindIndex(candidate => candidate.IsVoiceSent == 2);

            if (previous == -1)
            {
                previous = 0;
            }
            else
            {
                previous = MainActivity.listOfCandidates[previous].Id;
            }
        }
    }
}
