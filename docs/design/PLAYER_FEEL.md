# BREU - Player Feel

## Objetivo

O player de BREU DE DENTRO deve parecer um corpo assustado dentro do espaco, nao uma camera flutuando. A movimentacao precisa ser pesada, tensa e responsiva o suficiente para o jogador sentir controle, mas sem parecer arcade ou poderoso demais.

## Direcao

- Horror em primeira pessoa semi-linear.
- Camera corporal sutil, com headbob procedural controlado.
- Corrida com urgencia e consumo dramatico de stamina.
- Lanterna com sway leve, como objeto na mao.
- Dano com camera shake curto.
- Agachar para reduzir ritmo, tensao e ruido futuro.
- Lean basico para espiar cantos sem mover colisao fisica.

## Movimento corporal procedural

O sistema ativo fica em:

```text
res://scripts/player/PlayerBodyMotion.cs
Player/PlayerBodyMotion
```

Ele trabalha sobre os nos atuais do Player:

```text
Player
  CameraPivot
    Camera3D
      WeaponHolder
    Flashlight
```

Regras:

- `PlayerLook` continua controlando o mouse look no `CameraPivot`.
- `PlayerBodyMotion` aplica apenas offsets locais em `Camera3D`, `WeaponHolder` e `Flashlight`.
- O script nao altera fisica, colisao, stamina, lanterna, combate ou inventario.
- `PlayerMeleeAttack` continua animando `EquippedHammerVisual`; o body motion move apenas o `WeaponHolder`.

Camadas do sistema:

- **Gait phase:** `_gaitPhase` avanca conforme a velocidade horizontal do player.
- **Headbob vertical/horizontal:** camera sobe/desce e desloca lateralmente de forma leve.
- **Run roll:** corrida adiciona roll lateral para simular ombro.
- **Shoulder sway:** corrida aumenta deslocamento lateral e balanco do `WeaponHolder`.
- **Inercia:** aceleracao/parada inclinam a camera e atrasam a mao.
- **Step impact:** passos aplicam micro impacto visual sem duplicar audio de passos.
- **Respiracao visual:** stamina baixa aumenta oscilacao sutil da camera/mao.
- **Weapon/flashlight sway:** lanterna e martelo acompanham a mao com intensidade diferente por estado.
- **Damage shake:** `PlayerHealth.TakeDamage()` chama `PlayDamageShake()`.

Estados usados:

- `Idle`
- `Walking`
- `Running`
- `Crouching`
- `Exhausted`
- `Dead`

O objetivo e parecer mais fisico e cinematografico, sem copiar conteudo de referencias. A referencia de Outlast serve apenas para ritmo, tensao e imersao; BREU mantem identidade propria.

## Vulnerabilidade

O jogador pode reagir, bater, fugir e sobreviver, mas nao domina o espaco. Stamina baixa, arma quebravel, dano e morte precisam lembrar que todo confronto e arriscado.

## Parametros iniciais

| Parametro | Valor |
|-----------|-------|
| WalkBobAmount | 0.030 |
| WalkBobSpeed | 7.5 |
| RunBobAmount | 0.060 |
| RunBobSpeed | 11.0 |
| CrouchBobAmount | 0.015 |
| CrouchBobSpeed | 4.5 |
| DamageShakeAmount | 0.12 |
| DamageShakeDuration | 0.22 |
| LeanAngle | 8 graus |
| LeanOffset | 0.18 |
| FlashlightSwayAmount | 0.035 |

## Parametros do body motion procedural

| Parametro | Valor inicial |
|-----------|---------------|
| WalkStepFrequency | 7.2 |
| RunStepFrequency | 9.2 |
| CrouchStepFrequency | 4.2 |
| WalkBobVertical | 0.032 |
| WalkBobHorizontal | 0.016 |
| WalkRollAmount | 1.1 |
| RunBobVertical | 0.055 |
| RunBobHorizontal | 0.020 |
| RunRollAmount | 1.65 |
| RunPitchAmount | 0.85 |
| CrouchBobVertical | 0.014 |
| CrouchBobHorizontal | 0.008 |
| CrouchRollAmount | 0.5 |
| ShoulderSwayRunAmount | 0.030 |
| ShoulderRollRunAmount | 1.25 |
| AccelerationTiltAmount | 1.3 |
| StopTiltAmount | 1.8 |
| InertiaSwayAmount | 0.030 |
| WalkStepImpact | 0.010 |
| RunStepImpact | 0.018 |
| BreathingIdleAmount | 0.006 |
| BreathingTiredAmount | 0.022 |
| WeaponWalkSwayAmount | 0.028 |
| WeaponRunSwayAmount | 0.045 |
| WeaponCrouchSwayAmount | 0.014 |
| TiredHandShakeAmount | 0.016 |
| Smoothing | 12.0 |

## Ajuste de corrida e respiracao

No playtest, a caminhada foi validada como boa, mas a corrida estava com balanco lateral e roll fortes demais. A Sprint J.6 reduziu apenas os valores de corrida para deixar o movimento mais pesado, legivel e menos enjoativo.

Valores ajustados:

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

Fallback de ajuste fino, se a corrida ainda ficar lateral demais:

- `RunBobHorizontal = 0.014`
- `RunRollAmount = 1.2`
- `ShoulderRollRunAmount = 0.9`

Arquivos de audio de respiracao:

- `res://assets/audio/sfx/player/breath_light_01.ogg`
- `res://assets/audio/sfx/player/breath_heavy_01.ogg`
- `res://assets/audio/sfx/player/player_tired_01.ogg`

Regras atuais:

- `breath_light_01.ogg`: toca em loop durante corrida com stamina acima de 35%.
- `breath_heavy_01.ogg`: toca em loop quando a stamina fica abaixo de 35%.
- `player_tired_01.ogg`: one-shot quando stamina chega a zero ou o jogador tenta correr sem stamina, com cooldown de 3s.
- Na morte/retry, a respiracao para junto com o body motion.

Flags de debug no Inspector:

- `EnableBodyMotion`
- `EnableHeadBob`
- `EnableShoulderSway`
- `EnableWeaponSway`
- `EnableBreathingMotion`
- `EnableStepImpact`

## Cuidados

- Evitar camera shake forte demais ou nauseante.
- Nao alterar mouse look principal.
- Nao transformar lean em movimento fisico ate existir colisao propria.
- Nao usar HUD excessivo para explicar sensacoes.
- Stamina baixa deve ser sentida por camera, respiracao e ritmo, mas sem impedir o jogador de entender o que esta acontecendo.
- Se algum efeito causar desconforto, desligar camadas pelo Inspector e reduzir primeiro `RunBobVertical`, `RunRollAmount` e `ShoulderSwayRunAmount`.

## Audios futuros

Adicionar quando disponivel:

- `res://assets/audio/sfx/player/breath_light_01.ogg`
- `res://assets/audio/sfx/player/breath_heavy_01.ogg`
- `res://assets/audio/sfx/player/player_tired_01.ogg`

Enquanto esses arquivos nao existem, `PlayerBodyMotion` apenas imprime um aviso uma vez e segue funcionando sem audio.
