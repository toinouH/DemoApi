================================================================================
README - DemoApi
GUIDE COMPLET - CRÉER UNE API ASP.NET CORE AVEC C#
Projet PRODUCT API - Guide pour Débutants
================================================================================

Ce guide vous accompagne étape par étape pour créer votre première API web
avec ASP.NET Core, SQL Server et Entity Framework Core.

================================================================================
PARTIE 1 : PRÉPARATION DE L'ENVIRONNEMENT
================================================================================

ÉTAPE 1.1 : Installer Visual Studio
------------------------------------
1. Téléchargez Visual Studio 2022 Community (gratuit) depuis :
   https://visualstudio.microsoft.com/downloads/

2. Lors de l'installation, sélectionnez la charge de travail :
   "Développement web et ASP.NET"

   Cette charge de travail inclut :
   - .NET SDK
   - ASP.NET Core
   - Entity Framework Core
   - SQL Server Express LocalDB

3. Installez aussi SQL Server Express ou LocalDB (généralement inclus avec VS)

ÉTAPE 1.2 : Vérifier l'installation
------------------------------------
1. Ouvrez Visual Studio
2. Allez dans : Aide > À propos de Microsoft Visual Studio
3. Vérifiez que .NET SDK 10.0 est installé

================================================================================
PARTIE 2 : CRÉER LE PROJET
================================================================================

ÉTAPE 2.1 : Créer un nouveau projet
------------------------------------
1. Dans Visual Studio : Fichier > Nouveau > Projet
2. Recherchez : "ASP.NET Core Web API"
3. Sélectionnez le modèle "ASP.NET Core Web API"
4. Cliquez sur "Suivant"

ÉTAPE 2.2 : Configurer le projet
---------------------------------
1. Nom du projet : "DemoApi" (ou le nom de votre choix)
2. Emplacement : choisissez où sauvegarder votre projet
3. Solution : laissez "Créer une nouvelle solution"
4. Framework : sélectionnez ".NET 10.0" (ou .NET 8.0 si disponible)
5. Cochez :
   ✓ Utiliser des contrôleurs
   ✓ Activer OpenAPI (Swagger)
6. Cliquez sur "Créer"

EXPLICATION : Qu'est-ce qu'un projet Web API ?
-----------------------------------------------
Une Web API est une application qui expose des endpoints (points d'accès)
sur Internet. Ces endpoints permettent à d'autres applications (ou sites web)
de récupérer ou envoyer des données via HTTP.

Exemple : GET /api/products retourne la liste des produits

================================================================================
PARTIE 3 : COMPRENDRE LA STRUCTURE DU PROJET
================================================================================

Après la création, vous verrez ces dossiers :

📁 Controllers/
   └─ Contient les contrôleurs (les routes de votre API)

📁 Models/
   └─ Contient les modèles de données (classes qui représentent vos données)

📁 Data/
   └─ Contiendra le contexte de base de données (connexion à la DB)

📁 Services/
   └─ Contiendra la logique métier (règles de votre application)

📁 wwwroot/
   └─ Contient les fichiers statiques (HTML, CSS, images)

📄 Program.cs
   └─ Le point d'entrée de l'application (configuration)

📄 appsettings.json
   └─ Fichier de configuration (chaînes de connexion, etc.)

================================================================================
PARTIE 4 : CONFIGURER LA BASE DE DONNÉES
================================================================================

ÉTAPE 4.1 : Installer les packages NuGet nécessaires
-----------------------------------------------------
1. Clic droit sur le projet dans l'Explorateur de solutions
2. Sélectionnez "Gérer les packages NuGet..."
3. Onglet "Parcourir"
4. Installez ces packages (un par un) :

   ✓ Microsoft.EntityFrameworkCore
      Version : 10.0.0
      Utilité : Framework pour accéder aux bases de données

   ✓ Microsoft.EntityFrameworkCore.SqlServer
      Version : 10.0.0
      Utilité : Support spécifique pour SQL Server

   ✓ Microsoft.EntityFrameworkCore.Tools
      Version : 10.0.0
      Utilité : Outils pour créer les migrations (structure de la DB)

   ✓ Microsoft.AspNetCore.OpenApi
      Version : 10.0.2
      Utilité : Support OpenAPI/Swagger (documentation de l'API)

EXPLICATION : Qu'est-ce qu'Entity Framework Core ?
---------------------------------------------------
Entity Framework Core (EF Core) est un ORM (Object-Relational Mapping).
Il permet de travailler avec une base de données en utilisant des objets C#
au lieu d'écrire du SQL directement.

ÉTAPE 4.2 : Configurer la chaîne de connexion
-----------------------------------------------
1. Ouvrez le fichier "appsettings.json"
2. Ajoutez la section "ConnectionStrings" :

   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=DemoApiDb;Trusted_Connection=True;TrustServerCertificate=True;"
     },
     "Logging": {
       ...
     }
   }

