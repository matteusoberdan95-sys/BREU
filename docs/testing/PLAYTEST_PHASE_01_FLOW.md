# BREU - Playtest do fluxo da Fase 1

## Cena inicial

A cena principal do projeto agora e:

`res://scenes/levels/trail_intro/TrailIntro.tscn`

Ao apertar Play no Godot, o fluxo esperado e:

```text
TrailIntro -> HouseExterior -> DemoRoom
```

## Objetivo do teste

Validar se a primeira caminhada funciona como uma experiencia continua:

1. O player nasce no inicio da Trilha Noturna.
2. A silhueta escura da Pensao aparece no fim da trilha.
3. Ao chegar perto da luz/casa, ocorre fade para `HouseExterior.tscn`.
4. Na fachada real, mirar na porta mostra o prompt de interacao.
5. Apertar `E` troca com fade para `DemoRoom.tscn`.
6. No Quarto 07, a mensagem inicial aparece no HUD e o playtest continua.

## Mensagens esperadas

- Trilha: `A luz nao deveria estar acesa.`
- Chegada: `A Pensao Santa Luzia.`
- Fachada: `Tem cheiro de vela queimada.`
- Porta: `A porta range como se ja estivesse aberta por dentro.`
- Quarto: `O quarto parece preparado para alguem.`

## Checkpoints em memoria

O `CheckpointManager` ainda nao salva em disco. Ele apenas registra no console:

- `TrailIntro_Start`
- `HouseExterior_Entrance`
- `DemoRoom_Quarto07`

Isso serve para debug e prepara o futuro sistema de save/checkpoint.

## Validacao rapida

1. Abrir o projeto no Godot 4.7 Mono.
2. Apertar Play.
3. Clicar na aba/janela **Entrada** se o mouse nao capturar.
4. Caminhar pela trilha usando `WASD`.
5. Confirmar fade para a fachada.
6. Mirar na porta da fachada e apertar `E`.
7. Confirmar fade para o Quarto 07.

## Problemas conhecidos

- A silhueta da Pensao na trilha e propositalmente simples; a fachada detalhada existe apenas em `HouseExterior.tscn`.
- As colisoes da trilha e fachada ainda sao temporarias.
- O checkpoint ainda nao restaura posicao nem estado do inventario.
