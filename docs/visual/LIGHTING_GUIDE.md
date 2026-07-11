# Guia de Iluminacao - BREU DE DENTRO

## Principio

O jogo deve ser escuro, mas jogavel. A escuridao deve esconder, sugerir e pressionar, mas o jogador ainda precisa entender o espaco com ajuda da lanterna, lua, vela ou lampiao.

Aplicar ajustes visuais com intensidade moderada. Se a cena ficar escura demais, reduzir contraste, aumentar ambient light ou diminuir fog.

## Exterior noturno

Usar:

- luz fria de lua;
- ceu azul escuro;
- contraste com luz quente da casa;
- sombras longas;
- neblina leve;
- poucos pontos de luz.

Preset sugerido:

- World background: `#101722`
- Ambient Light: `0.08` a `0.16`
- MoonLight: cor `#AFC4D6`, Energy `0.25` a `0.45`, Shadow Enabled `true`
- Lampiao exterior: cor `#D6A34A`, Energy `1.5` a `2.5`, Range `5` a `8`

## Interior Quarto 07

Usar:

- luz fraca e quente;
- cantos escuros;
- sombra pesada;
- luz da lanterna como ferramenta principal;
- contraste com pontos de horror.

Preset:

- World background: `#050505`
- Ambient Light: `0.03` a `0.08`
- Luz quente: cor `#D6A34A`, Energy `1.0` a `2.0`, Range `3` a `5`

## Sala dos Santos Secos

Usar:

- velas quentes;
- altar com luz fraca;
- radio/interferencia;
- sombra forte;
- inimigo aparecendo parcialmente no escuro.

Preset:

- World background: `#050505`
- Ambient Light: `0.03` a `0.06`
- CandleLightMain: cor `#D6A34A`, Energy `1.8` a `2.4`, Range `4` a `6`
- BackAltarLight: cor `#B6782E`, Energy `0.8` a `1.4`, Range `2.5` a `4`

## Regras

- Nunca iluminar tudo por igual.
- Usar pontos de luz para guiar o olhar.
- Luz quente deve indicar perigo/curiosidade.
- Luz fria deve indicar noite/isolamento.
- A lanterna deve ser importante.
- Evitar ambiente cinza chapado.

## Sprint K.1 — Ajustes aplicados

**Data:** 2026-07-10 | **Objetivo:** mudanca visual perceptivel sem quebrar gameplay.

Decisao: ajustes aplicados **manualmente** nas cenas. `VisualProfileApplier` permanece com `ApplyOnReady = false` em todas as cenas principais.

### TrailIntro

| Parametro | Valor final |
|-----------|-------------|
| Background | `#08111C` |
| Ambient color | `#101722` |
| Ambient energy | `0.12` |
| Fog enabled | `true` |
| Fog color | `#111A22` |
| Fog density | `0.022` |
| Fog aerial perspective | `0.5` |
| Tonemap exposure | `0.92` |
| Adjustment contrast | `1.10` |
| Adjustment saturation | `0.90` |
| SSAO | `enabled` |
| MoonLight color | `#AFC4D6` |
| MoonLight energy | `0.48` |
| MoonLight shadow | `enabled` (soft) |
| HouseFrontLanternLight color | `#D6A34A` |
| HouseFrontLanternLight energy | `2.85` (flicker `2.0–3.2`) |
| HouseFrontLanternLight range | `9.5` |
| TrailPathFill energy | `0.16` (preenchimento frio minimo) |

### DemoRoom / Quarto 07

| Parametro | Valor final |
|-----------|-------------|
| Background | `#050505` |
| Ambient color | `#11100D` |
| Ambient energy | `0.05` |
| Tonemap exposure | `0.90` |
| Adjustment contrast | `1.12` |
| Adjustment saturation | `0.88` |
| SSAO | `enabled` |
| RoomLightPoint color | `#D6A34A` |
| RoomLightPoint energy | `1.75` |
| RoomLightPoint range | `4.2` |
| NoteSpotLight energy | `0.38` (destaque mesa/bilhete) |
| DebugLight | `visible = false` |
| CorridorLight_01 energy | `0.32` |

### RitualRoom / Sala dos Santos Secos

| Parametro | Valor final |
|-----------|-------------|
| Background | `#050505` |
| Ambient color | `#0F0D0A` |
| Ambient energy | `0.038` |
| Tonemap exposure | `0.88` |
| Adjustment contrast | `1.14` |
| Adjustment saturation | `0.86` |
| SSAO | `enabled` |
| Glow bloom | `0.10` (sutil em velas) |
| CandleLightMain position | `(0, 1.25, -0.75)` alinhada a mesa |
| CandleLightMain color | `#D6A34A` |
| CandleLightMain energy | `2.65` (flicker `2.0–2.95`) |
| CandleLightMain range | `5.5` |
| BackAltarLight color | `#B6782E` |
| BackAltarLight energy | `1.25` (flicker `0.9–1.4`) |
| BackAltarLight range | `3.5` |
| RoomDarkFill energy | `0.10` (reduzido para sombra no inimigo) |

### Problemas conhecidos (K.1)

- Fog depende do `WorldEnvironment` ativo da cena; nao ha segundo environment sobrescrevendo.
- Blockout ainda parece flat em partes; texturas/materiais finais ficam para sprint futura.
- SSAO pode ter custo em hardware fraco; desligar por cena se necessario.

## Sprint K.2 — Rebalanceamento de Iluminacao e Neblina

**Data:** 2026-07-10 | **Motivo:** K.1 ficou escura demais; fog exponencial quase invisivel.

Principio: **horror legivel** — navegacao funcional sem lanterna; lanterna continua util para detalhes.

### TrailIntro

| Parametro | Valor final K.2 |
|-----------|-----------------|
| Background | `#08131D` |
| Ambient color | `#0E1620` |
| Ambient energy | `0.21` |
| Fog mode | **Depth** (`fog_mode = 1`) |
| Fog color | `#162029` |
| Fog density | `0.035` (backup exponencial) |
| Fog depth begin / end | `8.0` / `34.0` |
| Fog depth curve | `1.15` |
| Tonemap exposure | `1.02` |
| Adjustment brightness | `1.04` |
| Adjustment contrast | `1.05` |
| SSAO intensity | `0.85` (reduzido) |
| MoonLight energy | `0.62` |
| TrailPathFill energy | `0.30` |
| HouseFrontLanternLight energy | `3.15` (flicker `2.4–3.6`) |
| HouseFrontLanternLight range | `11.0` |

