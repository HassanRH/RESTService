using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace RESTConsumer
{
    class Program
    {
        static void Main(string[] args)
        {
            foreach (var i in GetAllPassagerersAsync().Result) //man skal huske result for at få resultatet, (id,navn) så istedet for der bare står passasgere - det er vigtigt at have async med fordi det køre over nettet og det kan være langsomt 
            {
                Console.WriteLine(i);
            }


            Console.WriteLine("GEt ID");
            Console.WriteLine(GetPassagerByID(1).Result.ToString());
            Console.WriteLine("post");
            postPassager(new Passager{BagageAntal = 1,BagageVaegt = 1.2,Efternavn = "client",FlyNummer = 123,Navn = "restclint"});

            DeletePassager(10);

            PutPassager(new Passager {BagageVaegt = 1, Navn = "put", BagageAntal = 1, FlyNummer = 123, Efternavn = "hej"}, 1);


            foreach (var i in GetAllPassagerersAsync().Result)
            {
                Console.WriteLine(i);
            }


            Console.ReadLine();
        }
        //bruges ->foreach (var i in GetAllPassagerersAsync().Result)
        private static async Task<IList<Passager>> GetAllPassagerersAsync()
        {
            using (HttpClient client = new HttpClient())
            {
                string content = await client.GetStringAsync("http://restservice20180108125459.azurewebsites.net/service1.svc/passager"); //refference som den henter fra
                IList<Passager> cList = JsonConvert.DeserializeObject<IList<Passager>>(content);
                return cList;
            }
        }


        //bruges -> GetPassagerByID(1).Result.ToString()
        private static async Task<Passager> GetPassagerByID(int id)
        {
            using (HttpClient client = new HttpClient())
            {
                string content = await client.GetStringAsync($"http://restservice20180108125459.azurewebsites.net/service1.svc/Passager/{id}");
                Passager _Apartment = JsonConvert.DeserializeObject<Passager>(content); //convetere til json 
                return _Apartment;
            }
        }

        //Husk at nuget Json
        private static void postPassager(Passager Passager)
        {
            using (HttpClient client = new HttpClient())
            {

                string json = JsonConvert.SerializeObject(Passager);
                var content = new StringContent(json, Encoding.UTF8, "application/json"); //bliver omkodet til jason ved hjælp af UFT(man kan også bruge xml istedet for jason, vi har lære jason og det nemmere)
                var result = client.PostAsync("http://restservice20180108125459.azurewebsites.net/service1.svc/Passager/", content).Result;

            }
        }

        private static HttpResponseMessage DeletePassager(int id) //når man skal slette så skal man skrive id
        {
            using (HttpClient client = new HttpClient())
            {
                string idstring = id.ToString();
                client.BaseAddress = new Uri($"http://restservice20180108125459.azurewebsites.net/service1.svc/Passager/{id}"); //vi skrev i metoden vi kun skulle bruge id til at slette, derfor er det kun id der står der.
                client.DefaultRequestHeaders.Accept.Clear();
                HttpResponseMessage response = client.DeleteAsync(idstring).Result;
                return response;
            }
        }

        //husk at nuget api client
        private static HttpResponseMessage PutPassager(Passager emp, int id)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri($"http://restservice20180108125459.azurewebsites.net/service1.svc/Passager/{id}");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = client.PutAsJsonAsync($"{id}", emp).Result;
                return response;
            }
        }
    }
}
