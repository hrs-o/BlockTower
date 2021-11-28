using BlockTower.Models.Main;
using BlockTower.Models.Main.Interfaces;
using BlockTower.Presenters.Main;
using BlockTower.Presenters.Main.Enums;
using BlockTower.Views.Main;
using MessagePipe;
using VContainer;
using VContainer.Unity;

namespace BlockTower.LifetimeScopes
{
    public class MainLifetimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            Register(builder);
            RegisterComponent(builder);
            RegisterEntryPoint(builder);
            RegisterMessagePipe(builder);
        }

        private void Register(IContainerBuilder builder)
        {
            builder.Register<ICountdownText, CountdownTextModel>(Lifetime.Scoped);
        }

        private void RegisterComponent(IContainerBuilder builder)
        {
            builder.RegisterComponentInHierarchy<CountdownTextView>();
            builder.RegisterComponentInHierarchy<StageView>();
        }

        private void RegisterEntryPoint(IContainerBuilder builder)
        {
            builder.UseEntryPoints(Lifetime.Singleton, entryPoints =>
            {
                entryPoints.Add<CountdownTextPresenter>().AsSelf();
                entryPoints.Add<StagePresenter>().AsSelf();
            });
        }

        private void RegisterMessagePipe(IContainerBuilder builder)
        {
            var options = builder.RegisterMessagePipe();

            builder.RegisterMessageBroker<GameState>(options);
        }
    }
}
