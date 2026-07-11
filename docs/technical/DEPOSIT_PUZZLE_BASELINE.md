# BREU — Puzzle do Depósito (Baseline)

**Versão:** 1.0  
**Data:** 2026-07-11  
**Sprint:** 07 — aprovada e congelada  
**Status:** OFICIAL — não alterar puzzle sem nova sprint

---

## Regra de congelamento

> **Não modificar** fluxo, estado, mensagens ou posições do puzzle **sem solicitação explícita do usuário** ou sprint dedicada.

**Sem inventário global** — flags locais em `PensaoPuzzleState` apenas.

Baselines paralelas: Player, HUD, Interação, Térreo — ver `docs/technical/*_BASELINE.md`.

---

## Cena e scripts

| Item | Caminho |
|------|---------|
| Cena | `scenes/levels/pensao_santa_luzia/PensaoTerreoBlockout01.tscn` |
| Estado | `scripts/levels/pensao_santa_luzia/PensaoPuzzleState.cs` |
| Setup | `scripts/levels/pensao_santa_luzia/PensaoTerreoPuzzleSetup.cs` |
| Porta | `DepositDoorInteraction.cs` |
| Chave | `PickupKeyInteraction.cs` |
| Fusível | `PickupFuseInteraction.cs` |
| Playtest | `docs/testing/PENSAO_DEPOSIT_PUZZLE_PLAYTEST.md` |

---

## Estado local (`PensaoPuzzleState`)

| Flag | Default | Quando `true` |
|------|---------|---------------|
| `HasDepositKey` | `false` | Player pega chave no quarto 102 |
| `IsDepositUnlocked` | `false` | Player usa chave na porta |
| `HasOldFuse` | `false` | Player pega fusível no depósito |

Nó na cena: `PuzzleState` (filho de `PensaoTerreoBlockout01`).

---

## Fluxo aprovado

```
Trilha → Pensão → Quarto 102 → Chave → Depósito (destrancar) → Fusível (+ bilhete)
```

---

## Objetos e localização

| Nó | Posição (aprox.) | Tipo |
|----|------------------|------|
| `Key_Deposit_Old` | (-5, 0.75, -14.2) — quarto 102 | Area3D + pickup |
| `Door_Deposit_Blocked` | (0, ~1.1, -26.5) — fim corredor | StaticBody3D stateful |
| `Fuse_Old` | (0, 0.55, -30) — interior depósito | Area3D + pickup |
| `DepositNote` | (0.65, 1.05, -29.6) — interior depósito | Area3D + Interactable OneShot |

---

## Porta do depósito (`DepositDoorInteraction`)

| Estado | Prompt | Ação / mensagem |
|--------|--------|-----------------|
| Sem chave | `[E] Tentar abrir depósito` | "Está trancado. Preciso encontrar uma chave." |
| Com chave | `[E] Usar chave velha` | "A porta do depósito destrancou." + colisão off |
| Destrancada | _(vazio)_ | Porta invisível; player entra |

**Destravamento:** `Door_Deposit_Blocked.Visible = false`; `Door_Deposit_Blocking_Collision.Disabled = true`.

---

## Mensagens HUD

| Evento | Texto |
|--------|-------|
| Tentar abrir sem chave | Está trancado. Preciso encontrar uma chave. |
| Pegar chave | Chave velha adquirida. |
| Usar chave | A porta do depósito destrancou. |
| Pegar fusível | Fusível velho adquirido. |
| Ler bilhete | Não deixem os novos funcionários sozinhos depois das 22h. |

---

## Colisão / geometria relacionada

- Área futura escada bloqueada: `Wall_StairFuture_Blocker` (hotfix Sprint 07).
- Puzzle **não** altera colisão do térreo fora do depósito.

---

## Limitações futuras (fora Sprint 07)

| Item | Sprint sugerida |
|------|-----------------|
| Inventário visual / UI | Futura |
| Uso do fusível em puzzle elétrico | 09+ |
| Escada / 2º andar | 08–09 |
| Escada acessível na área bloqueada | 08+ |
| Inimigo / combate | 13–14 |

**Regra:** não criar inventário complexo nem expandir puzzle sem sprint dedicada.

---

## Commits de referência

| Commit | Descrição |
|--------|-----------|
| `d13f22f` | feat: add simple deposit key puzzle |
| `652497c` | fix: block unintended access near deposit puzzle area |
| _(Sprint 07)_ | feat: approve simple deposit key puzzle baseline |
