namespace BREU.Scripts.Ui;

public partial class NoteReaderUI : CanvasLayer
{
    [Export] public NodePath RootPath { get; set; } = "Root";
    [Export] public NodePath TitleLabelPath { get; set; } = "Root/NotePanel/VBoxContainer/TitleLabel";
    [Export] public NodePath BodyLabelPath { get; set; } = "Root/NotePanel/VBoxContainer/BodyLabel";

    private Control? _root;
    private Label? _titleLabel;
    private Label? _bodyLabel;
    private PlayerController? _player;

    public bool IsOpen => _root?.Visible == true;

    public override void _Ready()
    {
        AddToGroup("note_reader");
        _root = GetNodeOrNull<Control>(RootPath);
        _titleLabel = GetNodeOrNull<Label>(TitleLabelPath);
        _bodyLabel = GetNodeOrNull<Label>(BodyLabelPath);
        HideNote();
    }

    public override void _Input(InputEvent @event)
    {
        if (!IsOpen)
        {
            return;
        }

        if (@event.IsActionPressed("interact") || @event.IsActionPressed("pause"))
        {
            HideNote();
            GetViewport().SetInputAsHandled();
        }
    }

    public void ShowNote(string title, string body)
    {
        if (_titleLabel != null)
        {
            _titleLabel.Text = title;
        }

        if (_bodyLabel != null)
        {
            _bodyLabel.Text = body;
        }

        if (_root != null)
        {
            _root.Visible = true;
        }

        ProcessMode = ProcessModeEnum.Always;
        AddToGroup("note_reader_active");

        _player = GetTree().GetFirstNodeInGroup("player") as PlayerController;
        if (_player != null)
        {
            _player.MovementEnabled = false;
        }

        Input.MouseMode = Input.MouseModeEnum.Visible;
    }

    public void HideNote()
    {
        if (_root != null)
        {
            _root.Visible = false;
        }

        ProcessMode = ProcessModeEnum.Disabled;
        RemoveFromGroup("note_reader_active");

        if (_player != null)
        {
            _player.MovementEnabled = true;
            _player = null;
        }

        if (GetTree() != null)
        {
            Input.MouseMode = Input.MouseModeEnum.Captured;
        }
    }

    public static NoteReaderUI? Find(Node from)
    {
        return from.GetTree().GetFirstNodeInGroup("note_reader") as NoteReaderUI;
    }
}
