> **⚠️ DOCUMENTO OBSOLETO — REBOOT GREENFIELD (2026-07-11)**  
> Não usar como fonte operacional. Ver `docs/testing/PLAYTEST_PROTOCOL.md`.

# Playtest - Pensao Santa Luzia Vertical Slice

Cena: `res://scenes/levels/pensao_santa_luzia/PensaoSantaLuziaVerticalSlice.tscn`

Base oficial M.3.1: cena independente usando `pensao_santa_luzia_vertical_slice_v01.glb`. Nao depende mais da cena integrada, builder ou filtro visual antigos.

Validacao automatica concluida: build sem erros, cena executada em headless, nos principais presentes e puzzle liberando deposito/andar superior. Itens de movimento continuam desmarcados ate playtest manual com F6.

## Correcao apos primeiro playtest visual

A primeira expansao foi removida: ela mantinha o interior importado quebrado e colocava o novo piso sobre a escada/deposito existentes. A versao atual oculta a edificacao antiga e cria uma mansao blockout unica, com fundo fechado e escada em uma ala separada. O validador usa a capsula real do Player para testar segmentos horizontais da rota. A inclinacao da rampa continua pendente de teste manual.

## Exterior

- [ ] Player nasce na trilha
- [ ] Player chega a varanda
- [ ] Porta principal livre
- [ ] Placa legivel

## Terreo

- [ ] Recepcao acessivel
- [ ] Livro interativo
- [ ] Corredor acessivel
- [ ] Quarto 102 acessivel
- [ ] Bilhete do Quarto 102 legivel
- [ ] Cozinha acessivel
- [ ] Fusivel coletavel
- [ ] Deposito inicialmente bloqueado
- [x] Puzzle do fusivel muda os tres estados por validacao automatica
- [x] Puzzle desativa blockers do deposito e da escada por validacao automatica

## Escada

- [ ] Player sobe sem travar
- [ ] Rampa invisivel funciona
- [x] Piso superior possui vao fisico sobre a rampa
- [ ] Areas nao prontas estao bloqueadas

## Segundo andar

- [ ] Corredor superior acessivel
- [ ] Quarto do gerente acessivel
- [ ] Banheiro basico visivel
- [ ] Quarto trancado bloqueado
- [ ] Interacao principal funciona
- [ ] Evento de passos funciona no HUD

## Sistemas

- [ ] HUD funciona durante percurso manual
- [ ] Lanterna funciona durante percurso manual
- [ ] Stamina funciona durante percurso manual
- [x] Sem loading entre exterior, terreo e segundo andar
- [ ] Sem colisoes invisiveis bloqueando
- [x] Segmentos horizontais principais sem bloqueio no teste de movimento
- [x] Sem fog cards ativos na cena base
- [x] Cena antiga arquivada sem referencia runtime

## Limpeza M.3.1

- [x] Cenas antigas mapeadas
- [x] Cena integrada quebrada arquivada
- [x] GLB integrado antigo arquivado
- [x] Builder/filtro visual antigos arquivados
- [x] Nenhuma referencia runtime para a cena antiga
- [x] Cena, Blender e GLB oficiais documentados

## Pendencias reais

- Validar toda a rota com o player real em F6.
- Ajustar altura/inclinacao da rampa apos teste manual.
- Ajustar escala e posicao das divisorias do segundo andar conforme leitura em primeira pessoa.
- Adicionar arte, texturas e props somente depois da navegabilidade aprovada.
