# TrailIntro — Look Aprovado (Sprint L)

## Objetivo visual

Transformar a TrailIntro de blockout funcional em cena de terror jogável: terra escura, madeira velha, silhuetas frias e Pensão Santa Luzia como farol quente no fim da trilha.

## Neblina aprovada (NAO MEXER)

Preset oficial: **TrailIntro Fog Approved**

- `DepthFogPostProcess` + `depth_fog_postprocess.gdshader`
- Valores congelados — ver `LIGHTING_GUIDE.md`
- **Nesta sprint:** fog intocada

## Materiais (Sprint L)

### Trilha (`materials/environment/trail/`)

| Material | Uso |
|----------|-----|
| `mat_trail_dirt_dark.tres` | Caminho de terra |
| `mat_ground_dark.tres` | Terreno base |
| `mat_old_wood_dark.tres` | Cercas, tábuas, postes |
| `mat_cactus_dark.tres` | Cactos, arbustos secos |
| `mat_stone_cold_dark.tres` | Pedras |

### Pensão (`materials/environment/house/`)

| Material | Uso |
|----------|-----|
| `mat_house_plaster_dirty.tres` | Paredes sujas |
| `mat_roof_old_dark.tres` | Telhado |
| `mat_door_old_wood.tres` | Porta |
| `mat_window_dark.tres` | Janelas escuras |
| `mat_cross_dark_wood.tres` | Cruzes |
| `mat_cloth_dirty.tres` | Tecidos |
| `mat_lantern_warm_glow.tres` | Lampião (emissivo) |

Aplicação automática: `scripts/visual/TrailIntroVisualPass.cs`

## Paleta

- Noite fria — azul escuro (`#02060A` fundo)
- Madeira escura — marrom queimado
- Terra — marrom escuro dessaturado
- Pedra — cinza frio
- Pensão — bege sujo + luz quente `#D6A34A`
- Cactos — verde escuro seco

## Regras

1. **Nao** reativar FogVolume / particles / fog cards
2. **Nao** alterar depth fog sem motivo forte
3. Props via duplicação inteligente — nao bloquear caminho central
4. Colisões e gameplay intocados

## Grupos de cena

### Trilha
- `Trail_VisualEnhancement/Trail_Rocks`
- `Trail_VisualEnhancement/Trail_DryVegetation`
- `Trail_VisualEnhancement/Trail_Fence_Damage`
- `Trail_VisualEnhancement/Trail_Props`

### Pensão
- `House_Damage` — manchas, rachaduras, reboco quebrado
- `House_Props` — caixotes, pedras, vegetação seca
- `House_RitualDetails` — cruzes
- `House_Lights` — lampião

## Prints recomendados (validação)

1. Início da trilha (spawn)
2. Meio da trilha olhando para Pensão
3. Perto da entrada da Pensão
4. Fachada de frente

## Luz da Pensão

- `HouseEntranceWarmLight` — omni quente, flicker leve
- Nao alterar lanterna do player
