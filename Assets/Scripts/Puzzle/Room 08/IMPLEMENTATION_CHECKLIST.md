# ✅ ROOM 08 - IMPLEMENTATION CHECKLIST

Use this checklist to track your progress implementing Lisa's Bathroom!

---

## 📁 PHASE 1: SCRIPTS & SETUP

### **Scripts** (All Done! ✅)
- [x] Room08_Dialogues.cs created
- [x] Room08_FlowController.cs created
- [x] Room08_Interactable.cs created
- [x] Room08_MirrorQTE.cs created

### **Folder Structure**
- [ ] `Assets/Scripts/Puzzle/Room 08/` folder exists
- [ ] All 4 scripts copied to folder
- [ ] Scripts compile without errors

---

## 🎮 PHASE 2: GAMEOBJECTS

### **Controllers**
- [ ] Room08_FlowController GameObject created
- [ ] Room08_FlowController.cs component added
- [ ] Room08_MirrorQTE GameObject created
- [ ] Room08_MirrorQTE.cs component added

### **Interactable Objects**
- [ ] Bathtub GameObject created
  - [ ] Room08_Interactable.cs added
  - [ ] Object Type set to `Bathtub`
  - [ ] Collider2D added (trigger)
  - [ ] Sprite assigned
  - [ ] Positioned in scene

- [ ] MedicineCabinet GameObject created
  - [ ] Room08_Interactable.cs added
  - [ ] Object Type set to `MedicineCabinet`
  - [ ] Collider2D added (trigger)
  - [ ] Sprite assigned
  - [ ] Positioned in scene

- [ ] Mirror GameObject created
  - [ ] Room08_Interactable.cs added
  - [ ] Object Type set to `Mirror`
  - [ ] Collider2D added (trigger)
  - [ ] SpriteRenderer added
  - [ ] Sprite assigned
  - [ ] Positioned in scene

- [ ] Door GameObject created
  - [ ] Room08_Interactable.cs added
  - [ ] Object Type set to `Door`
  - [ ] Collider2D added (trigger)
  - [ ] Sprite assigned
  - [ ] Positioned in scene

- [ ] Passage GameObject created
  - [ ] Room08_Interactable.cs added
  - [ ] Object Type set to `Passage`
  - [ ] Collider2D added (trigger)
  - [ ] Sprite assigned
  - [ ] **Set to INACTIVE** (initially hidden)
  - [ ] Positioned behind mirror

### **Evidence Objects**
- [ ] Bandages GameObject created
  - [ ] Room08_Interactable.cs added
  - [ ] Object Type set to `Evidence`
  - [ ] Evidence ID set to `"bandages"`
  - [ ] Collider2D added (trigger)
  - [ ] Sprite assigned
  - [ ] Positioned near bathtub

- [ ] TornClothes GameObject created
  - [ ] Room08_Interactable.cs added
  - [ ] Object Type set to `Evidence`
  - [ ] Evidence ID set to `"torn_clothes"`
  - [ ] Collider2D added (trigger)
  - [ ] Sprite assigned
  - [ ] Positioned near bathtub

- [ ] ApologyNote GameObject created
  - [ ] Room08_Interactable.cs added
  - [ ] Object Type set to `Evidence`
  - [ ] Evidence ID set to `"apology_note"`
  - [ ] Collider2D added (trigger)
  - [ ] Sprite assigned
  - [ ] Positioned near medicine cabinet

---

## 🎨 PHASE 3: UI SETUP

### **QTE Panel**
- [ ] Canvas exists in scene
- [ ] Canvas has GraphicRaycaster component
- [ ] EventSystem exists in scene

- [ ] QTE_Panel created (Panel)
  - [ ] Anchors set to Stretch (full screen)
  - [ ] Color set to black with alpha 0.8
  - [ ] **Set to INACTIVE** (initially hidden)

- [ ] Mirror_Image created (Image)
  - [ ] Parent: QTE_Panel
  - [ ] Anchors: Center
  - [ ] Size: 400x600
  - [ ] Sprite: Mirror_Normal assigned
  - [ ] Preserve Aspect: TRUE

- [ ] Tap_Target_Parent created (Empty RectTransform)
  - [ ] Parent: QTE_Panel
  - [ ] Anchors: Center
  - [ ] Size: 400x300

