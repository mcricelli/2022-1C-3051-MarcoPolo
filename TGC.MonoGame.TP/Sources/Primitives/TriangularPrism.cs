using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TGC.MonoGame.TP.Geometries;

namespace TGC.MonoGame.TP.Sources.Primitives
{
    class TriangularPrism : GeometricPrimitive
    {
        /// <summary>
        ///     Create a triangle based on vertices and color.
        /// </summary>
        /// <param name="device">Used to initialize and control the presentation of the graphics device.</param>
        /// <param name="vertex1">Vertex of the triangle.</param>
        /// <param name="vertex2">Vertex of the triangle.</param>
        /// <param name="vertex3">Vertex of the triangle.</param>
        /// <param name="vertexColor">The color of the triangle.</param>
        public TriangularPrism(GraphicsDevice device, Vector3 vertex1, Vector3 vertex2, Vector3 vertex3,
            Color vertexColor, float width) : this(device, vertex1, vertex2, vertex3, vertexColor, vertexColor, vertexColor, width)
        {
        }

        /// <summary>
        ///     Create a triangle based on the vertices and a color for each one.
        /// </summary>
        /// <param name="graphicsDevice">Used to initialize and control the presentation of the graphics device.</param>
        /// <param name="vertex1">Vertex of the triangle.</param>
        /// <param name="vertex2">Vertex of the triangle.</param>
        /// <param name="vertex3">Vertex of the triangle.</param>
        /// <param name="vertexColor1">The color of the vertex.</param>
        /// <param name="vertexColor2">The color of the vertex.</param>
        /// <param name="vertexColor3">The color of the vertex.</param>
        public TriangularPrism(GraphicsDevice graphicsDevice, Vector3 vertex1, Vector3 vertex2, Vector3 vertex3,
            Color vertexColor1, Color vertexColor2, Color vertexColor3, float width)
        {
            // triangle 1
            AddIndex(CurrentVertex + 0);
            AddIndex(CurrentVertex + 1);
            AddIndex(CurrentVertex + 2);

            // triangle 2
            AddIndex(CurrentVertex + 3);
            AddIndex(CurrentVertex + 4);
            AddIndex(CurrentVertex + 5);

            // square 1
            AddIndex(CurrentVertex + 6);
            AddIndex(CurrentVertex + 7);
            AddIndex(CurrentVertex + 8);

            AddIndex(CurrentVertex + 9);
            AddIndex(CurrentVertex + 10);
            AddIndex(CurrentVertex + 11);

            // square 2
            AddIndex(CurrentVertex + 12);
            AddIndex(CurrentVertex + 13);
            AddIndex(CurrentVertex + 14);

            AddIndex(CurrentVertex + 15);
            AddIndex(CurrentVertex + 16);
            AddIndex(CurrentVertex + 17);

            // square 3
            AddIndex(CurrentVertex + 18);
            AddIndex(CurrentVertex + 19);
            AddIndex(CurrentVertex + 20);

            AddIndex(CurrentVertex + 21);
            AddIndex(CurrentVertex + 22);
            AddIndex(CurrentVertex + 23);

            Vector3 triangleNormal = Vector3.Normalize(Vector3.Cross(vertex2 - vertex1, vertex3 - vertex2));
            Vector3 squareNormal;

            var v1 = vertex1 - triangleNormal * width / 2f;
            var v2 = vertex2 - triangleNormal * width / 2f;
            var v3 = vertex3 - triangleNormal * width / 2f;
            var v4 = vertex1 + triangleNormal * width / 2f;
            var v5 = vertex2 + triangleNormal * width / 2f;
            var v6 = vertex3 + triangleNormal * width / 2f;
            
            // triangle 1
            AddVertex(v1, vertexColor1, triangleNormal);
            AddVertex(v2, vertexColor2, triangleNormal);
            AddVertex(v3, vertexColor3, triangleNormal);

            // triangle 2
            AddVertex(v4, vertexColor1, -triangleNormal);
            AddVertex(v6, vertexColor2, -triangleNormal);
            AddVertex(v5, vertexColor3, -triangleNormal);

            // square 1
            squareNormal = Vector3.Normalize(-Vector3.Cross(v3 - v1, v6 - v3));
            AddVertex(v1, vertexColor1, squareNormal);
            AddVertex(v3, vertexColor2, squareNormal);
            AddVertex(v4, vertexColor3, squareNormal);

            AddVertex(v4, vertexColor1, squareNormal);
            AddVertex(v3, vertexColor2, squareNormal);
            AddVertex(v6, vertexColor3, squareNormal);

            // square 2
            squareNormal = Vector3.Normalize(-Vector3.Cross(v5 - v4, v2 - v5));
            AddVertex(v4, vertexColor1, squareNormal);
            AddVertex(v5, vertexColor2, squareNormal);
            AddVertex(v1, vertexColor3, squareNormal);

            AddVertex(v1, vertexColor1, squareNormal);
            AddVertex(v5, vertexColor2, squareNormal);
            AddVertex(v2, vertexColor3, squareNormal);

            // square 3
            squareNormal = Vector3.Normalize(-Vector3.Cross(v2 - v3, v5 - v2));
            AddVertex(v3, vertexColor1, squareNormal);
            AddVertex(v2, vertexColor2, squareNormal);
            AddVertex(v6, vertexColor3, squareNormal);

            AddVertex(v6, vertexColor1, squareNormal);
            AddVertex(v2, vertexColor2, squareNormal);
            AddVertex(v5, vertexColor3, squareNormal);

            InitializePrimitive(graphicsDevice);
        }
    }
}
