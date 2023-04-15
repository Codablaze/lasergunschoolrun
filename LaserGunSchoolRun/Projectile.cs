using Microsoft.Xna.Framework;

namespace LaserGunSchoolRun;

public class Projectile
{
    public Vector2 Position;
    public Vector2 Direction = -Vector2.UnitY; // always up

    private const float Speed = 800f;

    public void Update(float dt)
    {
        Position += Direction * Speed * dt;
    }
}
