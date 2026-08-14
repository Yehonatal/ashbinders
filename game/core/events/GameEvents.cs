using System;

namespace Ashbinders.Core.Events;

public record HealthChangedEvent(int CurrentHealth, int MaxHealth) : IGameEvent;
public record EmberExtractedEvent(string EmberType) : IGameEvent;
public record EmberSocketedEvent(string EmberType) : IGameEvent;
public record WeaponHeadChangedEvent(string WeaponHeadName) : IGameEvent;
public record ToastNotificationEvent(string Message) : IGameEvent;
public record DialogueTriggerEvent(string SpeakerName, string DialogueText, DialogueChoiceOption[] Choices) : IGameEvent;
public record DeviceActivatedEvent(string DeviceId) : IGameEvent;
public record GateOpenedEvent(string GateId) : IGameEvent;
public record GameSavedEvent(string SaveLocation) : IGameEvent;

public record DialogueChoiceOption(string Label, string ResponseText);
