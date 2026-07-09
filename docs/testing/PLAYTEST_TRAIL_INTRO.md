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
- receber mensagem de chegada perto da casa.

## Validar escala

- A largura jogavel temporaria fica entre os bloqueios laterais em `X -3.5` e `X 3.5`.
- O comprimento jogavel usa aproximadamente `Z 15` no inicio da trilha ate `Z -15` perto da casa.
- O player deve parecer em escala humana diante da cerca, vegetacao e silhueta da casa.
- Se o GLB parecer rotacionado ou deslocado, ajustar os nos auxiliares da cena, nao o asset importado.

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
Z -14.8
```

Ao entrar no trigger, o console deve imprimir:

```text
Chegada a Pensao Santa Luzia.
```

Se o HUD estiver carregado, deve aparecer:

```text
A casa parece estar esperando por voce.
```

Ainda nao ha troca de cena. O script `HouseEntryTrigger.cs` tem TODO para futura transicao para `HouseExterior.tscn`.

## Audio

Ambiencia:

`Audio/TrailAmbience`

Asset:

`res://assets/audio/ambience/wind_old_house_01.ogg`

O import do OGG esta configurado com loop ligado e o player de audio usa `Autoplay = true`, `VolumeDb = -16`.

## Problemas conhecidos

- Colisoes sao caixas temporarias e podem nao seguir perfeitamente cercas/vegetacao.
- A cena nao troca para fachada ainda.
- A luz da casa e apenas guia visual temporario.
- A validacao headless direta com `--scene TrailIntro.tscn` derrubou o executavel Godot 4.7 nesta maquina; validar manualmente com F6 no editor.

## Proximos passos

- Ajustar a orientacao/escala da trilha dentro do editor apos playtest visual.
- Criar `HouseExterior.tscn`.
- Trocar o trigger de chegada por transicao real para a fachada.
- Adicionar props bloqueadores mais organicos nas laterais.
- Refinar ambiencia com camadas de noite, vento e casa distante.
