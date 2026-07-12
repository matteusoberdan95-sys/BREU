# Playtest funcional — Áudio Pensão (Sprint 16B)

**Cena:** `scenes/levels/pensao_santa_luzia/PensaoVerticalBlockout01.tscn`  
**Data:** 2026-07-11  
**Objetivo:** Validar passos audíveis (terra/madeira/corrida), gotas no depósito e debug F7.

---

## Debug de áudio (F7)

| Check | Esperado | Resultado |
|-------|----------|-----------|
| F7 liga Audio Debug Mode | Mensagem HUD + log `[AudioDebug] Mode ON` | ☐ |
| Tecla 1 | Sample de contexto ambience / settle | ☐ |
| Tecla 2 | Passo madeira | ☐ |
| Tecla 3 | Passo terra/cascalho | ☐ |
| Tecla 4 | Passo corrida | ☐ |
| Tecla 5 | Gota one-shot | ☐ |
| Tecla 6 | Distant step | ☐ |
| Tecla 7 | Door scratch | ☐ |
| Tecla 8 | Knock | ☐ |
| F7 desliga | Mode OFF; teclas 1–8 param de interceptar | ☐ |
| F6 / F10 / F11 | Continuam a funcionar | ☐ |

---

## Exterior

| Check | Esperado | Resultado |
|-------|----------|-----------|
| Caminhar na trilha | Passos dirt/gravel | ☐ |
| Correr na trilha | Passos run | ☐ |
| Parado | Sem passos | ☐ |

## Interior

| Check | Esperado | Resultado |
|-------|----------|-----------|
| Recepção / corredor | Passos madeira | ☐ |
| Corrida interna | Passos run | ☐ |
| Agachar | Volume/cadência menores | ☐ |
| Escada / 2º andar | Madeira ou run conforme estado | ☐ |

## Depósito / gotas

| Check | Esperado | Resultado |
|-------|----------|-----------|
| Ambience depósito | Loop deposit | ☐ |
| Loop gotas | `pension_water_drops_loop` audível (~−17 dB) | ☐ |
| One-shots gotas | `water_drop_01/02/03` a cada 6–16 s na zona | ☐ |
| Fusível | Puzzle intacto | ☐ |

## Eventos (Sprint 15 + áudio)

| Check | Esperado | Resultado |
|-------|----------|-----------|
| Entrada | `old_house_settle_02` | ☐ |
| Pós-chave | `distant_knock_01` | ☐ |
| Pós-fusível | `distant_step_01` + `02` | ☐ |
| Porta bloqueada | `door_scratch_02` | ☐ |

## Regressão

| Check | Resultado |
|-------|-----------|
| Movimento / stamina / crouch / look back / lean | ☐ inalterado |
| HUD / lanterna | ☐ |
| Fog / atmosfera / geometria / portas | ☐ |
| Puzzle chave → depósito → fusível | ☐ |

---

## Bugs encontrados

_Nenhum registrado na implementação 16B (pendente F6 do usuário)._

## Checklist de aprovação 16B

- [ ] Passos audíveis
- [ ] Terra ≠ madeira
- [ ] Corrida própria
- [ ] Gotas audíveis no depósito
- [ ] Eventos one-shot
- [ ] Debug F7
- [ ] Sem alteração de gameplay
- [ ] Sem crash por asset ausente
- [ ] Volumes aceitáveis
