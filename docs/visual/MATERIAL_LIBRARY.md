# Biblioteca de Materiais - BREU DE DENTRO

| Material | HEX base | Roughness | Metallic | Uso | Observacao |
|----------|----------|-----------|----------|-----|------------|
| `MAT_barro_seco` | `#7A6A55` | 0.9 | 0.0 | parede externa | usar variacoes com sujeira |
| `MAT_barro_escuro` | `#5B4D3E` | 0.95 | 0.0 | parede interna | bom para ambientes fechados |
| `MAT_madeira_velha` | `#3A2618` | 0.85 | 0.0 | portas/vigas/moveis | deve receber sombra forte |
| `MAT_madeira_queimada` | `#1D1713` | 0.95 | 0.0 | telhado/madeira podre | quase preta |
| `MAT_ferro_enferrujado` | `#4F3327` | 0.75 | 0.4 | metal velho | usar pouco brilho |
| `MAT_sangue_seco` | `#3A0705` | 0.65 | 0.0 | manchas | escuro e discreto |
| `MAT_vela_luz` | `#D6A34A` | 0.4 | 0.0 | chama visual | emissivo se necessario |
| `MAT_mofo` | `#2F3A2F` | 0.9 | 0.0 | umidade | usar em manchas |
| `MAT_tecido_sujo` | `#8A7A64` | 0.95 | 0.0 | roupa/rede/pano | sem brilho |
| `MAT_pele_doente` | `#8D7B6A` | 0.75 | 0.0 | inimigos | palida e morta |
| `MAT_pele_sombra` | `#5E4C40` | 0.85 | 0.0 | inimigos/sujeira | contraste |

## Regras

- Materiais do jogo devem ter aparencia opaca, suja e aspera.
- Evitar brilho plastico.
- Madeira, barro e pano devem ter roughness alto.
- Metal deve ter brilho controlado, nunca limpo demais.
- Sangue seco deve ser escuro, nao vermelho vivo.
- Luz de vela/lampiao deve vir principalmente de luzes no Godot, nao so do material.
