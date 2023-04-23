using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace LaserGunSchoolRun;

public class Background
{
    private readonly SpriteBatch _spriteBatch;
    private readonly Rectangle _viewport;
    private Texture2D _road;
    private float _offset;
    private float _offset2;

    // removing the two draw calls wraps the texture on the background automatically, this
    // is not good when the background changes thoughout the level

    public Background(SpriteBatch spriteBatch, Rectangle viewport)
    {
        _spriteBatch = spriteBatch;
        _viewport = viewport;

        _offset2 = -viewport.Height;
    }

    public void LoadContent(ContentManager content)
    {
        _road = content.Load<Texture2D>("road");
    }

    public void Update(float dt)
    {
        _offset += Constants.BackgroundScrollSpeed * dt;
        _offset2 += Constants.BackgroundScrollSpeed * dt;

        //if (_offset > _viewport.Height)
        //    _offset = 0;
        //if (_offset2 > 0)
        //    _offset2 = -_viewport.Height;
    }

    public void Draw()
    {
        _spriteBatch.Begin(samplerState: SamplerState.LinearWrap, transformMatrix: Matrix.Identity);

        _spriteBatch.Draw(_road, _viewport, new Rectangle(0, (int)_offset, _viewport.Width, _viewport.Height), Color.White);

        //_spriteBatch.Draw(_road, new Vector2(0, _offset), _viewport, Color.White);
        //_spriteBatch.Draw(_road, new Vector2(0, _offset2), _viewport, Color.White);

        _spriteBatch.End();
    }
}