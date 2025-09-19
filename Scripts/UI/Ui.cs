using Godot;
using static Godot.GD;
using System;
using System.Linq;
using Object = Asteroids.Scripts.Entities.Object;

namespace Asteroids.Scripts.UI {
    public partial class Ui : Control {
        private int _score;
        public int HighScore { get; private set; }
        private int _health;

        private string _heart = "❤️";
        
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
            _health = gameData.Health;
            SetHealthLabelText();
        }

        public void OnBulletHitAsteroid(Object obj) {
            UpdateScore(obj.Value);
        }
        
        public void OnInvaderDeath(Object obj) {
            UpdateScore(obj.Value);
        }

        public void OnPlayerHit(int health) {
            _health = health;
            SetHealthLabelText();
        }

        private void UpdateScore(int increment = 0) {
            _score += ScoreMultiplier * increment;
            HighScore = Math.Max(_score, HighScore);
            _scoreLabel.Text = _score.ToString(ScoreFormat);
        }

        private void SetHealthLabelText() {
            _healthLabel.Text = string.Concat(Enumerable.Repeat(_heart, _health));
        }
    }
}
