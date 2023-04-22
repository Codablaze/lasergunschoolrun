using Microsoft.Xna.Framework;

namespace LaserGunSchoolRun;

public enum BasicEnemyType
{
    Car,
    FastCar
}

public class BasicEnemy
{
    private readonly float _speed;

    public BasicEnemyType Type;
    public Vector2 Position;
    public Vector2 Direction = new (0, 1);

    public BasicEnemy(BasicEnemyType type, float speed)
    {
        Type = type;
        _speed = speed;
    }

    public void Update(float dt)
    {
        Position += Direction * _speed * dt;
    }
}