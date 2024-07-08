namespace PitangVac.Entity.Entities
{
    public abstract class EntityId<T> : IEntity
    {
        public T Id { get; set; }
    }
}
