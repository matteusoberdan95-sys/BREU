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
