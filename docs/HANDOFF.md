# BREU - Handoff

Ultima atualizacao: 2026-07-09

## Retomar

1. Ler `docs/START_HERE.md`.
2. Ler `docs/PROJECT_STATE.md`.
3. Ler `docs/SPRINT_HISTORY.md`.
4. Ler `docs/gameplay/NEXT_SPRINT_TASKS.md`.
5. Para direcao macro, ler `docs/design/GAME_VISION.md` e `docs/production/PHASE_01_02_SPRINT_PLAN.md`.

## Ultimas entregas

### Sprint B - Trilha Noturna

- Criada `scenes/levels/trail_intro/TrailIntro.tscn`.
- Instanciado `assets/blender_exports/trail_intro/trail_intro_blockout.glb`.
- Reutilizado `scenes/player/Player.tscn` no inicio da trilha, em `Vector3(0, 1, 14)`.
- Criadas colisoes temporarias: `TrailFloorCollision`, `LeftFenceBlocker`, `RightFenceBlocker`.
- Criado `scripts/levels/HouseEntryTrigger.cs`.
- Adicionado `HouseEntryTrigger` perto da casa, em `Vector3(0, 1, -14.8)`.
- Adicionadas luzes temporarias: `MoonLight` e `DistantHouseLight`.
- Adicionado `Audio/TrailAmbience` usando `wind_old_house_01.ogg`.
- O import do vento esta com loop ligado.
- Criado guia `docs/testing/PLAYTEST_TRAIL_INTRO.md`.

### Demo Room / Fase 1

- Player em primeira pessoa com stamina, agachamento, pulo, lanterna e passos por superficie.
- Quarto 07 com bilhete, martelo, porta do quarto, corredor, susto, porta final e transicao para `RitualRoom.tscn`.

## Fluxos atuais

```text
TrailIntro -> chegada na casa (mensagem HUD, sem transicao ainda)
```

```text
Quarto 07 -> porta do quarto -> corredor -> susto -> porta final -> RitualRoom
```

## Testar Trilha Noturna

1. Abrir `res://scenes/levels/trail_intro/TrailIntro.tscn`.
2. Rodar com F6.
3. Confirmar que o player nasce em `Z 14`.
4. Caminhar ate a casa em `Z negativo`.
5. Confirmar que o player nao cai no vazio e nao atravessa as laterais facilmente.
6. Confirmar audio de vento.
7. Entrar no `HouseEntryTrigger` perto de `Z -14.8`.
8. Esperado: console imprime `Chegada a Pensao Santa Luzia.` e HUD mostra `A casa parece estar esperando por voce.`

Guia completo: `docs/testing/PLAYTEST_TRAIL_INTRO.md`.

## Validacao feita

- `dotnet build BREU.sln`: sucesso, 0 erros.
- Godot editor headless importou o GLB da trilha e o OGG de vento.

Observacao: a carga direta por `--scene res://scenes/levels/trail_intro/TrailIntro.tscn --quit` derrubou o executavel headless Godot 4.7 nesta maquina. O projeto abriu/importou sem erro via `--headless --editor --path . --quit`. Validar jogabilidade manualmente no editor com F6.

## Proximo recomendado

1. Abrir `TrailIntro.tscn` no editor e ajustar escala/orientacao fina do GLB versus colisoes.
2. Criar `HouseExterior.tscn`.
3. Trocar o TODO do `HouseEntryTrigger` por transicao real para a fachada.
4. Refinar laterais da trilha com bloqueios mais organicos.
