using Godot;
using Godot.Collections;

public partial class CamerasController : Node3D {

	[Export] public Array<Camera3D> _Cameras = new Array<Camera3D>();

	private int _currentCameraIndex = 0;




	public override void _Ready() {
		if (_Cameras.Count == 0) {
			GD.PrintErr("Error: No cameras assigned in the 'Cameras' list.");
			return;
		}

		// Make first camera current and disable others
		for (int i = 1; i < _Cameras.Count; i++) {
			_Cameras[i].Current = false;
		}
		_Cameras[0].Current = true;
	}

	// public override void _Input(InputEvent @event) {
	// 	if (@event.IsActionPressed("switch_camera")) {
	// 		SwitchToNextCamera();
	// 	}
	// }

	public void SwitchToNextCamera() {
		// Do nothing if there is only one camera
		if (_Cameras.Count <= 1)
			return;

		// Disable current camera
		// _Cameras[_currentCameraIndex].Current = false;
		// Get next camera index
		_currentCameraIndex = (_currentCameraIndex + 1) % _Cameras.Count;
		// Enable next camera
		_Cameras[_currentCameraIndex].Current = true;
	}
}
