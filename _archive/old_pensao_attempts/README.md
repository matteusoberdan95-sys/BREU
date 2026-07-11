# Arquivo morto - tentativas antigas da Pensao Santa Luzia

Data da limpeza: 2026-07-11

Estes arquivos foram arquivados na Sprint M.3.1 porque dependiam do GLB integrado antigo ou de remendos que ocultavam/substituiam a edificacao em runtime. Eles nao devem ser usados por novas cenas.

## Cena oficial

- Godot: `res://scenes/levels/pensao_santa_luzia/PensaoSantaLuziaVerticalSlice.tscn`
- Blender: `assets/blender/trail_intro_pensao_vertical_slice_v01.blend`
- GLB: `assets/models/levels/pensao_santa_luzia/pensao_santa_luzia_vertical_slice_v01.glb`
- Exportador: `tools/blender/export_pensao_vertical_slice.py`

## Conteudo arquivado

- `scenes/`: `PensaoSantaLuziaIntegratedTest.tscn`.
- `assets/trail_intro_pensao_integrada_v01/`: GLB integrado antigo.
- `scripts/`: builder e filtro visual usados para remendar o interior antigo.
- `tools/`: exportador/validador da tentativa integrada.
- `docs/`: checklist antigo substituido pelo playtest da vertical slice.
- `source_receipts/`: GLB recebido que serviu para reconstruir o `.blend` oficial.
- `invalid_sources/`: `.blend` externo indicado no briefing, arquivado porque continha somente cubo, camera e luz padrao.

`_archive/.gdignore` impede que o Godot importe ou compile este legado.

O caminho original de `PensaoSantaLuziaIntegratedTest.tscn` foi recriado somente como wrapper de compatibilidade que instancia a cena oficial nova. Ele nao contem nem referencia a casa antiga arquivada.
