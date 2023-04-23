using System;
using System.Collections.Generic;

namespace LaserGunSchoolRun;

public class LaserGun
{
    private readonly List<LaserShot> _laserShots;
    private readonly int _textureWidth;
    private readonly int _textureHeight;
    private readonly TimeSpan Cooldown = TimeSpan.FromMilliseconds(160);
    private TimeSpan CurrentCooldown = TimeSpan.Zero;
    private readonly Player _player;

    public LaserGun(Player player, List<LaserShot> laserShots, int textureWidth, int textureHeight)
    {
        _player = player;
        _laserShots = laserShots;
        _textureWidth = textureWidth;
        _textureHeight = textureHeight;
    }

    public void Update(InputContext input, TimeSpan elapsed)
    {
        CurrentCooldown -= elapsed;
        if (CurrentCooldown >= TimeSpan.Zero || !input.IsFiring)
            return; // haven't waited long enough

        // cool beans, add a new projectile or laser or rocket, play sound
        _laserShots.Add(new LaserShot(_textureWidth, _textureHeight)
        {
            Position = _player.Position
        });
        CurrentCooldown = Cooldown;
    }
}
