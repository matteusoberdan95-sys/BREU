# BREU - Handoff entre Codex e Cursor

Ultima atualizacao: 2026-07-09

## Como retomar

Ao abrir uma nova sessao no Codex, Cursor IDE ou Cursor CLI, leia:

1. `docs/START_HERE.md`
2. `docs/PROJECT_STATE.md`
3. `docs/gameplay/NEXT_SPRINT_TASKS.md`
4. `docs/technical/TDD.md`
5. O arquivo de agente em `docs/agents/` relacionado ao trabalho.

## Estado atual

O Quarto 07 esta jogavel como primeiro playtest da vertical slice:

- Player em primeira pessoa.
- Movimento WASD, mouse look, sprint e lanterna.
- HUD minimo com prompt de interacao.
- Bilhete interativo.
- Martelo coletavel.
- Martelo placeholder aparece na mao depois da coleta.
- Inventario simples registra martelo e durabilidade.
- Porta interativa em modo debug.
- Corredor placeholder conectado depois da porta.
- Trigger de fim de demo no final do corredor.
- Documentacao de continuidade atualizada para Codex e Cursor (`README.md`, `docs/START_HERE.md`, `AGENTS.md`, `.cursorrules` e `.cursor/rules/breu-project.mdc`).

## Principais arquivos

- Cena principal: `scenes/levels/demo_room/DemoRoom.tscn`
- Player: `scenes/player/Player.tscn`
- HUD: `scenes/ui/HUD.tscn`
- Estado do projeto: `docs/PROJECT_STATE.md`
- Guia de teste: `docs/testing/PLAYTEST_DEMO_ROOM.md`
- Arquitetura: `docs/technical/TDD.md`

## Sistemas implementados

### Player

- `PlayerController.cs`: movimento, sprint, gravidade e Input Map basico.
- `PlayerLook.cs`: mouse look, captura/liberacao de mouse.
- `FlashlightController.cs`: lanterna ligada/desligada e bateria via debug/HUD.
- `PlayerInteractor.cs`: RayCast3D da camera com alcance de 2.5m.
- `PlayerEquipmentView.cs`: mostra o martelo placeholder na mao quando o inventario tem martelo.

### Interacao

- `IInteractable.cs`: contrato `GetInteractionText()` e `Interact(PlayerController player)`.
- `InteractableNote.cs`: bilhete com texto no console.
- `HammerPickup.cs`: coleta martelo, atualiza inventario e tenta esconder visual importado.
- `DoorInteractable.cs`: abre porta em modo debug, desativa colisao e tenta esconder `door_01`.

### Inventario e HUD

- `PlayerInventory.cs`: guarda `HasHammer`, `EquippedWeaponName` e `EquippedWeaponDurability`.
- `HUDController.cs`: mostra prompt `[E] <acao>`, lanterna e arma equipada.

### Level

- `DemoRoom.tscn`: instancia GLB, player, HUD, colisoes, interativos, corredor placeholder e trigger de fim.
- `assets/blender_exports/quarto_07/quarto_07_blockout.glb`: asset importado canonico do Quarto 07.

## Validacao feita

```powershell
dotnet build BREU.sln
```

Resultado:

- Build com sucesso.
- 0 erros.
- 0 avisos.

```powershell
& 'C:\Users\mober\OneDrive\Desktop\Godot_v4.7-stable_mono_win64\Godot_v4.7-stable_mono_win64_console.exe' --headless --path . --quit
```

Resultado:

- Projeto carregou sem erro.

## Como testar manualmente

1. Abrir `DemoRoom.tscn` no Godot.
2. Rodar com F6.
3. Clicar em **Entrada** para dar foco.
4. Testar:
   - WASD e mouse look;
   - F para lanterna;
   - prompt no HUD ao mirar em bilhete/martelo/porta;
   - E para ler bilhete;
   - E para coletar martelo;
   - martelo aparecendo na mao;
   - E para abrir porta;
   - corredor placeholder e mensagem de fim.

## Bugs/limitacoes conhecidas

- HUD e minimo.
- Bilhete ainda nao tem tela dedicada de leitura.
- Porta nao tem animacao/pivo/som real.
- Martelo na mao e placeholder, sem animacao e sem combate.
- Corredor e placeholder, sem encontro, audio, porta final real ou transicao.
- Colisoes dos moveis/interativos podem precisar de ajuste fino visual no editor.
- Combate e inimigo nao devem ser ativados sem tarefa explicita.

## Proximo passo recomendado

Trocar o corredor placeholder por uma cena modular definitiva e criar uma porta final/transicao. Depois criar UI de leitura do bilhete e feedback visual/sonoro de interacao.
