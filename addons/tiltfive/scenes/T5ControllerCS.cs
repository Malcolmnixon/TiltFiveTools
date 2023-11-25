using Godot;
using System;

public partial class T5ControllerCS : Node3D
{
	public bool IsButtonPressed(StringName name)
	{
		return Call("is_button_pressed", name).AsBool();
	}

	public Variant GetInput(StringName name)
	{
		return Call("get_input", name);
	}

	public float GetFloat(StringName name)
	{
		return Call("get_float", name).As<float>();
	}

	public Vector2 GetVector2(StringName name)
	{
		return Call("get_vector2", name).As<Vector2>();
	}
}
