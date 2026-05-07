# Graph Report - /mnt/8a0d3363-aa14-4b24-ab6a-36c7252187cf/Projects/PokeSync  (2026-05-07)

## Corpus Check
- Large corpus: 203 files · ~132,858 words. Semantic extraction will be expensive (many Claude tokens). Consider running on a subfolder, or use --no-semantic to run AST-only.

## Summary
- 1328 nodes · 2867 edges · 58 communities detected
- Extraction: 57% EXTRACTED · 43% INFERRED · 0% AMBIGUOUS · INFERRED: 1220 edges (avg confidence: 0.8)
- Token cost: 0 input · 0 output

## Community Hubs (Navigation)
- [[_COMMUNITY_Data Actions|Data Actions]]
- [[_COMMUNITY_Backup Routes|Backup Routes]]
- [[_COMMUNITY_Auth System|Auth System]]
- [[_COMMUNITY_Pokedex Generation|Pokedex Generation]]
- [[_COMMUNITY_PKM Conversion|PKM Conversion]]
- [[_COMMUNITY_Legacy Data Loading|Legacy Data Loading]]
- [[_COMMUNITY_Action Dispatcher|Action Dispatcher]]
- [[_COMMUNITY_HTTP Client & Static Data|HTTP Client & Static Data]]
- [[_COMMUNITY_Memory Cache|Memory Cache]]
- [[_COMMUNITY_Data Normalization|Data Normalization]]
- [[_COMMUNITY_EF Migrations|EF Migrations]]
- [[_COMMUNITY_Auth Controller|Auth Controller]]
- [[_COMMUNITY_PKM Data Access|PKM Data Access]]
- [[_COMMUNITY_Android API|Android API]]
- [[_COMMUNITY_File IO|File I/O]]
- [[_COMMUNITY_Community 15|Community 15]]
- [[_COMMUNITY_Community 16|Community 16]]
- [[_COMMUNITY_Community 17|Community 17]]
- [[_COMMUNITY_Community 18|Community 18]]
- [[_COMMUNITY_Community 19|Community 19]]
- [[_COMMUNITY_Community 20|Community 20]]
- [[_COMMUNITY_Community 21|Community 21]]
- [[_COMMUNITY_Community 22|Community 22]]
- [[_COMMUNITY_Community 23|Community 23]]
- [[_COMMUNITY_Community 24|Community 24]]
- [[_COMMUNITY_Community 25|Community 25]]
- [[_COMMUNITY_Community 26|Community 26]]
- [[_COMMUNITY_Community 27|Community 27]]
- [[_COMMUNITY_Community 28|Community 28]]
- [[_COMMUNITY_Community 29|Community 29]]
- [[_COMMUNITY_Community 30|Community 30]]
- [[_COMMUNITY_Community 31|Community 31]]
- [[_COMMUNITY_Community 32|Community 32]]
- [[_COMMUNITY_Community 33|Community 33]]
- [[_COMMUNITY_Community 34|Community 34]]
- [[_COMMUNITY_Community 35|Community 35]]
- [[_COMMUNITY_Community 36|Community 36]]
- [[_COMMUNITY_Community 37|Community 37]]
- [[_COMMUNITY_Community 38|Community 38]]
- [[_COMMUNITY_Community 39|Community 39]]
- [[_COMMUNITY_Community 59|Community 59]]
- [[_COMMUNITY_Community 60|Community 60]]
- [[_COMMUNITY_Community 61|Community 61]]
- [[_COMMUNITY_Community 62|Community 62]]
- [[_COMMUNITY_Community 63|Community 63]]
- [[_COMMUNITY_Community 64|Community 64]]
- [[_COMMUNITY_Community 65|Community 65]]
- [[_COMMUNITY_Community 66|Community 66]]
- [[_COMMUNITY_Community 67|Community 67]]
- [[_COMMUNITY_Community 68|Community 68]]
- [[_COMMUNITY_Community 69|Community 69]]
- [[_COMMUNITY_Community 70|Community 70]]
- [[_COMMUNITY_Community 71|Community 71]]
- [[_COMMUNITY_Community 72|Community 72]]
- [[_COMMUNITY_Community 73|Community 73]]
- [[_COMMUNITY_Community 74|Community 74]]
- [[_COMMUNITY_Community 75|Community 75]]
- [[_COMMUNITY_Community 76|Community 76]]

