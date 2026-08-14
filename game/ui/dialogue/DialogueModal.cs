using System;
using System.Collections.Generic;
using Godot;
using Ashbinders.Core.Events;

namespace Ashbinders.UI.Dialogue;

[GlobalClass]
public partial class DialogueModal : Control
{
    private Panel? _modalPanel;
    private Label? _speakerLabel;
    private Label? _dialogueTextLabel;
    private VBoxContainer? _optionsContainer;
    private Button? _closeButton;

    private string _fullText = string.Empty;
    private int _charIndex = 0;
    private double _typewriterTimer = 0.0;
    private DialogueChoiceOption[] _currentChoices = Array.Empty<DialogueChoiceOption>();

    public override void _Ready()
    {
        _modalPanel = GetNodeOrNull<Panel>("Control/ModalPanel") ?? GetNodeOrNull<Panel>("ModalPanel");
        _speakerLabel = GetNodeOrNull<Label>("Control/ModalPanel/SpeakerLabel") ?? GetNodeOrNull<Label>("ModalPanel/SpeakerLabel");
        _dialogueTextLabel = GetNodeOrNull<Label>("Control/ModalPanel/DialogueTextLabel") ?? GetNodeOrNull<Label>("ModalPanel/DialogueTextLabel");
        _optionsContainer = GetNodeOrNull<VBoxContainer>("Control/ModalPanel/OptionsContainer") ?? GetNodeOrNull<VBoxContainer>("ModalPanel/OptionsContainer");
        _closeButton = GetNodeOrNull<Button>("Control/ModalPanel/CloseButton") ?? GetNodeOrNull<Button>("ModalPanel/CloseButton");

        if (_closeButton != null)
        {
            _closeButton.Pressed += HideModal;
        }

        HideModal();
        EventBus.Subscribe<DialogueTriggerEvent>(OnDialogueTriggered);
    }

    public override void _ExitTree()
    {
        EventBus.Unsubscribe<DialogueTriggerEvent>(OnDialogueTriggered);
    }

    public override void _Process(double delta)
    {
        if (Visible && _charIndex < _fullText.Length && _dialogueTextLabel != null)
        {
            _typewriterTimer += delta;
            if (_typewriterTimer >= 0.015)
            {
                _typewriterTimer = 0.0;
                _charIndex++;
                _dialogueTextLabel.Text = _fullText.Substring(0, _charIndex);
            }
        }
    }

    private void OnDialogueTriggered(DialogueTriggerEvent evt)
    {
        ShowDialogue(evt.SpeakerName, evt.DialogueText, evt.Choices);
    }

    public void ShowDialogue(string speaker, string text, DialogueChoiceOption[] choices)
    {
        if (_speakerLabel != null) _speakerLabel.Text = speaker;
        _fullText = text;
        _charIndex = 0;
        _currentChoices = choices;
        if (_dialogueTextLabel != null) _dialogueTextLabel.Text = string.Empty;

        PopulateChoiceButtons();

        Visible = true;
        if (GetParent() is CanvasLayer canvas)
        {
            canvas.Visible = true;
        }
    }

    private void PopulateChoiceButtons()
    {
        if (_optionsContainer == null) return;

        foreach (Node child in _optionsContainer.GetChildren())
        {
            child.QueueFree();
        }

        foreach (var choice in _currentChoices)
        {
            var btn = new Button
            {
                Text = $"►  {choice.Label}",
                Alignment = HorizontalAlignment.Left
            };
            var optionCopy = choice;
            btn.Pressed += () => OnChoiceSelected(optionCopy);
            _optionsContainer.AddChild(btn);
        }
    }

    private void OnChoiceSelected(DialogueChoiceOption choice)
    {
        _fullText = choice.ResponseText;
        _charIndex = 0;
        if (_dialogueTextLabel != null) _dialogueTextLabel.Text = string.Empty;

        if (_optionsContainer != null)
        {
            foreach (Node child in _optionsContainer.GetChildren())
            {
                child.QueueFree();
            }

            var closeBtn = new Button
            {
                Text = "►  [ Continue ]",
                Alignment = HorizontalAlignment.Left
            };
            closeBtn.Pressed += HideModal;
            _optionsContainer.AddChild(closeBtn);
        }
    }

    public void HideModal()
    {
        Visible = false;
        if (GetParent() is CanvasLayer canvas)
        {
            canvas.Visible = false;
        }
    }
}
