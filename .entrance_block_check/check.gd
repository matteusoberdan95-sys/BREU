extends Node

func _ready() -> void:
	var level = load("res://scenes/levels/pensao_santa_luzia/PensaoVerticalBlockout01.tscn").instantiate()
	add_child(level)
	await get_tree().process_frame
	var player: CharacterBody3D = level.get_node("Player")
	player.set_physics_process(false)
	player.global_position = Vector3(0.0, 0.05, 0.0)
	player.rotation = Vector3.ZERO
	var wall: Node3D = level.get_node("PensionSecondFloor/UpperBalcony_Placeholder/UpperBalcony_BackWall")
	print("UPPER_BALCONY_BACK_WALL_GLOBAL=", wall.global_position)
