using Godot;
using static Godot.GD;
using System;

namespace Asteroids.Scripts.Entities {
    public partial class Object : Node2D {
        public Vector2 Velocity { get; protected set; }
        public bool Wrappable { get; protected set; }

        public override void _Process(double delta) {
            MoveAndWrap(delta);
        }

        private void MoveAndWrap(double delta) {
            Position += Velocity * (float)delta;
            if (Wrappable) {
                var size = GetViewportRect().Size;
                Position = new Vector2(
                    Mathf.PosMod(Position.X, size.X),
                    Mathf.PosMod(Position.Y, size.Y)
                );
            }
        }
    }
}
