
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using TGC.MonoGame.Tp.Geometries.Textures;

namespace TGC.MonoGame.TP.Sources.Maps.Map_Elements
{
	class LetterCube : BoxPrimitive
	{
		public static readonly float CubeSize = 75f;
		private const string textureA = Resources.ContentFolderTextures + "LetterCubeA";

		public LetterCube(GraphicsDevice graphicsDevice, ContentManager content) : 
			base(graphicsDevice, CubeSize * Vector3.One, content.Load<Texture2D>(textureA))
		{
		}
	}
}
