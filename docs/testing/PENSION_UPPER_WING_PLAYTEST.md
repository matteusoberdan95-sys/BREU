# Playtest — Saneamento 18C / ala superior

## Sprint 19 — Cômodos claustrofóbicos da ala superior

**Cena:** `UpperWingRooms.tscn` → `World/Level/SecondFloor/UpperWingRooms`  
**Deck:** `UpperWing_CollisionDeck` **não alterado** (`5, 2.4, 4.6` / `50×0,8×30,8`).  
**Sem** boundary global / mureta / collider solto / piso físico novo.

### Cômodos
- Corridor_UpperClaustro (intro + luz fraca)
- LaundryStorage (Fusível Superior)
- SharedBathroom (exame + espelho)
- Room204 (bilhete + cama)
- TechnicalRoom (painel)
- Room205_Locked (só mensagem)

### Puzzle / flags
`ReadRoom204Note` → `HasUpperFuse` → `IsUpperPowerRestored` → Room203 mensagem reforçada (não abre).

Sustos one-shot: CorridorIntro, BathroomScare, LaundryScare, Room204ExitScare.

### Regressão obrigatória
[ ] varanda andável sem queda / sem parede invisível  
[ ] térreo sem teleporte após abrir varanda  
[ ] porta verde + 203 OK  
[ ] rota cômodos completa sem prender  
[ ] Visible Collision Shapes: colliders só filhos de paredes; deck igual  

## ✅ CHECKPOINT — Varanda aprovada para gameplay (CONGELADA)

**Data:** 2026-07-12  
**Resultado:** APROVADA

Confirmado no playtest:
- [x] andar na varanda/laje superior;
- [x] sem queda no limbo;
- [x] sem teleporte do térreo para o segundo andar;
- [x] escada funcional;
- [x] porta verde funcional;
- [x] Quarto 203 acessível;
- [x] varanda aberta e navegável;
- [x] `UpperWing_CollisionDeck` intacto e congelado.

Congelado:
- não mexer no chão/deck da varanda;
- não recriar mureta / boundary / guarda-corpo / colliders soltos;
- não criar paredes invisíveis na área caminhável.

Próximas colisões: apenas paredes de cômodos novos, com collider filho da parede visual.

## Rollback — colliders invisíveis bugados da varanda

- último hotfix criou paredes invisíveis no meio da varanda (`BalconyWallColliders`);
- nodes removidos (não ajustados):
  - `World/Level/SecondFloor/Collisions/BalconyWallColliders` (container inteiro);
  - `BalconyWallCollider_Left`;
  - `BalconyWallCollider_Right`;
  - `BalconyWallCollider_FrontGuard`;
  - shapes `BalconyWallSideShape` / `BalconyWallFrontGuardShape`;
- F8: forward hit esperado nesses StaticBody3D quando o player batia no meio do caminho;
- `UpperWing_CollisionDeck` **não alterado** (`pos (5, 2.4, 4.6)`, `50×0,8×30,8`, layer/mask `1/0`);
- varanda volta ao estado aberto/navegável; sem boundary, mureta ou guarda-corpo;
- F9/LevelSanity passam a tratar `BalconyWallCollider*` como forbidden leftover.

Validação: [ ] circulação livre na varanda [ ] sem parede invisível [ ] deck intacto [ ] porta verde/203 [ ] térreo sem teleporte

## Hotfix final — colisão das paredes da varanda (REVERTIDO)

Tentativa de colliders finos por coordenada (`BalconyWallCollider_*`) bloqueou o caminho principal. Rollback completo; não recriar.

## Hotfix — varanda limpa e isolamento do térreo

### Boundary da varanda removida
- removidos: `BalconyBoundaryColliders`, `BalconyBoundary_Left`, `BalconyBoundary_Right`, `BalconyBoundary_Front` e shapes associados;
- preservados: `UpperWing_CollisionDeck` (chão funcional), `SecondFloor_MasterSlab` (visual), porta verde, Quarto 203, escada;
- sem mureta, rail global, placa escura ou boundary novo na varanda.

