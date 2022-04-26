using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TGC.MonoGame.TP.Sources;

namespace TGC.MonoGame.TP
{
    /// <summary>
    ///     Esta es la clase principal  del juego.
    ///     Inicialmente puede ser renombrado o copiado para hacer más ejemplos chicos, en el caso de copiar para que se
    ///     ejecute el nuevo ejemplo deben cambiar la clase que ejecuta Program <see cref="Program.Main()" /> linea 10.
    /// </summary>
    public class TGCGame : Game
    {
        private GraphicsDeviceManager Graphics { get; }
        private SpriteBatch SpriteBatch { get; set; }

        private FollowCamera FollowCamera;
        private Map Map;
        private Vehicle Vehicle { get; set; }

        public TGCGame()
        {
            Graphics = new GraphicsDeviceManager(this);
            Graphics.IsFullScreen = true;
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            var rasterizerState = new RasterizerState();
            rasterizerState.CullMode = CullMode.CullCounterClockwiseFace;
            GraphicsDevice.RasterizerState = rasterizerState;
            GraphicsDevice.BlendState = BlendState.Opaque;

            Graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width - 100;
            Graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height - 100;
            Graphics.ApplyChanges();

            FollowCamera = new FollowCamera(GraphicsDevice.Viewport.AspectRatio);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            //SpriteBatch = new SpriteBatch(GraphicsDevice);

            Map = new Map(Content, GraphicsDevice);
            Vehicle = new Vehicle(Content);

            base.LoadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            FollowCamera.Update(gameTime, Vehicle.World);
            Vehicle.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            Map.Draw(gameTime, FollowCamera.View, FollowCamera.Projection);
            Vehicle.Draw(gameTime, FollowCamera.View, FollowCamera.Projection);

            base.Draw(gameTime);
        }

        protected override void UnloadContent()
        {
            Content.Unload();
            base.UnloadContent();
        }
    }
}