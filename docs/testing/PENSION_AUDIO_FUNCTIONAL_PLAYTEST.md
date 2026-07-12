# Playtest funcional — Áudio Pensão (Sprint 16B–16E)

**Cena:** `scenes/levels/pensao_santa_luzia/PensaoVerticalBlockout01.tscn`  
**Data:** 2026-07-12

---

## Debug (F7)

| Check | Esperado | Resultado |
|-------|----------|-----------|
| F7 ON | Logs Footstep + Breathing | ☐ |
| Teclas 2–4 | Passos wood/dirt/run superfície | ☐ |
| Tecla 9 | One-shot `player_breath_heavy_*` | ☐ |
| Tecla 0 | Alterna panting de teste | ☐ |
| F10 / F11 | Intactos | ☐ |

---

## Passos (16D) — resumo

| Check | Resultado |
|-------|-----------|
| Terra walk/run mesmo banco, cadência 0,64 / 0,36 | ☐ |
| Madeira walk/run mesmo banco | ☐ |
| Sem `player_run_step_*` / sequence | ☐ |
| Terra pode ser refinada depois (nota) | — |

---

## Sprint 16E — Player breathing

**Volumes alvo:** normal −32 dB; panting −18 / −14 low stamina; one-shot −16 dB.

| Check | Esperado | Resultado |
|-------|----------|-----------|
| Parado / silêncio | Respiração bem baixa | ☐ |
| Andar | Sutil; não cobre passos | ☐ |
| Correr ≥ 2 s | Panting fade-in audível | ☐ |
| Parar após corrida | Panting 2–5 s depois fade-out | ☐ |
| Stamina &lt; 35% | Panting mais intenso | ☐ |
| One-shot | Fim de corrida longa / tecla 9; sem spam | ☐ |
| Não irrita em loop | ☐ | ☐ |

### Bugs encontrados (16E)

_Pendente F6 do usuário._

---

## Regressão

| Check | Resultado |
|-------|-----------|
| Movimento / sprint / stamina / crouch / head bob | ☐ |
| Passos terra/madeira | ☐ |
| Gotas / ambience / eventos / HUD / lanterna | ☐ |
| Fog / puzzle | ☐ |
| PlayerController não alterado | ☐ |

## Aprovação 16E

- [ ] Respiração normal audível e sutil
- [ ] Panting ao correr ≥ 2 s
- [ ] Recover com fade
- [ ] Sem alterar gameplay
