# BREU — Estado do projeto

**Última atualização:** 2026-07-11  
**Fase:** REBOOT GREENFIELD — Sprint 02 em correção  
**Baseline:** `docs/production/REBOOT_BASELINE_DECISION.md`

---

## Estado atual

| Item | Status |
|------|--------|
| Branch | `reboot/breu-clean-start` |
| Player FPS | Recriado — **correção aplicada** |
| Cena de teste | `res://scenes/test/PlayerMovementLab.tscn` |
| Sprint 02 | **Em validação** — aguarda playtest manual F6 |
| Sprint 03 | **Bloqueada** |

---

## Sprint 02 — correção

**Problema:** spawn dentro de túnel baixo + movimento invertido + ambiente escuro.

**Fix:** cena mínima estática, spawn `(0,0,0)`, direção `-Z` corrigida, luz direcional clara.

**Validação automática:** `IsOnFloor=true`, câmera Y=1.65, build OK.

**Validação manual pendente:**
1. Chão plano — movimento livre
2. Parede em Z=-5 — colisão estável
3. Rampa — subida sem jitter

Ver: `docs/testing/PLAYER_MOVEMENT_LAB_PLAYTEST.md`

---

## Player

| Sistema | Status |
|---------|--------|
| WASD + mouse | Corrigido (direção) |
| Sprint + stamina | OK |
| Agachar | OK |
| Reset (R) | OK |
| Lanterna | Off por padrão no lab |

---

## Próxima sprint

**Sprint 03 — HUD e Debug** — somente após Sprint 02 aprovada nos 3 testes.

---

## Retomar

1. `docs/HANDOFF.md`
2. `docs/testing/PLAYER_MOVEMENT_LAB_PLAYTEST.md`
3. F6 em `PlayerMovementLab.tscn`
