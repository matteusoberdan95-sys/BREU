# Checklist obrigatório — alteração de cenário (Pensão)

**Regra (Sprint 18C):** nenhuma sprint de cenário passa sem este checklist.

## Antes de commit

- [ ] Rodar **F9** → `LevelSanityChecker` e ler o Output.
- [ ] **0 ERROR** no LevelSanityChecker.
- [ ] Sem warnings graves (collider invisível, prompt duplicado, Area3D gigante).
- [ ] Não existem builders antigos da ala ativos (`BalconyWingPuzzleSetup`, `BalconyWing_Rebuilt`).
- [ ] Não existem nodes Old/Temp/Backup/Test/Legacy/Deprecated na árvore viva.
- [ ] Não existem colliders invisíveis sem função/nome.
- [ ] Todo piso novo tem colisão.
- [ ] Nenhum objeto do segundo andar invade o primeiro.
- [ ] Entrada principal olhando para cima está limpa.
- [ ] Recepção olhando para cima está limpa.
- [ ] Escada funciona.
- [ ] Porta verde funciona.
- [ ] Quarto 203 funciona.
- [ ] Prompts não aparecem longe / através de parede.
- [ ] Player não cai em piso novo.
- [ ] HUD, lanterna, F10/F11, áudio e movimento continuam funcionando.

## Referências

- Ownership: `docs/technical/PENSION_SCENE_OWNERSHIP.md`
- Métricas: `docs/technical/PENSION_LEVEL_METRICS.md`
- Validador: `scripts/debug/LevelSanityChecker.cs` (**F9**)
- Reset de player: ação `debug_reset_player` (**F3**)
