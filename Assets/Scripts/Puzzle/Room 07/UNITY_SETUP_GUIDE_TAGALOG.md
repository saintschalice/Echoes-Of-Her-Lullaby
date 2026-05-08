# Room 07 Unity Setup Guide (Tagalog)

## 📋 Kailangan Mo Bago Magsimula

### Assets na Kailangan:
- [ ] Game icon sprite (para sa sliding puzzle)
- [ ] Emily's Cup sprite
- [ ] Emily Doll sprite
- [ ] Curtain sprites (left closed, left open, right closed, right open)
- [ ] Jumpscare image (full-screen Emily)
- [ ] Sound effects (curtain, cup, doll, jumpscare, lullaby fragment 3)

---

## STEP 1: Scene Setup

### 1.1 Gumawa ng Bagong Scene
1. Sa Unity, right-click sa `Assets/Scenes` folder
2. Create → Scene
3. Pangalan: `Room07_Lisa'sBedroom`
4. Double-click para buksan

### 1.2 I-add ang Basic Objects
1. **Player**
   - Drag ang Player prefab sa scene
   - Siguraduhing may `JoystickPlayerController` script

2. **Main Camera**
   - Dapat may `CameraFollow` script
   - I-assign ang Player sa target

3. **Canvas**
   - Right-click sa Hierarchy → UI → Canvas
   - Canvas Scaler: Scale With Screen Size
   - Reference Resolution: 1920x1080

4. **EventSystem**
   - Automatic na magkakaroon kapag gumawa ng Canvas

---

## STEP 2: Gumawa ng Manager Object

### 2.1 Create Room07_Manager
1. Right-click sa Hierarchy → Create Empty
2. Pangalan: `Room07_Manager`
3. Position: (0, 0, 0)

### 2.2 I-add ang Scripts sa Manager
1. Select `Room07_Manager`
2. Sa Inspector, click **Add Component**
3. I-add ang mga sumusunod na scripts:
   - `Room07_BedroomController`
   - `Room07_FlowController`
   - `Room07UIManager`
   - `MirrorJumpscareSequence`

### 2.3 I-setup ang Room07_FlowController
Sa Inspector ng `Room07_Manager`:

**Room07_FlowController:**
- Emily AI: (huwag muna i-assign, gagawin pa natin)
- Bedroom Door Collider: (huwag muna)
- Toybox Music Box: (huwag muna)
- Lullaby Fragment 3: (i-drag ang audio clip)

---

## STEP 3: Gumawa ng Interactable Objects

### 3.1 Environmental Objects (Storytelling)

**IMPORTANTE:** Lahat ng objects ay kailangan ng:
1. **Collider2D** (Box Collider 2D o Circle Collider 2D)
2. **Is Trigger = TRUE** (very important!)
3. **Room07_Interactable** script
4. **UI Manager** reference (drag Room07_Manager)
5. **Correct Object Type** sa dropdown

Para sa bawat object:
1. Right-click sa Hierarchy → Create Empty
2. I-rename based sa object type
3. Add Component → Box Collider 2D (o Circle Collider 2D)
4. Check "Is Trigger"
5. Add Component → `Room07_Interactable`

**Mga Objects na Gagawin:**

#### A. Bed
- Pangalan: `Bed`
- Position: Kung saan mo gusto
- `Room07_Interactable`:
  - My Type: **Bed**
  - UI Manager: (i-drag ang Room07_Manager)

#### B. Wall Drawings
- Pangalan: `WallDrawings`
- `Room07_Interactable`:
  - My Type: **WallDrawings**
  - UI Manager: (i-drag ang Room07_Manager)

#### C. Nightstand/Diary
- Pangalan: `Nightstand_Diary`
- `Room07_Interactable`:
  - My Type: **Diary**
  - UI Manager: (i-drag ang Room07_Manager)

#### D. Emily's Chair
- Pangalan: `EmilyChair`
- `Room07_Interactable`:
  - My Type: **Chair**
  - UI Manager: (i-drag ang Room07_Manager)

