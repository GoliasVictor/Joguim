using Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jogo
{
    class Camera
    {
        Vector3 Centro;
        Viewport viewport;

        public Camera(Viewport viewport, Vector3 centro = default, float zoom = 1)
        {
            this.viewport = viewport;
            Zoom = zoom;
            Atualizar(centro);
        }
 

        float x
        {
            get => Centro.X;
            set => Centro.X = value;
        }
        float y
        {
            get => Centro.Y;
            set => Centro.Y = value;
        }
        public Matrix Transform { 
            get => Matrix.CreateTranslation(viewport.Width/ (2 * Zoom), viewport.Height / (2 * Zoom), 0) * 
                   Matrix.CreateScale(Zoom, Zoom, 0) *
                   Matrix.CreateTranslation(Centro);
         }
        public float Zoom { get;set;}

        public void Atualizar(Vector3 NovoCentro)
        {
            Centro = NovoCentro; 
        }
    }
}
