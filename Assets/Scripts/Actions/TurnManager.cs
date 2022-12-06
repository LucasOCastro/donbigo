namespace DonBigo
{
    public class TurnManager
    {
        private Entity[] _entities;
        private int _entityIndex;
        public Entity CurrentEntity => _entities[_entityIndex];

        public TurnManager(Entity[] entities)
        {
            if (entities == null || entities.Length == 0)
            {
                UnityEngine.Debug.LogError("Nenhuma entidade entregue ao TurnManager!");
                return;
            }
            _entities = entities;
        }

        private void CycleEntity() => _entityIndex = (_entityIndex + 1) % _entities.Length;

        public void Update()
        {
            //TODO as entidades devem ter um método que espera a seleção de uma action a ser realizada.
            Action action = new MoveAction(CurrentEntity, CurrentEntity.Tile); //CurrentEntity.GetAction();
            if (action != null)
            {
                action.Execute();
                CycleEntity();
            }
        }
    }
}