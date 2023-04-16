using Microsoft.Xna.Framework;

namespace LaserGunSchoolRun;

public class Player
{
    public Vector2 Position;
    public Rectangle Bounds;
    public float Width, Height;

    private const float MoveAmount = 400f;

    public void Update(InputContext input, float dt)
    {
        UpdatePosition(input, dt);
        ClampPosition();
    }

    private void UpdatePosition(InputContext context, float dt)
    {
        // move player first
        if (context.MoveLeft)
            Position.X -= MoveAmount * dt;
        if (context.MoveRight)
            Position.X += MoveAmount * dt;
        if (context.MoveForward)
            Position.Y -= MoveAmount * dt;
        if (context.MoveBack)
            Position.Y += MoveAmount * dt;
    }

    private void ClampPosition()
    {
        var halfWidth = Width / 2;
        var halfHeight = Height / 2;

        // keep player in bounds
        if (Position.X + halfWidth > Bounds.Right)
            Position.X = Bounds.Right - halfWidth; // clamp to right side of bounds
        if (Position.X - halfWidth < Bounds.Left)
            Position.X = Bounds.Left + halfWidth; // clamp to left side of bounds
        if (Position.Y + halfHeight > Bounds.Bottom)
            Position.Y = Bounds.Bottom - halfHeight; // clamp to bottom side of bounds
        if (Position.Y - halfHeight < Bounds.Top)
            Position.Y = Bounds.Top + halfHeight; // clamp to top side of bounds
    }
}
