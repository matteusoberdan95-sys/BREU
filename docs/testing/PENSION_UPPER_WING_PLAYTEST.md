# Playtest — Saneamento 18C / ala superior

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