- [ ] Timer_Text created (Text/TextMeshProUGUI)
  - [ ] Parent: QTE_Panel
  - [ ] Anchors: Top Center
  - [ ] Font Size: 48
  - [ ] Alignment: Center
  - [ ] Text: "2.00"
  - [ ] Color: White

- [ ] Progress_Text created (Text/TextMeshProUGUI)
  - [ ] Parent: QTE_Panel
  - [ ] Anchors: Bottom Center
  - [ ] Font Size: 36
  - [ ] Alignment: Center
  - [ ] Text: "1/5"
  - [ ] Color: White

- [ ] Shatter_Effect created (Particle System) [Optional]
  - [ ] Parent: QTE_Panel
  - [ ] Configured for glass shatter
  - [ ] **Set to INACTIVE** (initially hidden)

### **Tap Target Prefab**
- [ ] TapTarget GameObject created
  - [ ] Image component added
  - [ ] Size: 100x100
  - [ ] Sprite: Circle assigned
  - [ ] Color: White with alpha

- [ ] Button component added
  - [ ] Interactable: TRUE
  - [ ] Transition: Color Tint
  - [ ] Colors configured

- [ ] Prefab created
  - [ ] Saved to `Assets/Prefabs/UI/TapTarget.prefab`
  - [ ] Deleted from scene

---

## 🎨 PHASE 4: SPRITES

### **Mirror Sprites**
- [ ] Mirror_Normal created (clean mirror)
- [ ] Mirror_Crack_1 created (small crack)
- [ ] Mirror_Crack_2 created (more cracks)
- [ ] Mirror_Crack_3 created (even more)
- [ ] Mirror_Crack_4 created (almost shattered)
- [ ] Mirror_Crack_5 created (heavily cracked)

### **Evidence Sprites**
- [ ] Bandages sprite created/imported
- [ ] Torn Clothes sprite created/imported
- [ ] Apology Note sprite created/imported

### **UI Sprites**
- [ ] Tap Target circle sprite created/imported

---

## 🔊 PHASE 5: AUDIO

### **Ambient Audio**
- [ ] Emily humming audio clip imported
- [ ] AudioSource component added to Room08_FlowController
  - [ ] Loop: TRUE
  - [ ] Play On Awake: FALSE
  - [ ] Volume: 0.3-0.5

### **QTE Audio**
- [ ] Tap sound imported
- [ ] Crack sound imported
- [ ] Shatter sound imported
- [ ] Fail sound imported
- [ ] Glass stress sound 1 imported
- [ ] Glass stress sound 2 imported
- [ ] Glass stress sound 3 imported
- [ ] Glass stress sound 4 imported
- [ ] Glass stress sound 5 imported

---

## 🔗 PHASE 6: INSPECTOR REFERENCES

### **Room08_FlowController**
- [ ] Emily AI assigned (optional)
- [ ] Emily Humming Sound assigned
- [ ] Emily Audio Source assigned
- [ ] Bathroom Door assigned
- [ ] Next Scene Name: `"Room09_Master's_Bathroom"`

### **Room08_MirrorQTE**
- [ ] **QTE Settings:**
  - [ ] Total Taps: 5
  - [ ] Starting Time: 2.0
  - [ ] Minimum Time: 0.8
  - [ ] Max Failures: 3

- [ ] **UI References:**
  - [ ] QTE Panel assigned
  - [ ] Tap Target Prefab assigned
  - [ ] Tap Target Parent assigned
  - [ ] Timer Text assigned
  - [ ] Progress Text assigned

- [ ] **Visual Effects:**
  - [ ] Mirror Image assigned
  - [ ] Crack Sprites array (5 sprites) assigned
  - [ ] Shatter Effect assigned (optional)

- [ ] **Audio:**
  - [ ] Tap Sound assigned
  - [ ] Crack Sound assigned
  - [ ] Shatter Sound assigned
  - [ ] Fail Sound assigned
  - [ ] Glass Stress Sounds array (5 clips) assigned

- [ ] **Camera Shake:**
  - [ ] Shake Intensity: 0.1
  - [ ] Shake Duration: 0.2

---

## 🧪 PHASE 7: TESTING

