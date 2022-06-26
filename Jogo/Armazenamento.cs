using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
using Engine;
using System; 
using static Engine.Helper;
using Microsoft.Xna.Framework;

namespace Jogo
{
    
    
    public static class MapasPrefeitos
    {

        static void AdicionarParedes(Mapa mapa, double grossura)
        {
            var Lados = mapa.PosicaoLados;
            mapa.AdicionarEntidades( new[]{
                new Parede(new Vetor(Lados.Esquerda + grossura / 2, 0), grossura, mapa.Tamanho.Altura ),
                new Parede(new Vetor(Lados.Direita  - grossura / 2, 0), grossura, mapa.Tamanho.Altura ),
                new Parede(new Vetor(0 ,Lados.Cima  + grossura / 2   ), mapa.Tamanho.Largura, grossura),
                new Parede(new Vetor(0 ,Lados.Baixo - grossura / 2   ), mapa.Tamanho.Largura, grossura),
            });
        }
        public static Mapa GerarMapaDeTeste1()
        {
            var Map = new Mapa(21, 21);
            Map.PixelPorUnidade = 20;
            /*Pintor.Retangulo<Morte>(new Cord(4, 4), new Cord(Map.Tamanho.x - 5, Map.Tamanho.y - 5));*/
            AdicionarParedes(Map, 1);
            
            List<Porta> Portas = new List<Porta>(){
                new Porta(new Vetor(04-10, 10-10),3,null,null,1,1),
                new Porta(new Vetor(10-10, 04-10),3,null,null,1,1),
                new Porta(new Vetor(16-10, 10-10),3,null,null,1,1),
                new Porta(new Vetor(10-10, 16-10),3,null,null,1,1),
            };
            Map.AdicionarEntidades(Portas);
            Map.AdicionarEntidades(new IEntidade[]{
                new Parede((16-10, 0), 1, 20),
                new Parede(( 4-10, 0), 1, 20),
                new Parede(( 0, 4-10),20, 1),
                new Parede(( 0,16-10), 20, 1),

                new BateVolta((05-10, 05-10), Vetor.Baixo  /10,1,1,estilo: Color.Blue   ),
                new BateVolta((15-10, 05-10), Vetor.Direita/10,1,1,estilo: Color.DarkOrange) ,
                new BateVolta((05-10, 15-10), Vetor.Direita/10,1,1,estilo: Color.Yellow ),
                new BateVolta((15-10, 15-10), Vetor.Direita/10,1,1,estilo: Color.Yellow ),

                new Quadradinho((03-10, 10-10), true , Vetor.Baixo  ,1,1,estilo: Color.Violet ),
                new Quadradinho((17-10, 10-10), false, Vetor.Direita,1,1,estilo: Color.Violet ),
                new Quadradinho((10-10, 03-10), true , Vetor.Direita,1,1,estilo: Color.Violet ),
                new Quadradinho((10-10, 17-10), false, Vetor.Baixo  ,1,1,estilo: Color.Violet ),
 

                new Teletransporte(( 0, 1),(  0, -8),null,1,1),
                new Teletransporte(( 0,-1),(  0,  8),null,1,1),
                new Teletransporte(( 1, 0),( -8,  0),null,1,1),
                new Teletransporte((-1, 0),(  8,  0),null,1,1),
                
                new Botao((02-10, 05-10), Portas[0],1,1),
                new Botao((02-10, 10-10), Portas[0],1,1),
                new Botao((02-10, 15-10), Portas[0],1,1),
                
                new Botao((05-10, 02-10), Portas[1],1,1),
                new Botao((10-10, 02-10), Portas[1],1,1),
                new Botao((15-10, 02-10), Portas[1],1,1),
                
                new Botao((18-10, 05-10), Portas[2],1,1),
                new Botao((18-10, 10-10), Portas[2],1,1),
                new Botao((18-10, 15-10), Portas[2],1,1),
                
                new Botao((05-10, 18-10), Portas[3],1,1),
                new Botao((10-10, 18-10), Portas[3],1,1),
                new Botao((15-10, 18-10), Portas[3],1,1),
                new Jogador((0,0),0.5,1,1)
            }); 
            return Map;
        }
        public static Mapa GerarMapaDeTeste2()
        {
            var Map = new Mapa(10* (int)TP, 10* (int)TP);
            Map.AdicionarEntidades(new List<Entidade>
            {
                //new BateVolta((05*TP, 05*TP), Vetor.Baixo   ),
                //new BateVolta((15*TP, 05*TP), Vetor.Direita ) ,
                //new BateVolta((05*TP, 15*TP), Vetor.Direita ),
                //new BateVolta((05*TP, 05*TP), Vetor.Baixo   ),
                //new BateVolta((15*TP, 05*TP), Vetor.Direita ) ,
                //new BateVolta((05*TP, 15*TP), Vetor.Direita ),
            });
            return Map;
        }
        public static Mapa GerarMapaParticulas()
        {
            var Tamanho = 40*TP;
            var Map = new Mapa(Tamanho+TP, Tamanho+TP);
            AdicionarParedes(Map,TP);
            Map.PixelPorUnidade = 0.5;

            var N = 10 ;
            var Gap = Tamanho / N;
            for (double rx = 0; rx < Tamanho; rx+= Gap){
                Estilo Estilo = Estilo.Aleatorio();
                for (double ry = 0 ; ry < Tamanho; ry += Gap) {
                    var posicao =  new Vetor(rx, ry) + new Vetor(1,1) * ( -Tamanho/2 + Gap/2);
                    Map.AdicionarEntidade(new Particula(posicao, Map.RemoverEntidade ,double.PositiveInfinity,velocidade: 2, estilo: Estilo));
                }
            }
            return Map;
        } 

        public static Mapa GerarMapaDemonioDeLaplace()
        {
            var Tamanho = 40*TP;
            var Map = new Mapa(Tamanho+TP, Tamanho+TP);
            Map.PixelPorUnidade = 0.5;
            Map.AdicionarEntidade(new Parede((0, 0), TP, Tamanho));
            Map.AdicionarEntidade(new Parede((0, 0), Tamanho, TP));

            var N = 16 ;
            var Gap = Tamanho / N;
                
            for (double rx = 0; rx < Tamanho; rx+= Gap)
            {
                Estilo Estilo = Estilo.Aleatorio();
                for (double ry = 0 ; ry < Tamanho; ry += Gap)
                {
                    var posicao =  new Vetor(rx, ry) + new Vetor(1,1) * ( -Tamanho/2 + Gap/2);
                    Map.AdicionarEntidade(new Particula(posicao, 
                        Map.RemoverEntidade,
                        double.PositiveInfinity, 
                        velocidade: (ry+rx)*(ry+rx)/500000 , 
                        estilo: Estilo
                    ));
                }
            }

            return Map;
        } 

        public static  Mapa GerarMapaTestFIsica()
        {
            var Map = new Mapa(1000, 500);
            Map.PixelPorUnidade = 1;
            AdicionarParedes(Map, TP);
            var entidades = new Entidade[]{
                new BateVolta(new Vetor(-200, 0),(Vetor.Baixo+Vetor.Esquerda)*3),

                new BateVolta((-40,70), Vetor.Direita*2),
                new BateVolta(( 40,70), Vetor.Esquerda*2),
                new BateVolta(( 70,-40), Vetor.Cima *2),
                new BateVolta(( 70,40), Vetor.Baixo*2),
                
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
}