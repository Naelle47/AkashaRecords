# AkashaRecords

Base de connaissances de builds pour **Genshin Impact** — un projet personnel qui organise la connaissance communautaire (guides de build : armes, artefacts et statistiques conseillés) séparément des données du jeu.

![License](https://img.shields.io/badge/license-MIT-green)
![.NET](https://img.shields.io/badge/.NET-512BD4?logo=dotnet&logoColor=white)
![PostgreSQL](https://img.shields.io/badge/PostgreSQL-4169E1?logo=postgresql&logoColor=white)
![Dapper](https://img.shields.io/badge/Dapper-lightgrey)

## À propos

AkashaRecords catalogue les personnages, armes et artefacts de Genshin Impact, et surtout leurs **guides de build** : pour un personnage donné, les armes, sets d'artefacts et statistiques recommandés selon son rôle.

Le projet est la **refonte** d'une première version (wiki). L'objectif n'est pas de tout réécrire, mais de repartir sur une base propre : séparation nette entre *données de jeu* et *connaissance de build*, architecture en couches, migrations et tests.

> Version précédente archivée : **AkashaRecords v4-legacy** — _(lien à ajouter)_

## Stack technique

- **ASP.NET MVC** (.NET) — application web
- **Dapper + Npgsql** — accès données
- **PostgreSQL** — base de données
- **DbUp** — migrations SQL versionnées
- **xUnit + Testcontainers** — tests unitaires et d'intégration

## Architecture

Découpage en couches, dépendances orientées vers le cœur :

```
Web → Infrastructure → Core
```

- **Core** — modèle métier (entités, contrats), aucune dépendance
- **Infrastructure** — implémentations Dapper, Unit of Work
- **Web** — MVC (controllers, vues, ViewModels)
- **DbMigrator** — migrations DbUp (autonome)
- **tests** — unitaires + intégration

Détails de conception dans [`docs/`](docs/).

## Structure du dépôt

```
src/
  AkashaRecords.Core/
  AkashaRecords.Infrastructure/
  AkashaRecords.Web/
db/
  AkashaRecords.DbMigrator/
tests/
  AkashaRecords.UnitTests/
  AkashaRecords.IntegrationTests/
docs/              modèle de domaine, UML, notes d'architecture
```

## Statut

🚧 En cours — refonte active.

## Crédits & mentions

[Genshin Impact](https://genshin.hoyoverse.com/) est un jeu développé et édité par HoYoverse (COGNOSPHERE PTE. LTD.).

Ce projet est un projet personnel **non-officiel**, sans affiliation avec HoYoverse ni approbation de sa part. Les noms, données, images et marques du jeu appartiennent à HoYoverse. Seul le **code source** de ce dépôt est sous licence [MIT](LICENSE.txt).
