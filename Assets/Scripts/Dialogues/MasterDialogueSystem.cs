using UnityEngine;

/// <summary>
/// Master Dialogue System - Complete Story from Start to Finish
/// A psychological horror story about trauma, imaginary friends, and the truth we hide from ourselves
/// </summary>
public static class MasterDialogueSystem
{
    // ==================== GAME START / MAIN MENU ====================
    
    public static readonly string MAIN_MENU_TAGLINE = 
        "Some memories are better left forgotten...";
    
    // ==================== SPLASH SCREEN / INTRO ====================
    
    public static readonly string GAME_INTRO_1 = 
        "They say the mind protects us from what we cannot bear to remember.";
    
    public static readonly string GAME_INTRO_2 = 
        "It builds walls. Creates stories. Invents companions.";
    
    public static readonly string GAME_INTRO_3 = 
        "But walls crumble. Stories unravel.";
    
    public static readonly string GAME_INTRO_4 = 
        "And companions... they never truly leave.";
    
    // ==================== ROOM 01: FOYER - THE AWAKENING ====================
    
    public static readonly string R01_ENTRY = 
        "Where am I? This house... I know this house.";
    
    public static readonly string R01_ENTRY_2 = 
        "But everything feels wrong. Like a memory seen through broken glass.";
    
    public static readonly string R01_DOOR_LOCKED = 
        "The front door won't open. Of course it won't.";
    
    public static readonly string R01_DOOR_LOCKED_2 = 
        "I'm not meant to leave. Not yet. Not until I remember.";
    
    public static readonly string R01_MIRROR_FIRST = 
        "My reflection... is that really me?";
    
    public static readonly string R01_MIRROR_FIRST_2 = 
        "I look so tired. So afraid. How long have I been here?";
    
    public static readonly string R01_FAMILY_PHOTO = 
        "A family photo. Mother, father, and a little girl.";
    
    public static readonly string R01_FAMILY_PHOTO_2 = 
        "She's smiling, but her eyes look so empty. So alone.";
    
    public static readonly string R01_FAMILY_PHOTO_3 = 
        "That's me. I was that little girl.";
    
    public static readonly string R01_MOTHERS_VOICE = 
        "*A woman's voice echoes from somewhere deep in the house*";
    
    public static readonly string R01_MOTHERS_VOICE_2 = 
        "'Lisa... Lisa, where are you? Come here this instant!'";
    
    public static readonly string R01_MOTHERS_VOICE_3 = 
        "Mother. Even now, her voice makes my hands shake.";
    
    public static readonly string R01_FIRST_CLUE = 
        "A child's drawing on the floor. Two stick figures holding hands.";
    
    public static readonly string R01_FIRST_CLUE_2 = 
        "One labeled 'Me' and one labeled 'Emily.'";
    
    public static readonly string R01_FIRST_CLUE_3 = 
        "Emily. That name... why does it hurt to think about?";
    
    public static readonly string R01_PROCEED = 
        "I need to go deeper into the house. The answers are here.";
    
    public static readonly string R01_PROCEED_2 = 
        "I can feel them waiting in every shadow, every corner.";
    
    public static readonly string R01_PROCEED_3 = 
        "I'm afraid... but I have to know the truth.";
    
    // ==================== ROOM 02: LIVING ROOM - FIRST MEMORIES ====================
    
    public static readonly string R02_ENTRY = 
        "The living room. I spent so many hours here as a child.";
    
    public static readonly string R02_ENTRY_2 = 
        "Playing. Hiding. Pretending everything was normal.";
    
    public static readonly string R02_BROKEN_VASE = 
        "A broken vase... I remember this.";
    
    public static readonly string R02_BROKEN_VASE_2 = 
        "'You clumsy, worthless child! Can't you do anything right?'";
    
    public static readonly string R02_BROKEN_VASE_3 = 
        "But I didn't break it. Emily did. Emily was trying to protect me.";
    
    public static readonly string R02_COUCH = 
        "The couch where I would sit for hours, perfectly still, perfectly quiet.";
    
    public static readonly string R02_COUCH_2 = 
        "If I was quiet enough, maybe she wouldn't notice me.";
    
    public static readonly string R02_TV = 
        "The television. Father would watch it for hours, pretending not to hear.";
    
    public static readonly string R02_TV_2 = 
        "Pretending not to see. It was easier for him that way.";
    
    public static readonly string R02_EMILY_APPEARS = 
        "*A child's laughter echoes through the room*";
    
    public static readonly string R02_EMILY_APPEARS_2 = 
        "Emily? Is that you?";
    
    public static readonly string R02_EMILY_APPEARS_3 = 
        "I can almost see her... standing in the corner, watching me.";
    