**Fog:** trocado para depth fog para aparecer a distancia na trilha. Um unico `WorldEnvironment` ativo; `VisualProfileApplier` com `ApplyOnReady = false`.

### DemoRoom / Quarto 07

| Parametro | Valor final K.2 |
|-----------|-----------------|
| Background | `#080706` |
| Ambient color | `#12100D` |
| Ambient energy | `0.11` |
| Tonemap exposure | `0.98` |
| Adjustment brightness | `1.02` |
| Adjustment contrast | `1.06` |
| SSAO intensity | `1.0` |
| RoomLightPoint energy | `2.7` |
| RoomLightPoint range | `5.8` |
| NoteSpotLight energy | `0.52` |
| CorridorLight_01 energy | `0.42` |

### RitualRoom / Sala dos Santos Secos

| Parametro | Valor final K.2 |
|-----------|-----------------|
| Background | `#080706` |
| Ambient energy | `0.095` |
| Tonemap exposure | `0.96` |
| Adjustment contrast | `1.08` |
| CandleLightMain energy | `2.95` (flicker `2.2–3.2`) |
| CandleLightMain range | `5.8` |
| BackAltarLight energy | `1.45` (flicker `1.0–1.6`) |
| BackAltarLight range | `4.0` |
| RoomDarkFill energy | `0.24` (teto/paredes legiveis) |

### Problemas conhecidos (K.2)

- Fog depth pode precisar ajuste fino por resolucao/GPU; testar `fog_depth_end` entre `28` e `38`.
- Blockout ainda limita contraste visual mesmo com luz rebalanceada.
- Se a trilha ainda parecer escura, subir `ambient_light_energy` para `0.24` antes de aumentar luzes quentes.

## Sprint K.3 — Correção de Iluminação Prática e Neblina

**Data:** 2026-07-10 | **Motivo:** lâmpada do quarto não iluminava; fog ainda invisível; áreas pretas demais.

### Auditoria (pré-ajuste)

| Cena | WorldEnvironment | VisualProfileApplier | ApplyOnReady | Luzes principais |
|------|------------------|----------------------|--------------|------------------|
| TrailIntro | 1 ativo | presente, não aplica | `false` | MoonLight, HouseEntranceWarmLight |
| DemoRoom | 1 ativo | presente, não aplica | `false` | RoomLightPoint (fraca/mal posicionada) |
| RitualRoom | 1 ativo | presente, não aplica | `false` | CandleLightMain, BackAltarLight |

**Conclusão:** `VisualProfileApplier` **não** sobrescrevia Environment. Problema era luz prática fraca/desalinhada e fog só depth/exponencial sem volumétrico.

### TrailIntro — valores finais K.3

| Parametro | Valor |
|-----------|-------|
| Ambient energy | `0.20` |
| Ambient color | `#101822` |
| Fog depth begin / end | `6.0` / `45.0` |
| Fog density | `0.045` |
| Fog color | `#18222C` |
| **Volumetric fog** | `enabled`, density `0.022`, length `48` |
| MoonLight energy | `0.60` |
| HouseEntranceWarmLight energy | `3.8` (flicker `2.8–4.2`) |
| HouseEntranceWarmLight range | `12.0` |

### DemoRoom — valores finais K.3

| Parametro | Valor |
|-----------|-------|
| Ambient energy | `0.12` |
| Ambient color | `#14110D` |
| Exposure | `1.05` |
| SSAO intensity | `0.65` |
| **RoomCeilingLight** (nova) | energy `3.2`, range `6.5`, alinhada a `lamp_bulb` |
| RoomWallFill | energy `0.35` |
| `DemoRoomLightingSetup` | emissão no bulbo + snap da luz |
| RoomLightPoint | desativada (legado) |

### RitualRoom — valores finais K.3

| Parametro | Valor |
|-----------|-------|
| Background | `#050505` |
| Ambient energy | `0.09` |
| Ambient color | `#100C08` |
| CandleLightMain energy | `3.2`, range `6.0` |
| BackAltarLight energy | `1.5`, range `4.5` |
| RoomDarkFill energy | `0.32` |

### Fog visível?

Sim — correção K.3 habilita **fog depth + volumetric fog** no Forward+ (`config/features=Forward Plus`). Se ainda invisível em hardware específico, subir `volumetric_fog_density` para `0.03`.

### Problemas conhecidos (K.3)

- Lanterna do player não foi alterada nesta sprint (foco em luzes de ambiente).
- Blockout ainda limita leitura visual mesmo com luz corrigida.
- Volumetric fog tem custo GPU; reduzir density se FPS cair.

## Sprint K.3.1 — Correção definitiva da neblina da TrailIntro

**Data:** 2026-07-10 | **Escopo:** apenas exterior da trilha.

### Auditoria (Tarefa 1)

| Verificação | Resultado |
|-------------|-----------|
| WorldEnvironment na TrailIntro | **1 nó** (`TrailIntro/WorldEnvironment`) |
| Outro WorldEnvironment na cena | **Não** |
| Environment global no projeto | **Não** (`project.godot` sem override) |
| VisualProfileApplier sobrescreve | **Não** (`ApplyOnReady = false`) |
| Câmera usa Environment da cena | **Sim** (player sem override) |

**Causa raiz:** fog depth/volumétrico estava **fraco demais** (`volumetric_fog_density = 0.022`). Forward+ precisa de density maior + **FogVolumes** para neblina perceptível no gameplay.

### WorldEnvironment — valores finais K.3.1

| Parametro | Valor |
|-----------|-------|
| Background | `#071019` |
| Ambient color | `#0E1620` |
| Ambient energy | `0.22` |
| Fog enabled | `true` |
| Fog color | `#1A2630` |
| Fog density | `0.075` |
| Fog depth begin / end | `3.0` / `30.0` |
| Fog height density | `0.11` |
| Volumetric fog density | `0.072` |
| Volumetric fog length | `32.0` |
| Volumetric fog albedo | azul-cinza frio |

### Reforço visual (FogVolumes + partículas)

