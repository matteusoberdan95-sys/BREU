# BREU — Política de commit e push por sprint

**Versão:** 1.0  
**Data:** 2026-07-11  
**Regra Cursor:** `.cursor/rules/sprint-commit-push.mdc`

---

## Regra

> **Cada sprint concluída = commit + push automáticos**, usando Conventional Commits.

O agente (ou desenvolvedor) **não deve deixar** trabalho de sprint concluída apenas no working tree.

---

## Fluxo obrigatório

```
1. Implementar entregas da sprint
2. Validar DoD (build, playtest, checklist)
3. Atualizar documentação (PROJECT_STATE, HANDOFF, SPRINT_HISTORY, tasks da sprint)
4. git add (arquivos relevantes)
5. git commit -m "prefixo: mensagem"
6. git push -u origin <branch>  (se branch nova)
   git push                     (se branch já rastreada)
```

---

## Escolha do prefixo

| Prefixo | Uso |
|---------|-----|
| `feat:` | Nova funcionalidade — **padrão para sprints de gameplay/código** |
| `fix:` | Sprint focada em correção de bug |
| `docs:` | Sprint apenas documental (ex.: Sprint 00 auditoria) |
| `refactor:` | Reorganização sem mudar comportamento jogável |
| `chore:` | Infra, pastas, tooling, `.gitignore` |

Corpo do commit: mencionar número da sprint e objetivo.

---

## Exemplos por sprint (reboot)

| Sprint | Commit sugerido |
|--------|-----------------|
| S00 | `docs: establish BREU greenfield reboot baseline` |
| S01 | `feat: Sprint 01 Godot foundation bootstrap` |
| S02 | `feat: add clean first person player controller` |
| S03 | `feat: add minimal HUD and debug flags` |

---

## Quando NÃO commitar

- Checklist / DoD incompleto.
- `dotnet build` falhou.
- Bug bloqueante documentado e sprint **não** marcada como concluída.

Nesse caso: registrar em `docs/HANDOFF.md` e `docs/production/SPRINT_XX_TASKS.md`.

---

## Documentos a atualizar antes do commit

1. `docs/SPRINT_HISTORY.md`
2. `docs/PROJECT_STATE.md`
3. `docs/HANDOFF.md`
4. `docs/production/SPRINT_XX_TASKS.md` (checklist da sprint)
5. `docs/production/SPRINT_ROADMAP.md` (status da sprint)

Ver também: `.cursor/rules/pre-commit-docs.mdc`

---

## Branch

Durante reboot greenfield: `reboot/breu-clean-start` (ou `reboot/sNN-slug` conforme roadmap).

Push sempre para `origin` após commit bem-sucedido.
