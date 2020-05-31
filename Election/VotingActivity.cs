
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

            OnBackPressed();
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
