# AkashaRecords — Ce qui change entre v4 et le refactor

Carte de correspondance entre l'ancien repo (v4, archivé) et le nouveau (refactor). Sert de **pont** entre les deux dépôts, à défaut d'historique git continu. Le *pourquoi* détaillé de chaque choix est dans `akasharecords-architecture.md`.

## Ce qui NE change pas

- **La base PostgreSQL** — même instance, même schéma v4, même chaîne Npgsql. Elle ne bouge pas.
- **Le domaine métier** — personnages / builds / recommandations. Le modèle v4 est déjà sain (séparation données de jeu / connaissance de build déjà faite).
- **Le périmètre données** — officiel et sorti uniquement (pas de personnages annoncés, pas de stats de bêta).

## Carte de correspondance (ancien → nouveau)

| Élément | v4 (ancien repo) | Nouveau repo | Statut |
|---|---|---|---|
| Structure solution | 1 projet MVC | Core + Infrastructure + Web + DbMigrator + tests | Réécrit |
| Entités | dans le projet MVC | `Core/Entities` | Porté |
| Contrats | — | `Core/Contracts` (Repositories, Transactions) | Nouveau |
| Accès données | requêtes ad hoc dans le MVC | `Infrastructure/Repositories` + Dapper, SQL dans `Data/Sql` | Réécrit |
| Transactions | — | `UnitOfWork` (propriétaire de la transaction) | Nouveau |
| Migrations DB | modifs manuelles | DbUp — `Script0001_baseline` = photo du schéma v4 | Nouveau |
| Gestion erreurs | `try/catch` dispersés | `ExceptionHandlingMiddleware` + exceptions `Core` | Nouveau |
| Présentation | entités passées aux vues | ViewModels / DTO de lecture / InputModels | Réécrit |
| Helpers | `ImageHelper`, `CharacterHelper` | `Web/Helpers` (factorisés, culture explicite) | Porté + nettoyé |
| Tests | absents / minimes | `UnitTests` + `IntegrationTests` (WebApplicationFactory + Testcontainers) | Nouveau |
| Schéma DB | tables v4 | identique — seule décision ouverte : enum `slot`/`stat` | Quasi inchangé |
| Données | PostgreSQL | même PostgreSQL | Inchangé |

## Repos & git

- **Ancien repo** — archivé, tag `v4-legacy` ; son historique de commits reste intact et consultable.
- **Nouveau repo** — structure propre dès le premier commit, jamais d'état à moitié cassé.
- **Lien** — le README du nouveau repo pointe vers l'ancien et renvoie à ce fichier + à `akasharecords-architecture.md`.
- **Ce qu'on ne garde pas** — seulement un `git blame` / `bisect` continu *à travers* la frontière du refactor, qu'on ne veut de toute façon pas franchir. Le retraçage de l'évolution reste évident : commits v4 dans le legacy + cette carte, plus lisible qu'un pivot noyé dans un log unique.

## Règle de portage

On ne recopie un fichier que s'il **mérite** sa place. Un fichier qu'on ne comprend plus ou dont on ne se sert pas ne migre pas — il reste dans `v4-legacy`.

## Résumé

Le refactor ne re-modélise pas : il **durcit**. La v4 avait déjà la bonne séparation métier ; le nouveau repo ajoute les couches, les transactions, la gestion d'erreurs, les tests et les migrations autour de ce noyau déjà sain.
