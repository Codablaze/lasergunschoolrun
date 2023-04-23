using Microsoft.Xna.Framework;

namespace LaserGunSchoolRun;

public class Player
{
    public Vector2 Position;
    public Vector2 Velocity;
    public int Health = Constants.DefaultPlayerHealth;

    private readonly Rectangle Bounds;
    private readonly int _textureWidth;
    private readonly int _textureHeight;
    private readonly int halfWidth;
    private readonly int halfHeight;

    public Player(Rectangle viewport, int textureWidth, int textureHeight)
    {
        Bounds = viewport;
        _textureWidth = textureWidth;
        _textureHeight = textureHeight;
        halfWidth = textureWidth / 2;
        halfHeight = textureHeight / 2;
    }

    public BoundingBox2 GetBounds() => new(Position, _textureWidth, _textureHeight);

    public void Update(InputContext input, float dt)
    {
        UpdatePosition(input, dt);
        ClampPosition();
    }

    private void UpdatePosition(InputContext context, float dt)
    {
        // move player first
        if (context.MovingLeft)
            Velocity.X -= Constants.PlayerAcceleration * dt;
        if (context.MovingRight)
            Velocity.X += Constants.PlayerAcceleration * dt;
        if (context.MovingForward)
            Velocity.Y -= Constants.PlayerAcceleration * dt;
        if (context.MovingBack)
            Velocity.Y += Constants.PlayerAcceleration * dt;

        Position += Velocity;
        Velocity *= 0.9f; // rudimental, doesn't account for varying frame times
    }

    private void ClampPosition()
    {
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
