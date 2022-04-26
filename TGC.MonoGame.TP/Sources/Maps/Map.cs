using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using TGC.MonoGame.Tp.Geometries.Textures;
using TGC.MonoGame.TP.Geometries;
using TGC.MonoGame.TP.Sources.Maps.Map_Elements;
using TGC.MonoGame.TP.Sources.Primitives;

namespace TGC.MonoGame.TP.Sources
{
	class Map
	{
        private GraphicsDevice GraphicsDevice;
        private ContentManager ContentManager;
		
		// Ground
        private PlanePrimitive Ground { get; set; }
		private Matrix GroundWorld { get; set; }
		private const float groundLength = 8000;
		private const float groundRepeatTexture = 1000;

		// Walls
		private KeyValuePair<PlanePrimitive, Matrix>[] Walls = new KeyValuePair<PlanePrimitive, Matrix>[4];
		private const float wallHeight = 1000f;

		// Map elements
		// TODO: Handle only one list to abstract the map elements
		//private List<KeyValuePair<MapElement, Matrix>> LetterCubes = new List<KeyValuePair<MapElement, Matrix>>();

		// Letter cubes
		private List<KeyValuePair<LetterCube, Matrix>> LetterCubes = new List<KeyValuePair<LetterCube,Matrix>>();

		// Tennis balls
		private List<KeyValuePair<SpherePrimitive, Matrix>> TennisBalls = new List<KeyValuePair<SpherePrimitive, Matrix>>();
		float tennisBallSize = 200f;

		// Cylinders
		private List<KeyValuePair<CylinderPrimitive, Matrix>> Cylinders = new List<KeyValuePair<CylinderPrimitive, Matrix>>();

		// Triangular prisms
		private List<KeyValuePair<TriangularPrism, Matrix>> TriangularPrisms = new List<KeyValuePair<TriangularPrism, Matrix>>();

		// Prisms
		private List<KeyValuePair<Prism, Matrix>> Prisms = new List<KeyValuePair<Prism, Matrix>>();

		public Map(ContentManager contentManager, GraphicsDevice graphicsDevice)
		{
			GraphicsDevice = graphicsDevice;
			ContentManager = contentManager;

			CreateEmptyMap();
			CreateMapElements();
		}
		
		private void CreateEmptyMap()
		{
			var groundTexture = ContentManager.Load<Texture2D>(Resources.ContentFolderTextures + "wood-floor");
			Ground = new PlanePrimitive(GraphicsDevice, groundTexture, Vector3.Zero, Vector3.Up, Vector3.Forward, groundLength, groundLength, groundLength / groundRepeatTexture);
			GroundWorld = Matrix.Identity;

			var wallTexture = ContentManager.Load<Texture2D>(Resources.ContentFolderTextures + "wood-wall");
			Vector3[] center = { Vector3.Left , Vector3.Right, Vector3.Forward, Vector3.Backward };
			for(int i = 0; i < Walls.Length; i++)
			{
				var origin = center[i] * groundLength / 4f + Vector3.Up * wallHeight / 4f;
				var faceDirection = GroundWorld.Translation - origin;
				var wall = new PlanePrimitive(GraphicsDevice, wallTexture, origin, faceDirection, Vector3.Up, groundLength, wallHeight, 1);
				Walls[i] = new KeyValuePair<PlanePrimitive, Matrix>(wall, Matrix.CreateTranslation(origin));
			}
		}

		private void CreateMapElements()
		{
			CreateLetterCubes();
			CreateTennisBalls();
			CreateCilynders();
			CreateTriangularPrisms();
			CreatePrisms();
		}

