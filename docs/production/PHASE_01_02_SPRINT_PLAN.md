# Plano de Producao - Fases 1 e 2

Este documento organiza as sprints de producao para consolidar a Fase 1 e construir a Fase 2.

## Sprint A - Consolidar Quarto 07

**Status:** base jogavel concluida; ajustes finos continuam.

### Entregas

- Player
- Lanterna
- Passos
- Pulo
- Agachamento
- Martelo
- Bilhete
- Porta
- Corredor
- Susto
- HUD
- Audio base
- Porta final e transicao para `RitualRoom.tscn`

### Proximos ajustes

- Melhorar UI
- Sons finais
- Colisoes finas
- Porta final com arte/animacao real

---

## Sprint B - Trilha de Entrada

**Status:** base Godot criada e conectada.

### Entregas

- `TrailIntro.tscn`
- `trail_intro_blockout.glb`
- Player no inicio da trilha
- Colisoes temporarias
- Luz da casa ao longe
- Sons noturnos
- Trigger de chegada
- Transicao `TrailIntro -> HouseExterior`

### Proximos ajustes

- Vegetacao seca
- Cerca, cactos, pedras e galhos como bloqueios mais organicos
- Ajuste fino de escala/orientacao
- Ambiencia em camadas

---

## Sprint C - Fachada da Pensao Santa Luzia

**Status:** blockout importado; fluxo `TrailIntro -> HouseExterior -> DemoRoom` jogavel e costurado.

### Entregas

- `HouseExterior.tscn`
- `pensao_santa_luzia_exterior_blockout.glb`
- Player na frente da fachada
- Colisoes temporarias do terreno, paredes, varanda, porta e limites
- `MoonLight`
- `FrontLanternLight`
- `ExteriorAmbience`
- `EnterHouseTrigger` para `DemoRoom.tscn`
- `BackToTrailTrigger` informativo

### Sprint C.5 - Costura da Fase 1

**Status:** concluida como base jogavel.

### Entregas

- Cena principal do projeto apontando para `TrailIntro.tscn`.
- `SceneTransition` com `ChangeSceneWithFade(scenePath, message)` e mensagens de fade.
- `PlayerSpawnResolver` para alinhar player aos marcadores de spawn das cenas.
- `CheckpointManager` em memoria para registrar `TrailIntro_Start`, `HouseExterior_Entrance` e `DemoRoom_Quarto07`.
- `OneShotMessageTrigger` para mensagens narrativas curtas.
- `LightFlicker` aplicado na luz distante da silhueta da Pensao.
- Guia `docs/testing/PLAYTEST_PHASE_01_FLOW.md`.

### Proximos ajustes

- Validar escala/orientacao no editor
- Ajustar colisoes da fachada
- Criar porta visual/animada
- Criar retorno real `HouseExterior -> TrailIntro`, se necessario
- Refinar lampiao e leitura visual da entrada

---

## Sprint D - Sala dos Santos Secos

### Objetivos

- Criar sala ritualistica
- Mesa com velas
- Ossos
- Cruzes
- Rede
- Simbolos
- Primeira arena de combate/fuga

### Godot

- Substituir `RitualRoom.tscn` placeholder por asset Blender
- Luz de vela
- Trigger narrativo
- Inimigo placeholder
- Colisoes

---

## Sprint E - Primeiro Inimigo Placeholder

### Objetivos

- Melhorar placeholder
- Perseguicao simples
- Stun basico
- Som de respiracao
- Som de passos
- Reacao a lanterna em prototipo futuro

### Godot

- `EnemyPlaceholder.tscn`
- `EnemyAI.cs`
- `EnemyPerception.cs`
- `EnemyAudio.cs`

Nao usar Blender ainda.

---

## Sprint F - Inimigo Final no Blender

So iniciar apos validar:

- escala;
- distancia;
- perseguicao;
- timing do susto;
- comportamento basico.

### Criar primeiro inimigo

O Hospede.

### Assets Blender

- Modelo low/mid poly
- Roupa rasgada
- Materiais sujos
- Rig simples

### Animacoes

- Idle
- Walk
- Chase
- Attack
- Hit reaction
- Death/stun opcional

---

## Ordem recomendada

```text
Sprint A -> Sprint B -> Sprint C -> Sprint D -> Sprint E -> Sprint F
```

## Documentos relacionados

- `docs/design/GAME_VISION.md`
- `docs/design/PHASE_01_LEVEL_DESIGN.md`
- `docs/design/PHASE_02_LEVEL_DESIGN.md`
- `docs/design/ENEMY_DESIGN.md`
- `docs/design/SCENARIO_ART_DIRECTION.md`
- `docs/SPRINT_HISTORY.md`
- `docs/gameplay/NEXT_SPRINT_TASKS.md`
