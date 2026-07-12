# Auditoria de builders — ala da varanda

**Data:** 2026-07-12  
**Cena:** `PensaoVerticalBlockout01.tscn`

## Resultado

A ala era montada por geometria do `PensaoVerticalBlockout01Builder`, interações do `PensaoBalconyWingPuzzleSetup` e inicialização do `PensaoBalconyPuzzleSetup`. Essa divisão permitia que paredes, pisos, blockers e áreas antigas coexistissem com correções novas.

| Nó / script | `_Ready` | Mesh/StaticBody | Area/prompt | Situação após 17E |
|---|---:|---:|---:|---|
| `World/Builder` / `PensaoVerticalBlockout01Builder` | Sim | Sim, pensão geral | Interações gerais | Congelado para a ala: não cria porta, varanda ou cômodos; apenas mantém o vão estrutural |
| `World/PuzzleSetup` / `PensaoTerreoPuzzleSetup` | Sim | Não relevante | Puzzle térreo | Preservado sem alteração |
| `World/BalconyPuzzleSetup` / `PensaoBalconyPuzzleSetup` | Sim | Não | Nota/chave e inicialização da porta existente | Preservado como progressão; não cria geometria da ala |
| `World/BalconyWingPuzzleSetup` / `PensaoBalconyWingPuzzleSetup` | Sim | Criava props | Criava todas as áreas da ala | Removido da cena principal e `Enabled=false` por padrão |
| `BalconyWing` / `ManualBalconyWingController` | Sim | Não cria | Apenas inicializa nós já autorados | Ativo, sem criação runtime |

## Cena manual

`scenes/levels/pensao_santa_luzia/areas/BalconyWing.tscn` é a fonte única de:

- `BalconyDoor_Green`;
- patamar, piso e guarda-corpos;
- `Room_UpperBathroom` e `Room_OwnerBedroom`;
- colisões da microárea;
- `Interact_BalconyLookDown`, espelho, ralo, arame, porta do quarto e caderno.

As áreas são pequenas e posicionadas diante dos objetos. O raycast do player continua sendo a autoridade de foco e distância; nenhuma área da cena manual atravessa a parede da porta.

## Critério de congelamento

- `BuildUpperBalconyWing()` não é chamado.
- A porta verde não é criada por `BuildSecondFloorInteractions()`.
- `BalconyWingPuzzleSetup` não existe no Scene Tree.
- O arquivo histórico permanece com `Enabled=false`.
- A cena principal instancia `BalconyWing.tscn` exatamente uma vez.
