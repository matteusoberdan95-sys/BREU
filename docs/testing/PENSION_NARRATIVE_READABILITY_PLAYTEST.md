# Playtest — Leitura narrativa (Sprint 14)

**Cena:** `res://scenes/levels/pensao_santa_luzia/PensaoVerticalBlockout01.tscn`  
**Sprint:** 14 — Portas, quartos e leitura narrativa  
**Data:** 2026-07-11  
**Baseline:** `docs/technical/PENSION_ROOM_READABILITY_BASELINE.md`

---

## Status

Implementado — validar F6 antes de aprovar.

---

## Rota testada

Trilha → entrada → recepção → quarto 102 → chave → cozinha → depósito → fusível → escada → 2º andar → quartos 201/202 → porta bloqueada → retorno

---

## Portas/molduras adicionadas

- `Door_MainEntrance_Frame` — varanda/entrada
- `Door_ReceptionSouth_Frame` / `Door_ReceptionCorridor_Frame`
- `Door_Room102_Frame` / `Door_Kitchen_Frame`
- `Door_Deposit_Frame` (+ `Door_Deposit_Blocked` puzzle)
- `Door_StairEntry_Frame`
- `Door_Room201_Frame` / `Door_Room202_Frame`
- `Door_UpperBlocked_Frame` / `Door_UpperBlocked_Locked`

---

## Interações adicionadas/atualizadas

| ID | Prompt |
|----|--------|
| reception_book | Ler livro de hóspedes |
| reception_keys | Examinar chaves |
| reception_counter | Examinar recepção |
| room_102_bed | Examinar cama |
| room_102_suitcase | Examinar mala |
| kitchen | Examinar cozinha |
| kitchen_stove | Examinar fogão |
| kitchen_cabinet | Examinar armário |
| deposit_shelf | Examinar prateleira |
| stair_inspect | Examinar escada |
| room_201 / room_201_note | Examinar quarto 201 / Ler anotação |
| room_202 / room_202_cabinet | Examinar quarto 202 / Examinar armário |
| room_203_locked | Tentar abrir porta |
| Puzzle preservado | Pegar chave / Usar chave / Pegar fusível / Ler bilhete |

---

## Checklist técnico

- [ ] Movimento / HUD / lanterna / F10 / F11
- [ ] Fog intacto (sem quadrados)
- [ ] Puzzle depósito completo
- [ ] Escada / 2º andar navegáveis
- [ ] Sem prompts através de parede
- [ ] Sem mensagens vazias
- [ ] Sem prender em móveis

---

## Checklist visual

- [ ] Recepção, 102, cozinha, depósito legíveis
- [ ] Portas abertas vs trancadas compreensíveis
- [ ] Escada e quartos superiores legíveis
- [ ] Ainda blockout, mais narrativa

---

## Regressão

- [ ] Atmosfera Sprint 13 preservada
- [ ] Estrutura S05–12A preservada
