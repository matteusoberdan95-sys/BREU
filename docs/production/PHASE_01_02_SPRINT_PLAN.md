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

## Sprint F - Combate basico com Martelo Enferrujado

**Status:** base criada.

### Objetivos

- Ataque basico com martelo
- Raycast de melee
- Durabilidade da arma
- Stun do inimigo
- HUD atualizado apos cada hit/quebra

### Godot

- [x] `PlayerMeleeAttack.cs` no Player
- [x] Input `attack` no botao esquerdo
- [x] Raycast curto a partir da `Camera3D`
- [x] Hit em `EnemyPlaceholderAI.ReceiveHit`
- [x] Stun do inimigo via `ApplyStun`
- [x] Reducao de durabilidade no `GameSession`
- [x] HUD volta para `Maos vazias` quando o martelo quebra
- [x] Feedback visual simples do martelo com tween

### Proximos ajustes

- Adicionar animacao final de ataque.
- Adicionar sons dedicados `weapon_swing_01.ogg` e `weapon_hit_01.ogg`.
- Integrar custo de stamina ao ataque.
- Refinar alcance e timing apos playtest.

---

## Sprint G - Morte, retry e respawn por checkpoint

**Status:** base jogavel criada.

### Objetivos

- Finalizar `PlayerHealth` para o loop atual.
- Mostrar vida no HUD.
- Adicionar feedback visual de dano.
- Criar tela de morte com retry.
- Recarregar a cena do ultimo checkpoint.
- Restaurar snapshot simples do `GameSession`.
- Parar o inimigo quando o player morrer.

### Godot

- [x] `PlayerHealth.cs` com `TakeDamage`, `Heal`, `Kill` e `ResetHealth`.
- [x] HUD mostra `Vida 100/100`.
- [x] `DamageOverlay.tscn` com flash vermelho curto.
- [x] `DeathScreen.tscn` com botao `Tentar novamente`.
- [x] `CheckpointManager` guarda cena, posicao/rotacao e snapshot de arma/chave.
- [x] `RespawnResolver.cs` garante input/vida resetados ao abrir cena.
- [x] `EnemyPlaceholderAI` para de perseguir/atacar player morto.
- [x] Cenas `TrailIntro`, `DemoRoom` e `RitualRoom` instanciam overlay/tela de morte.

### Proximos ajustes

- Criar save em disco futuramente.
- Melhorar transicao visual/audio da morte.
- Ajustar balanceamento de dano e vida apos playtest.
- Criar checkpoints por Area3D quando a fase tiver ramificacoes.

---

## Sprint H - Polimento de terror e balanceamento da RitualRoom

**Status:** concluida (base jogavel).

### Objetivos

- Polimento de terror na Sala dos Santos Secos.
- Balanceamento do `EnemyPlaceholder`.
- Feedback de dano no player e hit no inimigo.
- Flicker de luz controlado (`LightFlicker`).
- Audio ambiente e de inimigo.
- Death screen melhorada.
- Console limpo (sem spam de debug).

### Godot

- [x] `EnemyPlaceholderAI` com exports de balanceamento revisados.
- [x] `PlayerMeleeAttack` com cooldown 0.85s, radius 0.8, debug desligado.
- [x] `DamageOverlay` com flash, som de dano e tremor de camera.
- [x] `DeathScreen` com fade in e stinger de morte.
- [x] `RitualRoomScareTrigger` com sequencia cinematografica.
- [x] `LightFlicker` nas velas da sala.
- [x] `RitualExitDoorTrigger` com mensagens diferenciadas por chave.
- [x] `docs/design/RITUAL_ROOM_BALANCE.md` com valores atuais.

### Proximos ajustes

- Playtest manual do fluxo completo F5.
- Ajustar valores apos feedback de jogadores.
- Criar assets `player_hurt_01.ogg` e `death_stinger_01.ogg`.

---

## Sprint I - Hospede Seco blockout no Godot

**Status:** blockout visual e animacoes placeholder integrados.

So iniciar apos validar:

