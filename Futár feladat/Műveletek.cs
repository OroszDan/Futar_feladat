using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Futár_feladat
{
    class Műveletek
    {
        //a fájlban az első sorban megadott adatok
        int Futar1Kezbesitesszam;  //az első futár kézbesítésszáma
        int Futar2Kezbesitesszam;  //a második futár kézbesítésszáma
        int SzuksIdo;              // a kiszállításhoz szükséges idő 

        int Talalkozasdb;  //ide mentem ki a találkozások számát

        string[] Kezbesitesadatok;  //ebbe a tömbbe mentem a soronkénti adatokat 

        string[] Eredmenytomb = new string[0]; //létrehozok egy tömböt és ide mentem ki hogy soronként a 
                                               //futárok hanyadik kézbesítésükkör találkoztak (2db szám)
        Futár Futar1;
        Futár Futar2; 

        public Műveletek()
        {
            StreamReader sr = new StreamReader("adatok.txt", Encoding.Default);   //beolvasom a fájlt soronként

            string[] alapadatok = sr.ReadLine().Split(' ');   //először csak az első sort olvasom be és fel is tördelem 3 részre

            Futar1Kezbesitesszam = int.Parse(alapadatok[0]);  //beolvasom az első számot ami megadja az első futár kézbesítéseinek számát
            Futar2Kezbesitesszam = int.Parse(alapadatok[1]);  //beolvasom a második számot ami megadja a második futár kézbesítéseinek számát
            SzuksIdo = int.Parse(alapadatok[2]);  //beolvasom az harmadik számot ami megadja a kiszállításhoz szükséges időt

            Kezbesitesadatok = new string[Futar1Kezbesitesszam + Futar2Kezbesitesszam];  //létrehozok egy új tömböt amibe mentem a soronkénti adatokat 

            int i = 0;

            while (!sr.EndOfStream)      //tovább olvasom a fájlt egészen a végéig
            {
                Kezbesitesadatok[i] += sr.ReadLine();  
                i++;
            }
            sr.Close();  //bezárom a fájlt

            //átadom az egész kézbesítés tömböt és azt hogy mettől meddig vannak benne az adott futár adatai
            Futar1 = new Futár(Kezbesitesadatok, 0, Futar1Kezbesitesszam, Futar1Kezbesitesszam);
            Futar2 = new Futár(Kezbesitesadatok, Futar1Kezbesitesszam, Futar2Kezbesitesszam + Futar1Kezbesitesszam, Futar2Kezbesitesszam);

        }     

        public void Munka()
        {
            int i = 0;
            int j = 0;

            //addig megyek amíg az egyik futár tömb végére nem érek
            while ((i < Futar1.Indulasok.Length) && (j < Futar2.Indulasok.Length))   
            {

                //addig megy a ciklus míg nem talál két olyan indulást amikor a két futár találkozhat
                while ((i < (Futar1.Indulasok.Length)) && (j < (Futar2.Indulasok.Length)) && ((Futar1.Indulasok[i] >= Futar2.Kiszallit(j,SzuksIdo)) || (Futar2.Indulasok[j] >= Futar1.Kiszallit(i,SzuksIdo))))
                {

                    //ha a 2. futár indulása a kiszállítással megnövelt ideővel is kisebb vagy egyenlő az 
                    //1. futár indulásánál akkor 2.futár egy másik indulására ugrok
                    if ((i < Futar1.Indulasok.Length) && (j < Futar2.Indulasok.Length) && (Futar1.Indulasok[i] >= Futar2.Kiszallit(j,SzuksIdo)))
                    {
                        j++;
                        
                    }

                    //ha az 1. futár indulása a kiszállítással megnövelt ideővel is kisebb vagy egyenlő a 
                    //2. futár indulásánál akkor egy másik indulásra ugrok
                    if ((i < Futar1.Indulasok.Length) && (j < Futar2.Indulasok.Length) && (Futar2.Indulasok[j] >= Futar1.Kiszallit(i,SzuksIdo)))
                    {
                        i++;
                        
                    }
                    
                }

                //ha találok két potenciális indulást akkor megvizsgálom hogy egyezik e az időponthoz tartozó telephely
                if ((i < Futar1.Indulasok.Length) && (j < Futar2.Indulasok.Length) && (Futar1.Telephelyek[i] != Futar2.Telephelyek[j]))
                {
                    //ha nem: ekkor biztosan találkoztak útközben
         
                    Tombbementes(i+1, j+1);  //kimentem a két változót eggyel megnövelve 
                    Talalkozasdb++;

                }

                i++;  //növelem az indexelő változókat hiszen a két időpont többször már nem szerepelhet
                j++;

            }

            Fajlbakiiras(Eredmenytomb, Talalkozasdb);  //miután a végére értem valamely tömbnek elkezdem a kíírást

        }

        private void Tombbementes(int kezbesitesdb1, int kezbesitesdb2)
        {
            string[] temp = Eredmenytomb;                       //ha új találkozásokat találok egy eggyel nagyobb tömbre lesz szükségem
            Eredmenytomb = new string[temp.Length + 1];

            for (int i = 0; i < temp.Length; i++)
            {
                Eredmenytomb[i] = temp[i];   //a már tárolt elemeket átmásolom
            }

            //az új találkozás adatait tömb utolsó helyére mentem
            Eredmenytomb[Eredmenytomb.Length - 1] = kezbesitesdb1.ToString() + " " + kezbesitesdb2.ToString();


        }

        private void Fajlbakiiras(string[] eredmeny, int db)
        {
            StreamWriter sw = new StreamWriter("talalkozasok.txt", false, Encoding.Default); //létrehozok egy egy új szövegfájlt 

            sw.WriteLine(db);  //az első sorba beleírom hogy összesen hányszor találkoztak a futárok

            if (eredmeny.Length != 0)  //csak akkor folytattom az írást a fájlba ha van érték ez eredmeny tömbben
            {

                for (int i = 0; i < eredmeny.Length; i++)
                {
                    sw.WriteLine(eredmeny[i]);  //végig megyek az eredmeny tömbön és mindegyik soronként bemásolom a fájlba
                }
            }

            sw.Close();  //lezárom a fájlt

        }

    }
}
