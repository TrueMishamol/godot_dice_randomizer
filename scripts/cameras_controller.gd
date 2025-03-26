class_name CamerasController
extends Node3D

@export var cameras: Array[Camera3D]

var _current_camera_index: int = 0




func _ready():
	if cameras.size() == 0:
		printerr("Error: No cameras assigned in the 'cameras' array.")
		return

	# Make first camera current
	for i in range(1, cameras.size()):
		cameras[i].set_current(false)
	cameras[0].set_current(true)

#func _input(event):
	#if event.is_action_pressed("switch_camera"):
		#switch_to_next_camera()

func switch_to_next_camera():
	# Do nothing if there is only one camera
	if cameras.size() <= 1:
		return
	
	# Disable current camera
	cameras[_current_camera_index].set_current(false)
	# Get next camera index
	_current_camera_index = (_current_camera_index + 1) % cameras.size()
	# Enable next camera
	cameras[_current_camera_index].set_current(true)
