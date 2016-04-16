using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Data.Json;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Under_Water_Quiz
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            newQuestion_Click(null, null);

            DispatcherTimerSetup();

        }


        DispatcherTimer dispatcherTimer;
        DateTimeOffset startTime;
        DateTimeOffset lastTime;
        DateTimeOffset stopTime;
        int timesTicked = 1;
        int timesToTick = 200;

        public void DispatcherTimerSetup()
        {
            dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += dispatcherTimer_Tick;
            dispatcherTimer.Interval = TimeSpan.FromMilliseconds(2000);
            //IsEnabled defaults to false 
            startTime = DateTimeOffset.Now;
            lastTime = startTime;
            dispatcherTimer.Start();
            //IsEnabled should now be true after calling start 
        }

        void dispatcherTimer_Tick(object sender, object e)
        {
            DateTimeOffset time = DateTimeOffset.Now;
            TimeSpan span = time - lastTime;
            lastTime = time;
            //Time since last tick should be very very close to Interval 
            timesTicked++;
            if (timesTicked > timesToTick)
            {
                stopTime = time;
                dispatcherTimer.Stop();
                //IsEnabled should now be false after calling stop 
                span = stopTime - startTime;

            }
            changeDynamic();
        }

        static string answerValue = "";

        private void option_Click(object sender, RoutedEventArgs e)
        {
            greetingOutput.Text = "Answer: " + answerValue;

        }

        private async void newQuestion_Click(object sender, RoutedEventArgs e)
        {
            getCount();
            string url = "https://quizdevmov.herokuapp.com/get";

            Windows.Web.Http.HttpClient clientOb = new Windows.Web.Http.HttpClient();
            Uri connectionUrl = new Uri(url);
            //text.Text  = pairs;
            Dictionary<string, string> pairs = new Dictionary<string, string>();
            pairs.Add("table", "underwater");
            Windows.Web.Http.HttpFormUrlEncodedContent formContent = new Windows.Web.Http.HttpFormUrlEncodedContent(pairs);
            Windows.Web.Http.HttpResponseMessage response = await clientOb.PostAsync(connectionUrl, formContent);
            if (response.IsSuccessStatusCode)
            {
                //greetingOutput.Text = response.Content.ToString();

                JsonArray root = JsonValue.Parse(response.Content.ToString()).GetArray();
                for (uint i = 0; i < root.Count; i++)
                {
                    string question = root.GetObjectAt(i).GetNamedString("question");
                    string option1 = root.GetObjectAt(i).GetNamedString("option1");
                    string option2 = root.GetObjectAt(i).GetNamedString("option2");
                    string option3 = root.GetObjectAt(i).GetNamedString("option3");
                    string option4 = root.GetObjectAt(i).GetNamedString("option4");
                    string answer = root.GetObjectAt(i).GetNamedString("answer");

                    this.getNewQuestion.Text = question;
                    this.option1.Content = "A: " + option1;
                    this.option2.Content = "B: " + option2;
                    this.option3.Content = "C: " + option3;
                    this.option4.Content = "D: " + option4;

                    answerValue = answer;

                }

            }
        }


        private async void getCount()
        {
            string url = "https://quizdevmov.herokuapp.com/getcount";

            Windows.Web.Http.HttpClient clientOb = new Windows.Web.Http.HttpClient();
            Uri connectionUrl = new Uri(url);
            //text.Text  = pairs;
            Dictionary<string, string> pairs = new Dictionary<string, string>();
            pairs.Add("table", "underwater");
            Windows.Web.Http.HttpFormUrlEncodedContent formContent = new Windows.Web.Http.HttpFormUrlEncodedContent(pairs);
            Windows.Web.Http.HttpResponseMessage response = await clientOb.PostAsync(connectionUrl, formContent);
            if (response.IsSuccessStatusCode)
            {
                //greetingOutput.Text = response.Content.ToString();

                JsonArray root = JsonValue.Parse(response.Content.ToString()).GetArray();
                for (uint i = 0; i < root.Count; i++)
                {
                    string count = root.GetObjectAt(i).GetNamedString("count");
                    this.greetingOutput.Text = "Currently, Our Servers have " + count + " questions, new questions are added every week";

                }

            }
        }

        private async void changeDynamic()
        {
            string url = "https://quizdevmov.herokuapp.com/get";

            Windows.Web.Http.HttpClient clientOb = new Windows.Web.Http.HttpClient();
            Uri connectionUrl = new Uri(url);
            //text.Text  = pairs;
            Dictionary<string, string> pairs = new Dictionary<string, string>();
            pairs.Add("table", "underwater");
            Windows.Web.Http.HttpFormUrlEncodedContent formContent = new Windows.Web.Http.HttpFormUrlEncodedContent(pairs);
            Windows.Web.Http.HttpResponseMessage response = await clientOb.PostAsync(connectionUrl, formContent);
            if (response.IsSuccessStatusCode)
            {
                //greetingOutput.Text = response.Content.ToString();

                JsonArray root = JsonValue.Parse(response.Content.ToString()).GetArray();
                for (uint i = 0; i < root.Count; i++)
                {
                    string option1 = root.GetObjectAt(i).GetNamedString("option1");

                    this.loveoption.Text = "We love " + option1;

                }

            }
        }

    }
}
