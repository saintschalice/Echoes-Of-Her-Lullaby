# Room 08 - Evidence Pickup System

## 🎯 OVERVIEW

Evidence items sa Room 08 ay may **notification system** na magpapakita ng item name at description pag na-pickup.

---

## 🔧 UNITY SETUP

### Step 1: Create Evidence Items

Para sa bawat evidence item (3 items total):

1. **Create GameObject**: `Evidence_1`, `Evidence_2`, `Evidence_3`
2. **Add Components**:
   - SpriteRenderer (assign evidence sprite)
   - BoxCollider2D or CircleCollider2D
     - **Is Trigger**: ✅ CHECKED
   - **Room08_EvidencePickup** script

3. **Configure Room08_EvidencePickup**:

```
Evidence_1:
├─ Evidence Id: "evidence_1"
├─ Evidence Name: "Torn Clothes"
├─ Evidence Description: "Torn and bloodied clothes. Someone was hurt here."
├─ Auto Pickup: ☑ (pickup on touch)
├─ Pickup Sound: [Assign pickup sound]
├─ Pickup Effect: [Optional particle effect]
└─ Debug Mode: ☑
```

```
Evidence_2:
├─ Evidence Id: "evidence_2"
├─ Evidence Name: "Apology Note"
├─ Evidence Description: "A crumpled note with tearstains. 'I'm sorry... I didn't mean to...'"
├─ Auto Pickup: ☑
├─ Pickup Sound: [Assign pickup sound]
├─ Pickup Effect: [Optional particle effect]
└─ Debug Mode: ☑
```

```
Evidence_3:
├─ Evidence Id: "evidence_3"
├─ Evidence Name: "Blood Stain"
├─ Evidence Description: "Old bloodstains on the floor. This is where it happened."
├─ Auto Pickup: ☑
├─ Pickup Sound: [Assign pickup sound]
├─ Pickup Effect: [Optional particle effect]
└─ Debug Mode: ☑
```

### Step 2: Configure Flow Controller

Sa **Room08_FlowController** GameObject:

```
Room08_FlowController:
├─ Total Evidence Items: 3
├─ Evidence Collected: 0 (runtime only)
└─ Has Collected All Evidence: ☐ (runtime only)
```

---

## 🎮 HOW IT WORKS

### Pickup Flow:

1. **Player touches evidence item**
   - Trigger enters (auto pickup)
   - OR player presses interact button (manual pickup)

2. **Pickup sequence starts**:
   - Play pickup sound
   - Show pickup effect (particles)
   - Hide sprite and collider immediately

3. **Show notification**:
   - ItemNotificationUI shows evidence name and description
   - Player must click "Continue" to dismiss
   - OR fallback to dialogue if notification UI not found

4. **Update flow controller**:
   - Call `Room08_FlowController.OnEvidenceCollected(evidenceId)`
   - Increment `evidenceCollected` counter
   - Check if all evidence collected

5. **All evidence collected**:
   - Set `hasCollectedAllEvidence = true`
   - Show dialogue: "I've collected all the evidence..."
   - Player can now interact with medicine cabinet

6. **Destroy evidence object**:
   - Evidence item is removed from scene

---

## 📋 EVIDENCE SUGGESTIONS

### Evidence 1: Torn Clothes
- **Name**: "Torn Clothes"
- **Description**: "Torn and bloodied clothes. Someone was hurt here."
- **Visual**: Torn fabric sprite, red stains
- **Location**: Near bathtub or floor

### Evidence 2: Apology Note
- **Name**: "Apology Note"
- **Description**: "A crumpled note with tearstains. 'I'm sorry... I didn't mean to...'"
- **Visual**: Crumpled paper sprite
- **Location**: On sink or counter

### Evidence 3: Blood Stain
- **Name**: "Blood Stain"
- **Description**: "Old bloodstains on the floor. This is where it happened."
- **Visual**: Dark red stain on floor
- **Location**: Floor near bathtub

### Evidence 4 (Optional): Broken Glass
- **Name**: "Broken Glass"
- **Description**: "Shattered glass from the mirror. Sharp edges glint in the dim light."
- **Visual**: Glass shards sprite
- **Location**: Floor near mirror

### Evidence 5 (Optional): Medicine Bottle
- **Name**: "Empty Medicine Bottle"
- **Description**: "An empty bottle of sleeping pills. The label is torn off."
- **Visual**: Small bottle sprite
- **Location**: Near sink or in trash

---

## 🎨 NOTIFICATION UI

### Using ItemNotificationUI:

If you already have **ItemNotificationUI** in your project:

1. **Notification will show**:
   - Evidence name (large text)
   - Evidence description (smaller text)
   - "Continue" button

2. **Player must click** to dismiss notification

3. **Automatic integration** - script uses `ItemNotificationUI.Instance`

### Fallback (No Notification UI):

If **ItemNotificationUI** not found:

1. **Dialogue will show** instead:
   - "Found: [Evidence Name]. [Description]"

2. **Player clicks** to dismiss dialogue

3. **Same functionality** - just different visual

---

## 🔊 AUDIO SETUP

### Pickup Sound:

- **Type**: Short, satisfying sound
- **Duration**: 0.2-0.5 seconds
- **Examples**:
  - Paper rustle (for note)
  - Cloth pickup (for clothes)
  - Item pickup (generic)
- **Volume**: Medium (not too loud)

### Recommended Sounds:

1. **Evidence 1 (Clothes)**: Cloth rustle sound
2. **Evidence 2 (Note)**: Paper pickup sound
3. **Evidence 3 (Blood)**: Subtle "discovery" sound

---

## ✅ TESTING CHECKLIST

### Test Evidence Pickup:

1. **Start Room 08**
   - ✅ 3 evidence items visible in scene
   - ✅ Each has collider (Is Trigger ✓)
   - ✅ Each has Room08_EvidencePickup script

2. **Walk to Evidence 1**
   - ✅ Auto pickup triggers
   - ✅ Pickup sound plays
   - ✅ Sprite disappears immediately
   - ✅ Notification shows (or dialogue)
   - ✅ Evidence name and description correct

3. **Click Continue**
   - ✅ Notification closes
   - ✅ Evidence object destroyed
   - ✅ Console shows: "Evidence collected: evidence_1 (1/3)"

4. **Collect Evidence 2 and 3**
   - ✅ Same process for each
   - ✅ Counter updates: (2/3), (3/3)

5. **After All Evidence Collected**
   - ✅ Console shows: "All evidence collected!"
   - ✅ Dialogue shows: "I've collected all the evidence..."
   - ✅ `hasCollectedAllEvidence = true`

6. **Try Medicine Cabinet**
   - ✅ Cabinet now opens (no longer blocked)
   - ✅ Can get hammer

---

## 🐛 TROUBLESHOOTING

### Issue: "No notification shows"

**Possible Causes**:
1. ItemNotificationUI not in scene
2. ItemNotificationUI.Instance is null

**Solution**:
1. Check if ItemNotificationUI exists in scene
2. Script will fallback to dialogue automatically
3. Check Console for warnings

### Issue: "Evidence doesn't disappear"

**Possible Causes**:
1. SpriteRenderer not assigned
2. Collider not assigned

**Solution**:
1. Check if GameObject has SpriteRenderer
2. Check if GameObject has Collider2D
3. Check Console for errors

### Issue: "Pickup doesn't trigger"

**Possible Causes**:
1. Collider not set to "Is Trigger"
2. Player doesn't have "Player" tag
3. Auto Pickup is disabled

**Solution**:
1. Set Collider2D → Is Trigger ✓
2. Check Player GameObject has "Player" tag
3. Enable Auto Pickup in Inspector

### Issue: "Counter doesn't update"

**Possible Causes**:
1. Room08_FlowController not in scene
2. OnEvidenceCollected() not called

**Solution**:
1. Check if Room08_FlowController exists
2. Check Console for debug logs
3. Enable Debug Mode in script

### Issue: "Cabinet still blocked after collecting all evidence"

**Possible Causes**:
1. `hasCollectedAllEvidence` not set to true
2. Evidence counter not reaching total

**Solution**:
1. Check `evidenceCollected` value in Inspector (runtime)
2. Check `totalEvidenceItems` matches actual items (default: 3)
3. Check Console for "All evidence collected!" message

---

## 💡 TIPS

### For Better Experience:

1. **Visual Feedback**:
   - Use particle effects for pickup (sparkles, glow)
   - Fade out sprite instead of instant hide (optional)
   - Highlight evidence items (outline, glow)

2. **Audio Feedback**:
   - Different sounds for different evidence types
   - Satisfying pickup sound
   - Completion sound when all collected

3. **Placement**:
   - Spread evidence around room
   - Make them visible but not too obvious
   - Guide player's exploration

4. **Descriptions**:
   - Keep descriptions short (1-2 sentences)
   - Make them atmospheric and story-relevant
   - Hint at what happened in the bathroom

---

## 📝 ALTERNATIVE: Manual Pickup

If you want **manual pickup** (player must press button):

1. **Disable Auto Pickup**:
   - Room08_EvidencePickup → Auto Pickup: ☐

2. **Add IInteractable**:
   - Evidence implements IInteractable
   - OnInteract() calls PickupEvidence()

3. **Show Interact Prompt**:
   - "Press E to examine evidence"
   - Player must press button to pickup

---

## 🎯 SCENE HIERARCHY

```
Room08_Lisa'sBathroom (Scene)
├─ Room08_FlowController (GameObject)
│   └─ Room08_FlowController (Script)
│       └─ Total Evidence Items: 3
│
├─ Evidence_1 (GameObject)
│   ├─ SpriteRenderer (torn clothes sprite)
│   ├─ BoxCollider2D (Is Trigger ✓)
│   └─ Room08_EvidencePickup (Script)
│       ├─ Evidence Id: "evidence_1"
│       ├─ Evidence Name: "Torn Clothes"
│       └─ Evidence Description: "..."
│
├─ Evidence_2 (GameObject)
│   ├─ SpriteRenderer (note sprite)
│   ├─ BoxCollider2D (Is Trigger ✓)
│   └─ Room08_EvidencePickup (Script)
│       ├─ Evidence Id: "evidence_2"
│       ├─ Evidence Name: "Apology Note"
│       └─ Evidence Description: "..."
│
└─ Evidence_3 (GameObject)
    ├─ SpriteRenderer (blood stain sprite)
    ├─ BoxCollider2D (Is Trigger ✓)
    └─ Room08_EvidencePickup (Script)
        ├─ Evidence Id: "evidence_3"
        ├─ Evidence Name: "Blood Stain"
        └─ Evidence Description: "..."
```

---

**Setup complete! Test evidence pickup with notifications!** 🎮✨

