# Direcao sonora - BREU DE DENTRO

## Visao geral

BREU DE DENTRO deve ter uma identidade sonora seca, intima, suja e opressiva. O som deve reforcar a sensacao de isolamento, calor, madeira velha, vento seco, respiracao pesada, medo fisico e presenca sobrenatural ambigua.

O audio deve fazer o jogador sentir:

- solidao;
- perigo proximo;
- lugar antigo e abandonado;
- corpo cansado;
- medo crescente;
- ambiente vivo;
- ameaca que nem sempre e vista.

A referencia de ritmo e tensao e horror cinematografico em primeira pessoa, com inspiracao em Outlast, mas sem copiar sons, vozes, identidade sonora, trilhas ou efeitos protegidos.

## Pilares sonoros

1. **Respiracao e corpo**
   O jogador precisa sentir o proprio corpo: cansaco, dor, medo, esforco e recuperacao.

2. **Ambiente nordestino macabro**
   Vento seco, insetos noturnos, madeira velha, barro, cerca, telhado, lampiao, radio, objetos antigos.

3. **Silencio como tensao**
   Nem todo momento precisa ter som. Pausas e silencio aumentam o medo.

4. **Sons proximos e intimos**
   Passos, roupas, respiracao, mao segurando lanterna, martelo, portas e rangidos devem parecer proximos ao jogador.

5. **Inimigos humanos perturbados**
   O Hospede Seco e futuros inimigos devem soar humanos, mas quebrados: respiracao ruim, garganta seca, gemidos, rosnados e ataques fisicos.

6. **Combate bruto e limitado**
   Martelo, faca, madeira e objetos improvisados devem soar secos, pesados e perigosos, sem sensacao arcade.

7. **Radio e interferencia**
   Radio AM, chiado, vozes cortadas e sinais incompletos devem ser usados para narrativa e susto.

## Categorias

- `ambience`: loops de ambiente por area, vento, sala, corredor e presenca geral.
- `player`: respiracao, dano, morte, cansaco, lanterna, pickup e feedback corporal.
- `footsteps`: passos por superficie, separados por material e variacao.
- `weapons`: swing, impacto, quebra e manuseio de armas improvisadas.
- `enemies`: respiracao, passos, ataque, dor, stun e presenca dos inimigos.
- `interactables`: objetos coletaveis, bilhetes, chaves e mecanismos pequenos.
- `doors`: abrir, fechar, trancar, rangidos e loops de porta.
- `horror_stingers`: sustos pontuais, revelacoes e picos de tensao.
- `radio`: static, interferencia, fragmentos de voz e sinais narrativos.
- `UI`: sons discretos de nota, checkpoint, menus e feedback de interface.
- `music_optional`: camadas musicais raras, usadas apenas quando o silencio/ambiencia nao bastarem.

## Padrao de nomes

Padrao recomendado:

```text
categoria_descricao_variacao.ogg
```

Exemplos:

- `ambience_trail_night_loop_01.ogg`
- `ambience_room07_old_house_loop_01.ogg`
- `player_breath_light_01.ogg`
- `player_breath_heavy_01.ogg`
- `player_tired_01.ogg`
- `footstep_dirt_01.ogg`
- `footstep_wood_01.ogg`
- `weapon_hammer_swing_01.ogg`
- `weapon_hammer_hit_flesh_01.ogg`
- `enemy_hospede_breath_01.ogg`
- `enemy_hospede_attack_01.ogg`
- `door_wood_open_slow_01.ogg`
- `radio_static_loop_01.ogg`
- `horror_stinger_ritual_01.ogg`

Regras:

- usar minusculas;
- usar underscore;
- usar numero no final;
- loops devem conter `_loop` no nome;
- one-shots nao precisam conter `_oneshot`;
- manter `.ogg` como formato principal no Godot.

Observacao: alguns arquivos atuais nasceram antes deste padrao, como `breath_light_01.ogg`, `enemy_breath_01.ogg` e `door_open_old_wood.ogg`. Eles podem continuar funcionando, mas novas geracoes devem preferir o padrao canonico acima.

## Status

Status possiveis:

- `missing`: ainda nao existe.
- `placeholder`: gerado rapidamente para teste.
- `prototype`: ja funciona no jogo, mas ainda pode melhorar.
- `approved`: aprovado para vertical slice.
- `final`: pronto para versao final/comercial.