- escala;
- distancia;
- perseguicao;
- timing do susto;
- comportamento basico.
- combate basico com martelo.
- morte/retry/respawn.
- polimento da RitualRoom.

### Entregas Godot

- [x] `enemy_hospede_seco_blockout.glb` importado em `assets/blender_exports/enemies/hospede_seco/`.
- [x] `EnemyPlaceholder.tscn` instancia o modelo em `Visual/HospedeSecoModel`.
- [x] Visual placeholder antigo (`BodyMesh`, `HeadMesh`, `Eyes`) fica oculto como fallback.
- [x] IA mantida em `EnemyPlaceholderAI.cs`.
- [x] Colisao principal, `EnemyHurtbox`, ataque, stun e dano continuam usando a estrutura existente.
- [x] `EnemyAnimationController.cs` criado para animacoes visuais placeholder.
- [x] Animacoes basicas: idle, walk/chase, attack, hit, stunned e death placeholder.
- [x] Sem rig/bones nesta sprint.

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

### Proximos ajustes

- Validar escala/orientacao do modelo no editor.
- Criar rig simples.
- Criar animacoes Idle, Walk/Chase, Attack e Hit reaction.
- Trocar feedback de stun/tween por animacao quando houver rig.

---

## Sprint J - Player Feel cinematografico

**Status:** base jogavel integrada.

### Objetivos

- Tirar a sensacao de camera flutuando.
- Aproximar movimento/camera de horror em primeira pessoa cinematografico.
- Registrar oficialmente o estilo de BREU DE DENTRO.

### Entregas Godot

- [x] `PlayerCameraFeel.cs` criado para headbob, camera shake, lean e sway da lanterna.
- [x] `PlayerBodyMotion.cs` criado como versao ativa do body motion procedural.
- [x] Gait, headbob vertical/horizontal, shoulder sway, inercia e step impact visual.
- [x] `WeaponHolder` e `Flashlight` com sway de mao por estado.
- [x] `BreathAudio` adicionado ao `Player.tscn`.
- [x] Audios de respiracao integrados: `breath_light`, `breath_heavy` e `player_tired`.
- [x] Corrida suavizada para reduzir balanco lateral/roll excessivo.
- [x] `PlayerHealth.TakeDamage()` chama camera shake do `PlayerBodyMotion`.
- [x] Inputs `lean_left = Q` e `lean_right = R`.
- [x] Movimento recebeu aceleracao/desaceleracao simples para mais peso.
- [x] Crouch existente preservado e integrado ao headbob menor.
- [x] Documentacao oficial do estilo registrada em `GAME_VISION.md`.

### Proximos ajustes

- Playtestar intensidade da corrida suavizada; fallback leve documentado em `PLAYER_FEEL.md`.
- Adicionar custo de stamina ao ataque.
- Criar animacoes finais de arma/camera quando houver maos rigadas.

---

## Sprint K - Direcao Visual e Pipeline Grafico

**Status:** base concluida; ajustes finos dependem de playtest visual.

### Objetivos

- Documentar a direcao visual oficial do BREU.
- Definir paleta, materiais, iluminacao e pos-processamento.
- Criar presets visuais iniciais no Godot.
- Aplicar um primeiro pass visual seguro em `TrailIntro`, `DemoRoom` e `RitualRoom`.
- Criar guia de exportacao Blender -> Godot.
- Criar biblioteca inicial de materiais.
- Preparar o projeto para sair do blockout e entrar em visual polish.

### Entregas

- [x] `docs/visual/VISUAL_DIRECTION.md`
- [x] `docs/visual/GRAPHICS_PIPELINE.md`
- [x] `docs/visual/MATERIAL_LIBRARY.md`
- [x] `docs/visual/LIGHTING_GUIDE.md`
- [x] `docs/visual/POST_PROCESSING_GUIDE.md`
- [x] `docs/visual/REUSABLE_ASSET_GUIDE.md`
- [x] `docs/visual/BLENDER_TO_GODOT_EXPORT_GUIDE.md`
- [x] `docs/visual/LEVEL_STREAMING_PLAN.md`
- [x] `resources/visual_profiles/visual_profile_exterior_night.tres`
- [x] `resources/visual_profiles/visual_profile_room07.tres`
- [x] `resources/visual_profiles/visual_profile_ritual_room.tres`
- [x] `scripts/visual/VisualProfileApplier.cs`
- [x] `scenes/testing/VisualLookdevRoom.tscn`
- [x] Primeiro pass visual moderado nas cenas principais.

