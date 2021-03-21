using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Drawing;
using System.Collections.Generic;
using static Armazenamento.Arqs;
using Armazenamento;
using Biblioteca;
namespace Test
{
   
    [TestClass]
    public class TestBin
    {
        static Porta AuxPorta = new Porta(2);
        static Botao AuxBotao = new Botao(AuxPorta);
        static Mapa Map = new Mapa(21, 21);
        
        [TestMethod] public void TestSalvarBin1() => SalvarBin(new Cord(1, 3), "BinTestMethod2");
        [TestMethod] public void TestSalvarBin2() => SalvarBin(new Morte(), "BinTestMethod2");
        [TestMethod] public void TestSalvarBin3() => SalvarBin((B: AuxBotao, A: AuxPorta), "BinTestMethod3");
        [TestMethod] public void TestSalvarBin4() {
            
          
        }
        //[TestMethod]
        //public void TestCarregarBin0()
        //{
        //    int[,] TestArr = CarregarBin<int[,]>("TestMethod0");
        //    for (int i = 0; i < TestArr.GetLength(0); i++)
        //    {
        //        for (int j = 0; j < TestArr.GetLength(1); j++)
        //            Console.Write($"{TestArr[i, j]}, ");
        //        Console.WriteLine();
        //    }

        //}
        //[TestMethod]
        //public void TestCarregarBin1()
        //{
        //    var Map = CarregarBin<Mapa>("TestMethod1");
        //    Assert.AreEqual(((Porta)Map[1, 1]).Aberta, false);

        //    ((Botao)Map[2, 2]).Interagir();
        //    Assert.AreEqual(((Porta)Map[1, 1]).Aberta, true);

        //}
        [TestMethod] public void TestCarregarBin1()
        {
            var Aux = CarregarBin<( Botao b, Porta p)>("BinTestMethod3");
            Assert.AreEqual(false, Aux.p.Aberta,"Botao antes");
            Assert.AreEqual(false, Aux.b.Prescionado, "Botao antes");
            Aux.b.Interagir();

            Assert.AreEqual( true, Aux.b.Prescionado,"Botao depois");
            Assert.AreEqual( true, Aux.p.Aberta,"Porta depois");

            Assert.AreNotEqual(AuxBotao, Aux.b);
            Assert.AreNotEqual(AuxPorta, Aux.p);
        }
        [TestMethod] public void TestCarregarBin4()
        {
             var Aux = CarregarBin<Mapa>("BinTestMethod4");
            Assert.AreEqual(Aux, Map);
        }



    }

    [TestClass]

    public class TestsGerais
    {
        [TestMethod]
        public void TestProcessDirectory()
        {
            var NomeArquivos = GetArquivos(@"C:\Users\Victor\Documents\Dev\Projetos_atuais\Joguim - Copia\Test\Saves");
            NomeArquivos.Sort((a, b) => { return Extensao(a).CompareTo(Extensao(b)); });
            NomeArquivos.ForEach((a) => Console.WriteLine(a));
        }
    }

}


