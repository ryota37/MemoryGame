using Godot;
using System;

public partial class Main : Control
{
	[Export] public PackedScene CardScene { get; set; }
	[Export] public int Rows { get; set; } = 4;
	[Export] public int Columns { get; set; } = 4;
	
	[Export] public Texture2D BackTexture { get; set; }
	[Export] public Texture2D[] FaceTextures { get; set; } = System.Array.Empty<Texture2D>();
	
	private GridContainer _grid;
	
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
			
			_grid.AddChild(card);			
		}
		
	}
}
