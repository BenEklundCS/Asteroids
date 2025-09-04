using Godot;
using System;
using System.Threading.Tasks;

namespace Asteroids.Scripts.UI {
    public partial class Title : Control {
        private static bool _opened = false;
        
        private Button _start;
        private Button _quit;
        private Control _scoreContainer;
        private Label _highScore;
        private GameSaver _gameSaver;
        private AudioStreamPlayer _titleTheme;
        
        [Export] public string ScoreFormat = "N0";

        public override void _Ready() {
            _start = GetNode<Button>("ButtonContainer/Start");
            _quit = GetNode<Button>("ButtonContainer/Quit");

            _start.Pressed += OnStartPressed;
            _quit.Pressed += OnQuitPressed;

            _titleTheme = GetNode<AudioStreamPlayer>("TitleTheme");
            _titleTheme.Finished += OnTitleThemeFinished;
            _titleTheme.Play();

            _scoreContainer = GetNode<Control>("ScoreContainer");
            _highScore = GetNode<Label>("ScoreContainer/HighScoreValue");
            _gameSaver = GetNode<GameSaver>("GameSaver");
            
            if (_opened) {
                _start.Text = "RESTART";
                var gameData = _gameSaver.Load();
                _highScore.Text = gameData.HighScore.ToString(ScoreFormat);
                _scoreContainer.Visible = true;
            }
            
            _opened = true;
        }

        private void OnTitleThemeFinished() {
            _titleTheme.Play();
        }

        private void OnStartPressed() {
            GetTree().ChangeSceneToFile("res://Scenes/root.tscn");
        }

        private void OnSettingsPressed() {
            
        }

        private void OnQuitPressed() {
            Callable.From(() => { GetTree().Quit(); }).CallDeferred();
        }
    }
}
