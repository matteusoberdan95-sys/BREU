# Plano de Produção — Fases 1 e 2

Este documento organiza as sprints de produção para consolidar a Fase 1 (demo atual) e construir a Fase 2 (Sala dos Santos Secos e primeiro combate).

---

## Sprint A — Consolidar Quarto 07

**Status:** Em andamento / parcialmente concluído.

### Objetivos

- Player
- Lanterna
- Passos
- Pulo
- Martelo
- Bilhete
- Porta
- Corredor
- Susto
- HUD
- Áudio base

### Próximos ajustes

- Melhorar UI
- Sons reais
- Colisões finas
- Porta final
- Trigger de transição

---

## Sprint B — Trilha de Entrada

### Objetivos

- Criar blockout da trilha
- Vegetação seca
- Cerca
- Cactos
- Luz da casa ao longe
- Sons noturnos
- Chegada à fachada

### Assets Blender

- Chão de terra
- Cactos
- Pedras
- Cerca
- Galhos
- Silhueta da casa

### Godot

- Cena `TrailIntro.tscn`
- Trigger de chegada
- Ambience loop
- Luz da lanterna
- Som de vento

---

## Sprint C — Fachada da Casa

### Objetivos

- Criar exterior da Pensão Santa Luzia
- Porta de entrada
- Janelas
- Quintal
- Cruzes
- Lampião
- Conexão com Quarto 07

### Assets Blender

- Casa de barro
- Telhado gasto
- Porta
- Janela
- Cruzes
- Cerca
- Lampião
- Entulho

### Godot

- Cena `HouseExterior.tscn`
- Porta de entrada
- Transição para Quarto 07
- Trigger de som

---

## Sprint D — Sala dos Santos Secos

### Objetivos

- Criar sala ritualística
- Mesa com velas
- Ossos
- Cruzes
- Rede
- Símbolos
- Primeira arena de combate/fuga

### Assets Blender

- Mesa ritual
- Velas
- Pratos
- Ossos
- Rede
- Cruzes
- Ferramentas
- Parede de barro

### Godot

- Cena `RitualRoom.tscn`
- Luz de vela
- Trigger narrativo
- Inimigo placeholder
- Colisões

---

## Sprint E — Primeiro Inimigo Placeholder

### Objetivos

- Melhorar placeholder
- Perseguição simples
- Stun básico
- Som de respiração
- Som de passos
- Reação à lanterna no futuro

### Godot

- `EnemyPlaceholder.tscn`
- `EnemyAI.cs`
- `EnemyPerception.cs`
- `EnemyAudio.cs`

**Não usar Blender ainda.**

---

## Sprint F — Inimigo Final no Blender

Só iniciar após validar:

- Escala
- Distância
- Perseguição
- Timing do susto
- Comportamento básico

### Criar primeiro inimigo

**O Hóspede**

### Assets Blender

- Modelo low/mid poly
- Roupa rasgada
- Materiais sujos
- Rig simples

### Animações

- Idle
- Walk
- Chase
- Attack
- Hit reaction
- Death/stun (opcional)

---

## Ordem recomendada

```
Sprint A (consolidar) → Sprint B (trilha) → Sprint C (fachada)
                              ↓
                    Sprint D (sala ritual)
                              ↓
                    Sprint E (IA placeholder)
                              ↓
                    Sprint F (modelo Blender)
```

## Documentos relacionados

- [Visão Geral do Jogo](../design/GAME_VISION.md)
- [Fase 1 — Level Design](../design/PHASE_01_LEVEL_DESIGN.md)
- [Fase 2 — Level Design](../design/PHASE_02_LEVEL_DESIGN.md)
- [Design de Inimigos](../design/ENEMY_DESIGN.md)
- [Direção de Arte](../design/SCENARIO_ART_DIRECTION.md)
- [Histórico de Sprints](../SPRINT_HISTORY.md)
- [Próximas Tarefas](../gameplay/NEXT_SPRINT_TASKS.md)
