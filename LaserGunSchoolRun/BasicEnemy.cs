using Microsoft.Xna.Framework;

namespace LaserGunSchoolRun;

public class BasicEnemy
{
    public Vector2 Position;
    public Vector2 Direction;
    public float Health = 1f; // [0-1]

    public static float NextSpeed = 50f;
    public float Speed;

    public static int Width, Height;

    public void Update(float dt)
    {
        Position += Direction * Speed * dt;
    }

    public Rectangle GetBounds()
    {
        return new Rectangle(
            (int)Position.X - Width / 2,
            (int)Position.Y - Height / 2,
            Width, Height);
    }
}
