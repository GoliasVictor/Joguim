﻿
using System;
using System.Diagnostics;
using System.Collections;
using System.Collections.ObjectModel;
using System.Linq.Expressions;
using static Engine.Helper;


namespace Engine
{


    public static class Helper
    {
        private static double tempo = 0;
        public static double TempoAnterior { get; private set; }
        public static double DeltaTempo => Tempo - TempoAnterior;
        public static Vetor ParedeVelocidade { get; set; }
        public static double Tempo
        {
            get => tempo;
            set
            {
                TempoAnterior = tempo;

                tempo = value;
            }
        }
        public static void TranferirForca(this IMovel A, IMovel B, Vetor ForcaTransferida)
        {
            A.AplicarForca(-ForcaTransferida);
            B.AplicarForca(ForcaTransferida);
        }
        public const double TamanhoPadrao = 20;

        public static Random Rnd = new Random();


    }

}