| Nó | Função |
|----|--------|
| `TrailFogVolumeNear` | neblina baixa perto do caminho (density `0.06`) |
| `TrailFogVolumeMid` | faixa central mais densa (density `0.12`) |
| `TrailFogVolumeFar` | fundo mais leve — pensão continua visível (density `0.08`) |
| `TrailMistParticles` | GPUParticles3D sutil (56 partículas, alfa baixo) |

### Iluminação externa K.3.1

| Luz | Valor |
|-----|-------|
| MoonLight energy | `0.75` |
| HouseEntranceWarmLight energy | `4.0` (flicker `3.2–5.0`) |
| HouseEntranceWarmLight range | `12.0` |
| TrailSilhouetteFill energy | `0.22` (fill frio mínimo) |

### Fog visível agora?

**Sim, por design:** combinação de depth fog + volumetric fog reforçado + 3 FogVolumes + partículas leves.

Se ainda parecer fraco: subir `TrailFogVolumeMid` density para `0.15` ou `volumetric_fog_density` para `0.09`.

### Problemas conhecidos (K.3.1)

- FogVolumes + partículas aumentam custo GPU; reduzir `TrailMistParticles.amount` se necessário.
- DemoRoom/RitualRoom **não** foram alterados nesta subsprint.

## Sprint K.3.2 — Refino Realista da Neblina

**Data:** 2026-07-10 | **Escopo:** TrailIntro + qualidade volumétrica global + lanterna do player.

### Auditoria — valores **antes** (K.3.1)

| Parametro | Valor K.3.1 |
|-----------|-------------|
| Fog color | `#1A2630` |
| Fog density | `0.075` |
| Fog depth begin / end | `3.0` / `30.0` |
| Fog height density | `0.11` |
| Volumetric fog density | `0.072` |
| Volumetric fog length | `32.0` |
| TrailFogVolumeMid density | `0.12` |
| MoonLight energy | `0.75` |
| HouseEntranceWarmLight energy / range | `4.0` / `12.0` |
| Flashlight energy / range / angle | `3.4` / `11.0` / `34°` |
| Flashlight volumetric contribution | `1.0` (padrao) |
| VisualProfileApplier sobrescreve | **Nao** (`ApplyOnReady = false`) |
| Project volumetric fog size / depth | `64` / `64` (padrao Godot) |

**Diagnostico:** neblina forte demais + spotlight com contribuicao volumetrica alta + froxel buffer padrao = **quadrados/blocos** quando a lanterna ilumina a fog.

### Presets de teste (TrailIntro)

#### Fog Leve

| Parametro | Valor |
|-----------|-------|
| Fog density | `0.045` |
| Fog color | `#16222C` |
| Depth begin / end | `8.0` / `42.0` |

#### Fog Medio *(aplicado)*

| Parametro | Valor |
|-----------|-------|
| Fog density | `0.055` |
| Fog color | `#16222C` |
| Depth begin / end | `6.0` / `38.0` |

#### Fog Forte

| Parametro | Valor |
|-----------|-------|
| Fog density | `0.070` |
| Fog color | `#1A2630` |
| Depth begin / end | `5.0` / `34.0` |

Se **Fog Medio** ainda bloquear com lanterna: usar **Fog Leve** + particulas sutis (ja configuradas menores).

### WorldEnvironment — valores finais K.3.2

| Parametro | Valor |
|-----------|-------|
| Fog enabled | `true` |
| Fog color | `#16222C` |
| Fog density | `0.055` |
| Fog depth begin / end | `6.0` / `38.0` |
| Fog depth curve | `1.25` |
| Fog height density | `0.06` |
| Volumetric fog density | `0.050` |
| Volumetric fog length | `36.0` |
| Volumetric fog albedo | azul-cinza frio escuro |
| Volumetric temporal reprojection | `true` |

### Qualidade volumetrica (`project.godot`)

| Setting | Antes | Depois |
|---------|-------|--------|
| `rendering/environment/volumetric_fog/volume_size` | `64` | `96` |
| `rendering/environment/volumetric_fog/volume_depth` | `64` | `96` |
| `rendering/environment/volumetric_fog/use_filter` | `1` | `1` (mantido) |

**Impacto esperado:** froxels maiores + filtro ativo = menos grade/quadrados visiveis com luzes dinamicas.

### FogVolumes + particulas (K.3.2)

| No | Density K.3.1 | Density K.3.2 |
|----|---------------|---------------|
| `TrailFogVolumeNear` | `0.06` | `0.038` |
| `TrailFogVolumeMid` | `0.12` | `0.065` |
| `TrailFogVolumeFar` | `0.08` | `0.048` |

| `TrailMistParticles` | K.3.1 | K.3.2 |
|----------------------|-------|-------|
| Quantidade | `56` | `72` |
| Quad size | `1.8` | `0.85` |
| Alpha | `0.12–0.14` | `0.07–0.08` |
| Cor base | `#9CADBB` | `#AFC4D6` |

Particulas **mantidas** como complemento — menores e mais transparentes para evitar placas quadradas.

### Lanterna do player (K.3.2)

| Parametro | Antes | Depois |
|-----------|-------|--------|
| `light_energy` | `3.4` | `2.9` |
| `light_volumetric_fog_energy` | `1.0` | `0.45` |
| `spot_range` | `11.0` | `10.0` |
| `spot_angle` | `34°` | `32°` |

Lanterna continua util para ler o caminho; contribuicao volumetrica reduzida para nao estourar a neblina.

### Iluminacao externa K.3.2

| Luz | Valor |
|-----|-------|
| MoonLight energy | `0.65` (`#AFC4D6`) |
| MoonLight volumetric contribution | `0.65` |
| HouseEntranceWarmLight energy / range | `4.2` / `13.0` |
| HouseEntranceWarmLight volumetric contribution | `0.75` |
| TrailSilhouetteFill energy | `0.22` (inalterado) |

### Preset escolhido

**Fog Medio** — neblina visivel sem parecer leite branco; quadrados da lanterna mitigados via qualidade volumetrica + contribuicao reduzida.

### Problemas conhecidos (K.3.2)

- `volume_size`/`volume_depth` em `96` aumentam custo GPU levemente; reduzir para `80` se FPS cair.
- Se fog ainda parecer forte: aplicar preset **Fog Leve** (`density 0.045`, depth begin `8.0`).
- Se fog sumir demais: subir `TrailFogVolumeMid` para `0.075` ou `fog_density` para `0.065`.
- Playtest visual F6 obrigatorio — ajustes finos podem variar por GPU/resolucao.

