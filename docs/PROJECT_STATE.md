# BREU — Estado do projeto

**Última atualização:** 2026-07-11  
**Fase:** REBOOT GREENFIELD — Sprint 02 concluída  
**Baseline:** `docs/production/REBOOT_BASELINE_DECISION.md`  
**Fonte oficial:** `docs/production/BREU_REBOOT_MASTER_PLAN.md`

---

## Estado atual

| Item | Status |
|------|--------|
| Decisão baseline | **Opção A — Greenfield** |
| Branch ativa | `reboot/breu-clean-start` |
| `project.godot` | **Presente** — Godot 4.7 + C# |
| Player FPS | **Presente** — Sprint 02 |
| Cena de teste movimento | `res://scenes/test/PlayerMovementLab.tscn` |
| Cena main (F5) | `res://scenes/levels/BootstrapEmpty.tscn` |
| Pensão / level real | **Ausente** (intencional) |
| HUD | **Ausente** — Sprint 03 |

---

## Player (Sprint 02)

| Sistema | Status |
|---------|--------|
| WASD + mouse look | OK |
| Sprint + stamina | OK |
| Agachar (C/Ctrl) | OK — teto baixo bloqueia levantar |
| Lanterna base (F) | OK — `DebugInfiniteLantern = true` |
| Gravidade / rampa | OK |
| Combate / interação | Fora de escopo |

Scripts em `scripts/player/`. Cena: `scenes/player/Player.tscn`.

---

## Cena principal

**F5:** `res://scenes/levels/BootstrapEmpty.tscn` (bootstrap vazio)

**F6 (playtest movimento):** `res://scenes/test/PlayerMovementLab.tscn`

---

## Pendências

- HUD (stamina, vida, lanterna) — Sprint 03
- Interação — Sprint 04
- Pensão térreo — Sprint 05+

---

## Próxima sprint

**Sprint 03 — HUD e Debug**

Ver: `docs/production/SPRINT_ROADMAP.md`

---

## Como retomar

1. `docs/HANDOFF.md`
2. `docs/testing/PLAYER_MOVEMENT_LAB_PLAYTEST.md`
3. Executar Sprint 03

---

## Relatórios

- Sprint 00: `docs/production/SPRINT_00_AUDIT_REPORT.md`
- Sprint 01: `docs/production/SPRINT_01_TASKS.md`
- Sprint 02: `docs/production/SPRINT_02_TASKS.md`
- Histórico: `docs/SPRINT_HISTORY.md`
