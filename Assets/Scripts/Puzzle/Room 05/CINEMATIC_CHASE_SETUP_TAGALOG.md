# Cinematic Chase Trigger - Gabay sa Tagalog

## 📋 Ano Ito?

**CinematicChaseTrigger.cs** ay reference script para sa pag-gawa ng cinematic chase sequences kasama si Emily. May full control ka sa bilis, lakas, at game over conditions ni Emily.

---

## ✨ Features

### Mga Tampok:
- ✅ **Adjustable Emily Speed** - Gaano kabilis si Emily (1-10)
- ✅ **Adjustable Catch Distance** - Gaano kalapit dapat si Emily para Game Over (0.5-3)
- ✅ **Knockback Effect** - Itulak pabalik ang player
- ✅ **Game Over on Contact** - Automatic Game Over pag nahuli ni Emily
- ✅ **Dialogue Support** - May dialogue pag nagsimula chase
- ✅ **Audio Support** - Jumpscare sound + looping chase music
- ✅ **One-time Trigger** - Isang beses lang mag-trigger
- ✅ **Visual Debugging** - Makikita mo sa Scene view ang lahat

---

## 🎮 Paano Gumagana

### Daloy:
1. Pumasok ang player sa trigger zone
2. **Knockback** - Itulak pabalik ang player
3. **Sound effect** - Tumugtog ang jumpscare sound
4. **Dialogue** - Lumabas ang dialogue (optional)
5. **Emily spawns** - Lumitaw si Emily sa designated position
6. **Emily chases** - Humabol si Emily sa player
7. **Game Over** - Pag nahuli ni Emily ang player

---

## 🔧 Unity Setup

### Step 1: Gumawa ng Trigger Zone

1. Create empty GameObject: `CinematicChaseTrigger`
2. Add **BoxCollider2D** o **CircleCollider2D**
3. ✅ I-check ang **"Is Trigger"**
4. I-position kung saan mo gusto magsimula ang chase

### Step 2: Gumawa ng Emily Spawn Point

1. Create empty GameObject: `EmilySpawnPoint`
2. I-position kung saan dapat lumitaw si Emily
3. Dito mag-teleport si Emily pag nagsimula ang chase

### Step 3: I-add ang Script

1. I-select ang `CinematicChaseTrigger` GameObject
2. Add Component → **CinematicChaseTrigger**
3. I-configure lahat ng settings (tingnan sa baba)

---

## ⚙️ Inspector Settings

### Trigger Settings

**Trigger Once**
- ✅ Enabled: Isang beses lang mag-trigger
- ❌ Disabled: Pwedeng mag-trigger ng maraming beses
- **Recommended**: ✅ Enabled

---

### Emily Configuration

**Emily GameObject**
- I-drag ang Emily GameObject mula sa Hierarchy
- Dapat may: `NavMeshAgent` + `EmilyGhost` components
- **IMPORTANTE**: Galing sa Hierarchy, HINDI prefab!

**Emily Spawn Point**
- I-drag ang `EmilySpawnPoint` GameObject
- Dito lalabas si Emily

**Emily Chase Speed**
- Range: 1.0 - 10.0
- Default: 5.5
- **Mabagal**: 3.0-4.0 (mas madali)
- **Normal**: 5.0-6.0 (balanced)
- **Mabilis**: 7.0-10.0 (intense!)

**Catch Distance**
- Range: 0.5 - 3.0
- Default: 1.0
- Gaano kalapit dapat si Emily para Game Over
- **Mas maliit** = Mas mahirap (dapat sobrang lapit)
- **Mas malaki** = Mas madali (mahuhuli kahit malayo pa)

---

### Knockback Settings

**Enable Knockback**
- ✅ Enabled: May knockback pag nag-trigger
- ❌ Disabled: Walang knockback

**Knockback Force**
- Range: 0 - 20
- Default: 10
- Gaano kalakas ang tulak

**Knockback Direction**
- Vector2 (X, Y)
- Default: (-1, 0.5) = Pabalik at pataas
- Mga halimbawa:
  - `(-1, 0)` = Tulak pakaliwa
  - `(1, 0)` = Tulak pakanan
  - `(0, 1)` = Tulak pataas
  - `(-1, 0.5)` = Tulak pabalik-kaliwa at pataas

---

### Dialogue Settings

**Show Dialogue**
- ✅ Enabled: May dialogue pag nagsimula chase
- ❌ Disabled: Walang dialogue

**Chase Dialogue**
- Text na lalabas
- Halimbawa: "Paparating na siya!"
- I-leave empty kung walang dialogue

**Speaker Name**
- Sino nagsasalita
- Default: "Lisa"

---

