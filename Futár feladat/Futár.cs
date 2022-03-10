using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Futár_feladat
{
    class Futár
    {
        public string[] Telephelyek { get; }
        public int[] Indulasok { get;}


        public Futár(string[] tomb ,int mettol ,int meddig,int hossz)
        {
            int mutato = 0;
            Telephelyek = new string[hossz];
            Indulasok = new int[hossz];

            for (int i = mettol; i < meddig; i++)
            {
                string[] adat = tomb[i].Split(' ');


                Telephelyek[mutato] = adat[0];
                Indulasok[mutato] = int.Parse(adat[1]);
                mutato++;
            }
            
        }

        public int Kiszallit(int i, int elteltido)
        {
            return Indulasok[i] + elteltido;
        }

    }
}
