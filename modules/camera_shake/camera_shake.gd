extends Node


@export var camera: Camera3D

## How quickly to move through the noise
@export var noise_shake_speed: float = 4.0

## Noise returns values in the range (-1, 1)
## So this is how much to multiply the returned value by
@export var noise_shake_strength: float = 1.0

## The starting range of possible offsets using random values
## Multiplier for lerping the shake strength to zero
@export var shake_decay_rate: float = 0.0



@onready var noise = FastNoiseLite.new()

## Used to keep track of where we are in the noise
## so that we can smoothly move through it
var noise_i: float = 0.0
@onready var rand = RandomNumberGenerator.new()

var shake_strength: float = 0.0


func _ready() -> void:
	if !camera:
		camera = get_parent()
	
	rand.randomize()
	
	## Randomize the generated noise
	noise.seed = rand.randi()
	
	## Period affects how quickly the noise changes values
	#noise.period = 2
	
	apply_noise_shake()
	
func apply_noise_shake() -> void:
	shake_strength = noise_shake_strength
	
func _process(delta: float) -> void:
	## Fade out the intensity over time
	shake_strength = lerp(shake_strength, 0.0, shake_decay_rate * delta)
	
	var shake_offset: Vector2
	
	shake_offset = get_noise_offset(delta, noise_shake_speed, shake_strength)
	
	## Shake by adjusting camera.offset so we can move the camera around the level via it's position
	## Camera2D
	#camera.offset = shake_offset
	## Camera3D
	camera.h_offset = shake_offset.x
	camera.v_offset = shake_offset.y

func get_noise_offset(delta: float, speed: float, strength: float) -> Vector2:
	noise_i += delta * speed
	
	## Set the x values of each call to 'get_noise_2d' to a different value
	## so that our x and y vectors will be reading from unrelated areas of noise
	return Vector2(
		noise.get_noise_2d(1, noise_i) * strength,
		noise.get_noise_2d(100, noise_i) * strength
	)

func get_random_offset() -> Vector2:
	return Vector2(
		rand.randf_range(-shake_strength, shake_strength),
		rand.randf_range(-shake_strength, shake_strength)
	)
