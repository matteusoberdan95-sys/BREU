# Playtest — Atmosfera base (Sprint 13)

**Cena:** `res://scenes/levels/pensao_santa_luzia/PensaoVerticalBlockout01.tscn`  
**Sprint:** 13 — Atmosfera base da Pensão  
**Data:** 2026-07-11  
**Baseline:** `docs/technical/PENSION_ATMOSPHERE_BASELINE.md`

---

## Status

Implementado — validar F6 antes de aprovar.

---

## Rota de teste

Trilha → entrada → recepção → quarto 102 → chave → cozinha → depósito → fusível → escada → 2º andar → quartos 201/202 → porta bloqueada → retorno térreo

---

## Checklist técnico

- [ ] Player nasce corretamente
- [ ] Movimento / sprint / crouch / lean / look back
- [ ] HUD funciona
- [ ] Lanterna funciona
- [ ] F10 — lanterna infinita ON/OFF
- [ ] F11 — fog normal → off → debug (ciclo)
- [ ] Interações OK
- [ ] Puzzle depósito OK
- [ ] Escada OK
- [ ] 2º andar navegável
- [ ] Sem queda do mapa / atravessar parede

---

## Checklist visual

- [ ] Entrada mais assustadora / foco de luz
- [ ] Interior com contraste melhor
- [ ] Corredores com profundidade
- [ ] Neblina suave na distância
- [ ] **Sem quadrados/cubos/planos de fog**
- [ ] Lanterna ilumina bem no escuro
- [ ] Escada legível
- [ ] 2º andar mais ameaçador que térreo

---

## Checklist performance

- [ ] FPS aceitável
- [ ] Sem lag perceptível na trilha/corredores

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

- [ ] Layout blockout intacto
- [ ] Teto/escada/puzzle/HUD/movimento preservados
