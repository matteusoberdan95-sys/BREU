# Playtest — Pensao Santa Luzia Integrada

Cena: `res://scenes/levels/pensao_santa_luzia/PensaoSantaLuziaIntegratedTest.tscn`

## Sprint M.3 - rollback e estabilizacao

- Visual M.2 revertido: sem `cliff_left_slope_m02` ou `cliff_right_slope_m02`.
- GLB anterior restaurado; props e vegetacao anteriores voltaram.
- Fog cards e texto 3D da oferta ocultos no Godot.
- Colisoes manuais agrupadas em `StaticGameplayCollisions`; nenhuma colisao automatica foi adicionada ao GLB.
- Entrada manual tem aproximadamente 2,85 m livres e corredor aproximadamente 3,3 m entre paredes de colisao.
- Escada continua com rampa invisivel e bloqueio temporario no topo.
- Validacao automatica passou; todos os itens de movimento abaixo continuam pendentes de F6.

Os itens ficam desmarcados ate validacao manual no editor. Na Sprint M.2, o C# compilou, o GLB foi reexportado do `.blend`, importado e a cena carregou, instanciou e executou por 300 frames em headless sem erros.

## Problemas da primeira importacao

- Barrancos principais eram blocos altos, retos e muito proximos da leitura em primeira pessoa.
- Textos 3D da oferta, fachada e sinalizacao interna podiam sair da placa.
- Fog cards do Blender podiam aparecer como planos cinza no caminho/interior.
- A malha visual nao era uma base segura para colisao automatica e props pequenos podiam bloquear o player.
- Interior, porta e escada precisavam de um pass manual de navegabilidade.

## Correcoes aplicadas na Sprint M.2 (revertidas)

- Barrancos: `cliff_left_main`, `cliff_right_main`, chunks e detalhes apoiados no topo plano foram substituidos por dois taludes baixos, segmentados e inclinados. A borda interna comeca em X +/-3,25 m, preservando 6,5 m visuais de caminho.
- Placas: cinco objetos de texto 3D foram excluidos do GLB. Oferta, fachada, recepcao, deposito e Quarto 102 agora usam `Label3D` em `WorldLabels`.
- Colisao: continua manual em `StaticCollision`; limites laterais foram alargados, afastados e inclinados. Props pequenos, textos e decoracao nao receberam colisao.
- Interior: piso, paredes principais, balcao e deposito mantem caixas simples; entrada principal permanece sem blocker.
- Escada: rampa invisivel continua ativa; topo continua bloqueado com TODO para o segundo andar.
- Fog: `World_Fog` e seis objetos `fog_*` foram excluidos. A cena usa somente Environment + depth fog aprovado.

## Status tecnico

- [x] Build C# com 0 erros e 0 avisos
- [x] GLB exportado diretamente do `.blend` original
- [x] GLB importado pelo Godot
- [x] Validador confirmou taludes novos e ausencia dos barrancos/textos/fog proibidos
- [x] Cena executou por 300 frames em headless
- [ ] Percurso completo confirmado manualmente em primeira pessoa

## Pendencias

- Confirmar alinhamento frontal e legibilidade dos cinco `Label3D` no editor.
- Confirmar que a colisao inclinada dos limites nao invade a faixa caminhavel.
- Percorrer trilha -> varanda -> recepcao -> corredor -> deposito/escada com o player real.
- Ajustar iluminacao interna e posicoes finas somente com evidencia do playtest F6.

## Exterior

- [ ] Player nasce na trilha
- [ ] Player anda ate a pensao
- [ ] Colisao do chao funciona
- [ ] Barrancos impedem sair da area
- [ ] Porteira/cerca nao bloqueiam indevidamente
- [ ] Placa e props visiveis
- [ ] Neblina aparece sem quadrados

## Entrada

- [ ] Varanda acessivel
- [ ] Porta principal passavel
- [ ] Sem loading para entrar

## Interior

- [ ] Recepcao acessivel
- [ ] Balcao visivel
- [ ] Livro/registro visivel
- [ ] Corredor acessivel
- [ ] Deposito bloqueado
- [ ] Quarto 102 visivel
- [ ] Cozinha lateral visivel
- [ ] Escada testavel ou bloqueada corretamente

## Player

- [ ] Nao cai do mapa
- [ ] Nao atravessa paredes principais
- [ ] Nao enrosca na porta
- [ ] Nao enrosca na escada
- [ ] HUD continua funcionando
- [ ] Lanterna continua funcionando
- [ ] Stamina continua funcionando
