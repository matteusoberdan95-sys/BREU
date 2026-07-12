# Playtest — Áudio ambiente base (Sprint 16)

**Cena:** `res://scenes/levels/pensao_santa_luzia/PensaoVerticalBlockout01.tscn`  
**Sprint:** 16 — Áudio ambiente base da Pensão  
**Data:** 2026-07-11  
**Baseline:** `docs/technical/PENSION_AUDIO_BASELINE.md`

---

## Status

**Implementado** — playtest F6 pendente de aprovação do usuário.

---

## Assets

- **Presentes:** todos os 14 `.ogg` em `assets/audio/pensao/`
- **Faltantes:** nenhum
- Lista: `docs/audio/PENSION_AUDIO_ASSET_LIST.md`

---

## Zonas

| Zona | Ambience | Prioridade |
|------|----------|------------|
| AudioZone_ExteriorTrail | exterior | 10 |
| AudioZone_Reception | reception (+ lamp_buzz) | 40 |
| AudioZone_GroundCorridor | ground_corridor | 50 |
| AudioZone_Room102 | ground_corridor | 45 |
| AudioZone_Kitchen | ground_corridor | 45 |
| AudioZone_Deposit | deposit | 80 |
| AudioZone_Stairwell | ground_corridor | 60 |
| AudioZone_SecondFloor | second_floor (+ lamp_buzz) | 70 |

---

## One-shots (eventos Sprint 15)

| Evento | Som |
|--------|-----|
| pension_entry_first_time | old_house_settle_01 |
| key_pickup_tension | wood_creak_02 |
| fuse_pickup_footsteps | distant_step_01 → distant_step_02 |
| stair_first_arrival | old_house_settle_02 |
| upper_corridor_presence | distant_knock_01 |
| upper_locked_door_hint | door_scratch_01 |

---

## Volumes

- Ambience: −18…−15 dB
- Lamp buzz: −22 dB
- One-shots: −14…−12 dB
- Crossfade: ~1,6 s

---

## Rota F6

- [ ] Trilha → exterior
- [ ] Entrada → recepção + settle
- [ ] Chave → creak
- [ ] Depósito → ambience deposit
- [ ] Fusível → passos distantes
- [ ] Escada / 2º andar → second_floor
- [ ] Presença / porta → knock / scratch
- [ ] Voltar térreo → crossfade OK

---

## Regressão

- [ ] Player / HUD / lanterna / F10/F11
- [ ] Atmosfera / fog
- [ ] Puzzle
- [ ] Eventos Sprint 15
- [ ] Sem portas/placas novas
- [ ] Sem inimigo/combat/chase
- [ ] Sem crash por asset null
- [ ] Volumes confortáveis

---

## Bugs conhecidos

Nenhum na implementação. Registrar após F6.
