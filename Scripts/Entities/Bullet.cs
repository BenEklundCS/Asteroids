using Godot;
using System;
using static Godot.GD;
using System.Collections;

namespace Asteroids.Scripts.Entities {
    public partial class Bullet : Object {
        [Signal]
        public delegate void OnHitEventHandler();

        private Area2D _hitBox;

        [Export] public float Speed = 1000.0f;

        public override void _Ready() {
            AddToGroup("Bullets");
            _hitBox = GetNode<Area2D>("Hitbox");
            _hitBox.AreaEntered += OnAreaEntered;

            Velocity = -Transform.Y.Normalized() * Speed;
        }

        public override void _Process(double delta) {
            Die();
            base._Process(delta);
        }

        private void Die() {
            if (
                (GlobalPosition.X < 0 || GlobalPosition.X > GetViewport().GetVisibleRect().Size.X)
                ||
                (GlobalPosition.Y < 0 || GlobalPosition.Y > GetViewport().GetVisibleRect().Size.Y)
            ) {
                QueueFree();
            }
        }

        private void OnAreaEntered(Area2D area) {
            if (area.IsInGroup("Asteroids")) {
                EmitSignalOnHit();
                QueueFree();
            }
        }
    }
}
