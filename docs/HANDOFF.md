# BREU - Handoff

Ultima atualizacao: 2026-07-10

## Retomar

1. Ler `docs/START_HERE.md`.
2. Ler `docs/PROJECT_STATE.md`.
3. Ler `docs/SPRINT_HISTORY.md`.
4. Ler `docs/gameplay/NEXT_SPRINT_TASKS.md`.
5. Para direcao macro, ler `docs/design/GAME_VISION.md` e `docs/production/PHASE_01_02_SPRINT_PLAN.md`.

## Ultimas entregas

### Sprint K - Direcao Visual e Pipeline Grafico

- Criados os guias oficiais em `docs/visual/`.
- Criados presets iniciais em `resources/visual_profiles/`.
- Criado `VisualProfileApplier.cs` como ferramenta opcional, com `ApplyOnReady = false` por padrao.
- Criada a cena isolada `scenes/testing/VisualLookdevRoom.tscn`.
- `TrailIntro`, `DemoRoom` e `RitualRoom` receberam primeiro pass visual moderado.
- Regra da sprint: preservar jogabilidade e visibilidade; se escurecer demais, reduzir contraste/ambient/fog.
- Checkpoint antes da sprint visual: `147a284` (`chore: checkpoint before visual direction sprint`).

### Sprint Audio Docs - Direcao sonora oficial

- Criado `docs/audio/AUDIO_DIRECTION.md` como referencia oficial da identidade sonora.
- Criado `docs/audio/AUDIO_ASSET_REGISTRY.md` para registrar arquivos, status e proximas necessidades.
- A direcao sonora atual prioriza som seco, intimo, sujo e opressivo, com corpo/respiracao, silencio, ambiente nordestino macabro, radio/interferencia e inimigos humanos perturbados.
- Sprint futura planejada: `Sprint Audio 01 - Direcao sonora e pack realista da Fase 1`.
- Nenhum script, cena ou arquivo de audio foi alterado nesta etapa.

### Sprint J.5 - Body Motion procedural

- `PlayerBodyMotion.cs` e o sistema ativo no `Player.tscn`.
- `PlayerCameraFeel.cs` ficou como script legado/compatibilidade, mas nao esta instanciado no Player.
- Camera agora tem gait procedural, headbob vertical/horizontal, run roll, ombro, inercia e step impact visual.
- Lean visual: `Q` para esquerda, `R` para direita. `E` continua exclusivo para interacao.
- Lanterna e martelo recebem sway de mao via `Flashlight` e `WeaponHolder`, sem alterar bateria/toggle ou hit detection.
- `PlayerHealth.TakeDamage()` chama shake curto de camera via `PlayerBodyMotion.PlayDamageShake()`.
- `BreathAudio` foi adicionado ao Player; os audios de respiracao ainda sao futuros e o sistema apenas avisa uma vez se estiverem ausentes.
- `PlayerController` recebeu aceleracao/desaceleracao simples para mais peso no movimento.
- Documentos atualizados: `PLAYER_FEEL.md`, playtests e plano de sprint.

### Sprint J.6 - Corrida suavizada e respiracao

- Corrida do `PlayerBodyMotion` foi reduzida para evitar balanco lateral/roll excessivo.
- Valores ativos: `RunStepFrequency 9.2`, `RunBobVertical 0.055`, `RunBobHorizontal 0.020`, `RunRollAmount 1.65`, `RunPitchAmount 0.85`, `ShoulderSwayRunAmount 0.030`, `ShoulderRollRunAmount 1.25`, `WeaponRunSwayAmount 0.045`, `RunStepImpact 0.018`, `Smoothing 12.0`.
- Arquivos adicionados/importados:
  - `assets/audio/sfx/player/breath_light_01.ogg`
  - `assets/audio/sfx/player/breath_heavy_01.ogg`
  - `assets/audio/sfx/player/player_tired_01.ogg`
- `breath_light` toca em corrida com stamina acima de 35%.
- `breath_heavy` toca em stamina baixa.
- `player_tired` toca como one-shot ao zerar stamina/tentar correr sem stamina, com cooldown de 3s.
- Fallback de ajuste fino documentado: `RunBobHorizontal 0.014`, `RunRollAmount 1.2`, `ShoulderRollRunAmount 0.9`.

### Sprint J - Player Feel cinematografico

- Primeira versao de `PlayerCameraFeel.cs` criada com headbob, lean, sway e damage shake.
- Definicao oficial do estilo registrada em `GAME_VISION.md`.

### Sprint I - Hospede Seco blockout visual

- Arquivo visual: `assets/blender_exports/enemies/hospede_seco/enemy_hospede_seco_blockout.glb`.
- `EnemyPlaceholder.tscn` instancia o GLB em `Visual/HospedeSecoModel`.
- `BodyMesh`, `HeadMesh` e `Eyes` antigos ficam ocultos como fallback.
- A IA continua sendo `EnemyPlaceholderAI.cs`; nao houve refactor de comportamento.
- Colisao, `EnemyHurtbox`, `DetectionArea`, `AttackArea` e audios continuam iguais.
- `EnemyAnimationController.cs` adiciona animacoes placeholder por transform/tween: idle, walk, attack, hit, stunned e death placeholder.
- Ainda nao ha rig/bones nem animacoes finais do Blender.

