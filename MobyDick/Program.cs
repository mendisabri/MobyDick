using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.Collections;
using System.Diagnostics;

namespace MobyDick
{
    class Program
    {
        static void Main(string[] args)
        {
            //Console.WriteLine("Dosyanın indireleceği linki giriniz");
            //string link = Console.ReadLine();
            string link = "http://www.gutenberg.org/files/58355/58355-8.txt";
            Download_File(link);

            string dosya = File.ReadAllText(Path.Combine(Environment.CurrentDirectory, "dosya.txt"));
            
            #region STRİNG_PARÇALA
            Console.WriteLine("String Parçalanıyor");

            char[] parcalamaNoktalari = { ',', '.', ';', ':', ' ', '\n', '!', '\'', '-', '?', '*', '(', ')', '[', ']', '{', '}', '<', '>',
                '\t', '\"', '\r', '_', '$', '€', '1', '2', '3', '4', '5', '6', '7', '8', '9', '0','&', '#','~' };
            string[] kelimeler = dosya.Split(parcalamaNoktalari);

            Console.WriteLine("String Parçalandı");
            #endregion

            #region SÖZCÜK_LİSTESİ_OLUŞTUR
            ArrayList sozcukListesi = new ArrayList();
            foreach (var kelime in kelimeler)
            {
                if (kelime.Equals("")||kelime.Length==1)
                    continue;
                    sozcukListesi.Add(kelime.ToUpper());
            }
            #endregion

            #region KELİME_SAY
            Stopwatch sw = new Stopwatch();
            sw.Start();
            Console.WriteLine("Kelimeler Sayılıyor");

            int kontrolBaslangic = 1, gecmeSayisi = 1;
            for (int i = 0; i < sozcukListesi.Count; i++)
            {
                string kontrolEdilen = sozcukListesi[i].ToString();
                for (int j = kontrolBaslangic; j < sozcukListesi.Count; j++)
                {
                    if (kontrolEdilen.Equals(sozcukListesi[j].ToString()))
                    {
                        gecmeSayisi++;
                        sozcukListesi.RemoveAt(j);
                    }
                }
                sozcukListesi[i] = $"<word><text>{kontrolEdilen}</text> <count>{gecmeSayisi}</count></word>";
                gecmeSayisi = 1;
                kontrolBaslangic++;
            }

            Console.WriteLine("Kelime Sayma İşlemi Tamamlandı");
            sw.Stop();
            Console.WriteLine("Kelimelerin Sayılma Süresi ~= " + sw.ElapsedMilliseconds/1000 + "sn");
            #endregion

            #region XML_OLUŞTUR
            sozcukListesi.Insert(0, "<words>");
            sozcukListesi.Insert(sozcukListesi.Count, "</words>");
            sozcukListesi.Insert(0, "<?xml version=\"1.0\"?>");

            Console.WriteLine("XML Dosyası Oluşturuluyor");
            Sonuc_Yaz(sozcukListesi);
            Console.WriteLine("XML Dosyası Oluşturuldu");
            #endregion

            Console.WriteLine("1. Kısım Bitti");
            
            Console.ReadKey();
        }

        private static void Sonuc_Yaz(ArrayList sozcukListesi)
        {
            if (File.Exists(@"D:\sonuc.xml"))
                File.Delete(@"D:\sonuc.xml");

            FileStream xml = new FileStream(@"D:\sonuc.xml", FileMode.OpenOrCreate, FileAccess.Write);
            StreamWriter yazici = new StreamWriter(xml);
            foreach (var sozcuk in sozcukListesi)
            {
                yazici.WriteLine(sozcuk);
            }
            yazici.Flush();
            yazici.Close();
            xml.Close();
        }

        static void Download_File(string link)
        {
            WebClient webClient = new WebClient();
            if (File.Exists(Path.Combine(Environment.CurrentDirectory, "dosya.txt")))
                File.Delete(Path.Combine(Environment.CurrentDirectory, "dosya.txt"));

            webClient.DownloadFile(link, Path.Combine(Environment.CurrentDirectory, "dosya.txt"));
            Console.WriteLine("Dosya indirildi");
        }

    }
}
