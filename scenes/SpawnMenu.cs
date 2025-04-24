using Godot;
using System;

public partial class SpawnMenu : Control {

	// Preload dice scenes
	private static readonly PackedScene D4 = GD.Load<PackedScene>("res://datapacks/default/dices/d4.tscn");
	private static readonly PackedScene D6 = GD.Load<PackedScene>("res://datapacks/default/dices/d6.tscn");
	private static readonly PackedScene D8 = GD.Load<PackedScene>("res://datapacks/default/dices/d8.tscn");
	private static readonly PackedScene D10 = GD.Load<PackedScene>("res://datapacks/default/dices/d10.tscn");
	private static readonly PackedScene D12 = GD.Load<PackedScene>("res://datapacks/default/dices/d12.tscn");
	private static readonly PackedScene D20 = GD.Load<PackedScene>("res://datapacks/default/dices/d20.tscn");

	[Export] private NodePath _SpawnPoint;  // Export the spawn point node path

	[Export] private CamerasController _CamerasController;

	private SpinBox _spinBoxD4;
	private SpinBox _spinBoxD6;
	private SpinBox _spinBoxD8;
	private SpinBox _spin_box_d10;
	private SpinBox _spinBoxD12;
	private SpinBox _spinBoxD20;

	private Button _spawnButton;
	private Button _cameraButton;

	public override void _Ready() {
		_spinBoxD4 = GetNode<SpinBox>("MarginContainer/Control/VBoxContainer/D4/SpinBoxD4");
		_spinBoxD6 = GetNode<SpinBox>("MarginContainer/Control/VBoxContainer/D6/SpinBoxD6");
		_spinBoxD8 = GetNode<SpinBox>("MarginContainer/Control/VBoxContainer/D8/SpinBoxD8");
		_spin_box_d10 = GetNode<SpinBox>("MarginContainer/Control/VBoxContainer/D10/SpinBoxD10");
		_spinBoxD12 = GetNode<SpinBox>("MarginContainer/Control/VBoxContainer/D12/SpinBoxD12");
		_spinBoxD20 = GetNode<SpinBox>("MarginContainer/Control/VBoxContainer/D20/SpinBoxD20");

		_spawnButton = GetNode<Button>("%SpawnButton");
		_cameraButton = GetNode<Button>("MarginContainer/Control/CameraButton");

		_spawnButton.Pressed += SpawnButton_OnPressed;
		_cameraButton.Pressed += CameraButton_OnPressed;
	}

	private void SpawnButton_OnPressed() {
		KillDices();

		var spawnPoint = GetNode<Node3D>(_SpawnPoint);  // Get the spawn point node

		// Spawn dice based on SpinBox values
		for (int i = 0; i < (int)_spinBoxD4.Value; i++)
			SpawnDice(D4, spawnPoint);

		for (int i = 0; i < (int)_spinBoxD6.Value; i++)
			SpawnDice(D6, spawnPoint);

		for (int i = 0; i < (int)_spinBoxD8.Value; i++)
			SpawnDice(D8, spawnPoint);

		for (int i = 0; i < (int)_spin_box_d10.Value; i++)
			SpawnDice(D10, spawnPoint);

		for (int i = 0; i < (int)_spinBoxD12.Value; i++)
			SpawnDice(D12, spawnPoint);

		for (int i = 0; i < (int)_spinBoxD20.Value; i++)
			SpawnDice(D20, spawnPoint);
	}

	private void SpawnDice(PackedScene diceScene, Node3D spawnPoint) {
		var diceInstance = diceScene.Instantiate() as RigidBody3D;  // Instantiate the dice
		if (diceInstance == null) {
			GD.PrintErr("Spawned instance is not a RigidBody3D");
			return;
		}

		AddChild(diceInstance);  // Add to the scene
		diceInstance.GlobalPosition = spawnPoint.GlobalPosition;  // Set position to spawn point

		// Random start rotation
		diceInstance.Rotation = new Vector3(
			(float)GD.RandRange(0, 2 * Math.PI),  // Random rotation around X-axis
			(float)GD.RandRange(0, 2 * Math.PI),  // Random rotation around Y-axis
			(float)GD.RandRange(0, 2 * Math.PI)   // Random rotation around Z-axis
		);

		// Random rotation inertia (angular velocity)
		diceInstance.AngularVelocity = new Vector3(
			(float)GD.RandRange(-10, 10),  // Random angular velocity on X-axis
			(float)GD.RandRange(-10, 10),  // Random angular velocity on Y-axis
			(float)GD.RandRange(-10, 10)   // Random angular velocity on Z-axis
		);
	}

	private void KillDices() {
		var dices = GetTree().GetNodesInGroup("dices");
		foreach (Node dice in dices) {
			if (IsInstanceValid(dice))  // Check if the node is still valid
			{
				// Option 1: If the dice node has a 'kill' function
				if (dice.HasMethod("kill"))
					dice.Call("kill");
				// Option 2: If you just want to remove the node from the scene
				else
					dice.QueueFree();  // Frees the node at the end of the frame
			}
		}
	}

	private void CameraButton_OnPressed() {
		if (_CamerasController == null) {
			GD.PrintErr("spawn_menu: _cameras_controller == null");
			return;
		}

		_CamerasController.SwitchToNextCamera();
	}
}
