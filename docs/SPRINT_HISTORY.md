# BREU — Histórico de sprints

Registro cronológico das sprints do reboot greenfield.

---

## Sprint 00 — Auditoria e limpeza

**Data:** 2026-07-11  
**Commit:** `78f4edd`  
**Objetivo:** Baseline Greenfield — remover gameplay antiga, documentar reboot.

**Entregas:**
- Decisão Opção A em `REBOOT_BASELINE_DECISION.md`
- Master plan, roadmap, DoD, agent roles
- 352 arquivos de gameplay removidos
- Branch `reboot/breu-clean-start`

---

## Sprint 01 — Fundação Godot mínima

**Data:** 2026-07-11  
**Commit:** `88b3d86`

**Entregas:**
- `project.godot`, `BREU.csproj`, `BREU.sln`, `GlobalUsings.cs`
- `scenes/levels/BootstrapEmpty.tscn`
- Estrutura de pastas vazia
- Input map placeholder

---

## Sprint 02 — Player Controller limpo

**Data:** 2026-07-11  
**Commit:** `d26bd7f`  
**Objetivo:** Movimentação FPS confiável com stamina, agachar e lanterna base.

**Entregas:**
- `scenes/player/Player.tscn`
- Scripts: `PlayerController`, `PlayerLook`, `PlayerStamina`, `PlayerCrouch`, `PlayerFlashlight`
- `scenes/test/PlayerMovementLab.tscn` + builder
- `docs/testing/PLAYER_MOVEMENT_LAB_PLAYTEST.md`
- Política de commit/push por sprint

---

## Próxima

**Sprint 03 — HUD e Debug**