    public static readonly string R02_MOTHERS_ROOM_LOCKED = 
        "Mother's room. The door is locked. It was always locked.";
    
    public static readonly string R02_MOTHERS_ROOM_LOCKED_2 = 
        "Her sanctuary. Where she would go to 'calm down' after...";
    
    public static readonly string R02_PUZZLE_HINT = 
        "There's something hidden here. Something I need to find.";
    
    public static readonly string R02_PUZZLE_HINT_2 = 
        "Emily would know where it is. She always knew where to hide things.";
    
    // ==================== ROOM 03: HALLWAY - THE CORRIDOR OF FEAR ====================
    
    public static readonly string R03_ENTRY = 
        "The hallway. Long and dark, like a throat waiting to swallow me. " +
        "I used to run through here as fast as I could. " +
        "Running from her voice. Running from her footsteps.";
    
    public static readonly string R03_FAMILY_PORTRAITS = 
        "Family portraits line the walls. Perfect smiles. Perfect poses. " +
        "The perfect family. What a beautiful lie.";
    
    public static readonly string R03_MOTHERS_PORTRAIT = 
        "Mother's portrait. She looks so elegant. So composed. " +
        "No one would ever guess what happened behind closed doors. " +
        "No one would ever believe me.";
    
    public static readonly string R03_SCRATCH_MARKS = 
        "Scratch marks on the wallpaper... small, desperate scratches. " +
        "Did I make these? Trying to claw my way out of this nightmare?";
    
    public static readonly string R03_EMILY_WHISPER = 
        "*A whisper in the darkness* 'Don't be afraid, Lisa. I'm here. I'll always be here.' " +
        "Emily... where are you? I need you.";
    
    public static readonly string R03_UPSTAIRS_LOCKED = 
        "The stairs to the second floor. I can't go up yet. " +
        "There's something I need to do first. Something I need to remember.";
    
    // ==================== ROOM 04: KITCHEN - WHERE IT HAPPENED ====================
    
    public static readonly string R04_ENTRY = 
        "The kitchen. My stomach tightens. Something terrible happened here. " +
        "I can feel it in the air, thick and suffocating.";
    
    public static readonly string R04_BROKEN_DISHES = 
        "Broken dishes scattered across the floor. " +
        "'Clean this up! Clean it up right now, you little brat!' " +
        "I was seven years old. My hands were bleeding from the broken glass.";
    
    public static readonly string R04_KNIFE_BLOCK = 
        "The knife block. One knife is missing. " +
        "I remember staring at it, wondering if... " +
        "No. Emily wouldn't let me. She said there was another way.";
    
    public static readonly string R04_MOTHERS_NOTE = 
        "A note in mother's handwriting: 'Dinner at 6 PM sharp. No excuses. No mistakes.' " +
        "Every meal was a test. Every bite was scrutinized. " +
        "I learned to eat in silence, to chew without making a sound.";
    
    public static readonly string R04_HIDDEN_FOOD = 
        "A hiding spot behind the cabinet. Old, moldy food. " +
        "I would hide food here when I was sent to bed without dinner. " +
        "Emily showed me this spot. She always took care of me.";
    
    public static readonly string R04_BLOOD_STAIN = 
        "A faint stain on the floor. Blood? " +
        "The memories are coming back now, sharp and painful. " +
        "That night. The night everything changed.";
    
    public static readonly string R04_REALIZATION = 
        "I'm starting to understand now. Emily wasn't just my friend. " +
        "She was my shield. My protector. My escape from reality.";
    
    // ==================== ROOM 05: DINING ROOM - THE PERFORMANCE ====================
    
    public static readonly string R05_ENTRY = 
        "The dining room. Where we performed the ritual of normalcy every evening. " +
        "The perfect family dinner. The perfect lie.";
    
    public static readonly string R05_TABLE_SET = 
        "The table is set for three. Mother. Father. Me. " +
        "But there's a fourth chair... Emily's chair. " +
        "I would set a place for her every night. Mother hated that.";
    
    public static readonly string R05_MOTHERS_CHAIR = 
        "Mother's chair at the head of the table. Her throne. " +
        "From here, she would watch every move, every bite, every breath. " +
        "Waiting for me to make a mistake.";
    
    public static readonly string R05_FATHERS_CHAIR = 
        "Father's chair. Always empty by dessert. " +
        "He would excuse himself, retreat to his study, bury himself in work. " +
        "Anything to avoid seeing. Anything to avoid knowing.";
    
    public static readonly string R05_MY_CHAIR = 
        "My chair. So small. I would sit here, back straight, hands folded. " +
        "The perfect daughter. The obedient child. " +
        "While inside, I was screaming.";
    
