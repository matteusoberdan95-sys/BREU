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
**Status:** ✅ Aprovada pelo usuário  
**Commit implementação:** `765c0fb`  
**Commit baseline:** `05a35d5`  
**Baseline:** `docs/technical/HUD_DEBUG_BASELINE.md`

**Entregas:**
- `HUD.tscn` + `HUDController` (vida, stamina, lanterna, mensagens)
- Autoload `PlaytestDebugSettings` (F10/F11)
- `PlayerHealth`, bateria em `PlayerFlashlight`
- Lab integrado com fog + HUD
- Aprovação usuário: HUD responsivo, debug visível, player intacto

---

## Sprint 04 — Sistema de interação mínimo

**Data:** 2026-07-11  
**Status:** ✅ Aprovada pelo usuário  
**Commit implementação:** `c1263c2`  
**Commit hotfix:** `e11147f`  
**Commit baseline:** `ea3daed`  
**Baseline:** `docs/technical/INTERACTION_SYSTEM_BASELINE.md`

**Entregas:**
- `IInteractable`, `Interactable`, `PlayerInteractionRaycast`
- Extensão HUD: prompt `[E]`
- `InteractionLab.tscn` — TestSign, TestBook, TestLockedDoor
- Hotfix: raycast path, collision layers, HUD message panel
- Aprovação usuário: prompt, E, mensagens, colisões, player/HUD intactos

---

## Sprint 05 — Pensão térreo blockout 01

**Data:** 2026-07-11  
**Status:** ✅ Aprovada  
**Baseline:** `docs/technical/PENSION_GROUND_FLOOR_BLOCKOUT_BASELINE.md`  
**Cena:** `PensaoTerreoBlockout01.tscn`

**Entregas:**
- Trilha, varanda, recepção, corredor, quarto 102, cozinha, depósito trancado
- Blockout Godot nativo + builder; hotfixes colisão + visual sealing
- 5 interactables placeholder
- Sem teto, escada, 2º andar, GLB

**Aprovação usuário:** caixa cinza jogável; HUD e interação intactos; exterior simples aceito.

---

## Sprint 06 — Fine playtest térreo

**Data:** 2026-07-11  
**Status:** ✅ Aprovada  
**Cena:** `PensaoTerreoBlockout01.tscn`  
**Baseline:** v1.3

**Entregas:**
- Fine playtest: placa, iluminação, legibilidade, hitboxes
- Hotfix: colisão móveis grandes; depósito selado
- Rota principal, colisão, interações, HUD e movimento validados

**Aprovação usuário:** térreo navegável; móveis com colisão; depósito fechado; exterior placeholder aceito.

---

## Próxima

**Sprint 08 — Escada isolada de teste**

---

## Sprint 07 — Puzzle depósito

**Data:** 2026-07-11  
**Status:** ✅ Aprovada  
**Baseline:** `DEPOSIT_PUZZLE_BASELINE.md`  
**Playtest:** `PENSAO_DEPOSIT_PUZZLE_PLAYTEST.md`

**Entregas:**
- Chave quarto 102 → destrancar depósito → fusível velho + bilhete
- Hotfix parede área futura

**Aprovação usuário:** fluxo completo validado; HUD/player/interação intactos.

---

## Sprint 08 — Escada isolada de teste

**Data:** 2026-07-11  
**Status:** ✅ Aprovada  
**Commit implementação:** `8ae455f`  
**Baseline:** `STAIR_RAMP_BASELINE.md`  
**Playtest:** `STAIR_MOVEMENT_LAB_PLAYTEST.md`

**Entregas:**
- `StairMovementLab.tscn` — rampa invisível + 14 degraus visuais + patamar superior
- `StairMovementLabBuilder.cs`, `StairMovementLabController.cs`
- Escada **não** integrada na Pensão

**Aprovação usuário:** subida/descida suaves; rampa invisível OK; degraus sem colisão; player/HUD intactos.

---

## Sprint 09A — Integrar escada no térreo

**Data:** 2026-07-11  
**Status:** 🔄 Implementada — playtest F6 pendente  
**Playtest:** `PENSAO_STAIR_INTEGRATION_PLAYTEST.md`

**Entregas:**
- Escada no álcove oeste via `StairRampAssembly`
- Patamar superior temporário; barreiras no topo
- Removido `Wall_StairFuture_Blocker`; puzzle depósito preservado

**Pendente:** aprovação usuário após playtest F6 na Pensão.

---
