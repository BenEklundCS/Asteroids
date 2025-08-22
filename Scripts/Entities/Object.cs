using Godot;
using static Godot.GD;
using System;

namespace Asteroids.Scripts.Entities {
    public partial class Object : Node2D {
        public Vector2 Velocity { get; protected set; }

        public override void _Process(double delta) {
            MoveAndWrap(delta);
        }

        private void MoveAndWrap(double delta) {
            Position = new Vector2(
                Mathf.PosMod(Position.X, GetViewportRect().Size.X), 
                Mathf.PosMod(Position.Y, GetViewportRect().Size.Y)
            ) + Velocity * (float)delta;
        }
    }
}
