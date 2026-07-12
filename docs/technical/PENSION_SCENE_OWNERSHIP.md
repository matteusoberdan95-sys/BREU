# Ownership da cena — Pensão Santa Luzia

**Sprint:** 18C  
**Cena oficial:** `scenes/levels/pensao_santa_luzia/PensaoVerticalBlockout01.tscn`

## Regra de ouro

Cada área tem **um dono**.  
É proibido dois sistemas criarem a mesma porta/piso/prompt.  
**Expansão de cômodos está pausada** até LevelSanity (F9) = 0 ERROR / 0 WARNING grave.

## Mapa de ownership

| Área | Dono | Status |
|------|------|--------|
| Térreo | `PensaoTerreoBlockout01Builder` via `World/Builder` | Ativo (baseline) |
| 2º andar estrutural (corredor/201/202/escada) | `PensaoVerticalBlockout01Builder` | Ativo (baseline) |
| Teto / forro GF | `BuildFirstFloorCeilings` | Ativo — selo 18B/18C |
| Ala varanda (porta verde, banheiro, proprietária) | `areas/BalconyWing.tscn` | Manual — único dono |
| Faixa liberada / walkway frontal | `areas/UpperWingExpansion.tscn` | Manual — **só laje + rail** (18C) |
| Quarto 203 | `areas/Room203Door.tscn` | Manual |
| Nota + chave varanda | `PensaoBalconyPuzzleSetup` | Ativo — sem geometria de ala |
| Depósito/fusível | `PensaoTerreoPuzzleSetup` | Ativo — não mexer |
| `BuildUpperBalconyWing` | Histórico | **Congelado** (no-op) |
| `PensaoBalconyWingPuzzleSetup` | Histórico | **Fora da cena / Enabled=false** |

## Proibido recriar em runtime

- segundo andar “extra” / `BalconyWing_Rebuilt`
- salas da ala / varanda / portas / prompts / blockers da microárea
- qualquer `BalconyWingPuzzleSetup` ativo

## Validador

- Script: `scripts/debug/LevelSanityChecker.cs`
- Atalho: **F9**
- Critério de passagem: **0 ERROR** (warnings graves também devem ser zerados)

## Checklist

`docs/production/LEVEL_CHANGE_CHECKLIST.md` — obrigatório antes de commit de cenário.