## Sprint K.3.3 — Neblina Hibrida e Remocao dos Artefatos em Grade

**Data:** 2026-07-10 | **Escopo:** TrailIntro — troca para fog hibrida (depth + particulas), volumetric fog secundaria.

### Auditoria — configuracao **anterior** (K.3.2)

| Parametro | Valor K.3.2 |
|-----------|-------------|
| Fog density (depth) | `0.055` |
| Fog depth begin / end | `6.0` / `38.0` |
| Volumetric fog density | `0.050` |
| FogVolumes ativos | **Sim** (Near/Mid/Far) |
| Particulas | `TrailMistParticles` unica (`72`) |
| Flashlight volumetric energy | `0.45` |
| Project volume size / depth | `96` / `96` |
| VisualProfileApplier | **Nao sobrescreve** |

**Diagnostico K.3.3:** artefatos em grade persistiam porque **volumetric fog + FogVolumes** dominavam o visual e reagiam demais a luzes dinamicas (lanterna/lua).

### Estrategia hibrida (K.3.3)

| Camada | Funcao |
|--------|--------|
| **Depth fog** | Protagonista — profundidade fria no meio/fundo |
| **Volumetric fog leve** | Base sutil (`0.020`), nao a neblina inteira |
| **GPUParticles3D x3 + ground** | Neblina organica distribuida na trilha |
| **FogVolumes** | **Desativados** (`visible = false`) — evitam grade |

### WorldEnvironment — valores finais K.3.3

| Parametro | Valor |
|-----------|-------|
| Fog enabled | `true` |
| Fog color | `#14202A` |
| Fog density (depth) | `0.048` |
| Fog depth begin / end | `8.0` / `40.0` |
| Fog depth curve | `1.18` |
| Fog height density | `0.05` |
| Volumetric fog density | **`0.020`** (leve) |
| Volumetric fog length | `28.0` |
| Volumetric temporal reprojection | `true` |

### Qualidade volumetrica (`project.godot`)

| Setting | K.3.2 | K.3.3 |
|---------|-------|-------|
| `volume_size` | `96` | **`128`** |
| `volume_depth` | `96` | **`128`** |
| `use_filter` | `1` | `1` |

### Particulas hibridas (nos criados)

| No | Posicao (Z) | Amount | Funcao |
|----|-------------|--------|--------|
| `TrailMistParticles_A` | `~11` (inicio) | `48` | neblina leve no spawn |
| `TrailMistParticles_B` | `~0` (meio) | `64` | faixa central mais densa |
| `TrailMistParticles_C` | `~-11` (fim) | `52` | profundidade perto da pensao |
| `TrailMistGroundLow` | `~-2`, Y `0.35` | `44` | neblina rasteira no chao |

Material compartilhado: quad `0.65`, alpha `0.04–0.06`, cor `#AFC4D6`, `distance_fade` ativo, blend aditivo.

### Lanterna do player (K.3.3)

| Parametro | K.3.2 | K.3.3 |
|-----------|-------|-------|
| `light_energy` | `2.9` | **`2.75`** |
| `light_volumetric_fog_energy` | `0.45` | **`0.22`** |
| `spot_range` | `10.0` | **`9.5`** |
| `spot_angle` | `32°` | **`31°`** |

### Iluminacao externa K.3.3

| Luz | Valor |
|-----|-------|
| MoonLight energy / volumetric | `0.68` / **`0.30`** |
| HouseEntranceWarmLight energy / range / volumetric | **`4.3`** / **`13.5`** / **`0.40`** |
| TrailSilhouetteFill volumetric | **`0.12`** |

### Resultado esperado

- Neblina **visivel e organica** sem parecer grade digital.
- Lanterna **util** para ler o caminho, sem revelar froxels.
- Pensao continua **farol quente** contrastando com cena fria.
- DemoRoom / RitualRoom / interior **nao alterados**.

### Problemas conhecidos (K.3.3)

- Particulas podem parecer fracas em GPUs com bloom desligado — subir alpha para `0.07` em `TrailMistParticles_B` se necessario.
- Se neblina sumir demais: subir `fog_density` para `0.055` (nao reativar FogVolumes).
- `volume_size`/`depth` em `128` tem custo GPU moderado; reduzir para `112` se FPS cair.
- Residuos leves de grade ainda possiveis em hardware com volumetric fog de baixa resolucao — proximo passo seria desligar volumetric (`density 0`) e usar so depth + particulas.

## Sprint K.3.4 — Restaurar Neblina Visivel da TrailIntro

**Data:** 2026-07-10 | **Escopo:** hotfix **apenas** `TrailIntro.tscn`.

### Auditoria — o que estava matando a neblina (K.3.3)

| Verificacao | Resultado |
|-------------|-----------|
| WorldEnvironment ativo | **Sim** |
| Fog enabled | **Sim** — mas parametros fracos |
| Volumetric fog enabled | **Sim** — density **`0.020`** (quase invisivel) |
| Fog depth begin | **`8.0`** — neblina comecava longe demais da camera |
| Fog density | **`0.048`** — abaixo do minimo perceptivel |
| FogVolumes | **Desativados** (`visible = false`) |
| TrailMistParticles A/B/C/Ground | **Ativos**, mas alpha **`0.04–0.06`** + `distance_fade` escondia perto da camera |
| VisualProfileApplier | **`ApplyOnReady = false`** — **nao interferia** |
| Script desligando fog em runtime | **Nao encontrado** |

**Causa raiz:** K.3.3 resolveu quadrados **apagando** a neblina (volumetric quase zero, depth begin alto, FogVolumes off, particulas invisiveis).

### Valores finais K.3.4

