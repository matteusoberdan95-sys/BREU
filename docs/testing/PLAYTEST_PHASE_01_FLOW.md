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

## Validacao visual K.1

Sprint curta de aplicacao visual real nas cenas principais. Testar fluxo completo:

```text
TrailIntro -> DemoRoom -> RitualRoom
```

### Checklist

- [ ] TrailIntro tem fog/neblina visivel a distancia?
- [ ] MoonLight fria aparece nas cercas/pedras?
- [ ] Pensao no fundo tem luz quente guia (`HouseFrontLanternLight`)?
- [ ] Quarto 07 esta escuro mas jogavel?
- [ ] Cantos do Quarto 07 ficaram mais escuros (DebugLight desligada)?
- [ ] Bilhete/mesa tem destaque leve sem apagar a lanterna?
- [ ] Sala dos Santos Secos tem foco quente na mesa ritual?
- [ ] Inimigo fica parcialmente na sombra ate a lanterna revelar?
- [ ] Lanterna continua util em DemoRoom e RitualRoom?
- [ ] Combate, HUD, morte/checkpoint e troca de cena continuam ok?

### Screenshots recomendados

1. **TrailIntro** — olhando para a Pensao (neblina + contraste frio/quente).
2. **DemoRoom** — enquadramento cama/mesa/janela (cantos escuros + luz local).
3. **RitualRoom** — mesa/altar/inimigo (velas quentes + sombra no fundo).

### Valores aplicados

Ver `docs/visual/LIGHTING_GUIDE.md` secao **Sprint K.1 — Ajustes aplicados**.

## Validacao visual K.2 — Rebalanceamento

Motivo: K.1 escureceu demais. Rebalanceamento prioriza **legibilidade sem lanterna**.

### Checklist

- [ ] TrailIntro legivel caminhando sem lanterna?
- [ ] Fog/neblina visivel a distancia (depth fog)?
- [ ] MoonLight perceptivel em cercas/cactos?
- [ ] Pensao com luz quente guia sem ofuscar?
- [ ] Quarto 07 navegavel sem lanterna (cama, mesa, porta, janela)?
- [ ] Lâmpada do teto ilumina o quarto de forma util?
- [ ] RitualRoom mantem foco na mesa sem teto preto total?
- [ ] Inimigo ainda surge parcialmente na sombra?
- [ ] Lanterna continua util para detalhes em todas as salas?
- [ ] Gameplay intacto (HUD, combate, checkpoint, troca de cena)?

Valores finais: `LIGHTING_GUIDE.md` secao **Sprint K.2**.

## Validacao visual K.3 — Iluminacao pratica e neblina

Hotfix focado em **luz real** e **fog volumetrico**.

### Checklist

**TrailIntro**
- [ ] Fog/neblina visivel olhando para a Pensao?
- [ ] MoonLight perceptivel em silhuetas?
- [ ] `HouseEntranceWarmLight` guia o jogador?
- [ ] Caminho legivel sem preto chapado?

**DemoRoom**
- [ ] `RoomCeilingLight` ilumina cama/mesa/porta/piso?
- [ ] Bulbo `lamp_bulb` parece aceso (emissivo)?
- [ ] Quarto navegavel **sem lanterna**?
- [ ] Lanterna ainda util para detalhes?

**RitualRoom**
- [ ] Mesa ritual em foco (`CandleLightMain`)?
- [ ] Entorno legivel (teto/paredes nao viram massa preta)?
- [ ] Inimigo ainda assustador na sombra?
- [ ] Combate/morte/checkpoint ok?

Valores finais: `LIGHTING_GUIDE.md` secao **Sprint K.3**.

## TrailIntro Fog Validation (K.3.1)

Subsprint **apenas TrailIntro** — neblina definitiva.

### Checklist

- [ ] Fog **visível a olho nu** olhando para a Pensão?
- [ ] Neblina aparece **sem imaginar** — não só “ativada no editor”?
- [ ] Trilha parece **menos limpa** / mais ameaçadora?
- [ ] Pensão continua **visível** como farol quente?
- [ ] MoonLight **perceptível** em cercas/cactos?
- [ ] Cactos/cercas ganham **profundidade**?
- [ ] Caminho continua **jogável**?
- [ ] Não virou **parede branca** de fog?

Valores finais: `LIGHTING_GUIDE.md` secao **Sprint K.3.1**.

## Validacao K.3.2 — Fog e Lanterna

Subsprint **TrailIntro + lanterna + qualidade volumetrica** — refino da neblina e correcao de quadrados.

### Checklist

- [ ] Fog **visivel** sem lanterna?
- [ ] Fog **natural** com lanterna ligada (nao parece leite branco)?
- [ ] Quadrados/blocos da lanterna **sumiram ou diminuiram bastante**?
- [ ] Pensao continua **visivel** como farol quente?
- [ ] Caminho continua **jogavel**?
- [ ] Fog **nao ficou branca demais**?
- [ ] Performance continua **aceitavel** (FPS estavel ao correr com lanterna)?

