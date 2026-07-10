# BREU - Playtest do fluxo da Fase 1

## Cena inicial

A cena principal do projeto agora e:

`res://scenes/levels/trail_intro/TrailIntro.tscn`

Ao apertar Play no Godot, o fluxo esperado e:

```text
TrailIntro -> DemoRoom -> RitualRoom
```

## Objetivo do teste

Validar se a primeira caminhada funciona como uma experiencia continua:

1. O player nasce no inicio da Trilha Noturna.
2. A fachada real da Pensao aparece no fim da trilha.
3. O player caminha ate a varanda/porta da Pensao.
4. Na porta da fachada, mirar mostra o prompt de interacao.
5. Apertar `E` troca com fade direto para `DemoRoom.tscn`.
6. No Quarto 07, a mensagem inicial aparece no HUD e o playtest continua.
7. Abrir a porta do quarto, atravessar o corredor e interagir com a porta final.
8. Confirmar fade para `RitualRoom.tscn`, a Sala dos Santos Secos.

## Mensagens esperadas

- Trilha: `A luz nao deveria estar acesa.`
- Porta da Pensao: `A porta range como se ja estivesse aberta por dentro.`
- Quarto: `O quarto parece preparado para alguem.`
- Porta final: `A madeira cede para um comodo quente e escuro.`
- RitualRoom: `O ar aqui e quente demais.`
- Susto ritual: `As velas tremem sem vento.`

## Checkpoints em memoria

O `CheckpointManager` ainda nao salva em disco. Ele apenas registra no console:

- `TrailIntro_Start`
- `DemoRoom_Quarto07`
- `RitualRoom_SantosSecos`
- `Ritual_Note_Read`

Isso serve para debug e prepara o futuro sistema de save/checkpoint.

## Validacao rapida

1. Abrir o projeto no Godot 4.7 Mono.
2. Apertar Play.
3. Clicar na aba/janela **Entrada** se o mouse nao capturar.
4. Caminhar pela trilha usando `WASD`.
5. Confirmar que a fachada real da Pensao aparece no fim da trilha.
6. Mirar na porta da fachada e apertar `E`.
7. Confirmar fade direto para o Quarto 07.
8. No corredor, disparar o susto e usar a porta final.
9. Confirmar fade para a Sala dos Santos Secos.

## Persistencia do martelo entre cenas

1. No Quarto 07, pegar o `Martelo Enferrujado`.
2. Confirmar HUD:

```text
Arma: Martelo Enferrujado 10/10
```

3. Abrir a porta do quarto, atravessar o corredor e entrar na `RitualRoom`.
4. Confirmar que o HUD ainda mostra:

```text
Arma: Martelo Enferrujado 10/10
```

5. Confirmar que o martelo placeholder continua visivel na mao.
6. Pegar a `Chave Velha`.
7. Confirmar que pegar a chave nao remove o martelo nem muda o HUD para maos vazias.

## Combate basico na Sala dos Santos Secos

1. Com o martelo equipado, disparar o susto da `RitualRoom`.
2. Esperar o `EnemyPlaceholder` aparecer e perseguir o player.
3. Mirar no inimigo e clicar com o botao esquerdo.
4. Confirmar que o inimigo entra em stun.
5. Confirmar que o HUD reduz a durabilidade:

```text
Arma: Martelo Enferrujado 9/10
```

6. Repetir ataques ate a durabilidade chegar a 0.
7. Confirmar mensagem de quebra e HUD:

```text
Arma: Maos vazias
```

## Problemas conhecidos

- `HouseExterior.tscn` continua existindo como cena de teste isolado/comparacao, mas saiu do fluxo principal.
- A silhueta antiga da Pensao ficou desativada como fallback.
- As colisoes da trilha e da fachada integrada ainda sao temporarias.
- O checkpoint ainda nao restaura posicao do player.
- O estado do martelo e da Chave Velha persiste apenas em memoria enquanto o jogo esta rodando.
- O combate do martelo e prototipo por raycast, sem animacao final.
- A RitualRoom ainda tem porta de saida bloqueada.
