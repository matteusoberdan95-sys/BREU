# BREU — Plano Mestre de Reboot

**Versão:** 1.0  
**Data:** 2026-07-11  
**Status:** Ativo — substitui planos de produção anteriores orientados a GLB/Vertical Slice

---

## 1. Visão do jogo

**BREU** é um horror brasileiro em primeira pessoa com combate, exploração e atmosfera pesada. A identidade é **rural, trash, anos 80/90**, com tensão investigativa antes da ameaça explícita — inspirado em claustrofobia tipo *Outlast*, mas com **combate como diferencial**.

### Premissa narrativa (fase inicial)

- Protagonista comum, cidade sem oportunidades.
- Proposta de emprego tentadora em região distante/interiorana.
- Viagem de ônibus/lotação até a região da **Pensão Santa Luzia**.
- Hospedagem provisória enquanto trabalha.
- Descoberta gradual: trabalho forçado, desaparecimentos, canibalismo, corrupção local (prefeito, polícia).
- **Pacing:** lento → investigativo → tensão sonora/visual → primeira ameaça real (não cedo demais).

### Gameplay desejada (macro)

| Sistema | Prioridade |
|---------|------------|
| Primeira pessoa, exploração | Alta — base |
| Lanterna, stamina, agachar | Alta — base |
| Interação com objetos | Alta — base |
| Puzzles simples | Média — após navegação |
| Combate | Média — após vertical slice explorável |
| Inimigos | Baixa no início — após tensão estabelecida |
| Progressão contínua (sem loading brusco) | Meta — quando possível |

---

## 2. Motivo do reboot

O projeto acumulou **lixo técnico**:

- Múltiplas cenas da Pensão (Integrated, Vertical Slice, Blockout Clean, Professional…) convivendo.
- GLBs e `.blend` como fonte de gameplay — colisões quebradas, barrancos atravessando interior, câmera em teto/laje.
- Scripts temporários de correção (barranco, teto, escada, fog) empilhados sobre empilhados.
- Sprints resolvendo dez problemas ao mesmo tempo.
- Documentação (`PROJECT_STATE.md`, `HANDOFF.md`) desatualizada em relação ao disco.
- **Estado crítico auditado em 2026-07-11:** working tree com deleções massivas não commitadas; `project.godot` ausente no disco; apenas `docs/` e cache `.godot/` permanecem localmente.

**Decisão:** recomeçar do zero **no mesmo repositório**, com sprints pequenas, agentes responsáveis, checkpoints e Definition of Done — **sem tentar remendar a cena antiga**.

---

## 3. Regra de ouro

> **Primeiro jogável, depois bonito.**

Ordem obrigatória de produção:

1. Projeto abre sem erro  
2. Player controller limpo  
3. HUD + debug mínimo  
4. Interação mínima  
5. **Térreo da Pensão** — 100% navegável, caixa cinza, **sem teto**  
6. Playtest + correção do térreo até aprovação  
7. Puzzle simples (depósito)  
8. Escada isolada de teste — rampa invisível, sem teto  
9. Segundo andar blockout — sem teto  
10. Teto modular + teste de câmera FPS  
11. Atmosfera (luz/fog) com modo debug  
12. Vertical slice da Pensão (sem inimigo)  
13. Primeiro encontro / combate base  
14. Arte modular / Blender — **somente após gameplay aprovada**

---

## 4. O que NÃO repetir

| Anti-padrão | Por quê |
|-------------|---------|
| GLB/Blender como fonte de colisão de level | Geometria opaca, remendos infinitos |
| Colisão automática no cenário inteiro | Impossível debugar quedas e brechas |
| Props pequenos com colisão antes da navegação | Falsos positivos, travas |
| Teto antes de validar altura da câmera | Clipping garantido em FPS |
| Segundo andar antes do térreo aprovado | Duplica problemas |
| Escada + 2º andar + teto + puzzle na mesma sprint | Caos e regressão |
| Cenas antigas “só por precaução” | Confusão de qual é oficial |
| Scripts `_fix`, `_patch`, `_temp` sem doc | Dívida técnica |
| Declarar “concluído” sem F6 real | Falso progresso |
| Múltiplas main scenes / referências órfãs | Projeto não abre |

---

## 5. Riscos principais do reboot

| Risco | Mitigação |
|-------|-----------|
| Working tree vazio / arquivos deletados localmente | Sprint 00: restaurar baseline Git ou commitar limpeza intencional |
| Docs mentindo sobre cena oficial | Atualizar `PROJECT_STATE.md` a cada sprint |
| Reintroduzir GLB cedo demais | Gate explícito na Sprint 15 |
| Agentes sobrepondo escopo | Um owner por sprint + DoD |
| OneDrive/sync conflitando arquivos | Checkpoint Git após cada sprint |
| Cache `.godot/` com referências mortas | Reimport limpo após Sprint 01 |
| Tentação de “aproveitar” código antigo quebrado | Cherry-pick só com QA + doc |

---

## 6. Plano macro de desenvolvimento

| Fase | Sprints | Entrega |
|------|---------|---------|
| **Fundação** | 00–04 | Projeto abre, player, HUD, interação |
| **Pensão térreo** | 05–07 | Layout navegável + puzzle depósito |
| **Verticalidade** | 08–10 | Escada, 2º andar, teto |
| **Atmosfera** | 11–12 | Luz/fog + slice completo sem inimigo |
| **Ameaça** | 13–14 | Encontro + combate base |
| **Arte** | 15+ | Módulos Blender substituindo blockout |

Detalhamento sprint a sprint: `docs/production/SPRINT_ROADMAP.md`.

---

## 7. Cena oficial (a definir na Sprint 01)

Até lá, **nenhuma cena antiga é válida**. A cena oficial será declarada em `docs/PROJECT_STATE.md` quando existir.

Nome provisório alvo: cena bootstrap vazia → depois `PensaoTerreo_Blockout_v1.tscn` (nome final decidido pelo Technical Director).

---

## 8. Métricas de level (FPS blockout)

| Elemento | Valor |
|----------|-------|
| Altura de parede | 3,0 m |
| Largura de porta | 1,4 m |
| Altura de porta | 2,3 m |
| Largura mínima de corredor | 2,2 m |
| Eixo vertical | **Y** (Godot) |
| Colisão | Manual — `StaticBody3D` + `BoxShape3D` |
| Teto | Proibido até sprint dedicada |

---

## 9. Governança

- **Definition of Done:** `docs/production/DEFINITION_OF_DONE.md`
- **Playtest:** `docs/testing/PLAYTEST_PROTOCOL.md`
- **Agentes:** `docs/agents/AGENT_ROLES.md`
- **Estrutura de pastas:** `docs/technical/GODOT_PROJECT_STRUCTURE.md`
- **Próxima sprint:** `docs/production/SPRINT_00_TASKS.md`

---

## 10. Critério de sucesso do reboot

O reboot só é considerado bem-sucedido quando:

1. Uma única cena oficial é declarada e testada.
2. Térreo da Pensão é navegável de ponta a ponta sem cair, sem teto na câmera, sem escada fantasma.
3. Documentação, Git e `project.godot` concordam entre si.
4. Nenhum agente precisa “adivinhar” qual cena usar.
