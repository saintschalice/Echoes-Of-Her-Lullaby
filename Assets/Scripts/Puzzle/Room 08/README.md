# 🛁 ROOM 08 - LISA'S BATHROOM

## 📋 OVERVIEW

Complete implementation package for Room 08 - Lisa's Bathroom. This room features:
- Evidence examination (bathtub, medicine cabinet, 3 evidence items)
- Mirror confrontation (Emily DID reveal)
- Mirror breaking QTE (5 taps, decreasing time)
- Escape through passage to Master Bathroom

**Status:** ✅ All scripts complete, ready for Unity implementation

---

## 📁 FILES IN THIS FOLDER

### **🔧 SCRIPTS (4 files)**

#### **1. Room08_Dialogues.cs**
- Static class containing all dialogues
- 1-2 sentence dialogues that fit in dialogue box
- Organized by sequence (entry, evidence, mirror, escape)
- **Usage:** Reference dialogues in other scripts

#### **2. Room08_FlowController.cs**
- Main progression controller
- Tracks evidence collection, mirror progress
- Handles intro sequence, Emily humming, scene transition
- **Attach to:** Empty GameObject named `Room08_FlowController`

#### **3. Room08_Interactable.cs**
- Handles all object interactions
- Supports: Bathtub, Medicine Cabinet, Mirror, Door, Passage, Evidence
- **Attach to:** All interactable objects in scene

#### **4. Room08_MirrorQTE.cs**
- Complete QTE system for mirror breaking
- 5 taps with decreasing time (2.0s to 0.8s)
- 3 failures = game over
- Progressive cracks, camera shake, audio
- **Attach to:** Empty GameObject named `Room08_MirrorQTE`

---

### **📖 GUIDES (5 files)**

#### **1. ROOM08_COMPLETE_GUIDE.md** ⭐ START HERE
- **Most comprehensive guide**
- Step-by-step Unity setup instructions
- All GameObjects, UI, sprites, audio
- Testing checklist
- Troubleshooting guide
- **Use this as your main reference**

#### **2. QTE_PANEL_SETUP.md**
- Quick reference for QTE UI setup
- Detailed hierarchy and settings
- Inspector values
- **Use this when setting up QTE Panel**

#### **3. ROOM08_VISUAL_GUIDE.md**
- Visual diagrams and layouts
- Scene layout, flow diagram
- GameObject hierarchy
- Sprite requirements
- **Use this for visual reference**

#### **4. IMPLEMENTATION_CHECKLIST.md**
- Checkbox list for tracking progress
- Organized by phases
- Testing scenarios
- **Use this to track your work**

#### **5. README.md** (this file)
- Overview of all files
- Quick start guide
- **Use this to understand the package**

---

## 🚀 QUICK START

### **Step 1: Read the Complete Guide**
Open `ROOM08_COMPLETE_GUIDE.md` and read through it to understand the full scope.

### **Step 2: Copy Scripts**
All 4 scripts are already in this folder:
- Room08_Dialogues.cs ✅
- Room08_FlowController.cs ✅
- Room08_Interactable.cs ✅
- Room08_MirrorQTE.cs ✅

### **Step 3: Follow the Checklist**
Open `IMPLEMENTATION_CHECKLIST.md` and check off items as you complete them.

### **Step 4: Use Visual Guide**
Refer to `ROOM08_VISUAL_GUIDE.md` for scene layout and hierarchy.

### **Step 5: Setup QTE Panel**
Use `QTE_PANEL_SETUP.md` for detailed QTE UI setup.

---

## 🎯 WHAT YOU NEED TO CREATE

### **GameObjects (13 total)**
1. Room08_FlowController (Empty)
2. Room08_MirrorQTE (Empty)
3. Bathtub (Interactable)
4. MedicineCabinet (Interactable)
5. Mirror (Interactable)
6. Door (Interactable)
7. Passage (Interactable, initially inactive)
8. Bandages (Evidence)
9. TornClothes (Evidence)
10. ApologyNote (Evidence)
11. QTE_Panel (UI)
12. TapTarget (Prefab)
13. Canvas (if not exists)

### **Sprites (10 total)**
- 6 mirror sprites (normal + 5 cracks)
- 3 evidence sprites (bandages, clothes, note)
- 1 tap target sprite (circle)

### **Audio (6 total)**
- 1 Emily humming clip (looping)
- 5 QTE sound clips (tap, crack, shatter, fail, stress x5)

---

## 📊 FLOW SUMMARY

```
Entry → Examine Evidence → Examine Mirror → Break Mirror (QTE) → Escape
```