### Sprint G - Morte, retry e respawn

- `PlayerHealth.cs` agora controla vida, invulnerabilidade curta, morte e reset.
- HUD mostra `Vida current/max`.
- `DamageOverlay` pisca vermelho ao tomar dano.
- `DeathScreen` mostra `VOCE MORREU` e botao `Tentar novamente`.
- `CheckpointManager` guarda checkpoint em memoria com cena, posicao/rotacao do player e snapshot do `GameSession`.
- `RespawnFromLastCheckpoint()` restaura o snapshot e recarrega a cena via `SceneTransition` quando disponivel.
- `PlayerSpawnResolver` reconhece retry e reposiciona o player no checkpoint salvo.
- `RespawnResolver` garante vida/input resetados quando a cena abre.
- `EnemyPlaceholderAI` para de perseguir/atacar quando o player esta morto.
- `TrailIntro`, `DemoRoom` e `RitualRoom` instanciam `DamageOverlay`, `DeathScreen` e `RespawnResolver`.

### Sprint F - Combate basico do martelo

- Criado `PlayerMeleeAttack.cs` no Player.
- Input `attack` no botao esquerdo.
- Ataque usa raycast curto da `Camera3D` como debug e hit volume esferico para contato corpo a corpo.
- Ao acertar `EnemyPlaceholderAI`, chama `ReceiveHit(HammerDamage)` e aplica stun.
- Durabilidade do martelo cai apenas quando acerta.
- Quando chega a 0, `GameSession` limpa a arma e o HUD volta para `Maos vazias`.
- Visual do martelo faz tween simples de golpe; audio de hit usa fallback se asset dedicado nao existir.
- Fix de hit detection: raycast agora colide com bodies/areas, usa mask ampla, imprime logs de debug e procura `EnemyPlaceholderAI` no collider ou nos parents.
- Fix adicional: se o raycast fino errar, o hit volume pode acertar inimigo proximo na frente do player.
- Fix adicional: `EnemyPlaceholder` agora tem `EnemyHurtbox` no grupo `enemy_hurtbox`, e o martelo ignora bilhete/chave/triggers ao procurar alvo.

### Persistencia simples de sessao

- Criado `GameSession` como autoload em `res://scripts/system/GameSession.cs`.
- Martelo Enferrujado agora persiste entre `DemoRoom` e `RitualRoom`.
- `PlayerWeaponController` sincroniza o Player novo com `GameSession` no `_Ready()`.
- HUD e visual do martelo na mao sao restaurados ao trocar de cena.
- `OldKeyPickup` marca `GameSession.HasOldKey` sem remover o martelo.

### Sprint E - IA basica do EnemyPlaceholder

- `EnemyPlaceholder.tscn` convertido para `CharacterBody3D`.
- Criado `EnemyPlaceholderAI.cs` com estados Dormant/Idle/Alert/Chasing/Attacking/Stunned.
- Criado `PlayerHealth.cs` em `Player.tscn`.
- `RitualRoomScareTrigger` agora chama `ActivateEnemy()`.
- Inimigo da RitualRoom persegue diretamente, toca respiracao/passos/growl e aplica dano simples.
- Stun preparado via `ApplyStun()` e `ReceiveHit()`.
- Ajuste de playtest: origem/capsula do `EnemyPlaceholder` alinhadas aos pes, spawn da RitualRoom movido para `Vector3(-0.85, 0.05, 2.45)` e spam de recuperacao de piso removido.
- `LockVerticalMovement` fica ligado no placeholder para impedir afundamento no chao ate a IA ter navmesh/colisoes finais.

### Sprint D - Sala dos Santos Secos

- Criada `scenes/levels/ritual_room/RitualRoom.tscn`.
- Instanciado `assets/blender_exports/ritual_room/sala_santos_secos_blockout.glb`.
- Reutilizado `Player.tscn`, HUD, NoteReaderUI, AudioManager e EnemyPlaceholder.
- Adicionadas colisoes temporarias de sala, mesa e porta de saida.
- Adicionadas luzes `CandleLightMain`, `BackAltarLight` e `RoomDarkFill`.
- Criados `RitualNoteInteractable.cs`, `OldKeyPickup.cs`, `RitualRoomScareTrigger.cs` e `RitualExitDoorTrigger.cs`.
- Porta final do corredor agora troca para `scenes/levels/ritual_room/RitualRoom.tscn`.
- Guia novo: `docs/testing/PLAYTEST_RITUAL_ROOM.md`.

### Decisao de fluxo - Fachada integrada na Trilha

