using Godot;
using System;

public partial class T5XRRig : SubViewport
{
	Node3D origin;
	Camera3D camera;
	Node3D[] wands = new Node3D[4];

	public string GlassesID { get; set; } 
	public T5Interface.GameboardType GameboardType { get; set; }
	public Aabb GameboardSize { get; set; }
	public Node3D Origin {  get { return origin; } }
	public Node3D Camera { get { return camera; } }
	public Node3D Wand(int i) 
	{
		if(i >= 0 && i < wands.Length) { return wands[i]; }
		return null;
	}

	static string[] wandPaths = { "Origin/Wand_1", "Origin/Wand_2", "Origin/Wand_3", "Origin/Wand_4" };

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		base._Ready();

		origin = GetNode<Node3D>("Origin");
		camera = GetNode<Camera3D>("Origin/Camera");
		for(int i = 0; i < wandPaths.Length; i++) 
		{
			wands[i] = GetNodeOrNull<Node3D>(wandPaths[i]);
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		foreach(var wand in wands)
		{
			if(wand != null)
			{ 
				var wandPose = wand.Call("get_pose").As<XRPose>();

				if (wandPose != null)
				{
					wand.Visible = wandPose.TrackingConfidence != XRPose.TrackingConfidenceEnum.None;
				}
				else
				{
					wand.Visible = false;
				}
			}
		}
	}
}
