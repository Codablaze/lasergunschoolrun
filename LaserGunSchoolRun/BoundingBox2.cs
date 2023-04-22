using Microsoft.Xna.Framework;

namespace LaserGunSchoolRun;

public readonly struct BoundingBox2
{
    private readonly float left, right, top, bottom;

    public BoundingBox2(Vector2 center, float width, float height)
    {
        left = center.X - width / 2;
        right = center.X + width / 2;
        top = center.Y - height / 2;
        bottom = center.Y + height / 2;
    }

    public bool Intersects(BoundingBox2 B) // might be useful to know by how much later
    {
        var AisToTheRightOfB = left > B.right;
        var AisToTheLeftOfB = right < B.left;
        var AisAboveB = bottom < B.top;
        var AisBelowB = top > B.bottom;
        return !(AisToTheRightOfB || AisToTheLeftOfB || AisAboveB || AisBelowB);
    }
}
