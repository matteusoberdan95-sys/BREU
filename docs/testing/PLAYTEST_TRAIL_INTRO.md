# BREU - Playtest da Trilha Noturna

## Como executar

1. Abrir o projeto no Godot 4.7 Mono.
2. Abrir `res://scenes/levels/trail_intro/TrailIntro.tscn`.
3. Rodar a cena com F6.
4. Clicar na janela/aba **Entrada** para dar foco ao mouse e teclado.

## Onde o player comeca

O player nasce no inicio da trilha:

```text
X 0
Y 1.0
Z 14
```

No Godot, `Y` e altura e `Z` e profundidade. O blockout veio do Blender, onde a profundidade de trabalho era `Y`, entao a orientacao visual deve ser conferida manualmente no editor.

## Objetivo do teste

Validar a primeira caminhada noturna antes da chegada a Pensao Santa Luzia:

- caminhar do inicio da trilha ate a casa;
- nao cair no vazio;
- nao atravessar facilmente as laterais;
- ouvir o vento/ambiencia;
- chegar ao trigger da casa;
- transicionar para `HouseExterior.tscn`.

## Fluxo esperado

```text
TrailIntro -> HouseExterior -> DemoRoom
```

## Validar escala

- A largura jogavel temporaria fica entre os bloqueios laterais em `X -3.5` e `X 3.5`.
- O comprimento jogavel usa aproximadamente `Z 15` no inicio da trilha ate `Z -13` perto do portao de transicao.
- A casinha placeholder antiga do GLB da trilha esta escondida na cena; a fachada real aparece apenas em `HouseExterior.tscn`.
- O player deve parecer em escala humana diante da cerca, vegetacao e silhueta da casa.
- Se o GLB parecer rotacionado ou deslocado, ajustar os nos auxiliares da cena, nao o asset importado.
- A luz da silhueta usa `LightFlicker` para oscilar sutilmente e servir como guia distante.

## Validar colisao

Nos temporarios:

- `Collisions/TrailFloorCollision`
- `Collisions/LeftFenceBlocker`
- `Collisions/RightFenceBlocker`

Teste:

1. Andar para frente e para tras pela trilha.
2. Tentar sair pelas laterais.
3. Confirmar que o player nao cai abaixo do chao.
4. Confirmar que a camera nao fica presa nos bloqueios em movimento normal.

## Validar chegada na casa

Trigger:

`Triggers/HouseEntryTrigger`

Posicao aproximada:

```text
X 0
Y 1.0
Z -12.9
```

Ao entrar no trigger, o console deve imprimir:

```text
Transicao: TrailIntro -> HouseExterior
```

Se o HUD estiver carregado, deve aparecer brevemente:

```text
A Pensao Santa Luzia.
```

Depois o jogo deve trocar para:

`res://scenes/levels/house_exterior/HouseExterior.tscn`

A troca usa `SceneTransition.ChangeSceneWithFade`, com tela preta e mensagem curta.

## Silhueta distante da Pensao

`TrailIntro.tscn` usa apenas uma silhueta simples da casa no fim da trilha:

`DistantHouseSilhouette`

Ela e composta por:

- `HouseShadowBody`
- `HouseShadowRoof`
- `HouseShadowDoor`
- `HouseDistantLamp`
- `HouseLampGlowMarker`

A silhueta fica em `Z -16.8`, atras do trigger de chegada. Ela nao tem colisao, interacao, janelas detalhadas ou props. A funcao dela e ser um objetivo visual distante: de longe, o jogador entende que existe uma casa no fim da trilha.

A fachada detalhada continua apenas em:

`res://scenes/levels/house_exterior/HouseExterior.tscn`

Isso evita carregar a casa completa duas vezes. Ao chegar perto da silhueta/portao, o `HouseEntryTrigger` troca para a fachada real.

Antes da troca, o trigger narrativo `SilhouetteMessageTrigger` pode mostrar:

```text
A luz nao deveria estar acesa.
```

## Testar continuidade ate o quarto

1. Rodar `TrailIntro.tscn`.
2. Caminhar ate a casa.
3. Confirmar troca para `HouseExterior.tscn`.
4. Caminhar ate a porta da fachada.
5. Confirmar troca para `DemoRoom.tscn`.

## Audio

Ambiencia:

`Audio/TrailAmbience`

Asset:

`res://assets/audio/ambience/wind_old_house_01.ogg`

O import do OGG esta configurado com loop ligado e o player de audio usa `Autoplay = true`, `VolumeDb = -16`.

## Problemas conhecidos

- Colisoes sao caixas temporarias e podem nao seguir perfeitamente cercas/vegetacao.
- A transicao para `HouseExterior.tscn` registra checkpoint em memoria, mas ainda nao preserva inventario/estado persistente.
- A luz da casa e apenas guia visual temporario.
- A validacao headless direta com `--scene TrailIntro.tscn` derrubou o executavel Godot 4.7 nesta maquina; validar manualmente com F6 no editor.

## Proximos passos

- Ajustar a orientacao/escala da trilha dentro do editor apos playtest visual.
- Refinar a area de chegada na fachada.
- Adicionar props bloqueadores mais organicos nas laterais.
- Refinar ambiencia com camadas de noite, vento e casa distante.