### Cuidados permanentes

- Antes de alterar cenas, verificar estado do Git e recomendar commit/checkpoint se houver mudancas nao salvas.
- Aplicar ajustes visuais com intensidade moderada, priorizando jogabilidade e visibilidade.
- `VisualProfileApplier` e opcional, controlado por exports, com `ApplyOnReady` desligado por padrao.

---

## Sprint K.1 — Aplicacao Visual Real nas Cenas

**Status:** concluida (2026-07-10).

### Objetivos

- Tornar as mudancas visuais **perceptiveis** no playtest (fog, lua, contraste, drama).
- Manter gameplay intacto (player, lanterna, HUD, inimigo, combate, checkpoint).
- Nao refazer cenarios nem trocar modelos.

### Entregas

- [x] TrailIntro: noite cinematografica (fog, MoonLight, luz quente da Pensao, SSAO/contraste).
- [x] DemoRoom: quarto mais opressivo (ambient baixo, DebugLight off, NoteSpotLight, RoomLightPoint mais quente).
- [x] RitualRoom: drama ritualistico (CandleLightMain na mesa, fill reduzido, inimigo na sombra).
- [x] Post-processamento leve (exposure, contrast, saturation, glow sutil, SSAO).
- [x] `VisualProfileApplier` mantido desligado (`ApplyOnReady = false`).
- [x] Documentacao em `LIGHTING_GUIDE.md` e `PLAYTEST_PHASE_01_FLOW.md`.

### Validacao

Playtest visual: `TrailIntro -> DemoRoom -> RitualRoom` + 3 screenshots (trilha, quarto, ritual).

---

## Sprint K.2 — Rebalanceamento de Iluminacao e Neblina

**Status:** concluida (2026-07-10).

### Motivo

K.1 ficou escura demais. Fog exponencial quase invisivel. Quarto 07 e trilha perdiam legibilidade sem lanterna.

### Entregas

- [x] TrailIntro: ambient/exposure subidos, MoonLight reforcada, fog **depth mode**, luz da Pensao ampliada.
- [x] DemoRoom: lâmpada do teto (`RoomLightPoint`) reforcada, ambient `0.11`, post-process menos agressivo.
- [x] RitualRoom: ambient/fill subidos mantendo velas como foco e inimigo na sombra.
- [x] Documentacao em `LIGHTING_GUIDE.md` e checklist K.2 em `PLAYTEST_PHASE_01_FLOW.md`.

### Validacao

Fluxo completo + confirmar navegacao **sem lanterna** nas tres salas principais.

---

## Sprint K.3 — Correção de Iluminação Prática e Neblina

**Status:** concluida (2026-07-10).

### Motivo

K.2 ainda deixou fog invisível, lâmpada do quarto sem efeito prático e áreas pretas demais.

### Entregas

- [x] `DemoRoomLightingSetup.cs` — alinha `RoomCeilingLight` ao `lamp_bulb` + emissão no bulbo.
- [x] `RoomCeilingLight` criada (energy 3.2, range 6.5); `RoomLightPoint` desativada.
- [x] TrailIntro: fog depth + **volumetric fog**; `HouseEntranceWarmLight` reforçada.
- [x] RitualRoom: velas/fill rebalanceados para legibilidade.
- [x] `ApplyOnReady = false` explícito nos três `VisualProfileApplier`.
- [x] Documentação K.3.

### Validacao

`TrailIntro -> DemoRoom -> RitualRoom` — quarto legível sem lanterna; fog perceptível na trilha.

---

## Sprint K.3.1 — Correção definitiva da neblina da TrailIntro

**Status:** concluida (2026-07-10).

### Motivo

Fog ainda invisível no playtest após K.3. Trilha parecia limpa demais.

### Entregas

