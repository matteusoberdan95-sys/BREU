extends SceneTree

func _init() -> void:
	var runner := Node.new()
	runner.set_script(load("res://tools/depth_fog_visual_test_runner.gd"))
	root.add_child(runner)
