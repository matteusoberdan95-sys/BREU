# BREU - Proximas tarefas

Ultima atualizacao: 2026-07-09

## Como usar

Backlog vivo. Historico de sprints concluidas: `docs/SPRINT_HISTORY.md`.  
Antes de commit/push: `.cursor/rules/pre-commit-docs.mdc`.

## Concluido recentemente

- [x] Corredor placeholder conectado (+Z) com colisoes e materiais.
- [x] Martelo na mao ajustado (`WeaponHolder`).
- [x] HUD lanterna corrigido (100/100, dreno condicional).
- [x] `GlobalUsings.cs` por camadas.
- [x] HUD survival horror (painel, prompt, mensagens).
- [x] Som de porta preparado (`DoorAudioController`).
- [x] Radio/interferencia (`RadioInterferenceController`).
- [x] Primeiro susto no corredor (`CorridorScareTrigger`).
- [x] Inimigo placeholder (`EnemyPlaceholder`).
- [x] `DemoRoomSequenceController`.

## Sprint 6 — Audio e polish de atmosfera (prioridade imediata)

- Adicionar `.ogg`: porta, radio, susto (caminhos em `PLAYTEST_DEMO_ROOM.md`).
- Ajustar volumes e distancias 3D no editor.
- UI dedicada de leitura do bilhete.
- Adicionar `PlayerStamina` ao `Player.tscn` e ligar ao HUD.

## Sprint 7 — Corredor e transicao

- Trocar corredor placeholder por cena modular Blender.
- Porta final / transicao de cena no fim do corredor.
- Escurecimento progressivo em `CorridorDarkZone`.

## Sprint 8 — Inimigo (sem combate completo)

- Modelar inimigo no Blender e substituir placeholder.
- Perseguicao simples (`CanChase`) sem dano ao player.
- Sons: respiracao e passos (`enemy_breath_01.ogg`, `enemy_step_01.ogg`).

## Sprint 9 — Combate (quando solicitado)

- Ataque do martelo com stamina e durabilidade.
- Dano ao inimigo e feedback visual/sonoro.
- Fallback soco quando martelo quebrar.

## Fora de escopo ate pedido explicito

- Combate completo balanceado.
- IA avancada com navmesh.
- Multiplos inimigos na demo room.
