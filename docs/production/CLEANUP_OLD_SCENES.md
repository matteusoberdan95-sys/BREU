# Limpeza M.3.2 — Hard Reset da Vertical Slice

**Data:** 2026-07-11  
**Branch:** `cleanup/hard-reset-level-scenes-m32`

## Problema resolvido

- Cenas antigas deletadas de vez (sem `_archive`)
- GLB reexportado com **BEVEL + WEIGHTED_NORMAL aplicados** (corrige cantos quadrados no Godot)
- Cena Godot recriada do zero com Label3D para placas
- Cache de import antigo limpo

## Cena oficial

| Tipo | Caminho |
|------|---------|
| Blender | `assets/blender/trail_intro_pensao_vertical_slice_v01.blend` |
| GLB | `assets/models/levels/pensao_santa_luzia/pensao_santa_luzia_vertical_slice_v01.glb` |
| Godot | `scenes/levels/pensao_santa_luzia/PensaoSantaLuziaVerticalSlice.tscn` |
| Exportador | `tools/blender/export_vertical_slice_official.py` |

## Nota critica sobre o .blend externo

O arquivo `quarto-07/trail_intro_pensao_vertical_slice_v01.blend` recebido continha apenas Cube/Camera/Light (cena padrao, 96 KB).
O script de exportacao detectou isso e **reconstruiu** o blend oficial a partir de `quarto-07/trail_intro_pensao_integrada_v01.blend`,
filtrando somente collections validas (sem M01/HeightFix, Fog, Guides, Lights, Cameras).
O blend reconstruido foi salvo em `assets/blender/trail_intro_pensao_vertical_slice_v01.blend` (663 KB).

**Acao necessaria do usuario:** salvar o trabalho real no Blender como `trail_intro_pensao_vertical_slice_v01.blend` em quarto-07.

---

# Limpeza de cenas antigas — Sprint M.3.1

**Data:** 2026-07-11  
**Branch:** `cleanup/delete-old-scenes-keep-vertical-slice`

## Motivo

O projeto acumulou varias tentativas quebradas da trilha, fachada, pensao integrada e correcoes temporarias (barranco, fog cards, height fix, colisoes debug). Essas cenas geravam regressao visual, referencias mortas e confusao no editor. A partir desta sprint, a fase inicial tem **uma unica base oficial**.

## Cena oficial atual

| Tipo | Caminho |
|------|---------|
| Cena Godot | `scenes/levels/pensao_santa_luzia/PensaoSantaLuziaVerticalSlice.tscn` |
| Blender | `assets/blender/trail_intro_pensao_vertical_slice_v01.blend` |
| GLB | `assets/models/levels/pensao_santa_luzia/pensao_santa_luzia_vertical_slice_v01.glb` |
| Exportador | `tools/blender/export_pensao_vertical_slice.py` |

**Cena principal do projeto:** `PensaoSantaLuziaVerticalSlice.tscn` (F5/F6).

## Aviso

**Nao reutilizar** cenas, GLBs ou scripts antigos da trilha/pensao. Eles foram **deletados**, nao arquivados.

## Cenas deletadas

- `scenes/levels/trail_intro/TrailIntro.tscn`
- `scenes/levels/house_exterior/HouseExterior.tscn`
- `scenes/levels/pensao_santa_luzia/PensaoSantaLuziaIntegratedTest.tscn`
- `scenes/levels/corridor/Corridor.tscn`
- `scenes/levels/phase_02/RitualRoom.tscn` (duplicata)
- `_archive/old_pensao_attempts/` (pasta inteira)

## Assets deletados

- `assets/blender_exports/trail_intro/trail_intro_blockout.glb` (+ `.import`)
- `assets/blender_exports/house_exterior/pensao_santa_luzia_exterior_blockout.glb` (+ `.import`)
- `_archive/old_pensao_attempts/assets/trail_intro_pensao_integrada_v01/`
- `_archive/old_pensao_attempts/source_receipts/`

## Scripts deletados

- `scripts/visual/TrailIntroVisualPass.cs`
- `scripts/fx/TrailFogCardDrift.cs`
- `scripts/fx/FogCard3D.cs`
- `scripts/levels/EnterHouseTrigger.cs`
- `scripts/levels/HouseEntryTrigger.cs`
- `scripts/levels/BackToTrailTrigger.cs`
- `_archive/old_pensao_attempts/scripts/PensaoMansionBlockoutBuilder.cs`
- `_archive/old_pensao_attempts/scripts/PensaoVerticalSliceVisualFilter.cs`

## Cenas/tools de teste deletados

- `scenes/fx/FogCard3D.tscn`
- `tools/dump_trail_nodes.gd` / `dump_trail_nodes_runner.gd`
- `tools/depth_fog_visual_test.gd` / `depth_fog_visual_test_runner.gd`
- `tools/overlay_visual_test.gd` / `overlay_visual_test_runner.gd`
- `_archive/old_pensao_attempts/tools/validate_pensao_scene.gd`
- `_archive/old_pensao_attempts/tools/export_pensao_santa_luzia.py`

## Documentacao de tentativas antigas deletada

- `docs/testing/PLAYTEST_TRAIL_INTRO.md`
- `docs/testing/PLAYTEST_HOUSE_EXTERIOR.md`
- `docs/testing/PLAYTEST_PHASE_01_FLOW.md`
- `docs/visual/TRAILINTRO_APPROVED_LOOK.md`
- `_archive/old_pensao_attempts/docs/PENSAO_SANTA_LUZIA_PLAYTEST.md`

## Sistemas preservados

- `Player.tscn` e scripts do player (movimento, stamina, lanterna, combate, camera feel)
- `HUD.tscn`, `DamageOverlay`, `DeathScreen`
- `DemoRoom.tscn`, `RitualRoom.tscn` (salas de teste uteis)
- Shaders aprovados: `shaders/fx/depth_fog_postprocess.gdshader`, `DepthFogPostProcess.cs`
- Inimigos, audio, checkpoints, `GameSession`, `SceneTransition`
- `project.godot`, assets globais nao relacionados a trilha/pensao antiga

## Nota sobre o .blend externo

O arquivo `quarto-07/trail_intro_pensao_vertical_slice_v01.blend` recebido continha apenas a cena padrao do Blender (Cube/Camera/Light). O GLB valido de `quarto-07/trail_intro_pensao_vertical_slice_v01.glb` foi usado como fonte para reconstruir o `.blend` oficial organizado via `export_pensao_vertical_slice.py` (8 guides removidos na exportacao).

## Pendencias

- [ ] Playtest manual F6: subir rampa da escada com Player real
- [ ] Ajustar inclinacao/altura da rampa se necessario apos playtest
- [ ] Reconectar fluxo completo `Vertical Slice -> DemoRoom -> RitualRoom` quando a costura for retomada