#### E. Closet
- Pangalan: `Closet`
- `Room07_Interactable`:
  - My Type: **Closet**
  - UI Manager: (i-drag ang Room07_Manager)

#### F. Reading Table
- Pangalan: `ReadingTable`
- `Room07_Interactable`:
  - My Type: **ReadingTable**
  - UI Manager: (i-drag ang Room07_Manager)

### 3.2 Puzzle Objects

#### G. Window Curtains
- Pangalan: `WindowCurtains`
- `Room07_Interactable`:
  - My Type: **WindowCurtains**
  - UI Manager: (i-drag ang Room07_Manager)

#### H. Small Cabinet (May Cup)
- Pangalan: `SmallCabinet`
- `Room07_Interactable`:
  - My Type: **Cabinet_Cup**
  - UI Manager: (i-drag ang Room07_Manager)

#### I. Tea Party Spot
- Pangalan: `TeaPartySpot`
- `Room07_Interactable`:
  - My Type: **TeaParty**
  - UI Manager: (i-drag ang Room07_Manager)

#### J. Toybox
- Pangalan: `Toybox`
- Add Component → Audio Source (para sa music box)
- `Room07_Interactable`:
  - My Type: **Toybox**
  - UI Manager: (i-drag ang Room07_Manager)

#### K. Dollhouse
- Pangalan: `Dollhouse`
- `Room07_Interactable`:
  - My Type: **Dollhouse**
  - UI Manager: (i-drag ang Room07_Manager)

#### L. Mirror
- Pangalan: `Mirror`
- `Room07_Interactable`:
  - My Type: **Mirror**
  - UI Manager: (i-drag ang Room07_Manager)

---

## STEP 4: Gumawa ng UI Panels

### 4.1 Curtain Panel

1. Right-click sa Canvas → UI → Panel
2. Pangalan: `CurtainPanel`
3. I-stretch para full screen (Anchor: Stretch-Stretch)
4. Background color: Semi-transparent black (0, 0, 0, 200)

**I-add ang Components:**
- Add Component → `CurtainPuzzleUI`

**Gumawa ng Buttons at Images:**

A. **Left Curtain Button**
   - Right-click sa CurtainPanel → UI → Button
   - Pangalan: `LeftCurtainButton`
   - Position: Left side ng screen

B. **Right Curtain Button**
   - Right-click sa CurtainPanel → UI → Button
   - Pangalan: `RightCurtainButton`
   - Position: Right side ng screen

C. **Curtain Images**
   - Right-click sa CurtainPanel → UI → Image
   - Gumawa ng 4 images:
     - `LeftCurtainClosed` (sprite: closed curtain)
     - `LeftCurtainOpen` (sprite: open curtain, initially hidden)
     - `RightCurtainClosed` (sprite: closed curtain)
     - `RightCurtainOpen` (sprite: open curtain, initially hidden)

D. **Close Button**
   - Right-click sa CurtainPanel → UI → Button
   - Pangalan: `CloseButton`
   - Position: Top-right corner
   - Text: "X"

**I-assign sa CurtainPuzzleUI:**
- Curtain Panel: (i-drag ang CurtainPanel)
- Left Curtain Button: (i-drag ang LeftCurtainButton)
- Right Curtain Button: (i-drag ang RightCurtainButton)
- Close Button: (i-drag ang CloseButton)
- Left Curtain Closed: (i-drag ang image)
- Left Curtain Open: (i-drag ang image)
- Right Curtain Closed: (i-drag ang image)
- Right Curtain Open: (i-drag ang image)
- Curtain Open Sound: (i-drag ang audio clip)

**I-disable ang Panel:**
- Uncheck ang checkbox sa taas ng Inspector

---

### 4.2 Tea Party Panel

1. Right-click sa Canvas → UI → Panel
2. Pangalan: `TeaPartyPanel`
3. Full screen, semi-transparent background

**I-add ang Components:**
- Add Component → `TeaPartyPuzzleUI`

**Gumawa ng Elements:**

A. **Background Image**
   - Right-click sa TeaPartyPanel → UI → Image
   - Sprite: Tea party scene (3 cups on floor)