### **Test 1: Entry Sequence**
- [ ] Enter Room 08
- [ ] Intro dialogue plays (4 parts)
- [ ] Emily humming sound plays
- [ ] Door is locked when clicked
- [ ] Player can move after intro

### **Test 2: Evidence Examination**
- [ ] Click bathtub → Dialogue shows
- [ ] Click medicine cabinet → Dialogue shows
- [ ] Click bandages → Dialogue shows, object disappears
- [ ] Click torn clothes → Dialogue shows, object disappears
- [ ] Click apology note → Dialogue shows, object disappears
- [ ] Player stops during dialogues
- [ ] No delays between dialogues

### **Test 3: Mirror Examination**
- [ ] Try mirror before evidence → "Need evidence" message
- [ ] Complete all evidence
- [ ] Click mirror → Long confrontation sequence (11 dialogues)
- [ ] All Emily dialogue shows correctly
- [ ] Prompt to break mirror appears
- [ ] Player stops during entire sequence

### **Test 4: Mirror QTE - Success**
- [ ] Click mirror again → QTE starts
- [ ] QTE panel shows
- [ ] Tap targets appear at random positions
- [ ] Timer counts down correctly
- [ ] Timer color changes (white → yellow → red)
- [ ] Click target → Success feedback
- [ ] Crack appears on mirror
- [ ] Crack sound plays
- [ ] Stress sound plays
- [ ] Camera shakes
- [ ] Progress text updates (1/5, 2/5, etc.)
- [ ] Complete 5 taps → Mirror shatters
- [ ] Shatter sound plays
- [ ] Big camera shake
- [ ] Passage revealed (becomes active)
- [ ] Emily humming stops

### **Test 5: Mirror QTE - Failure**
- [ ] Start QTE
- [ ] Miss a tap → Failure count increases
- [ ] Fail sound plays
- [ ] Progress text shows "MISS!"
- [ ] Miss 3 taps total → Game over sequence
- [ ] QTE panel closes
- [ ] Returns to checkpoint

### **Test 6: Escape**
- [ ] Click passage → Climb through dialogue
- [ ] Scene transitions to Master Bathroom
- [ ] Progress saved correctly

### **Test 7: Edge Cases**
- [ ] Try door multiple times → Always locked
- [ ] Try passage before mirror broken → Blocked message
- [ ] Try mirror multiple times → Correct response each time
- [ ] QTE timer runs out → Counts as failure

---

## 🐛 PHASE 8: DEBUGGING

### **Common Issues Checked**
- [ ] No console errors
- [ ] All references assigned (no "None" warnings)
- [ ] QTE panel shows/hides correctly
- [ ] Tap targets are clickable
- [ ] Audio plays correctly
- [ ] Camera shake works
- [ ] Scene transition works
- [ ] Save system works

### **Performance Checked**
- [ ] No lag during QTE
- [ ] Audio doesn't stutter
- [ ] Smooth scene transition
- [ ] No memory leaks

---

## 🎯 PHASE 9: POLISH

### **Visual Polish**
- [ ] Mirror sprites look good
- [ ] Evidence sprites are clear
- [ ] UI is readable
- [ ] Colors are consistent
- [ ] Animations are smooth

### **Audio Polish**
- [ ] Volume levels balanced
- [ ] No audio clipping
- [ ] Stress sounds escalate properly
- [ ] Humming is eerie but not annoying

### **Gameplay Polish**
- [ ] QTE difficulty feels fair
- [ ] Dialogues are well-paced
- [ ] Player movement feels good
- [ ] Feedback is clear

---

## ✅ FINAL CHECKLIST

- [ ] All scripts compile
- [ ] All GameObjects created
- [ ] All UI elements created
- [ ] All sprites assigned
- [ ] All audio assigned
- [ ] All references assigned
- [ ] All tests passed
- [ ] No console errors
- [ ] Performance is good
- [ ] Polish complete

---

## 🎉 COMPLETION

When all checkboxes are checked:

**ROOM 08 IS COMPLETE!** 🎮✨

Ready to move on to Room 09 (Master Bathroom)! 💖

---

## 📝 NOTES

Use this space to track issues or ideas:

```
[Your notes here]
```

---

**Last Updated:** [Date]
**Status:** [In Progress / Testing / Complete]
