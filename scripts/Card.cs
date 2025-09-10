using Godot;
using System;

public partial class Card : Button
{
	[Export] public Texture2D BackTexture { get; set; }
	[Export] public Texture2D FaceTexture { get; set; }
	[Export] public bool IsFaceUp { get; private set; } = false;
	
	private TextureRect _image;
	
	public override void _Ready()
	{
		FocusMode = FocusModeEnum.None;
		Text = "";
		CustomMinimumSize = new Vector2(120, 180);
		
		EnsureImageNode();
		_image.MouseFilter = MouseFilterEnum.Ignore;
		
		Pressed += OnPressed;
		UpdateVisual();
	}
	
	private void EnsureImageNode()
	{
		if (HasNode("Image"))
		{
			_image = GetNode<TextureRect>("Image");
		}
		else
		{
			_image = new TextureRect
			{
				Name = "Image", 
				StretchMode = TextureRect.StretchModeEnum.KeepAspectCentered
			};
			AddChild(_image);
			_image.SetAnchorsPreset(LayoutPreset.FullRect);
		}
	}
	
	private void OnPressed()
	{
		IsFaceUp = !IsFaceUp;
		UpdateVisual();
	}
	
	private void UpdateVisual()
	{
		if (_image == null) return;
		_image.Texture = IsFaceUp ? (FaceTexture ?? BackTexture) : BackTexture;
	}
}