- [x] WorldEnvironment reforçado (fog density `0.075`, height fog, volumetric `0.072`).
- [x] 3 `FogVolume` ao longo da trilha (`Near`, `Mid`, `Far`).
- [x] `TrailMistParticles` (GPUParticles3D sutil).
- [x] MoonLight `0.75`, `HouseEntranceWarmLight` `4.0`.
- [x] Documentação K.3.1.

### Validacao

F6/F5 na TrailIntro — fog deve ser **claramente visível** ao olhar para a Pensão.

---

## Sprint K.3.2 — Refino Realista da Neblina e Correção dos Quadrados da Lanterna

**Status:** concluida (2026-07-10).

### Motivo

Fog K.3.1 visivel, porem forte demais em alguns momentos; lanterna revelava **grade/quadrados** na neblina volumetrica.

### Entregas

- [x] Preset **Fog Medio** na TrailIntro (`density 0.055`, cor `#16222C`, depth `6–38`).
- [x] Height fog reduzido (`0.06`); FogVolumes rebalanceados (Mid `0.12` -> `0.065`).
- [x] Qualidade volumetrica global: `volume_size`/`volume_depth` `64` -> `96`, filtro ativo.
- [x] Lanterna: energy `2.9`, `light_volumetric_fog_energy 0.45`, range/angle reduzidos levemente.
- [x] Particulas menores e mais transparentes (`TrailMistParticles`).
- [x] MoonLight `0.65`; HouseEntranceWarmLight `4.2` / range `13`.
- [x] Presets Fog Leve/Medio/Forte documentados em `LIGHTING_GUIDE.md`.
- [x] Documentacao K.3.2.

### Validacao

F6 na TrailIntro — fog natural com e sem lanterna; quadrados mitigados; Pensao visivel; fluxo completo ate RitualRoom.

---

## Sprint K.3.3 — Neblina Hibrida e Remocao dos Artefatos em Grade

**Status:** concluida (2026-07-10).

### Motivo

K.3.2 melhorou a neblina, mas artefatos em **grade/quadrados** (froxel/volumetric) ainda perceptiveis com lanterna e sem ela.

### Entregas

- [x] Volumetric fog reduzida a base leve (`density 0.020`); depth fog como protagonista (`0.048`).
- [x] FogVolumes desativados; neblina organica via **4 sistemas de particulas** (A/B/C + ground).
- [x] Lanterna: volumetric energy `0.22`; luas/pensao com contribuicao volumetrica reduzida.
- [x] Qualidade volumetrica: `volume_size`/`volume_depth` `128`.
- [x] Documentacao K.3.3.

### Validacao

F6 — neblina hibrida visivel; grade muito reduzida; pensao e caminho legiveis.

---

## Sprint K.3.4 — Restaurar Neblina Visivel da TrailIntro

**Status:** concluida (2026-07-10).

### Motivo

K.3.3 eliminou quadrados, mas a neblina **sumiu** — trilha ficou limpa demais.

### Entregas

- [x] Fog global restaurada (`density 0.055`, depth `5–34`, height `0.055`).
- [x] Volumetric fog reforcada (`0.042`); FogVolumes reativados com density moderada.
- [x] Particulas A/B/C/Ground com alpha maior; `distance_fade` removido.
- [x] MoonLight `0.60`; HouseEntranceWarmLight `4.0` / range `12`.
- [x] Documentacao K.3.4.

### Validacao

F6 — neblina claramente visivel; trilha atmosferica; pensao legivel.

---

## Sprint K.3.5 — Remocao Definitiva dos Blocos de Neblina

**Status:** concluida (2026-07-10).

### Motivo

K.3.4 reativou FogVolumes e particulas — playtest mostrou **retangulos grandes** na neblina, especialmente olhando para a Pensao.

### Entregas

- [x] FogVolumes Near/Mid/Far **desativados** (visible + process disabled).
- [x] TrailMistParticles A/B/C/Ground **desativados** (visible + emitting false).
- [x] Volumetric fog **desligada**; neblina via depth + height fog only.
- [x] Lanterna: `light_volumetric_fog_energy = 0.0` (energy normal mantida).
- [x] Luces TrailIntro com volumetric contribution zerada.
- [x] Documentacao K.3.5.

