# Update Scripts for Jumpscare - Quick Reference

## 🎯 SIMPLE RULE

**Replace this**:
```csharp
GameOverManager.Instance?.TriggerGameOver("message");
```

**With this**:
```csharp
JumpscareManager.Instance?.TriggerJumpscare("message");
```

---

## 📝 SCRIPTS TO UPDATE

### 1. Room06_HallwayController.cs

**Location**: `Assets/Scripts/Puzzle/Room 06/Room06_HallwayController.cs`

**Find** (around line 438):
```csharp
private void TriggerGameOver()
{
    if (!isEmilyHunting) return;
    isEmilyHunting = false;
    
    if (debugMode) Debug.Log("[Room06] Emily caught player - Game Over!");
    
    // ... close panels ...
    
    GameOverManager gameOverManager = FindFirstObjectByType<GameOverManager>();
    if (gameOverManager != null)
    {
        gameOverManager.TriggerGameOver("Emily caught you...");
    }
}
```

**Replace with**:
```csharp
private void TriggerGameOver()
{
    if (!isEmilyHunting) return;
    isEmilyHunting = false;
    
    if (debugMode) Debug.Log("[Room06] Emily caught player - Game Over!");
    
    // CRITICAL: Close photo panel before game over
    if (photoPanel != null && photoPanel.activeSelf)
    {
        photoPanel.SetActive(false);
        if (debugMode) Debug.Log("[Room06] Photo panel closed before game over");
    }
    
    // Stop Emily movement
    if (emilyGameObject != null)
    {
        Rigidbody2D emilyRb = emilyGameObject.GetComponent<Rigidbody2D>();
        if (emilyRb != null)
        {
            emilyRb.linearVelocity = Vector2.zero;
        }
    }
    
    // Trigger jumpscare + game over
    if (JumpscareManager.Instance != null)
    {
        JumpscareManager.Instance.TriggerJumpscare("Emily caught you...");
    }
    else
    {
        // Fallback to direct game over
        GameOverManager.Instance?.TriggerGameOver("Emily caught you...");
    }
}
```

---

### 2. CinematicChaseTrigger.cs

**Location**: `Assets/Scripts/Puzzle/Room 05/CinematicChaseTrigger.cs`

**Find** (around line 298):
```csharp
private void TriggerGameOver()
{
    if (!isChasing) return;
    isChasing = false;
    
    if (debugMode) Debug.Log("[CinematicChase] Emily caught player! Triggering Game Over.");
    
    // Stop Emily movement
    // ...
    
    GameOverManager gameOverManager = FindFirstObjectByType<GameOverManager>();
    if (gameOverManager != null)
    {
        gameOverManager.TriggerGameOver(gameOverMessage);
    }
    else
    {
        Debug.LogError("[CinematicChase] GameOverManager not found! Cannot trigger Game Over.");
    }
}
```

**Replace with**:
```csharp
private void TriggerGameOver()
{
    if (!isChasing) return;
    isChasing = false;
    
    if (debugMode) Debug.Log("[CinematicChase] Emily caught player! Triggering Game Over.");
    
    // Stop Emily movement
    if (emilyGameObject != null)
    {
        Rigidbody2D emilyRb = emilyGameObject.GetComponent<Rigidbody2D>();
        if (emilyRb != null)
        {
            emilyRb.linearVelocity = Vector2.zero;
        }
        
        EmilyGhost emilyScript = emilyGameObject.GetComponent<EmilyGhost>();
        if (emilyScript != null)
        {
            emilyScript.isPaused = true;
        }
    }
    
    // Trigger jumpscare + game over
    if (JumpscareManager.Instance != null)
    {
        JumpscareManager.Instance.TriggerJumpscare(gameOverMessage);
    }
    else
    {
        // Fallback to direct game over
        GameOverManager.Instance?.TriggerGameOver(gameOverMessage);
    }
}
```

---

### 3. Room08_MirrorQTE.cs

**Location**: `Assets/Scripts/Puzzle/Room 08/Room08_MirrorQTE.cs`

**Find** (around line 243):
```csharp
System.Collections.IEnumerator GameOver()
{
    isQTEActive = false;
    
    // Show failure dialogue
    DialogueSystemV2.Instance?.StartDialogue(Room08_Dialogues.QTE_FAILED, "Lisa");
    
    while (DialogueSystemV2.Instance != null && DialogueSystemV2.Instance.IsDialogueActive())
    {
        yield return null;
    }
    
    yield return new WaitForSeconds(1f);
    
    // Close panel
    Room08UIManager uiManager = FindFirstObjectByType<Room08UIManager>();
    if (uiManager != null)
    {
        uiManager.HideAllPanels();
    }
    
    // Re-enable player
    EnablePlayer();
    
    // Trigger game over sequence
    Debug.Log("[Room08] QTE Failed - Game Over!");
    // TODO: Trigger actual game over
}
```

