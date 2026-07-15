# Sprint 32B — casca visual externa do telhado colonial

## Estado

Implementação visual concluída em 2026-07-14. A importação, a compilação e a carga headless da cena oficial foram executadas. O commit final permanece bloqueado até o playtest manual externo/interno, Visible Collision Shapes e a repetição final do F9 sem erros.

Checkpoint anterior à integração: `f87b330 checkpoint: before sprint 32b roof visual shell`.

## Assets importados

- `assets/models/architecture/pensao/roof/roof_main_colonial_01.glb`
- `assets/models/architecture/pensao/roof/roof_front_porch_colonial_01.glb`
- `assets/models/architecture/pensao/roof/roof_damage_tiles_01.glb`
- `assets/models/architecture/pensao/roof/roof_moss_patches_01.glb`

Dimensões locais auditadas antes da instalação:

| Asset | Dimensão local aproximada (m) |
|---|---:|
| telhado principal | `11,12 × 1,57 × 8,12` |
| telhado da entrada | `8,00 × 0,72 × 2,895` |
| telhas quebradas | `4,40 × 0,095 × 1,232` |
| musgo | `3,636 × 0,012 × 1,294` |

## Cenas reutilizáveis

- `scenes/architecture/pensao/roof/RoofMainColonial01.tscn`
- `scenes/architecture/pensao/roof/RoofFrontPorchColonial01.tscn`
- `scenes/architecture/pensao/roof/RoofDamageTiles01.tscn`
- `scenes/architecture/pensao/roof/RoofMossPatches01.tscn`

Cada wrapper possui apenas `Node3D/Visual` e a instância do GLB. Não contém `StaticBody3D`, `CollisionShape3D`, `Area3D`, `RigidBody3D`, navegação, câmera, luz ou script de gameplay.

## Instalação na cena oficial

Container único:

`World/ExteriorShell/Sprint32B_RoofVisualShell`

Transforms usados:

| Instância | Position | Rotation (rad) | Scale |
|---|---|---|---|
| `Roof_Main_Colonial` | `(0, 5.8, 8)` | `(0, 0, 0)` | `(1.36, 1, 1.08)` |
| `Roof_Front_Porch` | `(0, 5.74, 12.75)` | `(0, 0, 0)` | `(0.72, 1, 1)`; oculto após revisão |
| `Roof_Damage_Tiles` | `(-1.35, 6.72, 9.55)` | `(0.329867, 0, 0)` | `(1, 1, 1)` |
| `Roof_Moss_Patches` | `(2.8, 6.68, 6)` | `(-0.329867, 0, 0)` | `(1, 1, 1)` |

O telhado principal começa no topo interno validado, possui cerca de `15,12 m` de largura final e ultrapassa os `14,28 m` da fachada para formar beirais laterais. O beiral frontal avança aproximadamente `0,79 m` além da parede frontal. Telhas quebradas e musgo acompanham as duas águas com inclinação aproximada de 18,9 graus.

### Correção após o primeiro vídeo de fachada

A cobertura secundária de entrada tinha apenas `5,76 m` na escala instalada e aparecia empilhada acima da cobertura principal, como uma placa pequena e flutuante. Ela foi mantida no container para rastreabilidade, porém `visible=false`. A entrada passa a ser lida pelo beiral do telhado principal, sem o efeito de sanduíche. Nenhuma geometria interna foi ajustada para acomodar essa correção.

## Materiais

Foram preservados os materiais incorporados nos GLBs: barro antigo/escuro, madeira velha, bordas quebradas, sujeira preta e musgo escuro. Eles já usam metalicidade zero e roughness alta (`0,93` a `0,98`), portanto não foram criados overrides `.tres` duplicados. A decisão está registrada em `assets/materials/pensao/roof/README.md`.

## Segurança estrutural

- zero colisão no telhado;
- zero alteração no `UpperWing_CollisionDeck`;
- zero alteração em paredes, pisos, tetos internos, escada, varanda ou portas;
- zero alteração em puzzle, IA, perseguições, safe room e triggers;
- `LevelSanityChecker` agora rejeita qualquer nó físico ou de gameplay dentro do container 32B;
- marcadores de base, cumeeira e cobertura frontal foram adicionados apenas para auditoria de posicionamento.

## Validação executada

- importação Godot 4.7 dos quatro GLBs: concluída sem erro de importação;
- `dotnet build`: 0 erros; 1 aviso transitório de PDB bloqueado pela instância do editor já aberta;
- cena oficial carregada em modo headless: sem crash;
- auditoria 32B: quatro peças encontradas, zero física/gameplay, interior e deck congelado preservados;
- grid do deck existente: `49/49` pontos;
- paredes superiores existentes: `38` paredes pareadas;
- primeira execução automática do F9 durante a carga: `0 ERROR / 0 WARNING`.

Durante o encerramento headless, uma segunda execução diferida do F9 registrou dois erros antigos de ordem de aplicação da Sprint 31C (perfis da escada/patamar ainda não reaplicados naquele instante). Eles não apontam para o telhado, mas impedem considerar o F9 final aprovado antes de repetir o teste no editor.

## Playtest manual obrigatório antes do commit final

- [ ] da trilha, telhado principal, beiral e cobertura da entrada estão visíveis e coerentes;
- [ ] telhas parecem coloniais antigas, sem aparência moderna/plástica;
- [ ] telhado não flutua nem corta a fachada;
- [ ] telhado, telhas quebradas e musgo não aparecem dentro de quartos/corredores;
- [ ] entrada, janelas, portas, escada e varanda permanecem legíveis e navegáveis;
- [ ] sombras não escurecem o interior ou a fachada de forma absurda;
- [ ] puzzle, Quarto 203, IA, perseguições e esconderijo continuam funcionando;
- [ ] Visible Collision Shapes confirma ausência total de colisão no telhado;
- [ ] F9 manual final mostra `0 ERROR / 0 WARNING`.

## Próxima etapa

Após aprovação manual, fazer somente microajustes de `position`, `rotation`, `scale` ou sombra das meshes do telhado, se necessários. Nunca editar o interior para acomodar a cobertura. Depois, retomar o passe de deterioração Blender/Godot.