### Causa do teleporte térreo → segundo andar
- script: `DebugFallRecovery.cs`;
- bug: após visitar a ala superior (`Y >= 2,65`), qualquer posição com `Y < 1,9` dentro do AABB enorme do deck (`X=-20..30`, `Z=-10,8..20`) era tratada como queda e o player era mandado para `SafeMarker_SecondFloor`;
- isso incluía corredor e recepção do térreo após descer a escada.

### Correção
- `DebugFallRecovery` só ativa com `playerY < KillY` (`-3,0`);
- destino: `SafeMarker_Reception` se o último andar válido for o térreo; `SafeMarker_SecondFloor` só se a última posição válida foi segundo andar/varanda;
- logs: `[DebugFallRecovery] CHECK ...`, `TRIGGERED reason=...`, `ignored: player is on first floor`.

### Isolamento por andar
- volumes lógicos: `FirstFloorVolume`, `SecondFloorVolume`, `UpperWingVolume`;
- F8: `PlayerAreaProbe` — posição, andar estimado, raycasts, Area3D sobrepostas (ERROR se trigger superior pegar térreo);
- F9: `FloorTriggerIsolationChecker` + `LevelSanityChecker` — boundary ausente, deck ativo, triggers sem invadir térreo.

### Resultado esperado do playtest
- F8 no térreo (corredor/recepção): sem Area3D superior;
- F9: sem ERROR de isolamento / boundary;
- correr no primeiro andar após abrir a varanda: sem teleporte;
- varanda livre e navegável sem paredes/limites bugados.

Validação manual: [ ] varanda livre [ ] F8 térreo limpo [ ] F9 OK [ ] 3× rota térreo sem teleporte [ ] Visible Collision Shapes

Sem commit final até esses itens passarem.

## Hotfix final — colisão dos limites da varanda (REVERTIDO)

- tentativa anterior de `BalconyBoundaryColliders` poluiu a varanda com volumes/paredes;
- rollback completo no hotfix de isolamento; não recriar boundary global.

## Hotfix — UpperWing_CollisionDeck

- problema: o player ainda caía em partes da varanda, principalmente à direita, e atravessava o teto da recepção;
- solução: a colisão foi separada da laje visual e concentrada em `World/Level/SecondFloor/Floors/UpperWing_CollisionDeck`;
- `SecondFloor_MasterSlab` permanece somente visual, sem `StaticBody3D` ou `CollisionShape3D`;
- após análise do vídeo (queda na quina direita entre 30,8–32,9 s), a margem foi ampliada drasticamente;
- BoxShape3D final: `50 × 0,80 × 30,8 m`;
- posição global: `(5,00; 2,40; 4,60)`; topo em `Y=2,80`;
- AABB global: `X=-20,00..30,00`, `Y=2,00..2,80`, `Z=-10,80..20,00`;
- layer/mask `1/0`, copiados do piso funcional `PensionGroundFloor_MainFloor` (`AddSolid`);
- F8: 9 markers superiores e 2 markers inferiores atingiram `UpperWing_CollisionDeck`;
- F9: grade 7×7 passou `49/49`, sem buracos;
- o grid exclui outros corpos somente para medir a existência contínua do deck; blockers continuam cobertos pelo sanity check/inspeção manual;
- DebugFallRecovery durante rota normal: pendente de teste manual.

Teste manual: [ ] caminhada completa [ ] extrema direita [ ] diagonais [ ] retorno/203/porta verde [ ] pulos na recepção/entrada [ ] recovery não acionou

Sem commit final até o percurso manual passar.

## Hotfix definitivo — SecondFloor_MasterSlab

- problema confirmado no playtest: queda pela direita e travessia do teto da recepção ao pular;
- `SecondFloor_PhysicalSlab`, teto sul antigo e `Ceiling_Reception_Soffit` foram substituídos, não sobrepostos;
- peça única: `World/Level/SecondFloor/Floors/SecondFloor_MasterSlab`;
- MeshInstance3D e BoxShape3D: `23,7 × 0,60 × 19,4 m`;
- centro global `(4,15; 2,50; -1,10)`;
- AABB global `X=-7,70..16,00`, `Y=2,20..2,80`, `Z=-10,80..8,60`;
- layer/mask `1/0`, copiados do piso funcional `PensionGroundFloor_MainFloor` (`AddSolid`);
- `DebugFallRecovery` usa `SafeMarker_SecondFloor`; `SafeMarker_Reception` também está disponível;
- acionamento do failsafe na rota normal: pendente de teste manual.

