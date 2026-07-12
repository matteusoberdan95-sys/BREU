# Baseline — Áudio ambiente (Pensão) v2.1

**Versão:** 2.4  
**Sprint:** 16 / 16B–16E  
**Data:** 2026-07-12  
**Cena:** `res://scenes/levels/pensao_santa_luzia/PensaoVerticalBlockout01.tscn`  
**Pack:** `assets/audio/pensao/` (v2)

---

## Arquitetura

| Peça | Papel |
|------|--------|
| `PensionAudioManager` | Loops, crossfade, one-shots, flashlight, F7 debug, setup de zonas |
| `AmbienceZone3D` | `Area3D` mask player 16; sem colisão física |
| `SurfaceAudioZone3D` | Tipo de superfície (Wood / DirtGravel) para passos |
| `PlayerFootstepAudio` | Passos do player (audio-only; lê Velocity / IsOnFloor / sprint / crouch) |
| `PlayerBreathingAudio` | Respiração normal + panting (audio-only; lê sprint/stamina/speed) |
| `RandomOneShotEmitter3D` | Gotas one-shot aleatórias no depósito/cozinha |
| `OneShotAudioTrigger3D` | Trigger espacial opcional |
| Buses | Master / Ambience / SFX / UI (`default_bus_layout.tres`) |

**Não altera:** player movement, HUD, fog, WorldEnvironment, layout, portas, puzzle.

---

## Regras

1. Asset faltante → warning único; nunca crash.
2. Usar arquivo real se existir (sem placeholder).
3. Crossfade 1,0–2,5 s; prioridade deposit > second_floor > stairwell > corridor > reception > exterior.
4. Volumes conservadores; sem jumpscare alto.
5. **Áudio do player não altera movimento** — `PlayerFootstepAudio` / `PlayerBreathingAudio` só leem estado; nunca escrevem `Velocity` / `Position` / stamina / câmera.
6. Passos (16B–16D). Respiração wired (16E).
7. F7 = Audio Debug; teclas 1–8 SFX; **9** breath one-shot; **0** toggle panting teste.
8. Corrida usa o mesmo banco da superfície; `player_run_step_*` e `*_sequence` **não** são usados como footstep.
9. **Um único sistema** toca passos: `PlayerFootstepAudio`. Respiração: `PlayerBreathingAudio`.

---

## Zonas de ambience

| Zona | Ambience | Secundário |
|------|----------|------------|
| ExteriorTrail | exterior | — |
| Reception | reception | lamp_buzz |
| GroundCorridor / Room102 | ground_corridor | — |
| Kitchen | ground_corridor | water_drops |
| Deposit | deposit | water_drops (~−17 dB) |
| Stairwell | ground_corridor | — |
| SecondFloor | second_floor | lamp_buzz |

## Zonas de superfície (passos)

| Zona | SurfaceType | Prioridade |
|------|-------------|------------|
| `SurfaceAudio_Exterior_DirtGravel` | DirtGravel | 20 |
| `SurfaceAudio_Pension_Wood` | Wood | 40 |
| `SurfaceAudio_SecondFloor_Wood` | Wood | 50 |

Default sem zona: **Wood**.

## Passos — cadência / volume (Sprint 16D)

**Auditoria:** apenas `PlayerFootstepAudio` no `Player.tscn` dispara passos do player. Sem segundo sistema.

**Regra de samples:** somente one-shots individuais da superfície.  
**Não usar:** `player_footsteps_*_sequence`, `player_running_sequence`, `player_run_step_*` (reservados).

| Estado | Intervalo | Volume | Pitch |
|--------|-----------|--------|-------|
| Walk wood | 0,64 s | −12 dB | 0,98–1,03 |
| Walk dirt | 0,64 s | −11 dB | 0,98–1,03 |
| Run wood | 0,36 s | −9 dB | 0,99–1,04 |
| Run dirt | 0,36 s | −8 dB | 0,99–1,04 |
| Crouch | 0,85 s | −18 dB | 0,96–1,01 |

`MinimumStepCooldown` = **0,28 s** (impede duplo disparo).  
Um estado por frame: Idle / Crouch / Run / Walk. Um timer. Um `AudioStreamPlayer`. Stop antes de Play.

Bancos:
- DirtGravel → `player_footstep_dirt_gravel_01`…`12` (walk e run)
- Wood → `player_footstep_wood_01`…`08` (walk e run)
- Fallback → Wood

Com F7 ON: `[Footstep] state=… surface=… interval=… sample=…`

## Respiração — Sprint 16E

`PlayerBreathingAudio` no Player (bus **SFX**, `AudioStreamPlayer` 2D).

| Camada | Asset | Volume | Gatilho |
|--------|-------|--------|---------|
| Normal loop | `player_breath_heavy_loop` | −32 dB (walk −30) | Sempre (vivo) |
| Panting loop | `player_panting_loop` | −18 / −14 low stamina | Sprint ≥ 2 s; recover 2–5 s |
| One-shots | `player_breath_heavy_01`…`04` | −16 dB | Fim de corrida longa / low stamina / debug 9 |

Fade in panting ~1,2 s; fade out ~3 s. Não altera `PlayerController` / stamina.

Com F7 ON: `[Breathing] state=…`

---

## Playtest

`docs/testing/PENSION_AUDIO_FUNCTIONAL_PLAYTEST.md`  
`docs/testing/PENSION_AMBIENCE_AUDIO_PLAYTEST.md` (ambience Sprint 16)
