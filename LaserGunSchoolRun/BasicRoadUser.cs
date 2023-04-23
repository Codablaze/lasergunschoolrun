using System.Net.Http.Headers;
using Microsoft.Xna.Framework;

namespace LaserGunSchoolRun;

public enum BasicRoadUserType
{
    Car,
    FastCar
}

public class BasicRoadUser
{
    private readonly float _speed;
    private readonly int _textureWidth;
    private readonly int _textureHeight;
    public BasicRoadUserType Type;
    public Vector2 Position;
    public Vector2 Direction = Vector2.UnitY;
    public int Health = Constants.BasicRoadUserHealth;
    public int Damage;

    public BasicRoadUser(BasicRoadUserType type, float speed, int textureWidth, int textureHeight, int damage)
    {
        Type = type;
        _speed = speed;
        _textureWidth = textureWidth;
        _textureHeight = textureHeight;
        Damage = damage;
    }

    public BoundingBox2 GetBounds() => new (Position, _textureWidth, _textureHeight);

    public void Update(float dt)
    {
        Position += Direction * _speed * dt;
    }
}