# BREU - Technical Design Document

## Continuidade entre ferramentas

Antes de iniciar trabalho tecnico no Codex, Cursor IDE ou Cursor CLI, leia:

- `docs/START_HERE.md`
- `docs/PROJECT_STATE.md`
- `docs/HANDOFF.md`
- `docs/gameplay/NEXT_SPRINT_TASKS.md`

Ao encerrar trabalho tecnico, atualize esses mesmos arquivos se o estado do projeto mudou.

## Stack

- Engine: Godot 4.7 Mono/.NET.
- Linguagem: C#.
- Runtime alvo do projeto: .NET 10.
- Assets 3D: Blender, exportados como `.glb`.
- Cena principal atual: `res://scenes/levels/demo_room/DemoRoom.tscn`.

## Organizacao

- `scenes/player`: Player em primeira pessoa.
- `scenes/enemies`: inimigos futuros ou prototipos fora da demo atual.
- `scenes/levels`: salas e blocos de level design.
- `scenes/props`: props interativos reutilizaveis.
- `scenes/ui`: HUD e interfaces de jogo.
- `scripts/player`: movimento, camera, lanterna, equipamento e interacao do player.
- `scripts/weapons`: dados e logica futura de armas.
- `scripts/enemies`: IA e vida de inimigos futuros.
- `scripts/interaction`: contratos e props interativos.
- `scripts/inventory`: estado simples de inventario/equipamento.
- `scripts/ui`: controle do HUD.
- `resources/weapons`: dados editaveis das armas.
- `docs`: arquitetura, agentes, design, pipeline e backlog.

## Sistemas atuais

### Player

`PlayerController` controla deslocamento fisico com `CharacterBody3D`, usando WASD, sprint simples, gravidade e `MoveAndSlide`. `PlayerLook` controla yaw do corpo e pitch do `CameraPivot`, captura o mouse ao iniciar, libera com `pause`/Esc e recaptura com clique esquerdo. O player e adicionado ao grupo `player`.

`PlayerMeleeAttack` controla o ataque basico do martelo. Ele escuta `attack`/`attack_primary`, usa raycast curto da `Camera3D` como debug e, se necessario, um hit volume esferico na frente da camera para contato corpo a corpo. O hit volume prioriza `EnemyHurtbox` no grupo `enemy_hurtbox`, ignorando interactables como bilhete/chave. Ao encontrar `EnemyPlaceholderAI`, chama `ReceiveHit(HammerDamage)`, reduz durabilidade via `GameSession.ReduceWeaponDurability()` e atualiza HUD/visual pelo `PlayerWeaponController`. No prototipo, o ataque usa mask ampla, colide com bodies/areas e `DebugMelee` fica ligado para validar o hit detection.

### Lanterna

`FlashlightController` herda de `SpotLight3D`, inicia ligado, alterna com `flashlight_toggle`/F e drena bateria por segundo. Nesta etapa de playtest, o feedback de bateria usa `GD.Print`.

### Interacao

`PlayerInteractor` usa `RayCast3D` a partir da camera, com alcance de 2.5m. Interativos implementam `IInteractable`, expondo `GetInteractionText()` e `Interact(PlayerController player)`. No `DemoRoom`, areas interativas usam camada fisica separada para o raycast nao ser bloqueado pelas colisoes temporarias dos moveis.

Interativos ativos na demo:

- `InteractableNote`: mostra o texto do bilhete no console.
- `HammerPickup`: registra o martelo no inventario, esconde os visuais importados e ativa o placeholder na mao.
- `DoorInteractable`: abre a passagem para o corredor placeholder.

### Sessao e inventario

`GameSession` e um autoload em `res://scripts/system/GameSession.cs`. Ele guarda estado em memoria enquanto o jogo roda: `HasRustyHammer`, `HasOldKey`, `CurrentWeaponName`, `CurrentWeaponDurability` e `CurrentWeaponMaxDurability`. Nao salva em disco ainda.

`PlayerInventory` continua existindo como estado local do Player instanciado na cena, mas agora e sincronizado com `GameSession` por `PlayerWeaponController`. Isso permite trocar de cena e manter o martelo equipado.

