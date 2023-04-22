//using System;
//using System.Collections.Generic;
//using System.Linq;
//using Microsoft.Xna.Framework;

//namespace LaserGunSchoolRun;

//public class World
//{
//    private readonly Player _player;
//    private readonly Weapon _weapon;
//    private readonly List<Projectile> _projectiles;
//    private readonly List<BasicEnemy> _enemies;
//    private readonly Hud _hud;
//    private readonly Random _rng = new(Seed: 420);

//    private readonly TimeSpan EnemyCooldown = TimeSpan.FromMilliseconds(1500);
//    private TimeSpan EnemyCurrentCooldown = TimeSpan.Zero;

//    public int ScreenWidth { get; set; }

//    public World(Player player, Weapon weapon, List<Projectile> projectiles, List<BasicEnemy> enemies, Hud hud)
//    {
//        _player = player;
//        _weapon = weapon;
//        _projectiles = projectiles;
//        _enemies = enemies;
//        _hud = hud;
//    }

//    public void Update(InputContext input, GameTime gameTime)
//    {
//        var dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

//        _player.Update(input, dt);
//        _weapon.Update(input, gameTime.ElapsedGameTime);

//        foreach (var item in _projectiles)
//            item.Update(dt);

//        foreach (var item in _enemies)
//            item.Update(dt);
//    }

//}
