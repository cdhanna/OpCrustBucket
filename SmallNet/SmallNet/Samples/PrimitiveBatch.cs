#region File Description
//-----------------------------------------------------------------------------
// PrimitiveBatch.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
#endregion

namespace SmallNet.Samples
{

    // PrimitiveBatch is a class that handles efficient rendering automatically for its
    // users, in a similar way to SpriteBatch. PrimitiveBatch can render lines, points,
    // and triangles to the screen. In this sample, it is used to draw a spacewars
    // retro scene.
    public class PrimitiveBatch : IDisposable
    {
        #region Constants and Fields

        const int DefaultBufferSize = 15;
        Matrix translationMatrix;

        public VertexPositionColorTexture[] vertices = new VertexPositionColorTexture[DefaultBufferSize];
        

        public int positionInBuffer = 0;

        BasicEffect basicEffect;
        GraphicsDevice device;
        public PrimitiveType primitiveType;
        public int numVertsPerPrimitive;


        bool hasBegun = false;

        bool isDisposed = false;

        #endregion


        public PrimitiveBatch(GraphicsDevice graphicsDevice)
        {
            if (graphicsDevice == null)
            {
                throw new ArgumentNullException("graphicsDevice");
            }
            device = graphicsDevice;
            
            // set up a new basic effect, and enable vertex colors.
            basicEffect = new BasicEffect(graphicsDevice);
            basicEffect.VertexColorEnabled = true;
            translationMatrix = new Matrix();
            
            // projection uses CreateOrthographicOffCenter to create 2d projection
            // matrix with 0,0 in the upper left.

            basicEffect.Projection = Matrix.CreateOrthographicOffCenter
                (0, graphicsDevice.Viewport.Width,
                graphicsDevice.Viewport.Height, 0,
                0, 1);
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing && !isDisposed)
            {
                if (basicEffect != null)
                    basicEffect.Dispose();

                isDisposed = true;
            }
        }

        public void setCamera(Matrix cameraTeselation)
        {
            translationMatrix = cameraTeselation;
        }

        public void Begin(PrimitiveType primitiveType, Texture2D texture)
        {
            if (hasBegun)
            {
                throw new InvalidOperationException
                    ("End must be called before Begin can be called again.");
            }


            this.primitiveType = primitiveType;

            this.numVertsPerPrimitive = NumVertsPerPrimitive(primitiveType);

            basicEffect.CurrentTechnique.Passes[0].Apply();
            basicEffect.TextureEnabled = true;
            basicEffect.Texture = texture;
            //positionInBuffer = 0;

            hasBegun = true;
        }

        public void Begin(PrimitiveType primitiveType)
        {
            if (hasBegun)
            {
                throw new InvalidOperationException
                    ("End must be called before Begin can be called again.");
            }


            this.primitiveType = primitiveType;

            this.numVertsPerPrimitive = NumVertsPerPrimitive(primitiveType);

            basicEffect.CurrentTechnique.Passes[0].Apply();

            hasBegun = true;
        }

        public void addUVCord(int vIndex, Vector2 UVcord)
        {
            vertices[vIndex].TextureCoordinate = UVcord;
        }
        public void AddVertex(Vector2 vertex, Color color)
        {
            if (!hasBegun)
            {
                throw new InvalidOperationException
                    ("Begin must be called before AddVertex can be called.");
            }
            if (primitiveType.Equals(PrimitiveType.LineStrip))
            {
                numVertsPerPrimitive++;
            }
            else if (primitiveType.Equals(PrimitiveType.TriangleStrip))
            {
                
                if (numVertsPerPrimitive > 2)
                {
                    numVertsPerPrimitive++;
                    vertices[positionInBuffer] = vertices[0];
                    positionInBuffer++;
                    numVertsPerPrimitive++;
                    vertices[positionInBuffer] = vertices[positionInBuffer -2];
                    positionInBuffer++;
                }
                numVertsPerPrimitive++;
            }
            // are we starting a new primitive? if so, and there will not be enough room
            // for a whole primitive, flush.
            bool newPrimitive = ((positionInBuffer % numVertsPerPrimitive) == 0);

            if (newPrimitive &&
                (positionInBuffer + numVertsPerPrimitive) >= vertices.Length)
            {
                Flush();
            }

            // once we know there's enough room, set the vertex in the buffer,
            // and increase position.
            vertices[positionInBuffer].Position = new Vector3(vertex, 0);
            vertices[positionInBuffer].Color = color;
            
            positionInBuffer++;
        }

