# 🎮 ROOM 09 - COMPLETE IMPLEMENTATION GUIDE

## 📚 ALL DOCUMENTATION IN ONE PLACE

Welcome to Room 09! This is the **FINAL ROOM** of "Echoes of Her Lullaby". Ito ang lahat ng kailangan mo para ma-implement ang Room 09!

---

## 🎯 WHAT IS ROOM 09?

**Room Name**: Master Bedroom's Bathroom (Final Room)

**Purpose**: 
- Climactic puzzle room with 4 mirror puzzles
- Reveals complete truth about mother's murder-suicide plan
- Emily's breakdown and resolution
- Game ending with 20-dialogue cutscene

**Gameplay**:
- Player solves 4 mirror puzzles in any order
- Each puzzle has 60-90 second time limit
- Failure = Emily jumpscare → Game Over
- Success = All 4 complete → Ending cutscene → Main Menu

---

## 📁 DOCUMENTATION FILES

### **🎯 START HERE**:

1. **UNITY_SETUP_CHECKLIST_TAGALOG.md** ⭐ **MOST IMPORTANT**
   - Complete step-by-step setup guide
   - Everything you need to do in Unity
   - No prefabs needed!
   - Follow this first!

2. **ITEM_IDS_REFERENCE.md** ⭐ **QUICK REFERENCE**
   - All Item Ids and Puzzle Numbers
   - Copy-paste reference
   - Use when setting up DraggableItem scripts

3. **DRAG_DROP_TROUBLESHOOTING.md** ⭐ **IF PROBLEMS**
   - Common problems and solutions
   - Debug tips
   - Quick fixes

---

### **📖 DETAILED GUIDES**:

4. **SIMPLE_DRAG_DROP_SETUP.md**
   - How to create drag-and-drop puzzles
   - No prefabs needed
   - Step-by-step for each mirror

5. **VISUAL_SETUP_GUIDE.md**
   - Visual descriptions
   - What you should see
   - For visual learners

6. **CORRECT_INTERACTION_SETUP.md**
   - How interaction system works
   - IInteractable interface
   - PlayerInteractionController

7. **MOBILE_INTERACTION_SETUP.md**
   - Mobile-specific setup
   - Touch/tap interaction
   - Button setup

8. **MOBILE_QUICK_SETUP.md**
   - Quick mobile setup
   - 3-5 steps only
   - Tagalog guide

---

### **🎨 DESIGN DOCUMENTS**:

9. **ROOM09_COMPLETE_DESIGN.md**
   - Full room design
   - All 4 puzzles explained
   - Story and gameplay flow

10. **ROOM09_ASSETS_AND_FLOW.md**
    - Assets needed
    - Gameplay flow
    - Technical requirements

11. **FLASHBACK_IMAGE_PROMPTS.md**
    - AI image generation prompts
    - For Mirror 4 flashback images
    - 3 variations per image

12. **ROOM09_FINAL_ROOM_GUIDE_TAGALOG.md**
    - Tagalog guide
    - Room 09 as final room
    - No Room 10

13. **ROOM09_UPDATED_SUMMARY.md**
    - Summary of changes
    - Room 09 as final room
    - Ending cutscene details

---

### **💻 CODE FILES**:

14. **Room09_FlowController.cs**
    - Main controller for Room 09
    - Manages puzzle progress
    - Handles ending cutscene

15. **Room09_Dialogues.cs**
    - All dialogue constants
    - Entry, puzzle, success, failure dialogues
    - 20 ending dialogues

16. **Room09_Interactable.cs**
    - Implements IInteractable interface
    - Attached to each mirror
    - Triggers puzzles

17. **Mirror1_MedicineCabinet.cs**
    - 6 bottle puzzle
    - Chronological arrangement
    - 60 second timer

18. **Mirror2_BathtubDrain.cs**
    - 4 note piece puzzle
    - Reassemble torn note
    - 60 second timer

19. **Mirror3_VanityTerror.cs**
    - 8 diary page puzzle
    - Chronological arrangement
    - 90 second timer