| Parametro | K.3.3 (sumiu) | K.3.4 (restaurado) |
|-----------|---------------|---------------------|
| Background | `#071019` | `#071019` |
| Ambient energy | `0.22` | **`0.20`** |
| Fog color | `#14202A` | **`#16222C`** |
| Fog density | `0.048` | **`0.055`** |
| Fog depth begin / end | `8.0` / `40.0` | **`5.0` / `34.0`** |
| Fog height density | `0.05` | **`0.055`** |
| Volumetric fog density | `0.020` | **`0.042`** |
| Volumetric fog length | `28.0` | **`32.0`** |
| FogVolumes Near/Mid/Far | off | **reativados** (`0.032` / `0.045` / `0.032`) |
| TrailMistParticles alpha | `0.04–0.06` | **`0.065–0.09`** (sem distance fade) |
| MoonLight energy | `0.68` | **`0.60`** |
| HouseEntranceWarmLight | `4.3` / `13.5` | **`4.0` / `12.0`** |

### Lanterna do player

**Nao alterada nesta sprint** (escopo restrito a TrailIntro). Valores atuais do player: energy `2.75`, volumetric `0.22` (K.3.3).

### Presets de emergencia (se ainda fraco no playtest)

| Preset | Fog density |
|--------|-------------|
| Padrao K.3.4 | `0.055` |
| Reforco leve | `0.065` |
| Reforco forte | `0.075` |
| Se branco demais | voltar para `0.060` |

### Resultado visual esperado

- Neblina **visivel** com e sem lanterna.
- Trilha **nao limpa** — atmosfera de terror restaurada.
- Pensao continua **farol quente** no fundo.
- Quadrados podem reaparecer levemente (FogVolumes reativados) — aceitavel vs trilha sem neblina.

### Problemas conhecidos (K.3.4)

- Se quadrados voltarem demais: manter fog density e reduzir so FogVolume Mid para `0.038`.
- Se ainda invisivel: subir `fog_density` para `0.065` antes de qualquer outra mudanca.
- Player/ lanterna nao tocados — ajuste fino de volumetric da lanterna fica para sprint futura se quadrados persistirem.

## Sprint K.3.5 — Remocao Definitiva dos Blocos de Neblina

**Data:** 2026-07-10 | **Escopo:** `TrailIntro.tscn` + volumetric contribution da lanterna (`Player.tscn`).

### Causa dos blocos (K.3.4)

| Fonte | Problema |
|-------|----------|
| `TrailFogVolumeNear/Mid/Far` | Caixas volumetricas visiveis como **retangulos** na neblina |
| `TrailMistParticles_A/B/C/Ground` | Quads GPUParticles3D aparecendo como **placas** |
| Volumetric fog global | Froxel artifacts com luzes dinamicas |
| Lanterna volumetric contribution | Revelava grade ao iluminar fog volumetrica |

### Acao tomada

| Elemento | Status K.3.5 |
|----------|--------------|
| `TrailFogVolumeNear` | **Desativado** (`visible=false`, `process_mode=disabled`) |
| `TrailFogVolumeMid` | **Desativado** |
| `TrailFogVolumeFar` | **Desativado** |
| `TrailMistParticles_A` | **Desativado** (`visible=false`, `emitting=false`) |
| `TrailMistParticles_B` | **Desativado** |
| `TrailMistParticles_C` | **Desativado** |
| `TrailMistGroundLow` | **Desativado** |
| Volumetric fog (Environment) | **Desligada** (`volumetric_fog_enabled = false`) |
| Neblina | **Somente** depth fog + height fog do Environment |

### WorldEnvironment — valores finais K.3.5

| Parametro | Valor |
|-----------|-------|
| Background | `#071019` |
| Ambient color / energy | `#0E1620` / `0.20` |
| Fog enabled | `true` |
| Fog color | `#16222C` |
| Fog density | `0.055` |
| Fog depth begin / end | `8.0` / `42.0` |
| Fog height density | `0.035` |
| Volumetric fog | **`false`** |

Se neblina fraca: `fog_density = 0.065`. Se forte demais: `fog_density = 0.038`.

### Lanterna (`Player.tscn`)

| Parametro | K.3.4 | K.3.5 |
|-----------|-------|-------|
| `light_energy` | `2.75` | **`2.75`** (inalterado) |
| `light_volumetric_fog_energy` | `0.22` | **`0.0`** |

Lanterna continua iluminando chao/objetos; nao interage com fog volumetrica.

### Iluminacao TrailIntro (volumetric zerada nas luzes)

| Luz | Energy | Volumetric contribution |
|-----|--------|-------------------------|
| MoonLight | `0.60` | **`0.0`** |
| HouseEntranceWarmLight | `4.0` / range `12` | **`0.0`** |
| TrailSilhouetteFill | `0.22` | **`0.0`** |

### Resultado esperado

- **Sem quadrados/retangulos** grandes na neblina.
- Neblina **sutil mas visivel** via depth + height fog.
- Pensao continua **farol quente**; trilha **jogavel**.

### Problemas conhecidos (K.3.5)

- Depth/height fog sozinha pode parecer menos densa que volumetric — ajustar `fog_density` antes de reintroduzir qualquer volume/particula.
- Particulas futuras precisam usar textura soft circular ou shader custom — nunca quads crus visiveis.
- Reativar FogVolumes **proibido** sem solucao alternativa (ex.: noise 3D + shader).

## Sprint K.3.6 — Fog Cards Cinematograficos sem Blocos

**Data:** 2026-07-10 | **Escopo:** solucao definitiva de neblina na TrailIntro.

### Problema resolvido

Neblina instavel entre sprints K.3.x:
- FogVolumes = retangulos;
- GPUParticles = placas;
- Volumetric fog forte = grade com lanterna;
- remover tudo = neblina some.

### Solucao: Fog Cards + Environment base

| Camada | Funcao |
|--------|--------|
| **Environment depth/height fog** | Base sutil de profundidade (`density 0.025`) |
| **Fog Cards** (`FogCard3D.tscn`) | Neblina principal — manchas suaves com shader radial |
| FogVolumes | **PROIBIDOS** (`DISABLED_DO_NOT_USE_BLOCKY_FOG`) |
| GPUParticles antigas | **PROIBIDAS** |
| Volumetric fog | **Desligada** |

### Arquivos criados

| Arquivo | Funcao |
|---------|--------|
| `shaders/fx/fog_card.gdshader` | Shader unshaded, alpha radial + noise procedural |
| `scenes/fx/FogCard3D.tscn` | Cena reutilizavel (QuadMesh + ShaderMaterial) |
| `scripts/fx/FogCard3D.cs` | Billboard eixo Y + parametros exportados por instancia |

### Shader — parametros padrao

