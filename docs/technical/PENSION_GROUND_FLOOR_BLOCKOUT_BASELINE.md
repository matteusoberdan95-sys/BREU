# BREU — Pensão Térreo Blockout (Baseline)

**Versão:** 1.0  
**Data:** 2026-07-11  
**Sprint:** 05 — aprovada e congelada  
**Status:** OFICIAL — não alterar sem nova sprint de level/térreo

---

## Regra de congelamento

> **Não modificar** geometria, colisão, fluxo ou interactables do térreo **sem solicitação explícita do usuário** ou sprint dedicada (ex.: Sprint 06 — correção fina).

Baselines paralelas congeladas:
- Player: `PLAYER_CONTROLLER_BASELINE.md`
- HUD: `HUD_DEBUG_BASELINE.md`
- Interação: `INTERACTION_SYSTEM_BASELINE.md`

Sprints futuras (06+) **estendem ou refinam** este blockout — não reescrevem player, HUD ou core de interação.

---

## Cena oficial

| Item | Valor |
|------|-------|
| Cena | `res://scenes/levels/pensao_santa_luzia/PensaoTerreoBlockout01.tscn` |
| Builder | `scripts/levels/pensao_santa_luzia/PensaoTerreoBlockout01Builder.cs` |
| Controller | `scripts/levels/pensao_santa_luzia/PensaoTerreoBlockout01Controller.cs` |
| Playtest | `docs/testing/PENSAO_TERREO_BLOCKOUT_01_PLAYTEST.md` |

**F6:** abrir cena acima → playtest pensão térreo.

---

## Layout atual (blockout cinza)

Fluxo aprovado:

```
Trilha → Varanda → Recepção → Corredor → Quarto 102 / Cozinha → Depósito trancado
```

| Área | Dimensões / notas |
|------|-------------------|
| Trilha + exterior | Lote simples fechado; trilha elevada; bermas laterais |
| Varanda | Entrada principal; conexão com recepção |
| Recepção | Balcão lateral; vãos 1,4 m norte/sul |
| Corredor | 2,4 m de largura; portas 1,4 m × 2,3 m |
| Quarto 102 | Esquerda do corredor; cama blockout |
| Cozinha | Direita do corredor; balcão blockout |
| Depósito | Fim do corredor; porta trancada + interação |

**Colisão (3 lajes contínuas):**
- `Exterior_MainGround`
- `Porch_MainFloor`
- `PensionGroundFloor_MainFloor`

**Visual (pisos principais):**
- `Floor_Exterior_Main_Visual` + `Floor_Exterior_Trail`
- `Floor_PensionGround_Main_Visual` + `Floor_Porch_Main`
- Shell externo + paredes internas fechadas (hotfix 2)

**5 interactables:**
1. Placa exterior (`JobOfferSign`)
2. Livro recepção (`ReceptionBook`)
3. Quarto 102 (`Room102Inspect`)
4. Cozinha (`KitchenInspect`)
5. Depósito (`StorageDoorInteract`)

---

## O que está fora do escopo desta baseline

Não implementar dentro desta sprint/baseline sem nova sprint:

| Pendência | Sprint sugerida |
|-----------|-----------------|
| Barrancos / desnível externo | 06+ |
| Terreno externo artístico | 06+ / 11 |
| Vegetação | 15+ |
| Atmosfera avançada (fog, luz fina) | 11 |
| Arte modular / materiais finais | 15 |
| Teto | 10 |
| Escada | 08 |
| Segundo andar | 09 |
| Puzzle depósito (chave/fusível) | 07 |
| Blender / GLB | — |

---

## Critérios de aprovação (2026-07-11)

- [x] Player nasce na trilha e chega à pensão
- [x] Térreo navegável (recepção, corredor, cômodos)
- [x] Chão fechado visualmente; sem limbos grandes internos
- [x] Player não cai durante navegação normal
- [x] HUD e interação intactos
- [x] Depósito trancado com prompt
- [x] Exterior simples aceito como blockout temporário

---

## Commits de referência

| Commit | Descrição |
|--------|-----------|
| `d08184b` | feat: add pension ground floor blockout |
| `408ada1` | fix: close ground floor gaps in pension blockout |
| `2553f2e` | fix: resolve floor z-fighting and visual gaps |
| `77ae1c1` | fix: seal visual gaps in pension ground floor blockout |

**Baseline congelada:** commit `9b08dc5` — `feat: approve pension ground floor blockout baseline`
