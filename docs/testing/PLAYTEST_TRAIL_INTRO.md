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
- ver a fachada real da Pensao Santa Luzia no fim da trilha;
- chegar ate a varanda/porta;
- interagir com a porta e transicionar direto para `DemoRoom.tscn`.

## Fluxo esperado

```text
TrailIntro -> DemoRoom
```

## Validar escala

- A largura jogavel temporaria fica entre os bloqueios laterais em `X -3.5` e `X 3.5`.
- O comprimento jogavel usa aproximadamente `Z 15` no inicio da trilha ate a fachada real perto do fim da cena.
- A casinha placeholder antiga do GLB da trilha esta escondida na cena.
- A fachada real e instanciada como GLB visual em `Environment/HouseExteriorAtTrailEnd`; `HouseExterior.tscn` nao e instanciada.
- O player deve parecer em escala humana diante da cerca, vegetacao e fachada.
- Se o GLB parecer rotacionado ou deslocado, ajustar os nos auxiliares da cena, nao o asset importado.
- A luz `Lighting/HouseFrontLanternLight` usa `LightFlicker` para oscilar sutilmente e servir como guia.

## Validar colisao

Nos temporarios:

- `Collisions/TrailFloorCollision`
- `Collisions/LeftFenceBlocker`
- `Collisions/RightFenceBlocker`
- `Collisions/HouseExteriorCollisions`

Teste:

1. Andar para frente e para tras pela trilha.
2. Tentar sair pelas laterais.
3. Confirmar que o player nao cai abaixo do chao.
4. Confirmar que a camera nao fica presa nos bloqueios em movimento normal.
5. Confirmar que o player chega ate a varanda/porta, mas nao atravessa a casa.

## Validar fachada real e porta da Pensao

Visual:

`Environment/HouseExteriorAtTrailEnd`

Interativo:

`Interactables/EnterPensionDoor`

Posicao aproximada da porta:

```text
X 0
Y 1.0
Z -15.05
```

Ao mirar na porta, o HUD deve mostrar:

```text
[E] Entrar na Pensao
```

Ao apertar `E`, o console deve imprimir:

```text
Transicao: TrailIntro -> DemoRoom
```

O fade deve mostrar:

```text
A porta range como se ja estivesse aberta por dentro.
```

Depois o jogo deve trocar para:

`res://scenes/levels/demo_room/DemoRoom.tscn`

## Silhueta antiga da Pensao

`DistantHouseSilhouette` continua na cena como fallback, mas esta desativada:

```text
Visible = false
```

Ela nao deve aparecer no fluxo principal enquanto a fachada real estiver integrada no fim da trilha.

`HouseExterior.tscn` continua existindo como cena isolada de teste/comparacao.

## Testar continuidade ate o quarto

1. Rodar `TrailIntro.tscn`.
2. Caminhar ate a casa.
3. Confirmar prompt `[E] Entrar na Pensao`.
4. Apertar `E`.
5. Confirmar troca direta para `DemoRoom.tscn`.

## Audio

Ambiencia:

`Audio/TrailAmbience`

Asset:

`res://assets/audio/ambience/wind_old_house_01.ogg`

O import do OGG esta configurado com loop ligado e o player de audio usa `Autoplay = true`, `VolumeDb = -16`.

## Problemas conhecidos

- Colisoes sao caixas temporarias e podem nao seguir perfeitamente cercas/vegetacao.
- A fachada real usa colisoes auxiliares temporarias em `TrailIntro`.
- A porta da Pensao ainda nao tem animacao visual.
- A validacao headless direta com `--scene TrailIntro.tscn` derrubou o executavel Godot 4.7 nesta maquina; validar manualmente com F6 no editor.

## Proximos passos

- Ajustar a orientacao/escala da trilha dentro do editor apos playtest visual.
- Refinar a area da varanda/porta integrada na trilha.
- Adicionar props bloqueadores mais organicos nas laterais.
- Refinar ambiencia com camadas de noite, vento e lampiao da Pensao.
