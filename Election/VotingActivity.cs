
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;

using Android.App;
using Android.OS;

using Xamarin.Essentials;

namespace Election
{
    [Activity(Label = "VotingActivity", NoHistory = true)]
    public class VotingActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SendVoice();
        }

        protected override void OnResume()
        {
            base.OnResume();
            
        }

        void SendVoice()
        {
            int previousCandidate, currentCandidate;
            GetCurrentAndPreviousCandidate(out currentCandidate, out previousCandidate);

            int index = MainActivity.listOfCandidates.FindIndex(x => x.IsVoiceSent == 2);

            if (index != -1)
            {
                MainActivity.listOfCandidates[index].IsVoiceSent = 0;
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

            OnBackPressed();
        }

        private void GetCurrentAndPreviousCandidate(out int current, out int previous)
        {
            current = MainActivity.listOfCandidates[MainActivity.listOfCandidates.FindIndex(x => x.IsVoiceSent == 1)].Id;

            int index = MainActivity.listOfCandidates.FindIndex(x => x.IsVoiceSent == 2);

            if (index == -1)
            {
                previous = 0;
            }
            else
            {
                previous = MainActivity.listOfCandidates[index].Id;
            }
        }
    }
}