20. **Mirror4_EvidenceSequence.cs**
    - 4 evidence item puzzle
    - Correct sequence
    - Flashback images
    - 60 second timer

21. **DraggableItem.cs**
    - Drag-and-drop system
    - Works for all puzzles
    - Touch and mouse support

---

## 🚀 QUICK START GUIDE

### **Step 1: Read Documentation** (5 minutes)

```
1. Read UNITY_SETUP_CHECKLIST_TAGALOG.md
2. Skim ITEM_IDS_REFERENCE.md
3. Keep DRAG_DROP_TROUBLESHOOTING.md open
```

### **Step 2: Setup Scene** (10 minutes)

```
1. Create 4 mirror GameObjects
2. Add sprites, colliders, scripts
3. Set Mirror Numbers (1-4)
```

### **Step 3: Create UI Panels** (30 minutes)

```
1. Create 4 panels in Canvas
2. Add titles, timers, slots
3. Add draggable items
4. Add DraggableItem scripts
```

### **Step 4: Assign References** (10 minutes)

```
1. For each mirror GameObject
2. Assign panel, timer, slots
3. Check all references filled
```

### **Step 5: Test** (10 minutes)

```
1. Test each mirror interaction
2. Test drag-and-drop
3. Test puzzle completion
4. Fix any issues
```

**Total Time**: ~1 hour for all 4 mirrors

---

## 📋 IMPLEMENTATION CHECKLIST

### **Phase 1: Scene Setup**

- [ ] Create Room 09 scene
- [ ] Add Canvas (with Graphic Raycaster)
- [ ] Add EventSystem
- [ ] Add Player
- [ ] Add Joystick
- [ ] Add Interact Button

### **Phase 2: Mirrors**

- [ ] Create Mirror 1 GameObject
- [ ] Create Mirror 2 GameObject
- [ ] Create Mirror 3 GameObject
- [ ] Create Mirror 4 GameObject
- [ ] Add sprites to all mirrors
- [ ] Add colliders to all mirrors (Is Trigger: ✓)
- [ ] Add Room09_Interactable to all mirrors
- [ ] Add puzzle scripts to all mirrors

### **Phase 3: UI Panels**

- [ ] Create MedicineCabinet_Panel
- [ ] Create BathtubDrain_Panel
- [ ] Create VanityTerror_Panel
- [ ] Create EvidenceSequence_Panel
- [ ] Add titles and timers to all panels
- [ ] Create slots for all panels
- [ ] Create draggable items for all panels
- [ ] Add DraggableItem scripts to all items

### **Phase 4: References**

- [ ] Assign references for Mirror 1
- [ ] Assign references for Mirror 2
- [ ] Assign references for Mirror 3
- [ ] Assign references for Mirror 4
- [ ] Verify all references filled
- [ ] No "None" or "Missing" references

### **Phase 5: Testing**

- [ ] Test Mirror 1 interaction
- [ ] Test Mirror 2 interaction
- [ ] Test Mirror 3 interaction
- [ ] Test Mirror 4 interaction
- [ ] Test all drag-and-drop
- [ ] Test puzzle completion
- [ ] Test success dialogues
- [ ] Test failure (timeout)
- [ ] Test ending cutscene (all 4 complete)

### **Phase 6: Polish**

- [ ] Add sprites/art
- [ ] Add audio
- [ ] Add visual effects
- [ ] Test on mobile device
- [ ] Fix any bugs
- [ ] Optimize performance

---

## 🎮 HOW IT WORKS

### **Interaction Flow**:

```
Player walks near mirror
    ↓
PlayerInteractionController detects Room09_Interactable
    ↓
Interact button activates
    ↓
Player taps Interact button
    ↓
Room09_Interactable.OnInteract() called
    ↓
Mirror script StartPuzzle() called
    ↓
Panel opens, timer starts
    ↓
Player drags items to slots
    ↓
DraggableItem detects slot
    ↓
Notifies mirror script
    ↓
Mirror script checks solution
    ↓
If correct: Success dialogue → Panel closes
If timeout: Emily jumpscare → Game Over
```

