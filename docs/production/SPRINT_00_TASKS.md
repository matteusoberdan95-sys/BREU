# Sprint 00 — Auditoria e limpeza do projeto

**Branch alvo:** `reboot/breu-clean-start`  
**Owner:** Build & Repo Hygiene Agent  
**Revisores:** Security / Safety, Technical Director, QA  
**Duração sugerida:** 1 sessão  
**Gameplay nesta sprint:** **NÃO**

---

## Objetivo

Estabelecer a **verdade** sobre o repositório e preparar terreno para Sprint 01 — sem implementar Pensão, player ou level.

---

## Diagnóstico inicial (2026-07-11 — Pré-Sprint)

| Item | Estado |
|------|--------|
| `project.godot` no disco | **AUSENTE** |
| `BREU.csproj` no disco | **AUSENTE** |
| Cenas / scripts / assets | **AUSENTES** (deleções massivas não commitadas) |
| `docs/` | Presente (47 arquivos) |
| `.cursor/rules/` | Presente |
| `.godot/` cache | Presente (referências stale) |
| Branch Git | `reset/clean-godot-pensao-blockout` |
| Último commit | `de07ae6` — blockout clean pensão |
| `docs/PROJECT_STATE.md` | **OBSOLETO** — referencia Vertical Slice / GLB deletados |

**Alerta Security:** working tree apagou ~100% do jogo localmente. Confirmar se foi intencional antes de commitar deleções.

---

## Tarefas

### 1. Localizar e validar raiz

- [ ] Confirmar pasta com histórico Git: `BREU/` (contém `.git/`)
- [ ] Anotar caminho absoluto no relatório

### 2. Git — estado e branch

- [ ] `git status` — listar deletados vs untracked
- [ ] `git branch -a`
- [ ] `git log -5 --oneline`
- [ ] Criar branch: `git checkout -b reboot/breu-clean-start`

### 3. Decisão de baseline (OBRIGATÓRIO — humano + Tech Director)

Escolher **uma** opção e documentar em `PROJECT_STATE.md`:

**Opção A — Greenfield total (recomendado)**  
- Commitar deleções intencionais OU `git restore` só docs.  
- Sprint 01 recria `project.godot`, csproj, bootstrap vazio.  
- Código antigo vive apenas no histórico Git.

**Opção B — Restore mínimo**  
- `git checkout HEAD -- project.godot BREU.csproj BREU.sln GlobalUsings.cs icon.svg .gitignore`  
- **Não** restaurar cenas Pensão/GLB antigas.  
- Ajustar main scene para bootstrap vazio na Sprint 01.

**Opção C — Restore commit de07ae6 completo**  
- **Não recomendado** — reintroduz blockout remendado que motivou reboot.

### 4. Referências quebradas

- [ ] Se `project.godot` existir: verificar `run/main_scene`
- [ ] Buscar referências a arquivos deletados:
  ```bash
  rg -l "PensaoSantaLuziaVerticalSlice|PensaoSantaLuziaIntegrated|TrailIntro|BlockoutClean" docs/ project.godot 2>/dev/null
  ```
- [ ] Listar autoloads órfãos (se project existir)

### 5. Cena bootstrap temporária (Sprint 00 ou 01)

Se necessário para projeto abrir:

- [ ] Criar `scenes/levels/BootstrapEmpty.tscn`:
  - `Node3D` root
  - `WorldEnvironment` mínimo
  - `DirectionalLight3D`
  - **Sem** player até Sprint 02 (ou stub Sprint 01 se exigido)

- [ ] Apontar `run/main_scene` para bootstrap

### 6. Limpeza de cache

- [ ] Avaliar deletar `.godot/` local (reimport limpo na Sprint 01)
- [ ] **Não** commitar `.godot/`

### 7. Documentação

- [ ] Atualizar `docs/PROJECT_STATE.md` — estado real pós-decisão
- [ ] Atualizar `docs/HANDOFF.md` — apontar para reboot docs
- [ ] Marcar docs obsoletos com banner no topo (opcional):
  - `docs/production/PHASE_01_02_SPRINT_PLAN.md`
  - `docs/testing/PENSAO_SANTA_LUZIA_VERTICAL_SLICE_PLAYTEST.md`

### 8. Relatório Sprint 00

Criar seção em `docs/PROJECT_STATE.md` ou `docs/production/SPRINT_00_REPORT.md`:

```markdown
# Relatório Sprint 00

- Data:
- Baseline escolhida: A / B / C
- Branch: reboot/breu-clean-start
- project.godot main_scene:
- Arquivos no disco (resumo):
- Referências quebradas encontradas:
- Ações tomadas:
- Riscos remanescentes:
- Próxima sprint: 01 — Fundação mínima Godot
```

### 9. Checkpoint Git

- [ ] Commit ao fim da Sprint 00:
  ```
  docs: sprint 00 audit and reboot baseline
  ```
- [ ] **Não** push force em `main`

---

## Critérios de aceite (DoD Sprint 00)

- [ ] Branch `reboot/breu-clean-start` existe
- [ ] Decisão de baseline documentada
- [ ] Nenhuma doc afirma Vertical Slice / Integrated como cena oficial
- [ ] Plano claro para Sprint 01
- [ ] Commit de auditoria criado
- [ ] **Projeto abre** OU bloqueio documentado com plano Sprint 01 (aceitável se greenfield)

---

## Explicitamente FORA de escopo

- ❌ Pensão térreo
- ❌ Player controller completo
- ❌ HUD / combate / inimigos
- ❌ Blender / GLB
- ❌ Escada / 2º andar / teto
- ❌ Restaurar cenas antigas “para testar”

---

## Fechamento Sprint 00

| Item | Valor |
|------|-------|
| Data | 2026-07-11 |
| Commit | Ver `SPRINT_00_AUDIT_REPORT.md` |
| Branch | `reboot/breu-clean-start` |
| Baseline | Greenfield (Opção A) |
| Cena oficial | Nenhuma |
| QA F6 | N/A |
| Aprovado | **Sim** |
