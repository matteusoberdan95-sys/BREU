# Baseline — Áudio ambiente (Pensão) v2.1

**Versão:** 2.1  
**Sprint:** 16 / 16B  
**Data:** 2026-07-11  
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
5. **Áudio do player não altera movimento** — `PlayerFootstepAudio` só lê estado; nunca escreve `Velocity` / `Position` / stamina / câmera.
6. Passos wired (Sprint 16B). Respiração permanece **cadastrada, não wired** (sprint futura).
7. F7 = Audio Debug Mode; teclas 1–8 testam grupos sem depender da rota.

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

## Passos — cadência / volume

| Estado | Intervalo | Volume |
|--------|-----------|--------|
| Walk wood | 0,48 s | −12 dB |
| Walk dirt | 0,48 s | −10 dB |
| Run | 0,30 s | −8 dB |
| Crouch | 0,70 s | −18 dB |

Pitch random 0,94–1,06; sample aleatório sem repetir o último; sem passo se speed &lt; 0,2 ou no ar.

---

## Playtest

`docs/testing/PENSION_AUDIO_FUNCTIONAL_PLAYTEST.md`  
`docs/testing/PENSION_AMBIENCE_AUDIO_PLAYTEST.md` (ambience Sprint 16)
