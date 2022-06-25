using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
using Newtonsoft.Json;
using System.Drawing;
using Newtonsoft.Json.Linq;
using Engine;
using System; 
using static Engine.Helper;

namespace Armazenamento
{
    
    
    public static class MapasPrefeitos
    {
        public static BateVolta Zezinho = new BateVolta((30, 30), Vetor.Baixo + Vetor.Esquerda  ,estilo: Color.Violet );
        public static Mapa GerarMapaDeTeste1()
        {
            var Map = new Mapa(21, 21);
            Map.Spawn = new Cord(Map.Tamanho.x / 2, Map.Tamanho.y / 2);
            /*Pintor.Retangulo<Morte>(new Cord(4, 4), new Cord(Map.Tamanho.x - 5, Map.Tamanho.y - 5));*/
            List<Porta> Portas = new List<Porta>(){
                new Porta(new Cord(04, 10),3),
                new Porta(new Cord(10, 04),3),
                new Porta(new Cord(16, 10),3),
                new Porta(new Cord(10, 16),3),
            };
            Map.AdicionarEntidades(Portas);
            Map.AdicionarEntidades(new IEntidade[]{
                new Parede(( 4, 0), 1, 20),
                new Parede((16, 0), 1, 20),
                new Parede(( 0, 4),20, 1),
                new Parede(( 0,16), 20, 1),

                new BateVolta((05, 05), Vetor.Baixo  ,estilo: Color.Blue   ),
                new BateVolta((15, 05), Vetor.Direita,estilo: Color.DarkOrange) ,
                new BateVolta((05, 15), Vetor.Direita,estilo: Color.Yellow ),
                Zezinho,

                new Quadradinho((03, 10), true , Vetor.Baixo  ,estilo: Color.Violet ),
                new Quadradinho((17, 10), false, Vetor.Direita,estilo: Color.Violet ),
                new Quadradinho((10, 03), true , Vetor.Direita,estilo: Color.Violet ),
                new Quadradinho((10, 17), false, Vetor.Baixo  ,estilo: Color.Violet ),
 

                new Teletransporte((10, 11),(10, 19)),
                new Teletransporte((10, 09),(10, 01)),
                new Teletransporte((11, 10),(19, 10)),
                new Teletransporte((09, 10),(01, 10)),
                
                new Botao((02, 05), Portas[0]),
                new Botao((02, 10), Portas[0]),
                new Botao((02, 15), Portas[0]),
                
                new Botao((05, 02), Portas[1]),
                new Botao((10, 02), Portas[1]),
                new Botao((15, 02), Portas[1]),
                
                new Botao((18, 05), Portas[2]),
                new Botao((18, 10), Portas[2]),
                new Botao((18, 15), Portas[2]),
                
                new Botao((05, 18), Portas[3]),
                new Botao((10, 18), Portas[3]),
                new Botao((15, 18), Portas[3]),
            }); 
            return Map;
        }
        public static Mapa GerarMapaDeTeste2()
        {
            var Map = new Mapa(10* (int)TamanhoPadrao, 10* (int)TamanhoPadrao);
            Map.Spawn = new Cord(Map.Tamanho.x / 2, Map.Tamanho.y / 2);
            Map.AdicionarEntidades(new List<Entidade>
            {
                //new BateVolta((05*TamanhoPadrao, 05*TamanhoPadrao), Vetor.Baixo   ),
                //new BateVolta((15*TamanhoPadrao, 05*TamanhoPadrao), Vetor.Direita ) ,
                //new BateVolta((05*TamanhoPadrao, 15*TamanhoPadrao), Vetor.Direita ),
                //new BateVolta((05*TamanhoPadrao, 05*TamanhoPadrao), Vetor.Baixo   ),
                //new BateVolta((15*TamanhoPadrao, 05*TamanhoPadrao), Vetor.Direita ) ,
                //new BateVolta((05*TamanhoPadrao, 15*TamanhoPadrao), Vetor.Direita ),
                Zezinho,
            });
            return Map;
        }
        public static Mapa GerarMapaParticulas()
        {
            
            var Map = new Mapa(21 *(int)TamanhoPadrao, 21 * (int)TamanhoPadrao);
            Map.Spawn = new Cord(0, 0);
            Map.AdicionarEntidade(new Parede((10, 10), 5, 200));
            Map.AdicionarEntidade(new Parede((10, 10), 200, 5));

            for (double x = -Map.Tamanho.x/2 + 20; x < Map.Tamanho.x/2 ; x+= 20)
            {
                Estilo Estilo = Estilo.Aleatorio();
                for (double y = -Map.Tamanho.y / 2 +20; y < Map.Tamanho.y / 2; y += 20)
                {
                    Map.AdicionarEntidade(new Particula((x,y), Map.MorteParticula, estilo: Estilo));
                }
            }

            return Map;
        } 

