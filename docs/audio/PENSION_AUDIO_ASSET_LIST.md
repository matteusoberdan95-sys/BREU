# Lista de assets — Áudio Pensão (Sprint 16 / 16B / pack v2)

**Pasta:** `assets/audio/pensao/`  
**Formato:** OGG Vorbis  
**Regra:** usar arquivo real se existir; nunca placeholder quando o asset está presente.

## Ambientes (loops) — wired

| Arquivo | Uso | Bus | Volume | Status |
|---------|-----|-----|--------|--------|
| `exterior_night_wind_loop.ogg` | Trilha | Ambience | −16 dB | Presente / usado |
| `pension_reception_ambience_loop.ogg` | Recepção | Ambience | −18 dB | Presente / usado |
| `pension_ground_corridor_loop.ogg` | Corredor / 102 / cozinha / escada | Ambience | −17 dB | Presente / usado |
| `pension_deposit_room_loop.ogg` | Depósito | Ambience | −16 dB | Presente / usado |
| `pension_second_floor_loop.ogg` | 2º andar | Ambience | −15 dB | Presente / usado |
| `pension_water_drops_loop.ogg` | Depósito + cozinha (secundário) | Ambience | −17 dB | Presente / usado (16B) |
| `lamp_buzz_loop.ogg` | Recepção / 2º andar (secundário) | Ambience | −20 dB | Presente / usado |

## One-shots narrativos / SFX — wired

| Arquivo | Uso | Bus | Volume | Status |
|---------|-----|-----|--------|--------|
| `old_house_settle_02.ogg` | Entrada pensão | SFX | −14 dB | Presente / usado |
| `old_house_settle_01.ogg` | Topo escada / debug | SFX | −14 dB | Presente / usado |
| `distant_knock_01.ogg` | Pós-chave | SFX | −12 dB | Presente / usado |
| `distant_knock_02.ogg` | Presença corredor | SFX | −12 dB | Presente / usado |
| `distant_step_01/02.ogg` | Pós-fusível | SFX | −9 dB | Presente / usado |
| `distant_step_03/04.ogg` | Reserva | SFX | — | Presente (não wired) |
| `door_scratch_02.ogg` | Porta bloqueada | SFX | −12 dB | Presente / usado |
| `door_scratch_01.ogg` | Debug F7 | SFX | −11 dB | Presente / usado (debug) |
| `water_drop_01/02/03.ogg` | Depósito/cozinha (`RandomOneShotEmitter3D`) | SFX | −14 dB | Presente / usado (16B) |
| `flashlight_click_on_01/02.ogg` | Ligar lanterna | SFX | −12 dB | Presente / usado |
| `flashlight_click_off_01/02.ogg` | Desligar lanterna | SFX | −12 dB | Presente / usado |

## Passos — wired (Sprint 16B)

| Grupo | Arquivos | Uso | Status |
|-------|----------|-----|--------|
| Madeira | `player_footstep_wood_01`…`08` | Interior walk **e** run | Presente / usado |
| Terra/cascalho | `player_footstep_dirt_gravel_01`…`12` | Exterior walk **e** run | Presente / usado |
| Corrida | `player_run_step_01`…`12` | Reservado (chase/pânico futuro) | Presente / **não usado** (16C) |
| Sequences | `player_footsteps_*_sequence`, `player_running_sequence` | Reserva — **proibido** como footstep individual | Presente / **não usado** (16D) |

## Respiração — sprint futura (cadastrados, não wired)

| Grupo | Arquivos | Status |
|-------|----------|--------|
| Respiração | `player_breath_heavy_loop`, `_01`…`04`, `player_panting_loop` | Presente / preparado |

**Assets faltantes (runtime obrigatório):** nenhum dos loops/one-shots/passos wired.
