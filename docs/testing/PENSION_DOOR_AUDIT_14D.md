# Pensão Santa Luzia — Auditoria de portas 14D

Data: 11/07/2026  
Cena oficial: `scenes/levels/pensao_santa_luzia/PensaoVerticalBlockout01.tscn`

## Resultado

A cena não contém portas serializadas diretamente: a geometria é montada em runtime pelos builders. A auditoria, portanto, cobre os nós produzidos em runtime e os três prefabs oficiais em `scenes/props/doors/`.

| Nó runtime | Local aproximado | Moldura | Painel | Colisão própria | Decisão 14D |
|---|---|---:|---:|---:|---|
| `Door_MainEntrance_Frame` | Entrada principal, térreo | Sim | Não | Não | Manter somente moldura; folhas decorativas ocultas |
| `Door_ReceptionSouth_Frame` | Varanda → recepção | Sim | Folha aberta lateral | Não | Manter; folha fina, opaca e afastada da parede |
| `Door_ReceptionCorridor_Frame` | Recepção → corredor | Sim | Folha aberta lateral | Não | Manter |
| `Door_Room102_Frame` | Corredor, parede oeste | Sim | Folha aberta lateral | Não | Manter; abertura voltada para dentro do quarto |
| `Door_Kitchen_Frame` | Corredor, parede leste | Sim | Folha aberta lateral | Não | Manter |
| `Door_StairEntry_Frame` | Térreo, acesso à escada | Sim | Folha aberta lateral | Não | Manter; sem bloquear a escada |
| `Door_Deposit` | Final do térreo, Z = -26,5 | Sim | Sim, opaco | Sim | Manter; único painel destrancável |
| `Door_Room201_Frame` | Segundo andar, parede oeste | Sim | Folha aberta lateral | Não | Manter |
| `Door_Room202_Frame` | Segundo andar, parede leste | Sim | Folha aberta lateral | Não | Manter |
| `Door_UpperBlocked_Locked` | Final do corredor superior | Sim | Sim, opaco | Sim | Manter; porta trancada superior |
| `Door_UpperBalcony_Locked` | Varanda frontal superior | Sim | Sim, verde opaco | Sim | Manter; não abre nesta sprint |

## Estruturas com “Frame” no nome que não são portas duplicadas

| Nó | Função | Decisão |
|---|---|---|
| `Wall_Deposit_DoorFrame_Left/Right` | Segmentos da parede ao lado do vão do depósito | Manter; não são molduras sobrepostas |
| `Wall_Deposit_DoorHeader` | Parede acima do depósito | Manter |
| `Wall_UpperBlockedDoor_Frame_Left/Right` | Segmentos estruturais ao lado da porta superior | Manter |
| `Wall_UpperBlockedDoor_Header` | Parede estrutural acima da porta superior | Manter |
| `DoorThresholds/*Door*` | Placas finas de piso cobrindo emendas | Manter; sem interação ou bloqueio vertical |

## Prefabs oficiais

| Prefab | Painel | Bloqueio | Interação | Situação |
|---|---:|---:|---:|---|
| `DoorFrameOpen.tscn` | Apenas folha decorativa lateral opcional | Não | Não | Único prefab de passagem aberta |
| `DoorLocked.tscn` | Um painel a 0,06 m do plano da parede | Sim | Sim | Único prefab de porta permanentemente trancada |
| `DoorUnlockHidePanel.tscn` | Um painel a 0,06 m do plano da parede | Sim até destrancar | Sim | Único prefab do depósito |

## Removidos ou substituídos

- `DepositDoorInteraction.cs`: removido; referenciava nomes e colisões do sistema antigo.
- `UpperBalcony_Trail_DoorPanel`: removido; duplicava o painel verde.
- geradores `AddDoorFrameIn*`, `AddLockedDoorPanelZWall` e `BuildDepositDoorAssembly`: removidos.
- folhas decorativas da entrada principal: ocultas; a entrada usa somente moldura.
- placa de oferta: deslocada para fora do eixo de passagem da trilha.
- `UpperBalcony_BackWall`: corrigido de `Y = 1,45` para `Y = 4,25`; era o bloco retangular que aparecia no térreo diante da passagem.

## Verificações estruturais

- Materiais dos três tipos de porta têm transparência desativada.
- Painel visual e collider das portas fechadas usam a mesma posição local.
- Portas abertas não possuem `StaticBody3D` nem `CollisionShape3D`.
- O depósito abre somente com `DoorPanel.Visible = false` e `BlockingShape.Disabled = true`.
- O raycast não procura mais interações em nós irmãos de paredes ou pisos atingidos.
- A parede leste da caixa da escada foi estendida, mantendo livre a saída do patamar.

## Pendente de aprovação manual

- F6 completo com câmera próxima para confirmar ausência de flicker em hardware real.
- Rota chave → depósito → fusível.
- Colisão da porta verde e da porta superior.
- Entrada, quartos e cozinha sem aprisionar o Player.

## Sprint 14E — ajustes finais

- `Sign_PensaoSantaLuzia` adicionada na fachada principal.
- `Door_UpperBalcony_Locked` única no vão interior; removida da fachada da trilha.
- `ConfigureLockedDoor` / `ConfigureOpenDoor` com offsets anti z-fighting.
- Corredor inútil do 2º andar fechado (`UpperStair_BackClosureWall`, `UpperLanding_BackSeal`, `UpperStair_NorthEastSeal`).
- `Door_UpperBlocked_Locked` renomeada/substituída por `Door_UpperBalcony_Locked` com prompt de varanda.
