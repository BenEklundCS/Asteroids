using Godot;
using static Godot.GD;
using System;
using Asteroids.Scripts.Entities;
using Asteroids.Scripts.UI;

public partial class Root : Node2D {
    private UI _ui;
    private AudioStreamPlayer _mainTheme;
    private int _score;
    public override void _Ready() {
        _ui = GetNode<UI>("UI");
        _mainTheme = GetNode<AudioStreamPlayer>("MainTheme");
        _mainTheme.Finished += OnMainThemeFinished;
        _mainTheme.Play();
        GetTree().Root.ChildEnteredTree += OnChildEnteredTree;
    }

    private void OnMainThemeFinished() {
        _mainTheme.Play();
    }

    private void OnChildEnteredTree(Node node) {
        if (node.IsInGroup("Bullets")) {
            ((Bullet)node).OnHit += _ui.OnHit;
        }
    }
}
