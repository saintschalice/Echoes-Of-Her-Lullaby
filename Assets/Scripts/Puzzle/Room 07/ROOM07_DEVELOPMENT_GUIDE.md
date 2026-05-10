# Room 07 (Lisa's Bedroom) - Complete Development Guide

## Overview
This guide provides step-by-step instructions for implementing Room 07's complete flow, including environmental storytelling, ritual puzzles, and the climactic chase sequence.

---

## ✅ FIXED: Dialogue and Notification System
**All item pickups now follow this flow:**
1. Dialogue shows first
2. Wait for dialogue to finish
3. Show item notification
4. Player must tap to continue
5. No overlapping dialogue and notifications

---

## Phase 1: Scene Setup

### Step 1: Create the Scene Structure
1. Create scene: `Assets/Scenes/Room07_Lisa'sBedroom.unity`
2. Add the following GameObjects:
   - **Player** (with JoystickPlayerController)
   - **Emily AI** (initially disabled)
   - **Main Camera** (with CameraFollow script)
   - **Canvas** (for UI panels)
   - **EventSystem**

### Step 2: Create Interactable Objects
Create these GameObjects with colliders and the `Room07_Interactable` script:

**Environmental Objects:**
- `Bed` (ObjectType: Bed)
- `WallDrawings` (ObjectType: WallDrawings)
- `Nightstand_Diary` (ObjectType: Diary)
- `EmilyChair` (ObjectType: Chair)
- `Closet` (ObjectType: Closet)
- `ReadingTable` (ObjectType: ReadingTable)

**Puzzle Objects:**
- `WindowCurtains` (ObjectType: WindowCurtains)
- `SmallCabinet` (ObjectType: Cabinet_Cup) - Contains Emily's Cup
- `TeaPartySpot` (ObjectType: TeaParty)
- `Toybox` (ObjectType: Toybox)
- `Dollhouse` (ObjectType: Dollhouse)
- `Mirror` (ObjectType: Mirror)

**Other Objects:**
- `BedroomDoor` (with collider, will be locked during chase)
- `BathroomDoor` (escape route)

### Step 3: Create UI Panels
In the Canvas, create these panels (all initially hidden):
- `CurtainPanel` - For curtain puzzle
- `TeaPartyPanel` - For tea party puzzle
- `ToyboxPanel` - For sliding puzzle
- `DollhousePanel` - For doll placement
- `BlackScreenCutscene` - For memory cutscenes
- `JumpscareImage` - Full-screen Emily image for jumpscare

---

## Phase 2: Script Implementation

### Core Scripts Status

#### ✅ Already Created & Fixed:
1. **Room07_BedroomController.cs** - Main room state manager
2. **Room07_FlowController.cs** - Story progression tracker
3. **Room07_Interactable.cs** - Handles all object interactions (FIXED: Now waits for dialogue)
4. **Room07UIManager.cs** - Manages UI panels
5. **ItemPickupRoom07.cs** - Handles item pickups (FIXED: Now waits for dialogue)

#### ✅ New Puzzle Scripts Created:
6. **CurtainPuzzleUI.cs** - Curtain opening puzzle
7. **TeaPartyPuzzleUI.cs** - Drag & drop tea cup puzzle
8. **ToyboxSlidingPuzzle.cs** - 8-tile sliding puzzle
9. **DollhousePuzzleUI.cs** - Drag & drop Emily doll puzzle
10. **MirrorJumpscareSequence.cs** - Jumpscare and chase trigger

---

## Phase 3: Unity Setup Instructions

### Step 1: Assign Scripts to GameObjects

#### Main Controllers
1. Create empty GameObject: `Room07_Manager`
2. Add these scripts:
   - `Room07_BedroomController`
   - `Room07_FlowController`
   - `Room07UIManager`
   - `MirrorJumpscareSequence`

#### Interactable Objects
For each interactable object:
1. Add `Room07_Interactable` script
2. Set the `myType` enum to match the object
3. Assign `uiManager` reference to the Room07UIManager

#### UI Panels
1. **CurtainPanel:**
   - Add `CurtainPuzzleUI` script
   - Assign panel reference
   - Create left/right curtain buttons and images
   - Assign audio clips

2. **TeaPartyPanel:**
   - Add `TeaPartyPuzzleUI` script
   - Create draggable cup image
   - Create target slot
   - Add EventTrigger component to cup
   - Assign audio clips

3. **ToyboxPanel:**
   - Add `ToyboxSlidingPuzzle` script
   - Create Grid Layout Group for tiles
   - Assign puzzle image (game icon)
   - Assign audio clips

4. **DollhousePanel:**
   - Add `DollhousePuzzleUI` script
   - Create draggable doll image
   - Create target slot
   - Add EventTrigger component to doll
   - Assign audio clips

### Step 2: Configure MirrorJumpscareSequence
1. Assign `emilyGhostObject` (Emily AI GameObject)
2. Create empty GameObject behind mirror: `EmilyJumpscarePosition`
3. Assign `jumpscareImage` (full-screen image)
4. Assign `lullabyFragment3` audio clip
5. Assign `musicBoxSource` (AudioSource on toybox)
6. Assign `bedroomDoorCollider`
7. Assign `bathroomDoor`

### Step 3: Setup Emily AI
1. Ensure Emily GameObject has `EmilyGhost` script
2. Set initial state to disabled
3. Configure NavMesh for room
4. Emily will automatically use existing speed and state system:
   - Normal `huntSpeed`: ~0.5 (default)
   - Chase `huntSpeed`: 3.5 (set by jumpscare sequence)
   - State forced to `Hunt` during chase

