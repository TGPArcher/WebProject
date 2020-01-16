namespace Domain
{
    public interface IPersistanceContext
    {
        public IActionItemRepository GetActionItemRepository();

        public IUserRepository GetUserRepository();
    }
}
