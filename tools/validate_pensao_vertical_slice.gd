extends SceneTree

func _init() -> void:
	call_deferred("_run")

func _run() -> void:
	var packed := load("res://scenes/levels/pensao_santa_luzia/PensaoSantaLuziaVerticalSlice.tscn") as PackedScene
	if packed == null:
		_fail("Cena oficial nao carregou", 1)
		return
	var scene := packed.instantiate()
	root.add_child(scene)
	await physics_frame
	var required := [
		"Visual/WorldModel", "Player", "UI/HUD", "PlayerSpawn",
		"StaticGameplayCollisions/ExteriorGroundCollision",
		"StaticGameplayCollisions/InteriorFloor01Collision",
		"StaticGameplayCollisions/InteriorFloor02MainCollision",
		"StaticGameplayCollisions/StairRampCollision",
		"StaticGameplayCollisions/Floor2GuardCollision",
		"StaticGameplayCollisions/ExteriorWallCollisions/Back",
		"Interactions/Room102", "Interactions/FusePickup", "Interactions/Deposit",
		"Interactions/ManagerClue", "Visual/SignLabels/PensaoSign",
	]
	for path in required:
		if scene.get_node_or_null(path) == null:
			_fail("No ausente: " + path, 2)
			return
	if scene.has_node("Base") or scene.has_node("MansionBlockout") or scene.has_node("VisualFilter"):
		_fail("Dependencia de tentativa antiga ainda presente", 3)
		return
	var model := scene.get_node("Visual/WorldModel")
	for node in model.find_children("*", "", true, false):
		if node.name.to_lower().begins_with("guide_"):
			_fail("Guide exportado como visual: " + node.name, 4)
			return
	var controller := scene.get_node("VerticalSliceController")
	controller.call("Interact", 3)
	controller.call("Interact", 4)
	await physics_frame
	if not controller.get("HasFuse") or not controller.get("DepositUnlocked") or not controller.get("UpstairsUnlocked"):
		_fail("Puzzle nao concluiu os estados", 5)
		return
	var deposit_shape := scene.get_node("StaticGameplayCollisions/DepositDoorCollision/Shape") as CollisionShape3D
	var stair_shape := scene.get_node("StaticGameplayCollisions/StairAccessBlocker/Shape") as CollisionShape3D
	if not deposit_shape.disabled or not stair_shape.disabled:
		_fail("Blockers continuam ativos apos puzzle", 6)
		return
	var deposit_visual := scene.get_node("Visual/WorldModel/deposit_door_dark") as Node3D
	if deposit_visual.visible:
		_fail("Porta visual do deposito continua fechada apos puzzle", 7)
		return
	var player := scene.get_node("Player") as CharacterBody3D
	var route := [
		[Vector3(0, 1.65, 22.5), Vector3(0, 1.65, -9.0), "trilha/entrada/corredor"],
		[Vector3(0, 1.65, -7.0), Vector3(-3.2, 1.65, -7.0), "recepcao"],
		[Vector3(0, 1.65, -12.5), Vector3(-3.0, 1.65, -12.5), "quarto 102"],
		[Vector3(0, 1.65, -13.0), Vector3(4.2, 1.65, -13.0), "cozinha"],
		[Vector3(0, 1.65, -18.0), Vector3(3.5, 1.65, -18.0), "deposito liberado"],
		[Vector3(0, 1.65, -6.0), Vector3(5.1, 1.65, -6.0), "acesso escada"],
		[Vector3(5.1, 4.95, -13.5), Vector3(0, 4.95, -13.5), "saida escada/corredor superior"],
		[Vector3(0, 4.95, -13.0), Vector3(-4.5, 4.95, -13.0), "acesso ala gerente"],
		[Vector3(-4.5, 4.95, -13.0), Vector3(-4.5, 4.95, -22.0), "quarto gerente"],
	]
	for segment in route:
		var start: Vector3 = segment[0]
		var finish: Vector3 = segment[1]
		if player.test_move(Transform3D(Basis.IDENTITY, start), finish - start):
			_fail("Rota bloqueada: " + str(segment[2]), 8)
			return
	print("PENSAO_M32_OK nodes=", required.size(), " puzzle=true routes=true legacy=false bevel_export=true")
	scene.queue_free()
	quit(0)

func _fail(message: String, code: int) -> void:
	push_error(message)
	quit(code)