---

## Phase 4: Item Database Setup

### Add These Items to ItemDatabase:
1. **emily_cup**
   - Name: "Emily's Cup"
   - Description: "A small teacup with Emily's name on it"
   - Icon: Cup sprite
   - isUsable: false

2. **emily_doll**
   - Name: "Emily Doll"
   - Description: "A handmade doll representing Emily"
   - Icon: Doll sprite
   - isUsable: false

---

## Phase 5: Testing Checklist

### Environmental Storytelling
- [ ] Intro dialogue triggers on room entry
- [ ] All environmental objects show correct dialogue
- [ ] Bed shows note about Emily
- [ ] Wall drawings show two figures
- [ ] Diary shows Emily's protection
- [ ] Chair feels cold
- [ ] Closet shows scratches
- [ ] Reading table shows fairy tales

### Puzzle Flow
- [ ] Curtains can be opened (both left and right)
- [ ] Cabinet reveals cup after curtains opened
- [ ] Cup pickup: dialogue → notification → tap to continue
- [ ] Tea party requires cup in inventory
- [ ] Tea party drag & drop works
- [ ] Tea party completion triggers cutscene
- [ ] Toybox opens sliding puzzle
- [ ] Sliding puzzle can be solved
- [ ] Doll pickup: dialogue → notification → tap to continue → cutscene
- [ ] Dollhouse requires doll in inventory
- [ ] Dollhouse drag & drop works
- [ ] Dollhouse completion removes doll from inventory

### Climax Sequence
- [ ] Mirror only triggers when all puzzles complete
- [ ] Mirror shows "missing something" if incomplete
- [ ] Jumpscare plays correctly
- [ ] Emily appears behind Lisa
- [ ] Lullaby Fragment #3 plays
- [ ] Memory dialogue shows
- [ ] Bedroom door locks
- [ ] Player can move after sequence
- [ ] Emily chases at high speed
- [ ] Bathroom door is accessible

### Dialogue & Notification System
- [ ] No overlapping dialogue and notifications
- [ ] Dialogue always shows first
- [ ] Notification waits for dialogue to finish
- [ ] Player must tap to dismiss notification
- [ ] Multiple items show sequentially (not simultaneously)
- [ ] Inventory button works after all sequences

---

## Phase 6: Audio Requirements

### Sound Effects Needed:
- Curtain opening sound
- Cup placement sound
- Tea party success sound
- Tile move sound
- Puzzle success sound
- Doll placement sound
- Jumpscare sound (loud, sudden)
- Lullaby Fragment #3 (music box version)

### Music:
- Room ambient music (soft, eerie)
- Chase music (intense, fast-paced)

---

## Phase 7: Common Issues & Solutions

### Issue: Dialogue and notification overlap
**Solution:** ✅ FIXED - All scripts now use coroutines to wait for dialogue before showing notifications

### Issue: Items don't show notification
**Solution:** Use `AddItemWithNotification()` instead of `AddItem()`

### Issue: Puzzle doesn't pause game
**Solution:** Each puzzle script has PauseGame() method that:
- Pauses Emily AI
- Disables player controller
- Hides joystick

### Issue: Mirror triggers too early
**Solution:** MirrorJumpscareSequence checks all puzzle completion flags

### Issue: Emily doesn't chase
**Solution:** The jumpscare sequence automatically:
- Sets `huntSpeed` to 3.5 (faster chase)
- Sets `lostLOSTime` to 5 seconds (harder to escape)
- Forces Emily into `Hunt` state
- Unpauses Emily AI

---

## Phase 8: Save System Integration

### Add These Flags:
```csharp
// In Room07_FlowController.cs
private const string ROOM_NAME = "Room07_Bedroom";
private const string FLAG_CURTAINS = "Room07_CurtainsOpened";
private const string FLAG_TEAPARTY = "Room07_TeaPartyDone";
private const string FLAG_TOYBOX = "Room07_ToyboxSolved";
private const string FLAG_DOLLHOUSE = "Room07_DollhouseDone";
private const string FLAG_JUMPSCARE = "Room07_JumpscareSeen";
```

### Save Progress:
```csharp
void SaveProgress()
{
    if (areCurtainsOpened)
        SaveSystem.Instance.TriggerDialogue(FLAG_CURTAINS);
    
    if (isTeaPartyDone)
        SaveSystem.Instance.TriggerDialogue(FLAG_TEAPARTY);
    
    // etc...
}
```

---

## Phase 9: Final Polish

### Visual Effects:
- Add particle effects to Emily during jumpscare
- Add glow effect to puzzle slots when highlighted
- Add fade transitions between cutscenes

### Camera Work:
- Zoom in slightly during jumpscare
- Follow Emily briefly during reveal
- Shake during jumpscare

### UI Polish:
- Add button hover effects
- Add drag visual feedback
- Add completion animations

---

## Summary

All scripts are now created and fixed to prevent dialogue/notification overlap. The flow is:

1. **Environmental Storytelling** → Player explores and learns about Emily
2. **Curtain Puzzle** → Opens access to cup
3. **Tea Party Puzzle** → Triggers Memory Cutscene 1
4. **Toybox Puzzle** → Reveals doll compartment
5. **Dollhouse Puzzle** → Completes all rituals
6. **Mirror Jumpscare** → Triggers chase sequence
7. **Chase to Bathroom** → Escape Emily

**Key Fix:** All item pickups now properly sequence: Dialogue → Wait → Notification → Wait → Continue

Ready for implementation! 🎮👻