### Fluxo de teste obrigatorio

1. F6/F5 — iniciar na TrailIntro **sem** lanterna; olhar para a Pensao.
2. Andar ate metade da trilha; ligar lanterna (F); observar neblina no cone.
3. Correr com lanterna ligada — verificar flicker/artefatos.
4. Parar perto da Pensao — fog da profundidade sem esconder o objetivo.
5. `TrailIntro -> Pensao -> DemoRoom -> RitualRoom` — transicoes e HUD ok.

Valores finais: `LIGHTING_GUIDE.md` secao **Sprint K.3.2**.

## Validacao K.3.3 — Neblina Hibrida

Subsprint **TrailIntro** — fog hibrida (depth + particulas), reducao de artefatos em grade.

### Checklist

- [ ] Trilha com neblina **visivel** (sem parecer parede branca)?
- [ ] Quadrados/grade **diminuiram bastante** (com e sem lanterna)?
- [ ] Lanterna continua **util** para ler o caminho?
- [ ] Pensao continua **visivel** como farol quente?
- [ ] Caminho central continua **legivel/jogavel**?
- [ ] Performance continua **aceitavel** (correr com lanterna)?

### Fluxo de teste

1. Parado no inicio — fog + particulas visiveis sem grade agressiva.
2. Andar **sem** lanterna ate metade da trilha.
3. Ligar lanterna (F) — observar se grade some ou fica sutil.
4. Correr com lanterna — sem flicker/artefatos excessivos.
5. Olhar para pensao e laterais (cercas/cactos em silhueta).
6. Chegar perto da pensao — objetivo continua claro.

Valores finais: `LIGHTING_GUIDE.md` secao **Sprint K.3.3**.

## Validacao K.3.4 — Neblina Visivel Restaurada

Hotfix **apenas TrailIntro** — restaurar neblina apos K.3.3 ter apagado a atmosfera.

### Checklist

- [ ] Neblina **visivel sem lanterna**?
- [ ] Neblina **visivel com lanterna** (nao sumiu ao ligar F)?
- [ ] Pensao continua **visivel** como farol quente?
- [ ] Caminho central **jogavel/legivel**?
- [ ] Quadrados **reduzidos ou aceitaveis** (nao dominam a tela)?
- [ ] Trilha **nao voltou a ficar limpa**?

### Fluxo de teste

1. F6 — parado no spawn, olhar para a pensao (neblina perceptivel no fundo).
2. Andar ate metade da trilha sem lanterna — profundidade/atmosfera presentes.
3. Ligar lanterna — neblina continua visivel; quadrados nao dominam.
4. Correr com lanterna — performance ok.
5. Confirmar DemoRoom/RitualRoom inalterados.

Valores finais: `LIGHTING_GUIDE.md` secao **Sprint K.3.4**.

## Validacao K.3.5 — Sem Blocos na Neblina

Sprint **TrailIntro** — remocao definitiva de quadrados/retangulos na neblina.

### Checklist

- [ ] **Sem quadrados grandes** na neblina (com e sem lanterna)?
- [ ] Olhando para a **Pensao** — neblina suave, sem retangulos?
- [ ] Fog ainda **visivel** (nao sumiu totalmente)?
- [ ] Lanterna ligada — **sem artefatos** volumetricos?
- [ ] Pensao continua **visivel** como farol?
- [ ] Caminho central **jogavel/legivel**?
- [ ] Correndo com lanterna — sem blocos piscando?

### Fluxo de teste obrigatorio

1. F6 — parado no spawn, olhar para Pensao (lanterna off).
2. Ligar lanterna (F) — olhar para Pensao; **nenhum retangulo grande**.
3. Andar ate metade da trilha; olhar laterais (cercas/cactos).
4. Correr com lanterna ligada.
5. Confirmar DemoRoom/RitualRoom inalterados.

Valores finais: `LIGHTING_GUIDE.md` secao **Sprint K.3.5**.

## Validacao K.3.6 — Fog Cards Cinematograficos

Sprint **TrailIntro** — neblina via Fog Cards suaves, sem FogVolume/particles antigas.

### Checklist

- [ ] Fog **visivel** (cards + base environment)?
- [ ] **Sem quadrados** grandes na neblina?
- [ ] **Sem retangulos** grandes (olhar para Pensao)?
- [ ] Lanterna ligada — **sem artefatos** volumetricos?
- [ ] Pensao continua **visivel** como farol?
- [ ] Caminho central **jogavel**?
- [ ] Trilha mais **atmosferica** que limpa?

### Fluxo de teste comparativo

1. Inicio trilha — lanterna off/on.
2. Meio trilha — lanterna off/on.
3. Olhar para Pensao e laterais.
4. Correr com lanterna ligada.
5. Se card parecer quadrado: anotar qual (`FogCard_*`) e ajustar softness/alpha.

Valores finais: `LIGHTING_GUIDE.md` secao **Sprint K.3.6**.

