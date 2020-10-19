using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Net.Http;
using System.Windows;

namespace WpfAppHttpsServerComm
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
        }

        HttpClient client;
        private void ButtonConnect_Click(object sender, RoutedEventArgs e)
        {
            client = new HttpClient() { Timeout = TimeSpan.FromSeconds(10) };
            TextBoxResponse.Text = "";
        }

        private async void ButtonPost_Click(object sender, RoutedEventArgs e)
        {
            var parameters = new Dictionary<string, string>
            {
                {"cmd", TextBoxPostData.Text }
            };
            TextBoxResponse.Text = "";
            TextBoxStatus.Text = "";

            var sw = new System.Diagnostics.Stopwatch();
            using (var content = new FormUrlEncodedContent(parameters))
            {
                try
                {
                    sw.Start();
                    HttpResponseMessage response = await client.PostAsync(TextBoxUrl.Text, content);
                    TextBoxStatus.Text = response.ReasonPhrase;
                    HttpContent responseContent = response.Content;
                    using (var reader = new StreamReader(await responseContent.ReadAsStreamAsync()))
                    {
                        sw.Stop();
                        TextBoxResponse.Text = await reader.ReadToEndAsync();
                        TimeSpan ts = sw.Elapsed;
                        TextBoxResponse.Text += " / " + ts.Seconds + "秒" + ts.Milliseconds + "m秒";
                    }
                }
                catch(Exception ex)
                {
                    TextBoxStatus.Text = ex.Message;
                }
            }
        } 
    }
}
