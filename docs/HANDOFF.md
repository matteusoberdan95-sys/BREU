# BREU — Handoff

**Última atualização:** 2026-07-11  
**Status:** REBOOT GREENFIELD — Sprint 05 concluída  
**Branch:** `reboot/breu-clean-start`

---

## Retomar

1. `docs/PROJECT_STATE.md`
2. `docs/testing/PENSAO_TERREO_BLOCKOUT_01_PLAYTEST.md`
3. Sprint 06 — correções pós-playtest

---

## Sprint 05 — entregue

**Cena:** `PensaoTerreoBlockout01.tscn`

Fluxo: trilha → varanda → recepção → corredor → quarto 102 / cozinha → depósito trancado.

Builder: `PensaoTerreoBlockout01Builder.cs` (geometria sob Exterior / PensionGroundFloor / Interactions).

---

## Testar

Godot → `PensaoTerreoBlockout01.tscn` → **F6**

Spawn na trilha (z=45), olhar para -Z, caminhar até depósito.

---

## Próxima ação

**Sprint 06** — playtest QA, vídeo, correções de colisão/escala.

---

## Regra

Baselines player/HUD/interação **não alterar** sem pedido explícito.
