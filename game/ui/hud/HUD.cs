using System;
using Godot;
using Ashbinders.Core.Events;
using Ashbinders.Embers.Core;

namespace Ashbinders.UI.HUD;

[GlobalClass]
public partial class HUD : CanvasLayer
{
    private ProgressBar? _healthBar;
    private Label? _healthLabel;
    private Label? _weaponLabel;
    private Label? _socketedEmberLabel;
    private Label? _extractedEmberLabel;
    private Panel? _toastPanel;
    private Label? _toastLabel;
    private double _toastTimer;

    private string _currentSocketedEmber = "Motion Ember (+30% Move Speed)";
    private string _currentHeldEmber = "None";

    public override void _Ready()
    {
        _healthBar = GetNodeOrNull<ProgressBar>("Control/HudPanel/HealthBar") ?? GetNodeOrNull<ProgressBar>("Control/HealthBar");
        _healthLabel = GetNodeOrNull<Label>("Control/HudPanel/HealthBar/HealthLabel") ?? GetNodeOrNull<Label>("Control/HealthBar/HealthLabel");
        _weaponLabel = GetNodeOrNull<Label>("Control/HudPanel/WeaponLabel") ?? GetNodeOrNull<Label>("Control/WeaponLabel");
        _socketedEmberLabel = GetNodeOrNull<Label>("Control/HudPanel/SocketedEmberLabel");
        _extractedEmberLabel = GetNodeOrNull<Label>("Control/HudPanel/ExtractedEmberLabel");
        _toastPanel = GetNodeOrNull<Panel>("Control/ToastPanel");
        _toastLabel = GetNodeOrNull<Label>("Control/ToastPanel/ToastLabel") ?? GetNodeOrNull<Label>("Control/ToastLabel");

        EventBus.Subscribe<HealthChangedEvent>(OnHealthChanged);
        EventBus.Subscribe<WeaponHeadChangedEvent>(OnWeaponHeadChanged);
        EventBus.Subscribe<EmberExtractedEvent>(OnEmberExtracted);
        EventBus.Subscribe<EmberSocketedEvent>(OnEmberSocketed);
        EventBus.Subscribe<ToastNotificationEvent>(OnToastNotification);
        EventBus.Subscribe<GameSavedEvent>(OnGameSaved);

        if (_toastPanel != null) _toastPanel.Visible = false;
        if (_toastLabel != null && _toastPanel == null) _toastLabel.Visible = false;

        UpdateEmberLabels();
    }

    public override void _ExitTree()
    {
        EventBus.Unsubscribe<HealthChangedEvent>(OnHealthChanged);
        EventBus.Unsubscribe<WeaponHeadChangedEvent>(OnWeaponHeadChanged);
        EventBus.Unsubscribe<EmberExtractedEvent>(OnEmberExtracted);
        EventBus.Unsubscribe<EmberSocketedEvent>(OnEmberSocketed);
        EventBus.Unsubscribe<ToastNotificationEvent>(OnToastNotification);
        EventBus.Unsubscribe<GameSavedEvent>(OnGameSaved);
    }

    public override void _Process(double delta)
    {
        if (_toastTimer > 0.0)
        {
            _toastTimer -= delta;
            if (_toastTimer <= 0.0)
            {
                if (_toastPanel != null) _toastPanel.Visible = false;
                if (_toastLabel != null) _toastLabel.Visible = false;
            }
        }
    }

    private void OnHealthChanged(HealthChangedEvent evt)
    {
        if (_healthBar != null)
        {
            _healthBar.MaxValue = evt.MaxHealth;
            _healthBar.Value = evt.CurrentHealth;
        }
        if (_healthLabel != null)
        {
            _healthLabel.Text = $"HP: {evt.CurrentHealth} / {evt.MaxHealth}";
        }
    }

    private void OnWeaponHeadChanged(WeaponHeadChangedEvent evt)
    {
        if (_weaponLabel != null)
        {
            _weaponLabel.Text = $"Active Head: {evt.WeaponHeadName} (Keys 1-4 / Q)";
        }
    }

    private void OnEmberExtracted(EmberExtractedEvent evt)
    {
        _currentHeldEmber = $"{evt.EmberType} Ember";
        UpdateEmberLabels();
    }

    private void OnEmberSocketed(EmberSocketedEvent evt)
    {
        _currentSocketedEmber = $"{evt.EmberType} Ember";
        UpdateEmberLabels();
    }

    private void UpdateEmberLabels()
    {
        if (_socketedEmberLabel != null)
        {
            _socketedEmberLabel.Text = $"Socketed Ember: {_currentSocketedEmber}";
        }
        if (_extractedEmberLabel != null)
        {
            _extractedEmberLabel.Text = $"Held Extracted Ember: {_currentHeldEmber}";
        }
    }

    private void OnToastNotification(ToastNotificationEvent evt)
    {
        ShowToast(evt.Message);
    }

    private void OnGameSaved(GameSavedEvent evt)
    {
        ShowToast($"💾 Game Saved at {evt.SaveLocation}");
    }

    public void ShowToast(string text)
    {
        if (_toastLabel != null)
        {
            _toastLabel.Text = text;
            if (_toastPanel != null) _toastPanel.Visible = true;
            _toastLabel.Visible = true;
            _toastTimer = 3.0;
        }
    }
}
