using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class Main : Control
{
	[Export] public PackedScene CardScene { get; set; }
	[Export] public int Rows { get; set; } = 4;
	[Export] public int Columns { get; set; } = 4;
	[Export] public float FlipBackDelay { get; set; } = 1.0f;
	
	[Export] public Texture2D BackTexture { get; set; }
	[Export] public Texture2D[] FaceTextures { get; set; } = System.Array.Empty<Texture2D>();
	
	private GridContainer _grid;
	private List<Card> _faceUpCards = new List<Card>();
	private bool _isProcessing = false;
	
	public override void _Ready()
	{
		_grid = GetNode<GridContainer>("MarginContainer/Grid");
		_grid.Columns = Columns;
		
		int total = Rows * Columns;
		
		for (int i = 0; i < total; i++)
		{
			var card = CardScene.Instantiate<Card>();
			card.BackTexture = BackTexture;
			
			if (FaceTextures.Length > 0)
				card.FaceTexture = FaceTextures[i%FaceTextures.Length];
			
			card.CardClicked += OnCardClicked;
			_grid.AddChild(card);			
		}
	}
		
	private async void OnCardClicked(Card clickedCard)
	{
		if(_isProcessing || clickedCard.IsFaceUp)
			return;
		
		clickedCard.FlipToFace();
		_faceUpCards.Add(clickedCard);
		
		if(_faceUpCards.Count == 2)
		{
			_isProcessing = true;
			
			await ToSignal(GetTree().CreateTimer(FlipBackDelay), SceneTreeTimer.SignalName.Timeout);
			
			foreach(var card in _faceUpCards)
			{
				card.FlipToBack();
			}
			
			_faceUpCards.Clear();
			_isProcessing = false;
		}
	}
}
