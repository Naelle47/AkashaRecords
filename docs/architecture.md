# AkashaRecords — Architecture de la solution (refactor)

Consolidation des décisions de structure prises pour la reprise du refactor. Point de départ : le schéma **v4**. Objectif : reprendre proprement sur une base saine, pas refondre. Stack : ASP.NET MVC (.NET 10) + Dapper + Npgsql + PostgreSQL.

## Structure de la solution

```
AkashaRecords.sln
│
├─ 00_Documentation                 notes d'archi, modèle de domaine, UML
│
├─ 01_Core                          AkashaRecords.Core — le cœur, aucune dépendance
│  ├─ Entities/                     Character, Weapon, Artifact, Build,
│  │                                BuildWeapon, BuildArtifact,
│  │                                BuildMainStat, BuildSubStat,
│  │                                Element, Region, WeaponType, CharacterRole
│  ├─ Enums/                        ArtifactSlot, Rarity…
│  ├─ Exceptions/                   NotFoundException, DomainException…
│  └─ Contracts/
│     ├─ Repositories/              ICharacterRepository, IBuildRepository…
│     └─ Transactions/              IUnitOfWork
│
├─ 02_Infrastructure                AkashaRecords.Infrastructure — Dapper/Npgsql, dépend de Core
│  ├─ Data/
│  │   ├─ NpgsqlConnectionFactory   (IDbConnectionFactory)
│  │   ├─ UnitOfWork
│  │   └─ Sql/                      CharacterQueries, BuildQueries… (SQL centralisé)
│  ├─ Repositories/                 CharacterRepository, BuildRepository…
│  │   └─ Queries/                  repositories de lecture → DTO
│  └─ DependencyInjection.cs        méthode d'extension d'enregistrement
│
├─ 03_Web                           AkashaRecords.Web — MVC, dépend de Core + Infrastructure
│  ├─ Controllers/
│  ├─ Views/
│  ├─ Models/                       ViewModels / DTOs
│  ├─ Helpers/                      ImageHelper, CharacterHelper (formatage présentation)
│  ├─ Middleware/                   ExceptionHandlingMiddleware
│  ├─ wwwroot/
│  ├─ appsettings.json              chaîne de connexion Npgsql
│  └─ Program.cs                    câblage DI + pipeline middleware
│
├─ 04_DbMigrator                    AkashaRecords.DbMigrator — console + DbUp, autonome
│  ├─ Scripts/                      Script0001_baseline.sql, Script0002_…
│  ├─ Program.cs
│  └─ appsettings.json
│
└─ 05_Tests
   ├─ AkashaRecords.UnitTests/         cœur + logique, repos mockés
   └─ AkashaRecords.IntegrationTests/  repos réels sur Postgres jetable
      ├─ Factory/                      CustomWebApplicationFactory
      └─ Fixtures/                     Testcontainers, seed, reset
```

Les dossiers `00_Documentation` à `05_Tests` sont des **dossiers de solution** (regroupement Visual Studio) qui donnent cet ordre logique dans l'explorateur ; physiquement, chaque projet vit dans son propre dossier. Les sous-dossiers internes (`Entities/`, `Contracts/`, `Data/Sql/`…) décrivent la structure cible de chaque projet.

## Rôle de chaque projet

**Core** — le modèle métier : entités, enums, exceptions métier, et les contrats (interfaces de repository, `IUnitOfWork`). Aucune dépendance ; ne connaît ni le web ni PostgreSQL.

**Infrastructure** — la mécanique concrète : implémentations Dapper des repositories, fabrique de connexions Npgsql, Unit of Work. Dépend de Core.

**Web** — la présentation MVC : controllers, vues, ViewModels, middleware, et le câblage DI dans `Program.cs`. Dépend de Core + Infrastructure.

**DbMigrator** — console autonome qui applique les scripts SQL versionnés via DbUp. Ne dépend d'aucune autre couche.

**Tests** — `UnitTests` (briques isolées, dépendances mockées) et `IntegrationTests` (repos et endpoints réels contre un Postgres éphémère).

## Règle des dépendances

```
Web ──▶ Infrastructure ──▶ Core
  └───────────────────────▶ Core
DbMigrator            (autonome : DbUp + SQL)
Tests ──▶ Core, Infrastructure, Web (selon le projet testé)
```

Tout pointe vers Core ; Core ne dépend de personne.

## Décisions actées

**Nommage `Core`** plutôt que `Domain` — terme plus abordable, et assez large pour absorber pour l'instant les quelques services applicatifs sans couche `Application` séparée.

