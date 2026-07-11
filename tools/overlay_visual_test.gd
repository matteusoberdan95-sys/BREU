extends SceneTree

const SCENE := "res://scenes/levels/trail_intro/TrailIntro.tscn"
const OUT_PATH := "user://overlay_debug_capture.png"
const REPORT_PATH := "user://overlay_debug_report.txt"


func _init() -> void:
	var runner := Node.new()
	runner.set_script(load("res://tools/overlay_visual_test_runner.gd"))
	root.add_child(runner)
