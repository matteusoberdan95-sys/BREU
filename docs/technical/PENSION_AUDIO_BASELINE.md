# Baseline — Áudio ambiente (Pensão)

**Versão:** 1.0  
**Sprint:** 16  
**Data:** 2026-07-11  
**Cena:** `res://scenes/levels/pensao_santa_luzia/PensaoVerticalBlockout01.tscn`

---

## Arquitetura

| Peça | Papel |
|------|--------|
| `PensionAudioManager` | Loops, crossfade, one-shots, prioridade de zona |
| `AmbienceZone3D` | `Area3D` (mask player 16); sem colisão física |
| `OneShotAudioTrigger3D` | Trigger espacial opcional de one-shot |
| Buses | `Master` → `Ambience` / `SFX` / `UI` (`default_bus_layout.tres`) |

**Não altera:** player, HUD, fog, atmosfera visual, layout, portas, puzzle.

---

## Regras

1. **Asset faltante:** `ResourceLoader.Exists` + warning; nunca crash por null.
2. **Crossfade:** 1,0–2,5 s; um loop principal dominante; sem stack infinito.
3. **Prioridade de zona:** deposit > second_floor > stairwell > ground_corridor > reception > exterior.
4. **Volumes conservadores:** ambience ≈ −18…−15 dB; lamp buzz ≈ −22 dB; one-shots ≈ −14…−12 dB.
5. **Áudio sutil:** sem jumpscare alto, sem spam, sem inimigo/combat/chase.
6. **Eventos Sprint 15:** one-shots opcionais; mensagem + flicker continuam se o som faltar.

---

## Ambience IDs

| ID | Arquivo | Volume alvo |
|----|---------|-------------|
| `exterior` | `exterior_night_wind_loop.ogg` | −16 dB |
| `reception` | `pension_reception_ambience_loop.ogg` | −18 dB |
| `ground_corridor` | `pension_ground_corridor_loop.ogg` | −17 dB |
| `deposit` | `pension_deposit_room_loop.ogg` | −16 dB |
| `second_floor` | `pension_second_floor_loop.ogg` | −15 dB |
| `lamp_buzz` | `lamp_buzz_loop.ogg` (secundário) | −22 dB |

---

## Playtest

`docs/testing/PENSION_AMBIENCE_AUDIO_PLAYTEST.md`