### **Drag-and-Drop Flow**:

```
Player taps item
    ↓
DraggableItem.OnBeginDrag() called
    ↓
Item becomes semi-transparent
    ↓
Item follows finger/mouse
    ↓
Player releases
    ↓
DraggableItem.OnEndDrag() called
    ↓
Raycast to find what's underneath
    ↓
If slot found: Place in slot → Notify puzzle script
If nothing: Return to original position
```

### **Puzzle Completion Flow**:

```
All items placed correctly
    ↓
Mirror script checks solution
    ↓
Solution correct!
    ↓
Success dialogue shows
    ↓
Panel closes
    ↓
Room09_FlowController.OnMirrorComplete() called
    ↓
Check if all 4 mirrors complete
    ↓
If yes: Start ending cutscene
If no: Continue gameplay
```

---

## 🎯 KEY CONCEPTS

### **1. IInteractable Interface**

All interactable objects implement this interface:

```csharp
public interface IInteractable
{
    void OnInteract(PlayerContext context);
    void OnFocus(PlayerContext context);
    void OnBlur(PlayerContext context);
}
```

Room09_Interactable implements this to work with PlayerInteractionController.

### **2. DraggableItem System**

All draggable items use the same script:

```csharp
DraggableItem:
- Item Id: Unique identifier
- Puzzle Number: Which puzzle (1-4)
- Detects drag events
- Finds slots underneath
- Notifies puzzle scripts
```

### **3. Puzzle Scripts**

Each mirror has its own puzzle script:

```csharp
Mirror1_MedicineCabinet: 6 bottles
Mirror2_BathtubDrain: 4 note pieces
Mirror3_VanityTerror: 8 diary pages
Mirror4_EvidenceSequence: 4 evidence items
```

All have:
- StartPuzzle() method
- Timer system
- Solution checking
- Success/failure handling

### **4. Room09_FlowController**

Main controller that:
- Tracks puzzle completion
- Handles intro sequence
- Handles ending cutscene
- Manages Emily's state

---

## 🐛 COMMON ISSUES

### **Issue 1: Can't interact with mirrors**

**Solution**: Check CORRECT_INTERACTION_SETUP.md
- Collider Is Trigger: ✓
- Room09_Interactable attached
- Mirror Number set

### **Issue 2: Can't drag items**

**Solution**: Check DRAG_DROP_TROUBLESHOOTING.md
- Raycast Target: ✓
- Graphic Raycaster on Canvas
- EventSystem exists
- DraggableItem script attached

### **Issue 3: Items snap back**

**Solution**: Check ITEM_IDS_REFERENCE.md
- Slot names contain "Slot" or "Frame"
- Item Id is correct
- Puzzle Number is correct

### **Issue 4: Puzzle doesn't complete**

**Solution**: Check UNITY_SETUP_CHECKLIST_TAGALOG.md
- Items in correct order
- Slots assigned in Inspector
- Item Ids match expected values

---

## 📚 LEARNING RESOURCES

### **New to Unity UI?**

Read these in order:
1. VISUAL_SETUP_GUIDE.md (see what to create)
2. SIMPLE_DRAG_DROP_SETUP.md (step-by-step)
3. UNITY_SETUP_CHECKLIST_TAGALOG.md (complete guide)

### **New to Drag-and-Drop?**

Read these:
1. DraggableItem.cs (code with comments)
2. SIMPLE_DRAG_DROP_SETUP.md (how it works)
3. DRAG_DROP_TROUBLESHOOTING.md (common issues)

### **New to Interaction System?**

Read these:
1. CORRECT_INTERACTION_SETUP.md (how it works)
2. Room08_Interactable.cs (reference example)
3. Room09_Interactable.cs (implementation)

---

## 🎨 ASSETS NEEDED

### **Sprites**:

**Mirrors**:
- 4 different mirror sprites

**Mirror 1**:
- 6 prescription bottle sprites (or use white rectangles)

