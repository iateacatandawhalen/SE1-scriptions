public Program()
{
    Runtime.UpdateFrequency = UpdateFrequency.Update10; // Runs every 10 ticks (~166ms)
}

// Change these names to match your hinge blocks
const string HINGE_PRIMARY = "Hinge_Left";  // The hinge to copy from
const string HINGE_SECONDARY = "Hinge_Right"; // The hinge to mirror

void Main()
{
    IMyMotorStator hingePrimary = GridTerminalSystem.GetBlockWithName(HINGE_PRIMARY) as IMyMotorStator;
    IMyMotorStator hingeSecondary = GridTerminalSystem.GetBlockWithName(HINGE_SECONDARY) as IMyMotorStator;

    if (hingePrimary == null || hingeSecondary == null)
    {
        Echo("Error: Hinges not found.");
        return;
    }

    // Get the current angle of the primary hinge
    float primaryAngle = hingePrimary.Angle; // Radians

    // Mirroring logic: Flip the angle if the hinge is reversed
    hingeSecondary.TargetVelocityRad = -hingePrimary.TargetVelocityRad; // Match speed
    hingeSecondary.RotorLock = hingePrimary.RotorLock; // Match locking state
    hingeSecondary.SetValueFloat("UpperLimit", -hingePrimary.GetValueFloat("LowerLimit"));
    hingeSecondary.SetValueFloat("LowerLimit", -hingePrimary.GetValueFloat("UpperLimit"));

    // Set the mirrored hinge angle
    hingeSecondary.SetValueFloat("Velocity", hingePrimary.GetValueFloat("Velocity"));
    hingeSecondary.SetValueFloat("Displacement", -hingePrimary.GetValueFloat("Displacement"));

    // Convert to degrees for display
    Echo($"Primary Angle: {MathHelper.ToDegrees(primaryAngle):0.00}°");
    Echo($"Mirrored Angle: {-MathHelper.ToDegrees(primaryAngle):0.00}°");
}
