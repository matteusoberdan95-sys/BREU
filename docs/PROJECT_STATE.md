# BREU - Estado do projeto

Ultima atualizacao: 2026-07-10

## Resumo rapido

O fluxo inicial jogavel agora e:

```text
    TrailIntro -> DemoRoom -> corredor -> RitualRoom
```

A Trilha Noturna agora contem a fachada real da Pensao Santa Luzia no fim do caminho. A porta da Pensao leva direto para o Quarto 07. O Quarto 07 segue com bilhete, martelo, porta, corredor, susto, porta final e transicao para a Sala dos Santos Secos (`RitualRoom`).

A cena principal do projeto (`run/main_scene`) aponta para `TrailIntro.tscn`, para testar a entrada completa desde o inicio.

Historico: `docs/SPRINT_HISTORY.md`.

## Estado jogavel atual

### Player

- Movimento FPS: andar, correr com stamina, agachar (`Ctrl`/`C`) e pular (`Space`).
- Player feel cinematografico: `PlayerBodyMotion` com gait procedural, corrida suavizada, headbob, ombro/roll, inercia, step impact visual, respiracao visual/audio, lean (`Q`/`R`), sway da lanterna/martelo e camera shake curto ao tomar dano.
- Passos por superficie (`SurfaceTag` + grupos `surface_*`).
- Lanterna com HUD de bateria.
- HUD com stamina, lanterna, arma, prompts e mensagens temporarias.
- HUD com vida, flash vermelho de dano e tela de morte.
- Martelo persistente entre cenas via `GameSession`.
- Ataque basico com Martelo Enferrujado no botao esquerdo: raycast curto, stun no inimigo, durabilidade e quebra.
- Morte/retry: ao chegar a `0/100`, o player abre `DeathScreen` e pode voltar ao ultimo checkpoint.

### Trilha Noturna

- Cena: `scenes/levels/trail_intro/TrailIntro.tscn`.
- Asset: `assets/blender_exports/trail_intro/trail_intro_blockout.glb`.
- Player nasce em `Vector3(0, 1, 14)`.
- A fachada real da Pensao e instanciada como GLB visual em `Environment/HouseExteriorAtTrailEnd`.
- `Interactables/EnterPensionDoor` troca direto para `DemoRoom.tscn` com prompt `[E] Entrar na Pensao`.
- `HouseEntryTrigger` antigo foi mantido desativado como fallback.
- `DistantHouseSilhouette` foi mantida desativada como fallback visual.
- `SilhouetteMessageTrigger` mostra `A luz nao deveria estar acesa.`.
- `HouseFrontLanternLight` usa `LightFlicker` para oscilacao sutil do lampiao.

### Fachada da Pensao

- Cena: `scenes/levels/house_exterior/HouseExterior.tscn`.
- Asset: `assets/blender_exports/house_exterior/pensao_santa_luzia_exterior_blockout.glb`.
- Status: cena isolada de teste/comparacao; nao faz mais parte obrigatoria do fluxo principal.
- Player nasce em `Vector3(0, 1, -8.5)`, no inicio do terreno da fachada, olhando para a porta.
- Colisoes temporarias: chao, paredes, varanda, blocker de porta e limites do terreno.
- Luzes: `MoonLight` fria e `FrontLanternLight` quente.
- Audio: `wind_old_house_01.ogg` em loop via `Audio/ExteriorAmbience`.
- `EnterHouseDoor` e interativo: mirar na porta e apertar `E` troca para `DemoRoom.tscn`.
- `DoorScentMessageTrigger` mostra `Tem cheiro de vela queimada.` perto da entrada.
- `BackToTrailTrigger` mostra mensagem, mas ainda nao troca cena.

### Quarto 07 -> Corredor -> Fase 2

1. Explorar quarto, bilhete (`NoteReaderUI`), martelo, porta do quarto.
2. Corredor (+Z): passos, susto em Z ~5.5, inimigo placeholder.
3. Porta final (Z ~9.1): trancada ate o susto -> `[E] Entrar` -> fade -> `scenes/levels/ritual_room/RitualRoom.tscn`.

### Sala dos Santos Secos

- Cena: `scenes/levels/ritual_room/RitualRoom.tscn`.
- Asset: `assets/blender_exports/ritual_room/sala_santos_secos_blockout.glb`.
- Player nasce em `Vector3(0, 1, -3.15)`, olhando para o centro da sala.
- Colisoes temporarias: chao, teto, paredes, mesa e porta de saida bloqueada.
- Luzes: `CandleLightMain`, `BackAltarLight` e `RoomDarkFill`.
- Audio: `RitualRoomAmbience` com `room_tone_01.ogg` e `RadioStaticPoint` 3D.
- Interativos: `RitualNotePoint`, `OldKeyPickupPoint` e `ExitDoorTrigger`.
- Se o martelo foi coletado no Quarto 07, ele continua equipado ao entrar na sala.
- Susto: `RitualScareTrigger` pisca luzes, toca stinger, liga radio static e revela `EnemyPlaceholder`.
- Inimigo: `EnemyPlaceholderAI` ativa apos o susto, nasce em ponto visivel da sala, persegue diretamente o player, ataca perto e aplica dano simples.
- Visual do inimigo: `EnemyPlaceholder.tscn` usa o blockout `enemy_hospede_seco_blockout.glb` como `Visual/HospedeSecoModel`; a capsula/cabeca antiga fica oculta como fallback.
- Animacoes do inimigo: `EnemyAnimationController` aplica idle, walk/chase, attack, hit, stunned e death placeholder por transform/tween no `Visual`.
- Combate: mirar no `EnemyPlaceholder` e clicar com o botao esquerdo acerta com o martelo, aplica stun e reduz durabilidade.
- Checkpoint: `RitualRoom_SantosSecos`, com retry recarregando a sala e restaurando snapshot do `GameSession`.

