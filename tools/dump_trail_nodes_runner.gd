extends Node

func _ready() -> void:
	call_deferred("_run")


func _run() -> void:
	var scene := load("res://scenes/levels/trail_intro/TrailIntro.tscn").instantiate()
	_print_tree(scene, "")
	get_tree().quit(0)


func _print_tree(node: Node, indent: String) -> void:
	var extra := ""
	if node is MeshInstance3D:
		var mi := node as MeshInstance3D
		extra = " [MeshInstance3D mesh=%s]" % [mi.mesh]
	print("%s%s%s" % [indent, node.name, extra])
	for child in node.get_children():
		_print_tree(child, indent + "  ")