Markers automáticos: [x] Start [x] Center [x] Right [x] FarRight [x] Left [x] Front [x] Back [x] Room203Path [x] GreenDoorPath [x] ReceptionJump [x] EntryJump — os onze atingiram exclusivamente `SecondFloor_MasterSlab` em runtime; os dois testes inferiores atingiram a face em `Y=2,20`.

Testes manuais: [ ] caminhada completa/diagonais/retorno [ ] pulo sob recepção/entrada [ ] failsafe não acionou na rota normal [ ] Visible Collision Shapes

Não aprovado e sem commit final enquanto os itens manuais estiverem pendentes.

## Hotfix crítico — laje física única

- mureta frontal, corpo físico, shape, área e prompt removidos;
- `BalconyRail_Right` residual removido;
- piso oficial: `World/Level/SecondFloor/Floors/SecondFloor_PhysicalSlab`;
- MeshInstance3D/BoxShape3D: `23,7 × 0,60 × 19,4 m`;
- centro `(4,15; 2,50; -1,10)`; AABB `X=-7,70..16,00`, `Y=2,20..2,80`, `Z=-10,80..8,60`;
- layer/mask `1/0`, copiados do piso funcional `PensionGroundFloor_MainFloor` (`AddSolid`);
- corredor e piso principal terminam na borda `Z=-10,80`, sem colisões sobrepostas;
- `DebugFallRecovery` é exclusivo de debug e retorna o player ao `SafeMarker` abaixo de `Y=-3`.

Markers da laje: [x] Start [x] Center [x] Right [x] FarRight [x] Left [x] Back [x] Front [x] Room203Path — todos atingiram exclusivamente `SecondFloor_PhysicalSlab` em runtime.

Markers de teto: [x] Reception_CeilingTest [x] Entrance_CeilingTest — ambos atingiram a face inferior da laje em `Y=2,20`.

Manuais: [ ] caminhada completa e diagonais [ ] pulos sob recepção/entrada [ ] Visible Collision Shapes

Sem aprovação ou commit final enquanto os testes manuais estiverem pendentes.

**Cena:** `PensaoVerticalBlockout01.tscn`

## Sprint 18C — obrigatório

- [ ] **F9** LevelSanityChecker → 0 ERROR
- [ ] entrada olhando para cima → forro limpo
- [ ] recepção olhando para cima → forro limpo
- [ ] sem salas tortas / divisórias da expansão antiga
- [ ] laje superior caminhável Start→End sem cair
- [ ] porta verde OK
- [ ] Quarto 203 OK
- [ ] escada OK
- [ ] F3 reset player OK
- [ ] F10/F11 / HUD / lanterna / áudio OK

## Nota

Cômodos 204 / banheiro coletivo / rouparia / gerador / 205 foram **removidos provisoriamente** da `UpperWingExpansion` (18C) para limpar a rota. Reintroduzir só depois da cena passar F9 + F6 limpos.
# Hotfix — queda na direita da laje superior

## Hotfix — escada e laje superior

- node que atravessava a escada: `Ceiling_FirstFloor_Seal` monolítico;
- causa: placa visual de teto cobria também o retângulo do stair shaft;
- correção: substituída por `Seal_South`, `Seal_North`, `Seal_West` e `Seal_East`, deixando o vazio central limpo;
- piso principal do segundo andar agora termina em `Z=-7,8`, onde começa a laje oficial, sem competição;
- `UpperWing_SolidFloor` final: centro global `(3,65; 2,65; -1,10)`;
- limites: `X=-4,70..12,00`, `Z=-7,80..5,60`, topo `Y=2,80`;
- mesh/shape: `16,7 × 0,30 × 13,4 m`, idênticos;
- layer/mask: `1/1`;
- F8 agora imprime raycast frontal da câmera e raycast inferior do player.