### Sistemas

- `AudioManager` + pack v01 + `AmbienceController`.
- `SceneTransition` (autoload) para fade in/out entre cenas.
- `CheckpointManager` (autoload) registra checkpoints em memoria, com cena, posicao/rotacao e snapshot simples de arma/chave.
- `GameSession` (autoload) preserva Martelo Enferrujado e Chave Velha durante a sessao.
- `PlayerSpawnResolver` posiciona o player nos spawns de cada cena.
- `RespawnResolver`, `DamageOverlay` e `DeathScreen` fecham o ciclo basico de morte/retry.
- `DemoRoomSequenceController` para estados da sequencia do Quarto 07.
- `EnterHouseTrigger` e usado na porta da Pensao integrada na trilha e na cena isolada da fachada.
- `HouseEntryTrigger` permanece apenas como fallback antigo desativado na trilha.
- `PlayerHealth` guarda vida, invulnerabilidade curta, morte e reset do player.
- `PlayerBodyMotion` aplica camera corporal procedural sem interferir no mouse look: gait/headbob, shoulder sway, inercia, step impact visual, lean, shake de dano, sway do `WeaponHolder`/lanterna e respiracao ligada a stamina.
- Respiração do player: `breath_light_01.ogg` em corrida normal, `breath_heavy_01.ogg` com stamina abaixo de 35%, `player_tired_01.ogg` como one-shot de exaustao com cooldown.
- `EnemyPlaceholderAI` controla a IA basica da Sala dos Santos Secos e para de atacar player morto.
- Direcao sonora oficial: `docs/audio/AUDIO_DIRECTION.md`.
- Registro de assets de audio: `docs/audio/AUDIO_ASSET_REGISTRY.md`.
- Direcao visual oficial: `docs/visual/VISUAL_DIRECTION.md`.
- Pipeline grafico oficial: `docs/visual/GRAPHICS_PIPELINE.md`.
- `VisualProfileApplier` existe como ferramenta opcional; `ApplyOnReady` fica desligado por padrao para nao sobrescrever cenas automaticamente.
- Presets visuais iniciais ficam em `resources/visual_profiles/`.

## Cenas principais

| Cena | Caminho |
|------|---------|
| Trilha inicial | `scenes/levels/trail_intro/TrailIntro.tscn` |
| Fachada isolada/teste | `scenes/levels/house_exterior/HouseExterior.tscn` |
| Demo / Fase 1 | `scenes/levels/demo_room/DemoRoom.tscn` |
| Sala dos Santos Secos | `scenes/levels/ritual_room/RitualRoom.tscn` |

## Fora de escopo ainda

- Combate final com animacoes, stamina e balanceamento completo.
- IA de perseguicao avancada/pathfinding.
- Porta visual/animada na fachada.
- Rig e animacoes finais do Hospede Seco.
- Sala dos Santos Secos finalizada/polida.
- Texturas finais, decals complexos e materiais definitivos.
- Save em disco para inventario/checkpoints.
- Tela de morte final com audio, animacao e arte definitiva.

## Verificacao

- `dotnet build BREU.sln` - 0 erros (2026-07-10).
- Godot headless abriu o projeto com `PlayerBodyMotion` e importou os audios de respiracao; avisos de limpeza no exit continuam aparecendo no headless (2026-07-10).
- Sprint K: `dotnet build BREU.sln` - 0 erros (2026-07-10).

## Proximo passo

Validar manualmente com F6:

1. `TrailIntro.tscn`
2. caminhar ate a fachada real no fim da trilha;
3. mirar na porta e apertar `E`;
4. confirmar `DemoRoom.tscn`.

Depois, validar Sprint K e Sprint J:

1. Confirmar que `TrailIntro`, `DemoRoom` e `RitualRoom` continuam visiveis e jogaveis.
2. Confirmar que a lanterna ainda e util, mas nao obrigatoria para enxergar tudo.
3. Andar/correr/agachar para sentir gait, headbob, ombro e step impact suavizado.
4. Usar `Q`/`R` para lean sem quebrar `E` de interacao.
5. Correr ate stamina baixa e confirmar `breath_heavy_01.ogg`.
6. Zerar stamina e confirmar `player_tired_01.ogg` sem spam.
7. Tomar dano na RitualRoom e confirmar camera shake curto.
8. Confirmar que morte/retry bloqueia e restaura input/feel/audio.

## Manutencao

Antes de commit/push: atualizar `docs/SPRINT_HISTORY.md`, verificar `.cursor/rules/pre-commit-docs.mdc` e usar commits convencionais quando aplicavel.