**Mirror 2**:
- Bathtub sprite
- Drain cover sprite
- 4 torn note piece sprites (or use beige rectangles)

**Mirror 3**:
- 8 diary page sprites (or use light brown rectangles)

**Mirror 4**:
- Large mirror sprite
- 4 picture frame sprites
- Rope sprite
- Pills sprite
- Knife sprite
- Bloody towel sprite
- 4 flashback images (see FLASHBACK_IMAGE_PROMPTS.md)

**UI**:
- Emily jumpscare image
- Success effect (optional)

### **Audio**:

- Puzzle success sound
- Puzzle failure sound
- Emily scream sound
- Paper rustle sound
- Bottle clink sound
- Drain open sound
- Flashback sound
- Ending music

### **Temporary Placeholders**:

If you don't have sprites yet:
- Mirrors: White squares
- Bottles: White rectangles with text
- Notes: Beige rectangles with text
- Pages: Light brown rectangles with text
- Evidence: Colored rectangles (brown, white, gray, red)

**You can test everything with placeholders!**

---

## ✅ FINAL CHECKLIST

### **Before Calling It Done**:

- [ ] All 4 mirrors work
- [ ] All drag-and-drop works
- [ ] All puzzles can be completed
- [ ] Success dialogues show
- [ ] Failure (timeout) works
- [ ] Ending cutscene triggers
- [ ] No errors in Console
- [ ] Tested on mobile device
- [ ] Performance is good
- [ ] All sprites assigned (or placeholders)
- [ ] All audio assigned
- [ ] Documentation updated

---

## 🎉 SUMMARY

### **What You're Building**:

**Room 09**: Final room with 4 mirror puzzles
- Mirror 1: Medicine Cabinet (6 bottles)
- Mirror 2: Bathtub Drain (4 note pieces)
- Mirror 3: Vanity Terror (8 diary pages)
- Mirror 4: Evidence Sequence (4 evidence items)

**Total**: 22 draggable items, 4 puzzles, 1 ending cutscene

### **Key Features**:

- ✅ No prefabs needed
- ✅ Mobile-friendly
- ✅ Touch and mouse support
- ✅ Time limits
- ✅ Success/failure states
- ✅ Complete ending cutscene

### **Documentation**:

- ✅ 21 documentation files
- ✅ 8 code files
- ✅ Complete guides
- ✅ Troubleshooting
- ✅ Quick reference

---

## 📞 NEED HELP?

### **Check These First**:

1. **UNITY_SETUP_CHECKLIST_TAGALOG.md** - Complete setup guide
2. **DRAG_DROP_TROUBLESHOOTING.md** - Common problems
3. **ITEM_IDS_REFERENCE.md** - Quick reference
4. **Console logs** - Debug messages

### **Common Questions**:

**Q: Do I need prefabs?**
A: No! Create UI directly in Canvas.

**Q: How do I make items draggable?**
A: Add DraggableItem script, set Item Id and Puzzle Number.

**Q: Why can't I drag items?**
A: Check Raycast Target, Graphic Raycaster, EventSystem.

**Q: Why do items snap back?**
A: Check slot names, Item Ids, Puzzle Numbers.

**Q: How do I test on mobile?**
A: Use Unity Remote or build to device.

---

## 🚀 GET STARTED!

### **Ready to Begin?**

1. **Open**: UNITY_SETUP_CHECKLIST_TAGALOG.md
2. **Follow**: Step-by-step instructions
3. **Reference**: ITEM_IDS_REFERENCE.md
4. **Troubleshoot**: DRAG_DROP_TROUBLESHOOTING.md

### **Estimated Time**:

- Setup: 1 hour
- Testing: 30 minutes
- Polish: 1-2 hours
- **Total**: 2-3 hours

---

**ROOM 09 COMPLETE GUIDE!** 🎮✨

Everything you need is here!

**KAYA MO YAN!** 💪🎨🎮

**START WITH: UNITY_SETUP_CHECKLIST_TAGALOG.md** ⭐
