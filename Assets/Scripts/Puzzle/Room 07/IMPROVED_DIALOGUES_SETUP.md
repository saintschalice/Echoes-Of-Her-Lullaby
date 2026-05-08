# Room 07 - Improved Dialogues & Rug Transition Setup

## 🎯 What's New

### 1. **Story-Driven Dialogues**
All dialogues have been rewritten to create a cohesive, emotional narrative that builds throughout the sequence.

### 2. **Rug Transition System**
New script that allows Lisa to move to the next room ONLY after completing everything including the mirror interaction.

---

## 📖 Dialogue Improvements

### Before (Generic):
```
"Child's bed has two pillow indentations..."
"Crayon drawings show two figures..."
"Three cups set on floor..."
```

### After (Story-Driven):
```
"A child's bed... with two pillows. There's a note pinned to the second one. 
'For my friend Emily - she keeps me safe at night.' 
Emily... why does that name make my heart ache?"

"Crayon drawings on the wall... Two little girls holding hands. 
One is labeled 'Me' and the other... 'Emily.' 
We look so happy together. Were we... friends?"

"This is where we had our tea parties. Every day after school. 
Emily would tell me stories about faraway places where children were safe and loved."
```

---

## 🎮 Complete Story Flow

### 1. **Intro** - Familiarity
> "This room... it feels so familiar. Like I've been here a thousand times before. But that's impossible... isn't it?"

### 2. **Bed** - First Clue
> "Emily... why does that name make my heart ache?"

### 3. **Wall** - Friendship
> "We look so happy together. Were we... friends?"

### 4. **Diary** - Protection
> "Emily was always there when I needed her."

### 5. **Curtains** - Fear
> "What was I so afraid of? What was Emily protecting me from?"

### 6. **Cabinet** - Sacred Object
> "Emily's cup... her special cup. She never let anyone else touch it."

### 7. **Tea Party** - Memory
> "She made the pain go away. She made me feel... safe."

### 8. **Chair** - Presence
> "This was her favorite spot. She would watch over me while I slept."

### 9. **Closet** - Hiding
> "I hid here. And Emily... Emily would hide with me."

### 10. **Toybox** - Letters
> "'Dear Emily, thank you for making mommy stop hurting me yesterday.' ...I wrote these. I wrote all of these."

### 11. **Doll** - Creation
> "Emily's doll... I made this for her. My best friend. My only friend."

### 12. **Dollhouse** - Companionship
> "The little girl isn't alone anymore. Emily is with her. Emily is always with her."

### 13. **Reading Table** - Promise
> "'Emily promises she'll never leave me.' She kept that promise... didn't she?"

### 14. **Mirror** - Truth
> "I've remembered everything. I need to see the truth."

### 15. **Rug** - Moving Forward
> "Leaving this room means leaving Emily behind. ...I have to go. I have to understand what happened to us."

---

## 🪜 Rug Transition Setup

### Step 1: Create Rug GameObject

```
1. In Room 07 scene, create GameObject: "Rug"
2. Add Sprite Renderer (rug sprite)
3. Add Box Collider 2D
4. Add Room07_RugTransition component
```

### Step 2: Configure Room07_RugTransition

```
Inspector → Room07_RugTransition:

Scene Transition:
  Next Scene Name: "Room08_Lisa'sBathroom"
  Transition Delay: 1

Visual Feedback:
  Interaction Prompt: (optional UI prompt)
  Rug Move Sound: (sound effect)
  Trapdoor Open Sound: (sound effect)

Animation (Optional):
  Rug Animator: (if you have animation)
  Rug Move Animation Trigger: "Move"
```

### Step 3: Position the Rug

```
Place the rug in the room where you want the exit to be.
Common locations:
- Center of room
- Near the door
- Corner of room
```

---

## 🎯 How Rug Works

### Before Mirror Interaction:
```
Player interacts with Rug
  ↓
Shows dialogue: "The rug... there's something underneath it. 
But I can't move it yet. I need to face everything in this room first. 
I need to face Emily."
  ↓
Cannot transition yet
```

### After Mirror Interaction:
```
Player interacts with Rug
  ↓
Dialogue: "The rug... I can move it now. There's a trapdoor underneath..."
  ↓
Rug moves (animation + sound)
  ↓
Trapdoor opens (sound)
  ↓
Dialogue: "The trapdoor opens to darkness below. Another room. Another memory..."
  ↓
Fade out
  ↓
Load next scene (Room 08)
```

