# Playtest funcional — Áudio Pensão (Sprint 16B–16D)

**Cena:** `scenes/levels/pensao_santa_luzia/PensaoVerticalBlockout01.tscn`  
**Data:** 2026-07-12  
**Objetivo 16D:** caminhada lenta/espaçada; corrida mais rápida no mesmo banco; sem duplo trigger; sem sequence/run bank.

---

## Debug (F7)

| Check | Esperado | Resultado |
|-------|----------|-----------|
| F7 ON | Logs `[Footstep] state=… surface=… interval=… sample=…` | ☐ |
| Walk | `interval=0.64`, sample `player_footstep_*` | ☐ |
| Run | `interval=0.36`, **mesmo** prefixo de sample da superfície | ☐ |
| Run nunca loga | `player_run_step` / `*_sequence` | ☐ |
| Tecla 4 | Preview run da superfície atual | ☐ |

---

## Exterior / terra

| Check | Esperado | Resultado |
|-------|----------|-----------|
| Andar | dirt/gravel, cadência lenta (~0,64 s) | ☐ |
| Correr | mesmos samples dirt, cadência ~0,36 s | ☐ |
| Sem embolar / duplo disparo | Um passo por intervalo | ☐ |
| Sem `player_run_step_*` | Confirmado no log | ☐ |

## Interior / madeira

| Check | Esperado | Resultado |
|-------|----------|-----------|
| Andar | wood, ~0,64 s | ☐ |
| Correr | mesmos wood, ~0,36 s | ☐ |
| Escada / 2º | wood | ☐ |
| Sem duplo / sequence | ☐ | ☐ |

## Geral / regressão

| Check | Resultado |
|-------|-----------|
| Parado / no ar sem passos | ☐ |
| Crouch mais baixo/lento | ☐ |
| Gotas / ambience / eventos / lanterna / HUD | ☐ |
| PlayerController não alterado | ☐ |

---

## Auditoria de sistemas

| Sistema | Status |
|---------|--------|
| `PlayerFootstepAudio` | **Único** responsável pelos passos do player |
| Head bob / outro Footstep* | Não dispara áudio de passos |
| `*_sequence` / `player_run_step_*` | No disco, não wired |

## Aprovação 16D

- [ ] Walk mais lento que no vídeo 16C
- [ ] Run claramente mais rápido que Walk, mesmo timbre
- [ ] Sem duplo disparo
- [ ] Sem sequence / run bank
