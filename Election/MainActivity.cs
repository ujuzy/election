using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Widget;

using Newtonsoft.Json;

using Xamarin.Essentials;

using System.Collections.Generic;
using System.Net;
using System.IO;
using Android.Support.V7.Widget;
using Android.Views;

namespace Election
{
    [Activity(Label = "@string/app_name", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class MainActivity : AppCompatActivity
    {
        List<CandidateData> listOfCandidates;

        private int m_TotalVotes = 0;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.activity_main);

            listOfCandidates = LoadData();
            listOfCandidates.RemoveAt(listOfCandidates.Count - 1);

            TextView textViewTotalVotes = FindViewById<TextView>(Resource.Id.totalVotes);
            textViewTotalVotes.Text += $" {m_TotalVotes}";

            var listData = FindViewById<RecyclerView>(Resource.Id.recyclerView);

            RecyclerView.LayoutManager layoutManager = new LinearLayoutManager(this);
            listData.SetLayoutManager(layoutManager);

            var adapter = new CustomAdapter(this, listOfCandidates, listData);
            listData.SetAdapter(adapter);
        }

        private List<CandidateData> LoadData()
        {
            var request = (HttpWebRequest)WebRequest.Create("https://adlibtech.ru/elections/api/getcandidates.php");
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";

            string deviceName = DeviceInfo.Name;
            string deviceId = DeviceInfo.Model;

            string postRequest = "device_id=" + deviceId + "&device_name=" + deviceName;
            byte[] byteArray = System.Text.Encoding.UTF8.GetBytes(postRequest);

            using (Stream dataStream = request.GetRequestStream())
            {
                dataStream.Write(byteArray, 0, byteArray.Length);
            }

            var response = (HttpWebResponse)request.GetResponse();

            List<CandidateData> result;

            using (Stream stream = response.GetResponseStream())
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    result = JsonConvert.DeserializeObject<List<CandidateData>>(reader.ReadToEnd());
                }
            }

            foreach (var i in result)
            {
                m_TotalVotes += (int)i.Votes;
            }

            foreach (var i in result)
            {
                i.Percent = (int)(i.Votes / m_TotalVotes * 100.0f);
            }

            response.Close();

            return result;
        }
    }
}

