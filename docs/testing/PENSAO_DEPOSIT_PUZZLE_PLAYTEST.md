# Playtest — Puzzle do Depósito (Sprint 07)

**Cena:** `res://scenes/levels/pensao_santa_luzia/PensaoTerreoBlockout01.tscn`  
**Sprint:** 07 — puzzle depósito + hotfix parede  
**Data:** 2026-07-11

---

## Sprint 07 Hotfix — Parede atravessável (área futura)

| Item | Detalhe |
|------|---------|
| **Bug** | Player atravessava parede perto do bilhete/depósito e entrava em área futura (escada/cômodo) |
| **Causa** | Vão entre fundo do depósito (z=-31,5) e shell externo; alcoves laterais abertos na entrada |
| **Correção** | `Wall_StairFuture_Blocker` alinhado ao shell (z=-32,7); tampas `Wall_Deposit_AlcoveSouthCapWest/East` |
| **Status puzzle** | Fluxo chave → depósito → fusível **preservado** |

---

## Loop do puzzle

1. Depósito trancado → `[E] Tentar abrir depósito` → "Está trancado. Preciso encontrar uma chave."
2. Quarto 102 → `Key_Deposit_Old` → `[E] Pegar chave velha` → "Chave velha adquirida."
3. Depósito com chave → `[E] Usar chave velha` → "A porta do depósito destrancou."
4. Entrar no depósito → `[E] Pegar fusível velho` → "Fusível velho adquirido."
5. (Opcional) `[E] Ler bilhete` → "Não deixem os novos funcionários sozinhos depois das 22h."

---

## Checklist — Regressão

| Teste | OK |
|-------|-----|
| Player nasce na trilha | ☐ |
| Player entra na pensão | ☐ |
| Movimento aprovado | ☐ |
| Sprint / crouch / lean / look back | ☐ |
| HUD funciona | ☐ |
| Placa, livro, quarto, cozinha (interações antigas) | ☐ |

---

## Checklist — Puzzle

| Teste | OK |
|-------|-----|
| Depósito começa trancado | ☐ |
| Sem chave: mensagem correta | ☐ |
| Porta trancada bloqueia player | ☐ |
| Chave visível no quarto 102 | ☐ |
| Prompt pegar chave | ☐ |
| E pega chave + mensagem | ☐ |
| Chave some/desativa | ☐ |
| Com chave: prompt "Usar chave velha" | ☐ |
| E destranca porta | ☐ |
| Porta não bloqueia mais | ☐ |
| Player entra no depósito | ☐ |
| Fusível com prompt | ☐ |
| E pega fusível + mensagem | ☐ |
| Fusível some/desativa | ☐ |
| Bilhete (opcional) | ☐ |
| Sem caixa vazia / prompt preso | ☐ |

---

## Checklist — Colisão

| Teste | OK |
|-------|-----|
| Não atravessa paredes principais | ☐ |
| Não cai do mapa | ☐ |
| Não preso na porta (aberta ou fechada) | ☐ |
| Sai do depósito após entrar | ☐ |
| Não atravessa área futura atrás/lateral do depósito | ☐ |

---

## Estado do puzzle

| Flag | Descrição |
|------|-----------|
| `HasDepositKey` | Chave pegada no quarto 102 |
| `IsDepositUnlocked` | Porta destrancada |
| `HasOldFuse` | Fusível coletado |

Script: `PensaoPuzzleState.cs` (nó `PuzzleState` na cena).

---

## Critério gate

Fluxo completo: **quarto 102 → chave → depósito → fusível**.
