# Design — Ala da Varanda + Puzzle Macabro (Sprint 17C)

**Cena:** `scenes/levels/pensao_santa_luzia/PensaoVerticalBlockout01.tscn`  
**Status:** Implementada — playtest F6 pendente  
**Tom:** terror psicológico brasileiro; pensão úmida; sangue seco pontual; sem inimigo/combate/chase.

---

## Varanda

| Node | Função |
|------|--------|
| `BalconyLanding` | Patamar nivelado imediatamente após a porta verde |
| `BalconyWalkable` | Piso sólido e contínuo da varanda |
| `BalconyRail_Front` | Guarda-corpo da borda externa |
| `BalconyRail_Left` | Fechamento lateral oeste |
| `BalconyRail_Right` | Alias do fechamento leste feito pelas fachadas dos cômodos |
| `Interact_BalconyLookDown` | "Está alto demais para descer por aqui." |

Sem sistema de queda. Sem saída externa.

---

## Cômodos

### `Room_Bathroom` (aberto)

Blockout: pia, vaso, manchas, espelho escuro.

| Interação | Sem arame | Com arame |
|-----------|-----------|-----------|
| `Interact_BathroomMirror` | Marcas de dedos + flag `HasExaminedBathroomMirror` | — |
| `Interact_BathroomDrain` | Não alcança | Puxa chave → `HasOwnerRoomKey` |

### `Room_OwnerBedroom` (trancado)

| Node | Função |
|------|--------|
| `Room_OwnerDoor` / `Room_OwnerDoor_Panel` | Painel some + colisão off ao destravar |
| `Interact_OwnerLedger` | Caderno → evento macabro |

Props: cama, mesa, armário, manchas, marcas de unha.

---

## Puzzle

```
Varanda: arame (HasWireHook)
    → Banheiro: ralo + arame → chave (HasOwnerRoomKey)
    → Quarto: destravar (IsOwnerRoomUnlocked)
    → Caderno (HasReadOwnerLedger)
    → Event_OwnerLedger_Reveal
    → Objetivo: Quarto 203 (ainda bloqueado)
```

Flags em `PensaoPuzzleState` (17C):
- `HasWireHook`
- `HasExaminedBathroomMirror`
- `HasOwnerRoomKey`
- `IsOwnerRoomUnlocked`
- `HasReadOwnerLedger`

---

## Item — arame

| | |
|--|--|
| Node | `Interact_BalconyWireHook` |
| Prompt | Pegar arame torto |
| Uso | Permite puxar chave do ralo |

---

## Caderno + evento

Após ler:

1. Mensagens do caderno (nomes / "NÃO ABRA O QUARTO 203")
2. Luz pisca ~1–2 s
3. Knock + passos distantes
4. "Alguma coisa se moveu do outro lado da pensão."
5. "Preciso voltar para o corredor."
6. "Quarto 203..."

**Sem** inimigo, corpo, chase, tranca de saída, apagar lanterna.

Evento: `PensionNarrativeEvents.EventOwnerLedgerReveal`

---

## Quarto 203 (gancho próxima sprint)

| Node | `Door_Room203_Blocked` |
|------|-------------------------|
| Prompt | Tentar abrir quarto 203 |
| Mensagem | Algo pesado bloqueia a porta pelo outro lado. |
| Regra | **Não abre** nesta sprint. |

---

## Áudio (assets existentes)

| Zona | Ambience |
|------|----------|
| Varanda | Exterior + leve segundo andar |
| Banheiro | Segundo andar + `pension_water_drops_loop` + one-shots |
| Quarto proprietária | Segundo andar + lamp buzz |

---

## Rebuild controlado — Sprint 17E

- A varanda é uma área de transição: corredor superior → porta verde → varanda → banheiro/quarto da proprietária → retorno.
- A microárea anterior foi removida e refeita como uma unidade, sem corredor intermediário ou remendos de piso.
- `Room_Bathroom` e `Room_OwnerBedroom` permanecem diretamente acoplados à varanda, com largura livre para entrar, virar e sair.
- `Interact_BalconyLookDown` pertence somente à borda externa; sua área é baixa e rasa para exigir que o jogador olhe para baixo junto ao guarda-corpo.
- Objetos decorativos nunca podem ocupar a rota principal nem o espaço imediatamente após uma porta.
- O guarda-corpo serve apenas para impedir queda; não existe rail interno entre a porta verde, o patamar e a varanda.
- A porta da proprietária é a única interação de porta nessa ala; painel, colisão e área de interação são desativados após destravar.

## Scripts

- `PensaoUpperBalconyWingBuilder.cs` — geometria
- `PensaoBalconyWingPuzzleSetup.cs` — interações
- `BlockoutOwnerBedroomDoor.cs` — unlock hide
- `PensaoPuzzleState.cs` — flags
- `PensionNarrativeEvents.cs` — reveal

Preserva: porta verde (17/17B), puzzle depósito, player/HUD/fog/áudio base.