## God Nodes (most connected - your core abstractions)
1. `ActionService` - 28 edges
2. `StorageController` - 28 edges
3. `GenStaticOthers` - 25 edges
4. `PkmVariantLoader` - 22 edges
5. `FileIOService` - 22 edges
6. `IFileIOService` - 21 edges
7. `PKMConverterUtils` - 19 edges
8. `ImmutablePKM` - 19 edges
9. `IPkmVariantLoader` - 19 edges
10. `SavePkmLoader` - 19 edges

## Surprising Connections (you probably didn't know these)
- `FromSave()` --calls--> `GetSafeLanguage()`  [INFERRED]
  /mnt/8a0d3363-aa14-4b24-ab6a-36c7252187cf/Projects/PokeSync/PKVault.Backend/save-infos/dto/SaveInfosDTO.cs → /mnt/8a0d3363-aa14-4b24-ab6a-36c7252187cf/Projects/PokeSync/PKVault.Backend/settings/dto/SettingsDTO.cs

## Communities

### Community 0 - "Data Actions"
Cohesion: 0.04
Nodes (23): DataAction, DeletePkmVariantAction, DetachPkmSaveAction, DexSyncAction, EditPkmSaveAction, EditPkmVariantAction, EvolvePkmAction, IEntityLoader (+15 more)

### Community 1 - "Backup Routes"
Cohesion: 0.04
Nodes (16): BackupController, BackupService, DbSeedingService, IDbSeedingService, ArchiveEntry, FileIOService, GenStaticDataService, LoggerExtension (+8 more)

### Community 2 - "Auth System"
Cohesion: 0.03
Nodes (15): AuthDbContext, AuthService, BankLoader, IBankLoader, BoxLoader, IBoxLoader, DbContext, DexLoader (+7 more)

### Community 3 - "Pokedex Generation"
Cohesion: 0.04
Nodes (17): Dex123Service, Dex3ColoService, Dex3XDService, Dex4Service, Dex5Service, Dex6AOService, Dex6XYService, Dex7bService (+9 more)

### Community 4 - "PKM Conversion"
Cohesion: 0.08
Nodes (11): PK2Converter, PK3Converter, PK4Converter, PK5Converter, PK6Converter, PK7Converter, PK8Converter, PK9Converter (+3 more)

### Community 5 - "Legacy Data Loading"
Cohesion: 0.04
Nodes (15): IOException, LegacyBankLoader, LegacyBankNormalize, LegacyBoxLoader, LegacyBoxNormalize, LegacyDexLoader, LegacyDexNormalize, LegacyEntityLoader (+7 more)

### Community 6 - "Action Dispatcher"
Cohesion: 0.04
Nodes (9): ActionService, DataAction, DataService, DataUpdateFlags, DataUpdateFlagsState, DataUpdateSaveFlags, DataUpdateSaveListFlags, StorageQueryService (+1 more)

### Community 7 - "HTTP Client & Static Data"
Cohesion: 0.05
Nodes (11): AssemblyClient, Dictionary, GenStaticEvolves, StaticEvolvesData, GenStaticOthers, GenStaticSpecies, StaticSpeciesData, PokeApiFileClient (+3 more)

### Community 8 - "Memory Cache"
Cohesion: 0.04
Nodes (9): CacheWithTiming, LegacyEntityLoader, LegacyPkmVersionLoader, ISaveBoxLoader, SaveBoxLoader, ISavePkmLoader, SavePkmLoader, SaveWrapper (+1 more)

### Community 9 - "Data Normalization"
Cohesion: 0.05
Nodes (18): DataNormalizeAction, ILegalityAnalysisService, LegalityAnalysisService, LegalityAnalysisWrapper, PkmLegalityService, FromSave(), SaveInfosController, ISavesLoadersService (+10 more)

