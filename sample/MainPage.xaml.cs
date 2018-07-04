using System;
using System.Collections.Generic;
using System.Net;
using Lextm.SharpSnmpLib;
using Lextm.SharpSnmpLib.Messaging;
using Xamarin.Forms;

namespace sample
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        public void Button_Clicked(object sender, EventArgs args)
        {
            try
            {
                btnTest.IsEnabled = false;
                txtResult.Text = string.Empty;
                if (!IPAddress.TryParse(txtAddress.Text, out IPAddress address))
                {
                    txtResult.Text = "Please provide a valid IP address.";
                    return;
                }

                if (!int.TryParse(txtPort.Text, out int port) || port <= 0 || port > 65535)
                {
                    txtResult.Text = "Please provide a valid port number.";
                    return;
                }

                var result = Messenger.Get(
                    VersionCode.V1,
                    new IPEndPoint(address, port),
                    new OctetString(txtCommunity.Text),
                    new List<Variable> { new Variable(new ObjectIdentifier("1.3.6.1.2.1.1.1.0")) },
                    5000);
                if (result.Count != 1)
                {
                    txtResult.Text = "Invalid response data.";
                    return;
                }

                txtResult.Text = result[0].Data.ToString();
            }
            catch (Exception ex)
            {
                txtResult.Text = ex.Message;
            }
            finally
            {
                btnTest.IsEnabled = true;
            }
        }
    }
}
