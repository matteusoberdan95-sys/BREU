# BREU — Guia de atmosfera

Direcao sonora e visual da vertical slice do Quarto 07 e corredor.

## Principios

1. **Silencio antes do susto** — o corredor deve parecer quase vazio ate o trigger; evitar musica constante.
2. **Audio sugere, nao explica** — sons indicam presenca e perigo; nao narram em voz alta o que acontece.
3. **Luz fraca e suja** — amarelo enferrujado no quarto; corredor mais escuro com flicker no susto.
4. **Placeholder ate o Blender** — geometria simples no Godot; clima vem de luz, audio e UI.

## Quarto 07

- Tom: pensao velha, quarto apertado, lampada fraca.
- Sons desejados: `room_tone_01.ogg`, rangido distante, clique da lanterna.
- Narrativa: bilhete curto, tom de aviso, sem lore excessivo na UI.

## Corredor

- Tom: transicao entre seguranca relativa e ameaca.
- Escuro no final (`CorridorDarkZone`, luz ~0.25 apos susto).
- Sons: static de radio, stinger unico, passos/respiracao futuros no inimigo.

## Primeiro susto

Sequencia alvo:

1. Silencio / passos do jogador.
2. Trigger → flicker → mensagem HUD → radio.
3. Silhueta no fundo (1.5–2s).
4. Retorno ao escuro; jogador continua andando.

## Sons prioritarios para a demo

| Prioridade | Arquivo | Momento |
|------------|---------|---------|
| Alta | `door_open_old_wood.ogg` | Abrir porta |
| Alta | `scare_stinger_01.ogg` | Susto corredor |
| Alta | `radio_static_loop.ogg` | Apos susto |
| Media | `pickup_item.ogg` | Coletar martelo |
| Media | `flashlight_click.ogg` | Lanterna |
| Baixa | `enemy_breath_01.ogg` | Proxima sprint (perseguicao) |

## UI narrativa

- Bilhete: painel papel velho, fundo escurecido, leitura sem pressa.
- HUD: mensagens curtas; nao competir com o centro da tela durante leitura.

## O que evitar

- Jump scares com volume maximo sem contexto.
- Texto longo no HUD durante acao.
- Inimigo atacando antes da sprint de combate/IA.
