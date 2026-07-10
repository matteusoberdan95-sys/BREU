# BREU - Proximas tarefas

Ultima atualizacao: 2026-07-09

Plano detalhado: `docs/production/PHASE_01_02_SPRINT_PLAN.md`

## Concluido

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
- [ ] Reacao a lanterna em prototipo futuro.
- [ ] Integrar stun/impacto ao martelo.
- [ ] Criar feedback visual/HUD de vida.

## Sprint F - O Hospede no Blender

- [ ] Modelo apenas apos validar escala, susto e perseguicao no Godot.

## Fora de escopo ate pedido

- Combate completo com durabilidade de armas.
- Navmesh avancado.
- Multiplos inimigos.