================================================================================
PARTIE 5 : CRÉER LE MODÈLE (MODEL)
================================================================================

ÉTAPE 5.1 : Créer la classe Product
------------------------------------
📁 Models/Product.cs

   using System.ComponentModel.DataAnnotations;

   namespace DemoApi.Models;

   public class Product
   {
       public int Id { get; set; }

       [Required(ErrorMessage = "Le nom du produit est requis")]
       [MinLength(1, ErrorMessage = "Le nom doit contenir au moins 1 caractère")]
       public string Name { get; set; } = string.Empty;

       [Range(0.01, 1000000, ErrorMessage = "Le prix doit être entre 0.01 et 1,000,000")]
       public decimal Price { get; set; }
   }

ÉTAPE 5.2 : Comprendre les propriétés C#
-----------------------------------------
public int Id { get; set; }

Propriété automatique :
- get : lire
- set : écrire

Équivalent long :
private int _id;
public int Id 
{ 
    get { return _id; } 
    set { _id = value; } 
}

================================================================================
PARTIE 6 : CRÉER LE CONTEXTE DE BASE DE DONNÉES
================================================================================

ÉTAPE 6.1 : Créer AppDbContext
-------------------------------
📁 Data/AppDbContext.cs

   using Microsoft.EntityFrameworkCore;
   using DemoApi.Models;

   namespace DemoApi.Data;

   public class AppDbContext : DbContext
   {
       public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
       {
       }

       public DbSet<Product> Products { get; set; }

       protected override void OnModelCreating(ModelBuilder modelBuilder)
       {
           base.OnModelCreating(modelBuilder);

           modelBuilder.Entity<Product>()
               .Property(p => p.Price)
               .HasPrecision(18, 2);
       }
   }

================================================================================
PARTIE 7 : CRÉER LE SERVICE (COUCHE MÉTIER)
================================================================================

ÉTAPE 7.1 : Créer l'interface IProductService
----------------------------------------------
📁 Services/IProductService.cs

   using DemoApi.Models;

   namespace DemoApi.Services;

   public interface IProductService
   {
       Task<IEnumerable<Product>> GetAllAsync();
       Task<Product?> GetByIdAsync(int id);
       Task<IEnumerable<Product>> GetMoreExpensiveThanAsync(decimal minPrice);
       Task<Product> CreateAsync(string name, decimal price);
       Task<bool> UpdatePriceAsync(int id, decimal newPrice);
   }

ÉTAPE 7.2 : Créer ProductService (implémentation)
--------------------------------------------------
📁 Services/ProductService.cs

   using Microsoft.EntityFrameworkCore;
   using DemoApi.Data;
   using DemoApi.Models;

   namespace DemoApi.Services;

   public class ProductService : IProductService
   {
       private readonly AppDbContext _context;

       public ProductService(AppDbContext context)
       {
           _context = context;
       }

       public async Task<IEnumerable<Product>> GetAllAsync()
       {
           return await _context.Products
               .AsNoTracking()
               .ToListAsync();
       }

       public async Task<Product?> GetByIdAsync(int id)
       {
           return await _context.Products
               .AsNoTracking()
               .FirstOrDefaultAsync(p => p.Id == id);
       }

       public async Task<IEnumerable<Product>> GetMoreExpensiveThanAsync(decimal minPrice)
       {
           return await _context.Products
               .AsNoTracking()
               .Where(p => p.Price > minPrice)
               .OrderBy(p => p.Price)
               .ToListAsync();
       }

       public async Task<Product> CreateAsync(string name, decimal price)
       {
           var product = new Product
           {
               Name = name,
               Price = price
           };

           _context.Products.Add(product);
           await _context.SaveChangesAsync();
           return product;
       }

       public async Task<bool> UpdatePriceAsync(int id, decimal newPrice)
       {
           var product = await _context.Products.FindAsync(id);
           if (product == null)
               return false;

           product.Price = newPrice;
           await _context.SaveChangesAsync();
           return true;
       }
   }

📌 LINQ utilisé ici :
- Where(...)
- OrderBy(...)
- FirstOrDefaultAsync(...)

================================================================================
PARTIE 8 : CRÉER LE CONTRÔLEUR (API ENDPOINTS)
================================================================================

ÉTAPE 8.1 : Créer ProductsController
-------------------------------------
📁 Controllers/ProductsController.cs

