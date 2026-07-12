# Auditoria de builders — Pensão (Sprint 18C)

Ver: `docs/technical/PENSION_SCENE_OWNERSHIP.md`

| Nó / script | Status |
|---|---|
| `World/Builder` | Ativo — térreo + 2º estrutural + teto |
| `OpenFrontWallForBalconyPassage` | Ativo — só gap da porta verde |
| `BuildUpperBalconyWing` | **Congelado** (no-op) |
| `PensaoBalconyWingPuzzleSetup` | Fora da cena / Enabled=false |
| `PensaoBalconyPuzzleSetup` | Ativo — nota/chave + init porta |
| `BalconyWing.tscn` | Dono manual da microárea |
| `UpperWingExpansion.tscn` | Dono manual — **só laje/walkway** (18C) |
| `LevelSanityChecker` | **F9** — 0 ERROR exigido |

## Critério

- Uma `BalconyWing`
- Zero `BalconyWing_Rebuilt`
- Forro `Ceiling_FirstFloor_Seal` presente
- F9 limpo antes de commit
