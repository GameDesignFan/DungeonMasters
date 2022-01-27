using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GameProject0
{
    public class DungeonMasters : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Texture2D _background;
        private Texture2D _menuButton;
        private Texture2D _coinSheet;
        private SpriteFont _kaph;
        private float _coinTimer;
        private int _coinThresh;
        private Rectangle[] _coinRectangles;
        private byte _currAnim;

        public DungeonMasters()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            _background = Content.Load<Texture2D>("Blue");
            _menuButton = Content.Load<Texture2D>("b_5");
            _coinSheet = Content.Load<Texture2D>("coin_anim_strip_6");

            _coinTimer = 0;
            _coinThresh = 1000;

            _coinRectangles = new Rectangle[6];
            _coinRectangles[0] = new Rectangle(3, 0, 4, 8);
            _coinRectangles[1] = new Rectangle(10, 0, 6, 8);
            _coinRectangles[2] = new Rectangle(17, 0, 8, 8);
            _coinRectangles[3] = new Rectangle(24, 0, 8, 8);
            _coinRectangles[4] = new Rectangle(32, 0, 8, 8);
            _coinRectangles[5] = new Rectangle(41, 0, 6, 8);
            _currAnim = 1;

            _kaph = Content.Load<SpriteFont>("kaph");
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

             // TODO: Add your drawing code here

             _coinTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Draw the background
            _spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.LinearWrap, null, null);
            _spriteBatch.Draw(_background, new Vector2(0, 0), new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), Color.White, 0, Vector2.Zero, 1f, SpriteEffects.None, 0);
            _spriteBatch.End();

            _spriteBatch.Begin();

            // Draw the main menu text
            _spriteBatch.DrawString(_kaph, $"Dungeon Masters", new Vector2(95, 120), Color.CornflowerBlue);

            // Draw the play button
            _spriteBatch.Draw(_menuButton, new Vector2(340, 200), new Rectangle(0, 0, 602, 305), Color.White, 0, Vector2.Zero, 0.2f, SpriteEffects.None, 0);
            _spriteBatch.DrawString(_kaph, $"Play", new Vector2(360, 220), Color.Black, 0, Vector2.Zero, 0.5f, SpriteEffects.None, 0);

            // Draw the settings button
            _spriteBatch.Draw(_menuButton, new Vector2(340, 280), new Rectangle(0, 0, 602, 305), Color.White, 0, Vector2.Zero, 0.2f, SpriteEffects.None, 0);
            _spriteBatch.DrawString(_kaph, $"Settings", new Vector2(356, 300), Color.Black, 0, Vector2.Zero, 0.3f, SpriteEffects.None, 0);


            // Draw the quit button
            _spriteBatch.Draw(_menuButton, new Vector2(340, 360), new Rectangle(0, 0, 602, 305), Color.White, 0, Vector2.Zero, 0.2f, SpriteEffects.None, 0);
            _spriteBatch.DrawString(_kaph, $"Quit", new Vector2(360, 380), Color.Black, 0, Vector2.Zero, 0.5f, SpriteEffects.None, 0);

            // User instruction to exit alternatively
            _spriteBatch.DrawString(_kaph, $"Press ESC to exit", new Vector2(480, 440), Color.Black, 0, Vector2.Zero, 0.5f, SpriteEffects.None, 0);

            // Draw our animation
            if (_coinTimer > 0.2)
            {
                _currAnim++;
                if (_currAnim > 5) _currAnim = 0;
                _coinTimer -= (float)0.2;
            }
            _spriteBatch.Draw(_coinSheet, new Vector2(50, 130), _coinRectangles[_currAnim], Color.White, 0, Vector2.One, 4.0f, SpriteEffects.None, 0);
            _spriteBatch.Draw(_coinSheet, new Vector2(715, 130), _coinRectangles[_currAnim], Color.White, 0, Vector2.One, 4.0f, SpriteEffects.None, 0);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
