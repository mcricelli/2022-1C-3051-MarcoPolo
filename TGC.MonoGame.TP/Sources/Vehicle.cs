using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace TGC.MonoGame.TP.Sources
{
	class Vehicle
	{
        // TODO: Mover esto a la clase Arena (tal vez).
        // Variables de la física del mundo.
        float gravity = -2000f;
        Vector2 floorFriction = new Vector2(-0.5f, -50f);
        // Fin del TODO

        // Atributos del auto.
        private const float carAcceleration = 5000f;
        private const float carJumpAcceleration = 50000f;
        private const float rotationVelocity = 2f;

        public Model Model;
		public Matrix World;

        // Estado del auto.
        private Vector3 currentVelocity;
        private Vector3 currentAceleration;
        private float vehicleScale = 0.5f;

		public Vehicle(ContentManager content)
		{
			Model = content.Load<Model>(Resources.ContentModels + "VehicleA/VehicleA");
			World = Matrix.CreateScale(vehicleScale) * Matrix.CreateRotationY(MathF.PI) * Matrix.Identity;
		}

		public void Draw(GameTime gameTime, Matrix view, Matrix projection)
		{
            Model.Draw(World, view, projection);
        }

		public void Update(GameTime gameTime)
		{
			var keyboardState = Keyboard.GetState();

			float deltaTime = Convert.ToSingle(gameTime.ElapsedGameTime.TotalSeconds);
			float deltaRot = CalculateDeltaRotation(deltaTime, GetMoveDirection(keyboardState));
			Vector3 deltaMov = CalculateDeltaMovement(deltaTime, GetMoveDirection(keyboardState));

            World = Matrix.CreateRotationY(deltaRot) * World;
			World = World * Matrix.CreateTranslation(deltaMov);
		}

        private Vector3 GetMoveDirection(KeyboardState keyboardState)
        {
            Vector3 moveDirection = Vector3.Zero;
            if (keyboardState.IsKeyDown(Keys.W) || keyboardState.IsKeyDown(Keys.Up))
                moveDirection = Vector3.Backward;
            else if (keyboardState.IsKeyDown(Keys.S) || keyboardState.IsKeyDown(Keys.Down))
                moveDirection = Vector3.Forward;

            if (keyboardState.IsKeyDown(Keys.A) || keyboardState.IsKeyDown(Keys.Left))
                moveDirection += Vector3.Left;
            else if (keyboardState.IsKeyDown(Keys.D) || keyboardState.IsKeyDown(Keys.Right))
                moveDirection += Vector3.Right;

            if (keyboardState.IsKeyDown(Keys.Space))
                moveDirection += Vector3.Up;

            return moveDirection;
        }

        private float CalculateDeltaRotation(float deltaTime, Vector3 movementDirection)
        {
            // Para conseguir un movimiento más controlable y realista (ponele) la rotacion del auto la calculo con una velocidad constante.
            if (currentVelocity.Z != 0 || currentVelocity.X != 0)
                return MathF.Sign(-movementDirection.X) * rotationVelocity * deltaTime;

            return 0f;
        }

        private Vector3 CalculateDeltaMovement(float deltaTime, Vector3 movementDirection)
        {
            currentAceleration = CalculateAcceleration(movementDirection);

            Vector3 deltaMove = (currentVelocity * deltaTime + currentAceleration * deltaTime * deltaTime);

            currentVelocity += currentAceleration * deltaTime;

            // Me aseguro de no llevar el auto por debajo de la altura del piso.
            if (World.Translation.Y + deltaMove.Y < 0f)
                deltaMove.Y = -World.Translation.Y;

            return deltaMove;
        }

        private Vector3 CalculateAcceleration(Vector3 movementDirection)
        {
            Vector3 acceleration = Vector3.Zero;

            if (World.Translation.Y > 0) // El auto está en el aire, aplico gravedad.
            {
                acceleration += gravity * Vector3.Up;
            }
            else // El auto está en el piso.
            {
                if (movementDirection.Y > 0) // El jugador está intentando saltar, tiene prioridad por sobre las direcciones.
                {
                    acceleration += carJumpAcceleration * Vector3.Up;
                }
                else // Acelero hacia donde se quiere mover el jugador.
                {
                    acceleration += World.Forward * carAcceleration * MathF.Sign(-movementDirection.Z);
                }
            }

            // Vector de velocidad hacia adelante/atras pero relativa al auto.
            Vector3 currentVelocityHorizontal = new Vector3(currentVelocity.X, 0f, currentVelocity.Z);
            Vector3 frontalVelocity = Vector3.Normalize(World.Forward) * Vector3.Dot(currentVelocityHorizontal, World.Forward) / World.Forward.Length();

            // Vector de velocidad hacia los laterales pero relativa al auto.
            Vector3 lateralVelocity = currentVelocityHorizontal - frontalVelocity;

            // Agrego un factor de derrape para cuando el auto va muy rápido.
            float driftFactor = frontalVelocity.LengthSquared() > 0 ? 75f / frontalVelocity.Length() : 1f;

            // Evito el movimiento lateral del auto (pero teniendo en cuenta que puede derrapar).
            acceleration += lateralVelocity * floorFriction.Y * driftFactor;

            // Para frenar el auto si se deja de acelerar aplico una fuerza de fricción en la dirección en la que avanza el mismo.
            acceleration += frontalVelocity * floorFriction.X;

            return acceleration;
        }
    }
}