B. **Emily Cup Draggable**
   - Right-click sa TeaPartyPanel → UI → Image
   - Pangalan: `EmilyCupDraggable`
   - Sprite: Emily's cup
   - Add Component → Event Trigger (automatic na i-setup ng script)

C. **Emily Cup Slot**
   - Right-click sa TeaPartyPanel → UI → Image
   - Pangalan: `EmilyCupSlot`
   - Position: Kung saan dapat ilagay ang cup
   - Color: Yellow (para makita ang target)

D. **Slot Highlight**
   - Right-click sa EmilyCupSlot → UI → Image
   - Pangalan: `SlotHighlight`
   - Sprite: Glow effect
   - Color: White (normal), Yellow (highlighted)

E. **Close Button**
   - Same as curtain panel

**I-assign sa TeaPartyPuzzleUI:**
- Tea Party Panel: (i-drag ang TeaPartyPanel)
- Close Button: (i-drag ang CloseButton)
- Emily Cup Draggable: (i-drag ang EmilyCupDraggable)
- Emily Cup Slot: (i-drag ang EmilyCupSlot transform)
- Slot Highlight: (i-drag ang SlotHighlight image)
- Snap Distance: 50
- Cup Place Sound: (i-drag ang audio clip)
- Success Sound: (i-drag ang audio clip)

**I-disable ang Panel**

---

### 4.3 Toybox Panel (Sliding Puzzle)

1. Right-click sa Canvas → UI → Panel
2. Pangalan: `ToyboxPanel`
3. Full screen

**I-add ang Components:**
- Add Component → `ToyboxSlidingPuzzle`

**Gumawa ng Grid:**

A. **Tiles Parent**
   - Right-click sa ToyboxPanel → UI → Image
   - Pangalan: `TilesParent`
   - Size: 600x600 (square)
   - Center ng screen
   - Add Component → Grid Layout Group
     - Cell Size: 200x200
     - Spacing: 5x5
     - Constraint: Fixed Column Count = 3

B. **Close Button**
   - Same as other panels

**I-assign sa ToyboxSlidingPuzzle:**
- Toybox Panel: (i-drag ang ToyboxPanel)
- Close Button: (i-drag ang CloseButton)
- Tiles Parent: (i-drag ang TilesParent transform)
- Puzzle Image: (i-drag ang game icon sprite)
- Grid Size: 3
- Shuffle Moves: 20
- Tile Move Sound: (i-drag ang audio clip)
- Success Sound: (i-drag ang audio clip)

**I-disable ang Panel**

---

### 4.4 Dollhouse Panel

1. Right-click sa Canvas → UI → Panel
2. Pangalan: `DollhousePanel`
3. Full screen

**I-add ang Components:**
- Add Component → `DollhousePuzzleUI`

**Gumawa ng Elements:**

A. **Dollhouse Background**
   - Right-click sa DollhousePanel → UI → Image
   - Sprite: Dollhouse image

B. **Emily Doll Draggable**
   - Right-click sa DollhousePanel → UI → Image
   - Pangalan: `EmilyDollDraggable`
   - Sprite: Emily doll
   - Add Component → Event Trigger

C. **Doll Slot**
   - Right-click sa DollhousePanel → UI → Image
   - Pangalan: `DollSlot`
   - Position: Kung saan dapat ilagay ang doll

D. **Slot Highlight**
   - Same as tea party

E. **Close Button**

**I-assign sa DollhousePuzzleUI:**
- Dollhouse Panel: (i-drag ang DollhousePanel)
- Close Button: (i-drag ang CloseButton)
- Emily Doll Draggable: (i-drag ang EmilyDollDraggable)
- Doll Slot: (i-drag ang DollSlot transform)
- Slot Highlight: (i-drag ang SlotHighlight image)
- Snap Distance: 50
- Doll Place Sound: (i-drag ang audio clip)
- Success Sound: (i-drag ang audio clip)

**I-disable ang Panel**

---

### 4.5 Black Screen Cutscene

