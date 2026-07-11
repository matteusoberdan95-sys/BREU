# Playtest — Teto e cobertura blockout (Sprint 12)

**Cena:** `res://scenes/levels/pensao_santa_luzia/PensaoVerticalBlockout01.tscn`  
**Sprint:** 12 — Teto e fechamento superior  
**Data:** 2026-07-11  
**Baseline:** `docs/technical/PENSION_CEILING_BLOCKOUT_BASELINE.md`

---

## Status

Implementado — validar F6 antes de aprovar.

---

## Checklist geral

- [ ] Player nasce na trilha
- [ ] Player entra na Pensão
- [ ] Térreo continua navegável
- [ ] Recepção continua navegável
- [ ] Quarto 102 continua acessível
- [ ] Cozinha continua acessível
- [ ] Depósito continua acessível
- [ ] Puzzle chave → depósito → fusível funciona
- [ ] Escada sobe/desce sem travar
- [ ] Câmera não atravessa teto na escada
- [ ] 2º andar navegável
- [ ] Room201 / Room202 acessíveis
- [ ] Porta bloqueada superior OK
- [ ] HUD OK
- [ ] Interações OK
- [ ] Movimento OK

---

## Checklist visual

- [ ] Térreo não parece aberto para limbo (varanda com teto)
- [ ] 2º andar mostra teto ao olhar para cima
- [ ] Poço da escada com abertura controlada (não limbo lateral)
- [ ] Exterior mostra `Roof_Blockout_Main`
- [ ] Sem z-fighting forte no teto
- [ ] Layout legível com lanterna off/on

---

## Regressão

### Térreo
- [ ] Corredor, recepção, depósito, puzzle intactos

### Segundo andar
- [ ] Corredor desbloqueado; landing acessível

### Escada
- [ ] Subida/descida suaves; guarda-corpos preservado

---

## Entregas Sprint 12

- `Ceiling_FirstFloor_Main` — frente/varanda
- `Ceiling_SecondFloor_Main` + segmentos
- `Ceiling_StairBox_*` — fechamento parcial poço
- `Roof_Blockout_Main` + `Roof_Blockout_Ridge`
- Luzes: `VarandaCeilingLight`, `SecondFloorCeilingLight`, `StairShaftUpperLight`
