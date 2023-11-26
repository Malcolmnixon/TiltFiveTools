using Godot;
using System;

public partial class T5XRRig : SubViewport
{
	T5OriginCS origin;
	T5CameraCS camera;
	T5ControllerCS[] wands = new T5ControllerCS[4];

	public string GlassesID { get; set; } 
	public T5Interface.GameboardType GameboardType { get; set; }
	public Aabb GameboardSize { get; set; }
	public T5OriginCS Origin {  get { return origin; } }
	public T5CameraCS Camera{ get { return camera; } }
	public T5ControllerCS Wand(int i) 
	{
		if(i >= 0 && i < wands.Length) { return wands[i]; }
		return null;
	}

	static string[] wandPaths = { "Origin/Wand_1", "Origin/Wand_2", "Origin/Wand_3", "Origin/Wand_4" };

	// Called when the node enters the scene tree for the first time.
	public override void _EnterTree()
	{
		base._EnterTree();

		origin = GetNode<T5OriginCS>("Origin");
		camera = GetNode<T5CameraCS>("Origin/Camera");
		for(int i = 0; i < wandPaths.Length; i++) 
		{
			wands[i] = GetNodeOrNull<T5ControllerCS>(wandPaths[i]);
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		foreach(var wand in wands)
		{
			if(wand != null)
			{ 
				wand.Visible = wand.getHasTrackingData();
			}
		}
	}
}
