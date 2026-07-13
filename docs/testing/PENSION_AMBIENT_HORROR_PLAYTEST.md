# Sprint 26 — Playtest de eventos ambientais

## Estado

- Implementação automática: concluída.
- Playtest manual: aprovado pelo usuário em 2026-07-13.
- Cena: `PensaoVerticalBlockout01.tscn`.

## Preparação

1. Concluir o evento forte do Quarto 203 e sair do quarto.
2. Confirmar que nenhuma perseguição está ativa.
3. Não permanecer escondido nem dentro de safe zone.
4. Aguardar até 45 segundos entre eventos.

## Rota de validação

- Recepção: ouvir rangido, pancadas vindas do andar superior ou queda distante.
- Corredor do térreo/fundo: ouvir passos ou arranhado localizado.
- Pé da escada: confirmar áudio localizado e nenhuma ativação atravessando andares.
- Corredor superior: validar passos, flicker breve e sombra rara.
- Banheiro superior: após obter a chave do ralo, validar o sussurro/gotas do ralo.
- Saída do 203: confirmar que o evento forte do quarto não é interrompido nem repetido.

## Bloqueios obrigatórios

- [x] Nenhum evento antes da progressão pós-203.
- [x] Nenhum evento durante Chase ou Search.
- [x] Nenhum evento enquanto `PlayerHidden` ou `PlayerInSafeZone`.
- [x] Nunca mais de um evento ao mesmo tempo.
- [x] Intervalo de 25–45 s entre eventos concluídos.
- [x] Áudio espacial moderado, sem parecer colado ao ouvido (exceto respiração atrás).
- [x] Flicker restaura a energia original da luz.
- [x] Sombra desaparece em menos de um segundo e não tem colisão.
- [x] Nenhum trigger alcança outro andar ou ativa através de parede distante.
- [x] IA, puzzles, portas, lanterna, escada e movimento permanecem intactos.

## Eventos esperados

- `AmbientEvent_DistantDoorCreak`
- `AmbientEvent_UpperFloorKnock`
- `AmbientEvent_DistantFootsteps`
- `AmbientEvent_LightFlicker`
- `AmbientEvent_WallScratch`
- `AmbientEvent_ObjectFall`
- `AmbientEvent_BreathBehind`
- `AmbientEvent_DrainWhisper`
- `AmbientEvent_QuickShadow` (raro)

O Output registra `[AmbientHorror] Playing ...` e o cooldown escolhido para facilitar a validação sem alterar o gameplay.
