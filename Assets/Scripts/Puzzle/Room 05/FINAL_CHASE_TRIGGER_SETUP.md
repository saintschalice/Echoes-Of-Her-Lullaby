# Final Chase Trigger Setup Guide

## 📋 Overview

Yung `Trigger_FinalEmilyChase` GameObject ay dapat **naka-disable** sa simula, tapos mag-activate lang pag **tapos na lahat ng puzzle**.

---

## 🔧 Unity Setup

### Step 1: Prepare Final Chase Trigger

1. Select `Trigger_FinalEmilyChase` GameObject
2. **UNCHECK** ang checkbox sa Inspector (disable it)
3. Make sure may `TriggerFinalChase` script component

### Step 2: Assign to Room Controller

1. Select `Room05_DiningRoomController` GameObject
2. Find **"Final Chase Trigger"** section
3. Drag `Trigger_FinalEmilyChase` GameObject to **"Final Chase Trigger"** field

---

## 🎮 How It Works

### Flow:

1. **Game Start:**
   - `Trigger_FinalEmilyChase` = **DISABLED** (naka-off)
   - Player can't trigger final chase yet

2. **During Puzzle:**
   - Player solves cabinet puzzle
   - Player gets spoon
   - Player fixes 3 chairs
   - Player places spoon on table
   - First chase starts (calendar trigger)

3. **Player Hides Under Table:**
   - Emily disappears
   - Puzzle marked as **COMPLETE**
   - `puzzleCompleted = true`

4. **Final Chase Trigger Activates:**
   - `Trigger_FinalEmilyChase` = **ENABLED** (naka-on na!)
   - Player can now trigger final chase

5. **Player Walks to Exit:**
   - Player enters `Trigger_FinalEmilyChase` zone
   - Final chase starts!
   - Emily spawns faster and more aggressive

---

## 📝 Code Changes

### Room05_DiningRoomController.cs

**Added:**
```csharp
[Header("Final Chase Trigger")]
[Tooltip("Trigger GameObject na mag-activate pag tapos na ang puzzle")]
public GameObject finalChaseTrigger;
```

**In Start():**
```csharp
// Disable final chase trigger initially
if (finalChaseTrigger != null) finalChaseTrigger.SetActive(false);
```

**In EmilyDisappearsSequence():**
```csharp
// ACTIVATE FINAL CHASE TRIGGER (pag tapos na ang puzzle)
if (finalChaseTrigger != null)
{
    finalChaseTrigger.SetActive(true);
    Debug.Log("[Room05] Final Chase Trigger activated - puzzle complete!");
}
```

---

## ✅ Testing Checklist

### Initial State:
- [ ] `Trigger_FinalEmilyChase` is **DISABLED** in Hierarchy
- [ ] `Trigger_FinalEmilyChase` is assigned to Room Controller

### During Gameplay:
- [ ] Start game → Final chase trigger is **OFF**
- [ ] Solve puzzle → First chase starts
- [ ] Hide under table → Emily disappears
- [ ] Check Console → "Final Chase Trigger activated" message
- [ ] Check Hierarchy → `Trigger_FinalEmilyChase` is now **ENABLED**
- [ ] Walk to exit → Final chase triggers!

---

## 🐛 Troubleshooting

### Final chase triggers too early
**Problem:** Trigger activates before puzzle is done
**Solution:**
- Check kung naka-disable ang `Trigger_FinalEmilyChase` sa start
- Check kung naka-assign sa Room Controller

### Final chase never triggers
**Problem:** Trigger doesn't activate after puzzle
**Solution:**
- Check kung naka-assign ang `finalChaseTrigger` sa Room Controller
- Check Console for "Final Chase Trigger activated" message
- Check kung `puzzleCompleted = true` after hiding

### Trigger is visible in Hierarchy but not working
**Problem:** GameObject is enabled but trigger doesn't work
**Solution:**
- Check kung may `TriggerFinalChase` script component
- Check kung naka-check ang "Is Trigger" sa collider
- Check kung tama ang position ng trigger zone

---

## 🎯 Inspector Setup

### Room05_DiningRoomController:

```
Hunting System:
├─ Is Emily Hunting: ☐
├─ Puzzle Completed: ☐
├─ Emily Angry Spawn Point: (Transform)
├─ Emily Final Chase Spawn Point: (Transform)
├─ Initial Chase Speed: 3.5
└─ Final Chase Speed: 5.5

Final Chase Trigger: ⭐ NEW!
└─ Final Chase Trigger: Trigger_FinalEmilyChase
```

### Trigger_FinalEmilyChase:

```
GameObject: Trigger_FinalEmilyChase
Active: ☐ (DISABLED at start!)

Components:
├─ Transform
├─ Box Collider 2D (Is Trigger: ☑)
└─ TriggerFinalChase (Script)
```

---

## 💡 Why This Design?

### Problem:
- Kung naka-enable ang final chase trigger from start, pwedeng ma-trigger ng player kahit hindi pa tapos ang puzzle
- This breaks the game flow

### Solution:
- Disable trigger at start
- Enable trigger only after puzzle complete
- Ensures proper sequence: First chase → Hide → Puzzle complete → Final chase

### Benefits:
- ✅ Prevents early trigger
- ✅ Ensures correct flow
- ✅ Player must complete puzzle first
- ✅ Clean and simple logic

---

## 🎮 Game Flow Summary

```
[Start]
   ↓
Final Chase Trigger = DISABLED
   ↓
Solve Cabinet Puzzle
   ↓
Get Spoon
   ↓
Fix 3 Chairs
   ↓
Place Spoon on Table
   ↓
First Chase Starts (Calendar Trigger)
   ↓
Hide Under Table
   ↓
Emily Disappears
   ↓
Puzzle Complete!
   ↓
Final Chase Trigger = ENABLED ⭐
   ↓
Walk to Exit
   ↓
Final Chase Triggers!
   ↓
Emily Chases (Faster & More Aggressive)
   ↓
[Escape or Game Over]
```

---

## 📋 Quick Setup Checklist

1. [ ] Disable `Trigger_FinalEmilyChase` in Hierarchy
2. [ ] Assign `Trigger_FinalEmilyChase` to Room Controller
3. [ ] Test: Start game → Trigger is OFF
4. [ ] Test: Complete puzzle → Trigger turns ON
5. [ ] Test: Walk to exit → Final chase starts

---

## ✅ Done!

**Setup complete!** Final chase trigger will now activate only after puzzle is complete! 🎮✨

---

## 🔍 Debug Tips

### Check if trigger activated:
```csharp
// In Console, look for:
[Room05] Final Chase Trigger activated - puzzle complete!
```

### Check trigger state in Hierarchy:
- Before puzzle: `Trigger_FinalEmilyChase` (grayed out = disabled)
- After puzzle: `Trigger_FinalEmilyChase` (normal = enabled)

### Manual test:
```csharp
// In Room05_DiningRoomController, add to Update():
if (Input.GetKeyDown(KeyCode.T))
{
    if (finalChaseTrigger != null)
    {
        finalChaseTrigger.SetActive(!finalChaseTrigger.activeSelf);
        Debug.Log($"Final Chase Trigger: {finalChaseTrigger.activeSelf}");
    }
}
```

**Press T to toggle trigger on/off for testing!**

---

**Everything is ready!** 💪✨
