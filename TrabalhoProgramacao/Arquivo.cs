using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;

namespace TrabalhoProgramacao
{
    static class Arquivo
    {
        public static void Salvar(List<Pessoa> lista, string diretorio)
        {
            StreamWriter file = new StreamWriter(diretorio, false);
            string output = JsonConvert.SerializeObject(lista);
            file.Write(output);
            file.Close();
        }

        public static void Ler(ref List<Pessoa> lista, string diretorio)
        {
            string json = File.ReadAllText(diretorio);
            lista = JsonConvert.DeserializeObject<List<Pessoa>>(json);
        }

    }
}