| Parametro | Valor |
|-----------|-------|
| `fog_color` | `#16222C` |
| `alpha` | `0.08` |
| `softness` | `1.4` |
| `noise_strength` | `0.25` |
| `noise_scale` | `3.0` |

### Fog Cards na TrailIntro (`FogCards/`)

| Card | Alpha | Tamanho | Posicao (aprox.) |
|------|-------|---------|------------------|
| `FogCard_Near_01` | `0.04` | `5×2` | inicio trilha (z~5.5) |
| `FogCard_Mid_01` | `0.065` | `7×3` | meio esquerda |
| `FogCard_Mid_02` | `0.06` | `7×3` | meio direita |
| `FogCard_Far_01` | `0.08` | `9×4` | fundo esquerda |
| `FogCard_Far_02` | `0.075` | `9×4` | fundo direita |
| `FogCard_House_01` | `0.09` | `8×3.5` | perto da Pensao |

Billboard eixo Y via `FogCard3D.cs` — card nao mostra lateral como placa fina.

### Environment base (K.3.6)

| Parametro | Valor |
|-----------|-------|
| Fog density | `0.025` |
| Depth begin / end | `14.0` / `55.0` |
| Height fog density | `0.018` |
| Volumetric fog | **`false`** |
| Ambient energy | `0.20` |

### Lanterna

`light_volumetric_fog_energy = 0.0` (K.3.5) — mantido. Cards sao unshaded; nao reagem a luz forte.

### Como ajustar

- Neblina fraca: subir **alpha dos cards** (nao reativar FogVolume).
- Card quadrado: subir `softness`, reduzir `alpha`, mover card para longe.
- Debug: desligar cards individualmente no grupo `FogCards` no editor.

### Problemas conhecidos (K.3.6)

- Cards com `depth_draw_never` desenham por cima — aceitavel para neblina atmosferica; revisar se houver sorting estranho com objetos proximos.
- `_Process` billboard por card tem custo leve (6 cards = ok).
- Futuro: textura noise baked pode substituir hash procedural se quiser mais detalhe.

## Sprint K.3.7 — Fog Cards com Textura PNG Estavel

**Data:** 2026-07-10 | **Escopo:** solucao definitiva simples — `Sprite3D` + PNG na TrailIntro.

### Problema resolvido

Ciclo de erro K.3.x: FogVolumes/particles/volumetric/shader `.tscn` geravam blocos ou quebravam a cena.

### Solucao K.3.7

| Camada | Status |
|--------|--------|
| FogVolumes / GPUParticles antigas | **Proibidos** (`DISABLED_DO_NOT_USE_BLOCKY_FOG`) |
| Volumetric fog | **Desligada** |
| Environment depth/height fog | Base **muito sutil** |
| Neblina principal | **4× Sprite3D** com PNG soft |

### Texturas usadas

| Card | Textura | Alpha |
|------|---------|-------|
| `FogCard_Mid_01` | `fog_soft_card_01.png` | `0.18` |
| `FogCard_Mid_02` | `fog_soft_card_02.png` | `0.12` |
| `FogCard_Far_01` | `fog_soft_card_03.png` | `0.20` |
| `FogCard_House_01` | `fog_soft_card_04.png` | `0.16` |

Path base: `res://assets/textures/fx/fog/`

### Config Sprite3D

- `billboard = 2` (Fixed Y)
- `shaded = false`, `alpha_cut = 0`
- `cast_shadow = off`, `gi_mode = disabled`
- `modulate` azul-cinza frio `#AFC4D6` + alpha
- Escala via `transform` + `pixel_size`

### Posicoes aproximadas (Z spawn = 14)

| Card | Posicao (X, Y, Z) | Escala transform |
|------|-------------------|------------------|
| `FogCard_Mid_01` | `(0, 1.15, -2)` | `11 × 3.5` |
| `FogCard_Mid_02` | `(-3.6, 1.1, 0.5)` | `7.5 × 3` |
| `FogCard_Far_01` | `(-1.5, 1.25, -10)` | `13 × 4.5` |
| `FogCard_House_01` | `(0.8, 1.35, -14.5)` | `10 × 3.2` |

### Environment base (K.3.7)

| Parametro | Valor |
|-----------|-------|
| Fog color | `#16222C` |
| Fog density | `0.022` |
| Depth begin / end | `16.0` / `60.0` |
| Height fog density | `0.014` |
| Volumetric fog | **`false`** |

### Lanterna

`light_volumetric_fog_energy = 0.0` — mantido. PNGs com borda transparente; sem grade volumetrica.

### Como ajustar

- Neblina fraca: subir `modulate.a` dos Sprite3D (nao reativar FogVolume).
- Card quadrado: trocar PNG, reduzir alpha, aumentar escala, mover para longe.
- Cena quebrada: validar com Godot headless antes de commit.

### Problemas conhecidos (K.3.7)

- `FogCard3D.tscn` / shader K.3.6 permanecem no repo mas **nao sao usados** na TrailIntro.
- Sorting transparente pode sobrepor objetos muito proximos — aceitavel para neblina atmosferica.

## Sprint K.3.8 — Refinamento da Neblina da TrailIntro

**Data:** 2026-07-10 | **Escopo:** reorganizar fog cards + drift leve; eliminar efeito de faixa.

### Problemas K.3.7 resolvidos

| Problema | Causa | Solucao K.3.8 |
|----------|-------|---------------|
| Faixa/parede ao andar | 4 cards alinhados em Z similar (`-2`, `0.5`, `-10`) | **9 cards** em 3 zonas, Z espalhado `6.5` a `-13.2` |
| Neblina estatica | Sem animacao | Script `TrailFogCardDrift.cs` em cada card |

### Estrutura (`FogCards/`)

| Zona | Cards | Alpha medio | Z aprox. |
|------|-------|-------------|----------|
| **NearFog** (3) | Near_01, Near_02, Near_03 | `0.05–0.06` | `6.5` a `10` |
| **MidFog** (3) | Mid_01, Mid_02, Mid_03 | `0.10–0.12` | `-3.5` a `1.5` |
| **FarFog** (3) | Far_01, Far_02, Far_03 | `0.12–0.14` | `-7.5` a `-13.2` |

**Total: 9 fog cards.** Texturas: `fog_soft_card_01–04.png` alternadas.