Nao marcar nenhum som como `final` sem revisao manual, teste em contexto e aprovacao de direcao.

## Lista inicial de audios da Fase 1

| Arquivo | Categoria | Uso | Loop | Status | Observacao |
|---|---|---|---|---|---|
| `ambience_trail_night_loop_01.ogg` | ambience | Trilha Noturna | Sim | missing | Existe `wind_old_house_01.ogg` como prototype atual. |
| `ambience_room07_loop_01.ogg` | ambience | Quarto 07 | Sim | missing | `room_tone_01.ogg` pode servir como prototype. |
| `ambience_ritual_room_loop_01.ogg` | ambience | Sala dos Santos Secos | Sim | missing | `room_tone_01.ogg` e usado como base/prototype. |
| `ambience_corridor_loop_01.ogg` | ambience | Corredor | Sim | missing | Existe `corridor_tone_01.ogg` como prototype atual. |
| `player_breath_light_01.ogg` | player | Respiracao leve em corrida | Sim | missing | Arquivo atual: `breath_light_01.ogg` prototype. |
| `player_breath_heavy_01.ogg` | player | Respiracao pesada com stamina baixa | Sim | missing | Arquivo atual: `breath_heavy_01.ogg` prototype. |
| `player_tired_01.ogg` | player | Exaustao one-shot | Nao | prototype | Existe em `assets/audio/sfx/player/`. |
| `player_damage_01.ogg` | player | Dano no player | Nao | missing | Arquivo atual: `player_hurt_01.ogg` prototype. |
| `player_death_01.ogg` | player | Morte do player | Nao | missing | Arquivo atual: `death_stinger_01.ogg` em horror. |
| `footstep_dirt_01.ogg` | footsteps | Passo em terra | Nao | missing | Necessario para TrailIntro. |
| `footstep_dirt_02.ogg` | footsteps | Passo em terra variacao | Nao | missing | Necessario para TrailIntro. |
| `footstep_wood_01.ogg` | footsteps | Passo em madeira | Nao | prototype | Existe em `assets/audio/sfx/player/`. |
| `footstep_wood_02.ogg` | footsteps | Passo em madeira variacao | Nao | prototype | Existe em `assets/audio/sfx/player/`. |
| `footstep_concrete_01.ogg` | footsteps | Passo em concreto | Nao | prototype | Existe em `assets/audio/sfx/player/`. |
| `footstep_concrete_02.ogg` | footsteps | Passo em concreto variacao | Nao | prototype | Existe em `assets/audio/sfx/player/`. |
| `weapon_hammer_swing_01.ogg` | weapons | Swing do martelo | Nao | missing | Ataque ainda usa feedback visual/fallback. |
| `weapon_hammer_hit_flesh_01.ogg` | weapons | Impacto em inimigo | Nao | missing | `corridor_hit_01.ogg` funciona como fallback. |
| `weapon_hammer_hit_wood_01.ogg` | weapons | Impacto em madeira | Nao | missing | Necessario para portas/props. |
| `weapon_hammer_break_01.ogg` | weapons | Martelo quebrando | Nao | missing | Necessario para durabilidade. |
| `enemy_hospede_breath_01.ogg` | enemies | Respiracao do Hospede Seco | Sim | missing | Arquivo atual: `enemy_breath_01.ogg` prototype. |
| `enemy_hospede_step_01.ogg` | enemies | Passo do Hospede Seco | Nao | missing | Arquivo atual: `enemy_step_01.ogg` prototype. |
| `enemy_hospede_growl_01.ogg` | enemies | Growl do Hospede Seco | Nao | missing | Arquivo atual: `enemy_growl_01.ogg` prototype. |
| `enemy_hospede_attack_01.ogg` | enemies | Ataque do Hospede Seco | Nao | missing | Necessario para combate. |
| `enemy_hospede_pain_01.ogg` | enemies | Dor ao receber golpe | Nao | missing | Necessario para hit/stun. |
| `enemy_hospede_stun_01.ogg` | enemies | Stun do inimigo | Nao | missing | Necessario para feedback de martelo. |
| `door_wood_open_slow_01.ogg` | doors | Porta abrindo devagar | Nao | missing | Arquivo atual: `door_open_old_wood.ogg` prototype. |
| `door_wood_close_01.ogg` | doors | Porta fechando | Nao | missing | Arquivo atual: `door_close_old_wood.ogg` prototype. |
| `door_wood_locked_01.ogg` | doors | Porta trancada | Nao | missing | Arquivo atual: `door_locked_rattle.ogg` prototype. |
| `door_wood_creak_loop_01.ogg` | doors | Rangido de porta | Sim | missing | Necessario para tensao/porta viva. |
| `radio_static_loop_01.ogg` | radio | Chiado continuo | Sim | missing | Arquivo atual: `radio_static_loop.ogg` prototype. |
| `radio_interference_01.ogg` | radio | Interferencia one-shot | Nao | missing | Pode derivar do static atual. |
| `radio_voice_fragment_01.ogg` | radio | Voz cortada | Nao | missing | `radio_whisper_01.ogg` existe como prototype. |
| `horror_stinger_corridor_01.ogg` | horror_stingers | Susto do corredor | Nao | missing | Arquivo atual: `scare_stinger_01.ogg` prototype. |
| `horror_stinger_ritual_01.ogg` | horror_stingers | Susto ritual | Nao | missing | `scare_stinger_01.ogg` pode servir como prototype. |
| `horror_stinger_enemy_reveal_01.ogg` | horror_stingers | Revelacao do inimigo | Nao | missing | Necessario para Hospede Seco. |
| `ui_note_open_01.ogg` | UI | Abrir bilhete | Nao | missing | Necessario para NoteReaderUI. |
| `ui_note_close_01.ogg` | UI | Fechar bilhete | Nao | missing | Necessario para NoteReaderUI. |
| `ui_checkpoint_01.ogg` | UI | Checkpoint | Nao | missing | Necessario para feedback discreto. |

