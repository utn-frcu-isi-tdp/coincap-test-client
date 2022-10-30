using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace CoinCapTestClient.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            // Establecimiento del protocolo ssl de transporte
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            // Url de ejemplo
            var mUrl = "https://api.coincap.io/v2/assets";

            // Se crea el request http
            HttpWebRequest mRequest = (HttpWebRequest)WebRequest.Create(mUrl);

            try
            {
                // Se ejecuta la consulta
                WebResponse mResponse = mRequest.GetResponse();

                // Se obtiene los datos de respuesta
                using (Stream responseStream = mResponse.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);

                    // Se parsea la respuesta y se serializa a JSON a un objeto dynamic
                    dynamic mResponseJSON = JsonConvert.DeserializeObject(reader.ReadToEnd());
                    

                    // Se iteran cada uno de los resultados.
                    foreach (var bResponseItem in mResponseJSON.data)
                    {
                        // De esta manera se accede a los componentes de cada item
                        // Se decodifican algunos elementos HTML
                        System.Console.WriteLine("Solo id -> {0}", HttpUtility.HtmlDecode(bResponseItem.id.ToString()));

                        // Se muestra por pantalla cada item completo
                        System.Console.WriteLine("Item completo -> {0}", bResponseItem);

                    }
                }
            }
            catch (WebException ex)
            {
                WebResponse mErrorResponse = ex.Response;
                using (Stream mResponseStream = mErrorResponse.GetResponseStream())
                {
                    StreamReader mReader = new StreamReader(mResponseStream, Encoding.GetEncoding("utf-8"));
                    String mErrorText = mReader.ReadToEnd();

                    System.Console.WriteLine("Error: {0}", mErrorText);
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("Error: {0}", ex.Message);
            }

            System.Console.ReadLine();
        }
    }
}