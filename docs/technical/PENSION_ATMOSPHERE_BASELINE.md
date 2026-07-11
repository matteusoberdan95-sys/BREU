# Baseline — Atmosfera base (Pensão Santa Luzia)

**Versão:** 1.0  
**Sprint:** 13  
**Data:** 2026-07-11  
**Status:** Implementado — playtest F6 pendente  
**Cena:** `res://scenes/levels/pensao_santa_luzia/PensaoVerticalBlockout01.tscn`

---

## Objetivo

Atmosfera inicial de terror brasileiro — fria, úmida, abandonada — **sem alterar geometria aprovada**.

---

## Regra anti-quadrados (OBRIGATÓRIA)

A neblina **deve** ser feita exclusivamente via:

- `WorldEnvironment` → `Environment`
- `fog_enabled`, `fog_density`, `fog_light_color`
- `fog_sky_affect`, `fog_aerial_perspective`

**Proibido:**

- Cubos/planos/mesh transparentes de neblina
- Partículas ou sprites de fog
- Volumetric fog pesado se gerar artefatos visíveis

**Critério:** neblina = camada atmosférica na profundidade, nunca objeto na cena.

---

## WorldEnvironment — `Pension_WorldEnvironment`

| Parâmetro | Valor |
|-----------|-------|
| Background | Azul noite `(0.035, 0.045, 0.08)` |
| Ambient | Frio, energy **0.30** |
| Tonemap | ACES, exposure **0.82** |
| Adjustment | Contraste +10%, saturação -14% |
| Fog color | Azul-cinza `(0.26, 0.30, 0.40)` |
| Fog density | **0.026** (normal) |
| Fog aerial perspective | **0.45** |

---

## Debug F11 — modos de fog

| Modo | Comportamento |
|------|---------------|
| **fog normal** | Densidade da cena (0.026) |
| **fog off** | `fog_enabled = false` |
| **fog debug** | Densidade **0.072** (teste forte) |

Implementado em `PlaytestDebugSettings.cs` — ciclo F11.

HUD: `Debug F10: lanterna … | F11: fog normal/off/debug`

---

## Luzes principais

| Nó | Função | Tom |
|----|--------|-----|
| `MoonLight` | Lua/direcional exterior | Frio, baixo (0.26) |
| `EntranceLight` | Foco na entrada | Âmbar quente (1.38) |
| `TrailFillLight` | Trilha distante | Frio muito fraco |
| `ReceptionLight` | Recepção | Quente fraco |
| `CorridorLight` / `CorridorDeepLight` | Corredor térreo | Frio, profundidade |
| `Room102Light` | Quarto 102 | Escuro |
| `KitchenLight` | Cozinha | Amarelado desconfortável |
| `DepositLight` | Depósito | Muito escuro |
| `StairWellLight` / `StairShaftUpperLight` / `UpperLandingLight` | Escada | Legível, não claro |
| `UpperCorridorLight` / `UpperCorridorFarLight` | 2º andar | Mais escuro que térreo |
| `Room201Light` / `Room202Light` | Quartos superiores | Pouca luz |
| `UpperBlockedDoorLight` | Porta bloqueada | Destaque âmbar fraco |

**Total:** ~18 omni + 1 directional — sem exagero de sombras dinâmicas.

---

## Performance

- Sem volumetric fog mesh
- Sombras: apenas `MoonLight` (directional, max distance 48 m)
- Omni lights sem shadow (padrão Godot) — custo moderado
- Sem partículas de atmosfera

---

## Limitações / futuro

- Sem áudio diegético (rádio, rangidos)
- Sem eventos de tensão (porta batendo, etc.)
- Sem LUT/film grain final
- Atmosfera por volume (FogVolume) só se estável e invisível

---

## Playtest

`docs/testing/PENSION_ATMOSPHERE_BASE_PLAYTEST.md`

---

## Regra congelada

**Não refazer geometria** (térreo, 2º andar, escada, teto) ao ajustar atmosfera — apenas luz/environment.