1. Right-click sa Canvas → UI → Image
2. Pangalan: `BlackScreenCutscene`
3. Full screen, solid black
4. I-disable

---

### 4.6 Jumpscare Image

1. Right-click sa Canvas → UI → Image
2. Pangalan: `JumpscareImage`
3. Full screen
4. Sprite: Scary Emily image
5. I-disable

---

## STEP 5: I-setup ang Room07UIManager

Select ang `Room07_Manager`, sa Inspector:

**Room07UIManager:**
- Curtain Panel: (i-drag ang CurtainPanel)
- Tea Party Panel: (i-drag ang TeaPartyPanel)
- Toybox Panel: (i-drag ang ToyboxPanel)
- Dollhouse Panel: (i-drag ang DollhousePanel)
- Black Screen Cutscene: (i-drag ang BlackScreenCutscene)

---

## STEP 6: I-setup si Emily AI

### 6.1 Gumawa ng Emily GameObject

1. Right-click sa Hierarchy → Create Empty
2. Pangalan: `Emily`
3. Position: Kung saan mo gusto (malayo sa player)

### 6.2 I-add ang Components

**Required Components:**
- Rigidbody2D
  - Body Type: Dynamic
  - Gravity Scale: 0
  - Constraints: Freeze Rotation Z
- Nav Mesh Agent
  - Agent Type: Humanoid
  - Speed: 0.5
  - Angular Speed: 120
  - Acceleration: 8
- Audio Source
- Sprite Renderer (para makita si Emily)
- Animator (kung may animation)

**AI Scripts:**
- `EmilyGhost`
- `EmilyPerception`
- `EmilyMovement`
- `EmilyAudio`
- `EmilyAnimator` (optional)

### 6.3 I-configure ang EmilyGhost

**Speed Settings:**
- Patrol Speed: 0.5
- Investigate Speed: 0.5
- Hunt Speed: 0.5 (automatic na magiging 3.5 sa chase)

**State Timers:**
- Search Time: 12
- Cooldown Time: 18
- Lost LOS Time: 1.8 (automatic na magiging 5 sa chase)

### 6.4 I-disable si Emily
- Uncheck ang checkbox sa taas ng Inspector
- Mag-aactivate lang siya after jumpscare

---

## STEP 7: I-setup ang MirrorJumpscareSequence

Select ang `Room07_Manager`, sa Inspector:

**MirrorJumpscareSequence:**

**Jumpscare Elements:**
- Emily Ghost Object: (i-drag ang Emily GameObject)
- Emily Jumpscare Position: (gumawa ng empty object sa likod ng mirror)
- Jumpscare Image: (i-drag ang JumpscareImage)
- Jumpscare Duration: 2

**Audio:**
- Jumpscare Sound: (i-drag ang audio clip)
- Lullaby Fragment 3: (i-drag ang audio clip)
- Music Box Source: (i-drag ang Audio Source ng Toybox)

**Door Locking:**
- Bedroom Door Collider: (i-drag ang bedroom door)
- Bathroom Door: (i-drag ang bathroom door)

**Camera Shake:**
- Shake Intensity: 0.3
- Shake Duration: 0.5

---

## STEP 8: Gumawa ng Doors

### 8.1 Bedroom Door (Main Exit)
1. Create Empty → Pangalan: `BedroomDoor`
2. Add Component → Box Collider 2D
3. Tag: "Door" (o custom tag)
4. I-assign sa MirrorJumpscareSequence

### 8.2 Bathroom Door (Escape Route)
1. Create Empty → Pangalan: `BathroomDoor`
2. Add Component → Box Collider 2D
3. Tag: "Door"
4. Script para mag-load ng next scene (Room 08)

---

## STEP 9: Item Database Setup

1. Open ang ItemDatabase (Assets/Resources/Data)
2. I-add ang dalawang items:

### Emily's Cup
- Item ID: `emily_cup`
- Item Name: "Emily's Cup"
- Description: "A small teacup with Emily's name on it"
- Item Icon: (i-drag ang cup sprite)
- Is Usable: false
- Is Consumable: false