### Anti-faixa (transicao gradual)

- Near com alpha **muito baixo** (`0.05–0.06`), cards **grandes** e **afastados** do spawn (Z ≥ 6.5).
- Mid em **laterais** (X `-4.2` / `4.0`) e **alturas variadas** (Y `1.08–1.38`).
- Far antes da pensao sem cobrir fachada (Far_03 alpha `0.12`, Y `1.45`).
- **Nenhum card** no eixo central unico em Z=-2 (causa da faixa K.3.7).

### Movimento — `TrailFogCardDrift.cs`

| Parametro | Faixa tipica |
|-----------|--------------|
| `DriftAmplitude` | `0.30–0.50` m |
| `DriftSpeed` | `0.09–0.14` |
| `VerticalAmplitude` | `0.035–0.06` m |
| `VerticalSpeed` | `0.12–0.19` |
| `PhaseOffset` | unico por card (`0` a `6.8`) |

Movimento senoidal leve em X/Z + oscilacao vertical quase imperceptivel. **Sem shader.**

### Limitacoes restantes

- Drift usa `_Process` por card (9 nos — custo baixo).
- Se faixa persistir: reduzir alpha Near ou afastar Z dos cards near para `12+`.
- Pensao deve permanecer visivel — nao subir alpha Far_03 acima de `0.14`.

## Sprint K.3.9 — Neblina Cinematografica Screen-Space

**Data:** 2026-07-10 | **Escopo:** trocar neblina principal para overlay 2D animado; desativar fog cards no caminho.

### Motivo da mudanca

Fog cards 3D (Near/Mid/Far no caminho) apareciam como **planos/layers** quando o player se movia — faixas visiveis, imersao quebrada.

### Solucao K.3.9

| Camada | Funcao |
|--------|--------|
| **Environment fog** | Base sutil de profundidade (`density 0.020`, depth `18–70`) |
| **ScreenFogOverlay** | Neblina **principal** — shader canvas_item animado |
| **Fog cards 3D** | **Desativados** no caminho; 1 card distante opcional |

### Fog cards desativados

| Grupo/Card | Status |
|------------|--------|
| `NearFog` (3 cards) | **desativado** |
| `MidFog` (3 cards) | **desativado** |
| `FogCard_Far_01`, `Far_02` | **desativado** (player atravessava) |
| `FogCard_Far_03` | **ativo** — z `-15.5`, alpha `0.10`, perto da pensao |

### Screen-space overlay

| No | Config |
|----|--------|
| `AtmosphereOverlay` | `CanvasLayer` **layer `-10`** (atras do HUD layer `0`) |
| `ScreenFogOverlay` | `ColorRect` full-screen + `screen_fog_overlay.gdshader` |

### Shader — parametros finais

| Uniform | Valor |
|---------|-------|
| `fog_color` | `#16222C` |
| `alpha` | `0.16` |
| `noise_scale` | `2.8` |
| `noise_strength` | `0.45` |
| `drift_speed_x` | `0.018` |
| `drift_speed_y` | `0.004` |
| `horizon_start` / `horizon_end` | `0.20` / `0.78` |
| `bottom_fade` | `0.88` |

Animacao via `TIME` + FBM noise — movimento lento, organico, sem linhas duras.

### Environment base (K.3.9)

| Parametro | Valor |
|-----------|-------|
| Fog density | `0.020` |
| Depth begin / end | `18.0` / `70.0` |
| Height fog density | `0.012` |
| Volumetric fog | **`false`** |

### HUD

Overlay em layer `-10`; HUD default layer `0` — **texto legivel**.

### Ajuste fino

| Situacao | Acao |
|----------|------|
| Forte demais | `alpha` `0.10–0.13` |
| Fraca demais | `alpha` `0.18–0.22` |
| Parada | `drift_speed_x` `0.025` |
| Rapida | `drift_speed_x` `0.010` |
| Cobre chao | subir `bottom_fade` |
| Linha no card distante | desativar `FogCard_Far_03` |

## Sprint K.3.10 — Diagnostico e Calibracao Definitiva da Neblina

**Data:** 2026-07-10 | **Escopo:** provar overlay screen-space + corrigir shader + calibrar.

### Diagnostico — por que a neblina sumiu (K.3.9)

| Problema | Causa |
|----------|-------|
| Overlay “invisivel” | Mascara `bottom_fade` **invertida** — neblina so nos 12% inferiores da tela |
| Shader nao compilava | `return` no `fragment()` (invalido em canvas_item Godot 4.7) |

### DebugVisible — como testar

No `ShaderMaterial_screen_fog` da TrailIntro:

1. Setar `debug_visible = true` → tela deve ficar **azul** (`alpha 0.45`) em todo o gameplay.
2. Confirmar overlay renderiza → setar `debug_visible = false`.

**Estado atual:** `debug_visible = false` (modo normal calibrado).

### Configuracao final do overlay (K.3.10)

| Parametro | Valor |
|-----------|-------|
| `debug_visible` | **`false`** |
| `fog_color` | `#16222C` |
| `alpha` | **`0.18`** |
| `noise_scale` | `2.4` |
| `noise_strength` | `0.40` |
| `drift_speed_x` / `y` | `0.018` / `0.003` |
| `horizon_start` / `end` | `0.22` / `0.82` |
| `bottom_fade` | `0.88` |
| `top_fade` | **`0.08`** |

### Environment base

| Parametro | Valor |
|-----------|-------|
| Fog density | `0.018` |
| Depth begin / end | `18.0` / `70.0` |
| Height fog density | `0.012` |
| Volumetric fog | **`false`** |

### Fog cards 3D

**Todos desativados** (incl. `FogCard_Far_03`). Neblina principal = **somente** `ScreenFogOverlay`.

### HUD

`AtmosphereOverlay` layer **`-10`**, HUD layer **`0`** — HUD legivel.

### Ajuste fino pos-playtest

| Situacao | Acao |
|----------|------|
| Ainda fraca | `alpha` `0.22` → `0.26` |
| Filtro demais | `alpha` `0.14`, `noise_strength` `0.28` |
| Linha horizontal | suavizar `horizon_start/end` |
| Chao coberto | `bottom_fade` `0.78` |
| Ceu coberto | `top_fade` `0.16` |

## Sprint K.3.11 — Depth-Based Fog Real da TrailIntro

