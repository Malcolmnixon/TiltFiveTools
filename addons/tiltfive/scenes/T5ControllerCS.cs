using Godot;
using System;

[Tool]
public partial class T5ControllerCS : T5NodeCS
{
	public delegate void ButtonEvent(T5ControllerCS self, StringName name);
	public delegate void FloatEvent(T5ControllerCS self, StringName name, float value);
	public delegate void Vector2Event(T5ControllerCS self, StringName name, Vector2 value);

	public event ButtonEvent ButtonPressed;
	public event ButtonEvent ButtonReleased;
	public event FloatEvent FloatChanged;
	public event Vector2Event Vector2Changed;

	public override void _EnterTree()
	{
		base._EnterTree();

		Connect("button_pressed", Callable.From<StringName>(OnButtonPressed));
		Connect("button_released", Callable.From<StringName>(OnButtonReleased));
		Connect("input_float_changed", Callable.From<StringName, float>(OnFloatChanged));
		Connect("input_vector2_changed", Callable.From<StringName, Vector2>(OnVector2Changed));
	}
	
	public override void _ExitTree() {
		
		Disconnect("button_pressed", Callable.From<StringName>(OnButtonPressed));
		Disconnect("button_released", Callable.From<StringName>(OnButtonReleased));
		Disconnect("input_float_changed", Callable.From<StringName, float>(OnFloatChanged));
		Disconnect("input_vector2_changed", Callable.From<StringName, Vector2>(OnVector2Changed));
		
		base._ExitTree();
	}
	
	public void TriggerHapticPulse(float amplitude, int duration) 
	{
		Call("trigger_haptic_pulse", amplitude, duration);
	}

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

	protected void OnButtonPressed(StringName name)
	{
		ButtonPressed?.Invoke(this, name);
	}

	protected void OnButtonReleased(StringName name)
	{
		ButtonReleased?.Invoke(this, name);
	}

	protected void OnFloatChanged(StringName name, float value)
	{
		FloatChanged?.Invoke(this, name, value);
	}

	protected void OnVector2Changed(StringName name, Vector2 value)
	{
		Vector2Changed?.Invoke(this, name, value);
	}
}
