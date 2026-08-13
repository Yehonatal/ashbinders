namespace Ashbinders.Embers.Core;

public interface IEmberReceiver
{
    bool AcceptsEmberType(EmberType type);
    bool TryInsertEmber(Ember ember);
    Ember? TryExtractEmber();
    bool HasEmber { get; }
    Ember? CurrentEmber { get; }
}
