using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class Main : Control
{
	[Export] public PackedScene CardScene { get; set; }
	[Export] public int Rows { get; set; } = 4;
	[Export] public int Columns { get; set; } = 6;
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
		
		// Create lottery box
		List<int> numbers = new List<int>();
		for (int i = 0; i < total / 2; i++)
		{
			numbers.Add(i);
			numbers.Add(i);
		}
		List<int> shuffled = numbers.OrderBy(x => Guid.NewGuid()).ToList();
		
		for (int i = 0; i < total; i++)
		{
			var card = CardScene.Instantiate<Card>();
			card.BackTexture = BackTexture;
			
			if (FaceTextures.Length > 0)
			{
				int index = shuffled[i];
				card.FaceTexture = FaceTextures[index];
				card._index = index;
			}
			
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
			
			if (AreCardsMatching(_faceUpCards[0], _faceUpCards[1]))
			{
				GD.Print("Match!");
			}
			else
			{
				GD.Print("Not match...");
				
				await ToSignal(GetTree().CreateTimer(FlipBackDelay), SceneTreeTimer.SignalName.Timeout);
			
				foreach(var card in _faceUpCards)
				{
					card.FlipToBack();
				}
			}
			
			_faceUpCards.Clear();
			_isProcessing = false;
		}
	}
	
	private bool AreCardsMatching(Card card1, Card card2)
	{
		int half_total = Rows * Columns / 2;
		if (card1._index % half_total == card2._index % half_total) return true;
		else return false;
	}
	
}
