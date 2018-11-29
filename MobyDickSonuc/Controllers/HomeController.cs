using MobyDickSonuc.Models;
using System;
using System.Collections.Generic;
using System.Collections;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml;

namespace MobyDickSonuc.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            #region XML_OKUMA_HAZIRLIK
            FileStream file = new FileStream(@"D:\sonuc.xml", FileMode.Open);
            XmlDocument xml = new XmlDocument();
            xml.Load(file);
            XmlNodeList xmlNodeList = xml.GetElementsByTagName("word");
            Sozcukler ekle = new Sozcukler();
            List<Sozcukler> gonderilecek = new List<Sozcukler>();
            #endregion

            #region EN_FAZLA_GEÇEN_KELİMELERİ_TUTAN_DİZİ
            for (int i = 0; i < 10; i++)
            {
                ekle.Sozcuk = "asd";
                ekle.GecisSayisi = 0;
                gonderilecek.Add(ekle);
            }
            #endregion
            int x;
            #region EN_FAZLA_GECEN_10_KELİME_SEÇİMİ
            foreach (XmlNode okunan in xmlNodeList)
            {
                Sozcukler secilen = new Sozcukler();
                secilen.Sozcuk = okunan.SelectSingleNode("text").ChildNodes[0].Value.ToString();
                secilen.GecisSayisi = int.Parse(okunan.SelectSingleNode("count").ChildNodes[0].Value);
                for (int i = 0; i < 10; i++)
                {
                    if (gonderilecek[i].GecisSayisi < secilen.GecisSayisi)
                    {
                        gonderilecek[i] = secilen;
                        goto devam;
                    }
                }
                devam:
                x = 1;
            }
            #endregion

            file.Close();
            ViewBag.Message = gonderilecek;
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}