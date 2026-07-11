extends SceneTree

func _init() -> void:
	var runner := Node.new()
	runner.set_script(load("res://tools/dump_trail_nodes_runner.gd"))
	root.add_child(runner)