**Replace with**:
```csharp
System.Collections.IEnumerator GameOver()
{
    isQTEActive = false;
    
    // Show failure dialogue
    DialogueSystemV2.Instance?.StartDialogue(Room08_Dialogues.QTE_FAILED, "Lisa");
    
    while (DialogueSystemV2.Instance != null && DialogueSystemV2.Instance.IsDialogueActive())
    {
        yield return null;
    }
    
    yield return new WaitForSeconds(0.5f);
    
    // Close panel
    Room08UIManager uiManager = FindFirstObjectByType<Room08UIManager>();
    if (uiManager != null)
    {
        uiManager.HideAllPanels();
    }
    
    // DON'T re-enable player - jumpscare will handle it
    
    // Trigger jumpscare + game over
    if (JumpscareManager.Instance != null)
    {
        JumpscareManager.Instance.TriggerJumpscare("Time ran out...");
    }
    else
    {
        // Fallback to direct game over
        EnablePlayer(); // Only enable if no jumpscare
        GameOverManager.Instance?.TriggerGameOver("Time ran out...");
    }
}
```

---

### 4. Mirror1_MedicineCabinet.cs (Room 09)

**Location**: `Assets/Scripts/Puzzle/Room 09/Mirror1_MedicineCabinet.cs`

**Find** (around line 438):
```csharp
System.Collections.IEnumerator TriggerGameOver()
{
    // ... dialogues ...
    
    yield return new WaitForSeconds(1f);
    
    // Game over - reload scene or checkpoint
    UnityEngine.SceneManagement.SceneManager.LoadScene(
        UnityEngine.SceneManagement.SceneManager.GetActiveScene().name
    );
}
```

**Replace with**:
```csharp
System.Collections.IEnumerator TriggerGameOver()
{
    // Show Emily attack dialogues
    DialogueSystemV2.Instance?.StartDialogue(Room09_Dialogues.EMILY_ATTACK_1, "Lisa");
    while (DialogueSystemV2.Instance != null && DialogueSystemV2.Instance.IsDialogueActive())
    {
        yield return null;
    }
    
    yield return new WaitForSeconds(0.3f);
    
    DialogueSystemV2.Instance?.StartDialogue(Room09_Dialogues.EMILY_ATTACK_2, "Lisa");
    while (DialogueSystemV2.Instance != null && DialogueSystemV2.Instance.IsDialogueActive())
    {
        yield return null;
    }
    
    yield return new WaitForSeconds(0.5f);
    
    // Trigger jumpscare + game over
    if (JumpscareManager.Instance != null)
    {
        JumpscareManager.Instance.TriggerJumpscare("Emily caught you...");
    }
    else
    {
        // Fallback to direct game over
        GameOverManager.Instance?.TriggerGameOver("Emily caught you...");
    }
}
```

---

### 5. Mirror2_BathtubDrain.cs (Room 09)

**Location**: `Assets/Scripts/Puzzle/Room 09/Mirror2_BathtubDrain.cs`

**Find** (around line 351):
```csharp
System.Collections.IEnumerator TriggerGameOver()
{
    // ... dialogues ...
    
    yield return new WaitForSeconds(1f);
    
    // Game over - reload scene
    UnityEngine.SceneManagement.SceneManager.LoadScene(
        UnityEngine.SceneManagement.SceneManager.GetActiveScene().name
    );
}
```

**Replace with**:
```csharp
System.Collections.IEnumerator TriggerGameOver()
{
    // Show Emily attack dialogues
    DialogueSystemV2.Instance?.StartDialogue(Room09_Dialogues.EMILY_ATTACK_1, "Lisa");
    while (DialogueSystemV2.Instance != null && DialogueSystemV2.Instance.IsDialogueActive())
    {
        yield return null;
    }
    
    yield return new WaitForSeconds(0.3f);
    
    DialogueSystemV2.Instance?.StartDialogue(Room09_Dialogues.EMILY_ATTACK_2, "Lisa");
    while (DialogueSystemV2.Instance != null && DialogueSystemV2.Instance.IsDialogueActive())
    {
        yield return null;
    }
    
    yield return new WaitForSeconds(0.5f);
    
    // Trigger jumpscare + game over
    if (JumpscareManager.Instance != null)
    {
        JumpscareManager.Instance.TriggerJumpscare("Emily caught you...");
    }
    else
    {
        // Fallback to direct game over
        GameOverManager.Instance?.TriggerGameOver("Emily caught you...");
    }
}
```

---

### 6. Mirror3_VanityTerror.cs (Room 09)

**Location**: `Assets/Scripts/Puzzle/Room 09/Mirror3_VanityTerror.cs`