### Audio Settings

**Play Sound Effect**
- ✅ Enabled: May tunog pag nagsimula chase
- ❌ Disabled: Walang tunog

**Chase Sound Effect**
- I-drag ang AudioClip (jumpscare/scream sound)
- Tutugtog ng isang beses pag nagsimula chase

**Audio Source**
- Optional: I-drag ang AudioSource component
- Kung empty, gagamitin ang AudioManager

**Chase Loop Music**
- I-drag ang AudioClip (footsteps/tension music)
- Loop habang nag-chase
- Titigil pag Game Over

---

### Timing Settings

**Chase Start Delay**
- Range: 0 - 2 seconds
- Default: 0.2
- Delay bago magsimula si Emily humabol
- **0.0** = Instant (walang delay)
- **0.2** = Mabilis (konting pause)
- **0.5** = Dramatic (build tension)

---

### Game Over Settings

**Enable Game Over**
- ✅ Enabled: Game Over pag nahuli ni Emily
- ❌ Disabled: Walang Game Over (para sa testing)

**Game Over Message**
- Text na lalabas sa Game Over screen
- Default: "Emily caught you..."
- Mga halimbawa:
  - "Nahuli ka ni Emily..."
  - "Hindi ka nakaligtas..."
  - "Nakuha ka niya..."

---

### Debug Settings

**Debug Mode**
- ✅ Enabled: May debug messages sa Console
- ❌ Disabled: Walang debug messages
- **Recommended**: ✅ Enabled habang nag-develop

---

## 🎯 Mga Halimbawa ng Configuration

### Configuration 1: Unang Chase (Mas Madali)
```
Emily Chase Speed: 3.5
Catch Distance: 1.0
Knockback Force: 8.0
Chase Start Delay: 0.5
Chase Dialogue: "Ano yung tunog na yun?"
```

### Configuration 2: Final Chase (Mas Mahirap)
```
Emily Chase Speed: 5.5
Catch Distance: 1.0
Knockback Force: 10.0
Chase Start Delay: 0.2
Chase Dialogue: "Paparating na siya!"
```

### Configuration 3: Intense Chase (Sobrang Hirap)
```
Emily Chase Speed: 7.0
Catch Distance: 1.5
Knockback Force: 12.0
Chase Start Delay: 0.0
Chase Dialogue: "TAKBO!"
```

---

## 🔍 Visual Debugging (Gizmos)

Pag naka-select ang trigger sa Scene view, makikita mo:

**Red Semi-Transparent Box**
- Trigger area (kung saan dapat pumasok ang player)

**Red Wire Sphere**
- Emily spawn point (kung saan lalabas si Emily)

**Red Line**
- Connection ng trigger at spawn point

**Red Wire Circle**
- Catch distance (Game Over radius around Emily)

**Yellow Arrow**
- Knockback direction (saan itutulak ang player)

---

## 🎬 Paano Gamitin sa Iba't Ibang Rooms

### Room 05 (Dining Room) - Halimbawa

**First Chase Trigger:**
```
Position: Sa pagitan ng calendar at exit
Emily Speed: 3.5
Catch Distance: 1.0
Dialogue: "Ano yung tunog na yun?"
```

**Final Chase Trigger:**
```
Position: Malapit sa exit door
Emily Speed: 5.5
Catch Distance: 1.0
Dialogue: "Paparating na siya!"
Knockback: Enabled
```

---

## 🐛 Troubleshooting

### Hindi lumalabas si Emily
- ✅ Check kung naka-assign ang Emily GameObject
- ✅ Check kung may NavMeshAgent component si Emily
- ✅ Check kung naka-assign ang Emily Spawn Point
- ✅ Check kung naka-check ang "Is Trigger" sa collider

### Hindi nag-trigger ang Game Over
- ✅ Check kung enabled ang "Enable Game Over"
- ✅ Check kung may EmilyGhost component si Emily
- ✅ Check kung may GameOverManager sa scene
- ✅ Check kung hindi masyadong maliit ang Catch Distance

### Hindi gumagana ang Knockback
- ✅ Check kung enabled ang "Enable Knockback"
- ✅ Check kung may Rigidbody2D component ang player
- ✅ Check kung hindi 0 ang Knockback Force

### Mabagal/Mabilis masyado si Emily
- ✅ I-adjust ang "Emily Chase Speed" slider
- ✅ Check kung hindi naka-override ang NavMeshAgent speed
- ✅ Subukan ang iba't ibang values (3.5 = mabagal, 5.5 = normal, 7.0 = mabilis)

### Hindi lumalabas ang Dialogue
- ✅ Check kung enabled ang "Show Dialogue"
- ✅ Check kung may laman ang "Chase Dialogue"
- ✅ Check kung may DialogueSystemV2 sa scene