### Community 10 - "EF Migrations"
Cohesion: 0.03
Nodes (23): InitialCreate, PKVault.Backend.Migrations, AddDexLanguages, PKVault.Backend.Migrations, PKVault.Backend.Migrations, RemoveCascadeDelete, PKVault.Backend.Migrations, UpdateToPKHeX260306 (+15 more)

### Community 11 - "Auth Controller"
Cohesion: 0.06
Nodes (9): AuthController, ControllerBase, DexController, DexService, IMetaLoader, MetaLoader, SettingsController, WarningsController (+1 more)

### Community 12 - "PKM Data Access"
Cohesion: 0.08
Nodes (4): ImmutablePKM, ILegacyPkmFileLoader, LegacyPkmFileLoader, LegacyPKMLoadException

### Community 13 - "Android API"
Cohesion: 0.1
Nodes (3): AndroidController, AndroidSaveService, AndroidVaultService

### Community 14 - "File I/O"
Cohesion: 0.07
Nodes (5): Archive, IArchive, IArchiveEntry, IFileIOService, IDisposable

### Community 15 - "Community 15"
Cohesion: 0.16
Nodes (3): GenStaticSpritesheets, SpritesheetFileClient, StaticDataController

### Community 16 - "Community 16"
Cohesion: 0.15
Nodes (7): EntityJsonContext, JsonSerializerContext, LegacyEntityJsonContext, RouteJsonContext, SettingsMutableDTOJsonContext, SpritesheetJsonContext, StaticDataJsonContext

### Community 17 - "Community 17"
Cohesion: 0.18
Nodes (4): ISessionServiceMinimal, ISessionService, ISessionServiceMinimal, UserSessionState

### Community 18 - "Community 18"
Cohesion: 0.2
Nodes (5): BankEntity, BoxEntity, DexFormEntity, IEntity, PkmVariantEntity

### Community 19 - "Community 19"
Cohesion: 0.22
Nodes (3): ByteArrayJsonConverter, JsonConverter, NumberToStringConverter

### Community 20 - "Community 20"
Cohesion: 0.25
Nodes (4): AuthDbContextModelSnapshot, ModelSnapshot, PKVault.Backend.Migrations, SessionDbContextModelSnapshot

### Community 21 - "Community 21"
Cohesion: 0.53
Nodes (1): ExceptionHandlingMiddleware

### Community 22 - "Community 22"
Cohesion: 0.5
Nodes (2): IOutboundParameterTransformer, SlugifyParameterTransformer

### Community 23 - "Community 23"
Cohesion: 0.67
Nodes (2): IPkmSharePropertiesService, PkmSharePropertiesService

### Community 24 - "Community 24"
Cohesion: 0.5
Nodes (2): AddExternalFlags, PKVault.Backend.Migrations

### Community 25 - "Community 25"
Cohesion: 0.5
Nodes (2): AddAlphaFlags, PKVault.Backend.Migrations

### Community 26 - "Community 26"
Cohesion: 0.5
Nodes (2): InitialCreate, PKVault.Backend.Migrations

### Community 27 - "Community 27"
Cohesion: 0.5
Nodes (2): AddVariantContext, PKVault.Backend.Migrations

### Community 28 - "Community 28"
Cohesion: 0.5
Nodes (2): AddMetaTable, PKVault.Backend.Migrations

### Community 29 - "Community 29"
Cohesion: 0.5
Nodes (2): PKVault.Backend.Migrations, UpdateToPKHeX260306

### Community 30 - "Community 30"
Cohesion: 0.5
Nodes (2): AddDexLanguages, PKVault.Backend.Migrations

### Community 31 - "Community 31"
Cohesion: 0.5
Nodes (2): AddDexFormContext, PKVault.Backend.Migrations

### Community 32 - "Community 32"
Cohesion: 0.5
Nodes (2): PKVault.Backend.Migrations, RemoveCascadeDelete

### Community 33 - "Community 33"
Cohesion: 0.67
Nodes (2): IEntity, IWithId

