using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Nodes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace LaserGunSchoolRun;

public class GameMain : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private Texture2D _rangeRover, _roadUser;
    private Player _player;
    private Background _bg;
    private BasicRoadUserManager _basicEnemies;
    private bool _started = false;

    private Vector2 _playerStartPosition;

    public GameMain()
    {
        //var display = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode;
        _graphics = new GraphicsDeviceManager(this)
        {
            PreferredBackBufferWidth = Constants.HorizontalResolution,
            PreferredBackBufferHeight = Constants.VerticalResolution
        };
        Content.RootDirectory = "Content";
        IsMouseVisible = false;
        IsFixedTimeStep = false;
    }

    protected override void Initialize()
    {
        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        _rangeRover = Content.Load<Texture2D>("Range_Rover_Small");
        _roadUser = Content.Load<Texture2D>("Random_Road_User");

        var bounds = GraphicsDevice.Viewport.Bounds;

        _player = new Player(bounds, _rangeRover.Width, _rangeRover.Height);
        _player.Position.X = Constants.HorizontalResolution / 2 - _rangeRover.Width / 2;
        _player.Position.Y = Constants.VerticalResolution + 100; // place off screen

        _playerStartPosition = new Vector2(
            _player.Position.X, Constants.VerticalResolution - 150);

        _basicEnemies = new BasicRoadUserManager(bounds, _roadUser.Width, _roadUser.Height);

        _bg = new Background(_spriteBatch, bounds);
        _bg.LoadContent(Content);
    }

    protected override void Update(GameTime gameTime)
    {
        var dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
        _bg.Update(dt);

        // move to starting point, probably some sort of sound and text to indicate the level has started
        if (!_started)
        {
            _player.Position = Vector2.Lerp(_player.Position, _playerStartPosition, 0.08f);
            var len = _playerStartPosition - _player.Position;
            if (len.LengthSquared() < 1f)
                _started = true;
        }
        else
        {
            var keyboard = Keyboard.GetState();
            var mouse = Mouse.GetState();

            if (keyboard.IsKeyDown(Keys.Escape))
                Exit();

            var input = new InputContext
            {
                MovingLeft = keyboard.IsKeyDown(Keys.A) || keyboard.IsKeyDown(Keys.Left),
                MovingRight = keyboard.IsKeyDown(Keys.D) || keyboard.IsKeyDown(Keys.Right),
                MovingBack = keyboard.IsKeyDown(Keys.S) || keyboard.IsKeyDown(Keys.Down),
                MovingForward = keyboard.IsKeyDown(Keys.W) || keyboard.IsKeyDown(Keys.Up),
                IsFiring = mouse.LeftButton == ButtonState.Pressed || keyboard.IsKeyDown(Keys.Space)
            };

            _basicEnemies.Update(gameTime, dt);
            _player.Update(input, dt);
        }

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Black);

        _bg.Draw(); // uses graphics card texture wrapping for ultimate perf

        // draw screen items
        _spriteBatch.Begin();

        foreach (var enemy in _basicEnemies)
        {
            DrawCentered(_roadUser, enemy.Position, enemy.Type == BasicEnemyType.Car ? Color.White : Color.Red);
        }

        DrawCentered(_rangeRover, _player.Position);

        _spriteBatch.End();

        base.Draw(gameTime);
    }

    private void DrawCentered(Texture2D tex, Vector2 pos, Color? color = null)
    {
        _spriteBatch.Draw(
            texture: tex,
            position: pos,
            sourceRectangle: null,
            color: color ?? Color.White,
            rotation: 0f,
            origin: new Vector2(tex.Width / 2, tex.Height / 2),
            scale: 1f,
            SpriteEffects.None,
            1f);
    }
}