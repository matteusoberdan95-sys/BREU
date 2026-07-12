# Ownership da cena — Pensão Santa Luzia

**Sprint:** 18B  
**Cena oficial:** `scenes/levels/pensao_santa_luzia/PensaoVerticalBlockout01.tscn`

## Regra de ouro

Cada área tem **um dono**.  
É proibido dois sistemas criarem a mesma porta/piso/prompt.

## Mapa de ownership

| Área | Dono | Cria geometria? | Cria interações? | Status |
|------|------|-----------------|------------------|--------|
| Térreo (shell, salas, depósito) | `PensaoVerticalBlockout01Builder` ← `PensaoTerreoBlockout01Builder` | Sim (runtime) | Via `PensaoTerreoPuzzleSetup` | Ativo — baseline |
| Segundo andar (corredor, 201/202, escada) | `PensaoVerticalBlockout01Builder` | Sim (runtime) | Inspect areas no builder | Ativo — baseline |
| Teto / forro / fachada | `PensaoVerticalBlockout01Builder` | Sim (runtime) | Não | Ativo — 18B selou GF |
| Ala da varanda (porta verde, banheiro, proprietária) | `areas/BalconyWing.tscn` + `ManualBalconyWingController` | **Manual** | Manual | Ativo |
| Expansão ala superior (laje, 204…) | `areas/UpperWingExpansion.tscn` | **Manual** | Manual | Ativo — congelar expansão até limpeza OK |
| Quarto 203 | `areas/Room203Door.tscn` | **Manual** | Manual | Ativo |
| Nota + chave da varanda | `PensaoBalconyPuzzleSetup` | Props mínimos | Sim | Ativo — sem geometria de ala |
| Puzzle depósito/fusível | `PensaoTerreoPuzzleSetup` | Não relevante | Sim | Ativo — não mexer |
| `PensaoBalconyWingPuzzleSetup` | Histórico | Criava props/areas | Sim | **Desativado** (`Enabled=false`, fora da cena) |
| `BuildUpperBalconyWing()` | Histórico | Criaria `BalconyWing_Rebuilt` | Sim | **Congelado** (early-return + warning) |

## Builders vivos vs congelados

**Vivos (permitidos):**
- `World/Builder` — pensão geral + teto
- `World/PuzzleSetup` — depósito
- `World/BalconyPuzzleSetup` — nota/chave + init porta verde existente

**Congelados / proibidos na árvore:**
- `BalconyWingPuzzleSetup` — não instanciar
- `BuildUpperBalconyWing` — não chamar
- qualquer novo builder de ala sem revisão de ownership

## Containers da Scene Tree

```
PensaoVerticalBlockout01
├── PuzzleState / NarrativeEvents / AudioManager / LevelSanityChecker
├── World/Builder + PuzzleSetup + BalconyPuzzleSetup
├── Exterior
├── PensionGroundFloor          [level_first_floor]
├── PensionSecondFloor          [level_second_floor]
├── BalconyWing                 [level_upper_wing]  ← manual
├── UpperWingExpansion          [level_upper_wing]  ← manual
├── Room203Door                 [level_door]
├── PensionCeiling              [level_ceiling]
├── Collisions / Interactions
├── Lighting / Player / HUD
```

## Grupos

- `level_first_floor`
- `level_second_floor`
- `level_upper_wing`
- `level_ceiling`
- `level_door`
- `gameplay_interaction` (usar em Area3D novas)
- `deprecated_do_not_use` (nunca deixar ativo na cena)

## Anti-acúmulo

1. Não criar segundo script que spawna a mesma ala.
2. Antes de commit: F4 → `LevelSanityChecker`.
3. Seguir `docs/production/LEVEL_CHANGE_CHECKLIST.md`.
4. Expansão de cômodos **pausada** até playtest confirmar cena limpa.