1. **Entry:** Lisa enters, door locks, Emily humming outside
2. **Evidence:** Examine bathtub, medicine cabinet, 3 evidence items
3. **Mirror:** Long confrontation sequence (11 dialogues)
4. **QTE:** Break mirror (5 taps, decreasing time, 3 failures = game over)
5. **Escape:** Climb through passage to Master Bathroom

---

## 🎮 KEY FEATURES

### **Dialogue System**
- All dialogues are 1-2 sentences
- Player stops at START of dialogue sequence
- NO delays between dialogues
- Player re-enabled at END of sequence

### **Evidence System**
- 3 evidence items must be examined
- Objects disappear after examination
- All evidence required before mirror

### **Mirror QTE**
- 5 taps with decreasing time (2.0s → 0.8s)
- Random tap target positions
- Progressive crack sprites
- Camera shake on each tap
- Escalating stress sounds
- 3 failures = game over

### **Audio Design**
- Emily humming (ambient, looping)
- QTE sounds (tap, crack, shatter, fail)
- Glass stress sounds (5 stages, escalating)

---

## 🔗 DEPENDENCIES

### **Required Scripts (from other folders)**
- `DialogueSystemV2.cs` - For showing dialogues
- `JoystickPlayerController.cs` - For player movement
- `SaveSystem.cs` - For saving progress
- `AudioManager.cs` - For playing sounds
- `IInteractable.cs` - Interface for interactions

### **Required Components**
- Canvas with GraphicRaycaster
- EventSystem
- Main Camera

---

## 🐛 TROUBLESHOOTING

### **Scripts don't compile**
- Check all dependencies are present
- Check Unity version compatibility

### **QTE doesn't work**
- Check all references assigned in Inspector
- Check Canvas has GraphicRaycaster
- Check EventSystem exists

### **Audio doesn't play**
- Check AudioManager exists
- Check audio clips assigned
- Check volume levels

### **Dialogues don't show**
- Check DialogueSystemV2 exists
- Check dialogue strings are correct

---

## 📝 IMPLEMENTATION ORDER

1. **Phase 1:** Copy scripts, verify compilation
2. **Phase 2:** Create GameObjects (controllers, interactables, evidence)
3. **Phase 3:** Create UI (QTE Panel, tap target prefab)
4. **Phase 4:** Create/import sprites
5. **Phase 5:** Import audio clips
6. **Phase 6:** Assign all references in Inspector
7. **Phase 7:** Test each sequence
8. **Phase 8:** Debug and fix issues
9. **Phase 9:** Polish (visuals, audio, gameplay)

---

## ✅ TESTING CHECKLIST

- [ ] Entry sequence works
- [ ] Evidence examination works
- [ ] Mirror examination works
- [ ] QTE success path works
- [ ] QTE failure path works
- [ ] Escape sequence works
- [ ] No console errors
- [ ] Performance is good

---

## 🎨 DESIGN NOTES

### **Mood**
- Tense, claustrophobic
- Emily's presence is unsettling
- Mirror confrontation is emotional climax

### **Pacing**
- Slow exploration (evidence)
- Emotional revelation (mirror)
- Intense action (QTE)
- Relief (escape)

### **Difficulty**
- QTE is challenging but fair
- 3 failures gives room for mistakes
- Decreasing time adds pressure

---

## 💡 TIPS

1. **Start with Complete Guide** - Read it fully before starting
2. **Use Checklist** - Track your progress systematically
3. **Test Early** - Test each phase before moving on
4. **Adjust Difficulty** - Tweak QTE timing if needed
5. **Polish Last** - Get it working first, polish later

---

## 📞 SUPPORT

If you encounter issues:

1. Check `ROOM08_COMPLETE_GUIDE.md` troubleshooting section
2. Verify all references are assigned
3. Check console for errors
4. Test each component individually

---

## 🎉 COMPLETION

When all tests pass and everything works:

**ROOM 08 IS COMPLETE!** 🎮✨

You're ready to move on to Room 09 (Master Bathroom)! 💖

---

## 📄 FILE SUMMARY

| File | Type | Purpose |
|------|------|---------|
| Room08_Dialogues.cs | Script | All dialogue strings |
| Room08_FlowController.cs | Script | Main progression controller |
| Room08_Interactable.cs | Script | Object interactions |
| Room08_MirrorQTE.cs | Script | QTE system |
| ROOM08_COMPLETE_GUIDE.md | Guide | Main setup guide ⭐ |
| QTE_PANEL_SETUP.md | Guide | QTE UI reference |
| ROOM08_VISUAL_GUIDE.md | Guide | Visual diagrams |
| IMPLEMENTATION_CHECKLIST.md | Guide | Progress tracker |
| README.md | Guide | This file |

---

**Created:** May 2, 2026
**Status:** ✅ Complete and ready for implementation
**Version:** 1.0

**GOOD LUCK!** 🚀💖
