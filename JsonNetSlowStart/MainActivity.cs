using System.IO;
using System.Diagnostics;

using Android.App;
using Android.Widget;
using Android.OS;

using Newtonsoft.Json;
using System.Threading.Tasks;

namespace JsonNetSlowStart
{
    [Activity(Label = "Json.Net slow start", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        Button parseJsonButton;
        TextView resultText;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.Main);

            parseJsonButton = FindViewById<Button>(Resource.Id.parseJsonButton);
            resultText = FindViewById<TextView>(Resource.Id.resultText);
            parseJsonButton.Click += ParseJson;
        }

        private async void ParseJson(object sender, System.EventArgs e)
        {
            resultText.Text = "Parsing...";
            var stopWatch = Stopwatch.StartNew();

            await Task.Run(() =>
            {
                using (var stream = Assets.Open("people.json"))
                using (var streamReader = new StreamReader(stream))
                {
                    var json = streamReader.ReadToEnd();
                    var people = JsonConvert.DeserializeObject<Person[]>(json);
                    foreach (var person in people)
                    {
                        System.Diagnostics.Debug.WriteLine(person.Name);
                    }
                }
            });

            stopWatch.Stop();
            resultText.Text = $"Took {stopWatch.ElapsedMilliseconds}ms";
        }
    }
}