✅ Endpoints :
- GET    /api/products
- GET    /api/products/{id}
- GET    /api/products/expensive?minPrice=5
- POST   /api/products
- PUT    /api/products/{id}/price?newPrice=9.99

================================================================================
PARTIE 9 : CONFIGURER PROGRAM.CS
================================================================================

ÉTAPE 9.1 : Configurer les services
------------------------------------
- AddControllers()
- AddDbContext(AppDbContext)
- AddScoped<IProductService, ProductService>()
- AddOpenApi()

================================================================================
PARTIE 10 : MIGRATIONS ET BASE DE DONNÉES
================================================================================

ÉTAPE 10.1 : Ouvrir la Console NuGet
-------------------------------------
Outils > Gestionnaire de packages NuGet > Console

ÉTAPE 10.2 : Créer la migration
--------------------------------
Add-Migration InitialCreate

ÉTAPE 10.3 : Appliquer la migration
------------------------------------
Update-Database

================================================================================
PARTIE 11 : CRÉER L'INTERFACE WEB (FRONTEND)
================================================================================

ÉTAPE 11.1 : Créer le dossier wwwroot
--------------------------------------
wwwroot contient les fichiers statiques.

ÉTAPE 11.2 : Créer index.html
------------------------------
Interface HTML pour tester l’API (fetch).

ÉTAPE 11.3 : Créer swagger.html
--------------------------------
Swagger UI accessible via le navigateur.

================================================================================
PARTIE 12 : LANCER ET TESTER L'APPLICATION
================================================================================

ÉTAPE 12.1 : Lancer
--------------------
F5 dans Visual Studio

ÉTAPE 12.2 : Tester
--------------------
- Swagger : https://localhost:xxxx/swagger.html
- API JSON : https://localhost:xxxx/api/products
- Interface : https://localhost:xxxx/index.html

================================================================================
PARTIE 13 : DÉBOGAGE (RÉSOLUTION DE PROBLÈMES)
================================================================================

PROBLÈME : DB connexion
- vérifier LocalDB
- vérifier connection string
- Update-Database

PROBLÈME : migration déjà existante
- Remove-Migration
- Add-Migration InitialCreate
- Update-Database

PROBLÈME : port utilisé
- modifier launchSettings.json

PROBLÈME : CORS
- AddCors + UseCors

================================================================================
PARTIE 14 : CONCEPTS CLÉS À RETENIR
================================================================================

- MVC Pattern
- Services (séparation logique métier)
- Dependency Injection
- Async/Await
- EF Core + migrations
- API REST
- HTTP status codes

================================================================================
PARTIE 15 : PROJET FINAL (BACKEND + FRONTEND + BONUS)
================================================================================

🎯 Objectif : Ajouter des APIs + créer la partie frontend pour gérer
Fournisseurs et Matières premières.

--------------------------------------------------------------------------------
15.1 API Fournisseurs (Supplier)
--------------------------------------------------------------------------------
Fonctionnalités attendues :
✅ Ajouter un fournisseur
✅ Voir la liste des fournisseurs
✅ Modifier un fournisseur

Endpoints recommandés :
- GET  /api/suppliers
- GET  /api/suppliers/{id}
- POST /api/suppliers
- PUT  /api/suppliers/{id}

--------------------------------------------------------------------------------
15.2 API Matières Premières (RawMaterial)
--------------------------------------------------------------------------------
Fonctionnalités attendues :
✅ Ajouter une matière première
✅ Voir la liste des matières premières
✅ Modifier une matière première

Endpoints recommandés :
- GET  /api/rawmaterials
- GET  /api/rawmaterials/{id}
- POST /api/rawmaterials
- PUT  /api/rawmaterials/{id}

--------------------------------------------------------------------------------
15.3 FRONTEND (wwwroot)
--------------------------------------------------------------------------------
Créer une interface Web avec :
✅ Gestion Fournisseurs (ajout / liste / modification)
✅ Gestion Matières premières (ajout / liste / modification)

--------------------------------------------------------------------------------
15.4 BONUS (NOTE MAX) : RELATIONS + API DE RECHERCHE
--------------------------------------------------------------------------------

🎯 BONUS attendu :
Faire le lien entre les modèles et créer une API de recherche de produits.

Recherche Produits par Fournisseur :
✅ GET /api/products/by-supplier/{supplierId}

Recherche Produits par Matière Première :
✅ GET /api/products/by-rawmaterial/{rawMaterialId}

Relations recommandées :
- Supplier -> Products (1 fournisseur fournit plusieurs produits)
- Product <-> RawMaterial (Many-to-Many avec table de liaison)

================================================================================
FIN
================================================================================
Bravo 🚀 Votre projet DemoApi est une base solide pour construire des APIs
professionnelles en ASP.NET Core !
