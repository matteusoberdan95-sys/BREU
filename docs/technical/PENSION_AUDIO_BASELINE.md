# Baseline — Áudio ambiente (Pensão) v2

**Versão:** 2.0  
**Sprint:** 16  
**Data:** 2026-07-11  
**Cena:** `res://scenes/levels/pensao_santa_luzia/PensaoVerticalBlockout01.tscn`  
**Pack:** `assets/audio/pensao/` (v2)

---

## Arquitetura

| Peça | Papel |
|------|--------|
| `PensionAudioManager` | Loops, crossfade, one-shots, flashlight clicks, catálogo Sprint 17 |
| `AmbienceZone3D` | `Area3D` mask player 16; sem colisão física |
| `OneShotAudioTrigger3D` | Trigger espacial opcional |
| Buses | Master / Ambience / SFX / UI (`default_bus_layout.tres`) |

**Não altera:** player movement, HUD, fog, WorldEnvironment, layout, portas, puzzle.

---

## Regras

1. Asset faltante → warning único; nunca crash.
2. Usar arquivo real se existir (sem placeholder).
3. Crossfade 1,0–2,5 s; prioridade deposit > second_floor > stairwell > corridor > reception > exterior.
4. Volumes conservadores; sem jumpscare alto.
5. Passos/respiração **cadastrados** nesta sprint; **wired na Sprint 17**.

---

## Zonas

| Zona | Ambience | Secundário |
|------|----------|------------|
| ExteriorTrail | exterior | — |
| Reception | reception | lamp_buzz |
| GroundCorridor / Room102 / Kitchen | ground_corridor | — |
| Deposit | deposit | water_drops |
| Stairwell | ground_corridor | — |
| SecondFloor | second_floor | lamp_buzz |

---

## Playtest

`docs/testing/PENSION_AMBIENCE_AUDIO_PLAYTEST.md`
