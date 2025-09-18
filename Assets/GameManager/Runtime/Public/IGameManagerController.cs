using Playtika.Controllers;

namespace Game.GameManager
{
    public interface IGameManagerController : IControllerWithResult<EmptyControllerResult>, IController<EmptyControllerArg> 
    { }
}