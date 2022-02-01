using GameScripts.ConsumeSystem.Module;
using GameScripts.ResourceStorage.Interfaces;
using GameScripts.ResourceStorage.ResourceType;

namespace GameScripts.Game
{
    public class RotateHintViewModel : HintViewModelBase<RotateHint>
    {
        public RotateHintViewModel(IResourceStorage resourceStorage, AbstractConsumableFactory consumableFactory, FieldViewModelContainer fieldViewModelContainer) : base(resourceStorage, consumableFactory, fieldViewModelContainer)
        {
        }

        protected override void Use()
        {
            fieldViewModelContainer.FieldViewModel.Value.UseRotateHint();
        }
    }
}