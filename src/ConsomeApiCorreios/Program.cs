using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using static System.Console;
using System.Web.Script.Serialization;


namespace ConsomeApiCorreios
{
    class Program
    {
        public class CEP
        {
            public string Cep { get; set; }
            public string Logradouro { get; set; }
            public string Bairro { get; set; }
            public string Localidade { get; set; }
            public string Uf { get; set; }
        }

        static void Main()
        {
            Run().GetAwaiter().GetResult();
        }

        static async Task Run()
        {
            WriteLine("Digite 1 para buscar por CEP e 2 para buscar por endereço");
            int tipoBusca = int.Parse(ReadLine());
            if (tipoBusca == 1)
            {
                WriteLine("Digite seu CEP");
                var cep = ReadLine();
                WriteLine();

                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri("https://viacep.com.br");

                    HttpResponseMessage response = await client.GetAsync("/ws/" + cep + "/json");
                    var RetornoCEP = response.Content.ReadAsStringAsync().Result;

                    var listCep = new JavaScriptSerializer().Deserialize<CEP>(RetornoCEP);

                    WriteLine("CEP: " + listCep.Cep);
                    WriteLine("Logradouro: " + listCep.Logradouro);
                    WriteLine("Bairro: " + listCep.Bairro);
                    WriteLine("Uf: " + listCep.Uf);
                    WriteLine("Cidade: " + listCep.Localidade);

                    ReadKey();
                }
            }
            else
            {
                WriteLine("Digite a UF");
                string uf = ReadLine();
                WriteLine("Digite a Cidade");
                string cidade = ReadLine();
                WriteLine("Digite o Logradouro");
                string logradouro = ReadLine();
                WriteLine();

                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri("https://viacep.com.br");

                    HttpResponseMessage response = await client.GetAsync("/ws/" + uf + "/" + cidade + "/" + logradouro + "/json");
                    var retornoCEP = response.Content.ReadAsStringAsync().Result;

                    List<CEP> listCep = new JavaScriptSerializer().Deserialize<List<CEP>>(retornoCEP);
                    
                    //List<CEP> lstFiltro = listCep.Where(a => a.Bairro.Contains("Mafalda")).ToList();

                    foreach (var item in listCep)
                    {
                        WriteLine("CEP: " + item.Cep);
                        WriteLine("Logradouro: " + item.Logradouro);
                        WriteLine("Bairro: " + item.Bairro);
                        WriteLine("Uf: " + item.Uf);
                        WriteLine("Cidade: " + item.Localidade);
                        WriteLine();
                    }
                    ReadLine();
                }
            }
        }
    }
}
