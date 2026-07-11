# Hotfix 12A — Fechamento teto, casca externa e frestas

**Cena:** `res://scenes/levels/pensao_santa_luzia/PensaoVerticalBlockout01.tscn`  
**Sprint:** 12A  
**Data:** 2026-07-11  
**Status:** ✅ **Aprovada** — blockout funcional  
**Baseline:** `docs/technical/PENSION_CEILING_BLOCKOUT_BASELINE.md`

---

## Problemas confirmados (playtest Sprint 12)

### A — Fachada frontal / entrada
- Cobertura superior parecia volume flutuando sobre a varanda/entrada.
- Telhado único cobria toda a profundidade (incluindo área de 1 pavimento) em y≈6 m.

### B — Fresta na escada
- Vão do poço sem teto interno — limbo visível ao olhar para cima.
- Lacunas laterais (leste/sudeste) entre parede parcial e teto.

### C — Área aberta ligada à porta verde (UpperBlockedDoor)
- Espaço entre a porta trancada (z=-7,5) e a fachada superior (z=-5,8) sem paredes laterais internas.
- Leitura de volume cortado/aberto para o vazio.

---

## Soluções aplicadas

### A — Casca externa
- `Roof_Blockout_Main` — apenas sobre volume do 2º andar (z: -32,6 a -5,8), alinhado ao topo das paredes (y=5,8).
- `Roof_Blockout_LowerFront` — cobertura baixa (y≈3,0) sobre frente/varanda (z: -5,8 a 11,6).
- `Shell_FacadeUpper_FrontLeft/Right` — massa superior nas alas da fachada.
- `Shell_FacadeUpper_SideWest/East` — continuidade lateral da massa superior.
- `Shell_FacadeUpper_Parapet` — faixa simples no topo do 2º pavimento.
- Removido `Roof_Blockout_Ridge` (leitura de placa flutuante).

### B — Caixa da escada
- `Ceiling_StairBox_Main` — teto visual sobre todo o vão da escada (y=5,8).
- `Ceiling_StairBox_EastSeal` — faixa leste externa ao poço.
- `Ceiling_StairBox_SouthEastCap` — cap sudeste acima do patamar.
- `Ceiling_StairBox_LandingSeal` — selagem acima da transição landing/corredor.
- Mantida `StairBox_Wall_East` parcial (58%) para não bloquear saída do patamar.

### C — Cômodo placeholder sul (porta verde)
- `Wall_UpperSouthRoom_Left/Right` — fechamento lateral do volume atrás da porta.
- `Ceiling_UpperSouthRoom` — teto do placeholder (futuro cômodo/circulação).

---

## Checklist aprovado (2026-07-11)

- [x] Fachada fechada — volume superior coerente, sem placa flutuante
- [x] Teto/cobertura aceitos como blockout provisório
- [x] Caixa da escada melhor fechada — sem limbo crítico visível
- [x] Navegação preservada — térreo, escada, 2º andar
- [x] HUD preservado
- [x] Interação preservada
- [x] Puzzle do depósito preservado
- [x] Lanterna e movimentação preservadas

**Aprovação usuário:** Pensão não parece mais aberta por cima; blockout funcional aceito.

---

## Pendências restantes (fora de escopo — sprint futura)

- Telhado final (duas águas, beiral) — sprint de arte.
- Ajuste fino anti z-fighting nos encontros teto/parede.
- Teto modular com clipping zero (Sprint 14 roadmap).
