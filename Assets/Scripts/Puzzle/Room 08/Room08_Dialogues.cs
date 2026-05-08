using UnityEngine;

/// <summary>
/// All dialogues for Room 08 - Lisa's Bathroom
/// Short dialogues (1-2 sentences) that fit in dialogue box
/// </summary>
public static class Room08_Dialogues
{
    // ==================== ENTRY SEQUENCE ====================
    public static readonly string ENTRY_1 = "The bathroom. My only sanctuary.";
    public static readonly string ENTRY_2 = "The only room with a lock on the inside. I would hide here for hours. Days, even.";
    
    public static readonly string DOOR_LOCKED = "The door is locked. Emily locked me in.";
    public static readonly string EMILY_OUTSIDE = "I can hear her outside. Humming. Waiting.";

    // ==================== BATHTUB ====================
    public static readonly string BATHTUB_1 = "The bathtub. I would fill it with cold water.";
    public static readonly string BATHTUB_2 = "Sit here until I couldn't feel anything anymore. Until I could pretend to be someone else. Someone safe.";
    
    public static readonly string BATHTUB_EVIDENCE_1 = "Injury bandages. Torn clothes. Hidden under the sink.";
    public static readonly string BATHTUB_EVIDENCE_2 = "Evidence of what she did to me. What I survived.";

    // ==================== MEDICINE CABINET ====================
    public static readonly string MEDICINE_1 = "The medicine cabinet. Pills everywhere.";
    public static readonly string MEDICINE_2 = "Mother's pills. Father's pills. So many pills.";
    
    public static readonly string HAMMER_FOUND_1 = "A hammer. Hidden behind the pills.";
    public static readonly string HAMMER_FOUND_2 = "Why would mother hide a hammer here? Unless... she knew I'd need it.";

    // ==================== EVIDENCE DIALOGUES ====================
    public static readonly string BANDAGES_1 = "Bloodstained bandages. Hidden under the sink.";
    public static readonly string BANDAGES_2 = "Evidence of what she did to me. What I survived.";
    
    public static readonly string TORN_CLOTHES_1 = "My torn clothes. From that night.";
    public static readonly string TORN_CLOTHES_2 = "I remember the pain. The fear. Emily took it all away.";
    
    public static readonly string APOLOGY_NOTE_1 = "A handwritten note: 'I'm sorry. I'm so sorry.'";
    public static readonly string APOLOGY_NOTE_2 = "Mother's handwriting. Shaky. Desperate. But it doesn't change what happened.";
    
    // ==================== EMILY APPEARS IN MIRROR ====================
    public static readonly string ALL_EVIDENCE_FOUND = "I've found everything. All the evidence of what happened here.";
    
    // NEW: Emily enters the room (not mirror)
    public static readonly string EMILY_ENTERS = "The door! She broke through! Emily is inside!";
    public static readonly string EMILY_HUNTING = "I need to find a way out NOW! She's coming for me!";
    
    public static readonly string EMILY_APPEARS_1 = "Wait... there's someone in the mirror.";
    public static readonly string EMILY_APPEARS_2 = "Emily? But she's... inside the mirror. Inside me.";
    public static readonly string EMILY_APPEARS_3 = "She's not behind me. She's IN the reflection. She's part of me.";
    public static readonly string EMILY_APPEARS_4 = "I need to break this mirror. I need to face the truth.";

    // ==================== MIRROR ====================
    public static readonly string MIRROR_EXAMINE_1 = "The mirror... I see her reflection behind me.";
    public static readonly string MIRROR_EXAMINE_2 = "Wait. She's not behind me. She's IN the mirror. IN me.";
    
    public static readonly string MIRROR_TRUTH_1 = "She's IN me. She's always been in me.";
    public static readonly string MIRROR_TRUTH_2 = "The part of me that refused to give up. The part that survived.";
    
    public static readonly string MIRROR_TWO_WAY_1 = "This mirror... it's a two-way mirror.";
    public static readonly string MIRROR_TWO_WAY_2 = "She was watching from the other side... the whole time.";
    
    public static readonly string MIRROR_BREAK_PROMPT = "I need to break this mirror. I need to escape.";

    // ==================== QTE ====================
    public static readonly string QTE_START = "The mirror is cracking. Keep going!";
    public static readonly string QTE_FAILED = "No! I'm not fast enough!";
    public static readonly string QTE_SUCCESS = "The mirror shatters! There's a passage behind it!";

    // ==================== EMILY CONFRONTATION ====================
    public static readonly string EMILY_VOICE_1 = "*Emily's voice echoes in my head*";
    public static readonly string EMILY_VOICE_2 = "'You know the truth now, don't you, Lisa?'";
    public static readonly string EMILY_VOICE_3 = "'I'm not real. I never was. I'm you.'";
    public static readonly string EMILY_VOICE_4 = "'The part of you that refused to give up.'";
    
    public static readonly string LISA_RESPONSE_1 = "You saved me. All those years, you saved me.";
    public static readonly string LISA_RESPONSE_2 = "When mother hurt me, you took the pain. You were the friend I needed when I had no one.";
    
    public static readonly string EMILY_FAREWELL_1 = "'I was always you, Lisa. Your strength. Your courage.'";
    public static readonly string EMILY_FAREWELL_2 = "'You don't need me anymore. You're strong enough now.'";
    public static readonly string EMILY_FAREWELL_3 = "'Strong enough to face the truth. Strong enough to heal.'";

    // ==================== ESCAPE ====================
    public static readonly string PASSAGE_FOUND_1 = "A passage. Behind the mirror.";
    public static readonly string PASSAGE_FOUND_2 = "A narrow crawlspace leading to... Mother's bedroom.";
    
    public static readonly string FINAL_DOOR_1 = "There's one more door. One more room.";
    public static readonly string FINAL_DOOR_2 = "Mother's bedroom. Where it all ended. Where the final truth has been waiting all along.";
    
    public static readonly string CLIMB_THROUGH = "I can climb through. To the Master Bathroom.";

    // ==================== PREREQUISITES ====================
    public static readonly string NEED_EVIDENCE = "I need to examine the evidence first. Understand what happened here.";
    public static readonly string NEED_HAMMER = "I need something to break the mirror. The hammer from the medicine cabinet.";
    public static readonly string NEED_ALL_EVIDENCE = "I should examine all the evidence first. There's more to find.";
}