**Data:** 2026-07-10 | **Escopo:** neblina por profundidade da camera; abandona screen-space como solucao principal.

### Por que mudar

| Abordagem | Problema |
|-----------|----------|
| FogVolume / particles / fog cards | Quadrados, placas, faixas no caminho |
| Volumetric Fog | Froxels/quadrados visiveis |
| ScreenFogOverlay simples | Filtro 2D sem profundidade — nao parece neblina no espaco |

### Solucao

| Componente | Caminho |
|------------|---------|
| Shader | `shaders/fx/depth_fog_postprocess.gdshader` |
| Quad fullscreen | `Visual/PostProcess/DepthFogPostProcess` → reparent para `Player/CameraPivot/Camera3D` |
| Script | `scripts/fx/DepthFogPostProcess.cs` |
| Acabamento sutil | `ScreenFogOverlay` `alpha = 0.06` (opcional; desligar se parecer filtro) |

### Debug depth (obrigatorio uma vez)

1. Material `ShaderMaterial_depth_fog` → `debug_depth = true` → F6.
2. Tela deve mostrar **escala de cinza**: perto escuro, longe claro.
3. Voltar `debug_depth = false`.

**Comprovado:** contraste near/far `0.99` em captura real.

### Valores finais (K.3.11)

| Parametro | Valor |
|-----------|-------|
| `debug_depth` | **`false`** |
| `fog_color` | `#2A3A46` |
| `fog_near` | `6.0` |
| `fog_far` | `48.0` |
| `fog_strength` | `0.70` |
| `fog_power` | `1.35` |
| `noise_strength` | `0.10` |
| `noise_scale` | `2.0` |
| `drift_speed` | `0.012` |
| `horizon_bias` | `0.18` |

### ScreenFogOverlay (acabamento)

| Parametro | Valor |
|-----------|-------|
| `alpha` | `0.06` |
| Papel | acabamento atmosferico leve; **nao** e a neblina principal |

### Environment base (sutil)

| Parametro | Valor |
|-----------|-------|
| Fog density | `0.012` |
| Depth begin / end | `20.0` / `80.0` |
| Volumetric fog | **`false`** |

### Sistemas proibidos (permanecem off)

FogVolumes, GPUParticles antigas, fog cards no caminho, volumetric fog forte.

### Calibracao rapida

| Situacao | Acao |
|----------|------|
| Fraca | `fog_strength 0.85`, `fog_near 4.0`, `fog_far 42.0`, `fog_color #304351` |
| Forte demais | `fog_strength 0.50`, `fog_near 10.0`, `fog_far 60.0`, `fog_color #263541` |
| Parece filtro | reduzir `fog_strength`, aumentar `fog_near`, reduzir `noise_strength` |
| Esconde pensao | aumentar `fog_far`, reduzir `fog_strength` |
| Fundo sem atmosfera | reduzir `fog_far`, aumentar `fog_strength` |

## Sprint K.3.12 — Calibracao Definitiva do Depth Fog

**Data:** 2026-07-10 | **Hotfix:** fog exponencial + mascara horizonte + sky_fog minimo.

### Problema K.3.11

| Sintoma | Causa |
|---------|-------|
| Ceu lavado/azulado | Depth infinito recebia fog como objeto distante + `horizon_bias` |
| Caminho limpo demais | `fog_near/fog_far` longe + `smoothstep` suave demais |
| Neblina so na pensao | Fog comecava tarde demais |

### Shader (logica K.3.12)

- Fog **exponencial**: `1 - exp(-(dist - fog_start) * fog_density)`
- Ceu (`depth >= 0.9999`): `sky_fog` minimo (0.0 final)
- Mascara gaussiana no horizonte (`horizon_y`, `horizon_width`)
- `top_fade` / `bottom_fade` evitam chapado no ceu/chao

### Valores finais

| Parametro | Valor |
|-----------|-------|
| `debug_depth` | **`false`** |
| `fog_color` | `#384D5B` |
| `fog_start` | `2.5` |
| `fog_density` | `0.095` |
| `fog_strength` | `0.88` |
| `fog_power` | `0.72` |
| `sky_fog` | **`0.0`** |
| `horizon_y / width` | `0.50` / `0.42` |
| `noise_scale / strength` | `4.0` / `0.08` |
| `drift_speed` | `0.018` |

### ScreenFogOverlay

**Desativado** (`visible = false`) — depth fog e a unica neblina principal.

### Ajuste fino pos-playtest

| Situacao | Acao |
|----------|------|
| Caminho ainda limpo | `fog_start 1.5`, `fog_density 0.125`, `fog_strength 0.95`, `fog_power 0.65` |
| Ceu lavado | `sky_fog 0.015` → `0.0` |
| Filtro demais | `fog_strength 0.70`, `fog_density 0.075`, `fog_start 3.5` |

## Preset aprovado da TrailIntro (CONGELADO)

**Status:** aprovado em playtest | **Regra:** nao alterar depth fog sem motivo forte.

### Depth Fog — valores congelados

| Parametro | Valor | Mexer? |
|-----------|-------|--------|
| `fog_color` | `#384D5B` | **NAO** |
| `fog_start` | `2.5` | **NAO** |
| `fog_density` | `0.095` | **NAO** |
| `fog_strength` | `0.88` | **NAO** |
| `fog_power` | `0.72` | **NAO** |
| `horizon_y / width` | `0.50` / `0.42` | **NAO** |
| `noise_scale / strength` | `4.0` / `0.08` | **NAO** |
| `drift_speed` | `0.018` | **NAO** |
| `sky_fog` | **`0.0`** (max `0.005` se necessario) | so ceu |

### Ceu escuro (separado da neblina do caminho)

| Parametro | Valor |
|-----------|-------|
| `sky_fog` | **`0.0`** — ceu infinito nao recebe `fog_color` |
| Background color | `#02060A` |
| Ambient color | `#071019` |
| Ambient energy | `0.18` |

### Sistemas proibidos

FogVolumes, particles antigas, fog cards no caminho, volumetric fog forte, ScreenFogOverlay (desativado).

### Criterio de aceite (referencia)

- Neblina do caminho igual ao print aprovado
- Ceu escuro, clima de terror
- Pensao visivel e misteriosa
- Sem quadrados, faixas ou impacto no gameplay