		private void CreatePrisms()
		{
			float widthX = 50f;
			float widthZ = 100f;
			float height = 400f;

			float dx = widthZ * 2;
			float dz = widthZ * 2;
			int countI = 5;
			int countJ = 7;

			Vector3 mapOffset = new Vector3(groundLength * 0.1f,0,groundLength * 0.3f);
			for (int i = 0; i < countI; i++)
			{
				for(int j = 0; j < countJ; j++)
				{
					if (i != 0 && i != countI - 1 && j != 0 && j != countJ - 1)
						continue;

					if (i == 0 && (j == countJ / 2 || j == countJ + 1 / 2))
						continue;

					bool sideWall = i == 0 || i == countI-1;
					Vector3 pos = mapOffset + Vector3.Right * i * dx + Vector3.Forward * j * dz;
					if (!sideWall)
					{
						if(j == 0)
							pos -= Vector3.Forward * dz / 4f;
						else
							pos += Vector3.Forward * dz / 4f;
					}

					Prisms.Add(new KeyValuePair<Prism, Matrix>(
						new Prism(GraphicsDevice, sideWall ? widthX : widthZ, sideWall ? widthZ : widthX, height, Color.Green),
						Matrix.CreateTranslation(pos + Vector3.Up * height / 2f)
					));
				}
			}

			/*
			var texture = ContentManager.Load<Texture2D>(Resources.ContentFolderTextures + "WoodFloor");
			prism.Effect.Texture = texture;
			prism.Effect.TextureEnabled = true;
			prism.Effect.VertexColorEnabled = false;
			*/
		}

		private float Lerp(double val, float min, float max)
		{
			float cleanVal = MathF.Min(1f, MathF.Max(0f, (float)val));

			return min + (max - min) * cleanVal;
		}

		private void CreateTriangularPrisms()
		{
			Random random = new Random(5);
			float minHeight = 100f;
			float maxHeight = 200f;

			float minWidth = 250f;
			float maxWidth = 350f;

			float minDeep = 80f;
			float maxDeep = 200f;

			List<Vector3> originPoints = new List<Vector3>();
			for ( int i = 0; i < 6; i++)
			{
				float maxDist = 2000f;
				float x = Lerp(random.NextDouble(), -maxDist, maxDist);
				float z = Lerp(random.NextDouble(), -maxDist, maxDist);
				originPoints.Add(new Vector3(x, 0, z));
			}

			foreach (var position in originPoints)
			{
				float width = Lerp(random.NextDouble(), minWidth, maxWidth);
				float height = Lerp(random.NextDouble(), minHeight, maxHeight);
				float deep = Lerp(random.NextDouble(), minDeep, maxDeep);

				Vector3 vLeft = Vector3.Left * width / 2f;
				Vector3 vRight = Vector3.Right * width / 2f;
				float posX = Lerp(random.NextDouble(), -width/2f, width / 2f);
				Vector3 vTop = Vector3.Right * posX + Vector3.Up * height;

				var rotation = random.Next();

				TriangularPrisms.Add(new KeyValuePair<TriangularPrism, Matrix>(
					new TriangularPrism(GraphicsDevice, vLeft, vTop, vRight, Color.Red, deep),
					Matrix.CreateRotationY(rotation) * Matrix.CreateTranslation(position)
				));
			}

			/*
			var texture = ContentManager.Load<Texture2D>(Resources.ContentFolderTextures + "TennisBall");
			triangularPrism.Effect.TextureEnabled = true;
			triangularPrism.Effect.VertexColorEnabled = false;
			triangularPrism.Effect.Texture = texture;
			*/
		}

		private void CreateCilynders()
		{
			Cylinders.Add(new KeyValuePair<CylinderPrimitive, Matrix>(
				new CylinderPrimitive(GraphicsDevice, Color.Blue, tennisBallSize * 2f, 100f),
				Matrix.CreateTranslation(Vector3.Left * 500f + Vector3.Up * tennisBallSize)
			));
			
			/*
			var texture = ContentManager.Load<Texture2D>(Resources.ContentFolderTextwwadures + "TennisBall");
			cylinder.Effect.TextureEnabled = true;
			cylinder.Effect.VertexColorEnabled = false;
			cylinder.Effect.Texture = texture;
			*/
		}

		private void CreateTennisBalls()
		{
			TennisBalls.Add(new KeyValuePair<SpherePrimitive, Matrix>(
				new SpherePrimitive(GraphicsDevice, tennisBallSize, 16, Color.Orange),
				Matrix.CreateTranslation(Vector3.Backward * 500f + Vector3.Up * tennisBallSize / 2f)
			));
			
			/*
			var texture = ContentManager.Load<Texture2D>(Resources.ContentFolderTextures + "TennisBall");
			tennisBall.Effect.TextureEnabled = true;
			tennisBall.Effect.VertexColorEnabled = false;
			tennisBall.Effect.Texture = texture;
			*/
		}

