using Microsoft.Xna.Framework;

namespace LaserGunSchoolRun;

public class CollisionHelper
{
    public static bool Touches(Rectangle a, Rectangle b)
    {
        return a.Intersects(b);
    }
}
