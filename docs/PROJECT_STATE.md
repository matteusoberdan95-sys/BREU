# BREU — Estado do projeto

**Última atualização:** 2026-07-11  
**Fase:** REBOOT GREENFIELD — Sprint 00 concluída  
**Baseline:** `docs/production/REBOOT_BASELINE_DECISION.md`  
**Fonte oficial:** `docs/production/BREU_REBOOT_MASTER_PLAN.md`

---

## Estado atual

| Item | Status |
|------|--------|
| Decisão baseline | **Opção A — Greenfield** |
| Branch ativa | `reboot/breu-clean-start` |
| `project.godot` | **AUSENTE** — projeto não executável no disco |
| `BREU.csproj` | **AUSENTE** |
| Gameplay no working tree | **AUSENTE** (intencional) |
| Cena oficial jogável | **Nenhuma** |
| Histórico Git | Preservado como referência (`de07ae6` e anteriores) |

O projeto Godot **ainda não é executável no disco local**. A fundação técnica será recriada na **Sprint 01**.

---

## Cena principal

**Indefinida.** Alvo Sprint 01: `res://scenes/levels/BootstrapEmpty.tscn` (a criar).

---

## O que é válido

| Usar | Não usar |
|------|----------|
| `docs/production/BREU_REBOOT_MASTER_PLAN.md` | Docs com banner ⚠️ OBSOLETO |
| `docs/production/SPRINT_ROADMAP.md` | Vertical Slice / Integrated / BlockoutClean como oficial |
| `docs/production/REBOOT_BASELINE_DECISION.md` | `PHASE_01_02_SPRINT_PLAN.md` operacional |
| `docs/agents/AGENT_ROLES.md` | Playtests de DemoRoom / RitualRoom / Pensão antiga |
| `docs/testing/PLAYTEST_PROTOCOL.md` | GLB/Blender como fonte de colisão |
| Histórico Git (consulta) | Restaurar gameplay antiga |

---

## Próxima sprint

**Sprint 01 — Fundação Godot mínima**

- Recriar `project.godot`, `BREU.csproj`, estrutura de pastas.
- Cena bootstrap vazia.
- **Sem** player, pensão ou HUD nesta sprint (conforme roadmap).

Ver: `docs/production/SPRINT_ROADMAP.md`

---

## Como retomar

1. `docs/HANDOFF.md`
2. `docs/production/SPRINT_00_AUDIT_REPORT.md`
3. `docs/production/REBOOT_BASELINE_DECISION.md`
4. Executar Sprint 01

---

## Relatório Sprint 00

Ver `docs/production/SPRINT_00_AUDIT_REPORT.md`
