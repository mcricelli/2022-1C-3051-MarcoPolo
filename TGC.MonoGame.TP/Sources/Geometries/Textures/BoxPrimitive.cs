using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TGC.MonoGame.Tp.Geometries.Textures
{
    /// <summary>
    ///     Textured 3D box or cube.
    /// </summary>
    public class BoxPrimitive
    {
        /// <summary>
        ///     Create a box with a center at the given point, with a size and a color in each vertex.
        /// </summary>
        /// <param name="graphicsDevice">Used to initialize and control the presentation of the graphics device.</param>
        /// <param name="size">Size of the box.</param>
        /// <param name="texture">The box texture.</param>
        public BoxPrimitive(GraphicsDevice graphicsDevice, Vector3 size, Texture2D texture)
        {
            Effect = new BasicEffect(graphicsDevice);
            Effect.TextureEnabled = true;
            Effect.Texture = texture;
            Effect.EnableDefaultLighting();

            CreateVertexBuffer(graphicsDevice, size);
            CreateIndexBuffer(graphicsDevice);
        }

        /// <summary>
        ///     Represents a list of 3D vertices to be streamed to the graphics device.
        /// </summary>
        private VertexBuffer Vertices { get; set; }

        /// <summary>
        ///     Describes the rendering order of the vertices in a vertex buffer.
        /// </summary>
        private IndexBuffer Indices { get; set; }

        /// <summary>
        ///     Built-in effect that supports optional texturing, vertex coloring, fog, and lighting.
        /// </summary>
        private BasicEffect Effect { get; }

        /// <summary>
        ///     Create a vertex buffer for the figure with the given information.
        /// </summary>
        /// <param name="graphicsDevice">The graphics device.</param>
        /// <param name="size">Size of the box.</param>
        private void CreateVertexBuffer(GraphicsDevice graphicsDevice, Vector3 size)
        {
            var x = size.X / 2;
            var y = size.Y / 2;
            var z = size.Z / 2;

            var vectors = new[]
            {
                // Top-Left Front.
                new Vector3(-x, y, z),
                // Top-Right Front.
                new Vector3(x, y, z),
                // Top-Left Back.
                new Vector3(-x, y, -z),
                // Top-Right Back.
                new Vector3(x, y, -z),
                // Bottom-Left Front.
                new Vector3(-x, -y, z),
                // Bottom-Right Front.
                new Vector3(x, -y, z),
                // Bottom-Left Back.
                new Vector3(-x, -y, -z),
                // Bottom-Right Back.
                new Vector3(x, -y, -z)
            };

            // A box has six faces, each one pointing in a different direction.
            var normals = new[]
            {
                // Top.
                Vector3.UnitY,
                // Bottom.
                -Vector3.UnitY,
                // Front.
                Vector3.UnitZ,
                // Back.
                -Vector3.UnitZ,
                // Left.
                -Vector3.UnitX,
                // Right.
                Vector3.UnitX
            };

            var textureCoordinates = new[]
            {
                // Top-Left.
                Vector2.Zero,
                // Top-Right.
                Vector2.UnitX,
                // Bottom-Left.
                Vector2.UnitY,
                // Bottom-Right.
                Vector2.One
            };

            var vertices = new[]
            {
                // Top Face.
                new VertexPositionNormalTexture(vectors[0], normals[0], textureCoordinates[2]),
                new VertexPositionNormalTexture(vectors[1], normals[0], textureCoordinates[3]),
                new VertexPositionNormalTexture(vectors[2], normals[0], textureCoordinates[0]),
                new VertexPositionNormalTexture(vectors[3], normals[0], textureCoordinates[1]),
                // Bottom Face.
                new VertexPositionNormalTexture(vectors[4], normals[1], textureCoordinates[2]),
                new VertexPositionNormalTexture(vectors[5], normals[1], textureCoordinates[3]),
                new VertexPositionNormalTexture(vectors[6], normals[1], textureCoordinates[0]),
                new VertexPositionNormalTexture(vectors[7], normals[1], textureCoordinates[1]),
                // Left Face.
                new VertexPositionNormalTexture(vectors[2], normals[4], textureCoordinates[0]),
                new VertexPositionNormalTexture(vectors[0], normals[4], textureCoordinates[1]),
                new VertexPositionNormalTexture(vectors[6], normals[4], textureCoordinates[2]),
                new VertexPositionNormalTexture(vectors[4], normals[4], textureCoordinates[3]),
                // Right Face.
                new VertexPositionNormalTexture(vectors[3], normals[5], textureCoordinates[0]),
                new VertexPositionNormalTexture(vectors[1], normals[5], textureCoordinates[1]),
                new VertexPositionNormalTexture(vectors[7], normals[5], textureCoordinates[2]),
                new VertexPositionNormalTexture(vectors[5], normals[5], textureCoordinates[3]),
                // Back Face.
                new VertexPositionNormalTexture(vectors[0], normals[3], textureCoordinates[0]),
                new VertexPositionNormalTexture(vectors[1], normals[3], textureCoordinates[1]),
                new VertexPositionNormalTexture(vectors[4], normals[3], textureCoordinates[2]),
                new VertexPositionNormalTexture(vectors[5], normals[3], textureCoordinates[3]),
                // Front Face.
                new VertexPositionNormalTexture(vectors[2], normals[2], textureCoordinates[0]),
                new VertexPositionNormalTexture(vectors[3], normals[2], textureCoordinates[1]),
                new VertexPositionNormalTexture(vectors[6], normals[2], textureCoordinates[2]),
                new VertexPositionNormalTexture(vectors[7], normals[2], textureCoordinates[3])
            };

            Vertices = new VertexBuffer(graphicsDevice, VertexPositionNormalTexture.VertexDeclaration, vertices.Length,
                BufferUsage.WriteOnly);
            Vertices.SetData(vertices);
        }

        /// <summary>
        ///     Create an index buffer for the vertex buffer that the figure has.
        /// </summary>
        /// <param name="graphicsDevice">The graphics device.</param>
        private void CreateIndexBuffer(GraphicsDevice graphicsDevice)
        {
            var indices = new ushort[]
            {
                // Top.
                2, 1, 0, 2, 3, 1,
                // Back.
                18, 16, 17, 18, 17, 19,
                // Left.
                10, 8, 9, 10, 9, 11,
                // Front.
                22, 21, 20, 22, 23, 21,
                // Right.
                14, 13, 12, 14, 15, 13,
                // Bottom.
                6, 4, 5, 6, 5, 7
            };

            Indices = new IndexBuffer(graphicsDevice, IndexElementSize.SixteenBits, indices.Length,
                BufferUsage.WriteOnly);
            Indices.SetData(indices);
        }

        /// <summary>
        ///     Draw the box.
        /// </summary>
        /// <param name="world">The world matrix for this box.</param>
        /// <param name="view">The view matrix, normally from the camera.</param>
        /// <param name="projection">The projection matrix, normally from the application.</param>
        public void Draw(Matrix world, Matrix view, Matrix projection)
        {
            // Set BasicEffect parameters.
            Effect.World = world;
            Effect.View = view;
            Effect.Projection = projection;

            // Draw the model, using BasicEffect.
            Draw(Effect);
        }

        /// <summary>
        ///     Draws the primitive model, using the specified effect. Unlike the other Draw overload where you just specify the
        ///     world/view/projection matrices and color, this method does not set any render states, so you must make sure all
        ///     states are set to sensible values before you call it.
        /// </summary>
        /// <param name="effect">Used to set and query effects, and to choose techniques.</param>
        public void Draw(Effect effect)
        {
            var graphicsDevice = effect.GraphicsDevice;

            // Set our vertex declaration, vertex buffer, and index buffer.
            graphicsDevice.SetVertexBuffer(Vertices);
            graphicsDevice.Indices = Indices;

            foreach (var effectPass in effect.CurrentTechnique.Passes)
            {
                effectPass.Apply();
                graphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, Indices.IndexCount / 3);
            }
        }
    }
}