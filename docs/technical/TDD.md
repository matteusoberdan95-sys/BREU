# BREU - Technical Design Document inicial

## Continuidade entre ferramentas

Antes de iniciar trabalho tecnico no Codex, Cursor IDE ou Cursor CLI, leia:

- `docs/PROJECT_STATE.md`
- `docs/HANDOFF.md`
- `docs/gameplay/NEXT_SPRINT_TASKS.md`

Ao encerrar trabalho tecnico, atualize esses mesmos arquivos se o estado do projeto mudou.

## Stack

- Engine: Godot 4.x .NET.
- Linguagem: C#.
- Runtime alvo do projeto: .NET 10.
- Assets 3D: Blender, exportados como `.glb`.
- Cena principal atual: `res://scenes/levels/demo_room/DemoRoom.tscn`.

## Organizacao

- `scenes/player`: Player em primeira pessoa.
- `scenes/enemies`: inimigos prontos para instanciar.
- `scenes/levels`: salas e blocos de level design.
- `scenes/props`: props interativos reutilizaveis.
- `scripts/player`: movimento, camera, stamina, lanterna e interacao.
- `scripts/weapons`: dados e logica de armas.
- `scripts/enemies`: IA e vida de inimigos.
- `scripts/interaction`: contratos e props interativos.
- `resources/weapons`: dados editaveis das armas.
- `docs`: arquitetura, agentes, design, pipeline e backlog.

## Sistemas

### Player

`PlayerController` controla deslocamento fisico com `CharacterBody3D`. `PlayerLook` controla yaw do corpo e pitch da camera. O player e adicionado ao grupo `player`.

### Stamina

`PlayerStamina` expõe `Consume`, `HasStamina` e sinal `StaminaChanged`. Corrida e ataque consomem stamina. Regeneracao tem atraso curto apos consumo.

### Lanterna

`FlashlightController` herda de `SpotLight3D`, controla bateria, toggle e flicker leve. Emite `BatteryChanged` para HUD.

### Interacao

`PlayerInteractor` usa `RayCast3D` a partir da camera. Qualquer Node que implemente `IInteractable` pode receber `Interact(PlayerController player)`.

### Inventario

`PlayerInventory` guarda chaves, documentos e arma equipada. E propositalmente simples nesta etapa.

### Armas

`WeaponData` e um Resource com dano, alcance, durabilidade, custo de stamina, cooldown e impacto. `WeaponController` usa RayCast frontal para aplicar dano. Quando uma arma quebra, equipa `UnarmedWeapon`.

### Inimigo

`EnemyAI` usa estados simples e distancia ate o player para perseguir/atacar. `EnemyHealth` concentra vida, dano e morte.

## Decisoes temporarias

- Combate usa RayCast em vez de ShapeCast para acelerar a vertical slice.
- Ataque do inimigo ainda nao aplica dano real ao player; apenas loga o evento.
- HUD usa Labels simples, sem arte final.
- Props e sala usam Meshes basicos do Godot.

## Riscos

- Confirmar a versao exata do Godot.NET.Sdk instalada localmente.
- Confirmar suporte real do Godot usado ao `net10.0`.
- Trocar RayCast de combate por volume sincronizado com animacao.
- Adicionar vida/dano do player antes de balancear inimigos.
