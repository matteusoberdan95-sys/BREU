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
- Fachada real integrada no fim da trilha
- Porta da Pensao transiciona `TrailIntro -> DemoRoom`

### Proximos ajustes

- Vegetacao seca
- Cerca, cactos, pedras e galhos como bloqueios mais organicos
- Ajuste fino de escala/orientacao
- Ambiencia em camadas

---

## Sprint C - Fachada da Pensao Santa Luzia

**Status:** blockout importado; preservado como cena isolada de teste/comparacao.

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

### Decisao de fluxo

A fachada visual da Pensao Santa Luzia foi integrada diretamente ao fim de `TrailIntro.tscn` como GLB visual em `Environment/HouseExteriorAtTrailEnd`. Isso melhora continuidade e imersao: o fluxo principal agora pula `HouseExterior.tscn` e vai direto da porta da trilha para o Quarto 07.

`HouseExterior.tscn` nao foi removida. Ela continua disponivel como cena isolada para teste visual, comparacao e possivel separacao futura por performance.

### Sprint C.5 - Costura da Fase 1

**Status:** concluida como base jogavel.

### Entregas

- Cena principal do projeto apontando para `TrailIntro.tscn`.
- `SceneTransition` com `ChangeSceneWithFade(scenePath, message)` e mensagens de fade.
- `PlayerSpawnResolver` para alinhar player aos marcadores de spawn das cenas.
- `CheckpointManager` em memoria para registrar `TrailIntro_Start` e `DemoRoom_Quarto07`.
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

**Status:** base jogavel criada.

### Objetivos

- Criar sala ritualistica
- Mesa com velas
- Ossos
- Cruzes
- Rede
- Simbolos
- Primeira arena de combate/fuga

### Godot

- [x] `res://scenes/levels/ritual_room/RitualRoom.tscn`
- [x] Asset `sala_santos_secos_blockout.glb` importado como ambiente
- [x] Colisoes temporarias de sala, mesa e porta
- [x] Luz de vela, altar e fill escuro
- [x] Ambiencia `room_tone_01.ogg` e radio static 3D
- [x] Bilhete ritual interativo
- [x] Chave Velha coletavel com estado local
- [x] Trigger de susto ritualistico
- [x] `EnemyPlaceholder` preparado inicialmente para aparicao
- [x] Porta de saida bloqueada

### Proximos ajustes

- Integrar Chave Velha ao inventario real.
- Criar objetivo para liberar porta de saida.
- Refinar colisoes conforme escala do GLB.
- Evoluir inimigo para perseguicao/ataque simples na Sprint E.

---

## Sprint E - Primeiro Inimigo Placeholder

**Status:** base jogavel criada.

### Objetivos

- Melhorar placeholder
- Perseguicao simples
- Stun basico
- Som de respiracao
- Som de passos
- Reacao a lanterna em prototipo futuro

### Godot

- [x] `EnemyPlaceholder.tscn` convertido para `CharacterBody3D`
- [x] `EnemyPlaceholderAI.cs` com estados `Dormant`, `Idle`, `Alert`, `Chasing`, `Attacking`, `Stunned`
- [x] Perseguicao direta simples ate o player
- [x] Ataque com cooldown e dano simples
- [x] `PlayerHealth.cs` com `TakeDamage` e `Heal`
- [x] Audio basico de respiracao, passos e growl
- [x] `ApplyStun` e `ReceiveHit` preparados para proxima sprint

### Proximos ajustes

- Integrar stun/impacto ao martelo.
- Criar HUD de vida ou feedback visual de dano.
- Ajustar perseguicao com NavigationAgent3D se a sala exigir.

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