**Migrations via DbUp** (`dbup-postgresql`) — scripts `.sql` numérotés, versionnés, rejouables. Reprise de la base v4 par un `Script0001_baseline.sql` idempotent (photo de l'existant) ; les vrais changements du refactor à partir du 0002.

**Unit of Work pour les transactions** — `IUnitOfWork` (Core) / `UnitOfWork` (Infrastructure) est **le propriétaire** de la connexion et de la transaction ; les repositories travaillent dessus mais n'ouvrent jamais leur propre transaction — sinon on perd tout l'intérêt de l'UoW. Une opération métier (enregistrer un `Build` + ses sous-tables) est un `Commit` ou un `Rollback`, jamais un état à moitié écrit.

**Gestion centralisée des erreurs** — exceptions métier dans `Core/Exceptions` + un `ExceptionHandlingMiddleware` monté en premier dans le pipeline, qui les traduit en réponse propre. Plus de `try/catch` dispersés.

**Tests** — xUnit + FluentAssertions + NSubstitute (mocks) ; intégration via `CustomWebApplicationFactory` (`WebApplicationFactory<Program>`) + Testcontainers (Postgres jetable). L'abstraction `IDbConnectionFactory` rend l'override de la base en test trivial : une seule ligne à réenregistrer. Nécessite `Microsoft.AspNetCore.Mvc.Testing` et `public partial class Program { }` en bas de `Program.cs`.

**Accès données** — Dapper + Npgsql, SQL écrit à la main (pas d'ORM).

**Deux natures de repository** (pour lever l'ambiguïté du terme) — un *repository métier* renvoie des **entités Core** : écritures et opérations de domaine, via l'UoW. Un *query repository* (ou une méthode de lecture dédiée) renvoie directement un **DTO de lecture** taillé pour la vue, en un seul SQL. Règle simple : on écrit par les entités, on lit par les DTO.

**Frontière de présentation** — une entité Core ne franchit jamais Razor. Deux cas en lecture, pour ne pas imposer un mapping partout : *lecture simple* → `SQL → DTO lecture → View` (le DTO va droit à la vue) ; *lecture avec logique de présentation* → `SQL → DTO lecture → mapping Web → ViewModel → View`. En écriture, le formulaire est bindé sur un `InputModel` (uniquement les champs éditables, contre l'over-posting), validé, puis mappé vers l'entité. Mapping manuel au début (pas d'AutoMapper tant que ça ne se répète pas). Les helpers de présentation (URLs d'images, formatage de dates/rareté) vivent dans `Web/Helpers/` et sont appelés au moment du mapping ; culture explicite sur les dates pour un rendu déterministe (et des tests stables).

## Flux d'une requête

Le verbe HTTP décide déjà read ou write, et ça descend jusqu'au SQL — toute la pile s'aligne :

```
GET (lecture)        → query repository → SQL SELECT (Query<T>) → DTO lecture → [ViewModel] → View
POST (mutation MVC)  → InputModel → validation (ModelState + règles simples)
                     → repository métier → SQL INSERT/UPDATE/DELETE (Execute) → Commit UoW → Redirect (PRG)
```

`[ViewModel]` est optionnel — voir « Frontière de présentation » : DTO direct pour une lecture simple, ViewModel seulement s'il y a de la logique de présentation.

Trois règles qui en découlent :

- **Un GET ne mute jamais** — safe et idempotent : pas d'`Execute` ni de transaction derrière un GET. Une suppression se fait en deux temps : `GET /Build/Delete/15` affiche une vue de confirmation, `POST /Build/Delete/15` exécute réellement la suppression puis redirige. Cohérent avec la protection CSRF, l'historique navigateur, le cache HTTP et les crawlers.
- **Une mutation POST se protège** — jeton *antiforgery* (CSRF) sur les formulaires, et pattern **PRG** (Post → Redirect → Get) : après une écriture réussie, on redirige vers un GET pour qu'un rafraîchissement ne renvoie pas le formulaire deux fois.
- **Validation avant écriture** — `if (!ModelState.IsValid) return View(model);`, puis les règles métier simples, avant d'appeler le repository et de committer. Valable même sans couche Application.

Nuance MVC : un formulaire HTML n'envoie que **GET et POST**. En pratique, GET pour l'affichage et **POST pour toutes les mutations** (création, édition, suppression). Les vrais `PUT` / `PATCH` / `DELETE` viendront avec le futur projet API REST.

Chaîne cohérente de bout en bout : `verbe HTTP → nature du repo → méthode Dapper → forme de données`.

Cette séparation lecture/écriture est un *CQRS léger* (sans le nommer formellement) : elle optimise naturellement la lecture, ce qui colle à AkashaRecords — dominé par des pages de consultation (personnages, builds, classements, comparaisons) plutôt que par des workflows d'écriture complexes.

## Reporté (pas maintenant)

**Couche `Application`** (`Services` / `UseCases` / `DTOs`) — reportée. Déclencheur pour l'introduire : le jour où de la logique s'intercale *entre* le controller et le repository — calcul d'Akasha Score, génération de classement, import API, comparaison de builds, règles de ranking, validation complexe. Tant que le controller appelle directement le repository, inutile.

Aussi reporté : projet `Api` (REST/JSON) à côté du MVC, découpage multi-projets plus fin.

## Invariants architecturaux

Les règles non négociables du projet. À relire dans six mois, quand une solution rapide se présente, pour trancher : « non, ça casse un invariant ».

- Core ne référence aucun autre projet.
- Infrastructure référence uniquement Core.
- Web est le seul point d'entrée utilisateur.
- Les controllers orchestrent ; ils ne portent pas la logique métier.
- Les entités Core ne franchissent jamais Razor.
- Les repositories métier manipulent des entités.
- Les query repositories manipulent des DTO de lecture.
- Les transactions appartiennent exclusivement à l'UnitOfWork.
- Les migrations restent dans DbMigrator.
- Les SQL restent dans Infrastructure.
- Un GET n'écrit jamais.
- Une écriture valide toujours avant de persister.