		private void CreateLetterCubes()
		{
			Random random = new Random(5);
			List<Matrix> LetterCubeMatrices = new List<Matrix>();
			const float distanceBetweenWalls = groundLength * 0.35f;

			List<Vector3> originPoints = new List<Vector3>()
			{
				Vector3.Zero,
				Vector3.Right * distanceBetweenWalls,
				Vector3.Left * distanceBetweenWalls,
				Vector3.Forward * distanceBetweenWalls,
				Vector3.Backward * distanceBetweenWalls,
				(Vector3.Forward + Vector3.Right) * distanceBetweenWalls,
				(Vector3.Forward + Vector3.Left) * distanceBetweenWalls,
				(Vector3.Backward + Vector3.Right) * distanceBetweenWalls,
				(Vector3.Backward + Vector3.Left) * distanceBetweenWalls
			};

			int minCantH = 5;
			int maxCantH = 10;

			int minCantV = 2;
			int maxCantV = 6;

			float separation = MathF.Sqrt(2) * LetterCube.CubeSize;
			float distToCenter = LetterCube.CubeSize / 2f;

			foreach (var origin in originPoints)
			{
				int maxDistToOrigin = 200;
				var distanceToOrigin = Vector3.Normalize(new Vector3(random.Next(-100, 100), 0f, random.Next(-100, 100))) * random.Next(0, maxDistToOrigin);
				var wallDirection = Vector3.Normalize(new Vector3(random.Next(-100, 100), 0f, random.Next(-100, 100)));

				int cantH = random.Next(minCantH, maxCantH);
				int cantV = random.Next(minCantV, maxCantV);

				Vector3 wallStartPosition = origin + distanceToOrigin;

				var wallMatrixList = createRandomWallMatrixList(random, cantH, cantV, wallDirection, wallStartPosition, distToCenter, separation);
				LetterCubeMatrices.AddRange(wallMatrixList);
			}
				
			foreach(Matrix cubeMatrix in LetterCubeMatrices)
				LetterCubes.Add(new KeyValuePair<LetterCube, Matrix>(new LetterCube(GraphicsDevice, ContentManager), cubeMatrix));

		}
		private List<Matrix> createRandomWallMatrixList(Random random, int cantH, int cantV, Vector3 wallDirection, Vector3 wallStartPosition, float distToCenter, float separation)
		{
			List<Matrix> matrices = new List<Matrix>();

			int newHCant = cantH;
			int lastHCant = newHCant;

			float lastFloorOffset = 0;
			for (int i = 0; i < cantV; i++)
			{
				if (newHCant == 0)
					break;

				float brickWallOffset = i % 2 == 0 ? 0 : separation / 2f;
				if (newHCant == lastHCant)
					lastFloorOffset += brickWallOffset;
				else
					lastFloorOffset = i * separation / 2f;

				for (int j = 0; j < newHCant; j++)
				{
					float initialPosition = lastFloorOffset + j * separation;

					matrices.Add(
						Matrix.CreateRotationY(random.Next()) *
						Matrix.CreateTranslation(wallStartPosition + wallDirection * initialPosition + Vector3.Up * (distToCenter + i * 2f * distToCenter))
					);
				}

				lastHCant = newHCant;

				newHCant -= random.Next(0, newHCant / 2);
			}

			return matrices;
		}

		public void Draw(GameTime gameTime, Matrix view, Matrix projection)
        {
			Ground.Draw(GroundWorld, view, projection);

			for (int i = 0; i < Walls.Count(); i++)
				Walls[i].Key.Draw(Walls[i].Value, view, projection);

			for (int i = 0; i < LetterCubes.Count(); i++)
				LetterCubes[i].Key.Draw(LetterCubes[i].Value, view, projection);

			for (int i = 0; i < TennisBalls.Count(); i++)
				TennisBalls[i].Key.Draw(TennisBalls[i].Value, view, projection);

			for (int i = 0; i < Cylinders.Count(); i++)
				Cylinders[i].Key.Draw(Cylinders[i].Value, view, projection);

			for (int i = 0; i < TriangularPrisms.Count(); i++)
				TriangularPrisms[i].Key.Draw(TriangularPrisms[i].Value, view, projection);

			for (int i = 0; i < Prisms.Count(); i++)
				Prisms[i].Key.Draw(Prisms[i].Value, view, projection);
		}
    }
}
