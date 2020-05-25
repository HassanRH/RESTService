using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RESTService
{
    public class Passager
    {
        public int Id;

        public string Navn;

        public double BagageVaegt;

        public string Efternavn;

        public int BagageAntal;

        public int FlyNummer;


        public override string ToString()
        {
            return $"Id: {Id}, Navn: {Navn}, BagageVaegt: {BagageVaegt}, Efternavn: {Efternavn}, BagageAntal: {BagageAntal}, FlyNummer: {FlyNummer}";
        }
    }
}