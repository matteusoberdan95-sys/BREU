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

### Lanterna

`FlashlightController` herda de `SpotLight3D`, inicia ligado, alterna com `flashlight_toggle`/F e drena bateria por segundo. Nesta etapa de playtest, o feedback de bateria usa `GD.Print`.

### Interacao

`PlayerInteractor` usa `RayCast3D` a partir da camera, com alcance de 2.5m. Interativos implementam `IInteractable`, expondo `GetInteractionText()` e `Interact(PlayerController player)`. No `DemoRoom`, areas interativas usam camada fisica separada para o raycast nao ser bloqueado pelas colisoes temporarias dos moveis.

Interativos ativos na demo:

- `InteractableNote`: mostra o texto do bilhete no console.
- `HammerPickup`: registra o martelo no inventario, esconde os visuais importados e ativa o placeholder na mao.
- `DoorInteractable`: abre a passagem para o corredor placeholder.

### Inventario

`PlayerInventory` guarda estado simples do martelo: `HasHammer`, `EquippedWeaponName`, `EquippedWeaponDurability` e `PickupHammer(int durability)`. Nao ha combate nem troca de arma nesta etapa.

### HUD

`HUDController` escuta `PlayerInteractor.FocusChanged` para mostrar prompt minimo `[E] <acao>` na tela. Tambem escuta `PlayerInventory.InventoryChanged` para mostrar o martelo equipado e sua durabilidade. O HUD ignora input de mouse para nao bloquear o mouse look.

### Equipamento visual

`PlayerEquipmentView` mostra `WeaponHand/HammerVisual` quando `PlayerInventory.HasHammer` fica verdadeiro. O martelo na mao e placeholder editavel, sem animacao e sem combate.

### Ambiente

`DemoRoom.tscn` instancia o GLB importado em `Environment/quarto_07_blockout` e adiciona nos auxiliares para gameplay, colisoes, interacoes, luz, player e debug. O asset importado nao deve ser alterado para gameplay.

## Sistemas preparados, mas fora da demo atual

### Stamina

`PlayerStamina` expoe `Consume`, `HasStamina` e sinal `StaminaChanged`. A ideia futura e usar stamina para corrida e combate.

### Armas

`WeaponData` e um Resource com dano, alcance, durabilidade, custo de stamina, cooldown e impacto. `WeaponController` existe como base, mas combate nao esta ativo na demo room atual.

### Inimigo

`EnemyAI` e `EnemyHealth` existem como base futura. Inimigo nao deve ser ativado no Quarto 07 ate o usuario pedir combate/encontro.

## Decisoes temporarias

- `DemoRoom.tscn` instancia `Player.tscn` diretamente para o playtest do Quarto 07.
- Colisoes do Quarto 07 usam `StaticBody3D` com `BoxShape3D` temporarios em `DemoRoom/Collisions`.
- Colisoes dos moveis principais usam caixas auxiliares em `DemoRoom/Collisions/FurnitureCollisions`.
- Martelo, bilhete e porta usam `Area3D` auxiliar em `DemoRoom/InteractionPoints`.
- A porta usa `DoorInteractable`; ao abrir, desativa `DoorClosedCollision` e tenta esconder `door_01`.
- `DemoRoom/Environment/CorridorPlaceholder` fornece um corredor curto temporario depois da porta, com chao, paredes, teto, bloqueio final e `DemoEndTrigger`.
- Combate e inimigo nao entram neste playtest inicial.
- HUD usa Labels simples, sem arte final.

## Riscos e cuidados

- Confirmar compatibilidade entre Godot 4.7 Mono e `net10.0` ao atualizar SDKs.
- Ajustar colisoes auxiliares quando o GLB do quarto mudar.
- Trocar a porta debug por porta com pivo, animacao, audio e estado persistente.
- Criar UI real de leitura do bilhete antes de adicionar muitos documentos.
- Adicionar vida/dano do player antes de balancear inimigos.
