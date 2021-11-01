using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK;
using OpenTK.Input;
namespace Biblioteca
{
    [Serializable]
    public class Chao : Bloco
    {
        public bool Tangivel => false;
        public EstiloBloco Estilo { get; set; } = EstiloBloco.Chao;

    }
    [Serializable]
    public class Parede : Bloco
    {
        public bool Tangivel => true;
        public EstiloBloco Estilo { get; set; } = EstiloBloco.parede;

    }
    [Serializable]
    public class Morte : IInteragivel
    {
        public EstiloBloco Estilo { get; set; } = EstiloBloco.Morte;
        public bool Tangivel => true;
        public void Interagir(IMovel movel)
        {
            if (movel is IJogador) ((IJogador)movel).Dano(1);
        }
    }
    [Serializable]
    public class Teletransporte : Bloco, IInteragivel
    {
        public Cord Saida;
        public bool Tangivel => true;
        public EstiloBloco Estilo { get; set; } = EstiloBloco.Teletransporte;
        public Teletransporte(Cord saida) => Saida = saida;
        public Teletransporte(int x, int y) => Saida = (x, y);
        public Teletransporte() { }

        public void Interagir(IMovel e)
        {
            if (e is IJogador) e.ProximaPos = Saida.GetCord();
        }
    }
    [Serializable]
    public class Porta : Bloco, IReceptor
    {
        public bool Tangivel => !Aberta;
        public EstiloBloco Estilo { get => Aberta ? EstiloAberta : EstiloFechada; set => _ = value; }

        private EstiloBloco EstiloFechada { get; set; } = new EstiloBloco(Color.SaddleBrown);
        private EstiloBloco EstiloAberta { get; set; } = new EstiloBloco(Color.Sienna);
        public bool Aberta => RequesitosCompletos >= ResquisitosNescessarios;
        private int ResquisitosNescessarios { get; set; }
        private int RequesitosCompletos { get; set; }
        public void Receber(object e)
        {
            RequesitosCompletos++;
        }
        public Porta(int reqNescesarios)
        {
            ResquisitosNescessarios = reqNescesarios;
        }
    }
    [Serializable]
    public class Botao : IInteragivel
    {
        public bool Tangivel => false;
        public EstiloBloco Estilo { get; set; } = EstiloBloco.Botao;

        public bool Prescionado = false;
        public IReceptor Receptor;
        public void Emitir()
        {
            Receptor.Receber(this);
        }

        public void Interagir(IMovel e = null)
        {
            if (Prescionado == false)
            {
                Emitir();
                Prescionado = true;
            }
        }
        public Botao(IReceptor receptor) => Receptor = receptor;
    }
    [Serializable]
    public class Quadradinho : IMovel
    {
        public bool Tangivel => true;
        public EstiloBloco Estilo { get; set; }
        public Cord Pos { get; set; }
        public Cord ProximaPos { get; set; }
        Mapa Map;
        public bool HorarioAntihoriario;
        public Cord Forca = (1, 0);

        public void Movimento()
        {

            ProximaPos = Pos.GetCord();
            ProximaPos.x += Forca.x;
            ProximaPos.y += Forca.y;

            if (Map[ProximaPos].Tangivel || Map.EspecialTangivel(ProximaPos))
            {
                ProximaPos = Pos.GetCord();
                int aux = HorarioAntihoriario ^ Forca.x == 0 ? 1 : -1;
                Forca = (Forca.y * aux, Forca.x * aux);
            }
        }
        public void AtualizarPos() => Pos = ProximaPos.GetCord();
        public Quadradinho(int x, int y, Mapa map, bool horarioAntihoriario, Vector2 forca, EstiloBloco estilo =null)
        {
            Pos = (x, y);
            ProximaPos = Pos.GetCord();
            Map = map;
            HorarioAntihoriario = horarioAntihoriario;
            Vector2 NormForca = forca.Normalized();
            Forca = new Cord((int)NormForca.X, (int)NormForca.Y);
            Estilo = estilo ?? new EstiloBloco(Color.Black);
        }
    }
    [Serializable]
    public class BateVolta : IMovel
    {
        public bool Tangivel => true;
        public EstiloBloco Estilo { get; set; } 
        public Cord Pos { get; set; }
        public Cord ProximaPos { get; set; }
        Mapa Map;
        public Vector2 Forca;

        public void Movimento()
        {
            ProximaPos = Pos.GetCord();
            ProximaPos.x += (int)Forca.X;
            ProximaPos.y += (int)Forca.Y;
            if (Map[ProximaPos].Tangivel || Map.EspecialTangivel(ProximaPos))
            {
                ProximaPos = Pos.GetCord();
                Forca *= -1;
            }
        }
        public void AtualizarPos() => Pos = ProximaPos.GetCord();
        public BateVolta(int x, int y, Mapa map, bool horizontalVertical, Vector2 forca, EstiloBloco estilo = null)
        {
            Pos = (x, y);
            ProximaPos = Pos.GetCord();
            Map = map;
            Forca =  forca.Normalized();
            Estilo = estilo ?? new EstiloBloco(Color.Black);
        }
    }
    public class Empurravel : IMovel, IInteragivel
    {
        public bool Tangivel => true;
        public EstiloBloco Estilo { get; set; } 
        public Cord Pos { get; set; }
        public Cord ProximaPos { get; set; }
        Mapa Map;
        public void Movimento()
        {

        }
        public void Interagir(IMovel e)
        {
            ProximaPos = Pos.GetCord();
            ProximaPos.x += e.Pos.x > Pos.x ? -1 : e.Pos.x == Pos.x ? 0 : +1;
            ProximaPos.y += e.Pos.y > Pos.y ? -1 : e.Pos.y == Pos.y ? 0 : +1;
            if (Map[ProximaPos].Tangivel || Map.EspecialTangivel(ProximaPos))
                ProximaPos = Pos.GetCord();
            else e.ProximaPos = Pos.GetCord();
            Pos = ProximaPos.GetCord();
        }
        public void AtualizarPos() {; }

        public Empurravel(int x, int y, Mapa map, EstiloBloco estilo = null)
        {
            Pos = (x, y);
            ProximaPos = Pos.GetCord();
            Map = map;
            Estilo = estilo ?? new EstiloBloco(Color.Black);
        }

    }

}
