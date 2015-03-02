using APIMTraceViewer.Shared;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace APIMTraceViewer
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

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            //Could be MVVM but feels like overkill given the simplicity.
            EchoApiService echoTester = new EchoApiService();

            //Issue a request and get trace, disabling button to avoid concurrency. 
            StartBtn.IsEnabled = false;
            var result = await echoTester.GetRequestWithTrace(UrlTxtBx.Text);
            StartBtn.IsEnabled = true;

            //Show errors, if any
            if (result.HasError)
            {
                MessageBox.Show(string.Format("An error occured '{0}' Exception Details: '{1}'", result.ErrorDetails.Message, result.ErrorDetails.ToString()));
            }

            //Update the UI  
            TraceTxt.Text = result.TraceString;
            HeadersTxt.Text = result.ResponseMessage.Headers.ToString();
            BodyTxt.Text = result.BodyContent;
        }
    }
}
