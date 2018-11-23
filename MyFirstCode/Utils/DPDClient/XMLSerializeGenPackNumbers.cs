using System;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.IO;


namespace MyFirstCode.Utils.DPDClient
{

    public class Utf8StringWriter : StringWriter
    {
        public override Encoding Encoding => Encoding.UTF8;
    }


    [XmlRoot(ElementName = "Envelope", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
    public class SOAPEnvelope
    {

        [XmlNamespaceDeclarations]
        public XmlSerializerNamespaces xmlns = new XmlSerializerNamespaces();

        public SOAPEnvelope()
        {
            xmlns.Add("SOAP-ENV", "http://schemas.xmlsoap.org/soap/envelope/");
            xmlns.Add("ns1", "http://dpdservices.dpd.com.pl/");

        }

        public SAOPBody Body;
    }

    [XmlRoot(ElementName = "ns1", Namespace = "http://dpdservices.dpd.com.pl/")]
    public class SAOPBody
    {
        public genPackNumbers generatePackagesNumbersV2;
    }

    [XmlRoot(ElementName = "", Namespace = "")]
    public class genPackNumbers
    {
        public openUML openUMLV1;
        public string pkgNumsGenerationPolicyV1;
        public string langCode;
        public authData authDataV1;
    }

    public class openUML
    {
        public packages packages;
    }

    public class packages
    {
        [XmlElement]
        public parcels[] parcels;
        public string payerType;
        public Receiver receiver;
        public string ref1;
        public string ref2;
        public string ref3;
        public Sender sender;
    }


    public class parcels
    {
        public string content;
        public string customerData1;
        public string weight;
    }

    public class Receiver
    {
        public string address;
        public string city;
        public string company;
        public string countryCode;
        public string email;
        public string name;
        public string phone;
        public string postalCode;
    }

    public class Sender
    {
        public string address;
        public string city;
        public string company;
        public string countryCode;
        public string email;
        public string fid;
        public string name;
        public string phone;
        public string postalCode;
    }

    public class authData
    {
        public string login;
        public string masterFid;
        public string password;
    }


    public class XMLGeneratePackageNumbers
    {

        public string XMLSerializeGenPackNumbers(authData auth, Sender senderData, Receiver receiverData, parcels[] parcelsList, string PayerType = "SENDER")
        {

            XmlSerializer serializer = new XmlSerializer(typeof(SOAPEnvelope));

            SOAPEnvelope se = new SOAPEnvelope();
            SAOPBody body = new SAOPBody();

            var gpn = new genPackNumbers();

            gpn.pkgNumsGenerationPolicyV1 = "ALL_OR_NOTHING";
            gpn.langCode = "PL";

            var packages = new packages
            {
                parcels = parcelsList,
                payerType = PayerType,
                receiver = receiverData,
                sender = senderData,
                ref1 = "",
                ref2 = "",
                ref3 = ""
            };

            var openUML = new openUML();
            openUML.packages = packages;

            gpn.openUMLV1 = openUML;
            gpn.authDataV1 = auth;

            body.generatePackagesNumbersV2 = gpn;

            se.Body = body;

            using (StringWriter textWriter = new Utf8StringWriter())
            {
                serializer.Serialize(textWriter, se);
                return textWriter.ToString();
            }


        }


        public object XMLDeserializeGenPackNumbers(string ResultXML)
        {
            var value = XDocument.Parse(ResultXML);


            return value;


        }



    }

  
}
