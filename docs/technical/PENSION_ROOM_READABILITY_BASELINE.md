# Baseline — Leitura narrativa de cômodos (Pensão)

**Versão:** 1.0  
**Sprint:** 14  
**Data:** 2026-07-11  
**Status:** Implementado — playtest F6 pendente  
**Cena:** `res://scenes/levels/pensao_santa_luzia/PensaoVerticalBlockout01.tscn`

---

## Objetivo

Blockout narrativo — portas, molduras, props simples e interações de texto para leitura espacial **sem arte final**.

---

## Padrão de portas

| Tipo | Colisão | Comportamento |
|------|---------|---------------|
| **Aberta (frame)** | Não | Moldura visual; passagem livre |
| **Trancada (depósito)** | Sim | `Door_Deposit_Blocked` + puzzle Sprint 07 |
| **Bloqueada superior** | Sim (interação) | `UpperBlockedDoor` — mensagem narrativa |

Molduras: `_matDoorFrame` — peças visual-only (laterais + lintel).

---

## Padrão de interação narrativa

- `Area3D` pequena, local ao objeto
- Prompt claro em português
- Mensagem única via `Interactable`
- Objetos puzzle (`PickupKeyInteraction`, `DepositDoorInteraction`) **não alterados**

---

## Colisão de móveis

| Com colisão | Sem colisão |
|-------------|-------------|
| Cama térreo, balcão, fogão, mesa, armários grandes | Bilhete, chave, fusível, anotação, livro |
| Cadeira recepção, prateleira depósito | Molduras de porta |
| Mesa quarto 201 | Marcas de arrasto (visual fino) |

---

## Cômodos e função narrativa

| Cômodo | Leitura | Interações principais |
|--------|---------|------------------------|
| **Recepção** | Abandonada, sem funcionário | Livro, chaves, balcão |
| **Quarto 102** | Quarto do jogador | Cama, mala, chave (puzzle) |
| **Cozinha** | Comida velha, uso recente | Bancada, fogão, armário |
| **Depósito** | Sombrio, perigoso | Prateleira, bilhete, fusível (puzzle) |
| **Escada** | Rangidos, transição | Examinar escada |
| **Quarto 201** | Cama usada | Quarto, anotação |
| **Quarto 202** | Arrasto, armário | Quarto, armário |
| **Porta superior** | Futuro cômodo trancado | Tentar abrir porta |

---

## Regras congeladas

1. **Não refazer** atmosfera (Sprint 13), geometria (S10–12A) ou puzzle (S07).
2. Prompts **não atravessam paredes** — Area3D compacta.
3. **Sem** sistema de portas animadas nesta sprint.
4. Blockout ≠ arte final — sem GLB/Blender.

---

## Playtest

`docs/testing/PENSION_NARRATIVE_READABILITY_PLAYTEST.md`

---

## Implementação

- `PensaoTerreoNarrativeBuilder.cs` — térreo
- `PensaoVerticalNarrativeBuilder.cs` — 2º andar
