using System;

namespace Kdevaulo.SoundNinja.SoundMenuBehaviour.SoundsContainerSystem
{
    public sealed class SubscriptionContainer
    {
        public Action PlayClickedSubscription;

        public Action RemoveClickedSubscription;

        public Action StartGameClickedSubscription;
    }
}