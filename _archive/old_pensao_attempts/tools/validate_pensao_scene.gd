extends SceneTree

func _init() -> void:
	var packed := load("res://scenes/levels/pensao_santa_luzia/PensaoSantaLuziaIntegratedTest.tscn") as PackedScene
	if packed == null:
		push_error("Nao foi possivel carregar PensaoSantaLuziaIntegratedTest.tscn")
		quit(1)
		return
	var scene := packed.instantiate()
	if scene == null:
		push_error("Nao foi possivel instanciar PensaoSantaLuziaIntegratedTest.tscn")
		quit(2)
		return
	var required_paths := [
		"Player", "PlayerSpawn", "StaticGameplayCollisions/ExteriorGroundCollision",
		"StaticGameplayCollisions/PathCollision",
		"StaticGameplayCollisions/WorldBoundsCollisionLeft", "StaticGameplayCollisions/WorldBoundsCollisionRight",
		"StaticGameplayCollisions/PorchCollision", "StaticGameplayCollisions/InteriorFloorCollision",
		"StaticGameplayCollisions/StairRampCollision", "StaticGameplayCollisions/SecondFloorTemporaryBlocker",
		"WorldLabels/JobOfferSignText",
		"InteractionMarkers/GuestRegister", "InteractionMarkers/LockedDeposit",
	]
	for path in required_paths:
		if scene.get_node_or_null(path) == null:
			push_error("No obrigatorio ausente: " + path)
			scene.free()
			quit(3)
			return
	var imported_world := scene.get_node("ImportedWorld")
	var imported_names := PackedStringArray()
	for node in imported_world.find_children("*", "", true, false):
		imported_names.append(node.name.to_lower())
	for forbidden in ["cliff_left_slope_m02", "cliff_right_slope_m02"]:
		if imported_names.has(forbidden):
			push_error("Correcao visual M.2 ainda importada: " + forbidden)
			scene.free()
			quit(4)
			return
	for required_mesh in ["cliff_left_main", "cliff_right_main"]:
		if not imported_names.has(required_mesh):
			push_error("Visual anterior ausente: " + required_mesh)
			scene.free()
			quit(5)
			return
	for hidden_path in ["fog_entry_low", "fog_mid_path", "fog_pension_front", "fog_left_bank", "fog_right_bank", "fog_interior_corridor", "job_offer_board_text"]:
		var hidden_node := scene.get_node_or_null("ImportedWorld/LevelMesh/" + hidden_path) as Node3D
		if hidden_node == null or hidden_node.visible:
			push_error("Visual importado deveria estar oculto: " + hidden_path)
			scene.free()
			quit(6)
			return
	print("PENSAO_SCENE_OK root=", scene.name, " required_nodes=", required_paths.size())
	scene.free()
	quit(0)
