> **⚠️ DOCUMENTO OBSOLETO — REBOOT GREENFIELD (2026-07-11)**  
> Não usar como fonte operacional. Ver `docs/production/SPRINT_ROADMAP.md`.

# BREU - Proximas tarefas

Ultima atualizacao: 2026-07-11

Plano detalhado: `docs/production/PHASE_01_02_SPRINT_PLAN.md`

## Concluido

- [x] Sprint M.3.1 - cena oficial independente, Blender/GLB oficiais, colisoes manuais e legado da Pensao arquivado.

- [x] Base da Sprint M.3 Vertical Slice: cena derivada, terreo aberto, segundo andar blockout, puzzle do fusivel, interacoes e marcador de inimigo futuro.

- [x] Sprint M.1 - cena isolada da Pensao Santa Luzia integrada, com percurso trilha -> varanda -> recepcao -> corredor sem loading, colisoes auxiliares, luzes, depth fog e interacoes placeholder.
- [x] Sprint M.2 - export direto do `.blend`, taludes inclinados, textos importantes em `Label3D`, GLB sem fog cards e validacao estrutural/headless.
- [x] Sprint M.3 - rollback dos taludes M.2, retorno ao GLB visual anterior e colisoes manuais simples em `StaticGameplayCollisions`.

- [x] Sprint A - porta final, interacao, fade, `RitualRoom` placeholder.
- [x] Passos por superficie.
- [x] Stamina sprint + pulo no HUD.
- [x] Agachamento (`Ctrl`).
- [x] Documentacao de visao Fases 1 e 2.
- [x] Audio pack v01, passos, pulo, bilhete, susto.
- [x] Sprint B base - `TrailIntro.tscn` com GLB, player, colisoes, luz, audio e trigger de chegada.
- [x] Sprint C base - `HouseExterior.tscn` com GLB, player, colisoes, luz, audio e triggers.
- [x] Sprint C.5 - costura jogavel do fluxo `TrailIntro -> DemoRoom`.

## Sprint B - Trilha de entrada

- [x] Blockout `TrailIntro.tscn`.
- [x] Instanciar `trail_intro_blockout.glb`.
- [x] Player no inicio da trilha.
- [x] Colisoes temporarias de chao e laterais.
- [x] Ambience + chegada ate a fachada integrada na trilha.
- [x] Porta da Pensao na trilha transiciona direto para `DemoRoom.tscn`.
- [ ] Ajustar colisoes conforme o GLB final.
- [ ] Adicionar bloqueios mais organicos: vegetacao seca, cerca, cactos, pedras.

## Sprint C - Fachada da Pensao Santa Luzia

- [x] Exterior blockout importado.
- [x] `HouseExterior.tscn` mantida como cena isolada de teste.
- [x] GLB visual da fachada integrado em `TrailIntro.tscn`.
- [x] Porta de entrada na trilha leva para `DemoRoom.tscn`.
- [x] Fade com mensagem entre cenas.
- [x] Checkpoints em memoria por cena.
- [x] Mensagens narrativas curtas na trilha, fachada e quarto.
- [ ] Validar escala/orientacao manualmente no editor.
- [ ] Ajustar colisoes da fachada.
- [ ] Criar porta visual/animada.
- [ ] Decidir se `HouseExterior.tscn` sera mantida apenas como teste ou reaproveitada futuramente.

## Sprint D - Sala dos Santos Secos

- [x] Substituir `RitualRoom.tscn` placeholder por asset Blender em nova cena.
- [x] Criar colisoes temporarias da sala, mesa e porta.
- [x] Criar luzes de vela/altar/fill escuro.
- [x] Criar bilhete ritual interativo.
- [x] Criar Chave Velha coletavel simples.
- [x] Persistir Chave Velha em `GameSession`.
- [x] Criar trigger de susto ritualistico.
- [x] Preparar `EnemyPlaceholder` para aparicao inicial.
- [x] Criar porta de saida bloqueada.
- [ ] Validar escala/orientacao da Sala dos Santos Secos no editor.
- [x] Integrar estado simples da Chave Velha ao `GameSession`.
- [ ] Criar objetivo para liberar porta de saida.

## Sprint E - Primeiro inimigo placeholder

- [x] `EnemyPlaceholderAI.cs` - perseguicao simples.
- [x] Ataque simples com dano no player.
- [x] `PlayerHealth.cs`.
- [x] Stun basico preparado.
- [x] Audio de respiracao/passos/growl.
- [x] Spawn/capsula do placeholder estabilizados na Sala dos Santos Secos.
- [x] Martelo Enferrujado persistente entre cenas via `GameSession`.
- [x] Ataque basico com martelo por raycast.
- [x] Stun/impacto do martelo no `EnemyPlaceholder`.
- [x] Durabilidade do martelo e quebra em 0/10.
- [ ] Reacao a lanterna em prototipo futuro.
- [x] Criar feedback visual/HUD de vida.

