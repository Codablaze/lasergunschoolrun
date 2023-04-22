using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace LaserGunSchoolRun;

public class BasicRoadUserManager : IEnumerable<BasicEnemy>
{
    private readonly List<BasicEnemy> _enemies = new();
    private TimeSpan creationTimer = TimeSpan.FromSeconds(Constants.InitialBasicEnemyCreationDelaySeconds);
    private TimeSpan currentTime;
    private readonly Random _rng = new(Constants.RandomSeed); // providing seed as want game to be same each time
    private readonly Rectangle _viewport;
    private readonly int _textureWidth;
    private readonly int _textureHeight;
    private const int StartOffset = -100;

    public BasicRoadUserManager(Rectangle viewport, int textureWidth, int textureHeight)
    {
        _viewport = viewport;
        _textureWidth = textureWidth;
        _textureHeight = textureHeight;
    }

    private readonly List<BasicEnemy> toRemove = new();

    public void Update(GameTime gameTime, float dt)
    {
        toRemove.Clear();

        // update enemies
        foreach (var item in _enemies)
        {
            item.Update(dt);

            if (item.Position.Y - _textureHeight/2 > _viewport.Height)
                toRemove.Add(item);
        }

        // remove any off screen
        foreach (var item in toRemove)
            _enemies.Remove(item);

        // create a new one if timer has elapsed
        currentTime += gameTime.ElapsedGameTime;
        if (currentTime > creationTimer)
        {
            GenerateRoadUser();
            currentTime = TimeSpan.Zero;
        }
    }

    private void GenerateRoadUser()
    {
        // 10 lanes, randomly placed road user
        var laneWidth = Constants.HorizontalResolution / Constants.LaneCount;
        var nextLane = _rng.Next(Constants.LaneCount);
        var nextLaneCenter = laneWidth * nextLane + _textureWidth;

        var nextType = BasicEnemyType.Car;
        if (_rng.NextDouble() > 1.0 - Constants.FastCarChance) // 10% chance of being sports car
            nextType = BasicEnemyType.FastCar;

        var nextSpeed = nextType == BasicEnemyType.Car ? Constants.StandardCarSpeed : Constants.FastCarSpeed;

        _enemies.Add(new BasicEnemy(nextType, nextSpeed)
        {
            Position = new Vector2(nextLaneCenter, StartOffset)
        });
    }

    public IEnumerator<BasicEnemy> GetEnumerator()
    {
        return _enemies.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
