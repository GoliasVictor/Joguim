using Microsoft.Xna.Framework.Input;
namespace Jogo
{

    public static class Teclado
    {

        public static KeyboardState EstadoTeclado;
        public static KeyboardState EstadoAnteriorTeclado;
        public static bool Apertando(Keys Key) => EstadoTeclado.IsKeyDown(Key);
        public static bool Apertou(Keys Key) => EstadoTeclado.IsKeyDown(Key) && !EstadoAnteriorTeclado.IsKeyDown(Key);
        public static void Atualizar()
        {
            EstadoAnteriorTeclado = EstadoTeclado;
            EstadoTeclado = Keyboard.GetState();
        } 

    }
    static class ControladorMouse
    {
        static MouseState EstadoAnterior;
        static MouseState EstadoAtual;
        public static int DeltaWheelValue =>  EstadoAtual.ScrollWheelValue - EstadoAnterior.ScrollWheelValue;
        public static void Atualizar()
        {
            EstadoAnterior = EstadoAtual;
            EstadoAtual = Mouse.GetState(); 
        }
    } 

}

