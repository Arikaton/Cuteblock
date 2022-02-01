using GameScripts.ConsumeSystem.Module;
using GameScripts.ResourceStorage.Interfaces;
using GameScripts.ResourceStorage.ResourceType;

namespace GameScripts.Game
{
    public class DeleteHintViewModel : HintViewModelBase<DeleteHint>
    {
        public DeleteHintViewModel(IResourceStorage resourceStorage, AbstractConsumableFactory consumableFactory, FieldViewModelContainer fieldViewModelContainer) : base(resourceStorage, consumableFactory, fieldViewModelContainer)
        {
        }

        protected override void Use()
        {
            fieldViewModelContainer.FieldViewModel.Value.UseDeleteHint();
        }

        protected override bool CanUse()
        {
            return fieldViewModelContainer.FieldViewModel.Value.ShapesOnField.Count > 0;
        }
    }
}