### Validacao

F6 — sem quadrados grandes; fog sutil visivel; lanterna util; pensao legivel.

---

## Sprint K.3.6 — Fog Cards Cinematograficos sem Blocos

**Status:** concluida (2026-07-10).

### Motivo

Neblina instavel: FogVolumes/particles/volumetric geravam blocos; remover tudo apagava atmosfera.

### Entregas

- [x] `fog_card.gdshader` — alpha radial + noise, bordas invisiveis.
- [x] `FogCard3D.tscn` + `FogCard3D.cs` — billboard Y, parametros exportados.
- [x] 6 Fog Cards na TrailIntro (`FogCards/`).
- [x] Environment fog apenas como base sutil; volumetric off.
- [x] FogVolumes/particles antigas marcadas `DISABLED_DO_NOT_USE_BLOCKY_FOG`.
- [x] Documentacao K.3.6.

### Validacao

F6 — neblina visivel via cards; zero retangulos grandes; pensao e caminho ok.

---

## Sprint K.3.7 — Fog Cards com Textura PNG Estavel

**Status:** concluida (2026-07-10).

### Motivo

Ciclo K.3.x instavel. Solucao simples e estavel: Sprite3D + PNG com borda soft.

### Entregas

- [x] 4 Sprite3D (`FogCards/`) com `fog_soft_card_01–04.png`.
- [x] Environment fog base sutil; volumetric off; legacy desativado.
- [x] Instancias shader `FogCard3D.tscn` removidas da TrailIntro.
- [x] Cena validada Godot headless.
- [x] Documentacao K.3.7.

### Validacao

F6 — neblina PNG visivel; sem blocos; cena carrega.

---

## Sprint Audio 01 - Direcao sonora e pack realista da Fase 1

**Status:** planejada.

### Objetivos

- Revisar todos os sons existentes.
- Gerar/editar ambience loops.
- Criar sons do player.
- Criar sons do Hospede Seco.
- Criar sons de portas/interacoes.
- Criar radio/interferencia.
- Balancear volumes no Godot.

### Entregas esperadas

- `docs/audio/AUDIO_DIRECTION.md` como direcao sonora oficial.
- `docs/audio/AUDIO_ASSET_REGISTRY.md` atualizado com status real.
- Pack realista v02 com nomes canonicos.
- Teste de mix no fluxo `TrailIntro -> DemoRoom -> RitualRoom`.

---

---

## Sprint L — Pass Visual Real da Trilha + Fachada da Pensao

**Data:** 2026-07-11 | **Escopo:** materiais, props, composicao, luz quente da Pensao.

**NAO MEXER:** `DepthFogPostProcess`, preset **TrailIntro Fog Approved**.

**Entregas:**
- 12 materiais `materials/environment/trail/` e `house/`
- `TrailIntroVisualPass.cs`
- Props duplicados + grupos fachada
- `docs/visual/TRAILINTRO_APPROVED_LOOK.md`

---

## Ordem recomendada

```text
Sprint A -> Sprint B -> Sprint C -> Sprint D -> Sprint E -> Sprint F -> Sprint G -> Sprint H -> Sprint I -> Sprint J -> Sprint K -> Sprint L -> Sprint Audio 01
```

## Documentos relacionados

- `docs/design/GAME_VISION.md`
- `docs/design/PHASE_01_LEVEL_DESIGN.md`
- `docs/design/PHASE_02_LEVEL_DESIGN.md`
- `docs/design/ENEMY_DESIGN.md`
- `docs/design/SCENARIO_ART_DIRECTION.md`
- `docs/visual/VISUAL_DIRECTION.md`
- `docs/visual/GRAPHICS_PIPELINE.md`
- `docs/audio/AUDIO_DIRECTION.md`
- `docs/audio/AUDIO_ASSET_REGISTRY.md`
- `docs/SPRINT_HISTORY.md`
- `docs/gameplay/NEXT_SPRINT_TASKS.md`
