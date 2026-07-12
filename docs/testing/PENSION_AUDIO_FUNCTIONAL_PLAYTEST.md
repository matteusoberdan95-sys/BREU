# Playtest funcional — Áudio Pensão (Sprint 16B / 16C)

**Cena:** `scenes/levels/pensao_santa_luzia/PensaoVerticalBlockout01.tscn`  
**Data:** 2026-07-11  
**Objetivo:** Validar passos por superfície, gotas, debug F7 e ajuste 16C (cadência + corrida = mesmo banco).

---

## Debug de áudio (F7)

| Check | Esperado | Resultado |
|-------|----------|-----------|
| F7 liga Audio Debug Mode | HUD + log; footsteps passam a logar | ☐ |
| Tecla 2 | Passo madeira (walk) | ☐ |
| Tecla 3 | Passo terra (walk) | ☐ |
| Tecla 4 | Preview corrida com **mesmo banco da superfície** (não `player_run_step_*`) | ☐ |
| Tecla 5–8 | Gota / distant / scratch / knock | ☐ |
| F10 / F11 | Intactos | ☐ |

Com F7 ON, ao andar:
`[Footstep] surface=DirtGravel|Wood state=Walk|Run|Crouch sample=…`

---

## Exterior / terra (16C)

| Check | Esperado | Resultado |
|-------|----------|-----------|
| Andar na trilha | dirt/gravel | ☐ |
| Correr na trilha | **mesmos** samples dirt/gravel | ☐ |
| Corrida não usa `player_run_step_*` | Confirmado no log `sample=player_footstep_dirt_gravel_*` | ☐ |
| Cadência walk | ~0,55 s (mais lenta que 16B) | ☐ |
| Cadência run | ~0,36 s, natural | ☐ |
| Parado | Sem passos | ☐ |

## Interior / madeira (16C)

| Check | Esperado | Resultado |
|-------|----------|-----------|
| Andar recepção/corredor | madeira | ☐ |
| Correr recepção/corredor | **mesmos** samples madeira | ☐ |
| Escada / 2º andar | madeira | ☐ |
| Corrida não usa `player_run_step_*` | `sample=player_footstep_wood_*` | ☐ |

## Crouch / gotas / regressão

| Check | Esperado | Resultado |
|-------|----------|-----------|
| Agachar | Mais baixo e lento (−18 dB / 0,78 s) | ☐ |
| Gotas depósito | Loop + one-shots | ☐ |
| Movimento / sprint / stamina / HUD / lanterna | Inalterados | ☐ |
| Fog / puzzle / eventos | Inalterados | ☐ |

---

## Bugs encontrados

_Pendente F6 do usuário após 16C._

## Aprovação

- [ ] Cadência walk OK
- [ ] Run = mesmo timbre da superfície
- [ ] `player_run_step_*` não usado
- [ ] Superfícies corretas
- [ ] Sem regressão de gameplay
