# Room 09 - Scene Transition Guide

## 🎯 Overview

Room 09 uses simple scene transition - walang spawn point script needed. Just like other rooms, direct `SceneManager.LoadScene()` lang.

---

## 🔗 From Room 08 to Room 09

### Room 08 (Room8Manager.cs):

```csharp
private void FinishQTE()
{
    isQTEActive = false;
    qteButtonObject.SetActive(false);
    Debug.Log("Mirror Shatters! Lumilipat na sa Room 9...");
    SceneManager.LoadScene("Room09_Master's_Bathroom");
}
```

**That's it!** Simple lang - pag nabasag yung mirror, load lang ng Room 09.

---

## 🎮 Room 09 Setup

### No Spawn Point Script Needed!

Unity automatically handles player position:
- Player GameObject stays in the scene
- Position is maintained or reset based on your setup

### If You Want Specific Position:

**Option 1: Set Player Position in Scene**
1. Open Room 09 scene
2. Find Player GameObject
3. Set Transform position where you want Lisa to start
4. Save scene

**Option 2: Use PlayerSpawnPoint (if you have one)**
1. Create empty GameObject: `PlayerSpawnPoint`
2. Position it where Lisa should start
3. Tag it as "PlayerSpawnPoint"
4. Player will automatically move there (if you have spawn logic in Player script)

---

## 🎬 Entry Sequence

### Room 09 Flow:

1. **Scene Loads** → Room 09 appears
2. **Player Position** → Lisa is at spawn position (set in scene)
3. **Room09_FlowController.Start()** → Triggers intro sequence:
   - Shows Emily at full power
   - Plays tense music
   - Starts intro dialogue
   - Locks door
4. **Player Can Move** → After intro dialogue

---

## 📝 Room 09 Scene Setup

### Required GameObjects:

1. **Player** (should be in scene or persistent)
2. **Room09_FlowController** (handles intro and flow)
3. **Emily_Manifestation** (Emily sprite)
4. **4 Mirror GameObjects** (interactable)
5. **Canvas** (for UI panels)

### Player Position:

Set Player GameObject position in scene:
```
Position: (0, -2, 0) ← Adjust based on your layout
```

---

## 🎯 Testing

### Test 1: Scene Transition
1. Play Room 08
2. Complete QTE (break mirror)
3. **Expected**: Room 09 loads
4. **Expected**: Lisa appears in Room 09

### Test 2: Intro Sequence
1. Room 09 loads
2. **Expected**: Emily appears
3. **Expected**: Intro dialogue plays
4. **Expected**: Player can move after dialogue

---

## 💡 Simple Setup

**No special spawn script needed!**

Just:
1. Set Player position in Room 09 scene
2. Room08 loads Room 09 with `SceneManager.LoadScene()`
3. Room09_FlowController handles intro
4. Done!

---

## 🔧 If You Want Custom Entry Effects

### Option 1: In Room09_FlowController.Start()

```csharp
void Start()
{
    // Play entry effects
    PlayGlassBreakSound();
    ShowBloodEffects();
    
    // Show Emily
    if (emilyManifestation != null) emilyManifestation.SetActive(true);
    
    // Start intro
    Invoke(nameof(PlayIntro), 1f);
}
```

### Option 2: Separate Entry Script

Create `Room09_EntryEffects.cs`:
```csharp
public class Room09_EntryEffects : MonoBehaviour
{
    public AudioClip glassBreakSound;
    public GameObject bloodEffect;
    
    void Start()
    {
        // Play effects
        AudioManager.Instance?.PlaySFX(glassBreakSound);
        if (bloodEffect != null) bloodEffect.SetActive(true);
    }
}
```

---

## 🎯 Summary

**Simple Scene Transition**:
1. Room 08: `SceneManager.LoadScene("Room09_Master's_Bathroom")`
2. Room 09: Player appears at position set in scene
3. Room09_FlowController: Handles intro sequence
4. Done!

**No spawn point script needed!** ✅

---

## 📚 Related Files

- `Room8Manager.cs` - Handles Room 08 QTE and transition
- `Room09_FlowController.cs` - Handles Room 09 intro and flow
- `Room09_Dialogues.cs` - All Room 09 dialogues

---

**Yan lang!** Simple lang tulad ng ibang rooms! 🎯✨
