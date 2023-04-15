namespace LaserGunSchoolRun;

// bools for now, floats for movement in future as gamepad motion will be smoother
public struct InputContext
{
    public bool MoveLeft { get; set; }
    public bool MoveRight { get; set; }
    public bool MoveForward { get; set; }
    public bool MoveBack { get; set; }
    public bool IsFiring { get; set; }
}
