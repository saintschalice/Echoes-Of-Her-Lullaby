# Simple Fix: Load Foyer First, Then Persistent Scene

## Problema
Nakikita si Lisa bago mag-start ang cutscene kasi PersistentScene ang unang nag-load.

## Simpleng Solusyon
**I-load muna ang Room01_Foyer, tapos saka lang ang PersistentScene!**

---

## Unity Setup (SIMPLE!)

### Step 1: Change Build Settings Scene Order
1. Go to **File → Build Settings**
2. Reorder scenes:
   ```
   ✅ Scene 0: MainMenu
   ✅ Scene 1: Room01_Foyer  ← FOYER MUNA!
   ✅ Scene 2: PersistentScene  ← PERSISTENT KASUNOD
   ✅ Scene 3: Room02_LivingRoom
   ✅ Scene 4: Room03_Hallway
   ... (other rooms)
   ```

### Step 2: Modify MainMenu to Load Foyer First
Kailangan i-update ang MainMenu script para mag-load ng Room01_Foyer instead of PersistentScene.

**Current (Wrong):**
```csharp
// MainMenu loads PersistentScene first
SceneManager.LoadScene("PersistentScene");
```

**New (Correct):**
```csharp
// MainMenu loads Room01_Foyer first
SceneManager.LoadScene("Room01_Foyer");
```

### Step 3: Load PersistentScene from Foyer
Sa `FoyerIntroController.cs`, i-load ang PersistentScene as additive AFTER cutscene setup.

---

## Code Changes Needed

### Option A: Load PersistentScene Additively (RECOMMENDED)

#### FoyerIntroController.cs
```csharp
void Awake()
{
    // Black screen setup
    if (blackoutCanvasGroup != null)
    {
        blackoutCanvasGroup.alpha = 1f;
        blackoutCanvasGroup.gameObject.SetActive(true);
    }

    // Ensure cutscene starts disabled
    if (cutsceneObject != null)
    {
        cutsceneObject.SetActive(false);
    }

    // Load PersistentScene additively (Lisa will spawn here)
    StartCoroutine(LoadPersistentSceneAdditive());
}

IEnumerator LoadPersistentSceneAdditive()
{
    // Load PersistentScene in background
    AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("PersistentScene", LoadSceneMode.Additive);
    
    // Wait for it to load
    while (!asyncLoad.isDone)
    {
        yield return null;
    }
    
    Debug.Log("[FoyerIntro] PersistentScene loaded additively");
    
    // NOW Lisa exists, we can hide her for cutscene
    GameObject lisa = GameObject.FindGameObjectWithTag("Player");
    if (lisa != null)
    {
        lisa.SetActive(false);
        Debug.Log("[FoyerIntro] Lisa hidden for cutscene");
    }
}
```

#### After Cutscene Ends
```csharp
public void FinishIntro()
{
    manualFinishTriggered = true;
    Debug.Log("[FoyerIntro] Cutscene finished");
    
    // Show Lisa
    GameObject lisa = GameObject.FindGameObjectWithTag("Player");
    if (lisa != null)
    {
        lisa.SetActive(true);
        Debug.Log("[FoyerIntro] Lisa shown after cutscene");
    }
    
    DisableBlackout();
}
```

---

## How It Works

### New Game Flow (NEW APPROACH)
```
MainMenu
    ↓
Load Room01_Foyer (FIRST!)
    ↓
FoyerIntroController.Awake()
    ↓
Black screen active (Lisa doesn't exist yet)
    ↓
Load PersistentScene additively
    ↓
Lisa spawns (but immediately hidden by FoyerIntroController)
    ↓
Cutscene plays
    ↓
Cutscene ends → Show Lisa
    ↓
Game starts!
```

### Load Game Flow
```
MainMenu
    ↓
Load saved scene directly (with PersistentScene)
    ↓
Lisa visible immediately
    ↓
No cutscene
```

---

## Advantages

✅ **Simpler** - No complex PlayerPrefs detection  
✅ **Guaranteed** - Lisa doesn't exist until we want her to  
✅ **Clean** - Foyer controls its own intro  
✅ **No flicker** - Black screen is up before Lisa spawns  

---

## Alternative: Even Simpler Approach

### Just Disable Lisa in PersistentScene Inspector

**Simplest Solution:**
1. Open **PersistentScene**
2. Find **Lisa** GameObject
3. **Uncheck** the checkbox next to her name (disable her)
4. In `FoyerIntroController.cs`, enable her after cutscene:

```csharp
public void FinishIntro()
{
    GameObject lisa = GameObject.FindGameObjectWithTag("Player");
    if (lisa != null)
    {
        lisa.SetActive(true);
        Debug.Log("[FoyerIntro] Lisa enabled after cutscene");
    }
}
```

**For Load Game:**
In `SaveSystem.cs` or wherever you load a game:
```csharp
public void LoadGame(int slot)
{
    // ... existing load code ...
    
    // Enable Lisa for load game
    GameObject lisa = GameObject.FindGameObjectWithTag("Player");
    if (lisa != null)
    {
        lisa.SetActive(true);
    }
}
```

---

## Which Approach to Use?

### 🥇 BEST: Disable Lisa in Inspector (Simplest!)
- Disable Lisa GameObject in PersistentScene
- Enable her after cutscene in FoyerIntroController
- Enable her when loading game in SaveSystem

### 🥈 GOOD: Load Foyer First, Then Persistent Additive
- Change scene load order
- Load PersistentScene additively from Foyer
- Hide Lisa immediately after spawn

### 🥉 CURRENT: PersistentSpawnManager Detection
- Already implemented
- Works but more complex
- Requires PlayerPrefs coordination

---

## Recommendation

**Gawin mo yung pinaka-simple:**

1. **Disable Lisa sa PersistentScene Inspector**
2. **Enable her sa FoyerIntroController after cutscene**
3. **Enable her sa SaveSystem when loading game**

**Tapos na!** No complex code needed! 😊

---

## Quick Implementation (Simplest Way)

### Step 1: Unity Inspector
1. Open PersistentScene
2. Find Lisa GameObject
3. Uncheck to disable her

### Step 2: FoyerIntroController.cs
Add this to `FinishIntro()`:
```csharp
public void FinishIntro()
{
    manualFinishTriggered = true;
    
    // Enable Lisa after cutscene
    GameObject lisa = GameObject.FindGameObjectWithTag("Player");
    if (lisa != null && !lisa.activeSelf)
    {
        lisa.SetActive(true);
        Debug.Log("[FoyerIntro] Lisa enabled after cutscene");
    }
}
```

### Step 3: For Load Game
In `CheckAndPlayCutsceneRoutine()`, add:
```csharp
if (hasSeenCutscene)
{
    // Load game - enable Lisa immediately
    GameObject lisa = GameObject.FindGameObjectWithTag("Player");
    if (lisa != null && !lisa.activeSelf)
    {
        lisa.SetActive(true);
        Debug.Log("[FoyerIntro] Lisa enabled for load game");
    }
    
    yield return StartCoroutine(FadeInRoom());
}
```

**DONE!** Super simple! 🎉

---

## Which One Do You Want?

1. **Simplest** - Just disable Lisa in Inspector ← RECOMMENDED
2. **Scene Order** - Load Foyer first, then Persistent additive
3. **Keep Current** - Use PersistentSpawnManager detection (already done)

Sabihin mo lang kung alin gusto mo, i-implement ko! 😊
