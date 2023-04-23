using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace LaserGunSchoolRun;

public class GameMain : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private Texture2D _rangeRover, _roadUser, _laser, _pixel;
    private Player _player;
    private Background _bg;
    private BasicRoadUserManager _basicRoadUserManager;
    private LaserGun _laserGun;
    private List<LaserShot> _laserShots = new();
    private bool _started = false, _gameOver = false;

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
        _laser = Content.Load<Texture2D>("Laser");
        _pixel = Content.Load<Texture2D>("Pixel");

        var bounds = GraphicsDevice.Viewport.Bounds;

        _player = new Player(bounds, _rangeRover.Width, _rangeRover.Height);
        _player.Position.X = Constants.HorizontalResolution / 2 - _rangeRover.Width / 2;
        _player.Position.Y = Constants.VerticalResolution + 100; // place off screen

        _playerStartPosition = new Vector2(
            _player.Position.X, Constants.VerticalResolution - 150);

        _basicRoadUserManager = new BasicRoadUserManager(bounds, _roadUser.Width, _roadUser.Height);

        _laserGun = new LaserGun(_player, _laserShots, _laser.Width, _laser.Height);

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

            _basicRoadUserManager.Update(gameTime, dt);
            _player.Update(input, dt);

            // update weapon and lasers
            _laserGun.Update(input, gameTime.ElapsedGameTime);

            // update laser shots
            foreach (var item in _laserShots)
                item.Update(dt);

            // check for collisions between laser shots and basic road users
            var laserShotsToRemove = new List<LaserShot>();
            var roadUsersToRemove = new List<BasicRoadUser>();

            foreach (var roadUser in _basicRoadUserManager)
            {
                var userBounds = roadUser.GetBounds();
                foreach (var laserShot in _laserShots)
                {
                    var laserBounds = laserShot.GetBounds();
                    if (userBounds.Intersects(laserBounds))
                    {
                        // remove this laser shot if it touches an enemy
                        laserShotsToRemove.Add(laserShot);

                        // decrease road user health
                        roadUser.Health -= LaserShot.Damage;

                        if (roadUser.Health <= 0)
                            roadUsersToRemove.Add(roadUser);
                    }
                }
            }

            // check for collisions between road users and player
            var playerBounds = _player.GetBounds();
            foreach (var roadUser in _basicRoadUserManager)
            {
                var userBounds = roadUser.GetBounds();
                if (userBounds.Intersects(playerBounds))
                {
                    // hit car
                    roadUsersToRemove.Add(roadUser);
                    _player.Health -= roadUser.Damage;
                }
            }

            foreach (var item in roadUsersToRemove)
                _basicRoadUserManager.Remove(item);

            // clean up lasers which have left the screen
            foreach (var laserShot in _laserShots)
            {
                if (laserShot.Position.Y < -_laser.Height / 2)
                    laserShotsToRemove.Add(laserShot);
            }

            foreach (var item in laserShotsToRemove)
                _laserShots.Remove(item);

            if (_player.Health <= 0)
                _gameOver = true;
        }

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Black);

        if (_gameOver) return;

        _bg.Draw(); // uses graphics card texture wrapping for ultimate perf

        // draw screen items
        _spriteBatch.Begin();

        foreach (var enemy in _basicRoadUserManager)
            DrawCentered(_roadUser, enemy.Position, enemy.Type == BasicRoadUserType.Car ? Color.White : Color.Red);

        foreach (var laserShot in _laserShots)
            DrawCentered(_laser, laserShot.Position);

        DrawCentered(_rangeRover, _player.Position);

        DrawHealthBar();

        _spriteBatch.End();

        base.Draw(gameTime);
    }

    private void DrawHealthBar()
    {
        _spriteBatch.Draw(_pixel, new Rectangle(Constants.HorizontalResolution - 220, 30, 200, 20), Color.DarkGray);
        var playerHealthPercentage = _player.Health / 100.0;
        var color = Color.Green;
        if (playerHealthPercentage < 0.3)
            color = Color.Orange;
        else if (playerHealthPercentage < 0.1)
            color = Color.Red;

        _spriteBatch.Draw(_pixel, new Rectangle(Constants.HorizontalResolution - 220, 30, (int)(200.0 * playerHealthPercentage), 20), color);
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