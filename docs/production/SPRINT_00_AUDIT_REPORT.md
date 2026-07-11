# Sprint 00 — Audit Report

**Data:** 2026-07-11  
**Sprint:** 00 — Auditoria e limpeza do reboot  
**Decisão baseline:** Opção A — Greenfield  
**Aprovado:** Sim (documentação + checkpoint; gameplay fora de escopo)

---

## 1. Branch atual

`reboot/breu-clean-start` (criada a partir de `reset/clean-godot-pensao-blockout`)

---

## 2. Último commit antes do reboot (referência Git)

| Campo | Valor |
|-------|-------|
| Hash | `de07ae6` |
| Mensagem | checkpoint playable clean pension blockout before stair and floor fix |
| Branch origem | `reset/clean-godot-pensao-blockout` |

Histórico completo preservado em Git. **Não restaurado** no working tree.

---

## 3. Estado do working tree (pós-auditoria)

- Gameplay, cenas, scripts C#, assets, shaders e `project.godot` **removidos do disco** (deleções já existentes antes da Sprint 00).
- Sprint 00 **formalizou** essas deleções via commit Greenfield — **sem** `git restore` de arquivos antigos.
- Permanecem: `docs/`, `.cursor/rules/`, `.gitignore` (recriado), cache local `.godot/` (ignorado pelo Git).

---

## 4. project.godot

**NÃO EXISTE** no working tree.

O projeto Godot **não é executável** no disco local. Criação adiada para **Sprint 01**.

---

## 5. BREU.csproj

**NÃO EXISTE** no working tree.

Recriação adiada para **Sprint 01**.

---

## 6. Pastas existentes (top-level)

| Pasta | Conteúdo |
|-------|----------|
| `.git/` | Histórico Git |
| `.cursor/rules/` | Regras Cursor (3 arquivos `.mdc`) |
| `.godot/` | Cache local (não commitado) |
| `docs/` | Documentação (55 arquivos) |

**Ausentes:** `scenes/`, `scripts/`, `assets/`, `shaders/`, `materials/`, etc.

---

## 7. Arquivos principais existentes

### Raiz

- `.gitignore` (recriado — Sprint 00)

### Documentação reboot (ativa)

| Arquivo |
|---------|
| `docs/production/BREU_REBOOT_MASTER_PLAN.md` |
| `docs/production/SPRINT_ROADMAP.md` |
| `docs/production/DEFINITION_OF_DONE.md` |
| `docs/production/SPRINT_00_TASKS.md` |
| `docs/production/REBOOT_BASELINE_DECISION.md` |
| `docs/production/SPRINT_00_AUDIT_REPORT.md` (este arquivo) |
| `docs/agents/AGENT_ROLES.md` |
| `docs/testing/PLAYTEST_PROTOCOL.md` |
| `docs/technical/GODOT_PROJECT_STRUCTURE.md` |
| `docs/PROJECT_STATE.md` |
| `docs/HANDOFF.md` |

### Documentação histórica (mantida, banner obsoleto)

Ver seção 10.

---

## 8. Referências quebradas encontradas

### Arquivos ativos (.tscn, .cs, .godot, .tres)

**Nenhum** — todos ausentes do working tree. Não há referências ativas a cenas antigas em código ou cenas.

### Documentação

Busca por padrões antigos em `docs/**/*.md`:

| Padrão | Ocorrências (aprox.) | Classificação |
|--------|----------------------|---------------|
| PensaoSantaLuzia / Pensao | Múltiplas | Histórica |
| TrailIntro | Múltiplas | Histórica |
| DemoRoom / RitualRoom | Múltiplas | Histórica |
| VerticalSlice / Integrated / BlockoutClean | Múltiplas | Histórica |
| HeightFix / CliffFix / FogCard / TrailMistParticles | Várias | Histórica |

Docs de reboot (`SPRINT_ROADMAP`, `GODOT_PROJECT_STRUCTURE`) mencionam **nomes futuros** (ex.: `PensaoTerreo_Blockout_v1`) — **válido**, não é referência a implementação antiga.

### Cache `.godot/`

Contém referências stale a cenas mortas. **Não confiável.** Será regenerado na Sprint 01. **Não commitado.**

---

## 9. Referências corrigidas

| Ação | Arquivo |
|------|---------|
| Banner ⚠️ OBSOLETO | `docs/production/CLEANUP_OLD_SCENES.md` |
| Banner ⚠️ OBSOLETO | `docs/production/PHASE_01_02_SPRINT_PLAN.md` |
| Banner ⚠️ OBSOLETO | `docs/testing/PENSAO_SANTA_LUZIA_VERTICAL_SLICE_PLAYTEST.md` |
| Banner ⚠️ OBSOLETO | `docs/testing/PLAYTEST_DEMO_ROOM.md` |
| Banner ⚠️ OBSOLETO | `docs/testing/PLAYTEST_RITUAL_ROOM.md` |
| Banner ⚠️ OBSOLETO | `docs/gameplay/NEXT_SPRINT_TASKS.md` |
| Reescrito | `docs/PROJECT_STATE.md` |
| Reescrito | `docs/HANDOFF.md` |
| Criado | `docs/production/REBOOT_BASELINE_DECISION.md` |
| Recriado | `.gitignore` (ignora `.godot/`) |

**Nenhuma cena antiga restaurada** para corrigir referência.

---

## 10. Referências antigas mantidas (somente documentação histórica)

Permanecem sem deleção — consulta e aprendizado:

- `docs/design/*` (GDD, lore, level design phases)
- `docs/agents/01_*.md` … `12_*.md` (agentes legados)
- `docs/visual/*`, `docs/audio/*`, `docs/blender_pipeline/*`
- `docs/technical/TDD.md`, `AUDIO_SYSTEM.md`
- Docs marcados obsoletos na seção 9

**Regra:** docs com banner ⚠️ não orientam implementação.

---

## 11. Decisão baseline

**Opção A — Greenfield**

Documentada em: `docs/production/REBOOT_BASELINE_DECISION.md`

---

## 12. Próxima sprint recomendada

**Sprint 01 — Fundação Godot mínima**

Owner: Technical Director + Build & Repo Hygiene

Entregas:
- `project.godot`, `BREU.csproj`, `BREU.sln`
- Estrutura de pastas vazia
- `scenes/levels/BootstrapEmpty.tscn`
- Projeto abre sem erro

---

## Fechamento Sprint 00

| Item | Valor |
|------|-------|
| Data | 2026-07-11 |
| Commit | `78f4edd` — docs: establish BREU greenfield reboot baseline |
| Branch | `reboot/breu-clean-start` |
| Cena oficial | Nenhuma |
| QA F6 | N/A (sem projeto Godot) |
| Regressões | N/A |
| Aprovado | Sim — escopo Sprint 00 |