## Sprint F - Combate basico do martelo

- [x] Input `attack` = botao esquerdo.
- [x] `PlayerMeleeAttack.cs`.
- [x] Raycast melee a partir da camera.
- [x] Reduzir durabilidade apenas ao acertar.
- [x] Quebrar martelo e voltar para maos vazias.
- [ ] Integrar custo de stamina ao ataque.
- [ ] Criar animacao/audio finais de swing e impacto.

## Sprint G - Morte, retry e respawn

- [x] Finalizar `PlayerHealth` com morte e reset.
- [x] Mostrar `Vida` no HUD.
- [x] Criar flash vermelho de dano.
- [x] Criar tela `VOCE MORREU`.
- [x] Botao `Tentar novamente`.
- [x] Recarregar cena do ultimo checkpoint.
- [x] Restaurar snapshot simples do `GameSession`.
- [x] Parar inimigo quando player morre.
- [ ] Balancear dano, vida e invulnerabilidade por playtest.
- [ ] Criar save em disco futuramente.

## Sprint I - Hospede Seco blockout

- [x] Integrar `enemy_hospede_seco_blockout.glb` como visual do `EnemyPlaceholder`.
- [x] Ocultar capsula/cabeca antigas sem remover fallback.
- [x] Manter IA, colisao, hurtbox, ataque, stun e dano existentes.
- [x] Criar animacoes placeholder idle/walk/attack/hit/stunned/death por `EnemyAnimationController`.
- [ ] Validar escala/orientacao manualmente no editor.
- [ ] Criar rig e animacoes futuras.

## Sprint J - Player Feel cinematografico

- [x] `PlayerCameraFeel.cs` com headbob, camera shake, sway da lanterna e lean.
- [x] `PlayerBodyMotion.cs` substitui o feel ativo com gait procedural, shoulder sway, inercia, step impact e sway de mao.
- [x] Corrida suavizada para reduzir balanco lateral/roll excessivo.
- [x] Inputs `lean_left = Q` e `lean_right = R`.
- [x] `BreathAudio` no Player com `breath_light`, `breath_heavy` e `player_tired`.
- [x] `PlayerHealth` integrado ao shake de dano do body motion.
- [x] Movimento com aceleracao/desaceleracao simples.
- [x] `docs/design/GAME_VISION.md` com definicao oficial do estilo.
- [x] `docs/design/PLAYER_FEEL.md`.
- [ ] Playtest manual de conforto do body motion.
- [ ] Ajustar valores de feel apos feedback.

## Sprint K - Direcao Visual e Pipeline Grafico

- [x] Criar direcao visual oficial.
- [x] Criar pipeline grafico Blender -> Godot.
- [x] Criar biblioteca inicial de materiais.
- [x] Criar guia de iluminacao.
- [x] Criar guia de pos-processamento.
- [x] Criar guia de assets reutilizaveis.
- [x] Criar plano futuro para areas conectadas sem loading aparente.
- [x] Criar presets visuais iniciais.
- [x] Criar `VisualProfileApplier` opcional e seguro.
- [x] Criar `VisualLookdevRoom.tscn`.
- [x] Aplicar primeiro pass visual moderado em `TrailIntro`, `DemoRoom` e `RitualRoom`.
- [ ] Playtest manual de visibilidade nas tres cenas.
- [ ] Ajustar ambient/fill/fog se alguma cena ficar escura demais.

## Fora de escopo ate pedido

- Combate completo com combos/animacoes finais.
- Navmesh avancado.
- Multiplos inimigos.

## Proxima validacao - Sprint M.3

- [ ] Playtest manual no editor com F6 da cena integrada.
- [ ] Percorrer `PensaoSantaLuziaVerticalSlice.tscn`: trilha -> Quarto 102 -> cozinha -> deposito -> escada -> gerente.
- [ ] Ajustar rampa e divisorias superiores conforme o teste real do controller.
- [ ] Testar a rampa da cena oficial M.3.1 com F6 e ajustar apenas `StairRampCollision` se necessario.
- [ ] Confirmar em primeira pessoa o visual anterior, a placa de oferta, porta, recepcao, corredor e rampa.
- [ ] Ajustar colisao fina da varanda, porta e rampa somente apos o playtest.
- [ ] Liberar e produzir o segundo andar em sprint futura.
