using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MyFirstCode.Models;
using MyFirstCode.Utils.DPDClient;

namespace MyFirstCode.Controllers
{
    public class HomeController : Controller
    {
    
        public IActionResult Index()
        {


            DPDClientClass DPD = new DPDClientClass();
           
            DPD.setAuth(
                new authData
                {
                    login = "test",
                    masterFid = "1495",
                    password = "thetu4Ee"
                }
            );

            DPD.setSender(
                new Sender
                {
                    address = "Nieznana 0/11",
                    city = "Warszawa",
                    company = "OjtamOjtam S.A.",
                    countryCode = "PL",
                    email = "biuro@ojtam.pl",
                    fid = "1495",
                    name = "Roman Nieznany",
                    phone = "+22123456",
                    postalCode = "00950"
                }
            );


            Receiver receiverData = new Receiver
            {
                address = "Fikcyjna 0b/c",
                city = "Czestochowa",
                company = "Krzak Sp. z o.o.",
                countryCode = "PL",
                email = "biuro@krzak-fikcyjny.pl",
                name = "Bogdan Nerka",
                phone = "+34 555221112",
                postalCode = "42215"
            };

            parcels[] parcelsList = {
                new parcels {
                    content = "Kieliszki do szampana",
                    customerData1 = "Uwaga szkło!",
                    weight = "8"
                },
                new parcels {
                    content = "Wizytówki",
                    weight = "500"
                },
                new parcels {
                    content = "Gwoździe",
                    customerData1 = "paczka - 1 kg",
                    weight = "60"
                }
            };

            var result = DPD.generatePackageNumbers(receiverData, parcelsList);

            ViewData["Message"] = result;
            return View();
        }



    }

   

}
