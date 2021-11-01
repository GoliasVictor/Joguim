using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
using Newtonsoft.Json;
using System.Drawing;
using Newtonsoft.Json.Linq;
using Biblioteca;
using System;
using OpenTK;

namespace Armazenamento
{
    
    public enum TipBloco { Chao, Parede, Morte, Ponto, Teletransporte, Porta, Botao }
    public static class MapasPrefeitos
    {
        public static Mapa GerarMapaDeTeste1()
        {
            var Map = new Mapa(21, 21);
            Map.Spawn = new Cord(Map.Tamanho.x / 2, Map.Tamanho.y / 2);
            Pintor.Map = Map;
            
            Pintor.Linha(( 4, 0), ( 4, 20), new Parede());
            Pintor.Linha((16, 0), (16, 20), new Parede());
            Pintor.Linha(( 0, 4), (20,  4), new Parede());
            Pintor.Linha(( 0,16), (20, 16), new Parede());
            Pintor.Retangulo(new Cord(4, 4), new Cord(Map.Tamanho.x - 5, Map.Tamanho.y - 5), new Morte());
            Map.Especiais = new List<IMovel>()
            {
                new BateVolta(  05, 05, Map, true , new Vector2(0, 1), Color.Blue   ),
                new BateVolta(  15, 05, Map, false, new Vector2(1, 0), Color.DarkOrange) ,
                new BateVolta(  05, 15, Map, true , new Vector2(1, 0), Color.Yellow ),
                new BateVolta(  15, 15, Map, false, new Vector2(0, 1), Color.Violet ),
                new Quadradinho(03, 10, Map, true , new Vector2(0, 1), Color.Violet ),
                new Quadradinho(17, 10, Map, false, new Vector2(1, 0), Color.Violet ),
                new Quadradinho(10, 03, Map, true , new Vector2(1, 0), Color.Violet ),
                new Quadradinho(10, 17, Map, false, new Vector2(0, 1), Color.Violet ),
                new Empurravel( 05, 10, Map),
                new Empurravel( 15, 10, Map),
                new Empurravel( 10, 05, Map),
                new Empurravel( 10, 15, Map),
            };

            Map[10, 11] = new Teletransporte(10, 19);
            Map[10, 09] = new Teletransporte(10, 01);
            Map[11, 10] = new Teletransporte(19, 10);
            Map[09, 10] = new Teletransporte(01, 10);

            Map[04, 10] = new Porta(3);
            Map[10, 04] = new Porta(3);
            Map[16, 10] = new Porta(3);
            Map[10, 16] = new Porta(3);

            Map[02, 05] = new Botao((Porta)Map[04, 10]);
            Map[02, 10] = new Botao((Porta)Map[04, 10]);
            Map[02, 15] = new Botao((Porta)Map[04, 10]);

            Map[05, 02] = new Botao((Porta)Map[10, 04]);
            Map[10, 02] = new Botao((Porta)Map[10, 04]);
            Map[15, 02] = new Botao((Porta)Map[10, 04]);

            Map[18, 05] = new Botao((Porta)Map[16, 10]);
            Map[18, 10] = new Botao((Porta)Map[16, 10]);
            Map[18, 15] = new Botao((Porta)Map[16, 10]);

            Map[05, 18] = new Botao((Porta)Map[10, 16]);
            Map[10, 18] = new Botao((Porta)Map[10, 16]);
            Map[15, 18] = new Botao((Porta)Map[10, 16]);
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
