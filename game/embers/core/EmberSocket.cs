using System;
using Godot;

namespace Ashbinders.Embers.Core;

[GlobalClass]
public partial class EmberSocket : Node, IEmberReceiver
{
    [Signal]
    public delegate void EmberSocketedEventHandler(Ember ember);

    [Signal]
    public delegate void EmberUnsocketedEventHandler(Ember ember);

    [Export] public EmberType AllowedType { get; set; } = EmberType.Motion;
    [Export] public bool AcceptAnyType { get; set; } = true;

    public Ember? CurrentEmber { get; private set; }
    public bool HasEmber => CurrentEmber != null;

    public bool AcceptsEmberType(EmberType type)
    {
        return AcceptAnyType || AllowedType == type;
    }

    public bool TryInsertEmber(Ember ember)
    {
        if (HasEmber || !AcceptsEmberType(ember.Type))
        {
            return false;
        }

        CurrentEmber = ember;
        EmitSignal(SignalName.EmberSocketed, ember);
        return true;
    }

    public Ember? TryExtractEmber()
    {
        if (!HasEmber || CurrentEmber == null)
        {
            return null;
        }

        var ember = CurrentEmber;
        CurrentEmber = null;
        EmitSignal(SignalName.EmberUnsocketed, ember);
        return ember;
    }
}