    public static readonly string R05_BROKEN_PLATE = 
        "A broken plate. I dropped it once. Just once. " +
        "The punishment lasted for days. " +
        "Emily held my hand through all of it. She never left my side.";
    
    public static readonly string R05_MEMORY_FLASH = 
        "*A memory flashes* Mother's hand raised. The sound of impact. " +
        "My cheek burning. Father's newspaper rustling. " +
        "And Emily, whispering: 'It's okay. I'm here. She can't hurt you when I'm here.'";
    
    // ==================== ROOM 06: RETURN TO HALLWAY - UPSTAIRS ====================
    
    public static readonly string R06_ENTRY = 
        "The hallway again. But something's different now. " +
        "The air feels heavier. The shadows deeper. " +
        "Emily's presence is stronger here.";
    
    public static readonly string R06_STAIRS_UNLOCKED = 
        "The stairs are open now. I can go up. " +
        "Up to where the worst memories wait. " +
        "Up to where the truth lives.";
    
    public static readonly string R06_EMILY_MANIFESTATION = 
        "*A figure appears at the top of the stairs* " +
        "Emily! I can see her now, clearer than ever. " +
        "She's beckoning me. Calling me. " +
        "'Come, Lisa. It's time to remember everything.'";
    
    public static readonly string R06_FEAR_RISING = 
        "My heart is pounding. Every instinct screams at me to run. " +
        "But I can't. I've come too far. " +
        "I need to know what happened. I need to know the truth about Emily.";
    
    // ==================== ROOM 07: LISA'S BEDROOM - THE TRUTH ====================
    
    // (Using the improved dialogues we already created)
    // This is where the emotional climax happens
    
    // ==================== ROOM 08: BATHROOM - THE ESCAPE ====================
    
    public static readonly string R08_ENTRY = 
        "The bathroom. My sanctuary. The only room with a lock. " +
        "I would hide here for hours, sitting in the empty bathtub, " +
        "while Emily sang to me and told me stories.";
    
    public static readonly string R08_MIRROR_TRUTH = 
        "The mirror... I see her now. Really see her. " +
        "Emily isn't behind me. She's IN me. She's always been in me. " +
        "She's the part of me that refused to break. The part that survived.";
    
    public static readonly string R08_BATHTUB = 
        "The bathtub. I would fill it with cold water and sit here until I couldn't feel anything. " +
        "Until the pain went numb. Until I could pretend to be someone else. " +
        "Someone who wasn't afraid.";
    
    public static readonly string R08_MEDICINE_CABINET = 
        "The medicine cabinet. Pills. So many pills. " +
        "Mother's 'happy pills.' Father's sleeping pills. " +
        "I thought about it once. Emily stopped me. " +
        "'Not yet,' she said. 'You're stronger than this.'";
    
    public static readonly string R08_EMILY_CONFRONTATION = 
        "*Emily's voice, clear and strong* " +
        "'You know the truth now, don't you, Lisa? " +
        "I'm not real. I never was. I'm you. The you that refused to give up. " +
        "The you that fought back when you couldn't.'";
    
    public static readonly string R08_LISA_RESPONSE = 
        "You saved me. All those years, you saved me. " +
        "When mother hurt me, you took the pain. " +
        "When I wanted to disappear, you kept me alive. " +
        "You were the friend I needed when I had no one.";
    
    public static readonly string R08_EMILY_FAREWELL = 
        "'I was always you, Lisa. Your strength. Your courage. Your will to survive. " +
        "You don't need me anymore. You're strong enough now. " +
        "Strong enough to face the truth. Strong enough to heal.'";
    
    public static readonly string R08_FINAL_DOOR = 
        "There's one more door. One more room. " +
        "The room where it all ended. Where Emily was born. " +
        "Where the truth has been waiting all along.";
    
    // ==================== ROOM 09: MASTER BEDROOM - THE REVELATION ====================
    
    public static readonly string R09_ENTRY = 
        "Mother's bedroom. The forbidden room. " +
        "The door is open now. She can't stop me anymore.";
    
    public static readonly string R09_MOTHERS_BED = 
        "Her bed. Perfectly made. Always perfect on the outside. " +
        "But I know what happened here. The pills. The alcohol. The rage.";
    
    public static readonly string R09_DIARY_FOUND = 
        "Mother's diary. Hidden under the mattress. " +
        "Let me read it... let me understand...";
    
    public static readonly string R09_DIARY_ENTRY_1 = 
        "'I don't know what's wrong with me. I look at her and I feel... nothing. " +
        "She's my daughter, but she feels like a stranger. " +
        "Sometimes I hate her. God forgive me, sometimes I hate her.'";
    
    public static readonly string R09_DIARY_ENTRY_2 = 
        "'I hurt her again today. I didn't mean to. I never mean to. " +
        "But the anger just takes over and I can't stop. " +
        "She looked at me with those big, frightened eyes and I... I couldn't stop.'";
    
