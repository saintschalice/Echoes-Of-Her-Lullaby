# Tea Party Puzzle - Troubleshooting Guide

## 🐛 Common Issues & Solutions

---

## Issue 1: Can't Drag the Cup

### Symptoms:
- Click/touch on cup but nothing happens
- Cup doesn't move when dragging

### Solutions:

#### Check 1: Canvas Group
```
Select EmilyCup_Draggable
→ Check if Canvas Group component exists
→ If not, Add Component → Canvas Group

Canvas Group settings:
✓ Interactable (checked)
✓ Block Raycasts (checked)
✗ Ignore Parent Groups (unchecked)
Alpha: 1
```

#### Check 2: EventTrigger
```
The script adds EventTrigger automatically in Start()
But check if it exists:

Select EmilyCup_Draggable
→ Should have EventTrigger component
→ Should have 2 entries: Drag and EndDrag

If missing:
→ Script might not have run
→ Try entering Play Mode again
```

#### Check 3: Raycast Target
```
Select EmilyCup_Draggable
→ Image component
→ Raycast Target: ✓ (checked)
```

#### Check 4: Panel Active
```
Make sure TeaParty_Panel is active
→ Check hierarchy (should not be grayed out)
→ Check Inspector (checkbox at top should be checked)
```

---

## Issue 2: Slot Doesn't Highlight

### Symptoms:
- Slot stays white/normal color
- Doesn't turn yellow when cup is near

### Solutions:

#### Check 1: Slot Highlight Assigned
```
Select TeaParty_Panel
→ TeaPartyPuzzleUI component
→ Visual Feedback section
→ Slot Highlight: Should have EmilyCup_Slot assigned

If empty:
→ Drag EmilyCup_Slot to this field
```

#### Check 2: Colors Different
```
Normal Color: White (255, 255, 255, 255)
Highlight Color: Yellow (255, 255, 0, 255)

Make sure they're DIFFERENT!
If both white, you won't see the change.
```

#### Check 3: Snap Distance
```
Snap Distance: 100 (default)

If too small (e.g., 10):
→ Cup needs to be VERY close to highlight
→ Increase to 100-150

If too large (e.g., 500):
→ Always highlighted
→ Decrease to 50-100
```

#### Check 4: Slot Has Image Component
```
Select EmilyCup_Slot
→ Should have Image component
→ Color should be set (not transparent)
```

---

## Issue 3: Cup Doesn't Snap to Slot

### Symptoms:
- Cup returns to start position instead of snapping
- Cup is close but doesn't snap

### Solutions:

#### Check 1: Snap Distance Too Small
```
Select TeaParty_Panel
→ TeaPartyPuzzleUI
→ Snap Distance: Try 100 or 150

If 50 or less:
→ Too strict, hard to snap
→ Increase to 100
```

#### Check 2: Slot Position
```
Select EmilyCup_Slot
→ Check RectTransform position
→ Make sure it's visible on screen
→ Not off-screen or hidden
```

#### Check 3: Both Are RectTransforms
```
Select EmilyCup_Draggable
→ Should have RectTransform (not Transform)

Select EmilyCup_Slot
→ Should have RectTransform (not Transform)

If using regular Transform:
→ Won't work with UI
→ Use UI Image instead
```

#### Check 4: Distance Calculation
```
Add debug log to see distance:

In OnEndDragCup():
Debug.Log($"Distance: {distance}, Snap: {snapDistance}");

If distance is always > snapDistance:
→ Increase snapDistance
→ Or check positions are correct
```

---

## Issue 4: Panel Doesn't Close After Solving

### Symptoms:
- Cup snaps correctly
- Success sound plays
- But panel stays open

### Solutions:

#### Check 1: Room07UIManager Exists
```
In Hierarchy, find Room07_Manager
→ Should have Room07UIManager component
→ Should have OnTeaPartySolved() method

If missing:
→ Create empty GameObject "Room07_Manager"
→ Add Room07UIManager script
```

#### Check 2: Method Exists
```
Open Room07UIManager.cs
→ Check if OnTeaPartySolved() method exists
→ Should call HideAllPanels()
→ Should set isTeaPartyDone = true
```

#### Check 3: Console Errors
```
Open Console (Ctrl+Shift+C)
→ Check for errors when puzzle completes
→ Fix any errors shown
```

---

## Issue 5: Cup Starts at Wrong Position

### Symptoms:
- Cup appears in wrong place when panel opens
- Cup returns to wrong position when not snapped

### Solutions:

#### Check 1: Set Start Position
```
1. Select EmilyCup_Draggable
2. Position it where you want it to START
3. Script saves this position in Start()
4. Cup returns here if not snapped to slot
```

#### Check 2: Reset on Enable
```
Script resets cup position in OnEnable()
→ Uses cupStartPosition saved in Start()
→ Make sure Start() runs before OnEnable()
```

---

## Issue 6: Cup Disappears

### Symptoms:
- Cup vanishes when dragging
- Can't see cup anymore

### Solutions:

#### Check 1: Canvas Sorting
```
Select Canvas
→ Canvas component
→ Render Mode: Screen Space - Overlay
→ Sort Order: 100 (high number)
```

#### Check 2: Cup Layer
```
Select EmilyCup_Draggable
→ Layer: UI
→ Not Default or other layer
```

