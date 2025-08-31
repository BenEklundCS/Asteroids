using Godot;
using static Godot.GD;
using System;

namespace Asteroids.Scripts.UI {
    public partial class UI : Control {
        private int _score;
        private Label _label;
        
        [Export] public string ScoreFormat = "N0";
        [Export] public int ScoreMultiplier = 100;

        public override void _Ready() {
            _label = GetNode<Label>("Label");
        }

        public void OnHit() {
            _score += ScoreMultiplier;
            _label.Text = _score.ToString(ScoreFormat);
        }
    }
}
