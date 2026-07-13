# AkashaRecords V2 — Modèle de domaine (notes)

Version textuelle de référence : structure du modèle + choix de conception clés et leur justification.

## Principe directeur

Séparer strictement deux mondes qui étaient mélangés en V1 :

- **Les données du jeu** — ce qui définit un personnage/objet (lore + gameplay).
- **La connaissance communautaire** — ce que les guides recommandent (builds).

C'est ce qui rend l'application extensible (brancher une API, une UI, un import externe sans casser le modèle).

## Structure (5 paquets)

- **0 · Référentiels communs** — `Rarity`, `StatType`, `ElementSource` (enums transverses).
- **1 · Personnage & référentiels** — `Character`, `CharacterElement`, `Element`, `Region`, `WeaponType`.
- **2 · Guide de build (agrégat)** — `BuildGuide`, `BuildRole`, `CombatRole`, `GameVersion`.
- **3 · Recommandations** — `WeaponRecommendation`, `ArtifactRecommendation`, `ArtifactSetRecommendation`, `MainStatRecommendation`, `SubStatRecommendation`, `RecommendationTier`.
- **4 · Objets & stats** — `Weapon`, `WeaponSecondaryStat`, `ArtifactSet`, `ArtifactSlotType`, `Stat`.

## Choix de conception clés

**Personnage ≠ guide de build.** `Character` (entité lore/gameplay) est séparé de `BuildGuide` (unité de connaissance). Un personnage a plusieurs guides (DPS, support, bloom…). C'est la décision structurante du modèle.

**`CharacterElement` plutôt qu'un champ `element`.** L'élément est une propriété *gameplay*, pas lore. Le Voyageur (plusieurs éléments, pas de Vision) et les Archons deviennent des données normales : plusieurs lignes `CharacterElement` avec `source = RESONANCE`, au lieu d'un `if (nom == "Traveler")` codé en dur. `estPrincipal` marque l'élément principal.

**`WeaponType` obligatoire (`1`), `Region` optionnelle (`0..1`).** Le type d'arme est une caractérisation gameplay que tout personnage jouable possède. La région est du lore, absente pour le Voyageur ou les persos non encore attribués.

**`BuildRole` = classe d'association, pas un simple enum.** Une cardinalité entre une classe et un enum est en réalité une dépendance. Passer par `BuildRole { role, priorite }` permet le multi-rôle avec ordre : Furina SUB_DPS(1) + SUPPORT(2), Zhongli SHIELD(1) + SUPPORT(2).

**`ArtifactSetRecommendation` = le choix du guide, distinct de `ArtifactSet` (objet du jeu).** Même logique que `Weapon` / `WeaponRecommendation`. Gère le 4pc **et** le 2+2 (Emblem 4p, ou Golden Troupe 2p + Nymph 2p) via plusieurs lignes portant `nbPieces` + `priorite`.

**`Stat` + `StatType`, `ArtifactSlotType`.** Une stat porte son type (`FLAT` / `PERCENT`) — ATQ% ≠ ATQ fixe. Les main/sub stats renvoient à `Stat` ; la main stat porte aussi son slot (`ArtifactSlotType`).

**`WeaponSecondaryStat` porte une valeur, pas juste une référence.** `valeurMax` stocke la stat secondaire au niveau max (ex. ATQ% 33,1 % au niv. 90).

**`GameVersion` en entité, pas un `string`.** Hu Tao 5.8 et Hu Tao 6.0 sont deux guides distincts ; le versionnement des builds devient traçable.

**`Rarity` / `StatType` en référentiels communs.** Concepts transverses (personnage ET arme pour la rareté) sortis du paquet Objets pour ne pas créer de fausses dépendances.

**Agrégat DDD.** `BuildGuide` est la racine d'agrégation des recommandations qui composent un build. Les objets `WeaponRecommendation`, `ArtifactRecommendation` et les recommandations de stats (`MainStatRecommendation`, `SubStatRecommendation`) appartiennent au cycle de vie du guide et ne sont pas accessibles indépendamment. Les éléments référencés (`Weapon`, `ArtifactSet`, `Stat`…) sont des entités partagées identifiées par leur propre identité et restent hors de l'agrégat.

Cette distinction lève toute ambiguïté entre trois notions :

- la composition UML (`*--`) ;
- l'ownership métier DDD ;
- les références vers des entités externes.

`BuildGuide` n'est donc pas « un objet qui contient tout », mais un contexte métier qui assemble des connaissances existantes — probablement la partie la plus intéressante de la V2.

## Règles de gestion / invariants

- Un seul `CharacterElement.estPrincipal = true` par personnage.
- `BuildRole.priorite >= 1`, une seule priorité 1 par guide.
- Main stat uniquement sur `SANDS` / `GOBLET` / `CIRCLET` (Fleur = PV, Plume = ATQ, fixes).
- Somme des `nbPieces` d'une recommandation d'artefacts = 4 (à valider côté domaine, non exprimable en SQL pur).
- Invariant : `Weapon.WeaponType = Character.WeaponType` (arme recommandée cohérente avec le personnage).

## Reporté en V3

- **Formes jouables du Voyageur** en sous-typage (l'Option A actuelle via `CharacterElement` suffit).
- **`CharacterGameplay`** : talents, ascension, constellations.
- **`TeamComposition`** : builds dépendants de l'équipe (Nilou Bloom, réactions).
- **`StatValue` côté artefacts** : valeurs chiffrées des stats recommandées.
- **Scinder `RecommendationTier`** en deux axes (qualité vs provenance) si besoin de dire « BIS *et* F2P ».

## Fichiers du projet

- `akasharecords-domaine-v2.mermaid` — diagramme complet, coloré, regroupé (app / mermaid.live).
- `akasharecords-domaine-v2-notion.mermaid` — version à plat compatible Notion.
- `akasharecords-domaine-v2.puml` — source PlantUML.
- `akasharecords-schema-postgres.sql` — schéma PostgreSQL (enums natifs, contraintes, trigger).
