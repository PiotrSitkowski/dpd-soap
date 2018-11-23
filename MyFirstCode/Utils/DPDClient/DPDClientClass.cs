using System;
using System.IO;
using System.Net;
using System.Text;
using System.Net.Http;
using Newtonsoft.Json;



namespace MyFirstCode.Utils.DPDClient
{
    public class DPDClientClass
    {
        protected string SOAPURL = "https://dpdservicesdemo.dpd.com.pl/DPDPackageObjServicesService/DPDPackageObjServices?WSDL";

        protected authData authorize;
        protected Sender mySenderData;


        public void setAuth(authData auth)
        {
            authorize = auth;
        }

        public void setSender(Sender senderData)
        {
            mySenderData = senderData;
        }

        public string generatePackageNumbers(Receiver receceiverData, parcels[] parcelsList) 
        {
            var XMLGenerate = new XMLGeneratePackageNumbers();

            var soapString = XMLGenerate.XMLSerializeGenPackNumbers(authorize, mySenderData, receceiverData, parcelsList);

            //return soapString;

            var result = SOAPCall(soapString);

            var resObj = XMLGenerate.XMLDeserializeGenPackNumbers(result);

            return Dump(resObj);
        }



        private string SOAPCall(string SOAPStringXML)
        {

            var result = "none";

            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(SOAPURL);

            req.ProtocolVersion = HttpVersion.Version11;
            req.ContentType = "text/xml;charset=\"utf-8\"";
            req.Accept = "text/xml";
            req.KeepAlive = true;
            req.Method = "POST";

            using (Stream stm = req.GetRequestStream())
            {
                using (StreamWriter stmw = new StreamWriter(stm))
                    stmw.Write(SOAPStringXML);
            }
            using (StreamReader responseReader = new StreamReader(req.GetResponse().GetResponseStream()))
            {
                result = responseReader.ReadToEnd();

            }
            return result;
        }


        public static string Dump(object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }
    }
}