#### Check 3: Cup Alpha
```
Select EmilyCup_Draggable
→ Image component
→ Color: Alpha should be 255 (not 0)
```

---

## Issue 7: Multiple Cups Appear

### Symptoms:
- See multiple Emily's Cups
- Duplicates when opening panel

### Solutions:

#### Check 1: Only One Draggable
```
In Hierarchy:
→ Should have only ONE EmilyCup_Draggable
→ Delete any duplicates
```

#### Check 2: Don't Instantiate
```
Don't use Instantiate() for the cup
→ Cup should already exist in panel
→ Just enable/disable the panel
```

---

## Issue 8: Can't Close Panel Manually

### Symptoms:
- Close button doesn't work
- Stuck in panel

### Solutions:

#### Check 1: Close Button Assigned
```
Select TeaParty_Panel
→ TeaPartyPuzzleUI
→ Close Button: Should be assigned

If empty:
→ Drag Close_Button to this field
```

#### Check 2: Button Has Listener
```
Script adds listener in Start():
closeButton.onClick.AddListener(ClosePuzzle);

Make sure Start() runs
```

#### Check 3: Add ESC Key (Optional)
```
In Update(), add:
if (Input.GetKeyDown(KeyCode.Escape))
{
    ClosePuzzle();
}
```

---

## Issue 9: Game Doesn't Pause

### Symptoms:
- Emily still moves during puzzle
- Player can still move
- Joystick still visible

### Solutions:

#### Check 1: PauseGame() Called
```
Script calls PauseGame() in OnEnable()
→ Should pause Emily
→ Should disable player
→ Should hide joystick

Check Console for errors
```

#### Check 2: Emily Reference
```
Script uses FindFirstObjectByType<EmilyGhost>()
→ Make sure EmilyGhost exists in scene
→ Make sure it has isPaused property
```

#### Check 3: Player Reference
```
Script uses FindFirstObjectByType<JoystickPlayerController>()
→ Make sure player exists
→ Make sure it can be disabled
```

---

## Issue 10: Puzzle Solves Immediately

### Symptoms:
- Panel opens and immediately closes
- Puzzle completes without dragging

### Solutions:

#### Check 1: Cup Already at Slot
```
Check EmilyCup_Draggable position
→ Should NOT be at slot position initially
→ Move it away from slot
```

#### Check 2: Snap Distance Too Large
```
Snap Distance: 500 or more
→ Always within snap range
→ Decrease to 50-100
```

#### Check 3: isPuzzleSolved Flag
```
Script sets isPuzzleSolved = false in OnEnable()
→ Make sure this runs
→ Check if OnEnable() is called
```

---

## 🧪 Debug Tips

### Add Debug Logs:

#### In OnDragCup():
```csharp
Debug.Log($"Dragging cup to: {cupRectTransform.anchoredPosition}");
Debug.Log($"Distance to slot: {distance}");
```

#### In OnEndDragCup():
```csharp
Debug.Log($"Released cup. Distance: {distance}, Snap: {snapDistance}");
if (distance < snapDistance)
    Debug.Log("SNAPPED!");
else
    Debug.Log("TOO FAR - Returning to start");
```

#### In CompletePuzzle():
```csharp
Debug.Log("Tea Party Puzzle SOLVED!");
```

---

## ✅ Verification Checklist

### Setup:
- [ ] TeaParty_Panel exists
- [ ] EmilyCup_Draggable exists (with Canvas Group)
- [ ] EmilyCup_Slot exists (target)
- [ ] TeaPartyPuzzleUI script added
- [ ] All references assigned

### Dragging:
- [ ] Can click/touch cup
- [ ] Cup follows mouse/finger
- [ ] Cup moves smoothly

### Highlighting:
- [ ] Slot highlights when cup is near
- [ ] Slot returns to normal when cup moves away
- [ ] Colors are visibly different

### Snapping:
- [ ] Cup snaps to slot when close
- [ ] Cup returns to start when far
- [ ] Sound plays when snapping

### Completion:
- [ ] Panel closes after 1 second
- [ ] Cutscene plays
- [ ] Cup removed from inventory
- [ ] Game resumes

---

## 🎯 Quick Fixes

### Fix 1: Can't Drag
```
Add Canvas Group to EmilyCup_Draggable
✓ Interactable
✓ Block Raycasts
```

### Fix 2: No Highlight
```
Assign EmilyCup_Slot to Slot Highlight
Set Highlight Color to Yellow
```

### Fix 3: Won't Snap
```
Increase Snap Distance to 100
Check both objects are RectTransforms
```

### Fix 4: Won't Close
```
Check Room07UIManager exists
Check Console for errors
```

---

## 📞 Still Not Working?

### Check These:

1. **Console Errors**
   - Open Console (Ctrl+Shift+C)
   - Fix any red errors

2. **References**
   - All fields in Inspector filled?
   - No "None" or "Missing" references?

3. **Hierarchy**
   - Panel is child of Canvas?
   - All objects are UI objects (RectTransform)?

4. **Script**
   - TeaPartyPuzzleUI.cs exists?
   - No compile errors?

---

**Most issues are missing Canvas Group or wrong Snap Distance!** 🔧

**Check Inspector references first!** ✅
