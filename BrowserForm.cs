using CefSharp;
using CefSharp.WinForms;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;



namespace ParserBooking
{
    public partial class BrowserForm : Form
    {
        ChromiumWebBrowser browser;
        public BrowserForm()
        {
            InitializeComponent();
            Location = new Point(1500, 1500);
            InitializeChromium();
        }

        private void InitializeChromium()
        {
            var settings = new CefSettings();
            Cef.Initialize(settings);
            browser = new ChromiumWebBrowser();
            Controls.Add(browser);
            browser.Dock = DockStyle.Fill;
            DesktopLocation = new Point(1500, 1500);
        }

        public async Task<string> Algorithm(CancellationToken cts, string location, string start, string finish, BigWriter writer, bool hotels)
        {
            //var writer = new BigWriter();
            var formatter = new URLFormatter(start, finish, location);
            var allProps = new List<Property>();
            //Initiate
            var url = formatter.URL;
            await LoadPageAsync(url);

            var mainScript = "";
            using (StreamReader r = new StreamReader("RoomTypes.js"))
            {
                mainScript = await r.ReadToEndAsync();
            }
            using (StreamReader r = new StreamReader("ChooseProp.js"))
            {
                await EvaluateScript($"var hotels = {hotels.ToString().ToLower()};var apart = {(!hotels).ToString().ToLower()};" + await r.ReadToEndAsync());
            }
            using (StreamReader r = new StreamReader("AvailableOnly.js"))
            {
                await EvaluateScript(await r.ReadToEndAsync());
            }
            url = browser.Address;
            await LoadPageAsync(url);
            var addition = "";
            string exceps = "";
            int excepCounter = 0;
            int lastk = 0;
            for (int k = 0; k < 99999 + 1; k++)
            {
                var hotelnum = Convert.ToInt16(await EvaluateScript("document.getElementsByClassName('hotel_name_link url').length;")); ;
                while(hotelnum == 0)
                {
                    hotelnum = Convert.ToInt16(await EvaluateScript("document.getElementsByClassName('hotel_name_link url').length;"));
                    Thread.Sleep(1000);
                }
                //Add all urls of hotel on the current page
                var hotelURLs = new List<string>();
                for (int s = 0; s < hotelnum; s++)
                {
                    hotelURLs.Add(formatter.BaseURL + await EvaluateScript($"document.getElementsByClassName('hotel_name_link url')[{s}].getAttribute('href');"));
                }
                for (int i = 0; i < hotelnum; i++)
                {
                    if (cts.IsCancellationRequested)
                    {
                        throw new OperationCanceledException("Парсинг остановлен");
                    }
                    //LoadPage of hotel
                    await LoadPageAsync(hotelURLs[i]);

                    //MainScript is working
                    var json = await EvaluateScript(mainScript);
                    try
                    {
                        if (json.Equals("fuck"))
                        {
                            throw new InvalidOperationException("You were fucked");
                        }
                        var property = JsonConvert.DeserializeObject<Property>(json);
                        foreach (var item in allProps)
                        {
                            if (item.Name.Equals(property.Name))
                            {
                                throw new InvalidDataException(i.ToString());
                            }
                        }
                        allProps.Add(property);
                        writer.Write(property);

                    }
                    catch(InvalidDataException)
                    {
                        excepCounter++;
                    }
                    catch (InvalidOperationException e)
                    {
                        exceps += e.Message + '\t' + hotelURLs[i] + '\n';
                        excepCounter++;
                    }
                    catch (Exception e)
                    {
                        exceps += e.Message + '\t' + hotelURLs[i] + '\n';
                    }

                }
                if(addition.Equals(""))
                {
                    await LoadPageAsync(url);
                }
                else
                {
                    await LoadPageAsync(formatter.BaseURL + addition);
                }
                lastk = k;
                addition = await EvaluateScript($"var counter = 0;while(counter != 100)" +
                    "{if(document.getElementsByClassName('bui-pagination__link sr_pagination_link')[++counter].hasChildNodes()){if(parseInt(document.getElementsByClassName('bui-pagination__link sr_pagination_link')[counter].childNodes[2].innerText) == " +
                     $"{k + 2})" + "{break;}}}document.getElementsByClassName('bui-pagination__link sr_pagination_link')[counter].getAttribute('href');");
                if (addition.Equals("fuck"))
                {
                    break;
                }
                await LoadPageAsync(formatter.BaseURL + addition);
            }
            using (StreamWriter sw = new StreamWriter("exceptions.txt"))
            {
                await sw.WriteAsync(exceps);
                await sw.WriteAsync($"\nexcepCounter:{excepCounter}");
                await sw.WriteAsync($"\nlastk:{lastk}");
            }
            return null;
        }

        async Task<string> EvaluateScript(string script)
        {
            string toReturn = null;
            //We need to bu sure that V8 was fully loaded
            while (true)
            {
                if (browser.CanExecuteJavascriptInMainFrame)
                {
                    await browser.EvaluateScriptAsync(script).ContinueWith(x =>
                    {
                        var response = x.Result;

                        if (response.Success && response.Result != null)
                        {
                            toReturn = response.Result.ToString();
                        }
                    });
                    break;
                }
                else Thread.Sleep(500);
            }

            if (toReturn == null)
            {
                toReturn = "fuck";
            }
            return toReturn;
        }

        private Task LoadPageAsync(string address = null)
        {
            var tcs = new TaskCompletionSource<bool>();

            EventHandler<LoadingStateChangedEventArgs> handler = null;
            handler += (sender, args) =>
            {
                //Wait for while page to finish loading not just the first frame
                if (!args.IsLoading)
                {
                    browser.LoadingStateChanged -= handler;
                    tcs.TrySetResult(true);
                }
            };

            browser.LoadingStateChanged += handler;

            if (!string.IsNullOrEmpty(address))
            {
                browser.Load(address);
            }
            return tcs.Task;
        }
    }
}