        public static  Mapa GerarMapaTestFIsica()
        {
            var Map = new Mapa(1000, 500);

            var entidades = new Entidade[]{
                new BateVolta(new Cord(-200, 0),(Vetor.Baixo+Vetor.Esquerda)*3),

                new BateVolta(new Cord(-40,70), Vetor.Direita*2),
                new BateVolta(new Cord( 40,70), Vetor.Esquerda*2),
                new BateVolta(new Cord( 70,-40), Vetor.Cima *2),
                new BateVolta(new Cord( 70,40), Vetor.Baixo*2),
                
                //new BateVolta(new Cord(-  0, 0)),
                //new BateVolta(new Cord(Aux-= 20, 0)),
                //new BateVolta(new Cord(Aux-= 20, 0)),
                //new BateVolta(new Cord(Aux-= 20, 0)),
                //new BateVolta(new Cord(Aux-= 20, 0)),
                //new BateVolta(new Cord(Aux-= 20.1*2, 0), Vetor.Direita), 
            };
            Map.AdicionarEntidades(entidades);
            return Map;
        }
    }
    public static class Arqs
    {
        public static string Path = @"C:\Users\Victor\Documents\Dev\Projetos_atuais\Joguim - Copia\Mapas\";
        public static void Salvar<T>(T Object, string Nome)
        {

            File.WriteAllText(Path + Nome + ".json", JsonConvert.SerializeObject(Object, Formatting.Indented));
        }
        public static void SalvarWml<T>(T objectToWrite, string Name, bool append = false) where T : new()
        {
            TextWriter writer = null;
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));

                writer = new StreamWriter(Path + Name + ".xml", append);
                serializer.Serialize(writer, objectToWrite);
            }
            finally
            {
                if (writer != null)
                    writer.Close();
            }
        }
        public static List<string> GetArquivos() => GetArquivos(Path);
        public static List<string> GetArquivos(string PathDirectory)
        {
            string[] PathFileNames = Directory.GetFiles(PathDirectory);

            List<string> FileNames = new List<string>();
            foreach (string FileName in PathFileNames) FileNames.Add(FileName.Remove(0, PathDirectory.Length));
            return FileNames;
        }
        public static string Extensao(string FileName) => FileName.Contains(".") ? FileName.Remove(0, FileName.LastIndexOf('.')) : "";

        public static T Carregar<T>(string Nome)
        {
            string path = Path + Nome + ".json";

            return JsonConvert.DeserializeObject<T>(File.ReadAllText(path));

        }
        [STAThread]
        public static void SalvarBin(object Dados, string Nome)
        {

            FileStream fs = new FileStream(Path + Nome + ".dat", FileMode.Create);

            BinaryFormatter formatter = new BinaryFormatter();
            try
            {
                formatter.Serialize(fs, Dados);
            }
            catch (SerializationException e)
            {
                Console.WriteLine("Failed to serialize. Reason: " + e.Message);
                throw;
            }
            finally
            {
                fs.Close();
            }
        }

        public static T CarregarBin<T>(string Nome)
        {
            FileStream fs = new FileStream(Path + Nome + ".dat", FileMode.Open);
            try
            {
                BinaryFormatter formatter = new BinaryFormatter();
                return (T)formatter.Deserialize(fs);
            }
            catch (SerializationException e)
            {
                Console.WriteLine("Failed to deserialize. Reason: " + e.Message);
                throw;
            }
            finally
            {
                fs.Close();
            }
        }
    }
}
