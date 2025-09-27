using Godot;
using System;

public partial class Card : Button
{
	[Export] public Texture2D BackTexture { get; set; }
	[Export] public Texture2D FaceTexture { get; set; }
	[Export] public bool IsFaceUp { get; private set; } = false;
	
	public int _index;
	private TextureRect _image;
	
	[Signal] public delegate void CardClickedEventHandler(Card card);
	
	public override void _Ready()
	{
		FocusMode = FocusModeEnum.None;
		Text = "";
		CustomMinimumSize = new Vector2(64, 64);
		
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
		EmitSignal(SignalName.CardClicked, this);
	}
	
	public void FlipToFace()
	{
		IsFaceUp = true;
		UpdateVisual();
	}
	
	public void FlipToBack()
	{
		IsFaceUp = false;
		UpdateVisual();
	}
	
	private void UpdateVisual()
	{
		if (_image == null) return;
		_image.Texture = IsFaceUp ? (FaceTexture ?? BackTexture) : BackTexture;
	}
}
