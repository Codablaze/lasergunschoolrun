namespace LaserGunSchoolRun;

// bools for now, floats for movement in future as gamepad motion will be smoother
public struct InputContext
{
    public bool MovingLeft { get; set; }
    public bool MovingRight { get; set; }
    public bool MovingForward { get; set; }
    public bool MovingBack { get; set; }
    public bool IsFiring { get; set; }
}
