# BREU — Sistema de áudio

Ultima atualizacao: 2026-07-09

## Visao geral

O audio da demo usa `AudioManager` como ponto central para UI, 2D e 3D one-shot.  
Streams sao **opcionais** — caminhos em `scripts/audio/AudioPaths.cs` e lista em `assets/audio/AUDIO_ASSETS_NEEDED.md`.

## Instanciacao atual

Na `DemoRoom.tscn`:

```text
DemoRoom/UI/AudioManager
```

Grupo: `audio_manager`

Cena reutilizavel: `res://autoload/AudioManager.tscn`

## Autoload (opcional, futuro)

Para tornar global em todas as cenas:

1. Abrir **Projeto → Configurações do Projeto → Autoload**.
2. Adicionar `res://autoload/AudioManager.tscn` como `AudioManager`.
3. Remover instancia duplicada da DemoRoom se desejado.

## API

| Metodo | Uso |
|--------|-----|
| `PlayUiSound(stream)` | Cliques de UI, bilhete |
| `Play2DSound(stream)` | SFX 2D gerais |
| `Play3DSound(stream, position)` | Porta, susto, inimigo |
| `Play3DSoundAtPath(path, position)` | Carrega `.ogg` se existir |

Streams `null` ou arquivos ausentes → `Debug.Print("AudioManager: stream nao configurado.")` sem erro.

## Carregamento seguro

`AudioResourceLoader.TryLoad(path)` usa `ResourceLoader.Exists` antes de carregar.

## Integracoes

- `DoorAudioController` — carrega paths de `AudioPaths` e toca em 3D.
- `RadioInterferenceController` — static/whisper via paths.
- `CorridorScareTrigger` — `scare_stinger_01.ogg` via AudioManager.
- `HammerPickup` — `pickup_item.ogg` via AudioManager (2D) quando existir.

## Proximos passos

- Ambience loops no quarto/corredor.
- Autoload global quando houver multiplas cenas.
- Mixer/bus de volume (Master, SFX, UI, Ambience).
