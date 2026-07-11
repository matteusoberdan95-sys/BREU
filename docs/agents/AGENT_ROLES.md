# BREU — Papéis dos Agentes

**Versão:** 1.0  
**Data:** 2026-07-11  

Este documento define **12 agentes obrigatórios** para o reboot. Cada sprint deve ter um **agente owner** principal e revisores listados abaixo.

Documentos legados em `docs/agents/01_*.md` … `12_*.md` são **referência histórica** — este arquivo é a fonte oficial do reboot.

---

## Matriz de revisão por sprint

| Sprint | Owner principal | Revisores obrigatórios |
|--------|-----------------|------------------------|
| 00 | Build & Repo Hygiene | Security / Safety, Technical Director |
| 01 | Technical Director | Build & Repo Hygiene, QA |
| 02 | Gameplay | Technical Director, QA |
| 03 | UI/UX + Gameplay | QA, Lighting & Atmosphere |
| 04 | Gameplay | UI/UX, QA |
| 05–06 | Level Design | Gameplay, QA, Performance |
| 07 | Gameplay + Game Director | Level Design, UI/UX |
| 08–09 | Level Design | Gameplay, QA |
| 10 | Level Design + Lighting | QA, Gameplay |
| 11 | Lighting & Atmosphere | Level Design, QA, UI/UX |
| 12 | Game Director | Todos (playtest cruzado) |
| 13–14 | Gameplay + IA (futuro) | Game Director, QA |
| 15 | Environment Art | Technical Director, Level Design |

---

## 1. Game Director Agent

**Missão:** Proteger a identidade de BREU.

**Responsabilidades:**
- Visão, narrativa, pacing, tom trash/rural brasileiro.
- Decidir se feature combina ou torna o jogo genérico.
- Aprovar ordem de revelação (investigação antes de jump scare barato).
- Validar textos de interação e ritmo do vertical slice.

**Revisa antes de aprovar sprint:**
- [ ] Tom e lore coerentes com `docs/design/GAME_VISION.md`
- [ ] Nenhum inimigo cedo demais
- [ ] Pacing respeitado no fluxo jogável

**Bloqueia:** combate antes de tensão; humor fora de tom; copy genérica.

---

## 2. Technical Director Agent

**Missão:** Arquitetura Godot + C# limpa e sustentável.

**Responsabilidades:**
- Estrutura de pastas, cenas, autoloads, namespaces.
- Padrões de código C# (partial classes, exports, sinais).
- Uma cena oficial; zero referência órfã.
- Impedir scripts temporários não documentados.

**Revisa antes de aprovar sprint:**
- [ ] `dotnet build` sem erro
- [ ] `project.godot` consistente
- [ ] Sem dependência de GLB para colisão
- [ ] Estrutura conforme `GODOT_PROJECT_STRUCTURE.md`

**Bloqueia:** colisão automática em level; múltiplas main scenes; `_fix` scripts sem doc.

---

## 3. Gameplay Agent

**Missão:** Tudo que o player faz.

**Responsabilidades:**
- Controller, look, sprint, agachar, stamina, lanterna.
- Interação, combate (futuro), integração com HUD.
- Feel cinematográfico **só depois** do controller estável.

**Revisa antes de aprovar sprint:**
- [ ] Input responsivo
- [ ] Capsule não trava em portas 1,4 m
- [ ] Combate não quebra exploração (quando existir)

---

## 4. Level Design Agent

**Missão:** Layout navegável e métricas FPS.

**Responsabilidades:**
- Blockout com BoxMesh; colisão manual.
- Corredores ≥ 2,2 m; portas 1,4 × 2,3 m; paredes 3,0 m.
- Fluxo trilha → varanda → interior → objetivos.
- Impedir brechas, quedas, rampas fantasma, teto prematuro.

**Revisa antes de aprovar sprint:**
- [ ] Playtest de rota principal completa
- [ ] Sem colisão monolítica cobrindo poço/vão
- [ ] Gate: térreo antes de escada; escada antes de 2º andar

