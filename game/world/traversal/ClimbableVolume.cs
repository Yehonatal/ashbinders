using System;
using Godot;

namespace Ashbinders.World.Traversal;

[GlobalClass]
public partial class ClimbableVolume : Area2D
{
    [Export] public float LadderHeight { get; set; } = 120.0f;
    [Export] public float ClimbSpeed { get; set; } = 140.0f;
    [Export] public Vector2 TopLedgeOffset { get; set; } = new Vector2(0, -140.0f);
    [Export] public Vector2 BottomFloorOffset { get; set; } = new Vector2(0, 10.0f);

    public Vector2 TopLedgeGlobalPosition => GlobalPosition + TopLedgeOffset;
    public Vector2 BottomFloorGlobalPosition => GlobalPosition + BottomFloorOffset;

    public override void _Ready()
    {
        CollisionLayer = 256; // Layer 9: TraversalVolumes
        CollisionMask = 2;    // Layer 2: PlayerBody
    }
}
