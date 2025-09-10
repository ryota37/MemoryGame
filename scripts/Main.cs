using Godot;
using System;

public partial class Main : Control
{
	[Export] public PackedScene CardScene { get; set; }
	[Export] public int Rows { get; set; } = 4;
	[Export] public int Columns { get; set; } = 4;
	
	[Export] public Texture2D BackTexture { get; set; }
	
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
			_grid.AddChild(card);			
		}
		
	}
}
