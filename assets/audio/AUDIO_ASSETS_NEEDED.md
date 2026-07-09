# BREU — Assets de áudio necessários

Coloque os arquivos `.ogg` nas pastas abaixo. O jogo funciona sem eles (fallback seguro com `Debug.Print`).

## `sfx/doors/`

| Arquivo | Uso |
|---------|-----|
| `door_open_old_wood.ogg` | Abrir porta do Quarto 07 |
| `door_close_old_wood.ogg` | Fechar porta |
| `door_locked_rattle.ogg` | Porta trancada |

## `sfx/radio/`

| Arquivo | Uso |
|---------|-----|
| `radio_static_loop.ogg` | Loop de interferência |
| `radio_whisper_01.ogg` | Sussurro pontual |

## `sfx/horror/`

| Arquivo | Uso |
|---------|-----|
| `scare_stinger_01.ogg` | Stinger do primeiro susto |
| `corridor_hit_01.ogg` | Impacto/atmosfera no corredor |
| `distant_knock_01.ogg` | Batida distante |

## `sfx/player/`

| Arquivo | Uso |
|---------|-----|
| `flashlight_click.ogg` | Ligar/desligar lanterna |
| `pickup_item.ogg` | Coletar item |
| `heartbeat_low.ogg` | Tensão / baixa vida (futuro) |

## `sfx/enemies/`

| Arquivo | Uso |
|---------|-----|
| `enemy_breath_01.ogg` | Respiração do inimigo |
| `enemy_step_01.ogg` | Passos |
| `enemy_growl_01.ogg` | Rosnado |

## `ambience/`

| Arquivo | Uso |
|---------|-----|
| `room_tone_01.ogg` | Tom ambiente do quarto |
| `corridor_tone_01.ogg` | Tom ambiente do corredor |
| `wind_old_house_01.ogg` | Vento / casa velha |

## Caminhos Godot

Prefixo: `res://assets/audio/`

Exemplo: `res://assets/audio/sfx/doors/door_open_old_wood.ogg`

Ver também: `docs/technical/AUDIO_SYSTEM.md`
