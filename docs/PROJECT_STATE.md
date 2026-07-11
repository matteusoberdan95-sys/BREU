# BREU — Estado do projeto

**Última atualização:** 2026-07-11  
**Fase:** REBOOT GREENFIELD — Sprint 07 implementada  
**Baseline:** `docs/production/REBOOT_BASELINE_DECISION.md`

---

## Estado atual

| Item | Status |
|------|--------|
| Branch | `reboot/breu-clean-start` |
| Sprint 02–04 | **✅ Aprovadas** — player, HUD, interação |
| Sprint 05–06 | **✅ Aprovadas** — térreo blockout + fine playtest |
| Sprint 07 | **🔧 Hotfix** — puzzle OK; parede área futura bloqueada; validação F6 pendente |
| Sprint 08 | **Próxima** — Escada isolada (após aprovação Sprint 07) |

---

## Cena oficial

**F6:** `res://scenes/levels/pensao_santa_luzia/PensaoTerreoBlockout01.tscn`

---

## Sprint 07 — puzzle do depósito

**Loop:** depósito trancado → chave quarto 102 → destrancar → fusível (+ bilhete).

**Estado local** (`PensaoPuzzleState`):
- `HasDepositKey`
- `IsDepositUnlocked`
- `HasOldFuse`

**Scripts:** `DepositDoorInteraction`, `PickupKeyInteraction`, `PickupFuseInteraction`, `PensaoTerreoPuzzleSetup`

**Playtest:** `docs/testing/PENSAO_DEPOSIT_PUZZLE_PLAYTEST.md`

**Hotfix (2026-07-11):** `Wall_StairFuture_Blocker` + tampas alcove — área futura escada inacessível.

**Não alterado:** PlayerController, PlayerCameraFeel, HUD base, core `Interactable`/`PlayerInteractionRaycast`, lógica puzzle.

---

## Baselines congeladas

| Sprint | Documento |
|--------|-----------|
| 02 Player | `PLAYER_CONTROLLER_BASELINE.md` |
| 03 HUD | `HUD_DEBUG_BASELINE.md` |
| 04 Interação | `INTERACTION_SYSTEM_BASELINE.md` |
| 05–06 Térreo | `PENSION_GROUND_FLOOR_BLOCKOUT_BASELINE.md` |

---

## Relatórios

- Térreo: `docs/testing/PENSAO_TERREO_BLOCKOUT_01_PLAYTEST.md`
- Puzzle depósito: `docs/testing/PENSAO_DEPOSIT_PUZZLE_PLAYTEST.md`
- Histórico: `docs/SPRINT_HISTORY.md`
