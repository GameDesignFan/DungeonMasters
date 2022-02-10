using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using GameProject0.Content;
using GameProject0.StateManagement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GameProject0.Screens
{
    // This screen implements the actual game logic. It is just a
    // placeholder to get the idea across: you'll probably want to
    // put some more interesting gameplay in here!
    public class GameplayScreen : GameScreen
    {
        private ContentManager _content;

        private Texture2D _playerTexture;
        private Texture2D _pathTexture;

        private SpriteFont _instructionFont;

        private Vector2 _playerPosition;

        private float _pauseAlpha;
        private readonly InputAction _pauseAction;

        private readonly Rectangle[] _playerFrames = new[]
        {
            new Rectangle(0, 0, 13, 21),
            new Rectangle(16, 0, 14, 21),
            new Rectangle(30, 0, 14, 20),
            new Rectangle(45, 0, 14, 21)
        };

        private int _frames = 0;
        private float _elapsed = 0;
        private float _delay = 200f;

        private BoundingRectangle _upperWallBound;
        private BoundingRectangle _lowerWallBound;
        private BoundingRectangle _playerBound;

        public BoundingRectangle UpperWallBound => _upperWallBound;
        public BoundingRectangle LowerWallBound => _lowerWallBound;
        public BoundingRectangle PlayerBound => _playerBound;

        private bool _colliding = false;


        public GameplayScreen()
        {
            TransitionOnTime = TimeSpan.FromSeconds(1.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);

            _pauseAction = new InputAction(
                new[] { Buttons.Start, Buttons.Back },
                new[] { Keys.Back, Keys.Escape }, true);
        }

        // Load graphics content for the game
        public override void Activate()
        {
            _content ??= new ContentManager(ScreenManager.Game.Services, "Content");

            _playerTexture = _content.Load<Texture2D>("King");
            _pathTexture = _content.Load<Texture2D>("Dungeon");
            _instructionFont = _content.Load<SpriteFont>("kaph");

            // A real game would probably have more content than this sample, so
            // it would take longer to load. We simulate that by delaying for a
            // while, giving you a chance to admire the beautiful loading screen.
            Thread.Sleep(1000);

            // once the load has finished, we use ResetElapsedTime to tell the game's
            // timing mechanism that we have just finished a very long frame, and that
            // it should not try to catch up.
            ScreenManager.Game.ResetElapsedTime();

            _playerPosition = new Vector2(0, ScreenManager.GraphicsDevice.Viewport.Height - 150);
            _upperWallBound = new BoundingRectangle(0, ScreenManager.GraphicsDevice.Viewport.Height - 256, ScreenManager.GraphicsDevice.Viewport.Width, 20);
            _lowerWallBound = new BoundingRectangle(0, ScreenManager.GraphicsDevice.Viewport.Height - 128, ScreenManager.GraphicsDevice.Viewport.Width, 30);
            _playerBound = new BoundingRectangle(20, 20, 32, 32);
        }


        public override void Deactivate()
        {
            base.Deactivate();
        }

        public override void Unload()
        {
            _content.Unload();
        }

        // This method checks the GameScreen.IsActive property, so the game will
        // stop updating when the pause menu is active, or if you tab away to a different application.
        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, false);

            // Gradually fade in or out depending on whether we are covered by the pause screen.
            _pauseAlpha = coveredByOtherScreen ? Math.Min(_pauseAlpha + 1f / 32, 1) : Math.Max(_pauseAlpha - 1f / 32, 0);

            if (IsActive)
            {
                // This game isn't very fun! You could probably improve
                // it by inserting something more interesting in this space :-)
                _elapsed += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

                if (_elapsed >= _delay)
                {
                    if (_frames >= 3)
                    {
                        _frames = 0;
                    }
                    else
                    {
                        _frames++;
                    }

                    _elapsed = 0;
                }

                if (CollisionHelper.Collides(_playerBound, _upperWallBound))
                {
                    _playerPosition -= new Vector2(0, -2);
                }

                if (CollisionHelper.Collides(_playerBound, _lowerWallBound))
                {
                    _playerPosition += new Vector2(0, -2);
                    _colliding = true;
                }
                else
                {
                    _colliding = false;
                }

                _playerBound.X = _playerPosition.X - 16;
                _playerBound.Y = _playerPosition.Y - 16;
            }
        }

        // Unlike the Update method, this will only be called when the gameplay screen is active.
        public override void HandleInput(GameTime gameTime, InputState input)
        {
            if (input == null)
                throw new ArgumentNullException(nameof(input));

            // Look up inputs for the active player profile.
            int playerIndex = (int)ControllingPlayer.Value;
            var keyboardState = input.CurrentKeyboardStates[playerIndex];

            PlayerIndex player;
            if (_pauseAction.Occurred(input, ControllingPlayer, out player))
            {
                // ScreenManager.AddScreen(new PauseMenuScreen(), ControllingPlayer);
            }
            else
            {
                // Otherwise move the player position.
                var movement = Vector2.Zero;

                if (keyboardState.IsKeyDown(Keys.Left) || keyboardState.IsKeyDown(Keys.A))
                    movement.X--;

                if (keyboardState.IsKeyDown(Keys.Right) || keyboardState.IsKeyDown(Keys.D))
                    movement.X++;

                if (keyboardState.IsKeyDown(Keys.Up) || keyboardState.IsKeyDown(Keys.W))
                    movement.Y--;

                if (keyboardState.IsKeyDown(Keys.Down) || keyboardState.IsKeyDown(Keys.S))
                    movement.Y++;

                if (_playerPosition.X < 0)
                {
                    _playerPosition.X = 0;
                }

                if (_playerPosition.X > ScreenManager.GraphicsDevice.Viewport.Width - 40)
                {
                    _playerPosition.X = ScreenManager.GraphicsDevice.Viewport.Width - 42;
                }

                if (movement.Length() > 1)
                    movement.Normalize();

                _playerPosition += movement * 2f;
            }
        }

        public override void Draw(GameTime gameTime)
        {
            var height = ScreenManager.GraphicsDevice.Viewport.Height;
            var width = ScreenManager.GraphicsDevice.Viewport.Width;
            // This game has a blue background. Why? Because!
            ScreenManager.GraphicsDevice.Clear(ClearOptions.Target, Color.Black, 0, 0);

            // Our player and enemy are both actually just text strings.
            var spriteBatch = ScreenManager.SpriteBatch;

            spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.LinearWrap, null, null);
            Matrix.CreateScale(4f);
            spriteBatch.Draw(_pathTexture, new Rectangle(0, height - 256, width, 200), new Rectangle(0, 280, 128, 60), Color.White);
            spriteBatch.End();

            spriteBatch.Begin();
            spriteBatch.Draw(_playerTexture, _playerPosition, _playerFrames[_frames], Color.White, 0f, Vector2.Zero, 3.0f, SpriteEffects.None, 0f);

            if (_colliding)
            {
                spriteBatch.DrawString(_instructionFont, "Good Job!", new Vector2((width / 3) - 250, (height / 3) - 20), Color.White);
            }
            else
            {
                spriteBatch.DrawString(_instructionFont, "Touch Bottom Wall", new Vector2((width / 3) - 250, (height / 3) - 20), Color.White);
            }
            spriteBatch.End();
        }
    }
}
