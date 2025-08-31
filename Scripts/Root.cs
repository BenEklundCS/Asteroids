using Godot;
using static Godot.GD;
using System;
using Asteroids.Scripts.Entities;

using Asteroids.Scripts.UI;

namespace Asteroids.Scripts {
    public partial class Root : Node2D {
        private Ui _ui;
        private GameSaver _gameSaver;
        private AudioStreamPlayer _mainTheme;
        private Player _player;
        private int _score;
        
        public override void _Ready() {
            _player = GetNode<Player>("Controller/Player");
            _player.OnDeath += OnPlayerDeath;
            
            _gameSaver = GetNode<GameSaver>("GameSaver");
            var gameData = _gameSaver.Load();
            
            _ui = GetNode<Ui>("UI");
            gameData.Health = _player.MaxHealth;
            _ui.Init(gameData);
            
            _player.OnHit += _ui.OnPlayerHit;
            
            _mainTheme = GetNode<AudioStreamPlayer>("MainTheme");
            _mainTheme.Finished += OnMainThemeFinished;
            _mainTheme.Play();
            GetTree().Root.ChildEnteredTree += OnChildEnteredTree;
        }

        private void OnMainThemeFinished() {
            _mainTheme.Play();
        }

        private void OnPlayerDeath() {
            var gameData = new GameData();
            gameData.HighScore = _ui.HighScore;
            _gameSaver.Save(gameData);
            Callable.From(() => {
                GetTree().ChangeSceneToFile("res://Scenes/UI/title.tscn");
            }).CallDeferred();
        }

        private void OnChildEnteredTree(Node node) {
            if (node.IsInGroup("Bullets")) {
                ((Bullet)node).OnHit += _ui.OnBulletHit;
            }
        }
    }
}
