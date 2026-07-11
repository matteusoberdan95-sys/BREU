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
**Status:** ✅ Aprovada pelo usuário  
**Commit baseline:** `fd8d60e`  
**Baseline:** `docs/technical/PLAYER_CONTROLLER_BASELINE.md`

**Entregas:**
- Player FPS completo (movimento, stamina, crouch, lean, look back, camera feel)
- `PlayerMovementLab.tscn` aprovado
- Hotfixes: spawn, W/S, bob, look back ControllerPath

---

## Sprint 03 — HUD e Debug

**Data:** 2026-07-11  
**Status:** ✅ Concluída  
**Commit:** `765c0fb`  
**Baseline:** `docs/technical/HUD_DEBUG_BASELINE.md`

**Entregas:**
- `HUD.tscn` + `HUDController` (vida, stamina, lanterna, mensagens)
- Autoload `PlaytestDebugSettings` (F10/F11)
- `PlayerHealth`, bateria em `PlayerFlashlight`
- Lab integrado com fog + HUD

---

## Próxima

**Sprint 04 — Sistema de interação mínimo**
