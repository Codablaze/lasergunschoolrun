using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace LaserGunSchoolRun;

public class Weapon
{
    private readonly List<Projectile> _projectiles;
    private readonly TimeSpan Cooldown = TimeSpan.FromMilliseconds(200); // can fire every 200ms
    private TimeSpan CurrentCooldown = TimeSpan.Zero;
    private readonly Player _player;

    public Weapon(Player player, List<Projectile> projectiles)
    {
        _player = player;
        _projectiles = projectiles;
    }

    public void Update(InputContext context, TimeSpan elapsed)
    {
        CurrentCooldown -= elapsed;
        if (CurrentCooldown >= TimeSpan.Zero || !context.IsFiring)
            return; // haven't waited long enough

        // cool beans, add a new projectile or laser or rocket, play sound
        _projectiles.Add(new Projectile
        {
            Position = _player.Position
        });
        CurrentCooldown = Cooldown;
    }
}
