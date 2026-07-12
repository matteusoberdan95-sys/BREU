# Baseline — Eventos narrativos simples (Pensão)

**Versão:** 1.1  
**Sprint:** 15 (aprovada)  
**Data:** 2026-07-11  
**Cena:** `res://scenes/levels/pensao_santa_luzia/PensaoVerticalBlockout01.tscn`  
**Status:** ✅ Baseline oficial pós-aprovação Sprint 15

---

## Arquitetura

| Peça | Papel |
|------|--------|
| `PensionNarrativeEvents` | Controlador one-shot; fila de mensagens HUD; flicker; F8 reset |
| `NarrativeTrigger3D` | `Area3D` (mask player layer 16); dispara por `EventId`; desativa após uso |
| `LightFlickerOneShot` | Reduz energia de luz por curto período e restaura o valor original |
| `PensaoPuzzleState` eventos | `DepositKeyPickedUp` / `OldFusePickedUp` — observação apenas |
| `BlockoutLockedDoor.NarrativeFollowUpEventId` | Follow-up one-shot após mensagem de porta trancada |

---

## Regras oficiais (pós-Sprint 15)

1. **One-shot:** cada ID dispara no máximo uma vez por sessão (até F8 reset). Não repetir em loop.
2. **Não alterar geometria** — triggers não criam paredes, portas, placas ou layout.
3. **Não alterar player** — sem mexer em movimento, câmera, lanterna ou controles.
4. **Não spawnar inimigo** — sem combate, chase, dano ou entidade hostil.
5. **Próximos eventos** devem seguir este padrão (mensagem / flicker / áudio opcional / one-shot).
6. **Sem bloqueio:** triggers não colidem com o player (`CollisionLayer = 0`).
7. **Mensagens:** fila simples — duração 3–3,5 s; não empilhar sobrepostas.
8. **Flicker:** curto, reversível; nunca altera `WorldEnvironment` / fog.
9. **Áudio:** opcional. Sprint 15 sem áudio; áudio ambiente = Sprint 16.

---

## IDs de eventos

| ID | Disparo | Mensagem |
|----|---------|----------|
| `pension_entry_first_time` | Area recepção | A recepção está vazia, mas a casa parece ter ouvido minha chegada. |
| `key_pickup_tension` | Após pegar chave (~3,2 s) | Um rangido atravessa o corredor. |
| `fuse_pickup_footsteps` | Após pegar fusível (~3,2 s) | Passos lentos ecoam acima de mim. |
| `stair_first_arrival` | Area topo da escada | O ar aqui em cima é mais frio. |
| `upper_corridor_presence` | Area corredor superior | Por um instante, achei ter visto alguém no fim do corredor. |
| `upper_locked_door_hint` | Após tentar varanda/porta superior | Do outro lado, algo arranha a madeira. |

---

## Debug

- **F8:** `ResetAllEvents()` — limpa IDs disparados e reativa triggers.
- Console: `[Narrative] Triggered: <event_id>`

---

## Playtest

`docs/testing/PENSION_SIMPLE_NARRATIVE_EVENTS_PLAYTEST.md` — Sprint 15 **aprovada**