### Community 34 - "Community 34"
Cohesion: 0.67
Nodes (1): EnvUtil

### Community 35 - "Community 35"
Cohesion: 1.0
Nodes (1): PkmFileEntity

### Community 36 - "Community 36"
Cohesion: 1.0
Nodes (1): MetaEntity

### Community 37 - "Community 37"
Cohesion: 1.0
Nodes (1): UserEntity

### Community 38 - "Community 38"
Cohesion: 1.0
Nodes (1): IWithId

### Community 39 - "Community 39"
Cohesion: 1.0
Nodes (1): AndroidVaultEntity

### Community 59 - "Community 59"
Cohesion: 1.0
Nodes (1): Sprite: spritesheet_species_2.webp

### Community 60 - "Community 60"
Cohesion: 1.0
Nodes (1): Sprite: spritesheet_species_7.webp

### Community 61 - "Community 61"
Cohesion: 1.0
Nodes (1): Sprite: spritesheet_species_8.webp

### Community 62 - "Community 62"
Cohesion: 1.0
Nodes (1): Sprite: spritesheet_species_10.webp

### Community 63 - "Community 63"
Cohesion: 1.0
Nodes (1): Sprite: spritesheet_species_4.webp

### Community 64 - "Community 64"
Cohesion: 1.0
Nodes (1): Sprite: spritesheet_items_0.webp

### Community 65 - "Community 65"
Cohesion: 1.0
Nodes (1): Sprite: spritesheet_species_3.webp

### Community 66 - "Community 66"
Cohesion: 1.0
Nodes (1): Sprite: spritesheet_species_6.webp

### Community 67 - "Community 67"
Cohesion: 1.0
Nodes (1): Sprite: spritesheet_species_9.webp

### Community 68 - "Community 68"
Cohesion: 1.0
Nodes (1): Sprite: spritesheet_species_5.webp

### Community 69 - "Community 69"
Cohesion: 1.0
Nodes (1): Sprite: spritesheet_species_0.webp

### Community 70 - "Community 70"
Cohesion: 1.0
Nodes (1): Sprite: spritesheet_species_1.webp

### Community 71 - "Community 71"
Cohesion: 1.0
Nodes (1): Sprite: 249.png

### Community 72 - "Community 72"
Cohesion: 1.0
Nodes (1): Doc: README

### Community 73 - "Community 73"
Cohesion: 1.0
Nodes (1): Doc: PkmConvertService

### Community 74 - "Community 74"
Cohesion: 1.0
Nodes (1): Doc: ARCHITECTURE

### Community 75 - "Community 75"
Cohesion: 1.0
Nodes (1): Doc: DATA

### Community 76 - "Community 76"
Cohesion: 1.0
Nodes (1): Doc: SESSION

## Knowledge Gaps
- **47 isolated node(s):** `MainCreatePkmVariantActionInput`, `PkmFileEntity`, `MetaEntity`, `UserSessionState`, `UserEntity` (+42 more)
  These have ≤1 connection - possible missing edges or undocumented components.
- **Thin community `Community 21`** (6 nodes): `ExceptionHandlingMiddleware.cs`, `ExceptionHandlingMiddleware`, `.GetStatusCode()`, `.InvalidCharacterRegex()`, `.Invoke()`, `.WriteExceptionResponse()`
  Too small to be a meaningful cluster - may be noise or needs more connections extracted.
- **Thin community `Community 22`** (5 nodes): `IOutboundParameterTransformer`, `SlugifyParameterTransformer.cs`, `SlugifyParameterTransformer`, `.MyRegex()`, `.TransformOutbound()`
  Too small to be a meaningful cluster - may be noise or needs more connections extracted.
- **Thin community `Community 23`** (4 nodes): `IPkmSharePropertiesService`, `.SharePropertiesTo()`, `PkmSharePropertiesService`, `PkmSharePropertiesService.cs`
  Too small to be a meaningful cluster - may be noise or needs more connections extracted.
