# Auditoria de builders — Pensão (atualizado Sprint 18B)

Ver também: `docs/technical/PENSION_SCENE_OWNERSHIP.md`

## Resultado 18B

| Nó / script | Geometria | Interações | Status |
|---|---|---|---|
| `World/Builder` | Térreo + 2º + teto | Inspect 2º | Ativo |
| `OpenFrontWallForBalconyPassage` | Só gap estrutural | Não | Ativo (necessário) |
| `BuildUpperBalconyWing` | — | — | **Congelado** (no-op) |
| `PensaoBalconyWingPuzzleSetup` | — | — | Fora da cena / `Enabled=false` |
| `PensaoBalconyPuzzleSetup` | Nota/chave | Init porta verde | Ativo |
| `BalconyWing.tscn` | Ala varanda | Puzzle varanda | Dono manual |
| `UpperWingExpansion.tscn` | Laje/expansão | 18A | Dono manual |
| `Room203Door.tscn` | Porta 203 | 203 | Dono manual |
| `LevelSanityChecker` | Não | Não | F4 debug |

## Critério

- Uma instância de `BalconyWing`
- Zero `BalconyWing_Rebuilt`
- Forro `Ceiling_FirstFloor_Seal` presente
- Checklist: `docs/production/LEVEL_CHANGE_CHECKLIST.md`
