using Godot;
using System;

public partial class Main : Control
{
	[Export] public PackedScene CardScene { get; set; }
	
	private GridContainer _grid;
	
	public override void _Ready()
	{
		_grid = GetNode<GridContainer>("MarginContainer/Grid");
		_grid.Columns = 3;
		
		int count = 6;
		for (int i = 0; i < count; i++)
		{
		var card = CardScene.Instantiate<Node>();
		_grid.AddChild(card);			
		}
		
	}
}
