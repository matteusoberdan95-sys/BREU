# Design — Puzzle da Varanda (Sprint 17)

**Cena:** `PensaoVerticalBlockout01.tscn`  
**Status:** Implementado — playtest F6 pendente

## Objetivo narrativo

Dar motivo para explorar a Pensão após o fusível: a porta verde deixa de ser só “trancada” e vira acesso à ala superior frontal, preparando tensão **sem** inimigo físico.

## Porta verde

- Nó: `Door_UpperBalcony` + `Door_UpperBalcony_Blocker`
- Script: `BlockoutBalconyDoor`
- Sem chave: prompt *Tentar abrir varanda*
- Com chave: *Destravar varanda* → painel some + colisão desativa (padrão depósito)

## Nota da proprietária

- `Interact_Note_OwnerBalcony` no quarto 201
- Flag: `HasReadBalconyNote`
- Texto aponta a chave perto da recepção

## Chave da varanda

- `Interact_BalconyKey` atrás/embaixo do balcão da recepção
- Sem nota: *Examinar* → “nada chama atenção”
- Com nota: *Pegar chave da varanda*
- Flag: `HasBalconyKey`

## Ala superior

Grupo `UpperBalconyWing`:
- `UpperBalcony_Walkable` — piso com colisão + rails
- `Door_BalconyWing_Entry` — vão limpo (sem painel)
- `UpperBalconyWing_Corridor` — flanco leste do pocket sul
- `Room_203` — cama/mesa + bilhete
- `Room_OwnerOffice` — mesa/armário + caderno

## Preparação para inimigo

Caderno e bilhete sugerem presença no teto / hóspedes sumindo — **sem** vulto, chase ou combate nesta sprint.

## Flags

`HasReadBalconyNote` · `HasBalconyKey` · `IsBalconyUnlocked`