**Find** (around line 469):
```csharp
System.Collections.IEnumerator TriggerGameOver()
{
    // ... dialogues ...
    
    yield return new WaitForSeconds(1f);
    
    // Game over - reload scene
    UnityEngine.SceneManagement.SceneManager.LoadScene(
        UnityEngine.SceneManagement.SceneManager.GetActiveScene().name
    );
}
```

**Replace with**:
```csharp
System.Collections.IEnumerator TriggerGameOver()
{
    // Show Emily attack dialogues
    DialogueSystemV2.Instance?.StartDialogue(Room09_Dialogues.EMILY_ATTACK_1, "Lisa");
    while (DialogueSystemV2.Instance != null && DialogueSystemV2.Instance.IsDialogueActive())
    {
        yield return null;
    }
    
    yield return new WaitForSeconds(0.3f);
    
    DialogueSystemV2.Instance?.StartDialogue(Room09_Dialogues.EMILY_ATTACK_2, "Lisa");
    while (DialogueSystemV2.Instance != null && DialogueSystemV2.Instance.IsDialogueActive())
    {
        yield return null;
    }
    
    yield return new WaitForSeconds(0.5f);
    
    // Trigger jumpscare + game over
    if (JumpscareManager.Instance != null)
    {
        JumpscareManager.Instance.TriggerJumpscare("Emily caught you...");
    }
    else
    {
        // Fallback to direct game over
        GameOverManager.Instance?.TriggerGameOver("Emily caught you...");
    }
}
```

---

## ✅ CHECKLIST

Update these files:
- [ ] `Room06_HallwayController.cs`
- [ ] `CinematicChaseTrigger.cs`
- [ ] `Room08_MirrorQTE.cs`
- [ ] `Mirror1_MedicineCabinet.cs`
- [ ] `Mirror2_BathtubDrain.cs`
- [ ] `Mirror3_VanityTerror.cs`

Optional (if may iba pang game over triggers):
- [ ] `Room05_CinematicChaseTrigger.cs`
- [ ] `TriggerFinalChase.cs`
- [ ] Any other custom game over scripts

---

## 🔍 HOW TO FIND ALL GAME OVER TRIGGERS

### Method 1: Search in Files
1. Open Visual Studio or your code editor
2. Press `Ctrl+Shift+F` (Find in Files)
3. Search for: `TriggerGameOver`
4. Update all results

### Method 2: Search in Unity
1. In Unity, press `Ctrl+Shift+F`
2. Search for: `GameOverManager`
3. Check all scripts that use it

---

## 💡 PATTERN TO FOLLOW

### Standard Pattern:
```csharp
// 1. Stop gameplay
DisablePlayer();
StopEnemies();

// 2. Show dialogues (optional)
yield return ShowDialogues();

// 3. Wait a bit
yield return new WaitForSeconds(0.5f);

// 4. Trigger jumpscare
if (JumpscareManager.Instance != null)
{
    JumpscareManager.Instance.TriggerJumpscare("message");
}
else
{
    GameOverManager.Instance?.TriggerGameOver("message");
}
```

---

## 🐛 COMMON MISTAKES

### Mistake 1: Enabling player before jumpscare
```csharp
// ❌ WRONG
EnablePlayer();
JumpscareManager.Instance?.TriggerJumpscare("message");
```

```csharp
// ✅ CORRECT
// Don't enable player - jumpscare handles it
JumpscareManager.Instance?.TriggerJumpscare("message");
```

---

### Mistake 2: Not providing fallback
```csharp
// ❌ WRONG
JumpscareManager.Instance.TriggerJumpscare("message"); // Crashes if null!
```

```csharp
// ✅ CORRECT
if (JumpscareManager.Instance != null)
{
    JumpscareManager.Instance.TriggerJumpscare("message");
}
else
{
    GameOverManager.Instance?.TriggerGameOver("message");
}
```

---

### Mistake 3: Reloading scene directly
```csharp
// ❌ WRONG
SceneManager.LoadScene(SceneManager.GetActiveScene().name);
```

```csharp
// ✅ CORRECT
JumpscareManager.Instance?.TriggerJumpscare("message");
// Jumpscare will show game over, which has retry button
```

---

## 📝 TESTING CHECKLIST

After updating all scripts:
- [ ] Test Emily catches player in Room 06
- [ ] Test Emily catches player in Room 05 (chase)
- [ ] Test Room 08 mirror QTE failure
- [ ] Test Room 09 Mirror 1 failure
- [ ] Test Room 09 Mirror 2 failure
- [ ] Test Room 09 Mirror 3 failure
- [ ] Verify jumpscare plays in all scenarios
- [ ] Verify game over screen shows after jumpscare
- [ ] Verify retry button works

---

**Update complete! All game overs now have jumpscare!** 👻✨
