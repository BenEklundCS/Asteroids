using Godot;
using static Godot.GD;
using System;
using System.Linq;
using System.Runtime.InteropServices.JavaScript;

namespace Asteroids.Scripts.UI {
    public partial class Ui : Control {
        private int _score;
        public int HighScore { get; private set; }
        public int Health { get; set; }

        private String _heart = "❤️";
        
        private Label _scoreLabel;
        private Label _healthLabel;
        
        [Export] public string ScoreFormat = "N0";
        [Export] public int ScoreMultiplier = 100;

        public override void _Ready() {
            _scoreLabel = GetNode<Label>("Score");
            _healthLabel = GetNode<Label>("Health");
        }

        public void Init(GameData gameData) {
            HighScore = gameData.HighScore;
            Health = gameData.Health;
            SetHealthLabelText();
        }

        public void OnBulletHit() {
            _score += ScoreMultiplier;
            HighScore = Math.Max(_score, HighScore);
            _scoreLabel.Text = _score.ToString(ScoreFormat);
        }

        public void OnPlayerHit(int health) {
            Health = health;
            SetHealthLabelText();
        }

        private void SetHealthLabelText() {
            _healthLabel.Text = String.Concat(Enumerable.Repeat(_heart, Health));
        }
    }
}
