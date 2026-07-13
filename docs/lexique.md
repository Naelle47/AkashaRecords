# AkashaRecords — Lexique (version langage courant)

Le vocabulaire d'architecture / DDD, retraduit en termes abordables, avec un exemple AkashaRecords à chaque fois. Doc vivante : à enrichir au fil de l'apprentissage.

## Les briques de l'architecture

**Entité** → une *fiche* de ton monde, avec une identité propre. Un `Character`, une `Weapon`, un `Build`. Deux personnages du même nom restent deux fiches distinctes (leur `id` les différencie).

**Domain / Core** → le *cœur* : tes fiches + tes règles + les promesses de service. Il ne sait rien de la base ni du web. Juste « ce qu'est un personnage, ce qu'est un build ».

**Repository (dépôt)** → le *bibliothécaire*. Tu lui dis « donne-moi le build 42 » ou « range ce nouveau perso » ; lui se débrouille avec le SQL. Tu passes commande sans savoir comment il s'y prend. Deux variantes à distinguer pour éviter l'ambiguïté : le *repository métier* renvoie des **entités Core** (pour écrire / opérer sur le domaine), le *query repository* renvoie un **DTO de lecture** prêt pour la vue. On écrit par les entités, on lit par les DTO.

**Interface / contrat** → la *liste des promesses* du bibliothécaire, sans la recette. `ICharacterRepository` dit « je sais trouver un perso par id » — la promesse, pas le comment.

**Infrastructure** → la *mécanique concrète* qui tient les promesses : le vrai code Dapper / Npgsql qui parle à PostgreSQL. L'exécutant derrière le comptoir.

**Présentation (MVC / API)** → la *vitrine / le comptoir* : ce que voit l'utilisateur. Comptoir HTML pour le MVC, comptoir JSON pour une éventuelle API.

**Couche** → un *étage* de responsabilité (présentation, infrastructure, cœur). Chaque étage a un rôle et ne se mêle pas de celui des autres.

## Autour du modèle métier

**Racine d'agrégat** → le *chef de dossier*. `BuildGuide` commande ses recommandations ; on ne parle jamais à une recommandation directement, on passe toujours par le guide.

**DTO** (Data Transfer Object) → une *fiche allégée pour le transport*. Ce que tu envoies à la vue ou à l'API, souvent un sous-ensemble d'une entité (sans les champs internes).

**Value Object** → une *donnée définie par son contenu*, pas par une identité. Pas d'`id` : deux exemplaires identiques sont interchangeables. Exemple : une valeur de stat comme « ATQ% 46,6 », ou un seuil « Taux CRIT ≥ 70% ». À l'inverse d'une entité (`Character` a une identité), un value object *est* juste ses valeurs.

**ViewModel** → une *fiche taillée pour l'écran*. Ce que la vue reçoit : les données d'une entité mises en forme, plus ce qui est propre à l'affichage (stats calculées, badges, libellés). Une entité Core ne va jamais directement dans la vue — on passe toujours par un ViewModel.

**InputModel** → le *ViewModel de l'entrée* : la fiche sur laquelle un formulaire est bindé, contenant **uniquement** les champs éditables. On la mappe ensuite vers l'entité.

**Over-posting** → la *faille du binding trop large* : si un formulaire est bindé directement sur une entité, un utilisateur peut envoyer en douce un champ que tu ne voulais pas modifiable (`id`, `rarity`, un flag interne). L'`InputModel`, en ne listant que le permis, ferme cette porte.

**Helper** → une *boîte à outils statique* de petites méthodes réutilisables. Chez toi, du formatage de présentation : construire l'URL d'une icône (`ImageHelper`), afficher une date ou une rareté (`CharacterHelper`). Côté présentation → `Web/Helpers/`, appelé au mapping pour remplir les ViewModels.

## Autour de la base de données

**Migration** → le *journal de travaux* sur la base : chaque changement daté, versionné, rejouable. C'est ce que fait DbUp avec tes scripts numérotés.

**Seed** → les *données d'amorçage* insérées au départ (éléments, régions, types d'arme) pour que la base ne soit pas vide.

**Baseline** → la *photo de départ* : un premier script qui représente l'état actuel de la base, pour la mettre sous contrôle des migrations sans tout recréer.

**Unit of Work** → la *commande groupée* : plusieurs opérations regroupées dans une seule transaction, qui réussissent ou échouent ensemble. Exemple : enregistrer un `Build` avec ses `build_weapons` et `build_artifacts` d'un coup — soit tout est validé, soit rien, jamais un build à moitié saisi.

## Autour du câblage

**Inversion de contrôle (IoC)** → le *principe* : tu ne construis plus tes objets toi-même avec `new`, tu délègues à un *chef d'orchestre* (le conteneur) le soin d'assembler les pièces et de te les fournir. Tu déclares « voici mes besoins », il se charge du reste.

**Injection de dépendances (DI)** → la *mise en pratique* la plus courante de l'IoC : au lieu qu'une classe *fabrique* elle-même ses outils, on les lui *fournit* de l'extérieur. Le comptoir ne construit pas son bibliothécaire : on le lui branche au démarrage. Ça rend le tout interchangeable et testable.

**Mapping** → le *traducteur* entre deux formes d'une même chose. Transformer une ligne de la table `characters` en objet `Character`, ou un `Build` en `BuildDto` pour la vue. Avec Dapper, le mapping c'est la façon dont un résultat SQL devient ton objet C#.

**Middleware** → un *poste de contrôle* sur le trajet de chaque requête HTTP. Avant d'arriver à ton controller, la requête traverse une file de postes (authentification, logging, gestion d'erreurs, HTTPS…), et la réponse les retraverse en sortie. Dans une appli ASP.NET, tu montes cette chaîne dans `Program.cs`.

## Autour des tests

**Test unitaire** → vérifie *une brique isolée* (une méthode, une règle) sans base ni réseau, avec ses dépendances simulées. Rapide et ciblé. Exemple : la règle qui impose une seule priorité 1 dans un build.

**Test d'intégration** → vérifie que *plusieurs morceaux marchent ensemble pour de vrai* : un repository qui tape réellement PostgreSQL, ou un endpoint de bout en bout. Plus lent, plus proche du réel.

**Mock** → une *doublure* d'une dépendance, programmée pour répondre ce que tu veux. Tu remplaces le vrai repository par un faux qui dit « le perso 42 = Furina », pour tester le controller sans base.

**Fixture** → le *décor planté avant un test* : données de départ, base initialisée, objets prêts. Ce qu'on met en place pour que le test parte de conditions connues.

**Factory (de test)** → l'usine qui *démarre ton appli en mémoire* pour les tests d'intégration (`WebApplicationFactory`). À ne pas confondre avec `IDbConnectionFactory` (infra), qui fabrique des connexions à la base : deux « factory », deux rôles.

**Testcontainers** → lance une *vraie base jetable* dans un conteneur Docker le temps des tests, puis la détruit. Tes tests d'intégration tournent contre un Postgres réel mais éphémère, sans polluer ta base.

## La logique d'ensemble en une phrase

Le **comptoir** (MVC) passe commande au **bibliothécaire** (repository), dont la **promesse** vit dans le **cœur** (Core) et la **recette** dans l'**infrastructure** (Dapper) — et tout le monde regarde vers le cœur.

---

*Termes à ajouter au fur et à mesure : service applicatif, value object en record C#, middleware personnalisé, etc.*
