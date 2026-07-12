# Playtest — Eventos narrativos simples (Sprint 15)

**Cena:** `res://scenes/levels/pensao_santa_luzia/PensaoVerticalBlockout01.tscn`  
**Sprint:** 15 — Evento narrativo simples sem inimigo  
**Data:** 2026-07-11  
**Baseline:** `docs/technical/PENSION_NARRATIVE_EVENTS_BASELINE.md`

---

## Status

**✅ Sprint 15 APROVADA** (2026-07-11) — eventos narrativos one-shot sem inimigo.

---

## Eventos adicionados (ordem típica)

| # | ID | Quando | Mensagem |
|---|----|--------|----------|
| 1 | `pension_entry_first_time` | Entrar na recepção | A recepção está vazia, mas a casa parece ter ouvido minha chegada. |
| 2 | `key_pickup_tension` | Após pegar chave | Um rangido atravessa o corredor. |
| 3 | `fuse_pickup_footsteps` | Após pegar fusível | Passos lentos ecoam acima de mim. |
| 4 | `stair_first_arrival` | Topo da escada | O ar aqui em cima é mais frio. |
| 5 | `upper_corridor_presence` | Meio do corredor 2º | Por um instante, achei ter visto alguém no fim do corredor. |
| 6 | `upper_locked_door_hint` | Após tentar porta varanda/superior | Do outro lado, algo arranha a madeira. |

---

## Áudio

**Não usado nesta sprint.** Sem assets de áudio runtime estáveis; eventos usam apenas mensagem HUD + flicker de luz. Áudio real fica para Sprint 16.

---

## Flicker de luz

**Sim** — `LightFlickerOneShot` em:

- recepção (entrada);
- corredor térreo (pós-chave / pós-fusível);
- landing / corredor superior;
- luz da porta bloqueada (follow-up).

Flicker curto e reversível; energia original restaurada.

---

## Checklist aprovado

| Item | Resultado |
|------|-----------|
| Entrada da pensão | ✅ |
| Pós-chave | ✅ |
| Pós-fusível | ✅ |
| Chegada no segundo andar | ✅ |
| Corredor superior | ✅ |
| Porta bloqueada / varanda | ✅ |
| Regressão do puzzle | ✅ |
| Regressão do HUD | ✅ |
| Regressão da lanterna | ✅ |
| Regressão da atmosfera | ✅ |

---

## Rota de teste F6

- [x] Nascer na trilha
- [x] Entrar na pensão → evento entrada (1×)
- [x] Quarto 102 → pegar chave → evento rangido (1×)
- [x] Depósito → chave → fusível → evento passos (1×)
- [x] Subir escada → evento ar frio (1×)
- [x] Corredor superior → evento presença (1×)
- [x] Tentar varanda/porta → mensagem trancada + arranhão (1×)
- [x] Voltar ao térreo; eventos não repetem
- [x] F8 reseta eventos (opcional)

---

## Regressão

- [x] Player / HUD / lanterna / F10/F11 OK
- [x] Fog / atmosfera OK
- [x] Puzzle chave → depósito → fusível OK
- [x] Escada / 2º andar OK
- [x] Sem portas/placas novas bugadas
- [x] Sem inimigo / combate / chase
- [x] Sem crash por áudio ausente

---

## Bugs conhecidos

Nenhum após aprovação.