- **Thin community `Community 24`** (4 nodes): `AddExternalFlags`, `.BuildTargetModel()`, `PKVault.Backend.Migrations`, `20260311211548_AddExternalFlags.Designer.cs`
  Too small to be a meaningful cluster - may be noise or needs more connections extracted.
- **Thin community `Community 25`** (4 nodes): `AddAlphaFlags`, `.BuildTargetModel()`, `PKVault.Backend.Migrations`, `20260326011036_AddAlphaFlags.Designer.cs`
  Too small to be a meaningful cluster - may be noise or needs more connections extracted.
- **Thin community `Community 26`** (4 nodes): `InitialCreate`, `.BuildTargetModel()`, `PKVault.Backend.Migrations`, `20260128223806_InitialCreate.Designer.cs`
  Too small to be a meaningful cluster - may be noise or needs more connections extracted.
- **Thin community `Community 27`** (4 nodes): `AddVariantContext`, `.BuildTargetModel()`, `PKVault.Backend.Migrations`, `20260318143740_AddVariantContext.Designer.cs`
  Too small to be a meaningful cluster - may be noise or needs more connections extracted.
- **Thin community `Community 28`** (4 nodes): `AddMetaTable`, `.BuildTargetModel()`, `PKVault.Backend.Migrations`, `20260319132911_AddMetaTable.Designer.cs`
  Too small to be a meaningful cluster - may be noise or needs more connections extracted.
- **Thin community `Community 29`** (4 nodes): `PKVault.Backend.Migrations`, `UpdateToPKHeX260306`, `.BuildTargetModel()`, `20260307174237_UpdateToPKHeX260306.Designer.cs`
  Too small to be a meaningful cluster - may be noise or needs more connections extracted.
- **Thin community `Community 30`** (4 nodes): `AddDexLanguages`, `.BuildTargetModel()`, `PKVault.Backend.Migrations`, `20260203021248_AddDexLanguages.Designer.cs`
  Too small to be a meaningful cluster - may be noise or needs more connections extracted.
- **Thin community `Community 31`** (4 nodes): `AddDexFormContext`, `.BuildTargetModel()`, `PKVault.Backend.Migrations`, `20260307230359_AddDexFormContext.Designer.cs`
  Too small to be a meaningful cluster - may be noise or needs more connections extracted.
- **Thin community `Community 32`** (4 nodes): `PKVault.Backend.Migrations`, `RemoveCascadeDelete`, `.BuildTargetModel()`, `20260206121645_RemoveCascadeDelete.Designer.cs`
  Too small to be a meaningful cluster - may be noise or needs more connections extracted.
- **Thin community `Community 33`** (3 nodes): `IEntity`, `IEntity.cs`, `IWithId`
  Too small to be a meaningful cluster - may be noise or needs more connections extracted.
- **Thin community `Community 34`** (3 nodes): `EnvUtil`, `.ToInt()`, `EnvUtil.cs`
  Too small to be a meaningful cluster - may be noise or needs more connections extracted.
- **Thin community `Community 35`** (2 nodes): `PkmFileEntity.cs`, `PkmFileEntity`
  Too small to be a meaningful cluster - may be noise or needs more connections extracted.
- **Thin community `Community 36`** (2 nodes): `MetaEntity.cs`, `MetaEntity`
  Too small to be a meaningful cluster - may be noise or needs more connections extracted.
- **Thin community `Community 37`** (2 nodes): `UserEntity.cs`, `UserEntity`
  Too small to be a meaningful cluster - may be noise or needs more connections extracted.
- **Thin community `Community 38`** (2 nodes): `IWithId.cs`, `IWithId`
  Too small to be a meaningful cluster - may be noise or needs more connections extracted.
- **Thin community `Community 39`** (2 nodes): `AndroidVaultEntity.cs`, `AndroidVaultEntity`
  Too small to be a meaningful cluster - may be noise or needs more connections extracted.
- **Thin community `Community 59`** (1 nodes): `Sprite: spritesheet_species_2.webp`
  Too small to be a meaningful cluster - may be noise or needs more connections extracted.
