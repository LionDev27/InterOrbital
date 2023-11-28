using UnityEngine;
using PixelCrushers.DialogueSystem;

public class SetTypewriterForActor : MonoBehaviour
{
    public void OnBeginTypewriter() // Assign to typewriter's OnBegin() event.
    {
        // Look up character's audio clip:
        var actorName = DialogueManager.currentConversationState.subtitle.speakerInfo.nameInDatabase;
        var clipName = DialogueManager.masterDatabase.GetActor(actorName).LookupValue("AudioClip");
        var clip = Resources.Load<AudioClip>("Audio/Dialogue/" + clipName);

        // Assign to typewriter:
        var typewriter = GetComponent<AbstractTypewriterEffect>();
        typewriter.audioClip = clip;
        typewriter.audioSource.clip = clip;
    }
}
