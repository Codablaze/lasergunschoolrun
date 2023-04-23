using Microsoft.Xna.Framework;

namespace LaserGunSchoolRun;

public class LaserShot
{
    public Vector2 Position;
    public Vector2 Direction = -Vector2.UnitY; // always up

    public const float Speed = Constants.LaserSpeed;
    public const int Damage = Constants.LaserDamage;
    private readonly int _textureWidth;
    private readonly int _textureHeight;

    public LaserShot(int textureWidth, int textureHeight)
    {
        _textureWidth = textureWidth;
        _textureHeight = textureHeight;
    }

    public BoundingBox2 GetBounds() => new(Position, _textureWidth, _textureHeight);


    public void Update(float dt)
    {
        Position += Direction * Speed * dt;
    }
}
