extends Control



const D4 = preload("res://datapacks/default/dices/d4.tscn")
const D6 = preload("res://datapacks/default/dices/d6.tscn")
const D8 = preload("res://datapacks/default/dices/d8.tscn")
const D10 = preload("res://datapacks/default/dices/d10.tscn")
const D12 = preload("res://datapacks/default/dices/d12.tscn")
const D20 = preload("res://datapacks/default/dices/d20.tscn")

@export var _spawn_point: NodePath  # Export the spawn point node path
@export var _cameras_controller: CamerasController

@onready var _spin_box_d4: SpinBox = $MarginContainer/Control/VBoxContainer/D4/SpinBoxD4
@onready var _spin_box_d6: SpinBox = $MarginContainer/Control/VBoxContainer/D6/SpinBoxD6
@onready var _spin_box_d8: SpinBox = $MarginContainer/Control/VBoxContainer/D8/SpinBoxD8
@onready var _spin_box_d10: SpinBox = $MarginContainer/Control/VBoxContainer/D10/SpinBoxD10
@onready var _spin_box_d12: SpinBox = $MarginContainer/Control/VBoxContainer/D12/SpinBoxD12
@onready var _spin_box_d20: SpinBox = $MarginContainer/Control/VBoxContainer/D20/SpinBoxD20

@onready var _spawn_button: Button = %SpawnButton
@onready var _camera_button: Button = $MarginContainer/Control/CameraButton


#! Add new options to toggle on and off
#! Random inertia
#! Random rotation




func _ready() -> void:
	_spawn_button.pressed.connect(_on_spawn_button_pressed)
	_camera_button.pressed.connect(_on_camera_button_pressed)

func _on_spawn_button_pressed():
	kill_dices()

	var spawn_point = get_node(_spawn_point)  # Get the spawn point node

	# Spawn dice based on SpinBox values
	for i in range(_spin_box_d4.value):
		_spawn_dice(D4, spawn_point)

	for i in range(_spin_box_d6.value):
		_spawn_dice(D6, spawn_point)

	for i in range(_spin_box_d8.value):
		_spawn_dice(D8, spawn_point)

	for i in range(_spin_box_d10.value):
		_spawn_dice(D10, spawn_point)

	for i in range(_spin_box_d12.value):
		_spawn_dice(D12, spawn_point)

	for i in range(_spin_box_d20.value):
		_spawn_dice(D20, spawn_point)


func _spawn_dice(dice_scene: PackedScene, spawn_point: Node):
	var dice_instance: RigidBody3D = dice_scene.instantiate()  # Instantiate the dice
	#! If not RigidBody3D - return with error message
	add_child(dice_instance)  # Add to the scene
	dice_instance.global_position = spawn_point.global_position  # Set position to spawn point

	# Random start rotation
	dice_instance.rotation = Vector3(
		randf_range(0, 2 * PI),  # Random rotation around X-axis
		randf_range(0, 2 * PI),  # Random rotation around Y-axis
		randf_range(0, 2 * PI)   # Random rotation around Z-axis
	)

	# Random rotation inertia (angular velocity)
	dice_instance.angular_velocity = Vector3(
		randf_range(-10, 10),  # Random angular velocity on X-axis
		randf_range(-10, 10),  # Random angular velocity on Y-axis
		randf_range(-10, 10)   # Random angular velocity on Z-axis
	)


#! Make Global and Static
func kill_dices():
	var dices = get_tree().get_nodes_in_group("dices")
	for dice in dices:
		if is_instance_valid(dice):  # Check if the node is still valid
			# Option 1: If the dice node has a 'kill' function
			if dice.has_method("kill"):
				dice.kill()
			# Option 2: If you just want to remove the node from the scene
			else:
				dice.queue_free()  # Frees the node at the end of the frame


func _on_camera_button_pressed():
	if _cameras_controller == null:
		printerr("spawn_menu: _cameras_controller == null")
		return

	_cameras_controller.switch_to_next_camera()
