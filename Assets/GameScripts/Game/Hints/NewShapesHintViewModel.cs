using GameScripts.ConsumeSystem.Module;
using GameScripts.ResourceStorage.Interfaces;
using GameScripts.ResourceStorage.ResourceType;

namespace GameScripts.Game
{
    public class NewShapesHintViewModel : HintViewModelBase<NewShapesHint>
    {
        public NewShapesHintViewModel(IResourceStorage resourceStorage, AbstractConsumableFactory consumableFactory, FieldViewModelContainer fieldViewModelContainer) : base(resourceStorage, consumableFactory, fieldViewModelContainer)
        {
        }

        protected override void Use()
        {
            fieldViewModelContainer.FieldViewModel.Value.UseNewShapesHint();
        }
    }
}