# Room 07 Quick Setup Checklist ✅

Gamitin ito para siguruhing kumpleto ang setup mo!

---

## 📦 STEP 1: Scene Basics
- [ ] Scene created: `Room07_Lisa'sBedroom.unity`
- [ ] Player added with JoystickPlayerController
- [ ] Main Camera with CameraFollow
- [ ] Canvas (UI Scale: 1920x1080)
- [ ] EventSystem

---

## 🎮 STEP 2: Room07_Manager
- [ ] Empty GameObject: `Room07_Manager`
- [ ] Script: Room07_BedroomController ✅
- [ ] Script: Room07_FlowController ✅
- [ ] Script: Room07UIManager ✅
- [ ] Script: MirrorJumpscareSequence ✅

---

## 🏠 STEP 3: Environmental Objects (7 objects)
- [ ] Bed (Type: Bed)
- [ ] WallDrawings (Type: WallDrawings)
- [ ] Nightstand_Diary (Type: Diary)
- [ ] EmilyChair (Type: Chair)
- [ ] Closet (Type: Closet)
- [ ] ReadingTable (Type: ReadingTable)
- [ ] **Lahat may Room07_Interactable script**
- [ ] **Lahat may Collider2D (Is Trigger = true)**
- [ ] **Lahat naka-assign ang UI Manager**

---

## 🧩 STEP 4: Puzzle Objects (6 objects)
- [ ] WindowCurtains (Type: WindowCurtains)
- [ ] SmallCabinet (Type: Cabinet_Cup)
- [ ] TeaPartySpot (Type: TeaParty)
- [ ] Toybox (Type: Toybox) + AudioSource
- [ ] Dollhouse (Type: Dollhouse)
- [ ] Mirror (Type: Mirror)
- [ ] **Lahat may Room07_Interactable script**
- [ ] **Lahat may Collider2D (Is Trigger = true)**
- [ ] **Lahat naka-assign ang UI Manager**

---

## 🖼️ STEP 5: UI Panels

### CurtainPanel
- [ ] Panel created (full screen, semi-transparent)
- [ ] Script: CurtainPuzzleUI ✅
- [ ] LeftCurtainButton
- [ ] RightCurtainButton
- [ ] LeftCurtainClosed image
- [ ] LeftCurtainOpen image (disabled)
- [ ] RightCurtainClosed image
- [ ] RightCurtainOpen image (disabled)
- [ ] CloseButton
- [ ] **Lahat naka-assign sa CurtainPuzzleUI**
- [ ] **Panel disabled sa start**

### TeaPartyPanel
- [ ] Panel created (full screen)
- [ ] Script: TeaPartyPuzzleUI ✅
- [ ] EmilyCupDraggable image + EventTrigger
- [ ] EmilyCupSlot transform
- [ ] SlotHighlight image
- [ ] CloseButton
- [ ] **Lahat naka-assign sa TeaPartyPuzzleUI**
- [ ] **Panel disabled sa start**

### ToyboxPanel
- [ ] Panel created (full screen)
- [ ] Script: ToyboxSlidingPuzzle ✅
- [ ] TilesParent + Grid Layout Group (3x3)
- [ ] CloseButton
- [ ] Puzzle Image assigned (game icon)
- [ ] **Lahat naka-assign sa ToyboxSlidingPuzzle**
- [ ] **Panel disabled sa start**

### DollhousePanel
- [ ] Panel created (full screen)
- [ ] Script: DollhousePuzzleUI ✅
- [ ] EmilyDollDraggable image + EventTrigger
- [ ] DollSlot transform
- [ ] SlotHighlight image
- [ ] CloseButton
- [ ] **Lahat naka-assign sa DollhousePuzzleUI**
- [ ] **Panel disabled sa start**

### Other UI
- [ ] BlackScreenCutscene (full screen, black, disabled)
- [ ] JumpscareImage (full screen, Emily image, disabled)

---

## 🎨 STEP 6: Room07UIManager References
Sa Room07_Manager Inspector:
- [ ] Curtain Panel assigned
- [ ] Tea Party Panel assigned
- [ ] Toybox Panel assigned
- [ ] Dollhouse Panel assigned
- [ ] Black Screen Cutscene assigned

---

## 👻 STEP 7: Emily AI Setup
- [ ] Emily GameObject created
- [ ] Rigidbody2D (Dynamic, Gravity=0, Freeze Rotation Z)
- [ ] Nav Mesh Agent
- [ ] Audio Source
- [ ] Sprite Renderer
- [ ] Script: EmilyGhost ✅
- [ ] Script: EmilyPerception ✅
- [ ] Script: EmilyMovement ✅
- [ ] Script: EmilyAudio ✅
- [ ] **Emily disabled sa start**

---

## 💀 STEP 8: MirrorJumpscareSequence Setup
Sa Room07_Manager Inspector:
- [ ] Emily Ghost Object assigned
- [ ] Emily Jumpscare Position (empty object behind mirror)
- [ ] Jumpscare Image assigned
- [ ] Jumpscare Sound assigned
- [ ] Lullaby Fragment 3 assigned
- [ ] Music Box Source assigned (Toybox AudioSource)
- [ ] Bedroom Door Collider assigned
- [ ] Bathroom Door assigned

---

## 🚪 STEP 9: Doors
- [ ] BedroomDoor (collider, will be locked)
- [ ] BathroomDoor (collider, escape route)

---

## 📦 STEP 10: Item Database
- [ ] emily_cup item added
  - [ ] ID: emily_cup
  - [ ] Name: Emily's Cup
  - [ ] Icon assigned
- [ ] emily_doll item added
  - [ ] ID: emily_doll
  - [ ] Name: Emily Doll
  - [ ] Icon assigned

---

## 🗺️ STEP 11: NavMesh
- [ ] Floor objects selected
- [ ] Navigation window → Bake
- [ ] NavMesh covers whole room

---

## 🎵 STEP 12: Audio Clips
- [ ] Curtain open sound
- [ ] Cup place sound
- [ ] Tea party success sound
- [ ] Tile move sound
- [ ] Puzzle success sound
- [ ] Doll place sound
- [ ] Jumpscare sound
- [ ] Lullaby Fragment 3

---

## 🧪 STEP 13: Final Testing

### Environmental Objects (6/6)
- [ ] Bed dialogue works
- [ ] Wall Drawings dialogue works
- [ ] Diary dialogue works
- [ ] Chair dialogue works
- [ ] Closet dialogue works
- [ ] Reading Table dialogue works

### Puzzles (4/4)
- [ ] Curtain puzzle works
- [ ] Tea party puzzle works
- [ ] Toybox puzzle works
- [ ] Dollhouse puzzle works

### Item Flow (2/2)
- [ ] Cup: dialogue → notification → tap
- [ ] Doll: dialogue → notification → cutscene

### Climax
- [ ] Mirror checks completion
- [ ] Jumpscare triggers
- [ ] Emily chases fast
- [ ] Can escape to bathroom

---

## ✅ COMPLETION

Kung lahat ng checkbox ay checked, **READY NA ANG ROOM 07!** 🎉

**Total Objects:** 13 interactables + 4 UI panels + Emily + Manager = ~20 objects

**Estimated Setup Time:** 2-3 hours (kung may assets na)

---

## 🆘 Need Help?

Basahin ang:
- `UNITY_SETUP_GUIDE_TAGALOG.md` - Detailed step-by-step
- `ROOM07_DEVELOPMENT_GUIDE.md` - Technical documentation
- `IMPLEMENTATION_SUMMARY.md` - Quick reference
- `BUGFIX_NOTES.md` - Common issues

Good luck! 🎮👻