## Validacao K.3.7 — Fog Cards PNG

Sprint **TrailIntro** — neblina via Sprite3D + PNG soft.

### Checklist

- [ ] Fog cards **aparecem** olhando para a Pensao?
- [ ] **Sem quadrados/retangulos** duros?
- [ ] Lanterna ligada — **sem artefato** de grade?
- [ ] Pensao continua **visivel**?
- [ ] Caminho **jogavel**?
- [ ] **Cena carrega** sem erro no Godot?

Valores finais: `LIGHTING_GUIDE.md` secao **Sprint K.3.7**.

## Validacao K.3.8 — Neblina Refinada

### Checklist

- [ ] Neblina **visivel** com e sem lanterna?
- [ ] **Sem faixa/parede** ao andar pela trilha?
- [ ] **Sem quadrados** duros?
- [ ] **Leve movimento** perceptivel (organico)?
- [ ] Pensao **visivel** no fundo?
- [ ] Caminho **jogavel**?
- [ ] Cena **carrega** sem erro?

### Fluxo

1. F6 — olhar pensao, andar lento ate metade.
2. Correr com lanterna on/off.
3. Perto da pensao — fachada legivel.

Valores finais: `LIGHTING_GUIDE.md` secao **Sprint K.3.8**.

## Validacao K.3.9 — Neblina Screen-Space

### Checklist

- [ ] Neblina **visivel** (overlay + base environment)?
- [ ] **Sem linhas/faixas** ao andar?
- [ ] **Sem quadrados**?
- [ ] Neblina **animada** (movimento leve)?
- [ ] **HUD legivel** (nao coberto)?
- [ ] Pensao **visivel**?
- [ ] Caminho **jogavel**?
- [ ] Cena **carrega** sem erro?

### Fluxo

1. F6 — parado, andar lento, correr; lanterna on/off.
2. Olhar pensao e laterais — sem planos atravessaveis.
3. Confirmar HUD (bateria, mensagens) legivel.

Valores finais: `LIGHTING_GUIDE.md` secao **Sprint K.3.9**.

## Validacao K.3.10 — Diagnostico e Calibracao

### Checklist

- [ ] `debug_visible = true` comprovou overlay azul na tela?
- [ ] `debug_visible = false` — neblina **visivel** em modo normal?
- [ ] Neblina **animada** (drift leve)?
- [ ] **Sem quadrados** / **sem linhas** no caminho?
- [ ] **HUD legivel**?
- [ ] Pensao **visivel**?
- [ ] Caminho **jogavel**?
- [ ] Cena **carrega** sem erro de shader?

### Teste debug (obrigatorio uma vez)

1. Inspector → `ScreenFogOverlay` → Material → `debug_visible = true` → F6.
2. Tela deve ficar claramente azulada.
3. Voltar `debug_visible = false` → F6 → neblina fria no horizonte.

Valores finais: `LIGHTING_GUIDE.md` secao **Sprint K.3.10**.

## Validacao K.3.11 — Depth-Based Fog

### Checklist

- [ ] `debug_depth = true` comprovou escala de cinza (perto escuro / longe claro)?
- [ ] `debug_depth = false` — depth fog **visivel**?
- [ ] Objetos **distantes** mais embaçados que **proximos**?
- [ ] Pensao **atmosferica** mas ainda visivel?
- [ ] Caminho perto do player **limpo**?
- [ ] **Sem quadrados** / **sem faixas**?
- [ ] **HUD legivel**?
- [ ] Cena **carrega** sem erro?

### Teste debug (obrigatorio uma vez)

1. Inspector → `DepthFogPostProcess` → Material → `debug_depth = true` → F6.
2. Chao perto = escuro; horizonte/pensao = claro.
3. Voltar `debug_depth = false` → F6 → neblina por distancia.

- HUD legivel

## Sprint L — Pass Visual Real (2026-07-11)

**Escopo:** materiais, props, fachada. **Fog intocada.**

Ver: `docs/visual/TRAILINTRO_APPROVED_LOOK.md`

### Checklist F6

- [ ] Neblina igual ao preset **TrailIntro Fog Approved**
- [ ] Materiais visiveis (nao blockout chapado)
- [ ] Fachada mais assustadora
- [ ] Caminho jogavel
- [ ] Props nao bloqueiam player
- [ ] Entrada na Pensao funciona
- [ ] HUD legivel
- [ ] Sem erro no console

## Problemas conhecidos

- `HouseExterior.tscn` continua existindo como cena de teste isolado/comparacao, mas saiu do fluxo principal.
- A silhueta antiga da Pensao ficou desativada como fallback.
- As colisoes da trilha e da fachada integrada ainda sao temporarias.
- Checkpoints e respawn ainda sao apenas em memoria.
- O estado do martelo e da Chave Velha persiste apenas em memoria enquanto o jogo esta rodando.
- O combate do martelo e prototipo por raycast, sem animacao final.
- A RitualRoom ainda tem porta de saida bloqueada.
