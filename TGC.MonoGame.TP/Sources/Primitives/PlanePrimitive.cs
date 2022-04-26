using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace TGC.MonoGame.TP.Sources.Primitives
{
	class PlanePrimitive
	{
		private Vector3 FaceOrientation;
		private Vector3 UpDirection;
		private Vector3 Origin;

		public BasicEffect Effect;
		private VertexBuffer VertexBuffer;
		private IndexBuffer IndexBuffer;

		public PlanePrimitive(GraphicsDevice device, Texture2D texture, Vector3 origin, Vector3 faceOrientation, Vector3 upDirection, float width, float height, float textureRepeates)
		{
			FaceOrientation = faceOrientation;
			UpDirection = upDirection;
			Origin = origin;

			CreateEffect(device, texture);

			CreateVertexBuffer(device, width, height, textureRepeates);
			CreateIndexBuffer(device);
		}

		private void CreateEffect(GraphicsDevice device, Texture2D texture)
		{
			Effect = new BasicEffect(device);
			Effect.TextureEnabled = true;
			Effect.Texture = texture;
			Effect.EnableDefaultLighting();
		}

		private void CreateVertexBuffer(GraphicsDevice device, float width, float height, float textureRepeats)
		{
			var centerLeftDirection = Vector3.Normalize(Vector3.Cross(FaceOrientation, UpDirection)) * width / 2f;

			Vector3 TopLeft = Origin + centerLeftDirection + UpDirection * height / 2f;
			Vector3 TopRight = Origin - centerLeftDirection + UpDirection * height / 2f;
			Vector3 BottomLeft = Origin + centerLeftDirection - UpDirection * height / 2f;
			Vector3 BottomRight = Origin - centerLeftDirection - UpDirection * height / 2f;

			var textureTopLeft = Vector2.Zero;
            var textureTopRight = Vector2.UnitX;
            var textureBottomLeft = Vector2.UnitY;
            var textureBottomRight = Vector2.One;

            var vertices = new[]
            {
                new VertexPositionNormalTexture(BottomLeft, FaceOrientation, textureBottomLeft * textureRepeats),
                new VertexPositionNormalTexture(TopLeft, FaceOrientation, textureTopLeft * textureRepeats),
                new VertexPositionNormalTexture(BottomRight, FaceOrientation, textureBottomRight * textureRepeats),
                new VertexPositionNormalTexture(TopRight, FaceOrientation, textureTopRight * textureRepeats)
            };

            VertexBuffer = new VertexBuffer(device, VertexPositionNormalTexture.VertexDeclaration, vertices.Length, BufferUsage.WriteOnly);
            VertexBuffer.SetData(vertices);
		}

		private void CreateIndexBuffer(GraphicsDevice device)
		{
			var indices = new ushort[]
			{
				0, 1, 2,
				2, 1, 3
			};
			IndexBuffer = new IndexBuffer(device, IndexElementSize.SixteenBits, indices.Length, BufferUsage.None);
			IndexBuffer.SetData(indices);
		}

		private void Draw(Effect effect) 
		{
			var graphicsDevice = effect.GraphicsDevice;

			graphicsDevice.SetVertexBuffer(VertexBuffer);
			graphicsDevice.Indices = IndexBuffer;

			foreach (var effectPass in effect.CurrentTechnique.Passes)
			{
				effectPass.Apply();
				graphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, IndexBuffer.IndexCount / 3);
			}
		}

		public void Draw(Matrix world, Matrix view, Matrix projection)
		{
			Effect.World = world;
			Effect.View = view;
			Effect.Projection = projection;

			Draw(Effect);
		}
	}
}
