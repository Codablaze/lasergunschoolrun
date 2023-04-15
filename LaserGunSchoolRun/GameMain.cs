using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace LaserGunSchoolRun;

public class GameMain : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private Vector2 _pos;
    private Texture2D _monster;

    public GameMain()
    {
        _graphics = new GraphicsDeviceManager(this)
        {
            PreferredBackBufferWidth = 1920 / 2,
            PreferredBackBufferHeight = 1080 / 2
        };
        Content.RootDirectory = "Content";
        IsMouseVisible = false;
    }

    protected override void Initialize()
    {
        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        _monster = Content.Load<Texture2D>("Monster_Energy");
        _pos.X = GraphicsDevice.Viewport.Width / 2 - _monster.Width / 2;
        _pos.Y = GraphicsDevice.Viewport.Height / 2 - _monster.Height / 2;
    }

    protected override void Update(GameTime gameTime)
    {
        var kb = Keyboard.GetState();
        if (kb.IsKeyDown(Keys.Escape))
            Exit();

        if (kb.IsKeyDown(Keys.A))
            _pos.X -= 3;
        if (kb.IsKeyDown(Keys.D))
            _pos.X += 3;
        if (kb.IsKeyDown(Keys.W))
            _pos.Y -= 3;
        if (kb.IsKeyDown(Keys.S))
            _pos.Y += 3;

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Black);

        _spriteBatch.Begin();

        _spriteBatch.Draw(_monster, _pos, Color.White);

        _spriteBatch.End();

        base.Draw(gameTime);
    }
}