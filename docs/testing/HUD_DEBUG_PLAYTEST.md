# HUD e Debug — Playtest (Sprint 03)

**Cena:** `res://scenes/test/PlayerMovementLab.tscn`  
**Data aprovação:** 2026-07-11  
**Status:** ✅ Aprovado pelo usuário

---

## Checklist — HUD

| Teste | Esperado | OK |
|-------|----------|-----|
| Iniciar lab (F6) | Painel inferior esquerdo: Vida, Stamina, Lanterna | [x] |
| Sprint (Shift) | Barra e label de stamina descem | [x] |
| Parar sprint | Stamina regenera após ~0,75 s | [x] |
| F — lanterna | Label muda Ligada/Desligada | [x] |
| Mensagem inicial | Texto central inferior por ~3,5 s | [x] |
| F9 reset | Mensagem "Posicao resetada" | [x] |
| Centro da tela | Livre — HUD não atrapalha | [x] |

---

## Checklist — Debug

| Teste | Esperado | OK |
|-------|----------|-----|
| Painel superior direito | Mostra estado F10/F11 | [x] |
| F10 (toggle inf.) | Mensagem debug + label lanterna "(debug inf.)" | [x] |
| F10 ON + F ligada 10+ min | Bateria **permanece** 100/100 | [x] |
| F11 | Fog fica mais clara (densidade reduzida) | [x] |
| F11 novamente | Fog volta ao normal | [x] |

---

## Checklist — Regressão player

| Teste | Esperado | OK |
|-------|----------|-----|
| WASD | Direções corretas | [x] |
| Sprint / crouch / lean / look back | Igual Sprint 02 aprovada | [x] |
| Camera feel | Sem regressão perceptível | [x] |

---

## Baseline congelada

Ver: `docs/technical/HUD_DEBUG_BASELINE.md`
