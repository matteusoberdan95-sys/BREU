# Sprint 31 — material pass da Pensão Santa Luzia

## Escopo

Passe exclusivamente visual em paredes, pisos e tetos existentes. Nenhum transform estrutural, collider, porta, interação, trigger, puzzle, IA, perseguição, safe zone, escada ou deck físico foi alterado.

O conteúdo novo fica em `World/VisualPolish/Sprint31_Materials` e contém apenas `Node3D` e `MeshInstance3D`.

## Paleta compartilhada

Foram criados onze materiais reutilizados por setor:

- reboco antigo, reboco úmido e parede externa envelhecida;
- tábuas antigas, faixa de tráfego e piso de área de serviço;
- teto envelhecido e teto úmido;
- rodapé de madeira escura;
- mancha de umidade, sujeira de circulação e infiltração.

Todos usam alta rugosidade, zero metalicidade e cores discretas. Não há material ultrabrilhante nem textura chamativa.

## Aplicação

- Paredes: reboco bege/cinza envelhecido, com variante esverdeada em cozinha, banheiro e lavanderia.
- Pisos: madeira escura nos ambientes, desgaste mais forte em recepção/corredores e tom encardido nas áreas úmidas.
- Tetos: tom escuro envelhecido, com variante degradada nas áreas de serviço.
- Rodapés: gerados a partir do tamanho e transform das paredes existentes, sem colisão e sem atravessar vãos de porta.
- Manchas: treze marcas discretas no piso, cinco infiltrações no teto, vinte e uma manchas baixas de umidade e duas marcas de arrasto próximas ao Quarto 203.

O `UpperWing_CollisionDeck` foi explicitamente ignorado. O material do visual separado `SecondFloor_MasterSlab` pode mudar, mas o chão físico congelado, seu transform e seu `BoxShape3D` permanecem intactos.

## Validação automática

- [x] `dotnet build`: 0 erros / 0 avisos.
- [x] Cena oficial carregada no Godot.
- [x] F9: 0 ERROR / 0 WARNING.
- [x] Deck congelado: 49/49 pontos.
- [x] 38 paredes superiores continuam com colliders locais pareados.
- [x] 95 paredes, 6 pisos e 16 tetos receberam materiais.
- [x] 73 rodapés e 21 manchas de parede são visuais e sem colisão.
- [x] Zero `CollisionObject3D`, `CollisionShape3D`, `Area3D`, navegação, câmera ou luz no container.

## Playtest manual pendente

O commit final depende de F6 e do percurso completo: locomoção, portas, puzzle do ralo/painel, Quarto 203, duas perseguições, safe zones, esconderijos, quartos e segundo andar.
