# Checklist obrigatório — alteração de cenário (Pensão)

**Regra:** nenhuma sprint de level design / blockout pode commitar sem passar por este checklist.

## Antes de commit

- [ ] Rodar **F4** → `LevelSanityChecker` e ler o Output.
- [ ] Não criar builder novo sem atualizar `PENSION_SCENE_OWNERSHIP.md`.
- [ ] Não criar duplicata de porta/piso/prompt.
- [ ] Não deixar `StaticBody3D` invisível sem nome/propósito.
- [ ] Todo piso novo tem **mesh + colisão**.
- [ ] Parede/teto/laje do 2º **não atravessa** o volume do térreo ao olhar para cima.
- [ ] Prompt não aparece longe / através de parede.
- [ ] Player anda sem cair na área alterada.
- [ ] Entrada + recepção: olhar para cima → forro limpo.
- [ ] Segundo andar: olhar para baixo → laje coerente (sem limbo).
- [ ] Porta verde preservada.
- [ ] Quarto 203 preservado.
- [ ] Escada preservada.
- [ ] HUD / lanterna / F10 / F11 / áudio / passos / respiração / fog.
- [ ] Puzzle térreo (chave → depósito → fusível) intacto.
- [ ] Sem inimigo / combate / chase.

## Referências

- Ownership: `docs/technical/PENSION_SCENE_OWNERSHIP.md`
- Métricas: `docs/technical/PENSION_LEVEL_METRICS.md`
- Validador: `scripts/debug/LevelSanityChecker.cs` (F4)