## Uso de IA na producao de audio

O projeto pode usar IA para:

- vozes temporarias;
- respiracao;
- inimigos;
- vento;
- ambience;
- radio;
- portas;
- impactos;
- prototipos rapidos.

Cuidados obrigatorios:

- nao clonar voz de pessoa real sem permissao;
- nao imitar ator famoso;
- nao pedir som "igual" a jogo especifico;
- nao copiar identidade sonora de Outlast;
- guardar prompt, ferramenta, data e licenca;
- revisar todos os sons manualmente antes de considerar final;
- preferir prompts de direcao original do BREU: nordestino, seco, opressivo, fisico, intimo.

### Ferramentas possiveis

- Gemini / Google Cloud TTS para vozes e narracao.
- ElevenLabs para vozes e sound effects.
- Stable Audio para ambience e soundscapes.
- Audacity/Reaper para edicao.
- Godot para mixagem em contexto.

## Pipeline recomendado

1. Definir necessidade do som.
2. Gerar ou gravar variacoes.
3. Selecionar melhores takes.
4. Editar em Audacity/Reaper.
5. Cortar silencio desnecessario.
6. Aplicar fade in/out.
7. Normalizar volume.
8. Exportar `.ogg`.
9. Importar para Godot.
10. Testar em contexto.
11. Ajustar volume/pitch/range.
12. Atualizar status no documento.

## Direcao por area

### TrailIntro

Som seco, vento noturno, insetos, terra, cerca, passos em chao seco, sensacao de isolamento.

### Pensao Santa Luzia exterior

Vento batendo na madeira, lampiao, rangido distante, casa antiga viva.

### Quarto 07

Som abafado, madeira velha, lampada fraca, radio distante, quarto opressivo.

### Corredor

Som estreito, reverberacao curta, madeira rangendo, interferencia, sensacao de perseguicao.

### Sala dos Santos Secos

Ambiente ritualistico, velas, respiracao, radio, silencio pesado, presenca do Hospede Seco.

## Mixagem inicial

Recomendacoes:

- ambience deve ficar baixo e constante;
- passos devem ser claros;
- respiracao deve aparecer mais em cansaco;
- inimigo deve ser audivel antes de ser visto;
- sustos devem ser fortes, mas nao estourados;
- UI deve ser discreta;
- combate deve ser seco e pesado.

Volumes iniciais sugeridos:

- ambience: -18 dB a -12 dB
- passos player: -10 dB a -6 dB
- respiracao leve: -14 dB a -10 dB
- respiracao pesada: -9 dB a -5 dB
- inimigo perto: -8 dB a -3 dB
- impacto martelo: -7 dB a -3 dB
- horror stinger: -6 dB a -2 dB
- UI: -16 dB a -10 dB
