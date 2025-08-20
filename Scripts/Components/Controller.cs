using Godot;
using System;
using Asteroids.Scripts;

public partial class Controller : Node2D {
    [Export] public string TargetPlayer = "Player";
    private Player _controlTarget;

    public override void _Ready() {
        _controlTarget = GetNode<Player>(TargetPlayer);
    }

    public override void _Process(double delta) {
        if (Input.IsActionPressed("boost")) {
            _controlTarget.Boost();
        }

        if (Input.IsActionPressed("stop")) {
            _controlTarget.Stop();
        }

        if (Input.IsActionPressed("right")) {
            _controlTarget.Right();
        }

        if (Input.IsActionPressed("left")) {
            _controlTarget.Left();
        }
    }
}