- **Thin community `Community 60`** (1 nodes): `Sprite: spritesheet_species_7.webp`
  Too small to be a meaningful cluster - may be noise or needs more connections extracted.
- **Thin community `Community 61`** (1 nodes): `Sprite: spritesheet_species_8.webp`
  Too small to be a meaningful cluster - may be noise or needs more connections extracted.
- **Thin community `Community 62`** (1 nodes): `Sprite: spritesheet_species_10.webp`
  Too small to be a meaningful cluster - may be noise or needs more connections extracted.
- **Thin community `Community 63`** (1 nodes): `Sprite: spritesheet_species_4.webp`
  Too small to be a meaningful cluster - may be noise or needs more connections extracted.
- **Thin community `Community 64`** (1 nodes): `Sprite: spritesheet_items_0.webp`
  Too small to be a meaningful cluster - may be noise or needs more connections extracted.
- **Thin community `Community 65`** (1 nodes): `Sprite: spritesheet_species_3.webp`
  Too small to be a meaningful cluster - may be noise or needs more connections extracted.
- **Thin community `Community 66`** (1 nodes): `Sprite: spritesheet_species_6.webp`
  Too small to be a meaningful cluster - may be noise or needs more connections extracted.
- **Thin community `Community 67`** (1 nodes): `Sprite: spritesheet_species_9.webp`
  Too small to be a meaningful cluster - may be noise or needs more connections extracted.
- **Thin community `Community 68`** (1 nodes): `Sprite: spritesheet_species_5.webp`
  Too small to be a meaningful cluster - may be noise or needs more connections extracted.
- **Thin community `Community 69`** (1 nodes): `Sprite: spritesheet_species_0.webp`
  Too small to be a meaningful cluster - may be noise or needs more connections extracted.
- **Thin community `Community 70`** (1 nodes): `Sprite: spritesheet_species_1.webp`
  Too small to be a meaningful cluster - may be noise or needs more connections extracted.
- **Thin community `Community 71`** (1 nodes): `Sprite: 249.png`
  Too small to be a meaningful cluster - may be noise or needs more connections extracted.
- **Thin community `Community 72`** (1 nodes): `Doc: README`
  Too small to be a meaningful cluster - may be noise or needs more connections extracted.
- **Thin community `Community 73`** (1 nodes): `Doc: PkmConvertService`
  Too small to be a meaningful cluster - may be noise or needs more connections extracted.
- **Thin community `Community 74`** (1 nodes): `Doc: ARCHITECTURE`
  Too small to be a meaningful cluster - may be noise or needs more connections extracted.
- **Thin community `Community 75`** (1 nodes): `Doc: DATA`
  Too small to be a meaningful cluster - may be noise or needs more connections extracted.
- **Thin community `Community 76`** (1 nodes): `Doc: SESSION`
  Too small to be a meaningful cluster - may be noise or needs more connections extracted.

## Suggested Questions
_Questions this graph is uniquely positioned to answer:_

- **Why does `DexMainService` connect `Pokedex Generation` to `Data Actions`, `Auth System`?**
  _High betweenness centrality (0.026) - this node is a cross-community bridge._
- **Why does `FileIOService` connect `Backup Routes` to `File I/O`?**
  _High betweenness centrality (0.026) - this node is a cross-community bridge._
- **Why does `PkmVariantLoader` connect `Auth System` to `Data Actions`, `Legacy Data Loading`?**
  _High betweenness centrality (0.022) - this node is a cross-community bridge._
- **What connects `MainCreatePkmVariantActionInput`, `PkmFileEntity`, `MetaEntity` to the rest of the system?**
  _47 weakly-connected nodes found - possible documentation gaps or missing edges._
- **Should `Data Actions` be split into smaller, more focused modules?**
  _Cohesion score 0.04 - nodes in this community are weakly interconnected._
- **Should `Backup Routes` be split into smaller, more focused modules?**
  _Cohesion score 0.04 - nodes in this community are weakly interconnected._
- **Should `Auth System` be split into smaller, more focused modules?**
  _Cohesion score 0.03 - nodes in this community are weakly interconnected._