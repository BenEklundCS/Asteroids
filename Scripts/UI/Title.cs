using Godot;
using System;

namespace Asteroids.Scripts.UI {
    public partial class Title : Control {
        private Button _start;
        private Button _settings;
        private Button _quit;
        private AudioStreamPlayer _titleTheme;

        public override void _Ready() {
            _start = GetNode<Button>("ButtonContainer/Start");
            //_settings = GetNode<Button>("ButtonContainer/Settings");
            _quit = GetNode<Button>("ButtonContainer/Quit");

            _start.Pressed += OnStartPressed;
            //_settings.Pressed += OnSettingsPressed;
            _quit.Pressed += OnQuitPressed;

            _titleTheme = GetNode<AudioStreamPlayer>("TitleTheme");
            _titleTheme.Finished += OnTitleThemeFinished;
            _titleTheme.Play();
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
            GetTree().Quit();
        }
    }
}