Quando a durabilidade do martelo chega a 0, `GameSession.ClearWeapon()` limpa a arma equipada. `HasRustyHammer` pode continuar verdadeiro como historico de coleta, mas `HasWeapon()` passa a retornar falso e o HUD volta para `Maos vazias`.

### HUD

`HUDController` escuta `PlayerInteractor.FocusChanged` para mostrar prompt minimo `[E] <acao>` na tela. Tambem escuta `PlayerInventory.InventoryChanged` para mostrar o martelo equipado e sua durabilidade. O HUD ignora input de mouse para nao bloquear o mouse look.

### Equipamento visual

`PlayerEquipmentView` mostra o placeholder `CameraPivot/Camera3D/WeaponHolder/EquippedHammerVisual` quando `PlayerInventory.HasHammer` fica verdadeiro. `PlayerWeaponController` consulta `GameSession` no `_Ready()` do Player e reativa esse visual ao carregar cenas novas. `PlayerMeleeAttack` aplica um tween curto nesse visual como feedback temporario de golpe.

### Ambiente

`DemoRoom.tscn` instancia o GLB importado em `Environment/quarto_07_blockout` e adiciona nos auxiliares para gameplay, colisoes, interacoes, luz, player e debug. O asset importado nao deve ser alterado para gameplay.

`TrailIntro.tscn` instancia `trail_intro_blockout.glb` em `Environment/trail_intro_blockout` e tambem o GLB visual da fachada em `Environment/HouseExteriorAtTrailEnd`. A cena adiciona player, HUD, colisoes temporarias, luzes, ambiencia e a porta interativa `Interactables/EnterPensionDoor`. A trilha agora transiciona direto para `DemoRoom.tscn`. `DistantHouseSilhouette` e `HouseEntryTrigger` permanecem na cena como fallback antigo, mas desativados.

`HouseExterior.tscn` instancia `pensao_santa_luzia_exterior_blockout.glb` em `Environment/pensao_santa_luzia_exterior_blockout`. A cena tem player, HUD, colisoes auxiliares, luz da lua, lampiao, ambiencia externa, `EnterHouseDoor` e `BackToTrailTrigger`. Ela continua disponivel como cena isolada de teste/comparacao, mas nao faz mais parte obrigatoria do fluxo principal.

`RitualRoom.tscn` instancia `sala_santos_secos_blockout.glb` em `Environment/sala_santos_secos_blockout`. A cena usa nos auxiliares para colisoes, luzes, interativos, susto, audio e inimigo placeholder. O GLB nao recebe logica de gameplay.

`PlayerSpawnResolver` e usado nas cenas do fluxo principal para posicionar o player no marcador de spawn e registrar checkpoints em memoria.

### Sistemas globais

`SceneTransition` e autoload em `res://scenes/system/SceneTransition.tscn`. Ele usa `SceneTransitionController` para `ChangeSceneWithFade(scenePath, message)`, tela preta e mensagem opcional.

`CheckpointManager` e autoload em `res://scenes/system/CheckpointManager.tscn`. Ele guarda apenas `LastSceneName` e `LastCheckpoint` em memoria; nao existe save em disco ainda.

`GameSession` e autoload em `res://scripts/system/GameSession.cs`. Ele preserva arma atual e Chave Velha entre trocas de cena, mas nao persiste apos fechar o jogo.

### Triggers de level

`HouseEntryTrigger` e um `Area3D` antigo da Trilha Noturna. Ele ficou preservado como fallback, mas esta desativado no fluxo principal.

`EnterHouseTrigger` e o script da porta interativa da Pensao, usado em `TrailIntro/Interactables/EnterPensionDoor` e tambem no no `EnterHouseDoor` da cena isolada `HouseExterior`. O player precisa mirar na porta e apertar `E`; entao o script mostra mensagem no HUD, imprime debug e troca para `DemoRoom.tscn` usando `SceneTransition.ChangeSceneWithFade` quando disponivel.

`OneShotMessageTrigger` e um `Area3D` narrativo simples para mensagens curtas de atmosfera que disparam uma unica vez.

`BackToTrailTrigger` e um `Area3D` informativo usado na fachada. Ele mostra mensagem e imprime debug, mas ainda nao troca cena.

`RitualRoomScareTrigger` e um `Area3D` de susto da Sala dos Santos Secos. Ele mostra mensagem, toca stinger quando disponivel, liga radio static por alguns segundos, pisca luzes e ativa `EnemyPlaceholderAI`.

