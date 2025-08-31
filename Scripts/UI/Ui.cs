using Godot;
using static Godot.GD;
using System;

namespace Asteroids.Scripts.UI {
    public partial class Ui : Control {
        private int _score;
        public int HighScore { get; private set; }
        public int Health { get; set; }
        
        private Label _scoreLabel;
        private Label _highScoreLabel;
        private Label _healthLabel;
        
        [Export] public string ScoreFormat = "N0";
        [Export] public int ScoreMultiplier = 100;

        public override void _Ready() {
            _scoreLabel = GetNode<Label>("Score");
            _highScoreLabel = GetNode<Label>("HighScore");
            _healthLabel = GetNode<Label>("Health");
        }

        public void Init(GameData gameData) {
            HighScore = gameData.HighScore;
            Health = gameData.Health;
            _highScoreLabel.Text = HighScore.ToString(ScoreFormat);
            _healthLabel.Text = Health.ToString();
        }

        public void OnBulletHit() {
            _score += ScoreMultiplier;
            HighScore = Math.Max(_score, HighScore);
            _scoreLabel.Text = _score.ToString(ScoreFormat);
            _highScoreLabel.Text = HighScore.ToString(ScoreFormat);
        }

        public void OnPlayerHit(int health) {
            Health = health;
            _healthLabel.Text = Health.ToString();
        }
    }
}
