namespace Ashbinders.Core.Save;

public interface ISaveable
{
    string SaveKey { get; }
    void Save(SaveData saveData);
    void Load(SaveData saveData);
}