- `TrailIntro.tscn` agora instancia o GLB visual da fachada em `Environment/HouseExteriorAtTrailEnd`.
- `DistantHouseSilhouette` ficou preservada, mas oculta.
- `HouseEntryTrigger` antigo ficou preservado, mas desativado.
- Nova entrada principal: `Interactables/EnterPensionDoor`.
- A porta da Pensao na trilha troca direto para `DemoRoom.tscn`.
- `HouseExterior.tscn` continua existindo apenas como cena isolada de teste/comparacao.

### Sprint C.5 - Costura da Fase 1

- Cena principal do projeto agora inicia em `TrailIntro.tscn`.
- `SceneTransition` ganhou `ChangeSceneWithFade(scenePath, message)` e mensagem opcional durante fade.
- Criado `PlayerSpawnResolver.cs` para posicionar o player nos marcadores de spawn ao abrir/trocar cena.
- Criado `CheckpointManager.cs` como autoload em memoria.
- Criados `OneShotMessageTrigger.cs` e `LightFlicker.cs`.
- Mensagens narrativas adicionadas na trilha, fachada e inicio do quarto.
- `EnterHouseTrigger` foi organizado como `EnterHouseDoor` na cena da fachada.
- Guia novo: `docs/testing/PLAYTEST_PHASE_01_FLOW.md`.

### Sprint C - Fachada da Pensao

- Criada `scenes/levels/house_exterior/HouseExterior.tscn`.
- Instanciado `assets/blender_exports/house_exterior/pensao_santa_luzia_exterior_blockout.glb`.
- Player reutilizado em `Vector3(0, 1, -8.5)`, no inicio do terreno da fachada, olhando para a porta.
- Colisoes temporarias: chao, paredes, varanda, blocker de porta e limites.
- Luzes: `MoonLight` e `FrontLanternLight`.
- Ambiencia: `Audio/ExteriorAmbience` com `wind_old_house_01.ogg`.
- Criados `EnterHouseTrigger.cs` e `BackToTrailTrigger.cs`.
- `EnterHouseTrigger` agora e interativo: mirar na porta e apertar `E` para ir ao Quarto 07.
- Historico: `HouseEntryTrigger.cs` ja foi usado para `TrailIntro -> HouseExterior`; no fluxo atual ele esta desativado na trilha.
- Escondidos na cena da trilha os meshes `house_placeholder_*` do GLB antigo.
- Criada `DistantHouseSilhouette` em `TrailIntro.tscn`, silhueta simples e sem colisao para servir como objetivo visual distante.
- Criado guia `docs/testing/PLAYTEST_HOUSE_EXTERIOR.md`.
- Atualizado `docs/testing/PLAYTEST_TRAIL_INTRO.md`.

### Fluxo inicial

```text
TrailIntro -> DemoRoom -> RitualRoom
```

### Demo Room / Fase 1

- Player em primeira pessoa com stamina, agachamento, pulo, lanterna e passos por superficie.
- Quarto 07 com bilhete, martelo, porta do quarto, corredor, susto, porta final e transicao para `RitualRoom.tscn`.

## Testar fluxo completo

1. Abrir `res://scenes/levels/trail_intro/TrailIntro.tscn`.
2. Rodar com F6.
3. Caminhar ate a casa, em direcao a `Z negativo`.
4. Confirmar que a fachada real aparece no fim da trilha.
5. Mirar na porta e apertar `E` no prompt `[E] Entrar na Pensao`.
6. Confirmar transicao para `DemoRoom.tscn`.
7. Abrir a porta do Quarto 07, atravessar o corredor e disparar o susto.
8. Mirar na porta final e apertar `E`.
9. Confirmar transicao para `RitualRoom.tscn`.
10. Disparar o susto da RitualRoom, deixar o inimigo matar o player e testar `Tentar novamente`.
11. Confirmar Sprint J.5: gait/headbob, ombro correndo, lean `Q`/`R`, sway da lanterna/martelo, shake de dano e retry restaurando controle.

Guias:

- `docs/testing/PLAYTEST_TRAIL_INTRO.md`
- `docs/testing/PLAYTEST_HOUSE_EXTERIOR.md`
- `docs/testing/PLAYTEST_DEMO_ROOM.md`
- `docs/testing/PLAYTEST_PHASE_01_FLOW.md`
- `docs/testing/PLAYTEST_RITUAL_ROOM.md`

## Validacao feita

- `dotnet build BREU.sln`: sucesso, 0 erros.
- Godot editor headless importou o GLB da fachada.
- Sprint K: `dotnet build BREU.sln`: sucesso, 0 erros.

Observacao: o editor headless emite erros de cache/config em `AppData` nesta maquina, mas importa os recursos do projeto.

## Proximo recomendado

1. Playtestar manualmente o visual de `TrailIntro`, `DemoRoom` e `RitualRoom`.
2. Se alguma cena ficou escura demais, aumentar levemente ambient/fill antes de mexer em contraste/fog.
3. Testar morte/retry na `RitualRoom.tscn`.
4. Balancear dano, vida e invulnerabilidade.
5. Criar objetivo com Chave Velha para liberar a saida.