`RitualExitDoorTrigger` e a porta bloqueada da Sala dos Santos Secos. Mostra mensagem no HUD, imprime debug e ainda nao troca cena.

### Interativos da RitualRoom

`RitualNoteInteractable` abre `NoteReaderUI` com o bilhete da Sala dos Santos Secos e registra checkpoint `Ritual_Note_Read`.

`OldKeyPickup` coleta a Chave Velha com estado local `HasOldKey`, chama `GameSession.CollectOldKey()`, toca som quando disponivel e mostra mensagem no HUD. Ele nao limpa nem troca a arma equipada.

## Sistemas preparados, mas fora da demo atual

### Stamina

`PlayerStamina` expoe `Consume`, `HasStamina` e sinal `StaminaChanged`. A ideia futura e usar stamina para corrida e combate.

### Armas

`WeaponData` e um Resource com dano, alcance, durabilidade, custo de stamina, cooldown e impacto. `WeaponController` existe como base futura. O combate ativo da vertical slice, por enquanto, e `PlayerMeleeAttack`, focado apenas no Martelo Enferrujado persistido em `GameSession`.

### Inimigo

`EnemyPlaceholderAI` controla o inimigo placeholder usado na Sala dos Santos Secos. Ele herda de `CharacterBody3D`, possui estados `Dormant`, `Idle`, `Alert`, `Chasing`, `Attacking` e `Stunned`, persegue diretamente o player com `MoveAndSlide`, toca audio basico e aplica dano simples via `PlayerHealth`.

O placeholder usa origem nos pes, capsula alinhada acima do piso e ajuste inicial unico de altura ao ativar. Durante o prototipo, `LockVerticalMovement` fica ligado para manter o inimigo no plano do piso e evitar que ele afunde enquanto ainda nao temos navmesh/colisoes finais. Nao ha recuperacao de piso rodando todo frame.

`EnemyHurtbox` e uma `Area3D` filha do `EnemyPlaceholder`, no grupo `enemy_hurtbox`, usada pelo martelo para detectar acertos sem depender de raycast fino. Ela liga/desliga junto com o inimigo.

`EnemyAI` e `EnemyHealth` continuam como base futura para inimigos mais completos.

### PlayerHealth

`PlayerHealth` e um no filho de `Player.tscn`. Ele guarda `MaxHealth`, `CurrentHealth`, `TakeDamage(int)` e `Heal(int)`. Por enquanto, dano imprime debug e mostra mensagem simples no HUD; morte ainda e TODO.

## Decisoes temporarias

- `DemoRoom.tscn` instancia `Player.tscn` diretamente para o playtest do Quarto 07.
- Colisoes do Quarto 07 usam `StaticBody3D` com `BoxShape3D` temporarios em `DemoRoom/Collisions`.
- Colisoes dos moveis principais usam caixas auxiliares em `DemoRoom/Collisions/FurnitureCollisions`.
- Martelo, bilhete e porta usam `Area3D` auxiliar em `DemoRoom/InteractionPoints`.
- A porta usa `DoorInteractable`; ao abrir, desativa `DoorClosedCollision` e tenta esconder `door_01`.
- `DemoRoom/Environment/CorridorPlaceholder` fornece um corredor curto temporario depois da porta, com chao, paredes, teto, bloqueio final e `DemoEndTrigger`.
- Combate completo ainda nao entra neste playtest; o inimigo da RitualRoom ja persegue e aplica dano simples.
- HUD usa Labels simples, sem arte final.
- `TrailIntro.tscn` ainda usa colisoes laterais retas.
- `HouseExterior.tscn` usa colisoes temporarias em caixas, porta sem animacao visual, entrada por prompt `[E]` e retorno para trilha apenas informativo.

## Riscos e cuidados

- Confirmar compatibilidade entre Godot 4.7 Mono e `net10.0` ao atualizar SDKs.
- Ajustar colisoes auxiliares quando o GLB do quarto mudar.
- Trocar a porta debug por porta com pivo, animacao, audio e estado persistente.
- Criar UI real de leitura do bilhete antes de adicionar muitos documentos.
- Adicionar vida/dano do player antes de balancear inimigos.
