using Godot;
using System;

public partial class CameraShake : Node {

	// ############# //
	// # Variables # //
	// ############# //

	//# Export

	[Export] public Camera3D _Camera;

	// How quickly to move through the noise
	[Export] public float _NoiseShakeSpeed = 4.0f;

	// Noise returns values in the range (-1, 1)
	// So this is how much to multiply the returned value by
	[Export] public float _NoiseShakeStrength = 1.0f;

	// The starting range of possible offsets using random values
	// Multiplier for lerping the shake strength to zero
	[Export] public float _ShakeDecayRate = 0.0f;

	//# Regular

	private FastNoiseLite _noise = new FastNoiseLite();

	// Used to keep track of where we are in the noise so that we can smoothly move through it
	private float _noiseI = 0.0f;

	private float _shakeStrength = 0.0f;

	// #################### //
	// # Built-in Methods # //
	// #################### //

	public override void _Ready() {

		//# Get _Camera if null
		if (_Camera == null) {
			var parent = GetParent();
			if (parent is Camera3D camera) {
				_Camera = camera;
			} else {
				GD.PushError("_Camera == null & Parent is not a Camera3D node!");
			}
		}

		//# Randomize the generated noise
		_noise.Seed = (int)GD.Randi();

		//# Period affects how quickly the noise changes values
		// noise.Period = 2;

		ApplyNoiseShake();
	}

	public void ApplyNoiseShake() {
		_shakeStrength = _NoiseShakeStrength;
	}

	public override void _Process(double delta) {
		//# Fade out the intensity over time
		_shakeStrength = Mathf.Lerp(_shakeStrength, 0.0f, _ShakeDecayRate * (float)delta);

		Vector2 shake_offset = GetNoiseOffset((float)delta, _NoiseShakeSpeed, _shakeStrength);

		//# Shake by adjusting camera.offset so we can move the camera around the level via it's position

		// Camera2D
		// camera.offset = shake_offset

		// Camera3D
		_Camera.HOffset = shake_offset.X;
		_Camera.VOffset = shake_offset.Y;
	}

	// ################## //
	// # Custom Methods # //
	// ################## //

	private Vector2 GetNoiseOffset(float delta, float speed, float strength) {
		_noiseI += delta * speed;

		//# Set the x values of each call to 'get_noise_2d' to a different value so that our x and y vectors will be reading from unrelated areas of noise
		return new Vector2(
			_noise.GetNoise2D(1, _noiseI) * strength,
			_noise.GetNoise2D(100, _noiseI) * strength
		);
	}
}
