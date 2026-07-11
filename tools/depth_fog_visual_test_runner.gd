extends Node

const SCENE := "res://scenes/levels/trail_intro/TrailIntro.tscn"
const OUT_PATH := "user://depth_fog_capture.png"
const REPORT_PATH := "user://depth_fog_report.txt"


func _ready() -> void:
	call_deferred("_run")


func _run() -> void:
	var scene: Node = load(SCENE).instantiate()
	get_tree().root.add_child(scene)

	for _i in 12:
		await get_tree().process_frame

	var fog_node: MeshInstance3D = scene.get_node("Player/CameraPivot/Camera3D/DepthFogPostProcess") as MeshInstance3D
	var material: ShaderMaterial = fog_node.material_override as ShaderMaterial
	var debug_on: bool = bool(material.get_shader_parameter("debug_depth"))
	var debug_sky_on: bool = bool(material.get_shader_parameter("debug_sky"))

	var viewport: Viewport = get_tree().root
	var image: Image = viewport.get_texture().get_image()
	image.save_png(OUT_PATH)

	var width := image.get_width()
	var height := image.get_height()
	var near_y := int(height * 0.82)
	var far_y := int(height * 0.18)
	var center_x := int(width * 0.5)

	var near_c: Color = image.get_pixel(center_x, near_y)
	var far_c: Color = image.get_pixel(center_x, far_y)
	var near_lum := _luminance(near_c)
	var far_lum := _luminance(far_c)
	var depth_contrast := far_lum - near_lum

	var depth_ok := debug_on and depth_contrast > 0.12
	var sky_magenta_ok := false
	var geom_not_magenta_ok := false
	if debug_sky_on:
		var sky_c := _sample_region(image, 0.5, 0.08)
		var path_c := _sample_region(image, 0.5, 0.82)
		sky_magenta_ok = sky_c.x > 0.75 and sky_c.y < 0.25 and sky_c.z > 0.75
		geom_not_magenta_ok = not (path_c.x > 0.75 and path_c.y < 0.25 and path_c.z > 0.75)
	var fog_ok := false
	var mid_path_delta := 0.0
	var sky_washed := false
	if not debug_on:
		var fog_node_visible := fog_node.visible
		var with_img := viewport.get_texture().get_image()
		var mid_with := _sample_region(with_img, 0.5, 0.55)
		var near_with := _sample_region(with_img, 0.5, 0.78)
		var sky_with := _sample_region(with_img, 0.5, 0.12)
		fog_node.visible = false
		for _i in 3:
			await get_tree().process_frame
		var without_img := viewport.get_texture().get_image()
		var mid_without := _sample_region(without_img, 0.5, 0.55)
		var near_without := _sample_region(without_img, 0.5, 0.78)
		var sky_without := _sample_region(without_img, 0.5, 0.12)
		fog_node.visible = fog_node_visible
		mid_path_delta = (mid_with - mid_without).length()
		var near_delta := (near_with - near_without).length()
		var sky_delta := (sky_with - sky_without).length()
		fog_ok = mid_path_delta > 0.025
		sky_washed = sky_delta > 0.08 and _luminance(Color(sky_with.x, sky_with.y, sky_with.z)) > _luminance(Color(sky_without.x, sky_without.y, sky_without.z)) + 0.06
		image = with_img

	var report := ""
	report += "debug_depth_param=%s\n" % str(debug_on)
	report += "near_lum=%.4f\n" % near_lum
	report += "far_lum=%.4f\n" % far_lum
	report += "depth_contrast=%.4f\n" % depth_contrast
	report += "DEBUG_SKY_OK=%s\n" % ("YES" if sky_magenta_ok else "NO")
	report += "GEOM_NOT_MAGENTA=%s\n" % ("YES" if geom_not_magenta_ok else "NO")
	report += "FOG_MID_PATH=%s\n" % ("YES" if fog_ok else "NO")
	report += "mid_path_delta=%.4f\n" % mid_path_delta
	report += "sky_washed=%s\n" % str(sky_washed)
	report += "capture=%s\n" % ProjectSettings.globalize_path(OUT_PATH)

	var file := FileAccess.open(REPORT_PATH, FileAccess.WRITE)
	file.store_string(report)
	file.close()

	print(report)
	var ok := depth_ok or fog_ok or (debug_sky_on and sky_magenta_ok and geom_not_magenta_ok)
	get_tree().quit(0 if ok else 1)


func _luminance(c: Color) -> float:
	return c.r * 0.2126 + c.g * 0.7152 + c.b * 0.0722


func _sample_region(image: Image, x_ratio: float, y_ratio: float) -> Vector3:
	var x := int(image.get_width() * x_ratio)
	var y := int(image.get_height() * y_ratio)
	var c: Color = image.get_pixel(x, y)
	return Vector3(c.r, c.g, c.b)


func _sample_center_band(image: Image) -> Vector3:
	var width := image.get_width()
	var height := image.get_height()
	var sum := Vector3.ZERO
	var count := 0
	for y in range(int(height * 0.35), int(height * 0.65), max(1, height / 24)):
		for x in range(int(width * 0.35), int(width * 0.65), max(1, width / 24)):
			var c: Color = image.get_pixel(x, y)
			sum += Vector3(c.r, c.g, c.b)
			count += 1
	return sum / float(count)
