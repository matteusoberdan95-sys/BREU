# Baseline — Interação / puzzles locais (Pensão)

**Atualização:** Sprint 17 (2026-07-12)

## Princípios

- Interações via `IInteractable` + `Area3D` layer 2.
- Puzzles locais em `PensaoPuzzleState` (não inventário global).
- Portas destraváveis: padrão **esconder painel + desativar BlockingShape** (`BlockoutUnlockHideDoor` / `BlockoutBalconyDoor`).
- Sem animação de porta, sem scale, sem moldura complexa.

## Flags — depósito (Sprint 07)

- `HasDepositKey`
- `IsDepositUnlocked`
- `HasOldFuse`

## Flags — varanda (Sprint 17)

- `HasReadBalconyNote`
- `HasBalconyKey`
- `IsBalconyUnlocked`

## Setup

- `PensaoTerreoPuzzleSetup` — depósito / fusível  
- `PensaoBalconyPuzzleSetup` — nota / chave / porta verde / notas da ala
