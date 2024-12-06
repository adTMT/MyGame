using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

namespace pong.Animations
{
    public enum ActionType
    {
        Idle,
        Walk,
        Attack
        // Add more actions as needed
    }
    public class Animatie
    {
        public AnimationFrames CurrentFrame { get; private set; }
        private Dictionary<ActionType, List<AnimationFrames>> actionFrames;
        private Dictionary<ActionType, float> frameDurations;
        private float currentFrameTimer; private List<AnimationFrames> frames;
        public ActionType currentAction;
        private int counter;
        public Animatie()
        {
            actionFrames = new Dictionary<ActionType, List<AnimationFrames>>();
            frameDurations = new Dictionary<ActionType, float>();
            currentAction = ActionType.Idle;
            counter = 0;
        }
        public bool IsLastFrame()
        {
            if (!actionFrames.ContainsKey(currentAction) || actionFrames[currentAction].Count == 0)
                return false;

            return counter == actionFrames[currentAction].Count - 1;
        }
        public void AddFrame(ActionType action, AnimationFrames frame)
        {
            //frames.Add(animationFrames);
            //CurrentFrame = frames[0];
            if (!actionFrames.ContainsKey(action))
            {
                actionFrames[action] = new List<AnimationFrames>();
            }
            actionFrames[action].Add(frame);

            // Set the first frame of the action as the CurrentFrame if it's the first frame added
            if (actionFrames[action].Count == 1 && currentAction == action)
            {
                CurrentFrame = frame;
            }
        }
        public void Update()
        {
            if (actionFrames.ContainsKey(currentAction) && actionFrames[currentAction].Count > 0)
            {
                counter++;
                if (counter >= actionFrames[currentAction].Count)
                {
                    counter = 0;
                }
                CurrentFrame = actionFrames[currentAction][counter];
            }
        }
        public void SetAction(ActionType action)
        {
            if (currentAction != action)
            {
                currentAction = action;
                counter = 0; // Reset the frame counter when action changes
                CurrentFrame = actionFrames[currentAction][counter];
            }
        }
        public void SetFrameDuration(ActionType action, float duration)
        {
            frameDurations[action] = duration;
        }
    }
}
