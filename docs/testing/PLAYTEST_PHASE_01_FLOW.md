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

O `CheckpointManager` ainda nao salva em disco. Ele registra o ultimo checkpoint, caminho da cena, posicao/rotacao do player e um snapshot simples do `GameSession`:

- `TrailIntro_Start`
- `DemoRoom_Quarto07`
- `RitualRoom_SantosSecos`
- `Ritual_Note_Read`

Isso permite retry em memoria apos morte. Ao clicar `Tentar novamente`, a cena do ultimo checkpoint e recarregada e o player volta com vida cheia. O snapshot restaura arma atual, durabilidade, martelo coletado e Chave Velha conforme estavam quando o checkpoint foi registrado.

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

## Player Feel Sprint J

Controles novos/confirmados:

- `Q`: lean para esquerda.
- `R`: lean para direita.
- `Ctrl` ou `C`: agachar.
- `E`: continua sendo interacao.

Teste:

1. Iniciar pela `TrailIntro.tscn`.
2. Andar normalmente e confirmar headbob leve.
3. Segurar `Shift` e confirmar headbob mais forte, stamina drenando e sensacao de urgencia.
4. Gastar stamina ate abaixo de 35 e confirmar feedback de cansaco no console se os audios ainda nao existirem.
5. Segurar `Ctrl`/`C` e confirmar camera mais baixa, velocidade reduzida e headbob menor.
6. Segurar `Q` e `R` para confirmar lean sutil sem mover colisao fisica.
7. Alternar lanterna com `F` e andar/correr para confirmar sway leve.
8. Entrar na Pensao, pegar martelo e seguir ate a RitualRoom.
9. Tomar dano do Hospede Seco e confirmar flash vermelho + camera shake curto.
10. Morrer e confirmar que input/feel ficam bloqueados ate o retry.

Problemas conhecidos:

- Os audios `breath_light_01.ogg`, `breath_heavy_01.ogg` e `player_tired_01.ogg` ainda nao existem; o jogo registra aviso unico e segue sem audio de respiracao.
- Lean e apenas visual; ainda nao move a colisao nem detecta parede lateral.
- Valores de headbob ainda precisam de playtest para conforto.

## Movimento corporal procedural

Sistema ativo:

```text
Player/PlayerBodyMotion
```

Como testar:

1. F5 ou F6 em `TrailIntro.tscn`.
2. Parado: camera quase estavel, com respiracao muito leve.
3. Andando: camera sobe/desce e vai levemente para os lados.
4. Correndo: ombro e roll ficam mais fortes, com impacto visual dos passos.
5. Parar depois de correr: camera/mao fazem pequena compensacao de inercia e estabilizam.
6. Stamina baixa: respiracao visual e tremor de mao ficam mais perceptiveis, sem bloquear controle.
7. Agachado: camera baixa, sway/headbob menores e passo mais lento.
8. `Q`/`R`: lean continua funcionando sem usar `E`.
9. Lanterna/martelo: `WeaponHolder` e `Flashlight` balancam com a mao, sem quebrar ataque.
10. Tomar dano na RitualRoom: `PlayerBodyMotion.PlayDamageShake()` aplica shake curto.
11. Morrer e tentar novamente: body motion para na morte e volta apos retry.

Valores iniciais principais:

| Estado | Vertical | Horizontal | Roll |
|--------|----------|------------|------|
| Andar | 0.032 | 0.016 | 1.1 |
| Correr | 0.070 | 0.036 | 3.0 |
| Agachar | 0.014 | 0.008 | 0.5 |

Problemas conhecidos:

- O sistema e procedural e ainda precisa de ajuste fino por playtest.
- O step impact e apenas visual; audio de passos continua em `PlayerFootstepAudio`.
- Lean ainda nao move colisao fisica.
- Audios de respiracao seguem pendentes.

## Ajuste de corrida e respiracao

Valores de corrida que devem estar no `Player/PlayerBodyMotion`:

- `RunStepFrequency = 9.2`
- `RunBobVertical = 0.055`
- `RunBobHorizontal = 0.020`
- `RunRollAmount = 1.65`
- `RunPitchAmount = 0.85`
- `ShoulderSwayRunAmount = 0.030`
- `ShoulderRollRunAmount = 1.25`
- `WeaponRunSwayAmount = 0.045`
- `RunStepImpact = 0.018`
- `Smoothing = 12.0`

Teste:

1. Andar normalmente e confirmar que a caminhada continua igual/boa.
2. Correr com stamina cheia e confirmar que a camera balanca menos para os lados.
3. Confirmar que `breath_light_01.ogg` toca em corrida normal.
4. Correr ate stamina abaixo de 35 e confirmar que `breath_heavy_01.ogg` entra.
5. Correr ate stamina chegar a zero e confirmar que `player_tired_01.ogg` toca uma vez, sem spam.
6. Parar depois de correr e confirmar que respiracao reduz/para e a camera estabiliza.
7. Entrar na RitualRoom e confirmar combate, dano, morte e retry sem regressao.

Fallback se a corrida ainda estiver exagerada:

- `RunBobHorizontal = 0.014`
- `RunRollAmount = 1.2`
- `ShoulderRollRunAmount = 0.9`

## RitualRoom polida (Sprint H)

A Sala dos Santos Secos agora possui:

- susto com sequencia cinematografica (luzes 1.5s, radio 2-4s, stinger, pausa dramatica do inimigo);
- combate com feedback de hit (stun 1.25s, escala/recuo visual);
- feedback de dano no player (flash vermelho, tremor de camera, mensagem com cooldown);
- death screen com fade in e stinger de morte (fallback para scare_stinger);
- retry validado com log `Checkpoint restaurado.`;
- balanceamento documentado em `docs/design/RITUAL_ROOM_BALANCE.md`.

## Morte, retry e respawn

1. No fluxo completo, entrar na `RitualRoom`.
2. Disparar o susto e deixar o `EnemyPlaceholder` atacar.
3. Confirmar que o HUD mostra `Vida 100/100` e diminui a cada hit.
4. Confirmar flash vermelho leve ao tomar dano.
5. Quando a vida chegar a `0/100`, confirmar a tela:

```text
VOCE MORREU
A casa ainda esta ouvindo.
Tentar novamente
```

6. Confirmar que o player nao anda, nao ataca e nao alterna lanterna enquanto morto.
7. Clicar `Tentar novamente`.
8. Confirmar que volta ao ultimo checkpoint com `Vida 100/100`.
9. Confirmar que o martelo volta com a durabilidade salva no checkpoint.

## Problemas conhecidos

- `HouseExterior.tscn` continua existindo como cena de teste isolado/comparacao, mas saiu do fluxo principal.
- A silhueta antiga da Pensao ficou desativada como fallback.
- As colisoes da trilha e da fachada integrada ainda sao temporarias.
- Checkpoints e respawn ainda sao apenas em memoria.
- O estado do martelo e da Chave Velha persiste apenas em memoria enquanto o jogo esta rodando.
- O combate do martelo e prototipo por raycast, sem animacao final.
- A RitualRoom ainda tem porta de saida bloqueada.
