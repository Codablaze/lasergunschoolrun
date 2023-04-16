using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace LaserGunSchoolRun;

public class World
{
    private readonly Player _player;
    private readonly Weapon _weapon;
    private readonly List<Projectile> _projectiles;
    private readonly List<BasicEnemy> _enemies;
    private readonly Hud _hud;
    private readonly Random _rng = new(Seed: 420);

    private readonly TimeSpan EnemyCooldown = TimeSpan.FromMilliseconds(1500);
    private TimeSpan EnemyCurrentCooldown = TimeSpan.Zero;

    public int ScreenWidth { get; set; }

    public World(Player player, Weapon weapon, List<Projectile> projectiles, List<BasicEnemy> enemies, Hud hud)
    {
        _player = player;
        _weapon = weapon;
        _projectiles = projectiles;
        _enemies = enemies;
        _hud = hud;
    }

    public void Update(InputContext input, GameTime gameTime)
    {
        var dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

        _player.Update(input, dt);
        _weapon.Update(input, gameTime.ElapsedGameTime);

        foreach (var item in _projectiles)
            item.Update(dt);

        foreach (var item in _enemies)
            item.Update(dt);

        GenerateEnemies(gameTime.ElapsedGameTime);
        HandleProjectilesHittingEnemies();
    }

    private void HandleProjectilesHittingEnemies()
    {
        var enemiesToRemove = new List<BasicEnemy>();
        var projectilesToRemove = new List<Projectile>();

        // n squared algorithm, probably always ok for such a small game, worst case can use an elementary quad tree with probably only 2 levels
        foreach (var p in _projectiles)
        {
            foreach (var e in _enemies)
            {
                if (CollisionHelper.Touches(p.GetBounds(), e.GetBounds()))
                {
                    // decrease enemy health
                    e.Health -= Projectile.Damage;
                    if (e.Health <= 0)
                        enemiesToRemove.Add(e);

                    projectilesToRemove.Add(p);
                }
            }
        }

        // remove dead enemies from the list
        foreach (var item in enemiesToRemove)
        {
            _enemies.Remove(item);
            _hud.EnemiesKilled++;
        }

        // cull projectiles outside of screen
        foreach (var item in _projectiles)
            if (item.Position.Y < 10)
                projectilesToRemove.Add(item);

        // remove dead projectiles
        foreach (var item in projectilesToRemove)
            _projectiles.Remove(item);
    }

    private void GenerateEnemies(TimeSpan elapsed)
    {
        EnemyCurrentCooldown -= elapsed;
        if (EnemyCurrentCooldown >= TimeSpan.Zero)
            return;

        var toCreate = _rng.NextDouble() > .98 ? 2 : 1; // 2% chance of generating 2 enemies

        // increase the speed a little each time we create an enemy
        BasicEnemy.NextSpeed += 4f;

        for (int i = 0; i < toCreate; i++)
        {
            var pos = new Vector2(_rng.Next(10, ScreenWidth - 10), -100); // off screen

            var newEnemy = new BasicEnemy
            {
                Speed = BasicEnemy.NextSpeed,
                Position = pos,
                Direction = Vector2.UnitY // always straight down
            };

            _enemies.Add(newEnemy);
        }

        EnemyCurrentCooldown = EnemyCooldown;
    }
}