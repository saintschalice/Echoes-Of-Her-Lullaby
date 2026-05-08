# Fix para sa Lisa Visibility sa New Game - TAPOS NA

## Problema
Pag nag-click ng "New Game" sa main menu, nakikita agad si Lisa ng sandali bago mag-start yung intro cutscene. Nangyayari ito dahil:
1. PersistentScene ang unang nag-load at nag-spawn kay Lisa
2. Room01_Foyer scene ang pangalawa
3. FoyerIntroController sinusubukan i-hide si Lisa, pero nakita na siya

## Solusyon
Gumawa ng two-part system kung saan ang **PersistentSpawnManager** ang nag-hide kay Lisa sa simula, at ang **FoyerIntroController** ang nagpapakita kay Lisa pagkatapos ng cutscene.

---

## Mga Pagbabago

### 1. PersistentSpawnManager.cs
**Lokasyon**: `Assets/Scripts/Player/PersistentSpawnManager.cs`

#### Dinagdag na Setting
```csharp
[Header("New Game Settings")]
public bool hidePlayerOnNewGame = true;
```

#### Binago ang Start() Method
- Tinitignan kung new game ba ito gamit ang `PlayerPrefs.GetInt("LoadSlotOnStart")`
- Kung `LoadSlotOnStart == -1`, NEW GAME ito → I-hide si Lisa
- Kung `LoadSlotOnStart >= 0`, LOAD GAME ito → Ipakita si Lisa
- Nangyayari ito BAGO pa mag-load ang kahit anong scene

#### Dinagdag na Public Methods
```csharp
// Para ipakita si Lisa (tinatawag ng FoyerIntroController)
public void EnablePlayer()

// Para i-hide si Lisa (kung kailangan)
public void DisablePlayer()
```

---

### 2. FoyerIntroController.cs
**Lokasyon**: `Assets/Scripts/Puzzle/Room 01/FoyerIntroController.cs`

#### Tinanggal ang Local Player Management
- Tinanggal ang sariling code para i-hide/show si Lisa
- Lahat ng player visibility ay hawak na ng PersistentSpawnManager

#### Binago ang Logic
**Para sa Load Game:**
```csharp
// Ipakita si Lisa agad
PersistentSpawnManager.Instance.EnablePlayer();
```

**Para sa New Game:**
```csharp
// Ipakita si Lisa pagkatapos ng cutscene
PersistentSpawnManager.Instance.EnablePlayer();
```

---

## Paano Gumagana

### New Game Flow
1. **MainMenu** nag-set ng `PlayerPrefs.SetInt("LoadSlotOnStart", -1)`
2. **PersistentScene** nag-load → PersistentSpawnManager nag-spawn kay Lisa
3. **PersistentSpawnManager.Start()** nakita na `-1` → I-HIDE si Lisa AGAD
4. **Room01_Foyer** nag-load → Black screen lang makikita
5. **FoyerIntroController** nag-play ng cutscene
6. **Pagkatapos ng cutscene** → Tinatawag ang `EnablePlayer()`
7. **Lisa lumabas** (walang flicker!)

### Load Game Flow
1. **MainMenu** nag-set ng `PlayerPrefs.SetInt("LoadSlotOnStart", slotNumber)`
2. **PersistentScene** nag-load → PersistentSpawnManager nag-spawn kay Lisa
3. **PersistentSpawnManager.Start()** nakita na `>= 0` → IPAKITA si Lisa
4. **Room nag-load** → Nakikita na si Lisa (walang cutscene)
5. **FoyerIntroController** alam na nakita na yung cutscene → Fade in lang

---

## Unity Setup (Kailangan Gawin)

### PersistentScene Setup
1. I-select ang **PersistentSpawnManager** GameObject sa PersistentScene
2. Sa Inspector, siguraduhin:
   - `Hide Player On New Game` = **TRUE** (naka-check)
   - `Player` reference ay naka-assign kay Lisa GameObject
   - `Debug Mode` = TRUE (para makita ang logs)

### Room01_Foyer Setup
1. **FoyerIntroController** dapat may:
   - `Cutscene Object` assigned (yung cutscene GameObject)
   - `Blackout Canvas Group` assigned (yung black screen)
   - `Cutscene Save ID` = "IntroCutscene_Played"

