# BREU — Definition of Done (DoD)

**Versão:** 1.0  
**Data:** 2026-07-11  
**Aplica-se a:** todas as sprints do reboot

---

## DoD geral (obrigatório em TODA sprint)

Uma sprint **só é aprovada** quando **todos** os itens abaixo forem verdadeiros:

### Projeto e build

- [ ] Godot 4.7 abre o projeto sem erro fatal.
- [ ] `dotnet build` conclui com **0 erros**.
- [ ] `project.godot` → `run/main_scene` aponta para cena **existente**.
- [ ] Nenhuma referência quebrada óbvia no editor (cena/script missing).

### Cena principal

- [ ] Cena declarada em `docs/PROJECT_STATE.md` é a mesma do `project.godot`.
- [ ] Play (F5) ou Play Current Scene (F6) inicia sem crash.
- [ ] Player (quando existir) spawna em posição válida, não dentro de parede.

### Playtest

- [ ] Checklist da sprint em `docs/testing/PLAYTEST_PROTOCOL.md` executado.
- [ ] Teste **manual real** — não apenas headless, exceto Sprint 00–01.
- [ ] Regressão: features aprovadas em sprints anteriores ainda funcionam.

### Documentação

- [ ] `docs/PROJECT_STATE.md` atualizado (cena oficial, o que mudou, o que testar).
- [ ] Tarefas da sprint marcadas concluídas ou explicitamente adiadas com motivo.

### Versionamento

- [ ] Commit/checkpoint criado no fim da sprint.
- [ ] Mensagem de commit descreve **porquê**, não só arquivos.
- [ ] Branch nomeada conforme roadmap (`reboot/sXX-...`).

---

## DoD específico por tipo de entrega

### Player / Gameplay

- [ ] WASD + mouse look sem jitter grave.
- [ ] Capsule não atravessa paredes de 0,22 m em velocidade normal.
- [ ] Agachar não trava em portas de 2,3 m (quando existir).

### Level blockout

- [ ] Corredor principal ≥ 2,2 m onde declarado.
- [ ] Portas ≥ 1,4 m de largura.
- [ ] Piso caminhável tem colisão manual contínua.
- [ ] **Sem teto** até Sprint 10.
- [ ] Props pequenos **sem colisão** até navegação aprovada.

### Interação

- [ ] `[E]` ou prompt configurado funciona a ≤ 3 m.
- [ ] Mensagem aparece no HUD ou log claro.
- [ ] `ControllerPath` / wiring correto — nunca “Interação indisponível” por bug de path.

### Debug / Playtest

- [ ] `DebugInfiniteLantern = true` disponível na cena de teste.
- [ ] Lanterna não chega a 0 em 10 min com debug ativo.
- [ ] Fog reduzida disponível sem remover sistema final.

### Atmosfera

- [ ] Layout legível com modo debug de fog.
- [ ] Luzes suficientes para QA validar portas e corredores.

---

## O que NÃO conta como “done”

| Situação | Veredicto |
|----------|-----------|
| “Compila mas não testei” | **Falhou** |
| “Funciona no editor do agente, usuário não testou” | **Falhou** |
| Player cai do mapa | **Falhou** |
| Câmera atravessa teto/laje | **Falhou** |
| Main scene aponta para arquivo deletado | **Falhou** |
| Docs desatualizados | **Falhou** |
| Sprint resolveu 5 escopos ao mesmo tempo | **Revisar / dividir** |

---

## Regressão

Se uma mudança **quebra** algo já aprovado:

1. **Reverter** a mudança, ou
2. **Corrigir na mesma sprint** antes de fechar.

Não acumular “vamos arrumar depois”.

---

## Template de fechamento de sprint

Copiar no final de cada `SPRINT_XX_TASKS.md`:

```markdown
## Fechamento Sprint XX

- Data:
- Commit:
- Branch:
- Cena oficial:
- QA executou F6: sim/não
- Regressões:
- Itens adiados:
- Aprovado: sim/não
```
