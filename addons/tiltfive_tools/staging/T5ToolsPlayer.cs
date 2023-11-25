using Godot;

#nullable enable

/// <summary>
/// Tilt Five Tools Player Node
/// </summary>
/// <description>
/// This node is a T5XRRig with additional capabilities to work with the
/// Tilt Five Tools Staging, Scene, and Character features. Applications should
/// create a custom player scene extending from this scene/node to customize the
/// player - for example to adjust the visible layers or add pointers.
/// </description>
public partial class T5ToolsPlayer : T5XRRig
{
	// Visible layers
	private uint _visibleLayers = 5;
	
	// Player number
	private int _playerNumber = -1;

	/// <summary>
	/// Player visibility layers
	/// </summary>
	[Export(PropertyHint.Layers3DRender)]
	public uint VisibleLayers
	{
		get => _visibleLayers;
		set => SetVisibleLayers(value);
	}

	/// <summary>
	/// Player number [0..3] (set by Staging on load)
	/// </summary>
	[Export]
	public int PlayerNumber
	{
		get => _playerNumber; 
		set => SetPlayerNumber(value);
	}

	/// <summary>
	/// Called when the node enters the scene tree
	/// </summary>
    public override void _EnterTree()
    {
        base._EnterTree();
		_playerNumber = T5ToolsStaging.Instance?.NextFreePlayerNumber() ?? -1;
    }

    /// <summary>
    /// Called when the node enters the scene tree for the first time.
    /// </summary>
    public override void _Ready()
	{
		base._Ready();

		UpdateCameraCullMask();
	}

	/// <summary>
	/// Get the player viewport
	/// </summary>
	/// <returns>Player Viewport</returns>
	public SubViewport GetPlayerViewport()
	{
		return this;
	}

	/// <summary>
	/// Get the player origin
	/// </summary>
	/// <returns>Player origin</returns>
	public T5OriginCS? GetPlayerOrigin()
	{
		return GetNodeOrNull<T5OriginCS>("Origin");
	}

	/// <summary>
	/// Get the player camera
	/// </summary>
	/// <returns>Player camera</returns>
	public Camera3D? GetPlayerCamera()
	{
		return GetNodeOrNull<Camera3D>("Origin/Camera");
	}

	/// <summary>
	/// Get the player wand
	/// </summary>
	/// <param name="wandNum">Wand number [0..3]</param>
	/// <returns>Player wand</returns>
	public T5ControllerCS? GetPlayerWand(int wandNum)
	{
		return GetNodeOrNull<T5ControllerCS>($"Origin/Wand_{wandNum + 1}");
	}

	/// <summary>
	/// Get the visible layer mask associated with this player
	/// </summary>
	/// <returns>Player-specific visible layer</returns>
	public uint GetPlayerVisibleLayer()
	{
		// Handle invalid player number
		if (PlayerNumber < 0)
			return 0;

		// Return the unique player layer [11..14]
		return 1024U << PlayerNumber;
	}

	/// <summary>
	/// Get the physics layer mask associated with this player
	/// </summary>
	/// <returns>Player-specific physics layer</returns>
	public uint GetPlayerPhysicsLayer()
	{
		// Handle invalid player number
		if (PlayerNumber < 0)
			return 0;

		// Return the unique player layer [11..14]
		return 1024U << PlayerNumber;
	}

	/// <summary>
	/// Find the T5ToolsPlayer from a child node
	/// </summary>
	/// <param name="node">Node to search</param>
	/// <returns>Player instance or null</returns>
	public static T5ToolsPlayer? FindInstance(Node node)
	{
		// Search the ancestors for a player node
		while (node != null)
		{
			// If we have the player then return it
			if (node is T5ToolsPlayer player)
				return player;

			// Walk up to the parent
			node = node.GetParent();
		}

		// Could not find play
		return null;
	}

	/// <summary>
	/// Convert to string
	/// </summary>
	/// <returns>Player-specific strong</returns>
	public override string ToString()
	{
		return $"T5ToolsPlayer:<#{PlayerNumber}|#{GetInstanceId()}>";
	}

	/// <summary>
	/// Handle setting the player visible layers
	/// </summary>
	/// <param name="layers">Visible layers</param>
	private void SetVisibleLayers(uint layers)
	{
		_visibleLayers = layers;
		if (IsInsideTree())
			UpdateCameraCullMask();
	}

	/// <summary>
	/// Handle setting the player number
	/// </summary>
	/// <param name="playerNumber">Player number</param>
	private void SetPlayerNumber(int playerNumber)
	{
		_playerNumber = playerNumber;
		if (IsInsideTree())
			UpdateCameraCullMask();
	}

	/// <summary>
	/// Update the camera cull mask
	/// </summary>
	private void UpdateCameraCullMask()
	{
		// Set the camera cull mask to see the selected visible layers as well as
		// the layer specific to this player.
		var camera = GetPlayerCamera();
		if (camera != null)
			camera.CullMask = _visibleLayers | GetPlayerVisibleLayer();
	}
}