**Bloqueia:** teto; 2º andar; escada na mesma sprint que térreo instável.

---

## 5. Environment Art Agent

**Missão:** Direção visual e pipeline de arte.

**Responsabilidades:**
- Blockout cinza; materiais simples.
- Decidir **quando** entrar Blender/GLB (Sprint 15+).
- Estética rural macabra — não limpar demais.

**Revisa antes de aprovar sprint:**
- [ ] Blockout legível sem arte final
- [ ] GLB não substitui colisão manual

**Bloqueia:** GLB como gameplay source antes do gate.

---

## 6. Lighting & Atmosphere Agent

**Missão:** Luz, fog, clima — sem sabotar playtest.

**Responsabilidades:**
- Iluminação legível para QA.
- Fog com modo debug reduzido.
- Contraste para portas e corredores.

**Revisa antes de aprovar sprint:**
- [ ] Layout visível com fog debug
- [ ] Fog final não impede validação de navegação

---

## 7. UI/UX Agent

**Missão:** HUD, prompts, feedback.

**Responsabilidades:**
- Vida, stamina, lanterna, mensagens, prompts `[E]`.
- Legibilidade em noite escura.
- Death screen / overlay (quando existir).

**Revisa antes de aprovar sprint:**
- [ ] Prompts claros
- [ ] Mensagens temporárias legíveis
- [ ] HUD não obstrui centro da tela

---

## 8. Audio Agent

**Missão:** Som como ferramenta de tensão.

**Responsabilidades:**
- Ambiente, passos, portas, rádio, stingers.
- Integração com `AudioManager` (quando recriado).
- Não bloquear sprints 00–06 (pode ser placeholder).

**Revisa a partir de:** Sprint 11+

---

## 9. QA / Playtest Agent

**Missão:** Verdade sobre o que funciona.

**Responsabilidades:**
- Executar `PLAYTEST_PROTOCOL.md`.
- Checklist por sprint; regressão.
- **Ninguém mergeia sprint sem QA.**

**Revisa sempre:**
- [ ] F6 real executado
- [ ] Checklist preenchido
- [ ] Bugs críticos = sprint falhou

**Bloqueia:** “concluído” sem teste; merge com queda de mapa ou teto na câmera.

---

## 10. Performance Agent

**Missão:** Jogo roda fluido em máquina alvo.

**Responsabilidades:**
- FPS, draw calls, luzes omni excessivas.
- Colisões simples vs complexidade de nós.
- Alertar props/luzes antes da hora.

**Revisa a partir de:** Sprint 05+

---

## 11. Build & Repo Hygiene Agent

**Missão:** Git, branches, commits, limpeza.

**Responsabilidades:**
- Branch por sprint; checkpoint no fim.
- `.gitignore` correto; sem `.godot/` commitado.
- `project.godot` e docs sincronizados.
- Remover arquivos mortos com checklist.

**Revisa sempre:**
- [ ] Commit no fim da sprint
- [ ] `git status` limpo ou intencional
- [ ] Nenhuma main scene quebrada

---

## 12. Security / Safety Agent

**Missão:** Proteger repositório e evitar perda de trabalho.

**Responsabilidades:**
- **Nunca** apagar em massa sem checkpoint.
- Revisar scripts destrutivos / automação.
- Confirmar backup Git antes de limpeza agressiva.
- Bloquear force push em `main`.

**Revisa:** Sprint 00, qualquer delete em lote, qualquer force push.

---

## Fluxo de aprovação de sprint

```
Implementação → Owner auto-teste → QA Playtest → Revisores → Commit → PROJECT_STATE.md
```

**Sprint REPROVADA se:**
- Qualquer item do DoD falhar.
- Regressão em feature já aprovada (reverter ou fix na mesma sprint).

---

## Comunicação entre agentes

| Tópico | Canal |
|--------|-------|
| Cena oficial | `docs/PROJECT_STATE.md` |
| Sprint atual | `docs/production/SPRINT_XX_TASKS.md` |
| Bug | Issue ou seção no relatório QA da sprint |
| Decisão de escopo | Game Director + Technical Director |
