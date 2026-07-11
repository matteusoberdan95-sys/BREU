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

---

## Sprint 14A — Door readability and deposit door fix

**Data:** 2026-07-11  
**Baseline:** `docs/technical/PENSION_DOOR_BLOCKOUT_BASELINE.md`

### Correções

- **Depósito:** `Door_Deposit` reestruturado (Frame / Panel / Collision / Interact). Destravar = painel some + colisão desativa — sem scale/animação.
- **Molduras térreo:** entrada, quarto 102, cozinha, depósito — leitura Tipo A/B/C.
- **2º andar:** quartos 201/202 com moldura + folha aberta.
- **Porta verde → varanda:** `Door_UpperBalcony_Locked` — prompt *Tentar abrir varanda*; trancada.
- **Varanda placeholder:** `UpperBalcony_Placeholder` + guarda-corpos; piso fechado; sem acesso jogável.

### Portas/molduras (14A)

| Nó | Tipo |
|----|------|
| `Door_MainEntrance_Frame` | A — aberta |
| `Door_Room102_Frame` | A — aberta |
| `Door_Kitchen_Frame` | A — aberta |
| `Door_Deposit_*` | C — destravável |
| `Door_Room201_Frame` / `Door_Room202_Frame` | A — aberta |
| `Door_UpperBalcony_Locked` | B — varanda trancada |

### Checklist 14A

**Portas**
- [ ] Entrada / 102 / cozinha — moldura clara
- [ ] Depósito — porta clara; **não** estica/treme/canto da parede
- [ ] Depósito destranca com chave; passagem livre depois
- [ ] 201 / 202 — moldura clara
- [ ] Porta verde — prompt varanda; não abre; não atravessável

**Varanda**
- [ ] Frontal superior = varanda placeholder (piso + guarda-corpo)
- [ ] Não parece buraco; player não acessa

**Regressão**
- [ ] Movimento / HUD / lanterna / F10 / F11
- [ ] Fog intacto
- [ ] Puzzle chave → depósito → fusível
- [ ] Escada / 2º andar
- [ ] Prompts não atravessam parede

---

## Sprint 14B — Door system fix and balcony readability

**Data:** 2026-07-11  
**Baseline:** `docs/technical/PENSION_DOOR_BLOCKOUT_BASELINE.md` v1.1

### Removido (sistema quebrado 14/14A)

- Todas as folhas `Door_*_Leaf` (painéis flutuantes no vão)
- `AddOpenDoorLeafXWall` / `AddOpenDoorLeafZWall`
- Portas trancadas via `AddInteractableBody` + `_matInteractable` (verde translúcido)
- Molduras duplicadas no 2º andar (`AddSecondFloorDoorFrameIn*`)
- Moldura duplicada `Door_UpperBalcony_Frame` no placeholder
- Interação do depósito no `StaticBody3D` (agora em `Door_Deposit_InteractArea`)

### Padrão estável 14B

| Tipo | Implementação |
|------|---------------|
| A — aberta | Somente moldura, sem painel, sem colisão |
| B — trancada | `AddLockedDoorPanelZWall` — painel opaco + colisão WorldLayer + Area3D |
| C — depósito | Painel opaco + colisão; destravar = hide panel + disable collision |

### Varanda

- Interior: `UpperBalcony_Placeholder` (piso, guarda-corpos, parede fundo)
- Trilha: `UpperBalcony_TrailReadability` (piso saliente, guarda-corpo, painel verde)

### Checklist 14B

**Entrada**
- [ ] Sem porta grande bugada; só moldura; atravessável

**Térreo**
- [ ] 102 / cozinha — moldura, passagem livre
- [ ] Depósito — painel opaco bloqueia; destranca sem tremer/esticar
- [ ] Prompt depósito localizado na porta

**2º andar**
- [ ] 201 / 202 — moldura apenas
- [ ] `Door_UpperBalcony_Locked` — verde opaco, bloqueia, prompt varanda

**Varanda**
- [ ] Vista da trilha comunica varanda bloqueada
- [ ] Não parece buraco; sem acesso jogável

**Regressão**
- [ ] HUD / lanterna / F10 / F11 / fog / movimento / escada / puzzle intactos