2. **Cutscene GameObject** dapat:
   - DISABLED by default sa Inspector
   - Tumatawag ng `FoyerIntroController.FinishIntro()` pag tapos na cutscene

3. **Blackout Canvas Group** dapat:
   - ENABLED by default sa Inspector
   - `Alpha = 1` (fully black)
   - `Blocks Raycasts = true`

---

## Testing Checklist

### Test New Game
- [ ] I-click ang "New Game" sa main menu
- [ ] Black screen agad (walang Lisa)
- [ ] Cutscene nag-play
- [ ] Pagkatapos ng cutscene, lumabas si Lisa
- [ ] Walang flicker o sandaling visibility

### Test Load Game
- [ ] I-click ang "Load Game" sa main menu
- [ ] Nakikita agad si Lisa (walang cutscene)
- [ ] Room nag-fade in normally
- [ ] Pwede agad gumalaw si Lisa

### Debug Logs na Dapat Makita
**New Game:**
```
[PersistentSpawn] NEW GAME detected - Lisa hidden until cutscene ends
[FoyerIntro] First time seeing 'IntroCutscene_Played'. Playing cutscene.
[FoyerIntro] Lisa shown after cutscene via PersistentSpawnManager
[PersistentSpawn] Player enabled (called externally)
```

**Load Game:**
```
[PersistentSpawn] LOAD GAME detected - Lisa visible immediately
[FoyerIntro] Cutscene 'IntroCutscene_Played' already seen. Skipping and fading in.
[PersistentSpawn] Player enabled (called externally)
```

---

## Troubleshooting

### Nakikita pa rin si Lisa sa New Game
1. Check PersistentSpawnManager Inspector: `Hide Player On New Game` dapat TRUE
2. Check Console kung may log: "[PersistentSpawn] NEW GAME detected"
3. Verify na MainMenu nag-set ng `PlayerPrefs.SetInt("LoadSlotOnStart", -1)`
4. Check na may "Player" tag si Lisa GameObject

### Hindi lumalabas si Lisa pagkatapos ng cutscene
1. Check na tumatawag ang cutscene ng `FoyerIntroController.FinishIntro()`
2. Check Console kung may log: "[PersistentSpawn] Player enabled (called externally)"
3. Verify na hindi null ang PersistentSpawnManager.Instance
4. Check na nandoon si Lisa GameObject sa PersistentScene

### Hindi nakikita si Lisa sa Load Game
1. Check Console kung may log: "[PersistentSpawn] LOAD GAME detected"
2. Verify na MainMenu nag-set ng correct slot number (>= 0)
3. Check na tama ang value ng `PlayerPrefs.GetInt("LoadSlotOnStart")`

---

## Bakit Gumagana ang Approach na Ito

### Mga Dahilan
- **Early Detection**: PersistentSpawnManager.Start() tumatakbo BAGO pa mag-load ang Room01_Foyer
- **Centralized Control**: Lahat ng player visibility ay hawak ng PersistentSpawnManager
- **Clear Separation**: PersistentSpawnManager = spawn/visibility, FoyerIntroController = cutscene logic
- **No Race Conditions**: PlayerPrefs flag ay naka-set na ng MainMenu bago mag-load ang kahit anong scene

---

## Mga Related Files
- `Assets/Scripts/Player/PersistentSpawnManager.cs` - Player spawn at visibility management
- `Assets/Scripts/Puzzle/Room 01/FoyerIntroController.cs` - Intro cutscene controller
- `Assets/Scripts/GameManagement/SaveSystem.cs` - Save/load system
- `Assets/Scripts/UI/MainMenuManager.cs` - Main menu

---

## Status
✅ **TAPOS NA** - Fixed na ang Lisa visibility issue para sa new game at load game.

**Petsa**: 2026-05-04
**Tested**: Code complete, ready for Unity testing

---

## Quick Summary (Mabilis na Buod)

**Problema**: Nakikita si Lisa bago mag-start ang cutscene sa new game
**Solusyon**: PersistentSpawnManager nag-hide kay Lisa AGAD pag new game
**Result**: Walang flicker, smooth cutscene experience!

**Kailangan mo lang gawin sa Unity:**
1. I-check ang `Hide Player On New Game` sa PersistentSpawnManager
2. I-assign ang Player reference kay Lisa
3. Test new game at load game

**Tapos na!** 🎉
