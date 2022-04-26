using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TGC.MonoGame.TP.Geometries;

namespace TGC.MonoGame.TP.Sources.Primitives
{
	class Prism : GeometricPrimitive
    {
        public Prism(GraphicsDevice graphicsDevice) : this(graphicsDevice, 1, 1, 1, Color.White, Color.White,
            Color.White, Color.White, Color.White, Color.White)
        {
        }

        public Prism(GraphicsDevice graphicsDevice, float sizeA, float sizeB, float sizeC, Color color) : this(graphicsDevice, sizeA, sizeB, sizeC, color,
            color, color, color, color, color)
        {
        }

        /// <summary>
        ///     Constructs a new cube primitive, with the specified size.
        /// </summary>
        public Prism(GraphicsDevice graphicsDevice, float sizeA, float sizeB, float sizeC, Color color1, Color color2, Color color3,
            Color color4, Color color5, Color color6)
        {
            // A cube has six faces, each one pointing in a different direction.
            Vector3[] normals =
            {
                // front normal
                Vector3.UnitZ,
                // back normal
                -Vector3.UnitZ,
                // right normal
                Vector3.UnitX,
                // left normal
                -Vector3.UnitX,
                // top normal
                Vector3.UnitY,
                // bottom normal
                -Vector3.UnitY
            };

            Color[] colors =
            {
                color1, color2, color3, color4, color5, color6
            };

            var i = 0;
            // Create each face in turn.
            Vector3[] sizes = {
                new Vector3(sizeC, sizeA, sizeB),
                new Vector3(sizeC, sizeA, sizeB),

                new Vector3(sizeB, sizeC, sizeA),
                new Vector3(sizeB, sizeC, sizeA),
                
                new Vector3(sizeA, sizeB, sizeC),
                new Vector3(sizeA, sizeB, sizeC)
            };
            foreach (var normal in normals)
            {
                // Get two vectors perpendicular to the face normal and to each other.
                var side1 = new Vector3(normal.Y, normal.Z, normal.X);
                var side2 = Vector3.Cross(normal, side1);

                var size1 = sizes[i].X / 2f;//sizes[(i / 2) % 3] / 2f;
                var size2 = sizes[i].Y / 2f;//sizes[(i / 2 + 1) % 3] / 2f;
                var size3 = sizes[i].Z / 2f;//sizes[(i / 2 + 2) % 3] / 2f;

                // Six indices (two triangles) per face.
                AddIndex(CurrentVertex + 0);
                AddIndex(CurrentVertex + 1);
                AddIndex(CurrentVertex + 2);

                AddIndex(CurrentVertex + 0);
                AddIndex(CurrentVertex + 2);
                AddIndex(CurrentVertex + 3);

                // Four vertices per face.
                AddVertex((normal * size3 - side1 * size1 - side2 * size2), colors[i], normal);
                AddVertex((normal * size3 - side1 * size1 + side2 * size2), colors[i], normal);
                AddVertex((normal * size3 + side1 * size1 + side2 * size2), colors[i], normal);
                AddVertex((normal * size3 + side1 * size1 - side2 * size2), colors[i], normal);

                i++;
            }

            InitializePrimitive(graphicsDevice);
        }
    }
}
