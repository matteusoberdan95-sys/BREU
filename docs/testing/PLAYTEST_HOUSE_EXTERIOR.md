# BREU - Playtest da Fachada da Pensao Santa Luzia

## Como executar diretamente

1. Abrir o projeto no Godot 4.7 Mono.
2. Abrir `res://scenes/levels/house_exterior/HouseExterior.tscn`.
3. Rodar a cena com F6.
4. Clicar na janela/aba **Entrada** para dar foco ao mouse e teclado.

## Status no fluxo principal

`HouseExterior.tscn` continua disponivel para teste isolado e comparacao visual, mas nao faz mais parte obrigatoria do fluxo principal.

A fachada visual real agora aparece diretamente em:

`res://scenes/levels/trail_intro/TrailIntro.tscn`

Na Main Scene, a porta da Pensao na trilha leva direto para:

`res://scenes/levels/demo_room/DemoRoom.tscn`

Fluxo esperado:

```text
TrailIntro -> DemoRoom
```

## Onde o player comeca

Em `HouseExterior.tscn`, o player nasce em frente a Pensao:

```text
X 0
Y 1.0
Z -8.5
```

Ele deve olhar para a fachada/porta, com alguns metros de terreno antes do alpendre.

## Testar entrada na casa

Interativo:

`Triggers/EnterHouseDoor`

Posicao aproximada:

```text
X 0
Y 1.0
Z -15.05
```

Ao mirar na area da porta, o HUD deve mostrar:

```text
[E] Entrar na Pensao Santa Luzia
```

Ao apertar `E`, o console deve imprimir:

```text
Transicao: HouseExterior -> DemoRoom
```

Se o HUD estiver carregado, deve aparecer:

```text
Entrando na Pensao Santa Luzia...
```

Depois o jogo deve trocar para:

`res://scenes/levels/demo_room/DemoRoom.tscn`

## Testar volta para a trilha

Trigger:

`Triggers/BackToTrailTrigger`

Ao andar para tras, o console deve imprimir:

```text
BackToTrailTrigger ativado.
```

Se o HUD estiver carregado, deve aparecer:

```text
A trilha ficou para tras.
```

Nesta sprint esse trigger ainda nao troca cena.

## Testar colisoes da fachada

Nos temporarios:

- `Collisions/GroundCollision`
- `Collisions/HouseWallCollision`
- `Collisions/PorchCollision`
- `Collisions/DoorBlockerCollision`
- `Collisions/BoundaryCollisions`

Teste:

1. Andar pelo terreno.
2. Tentar sair pelas bordas.
3. Tentar atravessar paredes principais.
4. Mirar na porta, apertar `E` e confirmar que a transicao acontece sem atravessar o blocker.

## Testar luz do lampiao

No:

`Lighting/FrontLanternLight`

Esperado:

- luz quente perto da entrada;
- fachada legivel;
- clima ainda noturno;
- contraste com a luz fria da lua.

## Testar audio externo

No:

`Audio/ExteriorAmbience`

Asset:

`res://assets/audio/ambience/wind_old_house_01.ogg`

Esperado:

- tocar automaticamente;
- volume em `-16 dB`;
- loop configurado pelo import do OGG.

## Problemas conhecidos

- Colisoes ainda sao caixas simples.
- Porta ainda nao abre visualmente; a entrada usa prompt `[E]`.
- Transicao usa fade global se `SceneTransition` estiver ativo; caso contrario, troca direta.
- Fachada ainda e blockout.
- Iluminacao pode precisar ajuste fino depois do playtest visual.
- Posicao do player nao e preservada entre cenas.

## Proximos passos

- Ajustar colisoes conforme o GLB final.
- Criar porta visual/interativa com animacao.
- Criar transicao futura `HouseExterior -> TrailIntro`.
- Melhorar luz do lampiao com asset real.
- Conectar esse fluxo ao menu/inicio oficial da demo.