        // End is called once all the primitives have been drawn using AddVertex.
        // it will call Flush to actually submit the draw call to the graphics card, and
        // then tell the basic effect to end.
        public void End()
        {
            if (!hasBegun)
            {
                throw new InvalidOperationException
                    ("Begin must be called before End can be called.");
            }

            // Draw whatever the user wanted us to draw
            Flush();

            hasBegun = false;
        }

        // Flush is called to issue the draw call to the graphics card. Once the draw
        // call is made, positionInBuffer is reset, so that AddVertex can start over
        // at the beginning. End will call this to draw the primitives that the user
        // requested, and AddVertex will call this if there is not enough room in the
        // buffer.
        public void Flush()
        {
            if (!hasBegun)
            {
                throw new InvalidOperationException
                    ("Begin must be called before Flush can be called.");
            }

            // no work to do
            if (positionInBuffer == 0)
            {
                return;
            }


            for (int i = 0 ; i < vertices.Length ; i++)
            {
                vertices[i].Position = Vector3.Transform(vertices[i].Position, translationMatrix);
                //float x = vertices[i].Position.X;
                //vertices[i].Position.X = vertices[i].Position.Y;
                //vertices[i].Position.Y = x;
                //Vector2 v = Vector2.Transform(new Vector2(vertices[i].Position.X,vertices[i].Position.Y), translationMatrix);
                //vertices[i].Position = new Vector3(v.X, v.Y, 0);
            }
            // how many primitives will we draw?
            

            // submit the draw call to the graphics card
            //device.DrawUserPrimitives<VertexPositionColor>(primitiveType, vertices, 0,primitiveCount);

            int primitiveCount = positionInBuffer / numVertsPerPrimitive;
            if (primitiveType.Equals(PrimitiveType.LineList))
            {

                device.DrawUserPrimitives<VertexPositionColorTexture>(primitiveType, vertices, 0, primitiveCount);
            }
            else if (primitiveType.Equals(PrimitiveType.LineStrip))
            {
                vertices[positionInBuffer] = vertices[0];
                primitiveCount = numVertsPerPrimitive-2;
                device.DrawUserPrimitives<VertexPositionColorTexture>(primitiveType, vertices, 0, primitiveCount);
            }
            else if (primitiveType.Equals(PrimitiveType.TriangleStrip))
            {
                numVertsPerPrimitive++;
                vertices[positionInBuffer] = vertices[0];
                positionInBuffer++;
                numVertsPerPrimitive++;
                vertices[positionInBuffer] = vertices[positionInBuffer - 2];
                positionInBuffer++;
                numVertsPerPrimitive++;
                vertices[positionInBuffer] = vertices[0];

                primitiveCount = numVertsPerPrimitive - 2;

               
                device.DrawUserPrimitives<VertexPositionColorTexture>(primitiveType, vertices, 0, primitiveCount);
            }
            // now that we've drawn, it's ok to reset positionInBuffer back to zero,
            // and write over any vertices that may have been set previously.
            positionInBuffer = 0;
        }

        #region Helper functions

        // NumVertsPerPrimitive is a boring helper function that tells how many vertices
        // it will take to draw each kind of primitive.
        static private int NumVertsPerPrimitive(PrimitiveType primitive)
        {
            int numVertsPerPrimitive = 0;
            switch (primitive)
            {
                case PrimitiveType.LineList:
                    numVertsPerPrimitive = 2;
                    break;
                case PrimitiveType.TriangleList:
                    numVertsPerPrimitive = 3;
                    break;
                case PrimitiveType.LineStrip:
                    numVertsPerPrimitive = 2;
                    break;
                case PrimitiveType.TriangleStrip:
                    numVertsPerPrimitive = 0;
                    break;
                default:
                    break;
            }
            return numVertsPerPrimitive;
        }

        #endregion


    }
}