    public static readonly string R09_DIARY_ENTRY_3 = 
        "'She talks to someone named Emily now. An imaginary friend. " +
        "She says Emily protects her. Protects her from me. " +
        "What have I done to my child?'";
    
    public static readonly string R09_DIARY_FINAL = 
        "'I'm getting help. Real help this time. I can't do this anymore. " +
        "I can't hurt her anymore. Lisa deserves better. She deserves a real mother. " +
        "I'm so sorry, baby. I'm so, so sorry.'";
    
    public static readonly string R09_UNDERSTANDING = 
        "She was sick. Mother was sick. " +
        "It doesn't excuse what she did. It doesn't erase the pain. " +
        "But... she knew. She knew she was hurting me. And she tried to stop.";
    
    public static readonly string R09_PHOTOGRAPH = 
        "A photograph. Recent. Mother in a hospital gown, smiling weakly. " +
        "Me standing beside her, older now, holding her hand. " +
        "This... this is real. This is now. " +
        "I'm not a child anymore. I'm an adult. And I'm here, in this memory, trying to heal.";
    
    public static readonly string R09_FINAL_REALIZATION = 
        "This isn't real. None of this is real. " +
        "I'm not trapped in my childhood home. " +
        "I'm in my mind, walking through my memories, " +
        "trying to make peace with what happened. " +
        "Trying to forgive. Trying to heal.";
    
    // ==================== FINAL ROOM: THE TRUTH ====================
    
    public static readonly string FINAL_ENTRY = 
        "One more door. The last door. " +
        "Behind it... the truth. The whole truth.";
    
    public static readonly string FINAL_REVELATION_1 = 
        "I'm in a therapist's office. I've been here for months. " +
        "Working through the trauma. Facing the memories. " +
        "This journey through the house... it was all in my mind. " +
        "A way to process what happened. A way to understand.";
    
    public static readonly string FINAL_REVELATION_2 = 
        "Emily was my dissociative identity. A protector personality. " +
        "She emerged when I was seven, after the abuse became too much to bear. " +
        "She took the pain when I couldn't. She kept me alive when I wanted to die.";
    
    public static readonly string FINAL_REVELATION_3 = 
        "Mother got help. She's been in treatment for years. " +
        "We're rebuilding our relationship. Slowly. Carefully. " +
        "It's not perfect. It may never be perfect. But we're trying.";
    
    public static readonly string FINAL_REVELATION_4 = 
        "And Emily... I don't need her anymore. " +
        "I've integrated her strength into myself. " +
        "She's not gone. She's just... part of me now. " +
        "The part that survived. The part that's strong.";
    
    public static readonly string FINAL_CHOICE = 
        "I can leave now. I can walk out of this memory and back into my life. " +
        "Or I can stay here, trapped in the past, forever afraid. " +
        "What do I choose?";
    
    public static readonly string FINAL_LEAVE = 
        "I choose to leave. I choose to heal. I choose to live. " +
        "Thank you, Emily. Thank you for saving me. " +
        "But I can save myself now.";
    
    public static readonly string FINAL_GOODBYE = 
        "*Emily's voice, soft and fading* " +
        "'Goodbye, Lisa. I'm proud of you. You're going to be okay. " +
        "You're going to be more than okay. You're going to be free.'";
    
    public static readonly string FINAL_DOOR_OPENS = 
        "The door opens. Light floods in. Warm. Real. " +
        "I step through. " +
        "I step into my life. " +
        "I step into healing. " +
        "I step into freedom.";
    
    // ==================== EPILOGUE ====================
    
    public static readonly string EPILOGUE_1 = 
        "Six months later...";
    
    public static readonly string EPILOGUE_2 = 
        "I still see a therapist. I still have hard days. " +
        "But I'm healing. Really healing.";
    
    public static readonly string EPILOGUE_3 = 
        "Mother and I meet for coffee sometimes. " +
        "We talk. We cry. We try to understand each other.";
    
    public static readonly string EPILOGUE_4 = 
        "I don't hear Emily's voice anymore. " +
        "But sometimes, when I'm afraid, I feel her strength. " +
        "Because she was always me. And I was always strong.";
    
    public static readonly string EPILOGUE_5 = 
        "This is my story. This is my truth. " +
        "And I'm finally free to tell it.";
    
    public static readonly string THE_END = 
        "THE END\n\n" +
        "If you or someone you know is experiencing abuse, please reach out:\n" +
        "National Child Abuse Hotline: 1-800-422-4453\n" +
        "National Domestic Violence Hotline: 1-800-799-7233\n\n" +
        "You are not alone. You are believed. You deserve help.";
}
