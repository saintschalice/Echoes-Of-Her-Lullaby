# Room 08 - Evidence Pickup System (Tagalog)

## 🎯 OVERVIEW

Ang evidence items sa Room 08 ay may **notification system** na magpapakita ng item name at description pag na-pickup.

---

## 🔧 UNITY SETUP

### Step 1: Gumawa ng Evidence Items

Para sa bawat evidence item (3 items total):

1. **Create GameObject**: `Evidence_1`, `Evidence_2`, `Evidence_3`
2. **Add Components**:
   - SpriteRenderer (i-assign ang evidence sprite)
   - BoxCollider2D o CircleCollider2D
     - **Is Trigger**: ✅ CHECKED
   - **Room08_EvidencePickup** script

3. **I-configure ang Room08_EvidencePickup**:

```
Evidence_1:
├─ Evidence Id: "evidence_1"
├─ Evidence Name: "Torn Clothes"
├─ Evidence Description: "Torn and bloodied clothes. Someone was hurt here."
├─ Auto Pickup: ☑ (pickup on touch)
├─ Pickup Sound: [I-assign ang pickup sound]
├─ Pickup Effect: [Optional particle effect]
└─ Debug Mode: ☑
```

```
Evidence_2:
├─ Evidence Id: "evidence_2"
├─ Evidence Name: "Apology Note"
├─ Evidence Description: "A crumpled note with tearstains. 'I'm sorry... I didn't mean to...'"
├─ Auto Pickup: ☑
├─ Pickup Sound: [I-assign ang pickup sound]
├─ Pickup Effect: [Optional particle effect]
└─ Debug Mode: ☑
```

```
Evidence_3:
├─ Evidence Id: "evidence_3"
├─ Evidence Name: "Blood Stain"
├─ Evidence Description: "Old bloodstains on the floor. This is where it happened."
├─ Auto Pickup: ☑
├─ Pickup Sound: [I-assign ang pickup sound]
├─ Pickup Effect: [Optional particle effect]
└─ Debug Mode: ☑
```

---

## 🎮 PAANO GUMAGANA

### Pickup Flow:

1. **Player touches evidence item**
   - Trigger enters (auto pickup)
   - O player presses interact button (manual pickup)

2. **Magsisimula ang pickup sequence**:
   - Tutugtog ang pickup sound
   - Ipapakita ang pickup effect (particles)
   - Mawawala agad ang sprite at collider

3. **Ipapakita ang notification**:
   - ItemNotificationUI shows evidence name at description
   - Kailangan i-click ng player ang "Continue"
   - O fallback sa dialogue kung walang notification UI

4. **I-update ang flow controller**:
   - Tatawagan ang `Room08_FlowController.OnEvidenceCollected(evidenceId)`
   - Tataasan ang `evidenceCollected` counter
   - Check kung nakuha na lahat ng evidence

5. **Lahat ng evidence nakuha na**:
   - I-set ang `hasCollectedAllEvidence = true`
   - Ipapakita ang dialogue: "I've collected all the evidence..."
   - Pwede na i-interact ang medicine cabinet

6. **I-destroy ang evidence object**:
   - Tatanggalin ang evidence item sa scene

---

## 📋 EVIDENCE SUGGESTIONS

### Evidence 1: Torn Clothes
- **Name**: "Torn Clothes"
- **Description**: "Torn and bloodied clothes. Someone was hurt here."
- **Visual**: Torn fabric sprite, red stains
- **Location**: Malapit sa bathtub o floor

### Evidence 2: Apology Note
- **Name**: "Apology Note"
- **Description**: "A crumpled note with tearstains. 'I'm sorry... I didn't mean to...'"
- **Visual**: Crumpled paper sprite
- **Location**: Sa sink o counter

### Evidence 3: Blood Stain
- **Name**: "Blood Stain"
- **Description**: "Old bloodstains on the floor. This is where it happened."
- **Visual**: Dark red stain on floor
- **Location**: Floor malapit sa bathtub

---

## ✅ PAANO I-TEST

### Test Evidence Pickup:

1. **Start Room 08**
   - ✅ 3 evidence items visible sa scene
   - ✅ Bawat isa may collider (Is Trigger ✓)
   - ✅ Bawat isa may Room08_EvidencePickup script

2. **Lumapit sa Evidence 1**
   - ✅ Auto pickup triggers
   - ✅ Tumugtog ang pickup sound
   - ✅ Nawala agad ang sprite
   - ✅ Lumabas ang notification (o dialogue)
   - ✅ Tama ang evidence name at description

3. **I-click ang Continue**
   - ✅ Nagsara ang notification
   - ✅ Na-destroy ang evidence object
   - ✅ Console shows: "Evidence collected: evidence_1 (1/3)"

4. **Kolektahin ang Evidence 2 at 3**
   - ✅ Same process para sa bawat isa
   - ✅ Nag-update ang counter: (2/3), (3/3)

5. **Pagkatapos Makuha Lahat**
   - ✅ Console shows: "All evidence collected!"
   - ✅ Dialogue shows: "I've collected all the evidence..."
   - ✅ `hasCollectedAllEvidence = true`

6. **Subukan ang Medicine Cabinet**
   - ✅ Bubuksan na ang cabinet (hindi na blocked)
   - ✅ Makukuha ang hammer

---

## 🐛 TROUBLESHOOTING

### Issue: "Walang notification"

**Solution**: 
- Check kung may ItemNotificationUI sa scene
- Script will fallback sa dialogue automatically

### Issue: "Hindi nawawala ang evidence"

**Solution**:
- Check kung may SpriteRenderer ang GameObject
- Check kung may Collider2D ang GameObject

### Issue: "Hindi nag-t-trigger ang pickup"

**Solution**:
- I-set ang Collider2D → Is Trigger ✓
- Check kung may "Player" tag ang Player GameObject
- I-enable ang Auto Pickup sa Inspector

### Issue: "Hindi nag-u-update ang counter"

**Solution**:
- Check kung may Room08_FlowController sa scene
- Check ang Console para sa debug logs
- I-enable ang Debug Mode sa script

---

## 💡 TIPS

### Para sa Better Experience:

1. **Visual Feedback**:
   - Gumamit ng particle effects para sa pickup
   - I-highlight ang evidence items (outline, glow)

2. **Audio Feedback**:
   - Iba-ibang sounds para sa iba-ibang evidence types
   - Satisfying pickup sound

3. **Placement**:
   - I-spread ang evidence around room
   - Gawing visible pero hindi sobrang obvious

4. **Descriptions**:
   - Short lang (1-2 sentences)
   - Atmospheric at story-relevant

---

**Setup complete! Test mo na ang evidence pickup!** 🎮✨

