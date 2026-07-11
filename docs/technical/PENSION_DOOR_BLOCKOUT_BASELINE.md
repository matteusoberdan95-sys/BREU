# Baseline — Portas blockout (Pensão)

**Versão:** 1.0  
**Sprint:** 14A  
**Data:** 2026-07-11  
**Cena:** `res://scenes/levels/pensao_santa_luzia/PensaoVerticalBlockout01.tscn`

---

## Padrões

### Tipo A — Porta aberta
- Moldura (`Door_*_Frame`) — visual-only
- Folha aberta estática (`Door_*_Leaf`) — offset lateral, sem colisão
- Vão livre para o player

### Tipo B — Porta trancada
- Moldura + painel visível
- Colisão bloqueando passagem
- Interação com prompt E

### Tipo C — Depósito (destravável)
```
Door_Deposit/
  Door_Deposit_Frame
  Door_Deposit_Panel          — some ao destrancar
  Door_Deposit_Blocking/
    Door_Deposit_Collision    — desativa ao destrancar
  DepositDoorInteraction
```

**Regra crítica:** ao destrancar, **não** usar scale, animação ou reposicionamento.
- `Door_Deposit_Panel.Visible = false`
- `Door_Deposit_Collision.Disabled = true`
- Moldura permanece

---

## Porta verde / varanda

- Nó: `Door_UpperBalcony_Locked` (painel verde, colisão)
- Prompt: **Tentar abrir varanda**
- Mensagem: vento / porta emperrada
- Área: `UpperBalcony_Placeholder` + guarda-corpos
- **Não abre** nesta fase — acesso futuro

---

## Proibido (blockout)

- Animar porta com `Scale`
- Girar porta sem pivot correto
- Mover painel para canto da parede ao abrir
- Cubos/mesh de fog (Sprint 13)

---

## Playtest

`docs/testing/PENSION_NARRATIVE_READABILITY_PLAYTEST.md` — seção Sprint 14A
