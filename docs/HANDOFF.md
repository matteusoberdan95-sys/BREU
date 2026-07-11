# BREU — Handoff

**Última atualização:** 2026-07-11  
**Status:** REBOOT GREENFIELD — Sprint 00 concluída  
**Branch:** `reboot/breu-clean-start`

---

## Retomar em 60 segundos

1. `docs/production/REBOOT_BASELINE_DECISION.md` — decisão Greenfield
2. `docs/PROJECT_STATE.md` — estado real
3. `docs/production/SPRINT_00_AUDIT_REPORT.md` — auditoria
4. `docs/production/SPRINT_ROADMAP.md` — Sprint 01 em diante
5. `docs/agents/AGENT_ROLES.md` — papéis dos agentes

---

## Decisão oficial (2026-07-11)

**Opção A — Greenfield.** Recomeçar do zero no mesmo repositório. Histórico Git preservado; gameplay antiga **não** será restaurada.

---

## Estado do disco

- **Sem** `project.godot` — Godot não abre ainda
- **Sem** gameplay, cenas, scripts ou assets no working tree
- **Com** documentação de reboot completa em `docs/`

---

## Documentos obsoletos (não usar operacionalmente)

Marcados com banner ⚠️ no topo:

- `docs/production/PHASE_01_02_SPRINT_PLAN.md`
- `docs/production/CLEANUP_OLD_SCENES.md`
- `docs/testing/PENSAO_SANTA_LUZIA_VERTICAL_SLICE_PLAYTEST.md`
- `docs/testing/PLAYTEST_DEMO_ROOM.md`
- `docs/testing/PLAYTEST_RITUAL_ROOM.md`
- `docs/gameplay/NEXT_SPRINT_TASKS.md`

---

## Próxima ação

**Sprint 01 — Fundação Godot mínima** (Technical Director + Build & Repo Hygiene)

- Recriar `project.godot` e csproj
- Estrutura de pastas vazia
- `BootstrapEmpty.tscn`
- **Não** restaurar cenas antigas

---

## Regra de produção

> Primeiro jogável, depois bonito.  
> Térreo → escada → 2º andar → teto → atmosfera → inimigo → Blender.
