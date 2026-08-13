using System;
using System.Collections.Generic;

namespace Ashbinders.Core.Save;

public class PlayerSaveData
{
    public float PositionX { get; set; }
    public float PositionY { get; set; }
    public int CurrentHealth { get; set; } = 100;
    public int MaxHealth { get; set; } = 100;
    public List<string> SocketedEmbers { get; set; } = new();
    public int EmberCapacity { get; set; } = 1;
}

public class WorldSaveData
{
    public HashSet<string> ActivatedDevices { get; set; } = new();
    public HashSet<string> DefeatedEnemies { get; set; } = new();
    public HashSet<string> CollectedEmbers { get; set; } = new();
    public Dictionary<string, string> CustomFlags { get; set; } = new();
}

public class SaveData
{
    public int SchemaVersion { get; set; } = 1;
    public DateTime SaveTimestampUtc { get; set; } = DateTime.UtcNow;
    public PlayerSaveData Player { get; set; } = new();
    public WorldSaveData World { get; set; } = new();
}