---

## 📋 Requirements for Rug

The rug can ONLY be used when:
- ✅ All environmental checks complete
- ✅ All puzzles complete
- ✅ Mirror interaction complete
- ✅ `hasInteractedWithMirror = true`

---

## 🎬 Complete Sequence

```
1. Enter Room 07 (Intro)
2. Bed → Wall → Diary (Environmental)
3. Curtains → Cabinet → Tea Party (Puzzles)
4. Chair → Closet (Environmental)
5. Toybox → Get Doll (Puzzle)
6. Dollhouse (Puzzle)
7. Reading Table (Environmental)
8. Mirror (Final trigger + jumpscare)
9. Rug (Transition to next room) ← NEW!
```

---

## 🔧 Files Created/Updated

### New Files:
1. **Room07_ImprovedDialogues.cs** - All story-driven dialogues
2. **Room07_RugTransition.cs** - Rug transition script
3. **IMPROVED_DIALOGUES_SETUP.md** - This guide

### Updated Files:
1. **Room07_Interactable.cs** - Uses improved dialogues
2. **Room07_FlowController.cs** - Added `hasInteractedWithMirror` flag
3. **Room07UIManager.cs** - Updated panel dialogues
4. **CabinetItemPanel.cs** - Updated cup dialogue

---

## 🧪 Testing

### Test 1: Dialogue Flow
```
1. Play through entire sequence
2. Read each dialogue
3. Verify story builds naturally
4. Check emotional progression
```

### Test 2: Rug Before Mirror
```
1. Complete all tasks EXCEPT mirror
2. Try to use rug
3. Should show: "I need to face Emily first"
4. Cannot transition yet ✓
```

### Test 3: Rug After Mirror
```
1. Complete everything including mirror
2. Interact with rug
3. Should show transition dialogue
4. Rug moves, trapdoor opens
5. Loads next scene ✓
```

### Test 4: Full Playthrough
```
1. Start from room entry
2. Follow complete sequence
3. Interact with mirror
4. Use rug to transition
5. Verify next room loads ✓
```

---

## 💡 Dialogue Highlights

### Most Emotional Moments:

**Toybox Letters:**
> "'Dear Emily, thank you for making mommy stop hurting me yesterday.' 
> 'Dear Emily, you're the only one who loves me.' 
> 'Dear Emily, please don't ever leave me.' 
> ...I wrote these. I wrote all of these."

**Closet Discovery:**
> "I hid here. When mommy was angry, when the shouting got too loud, I would hide here. 
> And Emily... Emily would hide with me. She'd hold my hand in the darkness and sing to me."

**Mirror Realization:**
> "I've remembered everything. Every moment. Every memory. 
> Emily was always there. Through the pain, through the fear, through the loneliness. 
> She was my protector. My friend."

**Rug Farewell:**
> "Leaving this room means leaving Emily behind. 
> ...I have to go. I have to understand what happened to us."

---

## 🎯 Key Features

### Story-Driven:
- ✅ Each dialogue builds on previous ones
- ✅ Emotional progression throughout
- ✅ Reveals Lisa's past gradually
- ✅ Creates empathy for both Lisa and Emily

### Prerequisite System:
- ✅ Strict linear sequence
- ✅ Helpful validation messages
- ✅ Smart hints for mirror
- ✅ Clear progression feedback

### Transition System:
- ✅ Rug only works after everything complete
- ✅ Smooth transition with dialogue
- ✅ Sound effects and animation support
- ✅ Loads next room automatically

---

## 📝 Next Steps

### 1. Test All Dialogues
```
Play through and verify each dialogue feels natural and builds the story.
```

### 2. Setup Rug GameObject
```
Create rug in scene, add script, configure next scene name.
```

### 3. Create Next Room
```
Make sure "Room08_Lisa'sBathroom" scene exists and is in Build Settings.
```

### 4. Add Sound Effects
```
Assign rug move sound and trapdoor open sound for better immersion.
```

### 5. Optional: Add Animation
```
Create rug move animation for visual feedback.
```

---

## 🎮 Player Experience

**Before:**
- Generic dialogues
- No emotional connection
- Unclear story
- Abrupt ending

**After:**
- Story-driven narrative ✓
- Emotional journey ✓
- Clear character development ✓
- Smooth transition to next room ✓

---

**The dialogues now tell a complete, emotional story about Lisa and Emily's relationship!** 📖✨

**The rug provides a natural transition point to continue the journey!** 🪜🚪

