using Godot;
using static Godot.GD;
using System;
using Asteroids.Scripts;
using Asteroids.Scripts.Entities;
using Asteroids.Scripts.Interfaces;

namespace Asteroids.Scripts.Components {
    public partial class Controller : Node2D {
        [Export] public string Target = "Player";
        
        private IControllable _controlTarget;

        public override void _Ready() {
            _controlTarget = GetNode<IControllable>(Target);
        }

        public override void _Process(double delta) {
            if (Input.IsActionPressed("boost")) {
                _controlTarget.Boost();
            }

            if (Input.IsActionPressed("slow")) {
                _controlTarget.Slow();
            }

            if (Input.IsActionPressed("right")) {
                _controlTarget.Right();
            }

            if (Input.IsActionPressed("left")) {
                _controlTarget.Left();
            }

            if (Input.IsActionJustPressed("shoot")) {
                _controlTarget.Shoot();
            }

            if (Input.IsActionPressed("quit")) {
                Callable.From(() => { GetTree().Quit(); }).CallDeferred();
            }
        }
    }   
}
