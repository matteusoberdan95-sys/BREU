extends Node

const SCENE := "res://scenes/levels/trail_intro/TrailIntro.tscn"
const OUT_PATH := "user://overlay_debug_capture.png"
const REPORT_PATH := "user://overlay_debug_report.txt"


func _ready() -> void:
	call_deferred("_run")


func _run() -> void:
	var scene: Node = load(SCENE).instantiate()
	get_tree().root.add_child(scene)

	for _i in 8:
		await get_tree().process_frame

	var overlay: ColorRect = scene.get_node("AtmosphereOverlay/ScreenFogOverlay") as ColorRect
	var material: ShaderMaterial = overlay.material as ShaderMaterial
	var debug_on: bool = bool(material.get_shader_parameter("debug_visible"))

	var viewport: Viewport = get_tree().root
	var with_overlay := _sample_viewport(viewport)
	overlay.visible = false
	for _i in 3:
		await get_tree().process_frame
	var without_overlay := _sample_viewport(viewport)
	overlay.visible = true

	var delta := Vector3(
		with_overlay.x - without_overlay.x,
		with_overlay.y - without_overlay.y,
		with_overlay.z - without_overlay.z
	)
	var delta_len := delta.length()

	var image: Image = viewport.get_texture().get_image()
	image.save_png(OUT_PATH)

	var avg_r := with_overlay.x
	var avg_g := with_overlay.y
	var avg_b := with_overlay.z
	var blue_leads := avg_b > avg_r + 0.08 and avg_b > avg_g + 0.05
	var fog_present := (not debug_on) and delta_len > 0.008
	var visible := (debug_on and blue_leads) or fog_present

	var report := ""
	report += "debug_visible_param=%s\n" % str(debug_on)
	report += "overlay_visible=%s\n" % str(overlay.visible)
	report += "overlay_modulate=%s\n" % str(overlay.modulate)
	report += "canvas_layer=%s\n" % str(scene.get_node("AtmosphereOverlay").layer)
	report += "delta_rgb=%.4f,%.4f,%.4f\n" % [delta.x, delta.y, delta.z]
	report += "delta_len=%.4f\n" % delta_len
	report += "blue_leads=%s\n" % str(blue_leads)
	report += "fog_present=%s\n" % str(fog_present)
	report += "DEBUG_OVERLAY_VISIBLE=%s\n" % ("YES" if visible else "NO")
	report += "capture=%s\n" % ProjectSettings.globalize_path(OUT_PATH)
	report += "report=%s\n" % ProjectSettings.globalize_path(REPORT_PATH)

	var file := FileAccess.open(REPORT_PATH, FileAccess.WRITE)
	file.store_string(report)
	file.close()

	print(report)
	get_tree().quit(0 if visible else 1)


func _sample_viewport(viewport: Viewport) -> Vector3:
	var image: Image = viewport.get_texture().get_image()
	var width := image.get_width()
	var height := image.get_height()
	var sample_count := 0
	var sum_r := 0.0
	var sum_g := 0.0
	var sum_b := 0.0

	for y in range(height / 4, height * 3 / 4, max(1, height / 32)):
		for x in range(width / 4, width * 3 / 4, max(1, width / 32)):
			var c: Color = image.get_pixel(x, y)
			sum_r += c.r
			sum_g += c.g
			sum_b += c.b
			sample_count += 1

	return Vector3(sum_r / float(sample_count), sum_g / float(sample_count), sum_b / float(sample_count))
