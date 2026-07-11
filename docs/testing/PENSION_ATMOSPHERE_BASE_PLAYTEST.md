# Playtest — Atmosfera base (Sprint 13)

**Cena:** `res://scenes/levels/pensao_santa_luzia/PensaoVerticalBlockout01.tscn`  
**Sprint:** 13 — Atmosfera base da Pensão  
**Data:** 2026-07-11  
**Status:** ✅ **Aprovada**  
**Baseline:** `docs/technical/PENSION_ATMOSPHERE_BASELINE.md`

---

## Checklist aprovado (2026-07-11)

- [x] Luz exterior — entrada com foco; trilha legível
- [x] Luz do térreo — recepção/corredor/quartos/depósito graduados
- [x] Luz do segundo andar — mais sombrio e ameaçador
- [x] Lanterna — importância real no escuro
- [x] Neblina suave — profundidade nos corredores
- [x] Sem quadrados na neblina
- [x] Sem cubos/planos/meshes visíveis de fog
- [x] HUD/debug preservados (F10/F11)
- [x] Puzzle do depósito preservado
- [x] Escada preservada e navegável
- [x] Segundo andar preservado e navegável
- [x] Player movement aprovado intacto
- [x] Interações preservadas

**Aprovação usuário:** atmosfera base aceita; cena ainda em blockout, sem arte final.

---

## Rota validada

Trilha → entrada → recepção → quarto 102 → chave → cozinha → depósito → fusível → escada → 2º andar → quartos 201/202 → porta bloqueada → retorno térreo

---

## Configuração registrada

### Exterior
- `MoonLight` — fria, 0.26 energy
- `EntranceLight` — âmbar, 1.38 energy (foco)
- `TrailFillLight` — fill distante fraco

### Térreo
- Recepção quente fraca; corredor frio; depósito mais escuro

### 2º andar
- Corredor mais escuro; `UpperBlockedDoorLight` com destaque

### Fog
- Environment global — density 0.026, cor azul-cinza
- F11: normal / off / debug (0.072)

---

## Regressão

- [x] Layout blockout intacto
- [x] Teto/escada/puzzle/HUD/movimento preservados