### Hindi tumutugtog ang Sound
- ✅ Check kung enabled ang "Play Sound Effect"
- ✅ Check kung naka-assign ang AudioClip
- ✅ Check kung may AudioManager sa scene
- ✅ Check kung hindi 0 ang audio volume

---

## 🎯 Best Practices

### Speed Balancing:
- **Unang chase**: 3.0-4.0 (bigyan ng time ang player na matuto)
- **Mid-game chase**: 4.5-5.5 (balanced challenge)
- **Final chase**: 6.0-7.5 (intense climax)

### Catch Distance:
- **1.0** = Standard (dapat sobrang lapit)
- **1.5** = Forgiving (mas madaling mahuli)
- **0.8** = Strict (mas mahirap mahuli)

### Knockback:
- Gamitin ang knockback para lumayo ang player
- Direction dapat palayo kay Emily
- Force 8-12 ay usually okay

### Dialogue:
- Panatilihing MAIKLI (1-2 sentences)
- Gumawa ng urgency ("Paparating na siya!", "Takbo!")
- Match ang intensity sa difficulty ng chase

### Audio:
- Jumpscare sound: Maikli, malakas, biglaan
- Chase music: Looping, tense, rhythmic
- I-stop ang music sa Game Over para sa impact

---

## 🎮 Testing Checklist

- [ ] Nag-activate ang trigger pag pumasok ang player
- [ ] Nag-knockback ang player sa tamang direction
- [ ] Tumugtog ang sound effect
- [ ] Lumabas ang dialogue
- [ ] Lumabas si Emily sa tamang position
- [ ] Humahabol si Emily sa player
- [ ] Tama ang bilis ni Emily
- [ ] Nag-trigger ang Game Over pag nahuli
- [ ] Tama ang Game Over message
- [ ] Nag-loop ang chase music
- [ ] Isang beses lang nag-activate ang trigger

---

## 💡 Tips

### Para sa Mas Madaling Chase:
- Babaan ang Emily speed (3.0-4.0)
- Paliitin ang catch distance (0.8-1.0)
- Pahabain ang start delay (0.5-1.0)
- Palakasin ang knockback (12-15)

### Para sa Mas Mahirap na Chase:
- Itaas ang Emily speed (6.0-8.0)
- Palakihin ang catch distance (1.5-2.0)
- Paikliin ang start delay (0.0-0.2)
- Pahinain ang knockback (5-8)

### Para sa Cinematic Effect:
- Gumamit ng dialogue para sa tension
- Magdagdag ng camera shake (separate script)
- Gumamit ng dramatic sound effects
- I-time ang knockback sa music

---

## ✅ Summary

**CinematicChaseTrigger** ay flexible at configurable script para sa pag-gawa ng chase sequences kasama si Emily. Kaya nitong:
- ✅ I-spawn at i-control si Emily
- ✅ Mag-knockback ng player
- ✅ Mag-show ng dialogue at audio
- ✅ Mag-trigger ng Game Over pag nahuli
- ✅ Mag-show ng visual debugging

**Perfect para sa**: Pag-gawa ng intense, cinematic chase moments sa buong game!

**Customize**: I-adjust ang speed, distance, at timing para sa difficulty curve ng game mo!

---

## 🎯 Quick Start

1. Gumawa ng trigger zone (BoxCollider2D, Is Trigger = true)
2. Gumawa ng spawn point (empty GameObject)
3. I-add ang CinematicChaseTrigger script
4. I-assign ang Emily GameObject
5. I-assign ang spawn point
6. I-set ang Emily speed (5.5 recommended)
7. I-set ang catch distance (1.0 recommended)
8. I-enable ang knockback
9. Magdagdag ng dialogue at sound
10. Test!

**Ready ka na gumawa ng epic chase sequences!** 🎮✨

---

## 📝 Mga Importante

### DAPAT TANDAAN:
1. **Emily GameObject** - Galing sa Hierarchy, HINDI prefab
2. **Catch Distance** - Ito ang dahilan kung bakit hindi nag-Game Over dati
3. **Enable Game Over** - Dapat naka-check ito
4. **Emily Speed** - I-adjust para sa difficulty
5. **Knockback** - Para lumayo ang player bago humabol si Emily

### COMMON ISSUES:
- **Walang Game Over** = Check catch distance at enable game over
- **Mabilis/Mabagal si Emily** = Adjust Emily chase speed
- **Walang knockback** = Check enable knockback at force

**Tapos na! Pwede mo na i-customize si Emily para sa iba't ibang chase sequences!** 💪✨
