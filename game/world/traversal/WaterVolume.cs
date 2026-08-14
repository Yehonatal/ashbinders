using System;
using Godot;

namespace Ashbinders.World.Traversal;

[GlobalClass]
public partial class WaterVolume : Area2D
{
    [Export] public bool IsDeepWater { get; set; } = true;
    [Export] public float WaterSpeedMultiplier { get; set; } = 0.6f;
    [Export] public Vector2 WaterCurrent { get; set; } = Vector2.Zero;

    public override void _Ready()
    {
        CollisionLayer = 128; // Layer 8: WaterVolumes
        CollisionMask = 2;    // Layer 2: PlayerBody
    }
}
