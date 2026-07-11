# BREU — Pensão Térreo Blockout (Baseline)

**Versão:** 1.1  
**Data:** 2026-07-11  
**Sprint:** 05–06 — aprovada e congelada  
**Status:** OFICIAL — não alterar planta do térreo sem nova sprint

---

## Regra de congelamento

> **Não modificar** planta, fluxo ou layout geral do térreo **sem solicitação explícita do usuário** ou sprint dedicada (ex.: Sprint 07 puzzle, Sprint 08 escada).

Correções finas de playtest (Sprint 06) foram aplicadas **sem refazer a cena**.

Baselines paralelas congeladas:
- Player: `PLAYER_CONTROLLER_BASELINE.md`
- HUD: `HUD_DEBUG_BASELINE.md`
- Interação: `INTERACTION_SYSTEM_BASELINE.md`

---

## Cena oficial

| Item | Valor |
|------|-------|
| Cena | `res://scenes/levels/pensao_santa_luzia/PensaoTerreoBlockout01.tscn` |
| Builder | `scripts/levels/pensao_santa_luzia/PensaoTerreoBlockout01Builder.cs` |
| Controller | `scripts/levels/pensao_santa_luzia/PensaoTerreoBlockout01Controller.cs` |
| Playtest | `docs/testing/PENSAO_TERREO_BLOCKOUT_01_PLAYTEST.md` |

---

## Métricas aprovadas (Sprint 06)

| Métrica | Valor |
|---------|-------|
| Capsule player | raio 0,35 m · altura 1,8 m |
| Corredor | 2,4 m livres |
| Portas | 1,4 m × 2,3 m |
| Paredes | 0,2 m espessura · 3,0 m altura |
| Piso visual | 0,20 m espessura |
| Piso colisão | 3 lajes contínuas (overlap 0,08 m) |
| Shell edificação | ~14 m × ~44 m |
| Raycast interação | 3,0 m (baseline Sprint 04) |
| Interactables | 5 pontos fixos |

---

## Layout (inalterado desde Sprint 05)

```
Trilha → Varanda → Recepção → Corredor → Quarto 102 / Cozinha → Depósito trancado
```

**Colisão:** `Exterior_MainGround`, `Porch_MainFloor`, `PensionGroundFloor_MainFloor`

**Ajustes Sprint 06 (permitidos):**
- Balcões recepção/cozinha = visual-only
- Placa exterior reposicionada
- Áreas interactable ampliadas
- Luzes playtest: depósito, quarto, cozinha
- `Floor_Corridor_Readability` (tom mais escuro)

---

## Limitações conhecidas (aceitas)

| Item | Notas |
|------|-------|
| Exterior | Lote fechado simples; sem barranco |
| Terreno | Plano blockout; terreno artístico futuro |
| Atmosfera | Fog/ambient playtest básico |
| Arte | Caixas cinza/marrons; sem GLB/Blender |
| Teto | Aberto (céu visível) |
| Escada / 2º andar | Não implementados |
| Puzzle depósito | Porta trancada placeholder (Sprint 07) |

---

## Critérios Sprint 06 (atingidos)

- [x] Rota principal completa
- [x] Sem queda em navegação normal
- [x] Paredes principais bloqueiam
- [x] Sem prender em cantos críticos (balcões)
- [x] 5 interações funcionais
- [x] HUD intacto
- [x] Movimento aprovado intacto
- [x] Sem limbos grandes internos

---

## Commits de referência

| Commit | Descrição |
|--------|-----------|
| `d08184b` | feat: add pension ground floor blockout |
| `408ada1` | fix: close ground floor gaps |
| `2553f2e` | fix: resolve floor z-fighting |
| `77ae1c1` | fix: seal visual gaps |
| `3f71a0b` | feat: approve pension ground floor blockout baseline |
| _(Sprint 06)_ | fix: fine tune pension ground floor playtest issues |
