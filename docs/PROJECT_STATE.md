# BREU - Estado do projeto

Ultima atualizacao: 2026-07-09

## Resumo rapido

Vertical slice do Quarto 07 com corredor, susto, UI de bilhete dedicada e sistema de audio base (streams opcionais). Historico: `docs/SPRINT_HISTORY.md`.

## Estado jogavel atual

- Player FPS + lanterna + martelo na mao.
- HUD survival horror + **NoteReaderUI** (bilhete em painel papel velho).
- Porta, martelo, bilhete com feedback HUD; audio via **AudioManager** (sem .ogg = sem erro).
- Corredor + susto + inimigo placeholder + **porta trancada** no fim (mensagem HUD).
- `DemoRoom/UI`: HUD, NoteReaderUI, AudioManager.

## Fora de escopo

- Combate, IA de perseguicao, inimigo Blender final, transicao de cena, .ogg reais.

## Verificacao

`dotnet build BREU.sln` — 0 erros (2026-07-09).

## Proximo passo

Adicionar `.ogg`, porta final/transicao, ambience loops.

## Manutencao

Antes de commit/push: `docs/SPRINT_HISTORY.md`, `.cursor/rules/pre-commit-docs.mdc`, `.cursor/rules/conventional-commits.mdc`.