### Emily Doll
- Item ID: `emily_doll`
- Item Name: "Emily Doll"
- Description: "A handmade doll representing Emily"
- Item Icon: (i-drag ang doll sprite)
- Is Usable: false
- Is Consumable: false

---

## STEP 10: NavMesh Setup

1. Window → AI → Navigation
2. Select lahat ng floor objects
3. Sa Navigation window, click "Bake"
4. Siguraduhing walkable ang buong room

---

## STEP 11: Testing Checklist

### Phase 1: Environmental Objects
- [ ] Click Bed → May dialogue
- [ ] Click Wall Drawings → May dialogue
- [ ] Click Diary → May dialogue
- [ ] Click Chair → May dialogue
- [ ] Click Closet → May dialogue
- [ ] Click Reading Table → May dialogue

### Phase 2: Curtain Puzzle
- [ ] Click Window Curtains → Bumubukas ang panel
- [ ] Click left button → Bumubukas ang left curtain
- [ ] Click right button → Bumubukas ang right curtain
- [ ] Kapag both open → Automatic na nagsasara ang panel
- [ ] May dialogue after

### Phase 3: Cup Pickup
- [ ] Click Small Cabinet → May dialogue
- [ ] Tapos ng dialogue → May notification
- [ ] Tap notification → Nawawala
- [ ] Cup nasa inventory

### Phase 4: Tea Party
- [ ] Click Tea Party Spot → Bumubukas ang panel
- [ ] Drag cup → Nag-highlight ang slot
- [ ] Drop sa slot → Success!
- [ ] May cutscene (black screen)
- [ ] Cup nawala sa inventory

### Phase 5: Toybox Puzzle
- [ ] Click Toybox → Bumubukas ang sliding puzzle
- [ ] Pwede i-drag ang tiles
- [ ] Solve puzzle → Success!
- [ ] Panel nagsasara
- [ ] Click ulit ang Toybox → May dialogue
- [ ] Tapos ng dialogue → May notification
- [ ] Doll nasa inventory
- [ ] May cutscene

### Phase 6: Dollhouse
- [ ] Click Dollhouse → Bumubukas ang panel
- [ ] Drag doll → Nag-highlight ang slot
- [ ] Drop sa slot → Success!
- [ ] Doll nawala sa inventory

### Phase 7: Mirror Jumpscare
- [ ] Click Mirror → Check kung complete lahat
- [ ] Kung hindi complete → "Missing something" dialogue
- [ ] Kung complete → Jumpscare!
- [ ] Emily lumilitaw
- [ ] May lullaby
- [ ] Bedroom door locked
- [ ] Emily humahabol (mabilis!)
- [ ] Pwede tumakas sa bathroom

---

## STEP 12: Common Problems

### Problem: "NullReferenceException"
**Solution:** Check kung lahat ng references ay naka-assign sa Inspector

### Problem: Panels hindi lumalabas
**Solution:** 
- Check kung naka-disable ang panel sa start
- Check kung naka-assign ang panel sa UIManager

### Problem: Drag & drop hindi gumagana
**Solution:**
- Check kung may EventTrigger component
- Check kung may GraphicRaycaster sa Canvas

### Problem: Emily hindi gumagalaw
**Solution:**
- Check kung may NavMesh
- Check kung naka-enable si Emily
- Check kung isPaused = false

### Problem: Jumpscare hindi nag-trigger
**Solution:**
- Check kung lahat ng puzzles ay solved
- Check kung naka-assign lahat sa MirrorJumpscareSequence

---

## ✅ Tapos Na!

Kapag natapos mo na lahat ng steps, dapat gumagana na ang Room 07! 

**Test Flow:**
1. Enter room → Intro dialogue ✅
2. Explore objects → May dialogue ✅
3. Solve curtains → Get cup ✅
4. Tea party → Cutscene ✅
5. Toybox puzzle → Get doll ✅
6. Dollhouse → Complete ✅
7. Mirror → Jumpscare → Chase! ✅

Good luck! 🎮👻