Markers automáticos:

- [x] Start → `(0; 2,80; -7,30)` → `UpperWing_SolidFloor`
- [x] Center → `(3,65; 2,80; -1,10)` → `UpperWing_SolidFloor`
- [x] Right → `(8,00; 2,80; -1,10)` → `UpperWing_SolidFloor`
- [x] FarRight → `(11,20; 2,80; -1,10)` → `UpperWing_SolidFloor`
- [x] Left → `(-4,00; 2,80; -1,10)` → `UpperWing_SolidFloor`
- [x] End → `(3,65; 2,80; 5,00)` → `UpperWing_SolidFloor`

Resultado automático: seis de seis raycasts atingiram exclusivamente o piso oficial.

O teste manual continua obrigatório: escada limpa, extrema direita, diagonais e retorno.

## Hotfix — laje ainda falhava na direita

O playtest confirmou que o piso preliminar de `X=-1,7..6,8` ainda era insuficiente. O F8 `UpperFloorCollisionProbe` foi criado para imprimir posição do player, collider inferior, transform, AABB, mesh, shape, layer e mask, além de testar todos os markers.

- coordenadas globais finais: `X=-2,70..9,00`, `Y topo=2,80`, `Z=-5,80..3,60`;
- centro global: `(3,15; 2,65; -1,10)`;
- MeshInstance3D: `11,7 × 0,30 × 9,4 m`;
- CollisionShape3D: `11,7 × 0,30 × 9,4 m`;
- layer/mask: `1/1`;
- parent `UpperWing_Extension`: identidade, sem rotação/escala;
- `UpperWing_SolidFloor`: sem rotação e escala global `(1,1,1)`.

Raycasts automáticos ao carregar a cena:

- [x] Start → `UpperWing_SolidFloor`
- [x] Center → `UpperWing_SolidFloor`
- [x] Right → `UpperWing_SolidFloor`
- [x] FarRight (`X=8,5`) → `UpperWing_SolidFloor`
- [x] Left → `UpperWing_SolidFloor`
- [x] End → `UpperWing_SolidFloor`

Topo da escada: auditoria estática encontrou somente sólidos gerados por `AddWall/AddSolid`, com mesh e collider pareados. Nenhum collider órfão foi removido sem evidência do probe. O teste manual da passagem continua obrigatório.

O limite norte `Z=-5,8` encosta exatamente no piso principal funcional do segundo andar; a sobreposição anterior com esse slab foi removida.

- [ ] F8 no ponto real antes da antiga queda
- [ ] percurso manual direita/esquerda/diagonais
- [ ] topo da escada sem contato invisível
- [ ] confirmação final: player não cai mais

## Diagnóstico — queda para a direita da laje superior

- ponto aproximado: lateral direita da área aberta, além de `X=1,7`;
- causa: quatro pisos parciais em `BalconyWing.tscn` competiam com um piso central de apenas 3,4 m em `UpperWingExpansion.tscn`;
- a área visual/cômodos alcançava `X=6,8`, mas a colisão central terminava perto de `X=1,7`;
- removidos: `BalconyLanding`, `BalconyWalkable`, `UpperWing_FreeWalkableFloor`, pisos individuais dos cômodos e `UpperBalcony_FrontWalkway`;
- piso oficial: `UpperWing_SolidFloor`;
- cobertura: `X=-1,70..6,80`, `Z=-7,60..2,60`;
- mesh/colisão: `8,5 × 0,30 × 10,2 m`, dimensões idênticas;
- layer/mask: 1/0, igual aos pisos funcionais do nível.

## Teste Start/Center/Right/Left/End

- [x] markers Start, Center, Right, Left e End criados sobre a mesma BoxShape;
- [x] análise estrutural confirma cobertura da lateral direita;
- [x] cena carrega com um único piso oficial;
- [ ] F6 com Visible Collision Shapes;
- [ ] Start → Center → Right → Center → Left → Center → End;
- [ ] End → Right → Left → Start;
- [ ] diagonais e ida/volta sem queda.

O resultado de gameplay permanece pendente até o percurso manual. Não commitar como concluído se houver qualquer queda.
