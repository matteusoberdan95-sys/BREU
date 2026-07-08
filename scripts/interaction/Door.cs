using Godot;
using BREU.Scripts.Inventory;
using BREU.Scripts.Player;

namespace BREU.Scripts.Interaction;

public partial class Door : StaticBody3D, IInteractable
{
    [Export] public bool IsLocked { get; set; }
    [Export] public string RequiredKeyId { get; set; } = "";
    [Export] public float OpenAngleDegrees { get; set; } = 92.0f;
    [Export] public float OpenSpeed { get; set; } = 6.0f;

    public string Prompt => IsLocked ? "Porta trancada" : (_isOpen ? "Fechar porta" : "Abrir porta");

    private bool _isOpen;
    private float _closedY;
    private float _targetY;

    public override void _Ready()
    {
        _closedY = Rotation.Y;
        _targetY = _closedY;
    }

    public override void _Process(double delta)
    {
        var rotation = Rotation;
        rotation.Y = Mathf.LerpAngle(rotation.Y, _targetY, OpenSpeed * (float)delta);
        Rotation = rotation;
    }

    public void Interact(PlayerController player)
    {
        if (IsLocked)
        {
            var inventory = player.GetNodeOrNull<PlayerInventory>("PlayerInventory");
            if (inventory?.HasKey(RequiredKeyId) == true)
            {
                IsLocked = false;
                GD.Print("Porta destrancada.");
            }
            else
            {
                GD.Print("A porta nao abre. Algo prende pelo outro lado.");
                return;
            }
        }

        _isOpen = !_isOpen;
        _targetY = _isOpen ? _closedY + Mathf.DegToRad(OpenAngleDegrees) : _closedY;
        GD.Print(_isOpen ? "Porta aberta." : "Porta fechada.");
    }
}
